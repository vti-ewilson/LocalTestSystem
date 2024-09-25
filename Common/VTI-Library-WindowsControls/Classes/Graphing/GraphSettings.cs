using System;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Represents the settings for a
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">Graph Control</see>.
    /// </summary>
    public class GraphSettings : ObjectConfiguration
    {
        #region Fields (17)

        #region Private Fields (17)

        private bool _AutoScaleYMaxExp = false;
        private bool _AutoScaleYMinExp = false;
        private bool _CommentsVisible = true;
        private bool _DrawPlotCursorCallouts = true;
        private bool _LegendEnabled = true;
        private bool _LegendVisible = true;
        private bool _YAxisButtonsVisible = true;
        private int _LegendWidth = 110;
        private bool _PlotSemiLog = false;
        private int _XAxisScaleFactor = 1;
        private float _XMax = 30;
        private float _XMin = 0;
        private float _YMax = 100;
        private float _YMin = 0;
        private string _HeaderLeft = "";
        private string _HeaderRight = "";
        private int _YMaxExp = -5;
        private int _YMinExp = -13;

        #endregion Private Fields

        #endregion Fields

        #region Enums (1)

        /// <summary>
        /// Type of change that occurred
        /// </summary>
        public enum GraphSettingChangeType
        {
            HeaderLeft,
            HeaderRight,

            /// <summary>
            /// Indicates that the AutoScaleYMaxExp property changed
            /// </summary>
            AutoScaleYMaxExp,

            /// <summary>
            /// Indicates that the AutoScaleYMinExp property changed
            /// </summary>
            AutoScaleYMinExp,

            /// <summary>
            /// Indicates that the PlotSemiLog property changed
            /// </summary>
            PlotSemiLog,

            /// <summary>
            /// Indicates that the XAxisScaleFactor property changed
            /// </summary>
            XAxisScaleFactor,

            /// <summary>
            /// Indicates that the XMax property changed
            /// </summary>
            XMax,

            /// <summary>
            /// Indicates that the XMin property changed
            /// </summary>
            XMin,

            /// <summary>
            /// Indicates that the YMax property changed
            /// </summary>
            YMax,

            /// <summary>
            /// Indicates that the YMin property changed
            /// </summary>
            YMin,

            /// <summary>
            /// Indicates that the YMaxExp property changed
            /// </summary>
            YMaxExp,

            /// <summary>
            /// Indicates that the YMinExp property changed
            /// </summary>
            YMinExp,

            /// <summary>
            /// Indicates that the DrawPlotCursorCallouts property changed
            /// </summary>
            DrawPlotCursorCallouts,

            /// <summary>
            /// Indicates that the CommentsVisible property changed
            /// </summary>
            CommentsVisible,

            /// <summary>
            /// Indicates that the LegendEnabled property changed
            /// </summary>
            LegendEnabled,

            /// <summary>
            /// Indicates that the LegendVisible property changed
            /// </summary>
            LegendVisible,

            /// <summary>
            /// Indicates that the YAxisButtonsVisible property changed
            /// </summary>
            YAxisButtonsVisible,

            /// <summary>
            /// Indicates that the LegendWidth property changed
            /// </summary>
            LegendWidth
        }

        #endregion Enums

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphSettings">GraphSettings</see> class.
        /// </summary>
        /// <param name="sectionName">Section name in the user.config file to use for these settings</param>
        public GraphSettings(string sectionName)
          : base(sectionName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphSettings">GraphSettings</see> class.
        /// </summary>
        public GraphSettings()
          : base()
        {
        }

        #endregion Constructors

        #region Properties (15)

        /// <summary>
        /// Gets or sets a value to indicate if the Y-Axis should autoscale the Maximum Value
        /// </summary>
        public bool AutoScaleYMaxExp
        {
            get { return _AutoScaleYMaxExp; }
            set
            {
                _AutoScaleYMaxExp = value;
                OnChanged(GraphSettingChangeType.AutoScaleYMaxExp);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the Y-Axis should autoscale the Minimum Value
        /// </summary>
        public bool AutoScaleYMinExp
        {
            get { return _AutoScaleYMinExp; }
            set
            {
                _AutoScaleYMinExp = value;
                OnChanged(GraphSettingChangeType.AutoScaleYMinExp);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the comments should be visible
        /// </summary>
        public bool CommentsVisible
        {
            get { return _CommentsVisible; }
            set
            {
                _CommentsVisible = value;
                OnChanged(GraphSettingChangeType.CommentsVisible);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the "Plot Cursor" should have callouts for the values
        /// </summary>
        public bool DrawPlotCursorCallouts
        {
            get { return _DrawPlotCursorCallouts; }
            set
            {
                _DrawPlotCursorCallouts = value;
                OnChanged(GraphSettingChangeType.DrawPlotCursorCallouts);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the Legend should be enabled
        /// </summary>
        public bool LegendEnabled
        {
            get { return _LegendEnabled; }
            set
            {
                _LegendEnabled = value;
                OnChanged(GraphSettingChangeType.LegendEnabled);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the Legend should be visible
        /// </summary>
        public Boolean LegendVisible
        {
            get { return _LegendVisible; }
            set
            {
                _LegendVisible = value;

                OnChanged(GraphSettingChangeType.LegendVisible);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the Y-Axis buttons should be visible
        /// </summary>
        public Boolean YAxisButtonsVisible
        {
            get { return _YAxisButtonsVisible; }
            set
            {
                _YAxisButtonsVisible = value;

                OnChanged(GraphSettingChangeType.YAxisButtonsVisible);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate the width of the legend area
        /// </summary>
        public int LegendWidth
        {
            get { return _LegendWidth; }
            set
            {
                _LegendWidth = value;
                OnChanged(GraphSettingChangeType.LegendWidth);
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the DataPlot should plot data in log format.
        /// </summary>
        public bool PlotSemiLog
        {
            get { return _PlotSemiLog; }
            set
            {
                _PlotSemiLog = value;
                OnChanged(GraphSettingChangeType.PlotSemiLog);
            }
        }

        /// <summary>
        /// Gets or sets the scaling factor for the X-Axis
        /// </summary>
        public int XAxisScaleFactor
        {
            get { return _XAxisScaleFactor; }
            set
            {
                //if (value == 0) throw new Exception("X-Axis Scale Factor cannot be zero.");
                _XAxisScaleFactor = value;
                OnChanged(GraphSettingChangeType.XAxisScaleFactor);
            }
        }

        /// <summary>
        /// Gets or sets the Maximum Value for the X-Axis
        /// </summary>
        public float XMax
        {
            get { return _XMax; }
            set
            {
                if (_XMax != value)
                    _XMax = value;
                OnChanged(GraphSettingChangeType.XMax);
            }
        }

        /// <summary>
        /// Gets or sets the Minimum Value for the X-Axis
        /// </summary>
        public float XMin
        {
            get { return _XMin; }
            set
            {
                if (_XMin != value)
                    _XMin = value;
                OnChanged(GraphSettingChangeType.XMin);
            }
        }

        /// <summary>
        /// Gets or sets the Maximum Y-Axis value for plotting data in linear mode.
        /// </summary>
        public float YMax
        {
            get { return _YMax; }
            set
            {
                float newVal = value;
                if (newVal > 1E6)
                {
                    newVal = 1E6f;
                }
                else if (newVal < 1E-14)
                {
                    newVal = 1E-14f;
                }

                if (_YMax != newVal)
                {
                    _YMax = newVal;
                    OnChanged(GraphSettingChangeType.YMax);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Maximum Y-Axis value for plotting data in linear mode.
        /// </summary>
        public string HeaderLeft
        {
            get { return _HeaderLeft; }
            set
            {
                if (_HeaderLeft != value)
                    _HeaderLeft = value;
                OnChanged(GraphSettingChangeType.HeaderLeft);
            }
        }

        public string HeaderRight
        {
            get { return _HeaderRight; }
            set
            {
                if (_HeaderRight != value)
                    _HeaderRight = value;
                OnChanged(GraphSettingChangeType.HeaderRight);
            }
        }

        /// <summary>
        /// Gets or sets the Maximum Y-Axis Exponent for plotting data in log mode.
        /// </summary>
        public int YMaxExp
        {
            get { return _YMaxExp; }
            set
            {
                int newVal = value;
                if (newVal > 6)
                {
                    newVal = 6;
                }
                else if (newVal < -14)
                {
                    newVal = -14;
                }

                if (_YMaxExp != newVal)
                {
                    _YMaxExp = newVal;
                    OnChanged(GraphSettingChangeType.YMaxExp);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Minimum Y-Axis value for plotting data in linear mode.
        /// </summary>
        public float YMin
        {
            get { return _YMin; }
            set
            {
                float newVal = value;
                if (newVal > 1E6)
                {
                    newVal = 1E6f;
                }
                else if (newVal < 1E-14)
                {
                    newVal = 1E-14f;
                }

                if (_YMin != newVal)
                {
                    _YMin = newVal;
                    OnChanged(GraphSettingChangeType.YMin);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Minimum Y-Axis Exponent for plotting data in log mode.
        /// </summary>
        public int YMinExp
        {
            get { return _YMinExp; }
            set
            {
                int newVal = value;
                if (newVal > 6)
                {
                    newVal = 6;
                }
                else if (newVal < -14)
                {
                    newVal = -14;
                }

                if (_YMinExp != newVal)
                {
                    _YMinExp = newVal;
                    OnChanged(GraphSettingChangeType.YMinExp);
                }
            }
        }

        #endregion Properties

        #region Delegates and Events (1)

        #region Events (1)

        /// <summary>
        /// Occurs when one of the properties of the graph settings changes.
        /// </summary>
        public event EventHandler<GraphSettingChangeEventArgs> Changed;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (1)

        #region Protected Methods (1)

        /// <summary>
        /// Raises the <see cref="Changed">Changed</see> event.
        /// </summary>
        /// <param name="ChangeType"></param>
        protected virtual void OnChanged(GraphSettingChangeType ChangeType)
        {
            if (Changed != null)
                Changed(this, new GraphSettingChangeEventArgs(ChangeType));
        }

        #endregion Protected Methods

        #endregion Methods

        #region Nested Classes (1)

        /// <summary>
        /// An <see cref="EventArgs">EventArgs</see> class for the <see cref="Changed">Changed</see> event.
        /// </summary>
        public class GraphSettingChangeEventArgs : EventArgs
        {
            #region Constructors (1)

            /// <summary>
            /// Initializes a new instance of the <see cref="GraphSettingChangeEventArgs">GraphSettingChangeEventArgs</see>.
            /// </summary>
            /// <param name="changeType">Type of change that occurred.</param>
            public GraphSettingChangeEventArgs(GraphSettingChangeType changeType)
            {
                ChangeType = changeType;
            }

            #endregion Constructors

            #region Properties (1)

            /// <summary>
            /// Gets the type of change that occurred.
            /// </summary>
            public GraphSettingChangeType ChangeType { get; private set; }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}