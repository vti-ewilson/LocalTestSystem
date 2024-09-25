using System;
using System.Collections.Generic;

namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Represents the configuration settings for a DataPlot
    /// </summary>
    public sealed class DataPlotSettings : GraphSettings
    {
        #region Fields (12)

        #region Private Fields (12)

        private bool _AutoRun1 = true;
        private bool _AutoRun2 = true;
        private bool _AutoShowAll = true;
        private bool _AutoShowEnd = false;
        private float _DataCollectionInterval = 0.1F;
        private string _Header = string.Empty;

        private bool _LegendForPrintedPlot = true;

        private XAxisDisplayUnitsType _XAxisDisplayUnits = XAxisDisplayUnitsType.SecondsMinutes;
        private XAxisUnitsType _XAxisUnits = XAxisUnitsType.Seconds;
        private string _DecimationThreshold = "30000"; // number of data point allowed before DataPlot performs automatic decimation
        private bool _AutoSave = false; // Set to true to have DataPlot automatically save data in DataPlot window after reach AutoPlot Threshold
        private float _XWindow = 120;
        private XWindowShiftType _XWindowShiftPercent = XWindowShiftType.Continuous;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlotSettings">DataPlotSettings</see>.
        /// </summary>
        /// <param name="sectionName">Section name in the user.config file to use for these settings</param>
        public DataPlotSettings(string sectionName)
          : base(sectionName)
        {
            TraceColors = new List<string>();
            TraceVisibility = new List<bool>();
            IsDecimatingData = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlotSettings">DataPlotSettings</see>.
        /// </summary>
        public DataPlotSettings()
          : base()
        {
            TraceColors = new List<string>();
            TraceVisibility = new List<bool>();
        }

        #endregion Constructors

        #region Properties (14)

        /// <summary>
        /// Gets or sets a value to indicate if the DataPlot should automatically
        /// start with port #1 of the system.
        /// </summary>
        public bool AutoRun1
        {
            get { return _AutoRun1; }
            set { _AutoRun1 = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the DataPlot should automatically
        /// start with port #2 of the system.
        /// </summary>
        public bool AutoRun2
        {
            get
            {
                return _AutoRun2;
            }
            set
            {
                _AutoRun2 = value;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the DataPlot should automatically
        /// adjust the X-Axis so all of the data is always visible.
        /// </summary>
        public bool AutoShowAll
        {
            get
            {
                return _AutoShowAll;
            }
            set
            {
                _AutoShowAll = value;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the DataPlot should automatically
        /// adjust the X-Axis so that the end of the data is always visible.
        /// </summary>
        public bool AutoShowEnd
        {
            get
            {
                return _AutoShowEnd;
            }
            set
            {
                _AutoShowEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval at which data should be collected
        /// </summary>
        public float DataCollectionInterval
        {
            get
            {
                return _DataCollectionInterval;
            }
            set
            {
                _DataCollectionInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets the text to be printed for the header
        /// when the data plot is printed.
        /// </summary>
        public String Header
        {
            get
            {
                return _Header;
            }
            set
            {
                _Header = value;
            }
        }

        public bool LegendForPrintedPlot
        {
            get
            {
                return _LegendForPrintedPlot;
            }
            set
            {
                _LegendForPrintedPlot = value;
            }
        }

        /// <summary>
        /// Gets or sets the text to be printed on the left side of the header
        /// when the data plot is printed.
        /// </summary>

        /// <summary>
        /// Gets or sets the text to be printed on the right side of the header
        /// when the data plot is printed.
        /// </summary>

        /// <summary>
        /// Gets or sets a list of traces colors
        /// </summary>
        public List<string> TraceColors { get; set; }

        /// <summary>
        /// Gets or sets a list that indicates which traces are visible
        /// </summary>
        public List<bool> TraceVisibility { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate the type of units to display on the X-Axis
        /// </summary>
        public XAxisDisplayUnitsType XAxisDisplayUnits
        {
            get
            {
                return _XAxisDisplayUnits;
            }
            set
            {
                _XAxisDisplayUnits = value;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the X-Axis should be in seconds or minutes
        /// </summary>
        //[XmlIgnore]
        public XAxisUnitsType XAxisUnits
        {
            get
            {
                return _XAxisUnits;
            }
            set
            {
                _XAxisUnits = value;
            }
        }

        /// <summary>
        /// Tells whether DataPlot is currently decimating data
        /// </summary>
        public bool IsDecimatingData { get; set; }

        /// <summary>
        /// Number of data point allowed before DataPlot performs automatic decimation
        /// </summary>
        public string DecimationThreshold
        {
            get { return _DecimationThreshold; }
            set { _DecimationThreshold = value; }
        }

        /// <summary>
        /// Check this box to have the DataPlot automatically save the data in DataPlot window after 'AutoSave Threshold' points have been collected.
        /// </summary>
        public bool AutoSave
        {
            get { return _AutoSave; }
            set { _AutoSave = value; }
        }

        /// <summary>
        /// Gets or sets the width of the window to be used on the X-Axis
        /// when showing the end of the plot data.
        /// </summary>
        public float XWindow
        {
            get
            {
                return _XWindow;
            }
            set
            {
                if (value <= 0) _XWindow = 120;
                else _XWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate the percentage of total width
        /// that the window should shift when data reaches the end of the window.
        /// </summary>
        public XWindowShiftType XWindowShiftPercent
        {
            get
            {
                return _XWindowShiftPercent;
            }
            set
            {
                _XWindowShiftPercent = value;
            }
        }

        #endregion Properties

        #region Methods (1)

        #region Public Methods (1)

        /// <summary>
        /// Returns a value to indicate if the DataPlot should start automatically.
        /// </summary>
        /// <param name="index">Index of the port</param>
        /// <returns>True if the DataPlot should autostart</returns>
        public bool AutoRun(int index)
        {
            switch (index)
            {
                case 0:
                    return this.AutoRun1;

                case 1:
                    return this.AutoRun2;

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}