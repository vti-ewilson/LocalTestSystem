using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;
using VTIWindowsControlLibrary.Classes.Graphing.Util;

namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Represents a base class for any class that needs to implement the
    /// <see cref="IGraphTrace{TTrace, TPoint}">IGraphTrace</see> interface.
    /// </summary>
    /// <typeparam name="TTrace">Type of trace</typeparam>
    /// <typeparam name="TPoint">Type of graph point</typeparam>
    public abstract class TraceTypeBase<TTrace, TPoint> : IGraphTrace<TTrace, TPoint>
        where TTrace : class, IGraphTrace<TTrace, TPoint>, new()
        where TPoint : class, IGraphPoint, new()
    {
        #region Fields (7) 

        #region Private Fields (7) 

        private Color _Color;
        private bool _IsOverlay = false;
        private string _Key;
        private string _Label;
        private List<TPoint> _Points;
        private int _PointSize = 5;

        //private float _Value;
        private bool _Visible = true;

        #endregion Private Fields 

        #endregion Fields 

        #region Methods (1) 

        #region Protected Methods (1) 

        /// <summary>
        /// Raises the <see cref="Changed">Changed</see> event
        /// </summary>
        /// <param name="Change">Type of change that occured</param>
        /// <param name="Item">Trace that was changed</param>
        /// <param name="Replacement">Replacement trace, if the trace is being replaced</param>
        protected virtual void OnChanged(TraceChangeType Change,
            TTrace Item, TTrace Replacement)
        {
            if (Changed != null)
                Changed(this, new TraceCollectionChangedEventArgs<TTrace, TPoint>(Change, Item, Replacement));
        }

        #endregion Protected Methods 

        #endregion Methods 

        #region IGraphTrace Members

        /// <summary>
        /// Occurs when a property of the trace changes.
        /// </summary>
        public event EventHandler<TraceCollectionChangedEventArgs<TTrace, TPoint>> Changed;

        //public event EventHandler ValueChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTypeBase{TTrace, TPoint}">TraceTypeBase</see> class.
        /// </summary>
        public TraceTypeBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTypeBase{TTrace, TPoint}">TraceTypeBase</see> class.
        /// </summary>
        /// <param name="key">Key for the trace</param>
        /// <param name="label">Label for the trace</param>
        public TraceTypeBase(string key, string label)
            : this(key, label, true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTypeBase{TTrace, TPoint}">TraceTypeBase</see> class.
        /// </summary>
        /// <param name="key">Key for the trace</param>
        /// <param name="label">Label for the trace</param>
        /// <param name="visible">Value to indicate whether the trace is visible</param>
        public TraceTypeBase(string key, string label, bool visible)
        {
            _Key = key;
            _Label = label;
            _Visible = visible;
            _Points = new List<TPoint>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTypeBase{TTrace, TPoint}">TraceTypeBase</see> class.
        /// </summary>
        /// <param name="key">Key for the trace</param>
        /// <param name="label">Label for the trace</param>
        /// <param name="points">List of graph points</param>
        public TraceTypeBase(string key, string label, List<TPoint> points)
            : this(key, label, points, true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTypeBase{TTrace, TPoint}">TraceTypeBase</see> class.
        /// </summary>
        /// <param name="key">Key for the trace</param>
        /// <param name="label">Label for the trace</param>
        /// <param name="points">List of graph points</param>
        /// <param name="visible">Value to indicate whether the trace is visible</param>
        public TraceTypeBase(string key, string label, List<TPoint> points, bool visible)
        {
            _Key = key;
            _Label = label;
            _Visible = visible;
            _Points = points;
        }

        /// <summary>
        /// Gets or sets the key for the trace
        /// </summary>
        public virtual String Key
        {
            get { return _Key; }
            set
            {
                if (string.IsNullOrEmpty(_Key)) _Key = value;
                else throw new Exception("Key already set.");
            }
        }

        /// <summary>
        /// Gets or sets the label for the trace
        /// </summary>
        public virtual String Label
        {
            get
            {
                return _Label;
            }
            set
            {
                _Label = value;

                OnChanged(TraceChangeType.Changed, this as TTrace, null);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the trace is visible
        /// </summary>
        public virtual bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;

                OnChanged(TraceChangeType.Changed, this as TTrace, null);
            }
        }

        /// <summary>
        /// Gets or sets the color of the trace
        /// </summary>
        [XmlIgnore()]
        public virtual Color Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                {
                    _Color = value;
                    OnChanged(TraceChangeType.Changed, this as TTrace, null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the trace in the Html Color format.
        /// </summary>
        [XmlElement("Color")]
        public virtual string XmlColor
        {
            get { return ColorTranslator.ToHtml(_Color); }
            set
            {
                try
                {
                    _Color = ColorTranslator.FromHtml(value);
                }
                catch
                {
                    _Color = DefaultTraceColors.NextColor;
                }
            }
        }

        /// <summary>
        /// Gets or sets the point size to be used when displaying the trace as
        /// <see cref="TraceDisplayType.Point">Points</see>,
        /// <see cref="TraceDisplayType.Cross">Crosses</see> or
        /// <see cref="TraceDisplayType.Plus">Pluses</see>.
        /// </summary>
        public virtual int PointSize
        {
            get { return _PointSize; }
            set
            {
                if (_PointSize != value)
                {
                    _PointSize = value;
                    OnChanged(TraceChangeType.Changed, this as TTrace, null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of graph points.
        /// </summary>
        public virtual List<TPoint> Points
        {
            get { return _Points; }
            set { _Points = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the trace is an overlay trace.
        /// </summary>
        public virtual bool IsOverlay
        {
            get
            {
                return _IsOverlay;
            }
            set
            {
                _IsOverlay = value;
            }
        }

        /// <summary>
        /// Gets or sets the units for the trace.
        /// </summary>
        public virtual string Units { get; set; }

        /// <summary>
        /// Gets or sets the format string to be used when displaying numeric values.
        /// </summary>
        public virtual string Format { get; set; }

        private int _LineWidth = 1;

        /// <summary>
        /// Gets or sets the line width to be used when displaying the trace as a
        /// <see cref="TraceDisplayType.Line">Line</see> or
        /// <see cref="TraceDisplayType.Histogram">Histogram</see>.
        /// </summary>
        public virtual int LineWidth
        {
            get { return _LineWidth; }
            set { _LineWidth = value; }
        }

        private int _ZOrder = 0;

        /// <summary>
        /// Order of the trace in the graph.
        /// </summary>
        public virtual int ZOrder
        {
            get { return _ZOrder; }
            set { _ZOrder = value; }
        }

        private TraceDisplayType _DisplayType = TraceDisplayType.Line;

        /// <summary>
        /// Gets or sets a value to indicate how to display the trace.
        /// </summary>
        public virtual TraceDisplayType DisplayType
        {
            get { return _DisplayType; }
            set { _DisplayType = value; }
        }

        #endregion
    }
}