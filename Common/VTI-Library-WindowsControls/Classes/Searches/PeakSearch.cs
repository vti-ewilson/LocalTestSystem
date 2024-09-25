using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using VTIWindowsControlLibrary.Classes.Graphing;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;
using VTIWindowsControlLibrary.Classes.Graphing.Util;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Classes.Searches
{
    /// <summary>
    /// Performs a search in a range of data for a particular arrangement
    /// of peaks in the data.  The peaks to be located are designated by a
    /// list of <see cref="PeakSearchObjective">Peak Search Objectives</see>.
    /// The peak search works by comparing the relative horizontal spacing of
    /// the peaks to find the best match.  It also requires that certain peaks
    /// meet a qualification of being a "major peak".
    /// </summary>
    /// <typeparam name="TData">Type of graph data class.
    /// Must be a sub-class of the <see cref="GraphData{TCollection, TTrace, TPoint, TSettings}">GraphData</see> class.</typeparam>
    /// <typeparam name="TCollection">Type of trace collection containing the trace data
    /// to be searched.  Must be a sub-class of the <see cref="KeyedTraceCollection{TTrace, TPoint}">KeyedTraceCollection</see> class.</typeparam>
    /// <typeparam name="TTrace">Type of the trace to be searched.
    /// Must implement the <see cref="IGraphTrace{TTrace, TPoint}">IGraphTrace</see> interface.</typeparam>
    /// <typeparam name="TPoint">Type of the graph point in the trace to be searched.
    /// Must implement the <see cref="IGraphPoint">IGraphPoint</see> interface.</typeparam>
    /// <typeparam name="TSettings">Type of the graph settings for the graph which contains
    /// the trace being searched.
    /// Must be a sub-class of the <see cref="GraphSettings">GraphSettings</see> class.</typeparam>
    public class PeakSearch<TData, TCollection, TTrace, TPoint, TSettings>
        where TData : GraphData<TCollection, TTrace, TPoint, TSettings>, new()
        where TCollection : KeyedTraceCollection<TTrace, TPoint>, new()
        where TTrace : class, IGraphTrace<TTrace, TPoint>, new()
        where TPoint : class, IGraphPoint, new()
        where TSettings : GraphSettings, new()
    {
        #region Fields (14) 

        #region Private Fields (14) 

        private GraphControl<TData, TCollection, TTrace, TPoint, TSettings> _graphControl;
        private List<TPoint> _majorPeaks;
        private TTrace _peaks;
        private List<PeakSearchObjective> _peakSearchObjectives;
        private List<TTrace> _phenotypeTraces;
        private List<PeakSearchResult> _searchResults;
        private TTrace _smoothTrace;
        private List<TPoint> _sourceData;
        private BackgroundWorker backgroundWorker;
        private bool cancelTune;
        private GeneticSearch geneticSearch;
        private float majorPeakThreshold;
        private float[,] peakProbability;
        private string statusMessage;

        #endregion Private Fields 

        #endregion Fields 

        #region Constructors (2) 

        /// <summary>
        /// Initializes a new instance of a <see cref="PeakSearch{TData, TCollection, TTrace, TPoint, TSettings}">PeakSearch</see> class.
        /// </summary>
        /// <param name="peakSearchObjectives">List of <see cref="PeakSearchObjective">Target Peaks</see> to attempt to locate in the source data.</param>
        /// <param name="sourceData">List of points through which to search for the target peaks.</param>
        /// <param name="graphControl">Graph control which can be used to display the peak search in progress.</param>
        public PeakSearch(List<PeakSearchObjective> peakSearchObjectives, List<TPoint> sourceData, GraphControl<TData, TCollection, TTrace, TPoint, TSettings> graphControl)
            : this(peakSearchObjectives, sourceData)
        {
            _graphControl = graphControl;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="PeakSearch{TData, TCollection, TTrace, TPoint, TSettings}">PeakSearch</see> class.
        /// </summary>
        /// <param name="peakSearchObjectives">List of <see cref="PeakSearchObjective">Target Peaks</see> to attempt to locate in the source data.</param>
        /// <param name="sourceData">List of points through which to search for the target peaks.</param>
        public PeakSearch(List<PeakSearchObjective> peakSearchObjectives, List<TPoint> sourceData)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);

            _peakSearchObjectives = peakSearchObjectives;
            if (_peakSearchObjectives.Count < 3) throw new Exception("Peak search must contain at least 3 objectives.");
            if (_peakSearchObjectives.Count > 12) throw new Exception("Peak search can only contain up to 12 objectives.");

            _sourceData = sourceData;
        }

        #endregion Constructors 

        #region Properties (1) 

        /// <summary>
        /// Gets the list of <see cref="PeakSearchResult">Peak Search Results</see> once the
        /// peak search is complete.
        /// </summary>
        public List<PeakSearchResult> SearchResults
        {
            get { return _searchResults; }
        }

        #endregion Properties 

        #region Delegates and Events (3) 

        #region Events (3) 

        /// <summary>
        /// Event handler for displaying detailed messages as the peak search progresses.
        /// </summary>
        public event EventHandler<DetailedMessageEventArgs> DetailedMessageEvent;

        /// <summary>
        /// Event handler for displaying peak search progress percentage as the peak search progresses.
        /// </summary>
        public event EventHandler<ProgressEventArgs> ProgressEvent;

        /// <summary>
        /// Event handler to signal that the peak search is complete.
        /// </summary>
        public event EventHandler<SearchCompleteEventArgs> SearchComplete;

        #endregion Events 

        #endregion Delegates and Events 

        #region Methods (16) 

        #region Public Methods (3) 

        /// <summary>
        /// Resets the peak search and removes any automatically generated traces from the graph control.
        /// </summary>
        public void Reset()
        {
            if (_smoothTrace != null && _graphControl.Traces.Contains(_smoothTrace))
                _graphControl.Traces.Remove(_smoothTrace);

            if (_peaks != null && _graphControl.Traces.Contains(_peaks))
                _graphControl.Traces.Remove(_peaks);

            if (_phenotypeTraces != null)
            {
                foreach (var trace in _phenotypeTraces)
                {
                    if (_graphControl.Traces.Contains(trace))
                        _graphControl.Traces.Remove(trace);
                }
            }

            _graphControl.ReDrawGraph();
        }

        /// <summary>
        /// Starts the peak search.
        /// </summary>
        public void Start()
        {
            if (_sourceData.Count == 0) throw new Exception("Source Data must not be empty.");

            cancelTune = false;

            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Stops the peak search.
        /// </summary>
        public void Stop()
        {
            if (backgroundWorker.IsBusy)
                cancelTune = true;
            else
                Reset();
        }

        #endregion Public Methods 
        #region Protected Methods (3) 

        /// <summary>
        /// Raises the <see cref="DetailedMessageEvent">DetailedMessageEvent</see> event.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        protected virtual void OnDetailedMessageEvent(string message)
        {
            if (DetailedMessageEvent != null)
                DetailedMessageEvent(this, new DetailedMessageEventArgs(message));
        }

        /// <summary>
        /// Raises the <see cref="ProgressEvent">ProgressEvent</see> event.
        /// </summary>
        /// <param name="status">Status message to be sent.</param>
        /// <param name="progressPercentage">Progress percentage.</param>
        protected virtual void OnProgressEvent(string status, int progressPercentage)
        {
            if (ProgressEvent != null)
                ProgressEvent(this, new ProgressEventArgs(status, progressPercentage));
        }

        /// <summary>
        /// Raises the <see cref="SearchComplete">SearchComplete</see> event.
        /// </summary>
        /// <param name="peakSearchResults">List of <see cref="PeakSearchResult">Peak Search Results</see>.</param>
        protected virtual void OnSearchComplete(List<PeakSearchResult> peakSearchResults)
        {
            if (SearchComplete != null)
                SearchComplete(this, new SearchCompleteEventArgs(peakSearchResults));
        }

        #endregion Protected Methods 
        #region Private Methods (10) 

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            OnDetailedMessageEvent("Peak Search Started...");

            OnDetailedMessageEvent(
                string.Format("Peak Search Objectives: {0}",
                    string.Join(", ",
                        _peakSearchObjectives.Select(M => M.TargetLocation.ToString()).ToArray())));

            // Create the "Smooth Trace"
            _smoothTrace = Activator.CreateInstance(typeof(TTrace), "Smooth", "Smooth Trace") as TTrace;
            _smoothTrace.Color = Color.Red;

            if (_graphControl != null)
            {
                if (_graphControl.Traces.Contains(_smoothTrace.Key))
                    _graphControl.Traces.Remove(_smoothTrace.Key);
                _graphControl.Traces.Add(_smoothTrace);
                _graphControl.BringTraceToFront(_smoothTrace);
            }

            // Create the "Peak Trace", which will highlight all possible peaks
            _peaks = Activator.CreateInstance(typeof(TTrace), "Peaks", "Peaks") as TTrace;
            _peaks.DisplayType = TraceDisplayType.Plus;
            _peaks.PointSize = 5;
            _peaks.Color = Color.Orange;
            if (_graphControl != null)
            {
                if (_graphControl.Traces.Contains(_peaks)) _graphControl.Traces.Remove(_peaks);
                _graphControl.Traces.Add(_peaks);
                _graphControl.BringTraceToFront(_peaks);
            }

            // Create the "Phenotype Traces", which will plot the possible locations
            // for each of the peak search objectives from the phenotype population.
            DefaultTraceColors.Reset();
            _phenotypeTraces = new List<TTrace>();
            foreach (var objective in _peakSearchObjectives)
            {
                TTrace trace = Activator.CreateInstance(typeof(TTrace),
                    string.Format("peak{0}points", objective.TargetLocation),
                    string.Format("Peak {0} Points", objective.TargetLocation)) as TTrace;
                trace.DisplayType = TraceDisplayType.Fill;
                trace.Color = DefaultTraceColors.NextColor;
                if (trace.Color == Color.Blue) trace.Color = DefaultTraceColors.NextColor;
                _phenotypeTraces.Add(trace);
                if (_graphControl != null)
                {
                    if (_graphControl.Traces.Contains(trace.Key))
                        _graphControl.Traces.Remove(trace.Key);
                    _graphControl.Traces.Add(trace);
                    _graphControl.BringTraceToFront(trace);
                }
            }

            _majorPeaks = new List<TPoint>();

            // Calculate the smooth trace
            statusMessage = string.Format("Smoothing Data...");
            OnDetailedMessageEvent(statusMessage);
            backgroundWorker.ReportProgress(5, statusMessage);

            lock (((IList)_sourceData).SyncRoot)
            {
                _smoothTrace.Points.AddRange(_sourceData.GetSmoothPoints());
            }
            if (_graphControl != null) _graphControl.ReDrawGraph();
            if (cancelTune) return;

            // Find the possible peaks
            statusMessage = string.Format("Finding Peaks...");
            OnDetailedMessageEvent(statusMessage);
            backgroundWorker.ReportProgress(10, statusMessage);

            lock (((IList)_peaks.Points).SyncRoot)
            {
                _peaks.Points.Clear();
                float minX = _sourceData.First().X + 0.5F;
                lock (((IList)_smoothTrace.Points).SyncRoot)
                {
                    _peaks.Points.AddRange(
                        _smoothTrace.Points.Where(p => p.X > minX).FindPeaks());
                }

                // Limit list to 28 possible peaks.
                // If there are more than 28, throw away the lowest peaks.
                if (_peaks.Points.Count > 28)
                {
                    _peaks.Points.Sort((a, b) => a.Y.CompareTo(b.Y));
                    _peaks.Points.Reverse();
                    _peaks.Points.RemoveRange(28, _peaks.Points.Count - 28);
                }
                _peaks.Points.Sort((a, b) => a.X.CompareTo(b.X));
            }

            OnDetailedMessageEvent("Candidate peaks found at:");
            OnDetailedMessageEvent(string.Join(", ",
                _peaks.Points.Select(P => P.X.ToString()).ToArray()));

            // Set the "major peak threshold".  Any peak to be considered for a
            // "major" peak must be at least 1/5th of the largest peak.
            majorPeakThreshold = _peaks.Points.OrderByDescending(p => p.Y).First().Y / 5F;
            //if (majorPeakThreshold < 5E-10F) majorPeakThreshold = 5E-10F;

            OnDetailedMessageEvent("Major peaks found at:");
            OnDetailedMessageEvent(string.Join(", ",
                _peaks.Points.Where(p => IsMajorPeak(p)).Select(P => P.X.ToString()).ToArray()));

            peakProbability = new float[_peaks.Points.Count, _peakSearchObjectives.Count];

            // Initialize the genetic algorithm
            statusMessage = "Initializing Peak Search...";
            OnDetailedMessageEvent(statusMessage);

            if (_graphControl != null) _graphControl.Invalidate();

            geneticSearch = new GeneticSearch(_peakSearchObjectives.Count, 5, FitnessFunction);
            // high (75%) mutation probability keeps GA from settling into an incorrect solution
            geneticSearch.MutationProbability = 0.75;
            geneticSearch.MaxGene = _peaks.Points.Count;
            geneticSearch.InitializePopulation(Math.Max(1000, _peaks.Points.Count * _peakSearchObjectives.Count * 20));
            if (cancelTune) return;
            CalculatePeakProbabilities();
            PlotPhenotypes();
            if (cancelTune) return;

            double fitness = 0; // fitness of current generation
            List<double> fitnessTrack = new List<double>(); // list to track fitness from generation to generation, to tell when we're done
            int prog = 20; // value for the progress bar

            statusMessage = "Performing Peak Search...";
            int generations = _peaks.Points.Count * _peakSearchObjectives.Count * 10;
            for (int generation = 0; generation < generations; generation++)
            {
                // Breed the new generation of solutions
                geneticSearch.Breed();
                // Calculate the peak probabilities
                CalculatePeakProbabilities();
                // Update the phenotype traces for the eye candy
                PlotPhenotypes();
                // Calculate the average fitness for this generation
                fitness = geneticSearch.Phenotypes.Average(p => p.Fitness);
                OnDetailedMessageEvent(
                    string.Format("Pass {0} complete. Average fitness: {1:0.0000000000}",
                        generation + 1, fitness));

                // Calculate a value for the progress bar based on the maximum
                // probability for each of the peak search objectives.
                float temp = 0;
                for (int j = 0; j < _peakSearchObjectives.Count; j++)
                {
                    float maxProb = 0;
                    for (int i = 0; i < _peaks.Points.Count; i++)
                    {
                        maxProb = Math.Max(maxProb, peakProbability[i, j]);
                    }
                    temp += maxProb;
                }
                temp = 0.95F * temp / (float)(_peakSearchObjectives.Count);
                prog = (int)Math.Max(prog, temp);

                // Track the fitness for 10 generations to see when we're done.
                fitnessTrack.Add(fitness);
                if (fitnessTrack.Count > 10) fitnessTrack.RemoveAt(0);
                if (fitnessTrack.Count(f => f == fitnessTrack.Max()) == 5)
                    prog = 97;
                if (fitnessTrack.Count(f => f == fitnessTrack.Max()) == 10)
                {
                    OnDetailedMessageEvent("Fitness unchanged for 10 passes.");
                    break;
                }

                // Report the progress
                backgroundWorker.ReportProgress(prog, statusMessage);

                // Update the eye candy
                if (_graphControl != null) _graphControl.ReDrawGraph();

                //Application.DoEvents();
                if (cancelTune) return;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgressEvent(e.UserState as string, e.ProgressPercentage);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (cancelTune) Reset();
            else
            {
                OnProgressEvent("Peak Search Complete.", 100);
                OnDetailedMessageEvent("Peak Search Complete.");

                OnDetailedMessageEvent("Peak Location Analysys:");

                // Check for which peaks were found by the GA
                // Peaks with probability > 99.9% are considered good
                List<PeakSearchResult> prelimResults = new List<PeakSearchResult>();
                for (int i = 0; i < _peaks.Points.Count; i++)
                {
                    for (int j = 0; j < _peakSearchObjectives.Count; j++)
                    {
                        if (peakProbability[i, j] > 99.9F)
                        {
                            prelimResults.Add(new PeakSearchResult(
                                /*j,*/
                                _peakSearchObjectives[j].TargetLocation,
                                _peaks.Points[i].X));
                        }
                    }
                }

                prelimResults.Sort((a, b) => a.TargetLocation.CompareTo(b.TargetLocation));

                OnDetailedMessageEvent("Possible Peaks Located:");
                OnDetailedMessageEvent(string.Join(", ",
                    prelimResults.Select(p => p.ActualLocation.ToString()).ToArray()));

                // Verify the located peaks
                // Some peaks may go to 100% even if they are incorrect, if there is a
                // missing tune mass and only one other peak in the vicinity.
                // Peaks are "verified" if the "tune ratio" between two of it's neighbors is
                // greater than 0.5 (which equates to a +/- 3% accuracy).
                _searchResults = new List<PeakSearchResult>();
                for (int i = 0; i < prelimResults.Count; i++)
                {
                    int j = i - 2;
                    if (j < 0) j = 0;
                    for (; j <= i && j < prelimResults.Count - 2; j++)
                    {
                        if (// Check Y-Intercept of first pair
                            CheckYIntercept(
                                prelimResults[j].TargetLocation,
                                prelimResults[j + 1].TargetLocation,
                                prelimResults[j].ActualLocation,
                                prelimResults[j + 1].ActualLocation) &&
                            // Check Y-Intercept of second pair
                            CheckYIntercept(
                                prelimResults[j + 1].TargetLocation,
                                prelimResults[j + 2].TargetLocation,
                                prelimResults[j + 1].ActualLocation,
                                prelimResults[j + 2].ActualLocation) &&
                            // Check the tune ratio
                            CheckTuneRatio(
                                prelimResults[j].TargetLocation,
                                prelimResults[j + 1].TargetLocation,
                                prelimResults[j + 2].TargetLocation,
                                prelimResults[j].ActualLocation,
                                prelimResults[j + 1].ActualLocation,
                                prelimResults[j + 2].ActualLocation) > 0.5F &&
                            // Make sure the peaks are in order
                            prelimResults[j].ActualLocation <
                                prelimResults[j + 1].ActualLocation &&
                            prelimResults[j + 1].ActualLocation <
                                prelimResults[j + 2].ActualLocation &&
                            // Make sure this peak is greater than all previous peaks
                            (_searchResults.Count == 0 ||
                             prelimResults[i].ActualLocation > _searchResults.Last().ActualLocation))
                        {
                            _searchResults.Add(prelimResults[i]);
                            break;
                        }
                    }
                }

                OnDetailedMessageEvent(
                    string.Format("Total Peaks Located: {0} out of {1}",
                        _searchResults.Count,
                        _peakSearchObjectives.Count));

                for (int i = 0; i < _searchResults.Count; i++)
                {
                    OnDetailedMessageEvent(
                        string.Format("Peak {0} located at {1:0.000}",
                            _searchResults[i].TargetLocation,
                            _searchResults[i].ActualLocation));
                }

                OnSearchComplete(_searchResults);
            }
        }

        private void CalculatePeakProbabilities()
        {
            int i, j;
            for (i = 0; i < _peaks.Points.Count; i++)
                for (j = 0; j < _peakSearchObjectives.Count; j++)
                    peakProbability[i, j] = 0;

            foreach (var phenotype in geneticSearch.Phenotypes)
                for (i = 0; i < _peakSearchObjectives.Count; i++)
                    if (phenotype.Genes[i] < _peaks.Points.Count)
                        peakProbability[phenotype.Genes[i], i] += 100F / (float)geneticSearch.Phenotypes.Count;
        }

        private double CheckTuneRatio(float target1, float target2, float target3, float actual1, float actual2, float actual3)
        {
            // Ratio of the 3 tune masses
            double tuneRatio = ((target3 - target1) / (target2 - target1));
            // Ratio of the peaks in question
            double ratio = ((actual3 - actual1) / (actual2 - actual1));
            // Compare the two ratios to come up with a % difference
            double comp = Math.Abs(tuneRatio - ratio) / tuneRatio;
            // If the % difference is less than 6%, calculate a return value
            // that is scaled from 0..1 for 6%..0% difference.
            if (comp < 0.06F) return (0.06F - comp) / 0.06F;
            else return 0;
        }

        private bool CheckYIntercept(float target1, float target2, float actual1, float actual2)
        {
            float b = actual1 - target1 * (actual2 - actual1) / (target2 - target1);
            return b > -10;  // give a little room
        }

        private TPoint CreatePoint(float x, float y)
        {
            TPoint point = Activator.CreateInstance(typeof(TPoint), x, y) as TPoint;
            return point;
        }

        /// <summary>
        /// Calculates the "fitness" of any given phenotype (individual).
        /// Fitness is based on the order of the peaks, and the ratios between
        /// the peak masses.  It is designed to be ever increasing until the
        /// optimal solution is found.
        /// </summary>
        /// <param name="phenotype">The individual for which to calculate the fitness.</param>
        /// <returns>Fitness of the individual.</returns>
        private double FitnessFunction(GeneticSearch.Genotype phenotype)
        {
            double fitness = 0;
            try
            {
                int i, j, k;
                TPoint peak1, peak2, peak3;
                bool hasMajor = false;

                // Add 0.1 to fitness for each peak pair that are in order
                for (i = 0; i < _peakSearchObjectives.Count - 1; i++)
                {
                    if (_peaks.Points[phenotype.Genes[i]].X < _peaks.Points[phenotype.Genes[i + 1]].X)
                        fitness += 0.1D;

                    if (!CheckYIntercept(
                            _peakSearchObjectives[i].TargetLocation, _peakSearchObjectives[i + 1].TargetLocation,
                            _peaks.Points[phenotype.Genes[i]].X, _peaks.Points[phenotype.Genes[i + 1]].X))
                        return 0;
                }

                // Add 1.0 to fitness if ALL peak pairs are in order
                if (fitness + 1E-6F >= 0.1D * (double)(_peakSearchObjectives.Count - 1))
                    fitness += 1D;

                // Locate any "major" peaks for this phenotype
                _majorPeaks.Clear();
                for (i = 0; i < _peakSearchObjectives.Count; i++)
                {
                    peak1 = _peaks.Points[phenotype.Genes[i]];
                    if (_peakSearchObjectives[i].IsMajor && IsMajorPeak(peak1))
                    {
                        hasMajor = true;
                        if (!_majorPeaks.Contains(peak1)) _majorPeaks.Add(peak1);
                    }
                }

                // Phenotype must have at least one major peak to continue
                if (hasMajor)
                {
                    // Pick 3 peaks to compare the ratios
                    for (i = 0; i < _peakSearchObjectives.Count - 2; i++)
                    {
                        peak1 = _peaks.Points[phenotype.Genes[i]];

                        for (j = i + 1; j < _peakSearchObjectives.Count - 1; j++)
                        {
                            peak2 = _peaks.Points[phenotype.Genes[j]];

                            for (k = j + 1; k < _peakSearchObjectives.Count; k++)
                            {
                                peak3 = _peaks.Points[phenotype.Genes[k]];

                                // Determine if there are "inverted" peaks
                                // (i.e. a "non-major peak" is taller than a "major peak"
                                for (int ii = 0; ii < 2; ii++)
                                {
                                    for (int jj = 0; jj < 2; jj++)
                                    {
                                        if (ii != jj)
                                        {
                                            if (_peakSearchObjectives[i + ii].IsMajor && !_peakSearchObjectives[i + jj].IsMajor &&
                                                _peaks.Points[phenotype.Genes[i + ii]].Y < _peaks.Points[phenotype.Genes[i + jj]].Y)
                                                return 0; // Die, all inverted bastards!  Die!
                                        }
                                    }
                                }
                                // Throw out any solutions with duplicate peaks
                                if (peak1.X == peak2.X || peak2.X == peak3.X || peak1.X == peak3.X) return 0;

                                // If the 3 peaks are in order, continue...
                                if (peak1.X < peak2.X && peak2.X < peak3.X)// &&
                                                                           //peak1.Y > 1E-11 && peak2.Y > 1E-11 && peak3.Y > 1E-11)
                                {
                                    // Calculate a value which is proportional to how close the peaks are to the correct ratio
                                    // If the peaks are out by more than 6%, this value is zero.
                                    // Otherwise, the value is 0 at 6% difference to 1 at 0% difference
                                    double ratioVal =
                                        CheckTuneRatio(_peakSearchObjectives[i].TargetLocation, _peakSearchObjectives[j].TargetLocation, _peakSearchObjectives[k].TargetLocation,
                                            peak1.X, peak2.X, peak3.X);

                                    // weight more heavily to the lower peaks
                                    ratioVal *=
                                        ((double)(_peakSearchObjectives.Count - i) / 3D +
                                         (double)(_peakSearchObjectives.Count - j - 1) / 3D +
                                         (double)(_peakSearchObjectives.Count - k - 2) / 3D);
                                    fitness += ratioVal;
                                    // If ratio is within 6% (ratio value > 0), give additional fitness
                                    // to solutions which also contain major peaks
                                    if (ratioVal > 0)
                                    {
                                        if (_peakSearchObjectives[i].IsMajor && _majorPeaks.Contains(peak1))
                                            fitness += ratioVal;
                                        if (_peakSearchObjectives[j].IsMajor && _majorPeaks.Contains(peak2))
                                            fitness += ratioVal;
                                        if (_peakSearchObjectives[k].IsMajor && _majorPeaks.Contains(peak3))
                                            fitness += ratioVal;
                                    }

                                    //// Perform linear regression on 3 points and check R-Squared Value
                                    //List<GraphPoint> peakSearchResults = new List<GraphPoint>();
                                    //peakSearchResults.Add(
                                    //    new GraphPoint(1F / peak1.X, 1F / _peakSearchObjectives[i].TargetLocation));
                                    //peakSearchResults.Add(
                                    //    new GraphPoint(1F / peak2.X, 1F / _peakSearchObjectives[j].TargetLocation));
                                    //peakSearchResults.Add(
                                    //    new GraphPoint(1F / peak3.X, 1F / _peakSearchObjectives[k].TargetLocation));
                                    //peakSearchResults.Sort((a, b) => a.X.CompareTo(b.X));
                                    //double rSquaredValue =
                                    //    peakSearchResults.GetLinearRegression().RSquaredValue;

                                    //if (rSquaredValue > 0.9999D)
                                    //    fitness += 1F;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                fitness = 0; // No room for "exceptional" solutions here!  (Sorry, bad pun...)
            }

            return fitness;
        }

        private bool IsMajorPeak(TPoint peak)
        {
            // Find nearby peaks (within 2 amu, but not this one)
            var nearby = _peaks.Points
                .Where(p => p.X != peak.X &&
                    p.X > peak.X - 2 &&
                    p.X < peak.X + 2);
            // Peak is a "major" peak if it is above the "major peak threshold"
            // And taller than any of its neighbors
            return (peak.Y > majorPeakThreshold &&
                    (nearby.Count() == 0 ||
                     nearby.Count(p => p.Y > peak.Y) == 0));
        }

        private void PlotPhenotypes()
        {
            lock (_graphControl.DrawingLock)
            {
                int i, j;
                // Place peaks in the phenotype traces proportional to the probabilities
                for (i = 0; i < _peakSearchObjectives.Count; i++)
                {
                    _phenotypeTraces[i].Points.Clear();
                    for (j = 0; j < _peaks.Points.Count; j++)
                    {
                        TPoint point = CreatePoint(_peaks.Points[j].X,
                            peakProbability[j, i] * _peaks.Points[j].Y / 100F);
                        _phenotypeTraces[i].Points.Add(point);
                    }
                }

                // Widen the peaks
                for (i = 0; i < _peakSearchObjectives.Count; i++)
                {
                    for (j = 0; j < _peaks.Points.Count; j++)
                    {
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X - 0.1F,
                                _phenotypeTraces[i].Points[j].Y * 0.8F));
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X + 0.1F,
                                _phenotypeTraces[i].Points[j].Y * 0.8F));
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X - 0.3F,
                                _phenotypeTraces[i].Points[j].Y * 0.05F));
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X + 0.3F,
                                _phenotypeTraces[i].Points[j].Y * 0.05F));
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X - 0.4F,
                                _phenotypeTraces[i].Points[j].Y * 0.005F));
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X + 0.4F,
                                _phenotypeTraces[i].Points[j].Y * 0.005F));
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X - 0.5F,
                                0));
                        _phenotypeTraces[i].Points.Add(
                            CreatePoint(
                                _phenotypeTraces[i].Points[j].X + 0.5F,
                                0));
                    }
                    _phenotypeTraces[i].Points.Add(CreatePoint(0, 0));
                    _phenotypeTraces[i].Points.Add(CreatePoint(_smoothTrace.Points.Last().X, 0));
                    _phenotypeTraces[i].Points.Sort((a, b) => a.X.CompareTo(b.X));
                }
            }
            _graphControl.ReDrawGraph();
        }

        #endregion Private Methods 

        #endregion Methods 

        #region Nested Classes (3) 

        /// <summary>
        /// An <see cref="EventArgs">EventArgs</see> class for the
        /// <see cref="DetailedMessageEvent">DetailedMessageEvent</see> event.
        /// </summary>
        public class DetailedMessageEventArgs : EventArgs
        {
            #region Constructors (1) 

            /// <summary>
            /// Initializes a new instance of the <see cref="DetailedMessageEventArgs">DetailedMessageEventArgs</see> class.
            /// </summary>
            /// <param name="message">Message to be sent.</param>
            public DetailedMessageEventArgs(string message)
            {
                Message = message;
            }

            #endregion Constructors 

            #region Properties (1) 

            /// <summary>
            /// Gets the message sent by the <see cref="DetailedMessageEvent">DetailedMessageEvent</see> event.
            /// </summary>
            public string Message { get; private set; }

            #endregion Properties 
        }

        /// <summary>
        /// An <see cref="EventArgs">EventArgs</see> class for the
        /// <see cref="DetailedMessageEvent">ProgressEvent</see> event.
        /// </summary>
        public class ProgressEventArgs : EventArgs
        {
            #region Constructors (1) 

            /// <summary>
            /// Initializes a new instance of the <see cref="ProgressEventArgs">ProgressEventArgs</see> class.
            /// </summary>
            /// <param name="status">Status message to be sent</param>
            /// <param name="progressPercentage">Progress percentage</param>
            public ProgressEventArgs(string status, int progressPercentage)
            {
                Status = status;
                ProgressPercentage = progressPercentage;
            }

            #endregion Constructors 

            #region Properties (2) 

            /// <summary>
            /// Gets the progress percentage
            /// </summary>
            public int ProgressPercentage { get; private set; }

            /// <summary>
            /// Gets the status message
            /// </summary>
            public string Status { get; private set; }

            #endregion Properties 
        }

        /// <summary>
        /// An <see cref="EventArgs">EventArgs</see> class for the
        /// <see cref="SearchComplete">SearchComplete</see> event.
        /// </summary>
        public class SearchCompleteEventArgs : EventArgs
        {
            #region Constructors (1) 

            /// <summary>
            /// Initializes a new instance of the <see cref="SearchCompleteEventArgs">SearchCompleteEventArgs</see> class.
            /// </summary>
            /// <param name="peakSearchResults">List of <see cref="PeakSearchResult">Peak Search Results</see></param>
            public SearchCompleteEventArgs(List<PeakSearchResult> peakSearchResults)
            {
                PeakSearchResults = peakSearchResults;
            }

            #endregion Constructors 

            #region Properties (1) 

            /// <summary>
            /// Gets the list of <see cref="PeakSearchResult">Peak Search Results</see> from the peak search.
            /// </summary>
            public List<PeakSearchResult> PeakSearchResults { get; private set; }

            #endregion Properties 
        }

        #endregion Nested Classes 
    }
}