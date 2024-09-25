using System;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;

namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Event arguments for the
    /// <see cref="KeyedTraceCollection{TTrace, TPoint}.Changed">KeyedTraceCollection.Changed</see> event
    /// </summary>
    public class TraceCollectionChangedEventArgs<TTrace, TPoint> : EventArgs
        where TTrace : IGraphTrace<TTrace, TPoint>
        where TPoint : IGraphPoint
    {
        #region Fields (3) 

        #region Private Fields (3) 

        private TTrace _ChangedItem;
        private TraceChangeType _ChangeType;
        private TTrace _ReplacedWith;

        #endregion Private Fields 

        #endregion Fields 

        #region Constructors (1) 

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceCollectionChangedEventArgs{TTrace, TPoint}">TraceCollectionChangedEventArgs</see>
        /// </summary>
        /// <param name="change">Type of change that occured</param>
        /// <param name="item">Item that was changed</param>
        /// <param name="replacement">Replacement item, if the item is being replaced</param>
        public TraceCollectionChangedEventArgs(TraceChangeType change,
            TTrace item, TTrace replacement)
        {
            _ChangeType = change;
            _ChangedItem = item;
            _ReplacedWith = replacement;
        }

        #endregion Constructors 

        #region Properties (3) 

        /// <summary>
        /// Gets the <see cref="IGraphTrace{TTrace, TPoint}">Item</see> that was changed
        /// </summary>
        public TTrace ChangedItem { get { return _ChangedItem; } }

        /// <summary>
        /// Gets the <see cref="TraceChangeType">Type of Change</see> that occured
        /// </summary>
        public TraceChangeType ChangeType { get { return _ChangeType; } }

        /// <summary>
        /// Gets the replacement <see cref="IGraphTrace{TTrace, TPoint}">Item</see>, if the item is being replaced
        /// </summary>
        public TTrace ReplacedWith { get { return _ReplacedWith; } }

        #endregion Properties 
    }
}