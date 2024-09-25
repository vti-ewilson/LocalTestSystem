using System;
using VTIWindowsControlLibrary.Classes.IO;

namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Used to create traces on the DataPlot
    /// Can be used either in conjuction with an AnalogSignal or with a local Label and Value
    /// </summary>
    public class DataPlotTraceType : TraceTypeBase<DataPlotTraceType, DataPointType>
    {
        #region Fields (8) 

        #region Private Fields (7) 

        private AnalogSignal _AnalogSignal;
        private float _Value;
        #endregion Private Fields 

        #endregion Fields 

        #region Constructors (5) 

        /// <summary>
        /// Initializes a new instance of a <see cref="DataPlotTraceType">DataPlotTraceType</see>
        /// </summary>
        public DataPlotTraceType()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of a <see cref="DataPlotTraceType">DataPlotTraceType</see>
        /// </summary>
        /// <param name="key">Key for the trace</param>
        /// <param name="label">Label for the trace</param>
        /// <param name="visible">Value indicating whether the trace should be visible</param>
        public DataPlotTraceType(string key, string label, bool visible)
            : base(key, label, visible)
        { }

        /// <summary>
        /// Initializes a new instance of a <see cref="DataPlotTraceType">DataPlotTraceType</see>
        /// </summary>
        /// <param name="key">Key for the trace</param>
        /// <param name="label">Label for the trace</param>
        public DataPlotTraceType(string key, string label)
            : base(key, label)
        { }

        /// <summary>
        /// Initializes a new instance of a <see cref="DataPlotTraceType">TraceType</see>
        /// </summary>
        /// <param name="analogSignal">Analog Signal associated with the trace</param>
        public DataPlotTraceType(AnalogSignal analogSignal)
            : base(analogSignal.Key, analogSignal.Label)
        {
            _AnalogSignal = analogSignal;
            Units = analogSignal.Units;
            Format = analogSignal.Format;
            analogSignal.ValueChanged += new EventHandler(AnalogSignal_ValueChanged);
        }

        #endregion Constructors 

        #region Properties (7) 

        /// <summary>
        /// Gets the <see cref="AnalogSignal">AnalogSignal</see> for the trace
        /// </summary>
        public AnalogSignal AnalogSignal
        {
            get { return _AnalogSignal; }
        }

        /// <summary>
        /// Gets the label for the trace
        /// </summary>
        public override string Label
        {
            get
            {
                if (_AnalogSignal == null)
                    return base.Label;
                else
                    return _AnalogSignal.Label;
            }
            set
            {
                if (_AnalogSignal == null)
                    base.Label = value;
                else
                    _AnalogSignal.Label = value;

                OnChanged(TraceChangeType.Changed, this, null);
            }
        }

        /// <summary>
        /// Gets or sets the current value of the trace
        /// </summary>
        public float Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                OnValueChanged();
            }
        }

        #endregion Properties 

        #region Delegates and Events (2) 

        #region Events (2) 

        /// <summary>
        /// Occurs when the value of the trace changes
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion Events 

        #endregion Delegates and Events 

        #region Methods (3) 

        #region Protected Methods (2) 

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        #endregion Protected Methods 
        #region Private Methods (1) 

        private void AnalogSignal_ValueChanged(object sender, EventArgs e)
        {
            this.Value = (float)((sender as AnalogSignal).Value);
        }

        #endregion Private Methods 

        #endregion Methods 
    }
}