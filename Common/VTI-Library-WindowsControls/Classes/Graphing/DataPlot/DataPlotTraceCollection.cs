using System;
using System.Drawing;
using VTIWindowsControlLibrary.Classes.Graphing.Util;
using VTIWindowsControlLibrary.Classes.IO;

namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Collection of traces for the DataPlot
    /// Traces can be added from AnalogSignals via the AddAnalogSignal() and AddAllAnalogSignals() methods
    /// Traces with a general purpose string value may be added via the Add() method
    /// The non-AnalogSignal traces can be used to write additional non-visual information to the DataPlot output files
    /// </summary>
    public class DataPlotTraceCollection : KeyedTraceCollection<DataPlotTraceType, DataPointType>
    {
        #region Fields (1) 

        #region Private Fields (1) 

        private DataPlotSettings _settings;

        #endregion Private Fields 

        #endregion Fields 

        #region Methods (14) 

        #region Public Methods (5) 

        /// <summary>
        /// Adds a general-purpose trace to the DataPlot
        /// </summary>
        /// <param name="newTrace">Trace to be added</param>
        public new void Add(DataPlotTraceType newTrace)
        {
            base.Add(newTrace);

            // Retrieve trace visibility from DataPlot.Settings
            if (_settings != null &&
                _settings.TraceVisibility != null &&
                _settings.TraceVisibility.Count >= this.Count)
            {
                newTrace.Visible = _settings.TraceVisibility[this.Count - 1];
            }
            else
                newTrace.Visible = true;    // Default to Visible

            // Retrieve trace color from DataPlot.Settings
            if (_settings != null &&
                _settings.TraceColors != null &&
                _settings.TraceColors.Count >= this.Count)
                try
                {
                    newTrace.Color = ColorTranslator.FromHtml(_settings.TraceColors[this.Count - 1]);
                }
                catch
                {
                    newTrace.Color = DefaultTraceColors.NextColor;
                }
            else
                newTrace.Color = DefaultTraceColors.NextColor;
        }

        /// <summary>
        /// Adds all of the Analog Signals in the IO class to the DataPlot
        /// </summary>
        /// <param name="analogSignals">Instance of the IO class from the client application</param>
        public void AddAllAnalogSignals(AnalogSignalCollection analogSignals)
        {
            foreach (var signal in analogSignals)
                AddAnalogSignal(signal);
        }

        /// <summary>
        /// Adds a trace to the DataPlot for an AnalogSignal
        /// </summary>
        /// <param name="analogSignal">Analog Signal for the trace</param>
        public void AddAnalogSignal(AnalogSignal analogSignal)
        {
            VtiEvent.Log.WriteVerbose("Adding Analog Signal '" + analogSignal.Label + "' to Data Plot Traces.");

            DataPlotTraceType newTrace = new DataPlotTraceType(analogSignal);
            Add(newTrace);
            analogSignal.VisibleChanged += new EventHandler(AnalogSignal_VisibleChanged);
            analogSignal.LabelChanged += new EventHandler(AnalogSignal_LabelChanged);
            analogSignal.ValueChanged += new EventHandler(AnalogSignal_ValueChanged);
            analogSignal.Trace = newTrace;
        }

        private void AnalogSignal_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                AnalogSignal signal = sender as AnalogSignal;
                this[signal.Key].Value = (float)signal.Value;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Adds a range of new traces to the DataPlot
        /// </summary>
        /// <param name="traces">Array of traces to be added</param>
        public void AddRange(DataPlotTraceType[] traces)
        {
            foreach (var trace in traces) Add(trace);
        }

        #endregion Public Methods 

        #region Private Methods (3) 

        private void AnalogSignal_LabelChanged(object sender, EventArgs e)
        {
            AnalogSignal signal = sender as AnalogSignal;
            if (this[signal.Key].Label != signal.Label) this[signal.Key].Label = signal.Label;
        }

        private void AnalogSignal_VisibleChanged(object sender, EventArgs e)
        {
            AnalogSignal signal = sender as AnalogSignal;
            this[signal.Key].Visible = signal.Visible;
        }

        #endregion Private Methods 

        #endregion Methods 

        /// <summary>
        /// Gets or sets the settings for the associated data plot.
        /// </summary>
        public DataPlotSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
    }
}