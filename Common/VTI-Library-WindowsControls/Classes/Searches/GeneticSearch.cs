using System;
using System.Collections.Generic;
using System.Linq;

namespace VTIWindowsControlLibrary.Classes.Searches
{
    /// <summary>
    /// Performs a <see href="http://en.wikipedia.org/wiki/Genetic_algorithm">Genetic Search</see>
    /// to locate an optimal solution to a problem.
    /// </summary>
    /// <remarks>
    /// The potential solutions to a problem are represented by a population of <see cref="Phenotypes">phenotypes</see>.
    /// The <see cref="Genotype">Genotype</see> is represented as a 64-bit integer which can also be referenced
    /// as a <see cref="Genotype.Genes">list of genes</see>.  These genes are indexes into the problem space.
    /// The genetic algorithm itself neither knows nor cares the source of these genes.  The genetic search
    /// progresses by breeding new generations of potential solutions with the goal of maximizing a "fitness function".
    /// The fitness function is a function which provides an ever-increasing value as the potential solutions
    /// approach an optimal solution.  It is up to the consumer of the genetic search to examine the population
    /// of phenotypes at the end of each generation to determine if an appropriate solution has been located, or
    /// if the search should continue.
    /// </remarks>
    public class GeneticSearch
    {
        #region Fields (10) 

        #region Public Fields (2) 

        /// <summary>
        /// The numerical maximum value of each gene in the genotype
        /// </summary>
        public int MaxGene;

        /// <summary>
        /// The probability that a mutation will occur in any given breed function.
        /// </summary>
        /// <value>0==0%, 1==100%</value>
        /// <remarks>
        /// Mutations help prevent the algorithm from settling early into a
        /// non-optimal solution.
        /// </remarks>
        public double MutationProbability = 0.001;

        #endregion Public Fields 
        #region Private Fields (8) 

        private Func<Genotype, double> _FitnessFunction;
        private int _GeneLen = 4;
        private int _NumGenes = 5;
        private List<Genotype> _phenotypes;
        private float _UnfitBreedPercent = 10;
        private UInt64 GeneMask;
        private Random rnd = new Random();
        private float unfitBreedFactor = 0.1F;
        private int _populationSize;

        #endregion Private Fields 

        #endregion Fields 

        #region Constructors (1) 

        /// <summary>
        /// Initializes a new instance of a <see cref="GeneticSearch">GeneticSearch</see>.
        /// </summary>
        /// <param name="numGenes">Number of genes in the genotype</param>
        /// <param name="geneLen">Length (in bits) of each gene in the genotype</param>
        /// <param name="fitnessFunction">Fuction which should be ever-increasing as a potential solution approaches an optimal solution</param>
        public GeneticSearch(int numGenes, int geneLen, Func<Genotype, double> fitnessFunction)
        {
            NumGenes = numGenes;
            GeneLen = geneLen;
            _FitnessFunction = fitnessFunction;
            _phenotypes = new List<Genotype>();
        }

        #endregion Constructors 

        #region Properties (5) 

        private int ChrosomeLength
        {
            get { return _NumGenes * _GeneLen; }
        }

        /// <summary>
        /// Length (in bits) of each gene in the genotype.  GeneLen x NumGenes must not exceed 64 bits.
        /// </summary>
        public int GeneLen
        {
            get { return _GeneLen; }
            set
            {
                if (_NumGenes * _GeneLen > 64) throw new Exception("NumGenes * GenLen must be 64 or less.");
                _GeneLen = value;
                MaxGene = 1 << GeneLen;
                GeneMask = (((UInt64)(1 << GeneLen)) - 1);
            }
        }

        /// <summary>
        /// Number of genes in the genotype.  GeneLen x NumGenes must not exceed 64 bits.
        /// </summary>
        public int NumGenes
        {
            get { return _NumGenes; }
            set
            {
                if (_NumGenes * _GeneLen > 64) throw new Exception("NumGenes * GenLen must be 64 or less.");
                _NumGenes = value;
            }
        }

        /// <summary>
        /// List of phenotypes (individuals) in the genetic search population.
        /// </summary>
        public List<Genotype> Phenotypes
        {
            get { return _phenotypes; }
        }

        /// <summary>
        /// Percent of less-than-average phenotypes to include in each breed iteration.
        /// </summary>
        /// <remarks>
        /// Including a small percentage of less-than-average phenotypes helps to
        /// keep the genetic algorithm from settling early into a non-optimal solution.
        /// </remarks>
        public float UnfitBreedPercent
        {
            get { return _UnfitBreedPercent; }
            set
            {
                if (value < 0 || value > 100) throw new ArgumentOutOfRangeException("UnfitBreedPercent must be between 0 and 100.");
                _UnfitBreedPercent = value;
                unfitBreedFactor = _UnfitBreedPercent / 100F;
            }
        }

        #endregion Properties 

        #region Delegates and Events (2) 

        #region Events (2) 

        /// <summary>
        /// Occurs during the breed process between creating each new phenotype
        /// </summary>
        public event EventHandler<IterationEventArgs> BreedIterationEvent;

        /// <summary>
        /// Occurs when initializing the population between creating each new phenotype
        /// </summary>
        public event EventHandler<IterationEventArgs> PopulationInitEvent;

        #endregion Events 

        #endregion Delegates and Events 

        #region Methods (4) 

        #region Public Methods (2) 

        /// <summary>
        /// "Breeds" the potential solutions into a new generation of potential solutions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All of the average-or-better phenotypes and a
        /// <see cref="UnfitBreedPercent">small percentage</see> of less-than-average
        /// phenotypes are included in the breed function.
        /// </para>
        /// <para>
        /// After the new generation has been created, they are sorted according to
        /// fitness, and the population is truncated to the original population size.
        /// </para>
        /// </remarks>
        public void Breed()
        {
            double avgFitness = _phenotypes.Average(p => p.Fitness);

            List<Genotype> fitPhenotypes = new List<Genotype>();
            fitPhenotypes.AddRange(_phenotypes.Where(p => p.Fitness >= avgFitness));

            List<Genotype> unfitPhenotypes = new List<Genotype>();
            unfitPhenotypes.AddRange(_phenotypes.Where(p => p.Fitness < avgFitness));

            int totalInterations = fitPhenotypes.Count + (int)(unfitPhenotypes.Count * unfitBreedFactor);
            int iteration = 0;

            List<Genotype> newPhenotypes = new List<Genotype>();

            // Breed "fit" phenotypes randomly
            for (int i = 0; i < fitPhenotypes.Count; i++, iteration++)
            {
                newPhenotypes.Add(
                    new Genotype(
                        this,
                        _phenotypes[i],
                        fitPhenotypes[rnd.Next(fitPhenotypes.Count)]));
                OnBreedIterationEvent(iteration, totalInterations);
            }

            // Breed small percent of unfit phenotypes
            for (int i = 0; i < unfitPhenotypes.Count * unfitBreedFactor; i++, iteration++)
            {
                newPhenotypes.Add(
                    new Genotype(
                        this,
                        unfitPhenotypes[rnd.Next(unfitPhenotypes.Count)],
                        _phenotypes[rnd.Next(_phenotypes.Count)]));
                OnBreedIterationEvent(iteration, totalInterations);
            }

            newPhenotypes.AddRange(_phenotypes);

            _phenotypes.Clear();

            _phenotypes.AddRange(newPhenotypes.OrderByDescending(p => p.Fitness).Take(_populationSize));
        }

        /// <summary>
        /// Initializes a new population of random potential solutions
        /// </summary>
        /// <param name="populationSize">Size of the population</param>
        public void InitializePopulation(int populationSize)
        {
            _populationSize = populationSize;
            for (int i = 0; i < populationSize; i++)
            {
                _phenotypes.Add(new Genotype(this));
                OnPopulationInitEvent(i, populationSize);
            }
        }

        #endregion Public Methods 
        #region Protected Methods (2) 

        /// <summary>
        /// Raises the <see cref="BreedIterationEvent">Breed Iteration Event</see>.
        /// </summary>
        /// <param name="item">Number of the current item</param>
        /// <param name="total">Total number of items to process</param>
        protected virtual void OnBreedIterationEvent(int item, int total)
        {
            if (BreedIterationEvent != null)
                BreedIterationEvent(this, new IterationEventArgs(item, total));
        }

        /// <summary>
        /// Raises the <see cref="PopulationInitEvent">Population Init Event</see>.
        /// </summary>
        /// <param name="item">Number of the current item</param>
        /// <param name="total">Total number of items to process</param>
        protected virtual void OnPopulationInitEvent(int item, int total)
        {
            if (PopulationInitEvent != null)
                PopulationInitEvent(this, new IterationEventArgs(item, total));
        }

        #endregion Protected Methods 

        #endregion Methods 

        #region Nested Classes (2) 

        /// <summary>
        /// Represents the "genetic structure" of the potential solutions.  (i.e. the chromosome)
        /// </summary>
        public class Genotype
        {
            #region Fields (2) 

            #region Private Fields (2) 

            private List<ushort> _Genes = new List<ushort>();
            private GeneticSearch _geneticSearch;

            #endregion Private Fields 

            #endregion Fields 

            #region Constructors (4) 

            /// <summary>
            /// Initializes a new instance of a <see cref="Genotype">Genotype</see> class,
            /// (i.e. a phenotype, an individual), from two parents.
            /// </summary>
            /// <param name="geneticSearch">Instance of the <see cref="GeneticSearch">Genetic Search</see> object.</param>
            /// <param name="parent1">Parent phenotype #1</param>
            /// <param name="parent2">Parent phenotype #2</param>
            public Genotype(GeneticSearch geneticSearch, Genotype parent1, Genotype parent2)
                : this(geneticSearch, parent1.Chromosome, parent2.Chromosome)
            {
            }

            /// <summary>
            /// Initializes a new instance of a <see cref="Genotype">Genotype</see> class,
            /// (i.e. a phenotype, an individual), from two parent chromosomes.
            /// </summary>
            /// <param name="geneticSearch">Instance of the <see cref="GeneticSearch">Genetic Search</see> object.</param>
            /// <param name="chromosome1">Chromosome of parent phenotype #1</param>
            /// <param name="chromosome2">Chromosome of parent phenotype #2</param>
            public Genotype(GeneticSearch geneticSearch, UInt64 chromosome1, UInt64 chromosome2)
            {
                _geneticSearch = geneticSearch;
                int crossPoint = _geneticSearch.rnd.Next(_geneticSearch.ChrosomeLength);
                UInt64 lowmask = (UInt64)(1 << crossPoint) - 1;
                UInt64 highmask = ((UInt64)(1 << _geneticSearch.ChrosomeLength) - 1) ^ lowmask;
                UInt64 newChromosome;

                if (crossPoint == 0) newChromosome = chromosome1;
                else if (crossPoint == _geneticSearch.ChrosomeLength) newChromosome = chromosome2;
                else newChromosome = (chromosome1 & lowmask) | (chromosome2 & highmask);

                if (_geneticSearch.rnd.NextDouble() < _geneticSearch.MutationProbability)
                    newChromosome ^= (UInt64)(1 << (_geneticSearch.rnd.Next(_geneticSearch.ChrosomeLength)));

                _Genes.Clear();
                _Genes.AddRange(ChromosomeToGenes(newChromosome));
                for (int i = 0; i < _Genes.Count; i++)
                {
                    while (_Genes[i] > _geneticSearch.MaxGene) _Genes[i] = (ushort)_geneticSearch.rnd.Next(_geneticSearch.MaxGene);
                }

                Fitness = _geneticSearch._FitnessFunction(this);
            }

            /// <summary>
            /// Initializes a new instance of a <see cref="Genotype">Genotype</see> class,
            /// (i.e. a phenotype, an individual), from a list of genes.
            /// </summary>
            /// <param name="geneticSearch">Instance of the <see cref="GeneticSearch">Genetic Search</see> object.</param>
            /// <param name="genes">List of genes</param>
            public Genotype(GeneticSearch geneticSearch, IEnumerable<ushort> genes)
            {
                _geneticSearch = geneticSearch;
                _Genes.AddRange(genes);
                Fitness = _geneticSearch._FitnessFunction(this);
            }

            /// <summary>
            /// Initializes a new instance of a <see cref="Genotype">Genotype</see> class,
            /// (i.e. a phenotype, an individual), from a chromosome.
            /// </summary>
            /// <param name="geneticSearch">Instance of the <see cref="GeneticSearch">Genetic Search</see> object.</param>
            /// <param name="chromosome"></param>
            public Genotype(GeneticSearch geneticSearch, UInt64 chromosome)
            {
                _geneticSearch = geneticSearch;
                _Genes.AddRange(ChromosomeToGenes(chromosome));
                Fitness = _geneticSearch._FitnessFunction(this);
            }

            /// <summary>
            /// Initializes a new instance of a <see cref="Genotype">Genotype</see> class,
            /// (i.e. a phenotype, an individual).  The new instance will have randomly generated genes.
            /// </summary>
            /// <param name="geneticSearch">Instance of the <see cref="GeneticSearch">Genetic Search</see> object.</param>
            public Genotype(GeneticSearch geneticSearch)
            {
                _geneticSearch = geneticSearch;
                _Genes = new List<ushort>();

                for (int i = 0; i < _geneticSearch._NumGenes; i++)
                    _Genes.Add((ushort)_geneticSearch.rnd.Next(_geneticSearch.MaxGene));
                //_Genes.Sort();
                Fitness = _geneticSearch._FitnessFunction(this);
            }

            #endregion Constructors 

            #region Properties (3) 

            /// <summary>
            /// Gets or sets the genetic structure as a single 64-bit chromosome
            /// </summary>
            public UInt64 Chromosome
            {
                get
                {
                    return GenesToChromosome(_Genes);
                }
                set
                {
                    _Genes.Clear();
                    _Genes.AddRange(ChromosomeToGenes(value));
                }
            }

            /// <summary>
            /// Gets the fitness of this phenotype
            /// </summary>
            public double Fitness { get; private set; }

            /// <summary>
            /// Gets the genetic structure as a list of genes
            /// </summary>
            public List<ushort> Genes
            {
                get { return _Genes; }
            }

            #endregion Properties 

            #region Methods (2) 

            #region Private Methods (2) 

            private IEnumerable<ushort> ChromosomeToGenes(UInt64 chromosome)
            {
                return
                        Enumerable.Range(0, _geneticSearch._NumGenes)
                            .Select(i =>
                                (ushort)((chromosome & (_geneticSearch.GeneMask << (i * _geneticSearch._GeneLen))) >> (i * _geneticSearch._GeneLen)));
            }

            private UInt64 GenesToChromosome(IEnumerable<ushort> genes)
            {
                UInt64 chromosome = 0;
                int i = 0;

                foreach (var gene in genes)
                {
                    chromosome |= (UInt64)gene << i;
                    i += _geneticSearch._GeneLen;
                }

                return chromosome;
            }

            #endregion Private Methods 

            #endregion Methods 
        }

        /// <summary>
        /// An <see cref="EventArgs">EventArgs</see> for iteration events.
        /// </summary>
        public class IterationEventArgs : EventArgs
        {
            #region Constructors (1) 

            /// <summary>
            /// Initializes a new instance of the <see cref="IterationEventArgs">IterationEventArgs</see> class.
            /// </summary>
            /// <param name="item">Number of this iteration</param>
            /// <param name="total">Total iterations</param>
            public IterationEventArgs(int item, int total)
            {
                Item = item;
                Total = total;
            }

            #endregion Constructors 

            #region Properties (2) 

            /// <summary>
            /// Gets the number of this iteration
            /// </summary>
            public int Item { get; private set; }

            /// <summary>
            /// Gets the total number of iterations
            /// </summary>
            public int Total { get; private set; }

            #endregion Properties 
        }

        #endregion Nested Classes 
    }
}