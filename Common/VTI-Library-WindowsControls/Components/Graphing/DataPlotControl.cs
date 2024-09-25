using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Graphing;
using VTIWindowsControlLibrary.Classes.Graphing.DataPlot;
using VTIWindowsControlLibrary.Classes.Graphing.Util;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Components.Graphing
{
    /// <summary>
    /// A UserControl that represents a Data Plot
    /// </summary>
    public partial class DataPlotControl : UserControl
    {
        #region Fields (14)

        #region Private Fields (14)

        private bool _AllowClose = true;
        private bool _allowOpenFile = true;
        private bool _autoRun1Visible;
        private bool _autoRun2Visible;
        private bool _autoShowAllVisible;
        private bool _autoShowEndVisible;
        private string _Caption = "Data Plot";
        private string _filename;
        private BindingList<IDigitalIO> _monitoredIO;
        private string _plotName = string.Empty;
        private string _yAxisPrintLabel = string.Empty;
        private bool _showAllVisible;
        private bool _showEndVisible;
        private bool collectingData;

        //        private PlotPropForm frmPlotProp1;
        public PlotPropForm frmPlotProp1 { get; set; }

        private Stopwatch stopWatchCollectData = new Stopwatch();
        private DateTime dtPrevYMaxClickTime, dtPrevYMinClickTime;
        private int ndxOfPrevYMaxIncrDigit, ndxOfPrevYMinIncrDigit;
        private DataPlotDockControl _dataPlotDockControl;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        //internal object PlotDataLock = new object();
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlotControl">DataPlotControl</see>
        /// </summary>
        public DataPlotControl()
        {
            frmPlotProp1 = new PlotPropForm(this);
            //Settings = new DataPlotSettings();
            //Settings.SettingChanging += new System.Configuration.SettingChangingEventHandler(Settings_SettingChanging);
            //_CycleComments = new List<CycleComment>();

            InitializeComponent();

            //graphControl1.GraphSettingChanged += new EventHandler<DataPlotGraphControl.GraphSettingChangeEventArgs>(graphControl1_GraphSettingChanged);
            graphControl1.ShowProperties += new EventHandler(graphControl1_ShowProperties);
            graphControl1.FormatXAxisTicLabel = FormatXAxisTicLabel;
            graphControl1.FormatPlotCursorAxisLabel = FormatPlotCursorAxisLabel;
            graphControl1.GetDateTimeOfPoint = GetDateTimeOfPoint;
            graphControl1.Traces.Changed += new EventHandler<TraceCollectionChangedEventArgs<DataPlotTraceType, DataPointType>>(Traces_Changed);
            graphControl1.GraphTypeName = "Data Plot";
            Settings.Loaded += new EventHandler(Settings_Loaded);
            Settings.Upgraded += new EventHandler(Settings_Upgraded);

            graphControl1.GraphData.IOStates = new List<DataPlotIOState>();
            dtPrevYMaxClickTime = DateTime.MinValue;
            dtPrevYMinClickTime = DateTime.MinValue;
            ndxOfPrevYMaxIncrDigit = -1;
            ndxOfPrevYMinIncrDigit = -1;

            _monitoredIO = new BindingList<IDigitalIO>();
            _monitoredIO.AllowNew = true;
            _monitoredIO.AllowRemove = true;
            _monitoredIO.AllowEdit = false;
            _monitoredIO.RaiseListChangedEvents = true;
            _monitoredIO.ListChanged += new ListChangedEventHandler(_monitoredIO_ListChanged);
        }

        private void Settings_Upgraded(object sender, EventArgs e)
        {
            SetSettings();
        }

        private void Settings_Loaded(object sender, EventArgs e)
        {
            SetSettings();
        }

        #endregion Constructors

        #region Properties (18)

        /// <summary>
        /// Gets or sets a value to indicate if the Data Plot can be closed by the operator.
        /// </summary>
        public Boolean AllowClose
        {
            get { return _AllowClose; }
            set
            {
                _AllowClose = value;
                //toolStripButtonClose.Visible = value;
                //toolStripSeparatorLinLog.Visible = value;
                //closeWindowToolStripMenuItem.Visible = value;
                closeToolStripMenuItem.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether or not this instance of the Data Plot Control
        /// is allowed to open files.
        /// </summary>
        public bool AllowOpenFile
        {
            get { return _allowOpenFile; }
            set { _allowOpenFile = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the "Auto Run 1" button on the
        /// toolbar should be visible.
        /// </summary>
        public Boolean AutoRun1Visible
        {
            get { return _autoRun1Visible; }
            set
            {
                _autoRun1Visible = value;
                toolStripButtonAutoRun1.Visible = _autoRun1Visible;
                toolStripSeparatorAutoRun.Visible =
                    toolStripButtonAutoRun1.Visible ||
                    toolStripButtonAutoRun2.Visible;
                toggleAutoRunMenuItem.Checked = _autoRun1Visible;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the "Auto Run 2" button on the
        /// toolbar should be visible.
        /// </summary>
        public Boolean AutoRun2Visible
        {
            get { return _autoRun2Visible; }
            set
            {
                _autoRun2Visible = value;
                toolStripButtonAutoRun2.Visible = _autoRun2Visible;
                toolStripSeparatorAutoRun.Visible =
                    toolStripButtonAutoRun1.Visible ||
                    toolStripButtonAutoRun2.Visible;
                toggleAutoRunMenuItem.Checked = _autoRun1Visible && _autoRun2Visible;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the "Auto Show All" button on the
        /// toolbar should be visible.
        /// </summary>
        public Boolean AutoShowAllVisible
        {
            get { return _autoShowAllVisible; }
            set
            {
                _autoShowAllVisible = value;
                toolStripButtonAutoShowAll.Visible = _autoShowAllVisible;
                toolStripSeparatorShowAll.Visible =
                    toolStripButtonAutoShowAll.Visible ||
                    toolStripButtonShowAll.Visible;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the "Auto Show End" button on the
        /// toolbar should be visible.
        /// </summary>
        public Boolean AutoShowEndVisible
        {
            get { return _autoShowEndVisible; }
            set
            {
                _autoShowEndVisible = value;
                toolStripButtonAutoShowEnd.Visible = _autoShowEndVisible;
                toolStripSeparatorShowEnd.Visible =
                    toolStripButtonAutoShowEnd.Visible ||
                    toolStripButtonShowEnd.Visible;
            }
        }

        /// <summary>
        /// Gets or sets the caption for the Data Plot.
        /// </summary>
        public String Caption
        {
            get { return _Caption; }
            set { SetCaption(value); }
        }

        /// <summary>
        /// Gets a value indicating the total elapsed time in seconds for the Data Plot
        /// </summary>
        public double ElapsedTime
        {
            get { return stopWatchCollectData.Elapsed.TotalSeconds; }
        }

        internal DataPlotGraphControl GraphControl { get { return graphControl1; } }

        /// <summary>
        /// Prints the dataplot to default printer
        /// </summary>
        public void PrintGraph()
        {
            graphControl1.PrintGraph();
        }

        /// <summary>
        /// Gets a value to indicate if the Data Plot is currently running / collecting data
        /// </summary>
        public Boolean IsRunning
        {
            get { return this.timerCollectData.Enabled; }
        }

        /// <summary>
        /// Returns whether AutoRun1 is enabled
        ///  </summary>
        public Boolean IsAutoRun1Enabled
        {
            get { return this.toolStripButtonAutoRun1.Checked; }
        }

        /// <summary>
        /// Gets the list of Digital I/O that is automatically monitored and logged by the data plot.
        /// The valve states of the monitored I/O are logged and displayed via the plot cursor.
        /// </summary>
        /// <value>The monitored IO.</value>
        public BindingList<IDigitalIO> MonitoredIO
        {
            get { return _monitoredIO; }
        }

        /// <summary>
        /// Gets the y-Axis Print Label
        /// </summary>
        public string YAxisPrintLabel
        {
            get { return _yAxisPrintLabel; }
        }

        /// <summary>
        /// Gets or sets the name for the Data Plot.
        /// </summary>
        public String PlotName
        {
            get { return _plotName; }
            set
            {
                _plotName = value;
                graphControl1.GraphTypeName = _plotName;

                // Set the SectionName based on the PlotName
                if (!String.IsNullOrEmpty(_plotName))
                    Settings.SectionName =
                        string.Format("{0}_{1}.Settings", this.GetType().ToString(), _plotName.Replace(" ", "_"));
                else
                    Settings.SectionName = string.Format("{0}.Settings", this.GetType().ToString());

                // Reload the settings using the new SectionName
                Settings.Load();
                graphControl1.AssignTimeUnits();
                if (Settings.XAxisUnits == XAxisUnitsType.Minutes)
                {
                    Settings.XMin /= 60.0f;
                    Settings.XMax /= 60.0f;
                }

                if (Settings.PlotSemiLog)
                {
                    YMaxExpDn.Visible = false;
                    YMaxExpUp.Visible = false;
                    YMinExpDn.Visible = false;
                    YMinExpUp.Visible = false;
                }
                //// Check to see if these settings need to be upgraded
                //if (Settings.CallUpgrade)
                //{
                //    Settings.Upgrade();
                //    Settings.CallUpgrade = false;
                //    Settings.Save();
                //}

                //SetSettings();
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the Run and Stop buttons on the
        /// toolbar should be visible.
        /// </summary>
        public Boolean RunStopVisible
        {
            get { return toolStripButtonRun.Visible; }
            set
            {
                toolStripButtonRun.Visible =
                toolStripButtonStop.Visible =
                toolStripSeparatorRunStop.Visible =
                    value;
            }
        }

        /// <summary>
        /// Gets the settings for the data plot.
        /// </summary>
        public DataPlotSettings Settings
        {
            get { return graphControl1.Settings; }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the "Show All" button on the
        /// toolbar should be visible.
        /// </summary>
        public Boolean ShowAllVisible
        {
            get { return _showAllVisible; }
            set
            {
                _showAllVisible = value;
                toolStripButtonShowAll.Visible = _showAllVisible;
                toolStripSeparatorShowAll.Visible =
                    toolStripButtonAutoShowAll.Visible ||
                    toolStripButtonShowAll.Visible;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the "Show End" button on the
        /// toolbar should be visible.
        /// </summary>
        public Boolean ShowEndVisible
        {
            get { return _showEndVisible; }
            set
            {
                _showEndVisible = value;
                toolStripButtonShowEnd.Visible = _showEndVisible;
                toolStripSeparatorShowEnd.Visible =
                    toolStripButtonAutoShowEnd.Visible ||
                    toolStripButtonShowEnd.Visible;
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if a status strip at the bottom
        /// of the Data Plot should be visible.
        /// </summary>
        public Boolean StatusStripVisible
        {
            get { return statusStrip1.Visible; }
            set { statusStrip1.Visible = value; }
        }

        /// <summary>
        /// Collection of Traces in the Data Plot
        /// </summary>
        public DataPlotTraceCollection Traces
        {
            get
            {
                return this.graphControl1.Traces;
            }
        }

        //public ToolStripMenuItem Menu
        //{
        //    get { return dataPlotToolStripMenuItem; }
        //}

        private bool _localDropDownMenus = false;

        /// <summary>
        /// Gets or sets a value indicating whether the data plot menus should
        /// be local drop down menus, or that the menus should be merged with the parent
        /// form's menus.
        /// </summary>
        /// <value><c>true</c> if the data plot menus should
        /// be local drop down menus; otherwise, <c>false</c>.</value>
        public bool LocalDropDownMenus
        {
            get { return _localDropDownMenus; }
            set
            {
                _localDropDownMenus = value;
                if (_localDropDownMenus)
                {
                    fileToolStripMenuItem.Visible = false;
                    dataPlotToolStripMenuItem.Visible = false;

                    dataPlotToolStripDropDownButton.Visible = true;
                    if (dataPlotToolStripDropDownButton.DropDownItems.Count == 0)
                    {
                        dataPlotToolStripDropDownButton.DropDownItems.AddRange(new ToolStripItem[]
                                    {
                            this.overlayToolStripMenuItem,
                            this.saveToolStripMenuItem,
                            this.saveAsToolStripMenuItem,
                            this.toolStripSeparator10,
                            this.pageSetupToolStripMenuItem1,
                            this.printToolStripMenuItem1,
                            this.toolStripSeparator11,
                            this.toggleAutoRunMenuItem,
                            //this.toggleTimeUnitsMenuItem,
                            this.assignYAxisPrintLabelMenuItem,
                            this.propertiesToolStripMenuItem,
                            this.toolStripSeparatorClose,
                            this.closeToolStripMenuItem
                                    });
                    }
                    toolStripSeparatorMenus.Visible = true;
                }
                else
                {
                    fileToolStripMenuItem.Visible = true;
                    dataPlotToolStripMenuItem.Visible = true;

                    dataPlotToolStripDropDownButton.Visible = false;
                    toolStripSeparatorMenus.Visible = false;
                }
            }
        }

        #endregion Properties

        #region Delegates and Events (2)

        #region Events (2)

        ///// <summary>
        ///// Gets or sets the <see cref="SystemSignalsControl">SystemSignalsControl</see> associated with this Data Plot
        ///// </summary>
        //public SystemSignalsControl SystemSignals
        //{
        //    get { return _SystemSignals; }
        //    set { _SystemSignals = value; }
        //}
        /// <summary>
        /// Occurs when the caption of the Data Plot changes
        /// </summary>
        public event EventHandler CaptionChanged;

        /// <summary>
        /// Occurs when the Close tool strip button or Close Window menu item are clicked
        /// </summary>
        public event EventHandler Close;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (69)

        #region Public Methods (13)

        /// <summary>
        /// Adds a comment to the graph.
        /// </summary>
        /// <param name="text">Text of the comment</param>
        /// <param name="plotTime">X coordinate of the location of the comment</param>
        /// <param name="signalValue">Y coordinate of the location of the comment</param>
        /// <param name="offset">Offset from the location of the comment to the text box</param>
        /// <returns>The comment that was added.</returns>
        public void AddComment(String text, float plotTime, float signalValue, Point offset)
        {
            graphControl1.AddComment(text, plotTime, signalValue, offset);
        }

        /// <summary>
        /// Adds a comment to the graph
        /// </summary>
        /// <param name="text">Text of the comment</param>
        /// <param name="location">Location of the comment</param>
        /// <param name="offset">Offset from the location of the comment to the text box</param>
        /// <returns>The comment that was added.</returns>
        public void AddComment(String text, PointF location, Point offset)
        {
            graphControl1.AddComment(Text, location, offset);
        }

        /// <summary>
        /// Adds a comment to the graph
        /// </summary>
        /// <param name="text">Text of the comment</param>
        /// <param name="location">Location of the comment</param>
        /// <param name="offset">Offset from the location of the comment to the text box</param>
        /// <param name="visible">Value to indicate whether or not the comment should be visible</param>
        /// <returns>The comment that was added.</returns>
        public void AddComment(String text, PointF location, Point offset, bool visible)
        {
            graphControl1.AddComment(text, location, offset, visible);
        }

        /// <summary>
        /// Adds a comment to the Data Plot at the given sigal value
        /// </summary>
        /// <param name="Text">Text of the comment</param>
        /// <param name="Signal">Signal value for the comment</param>
        public void AddComment(String Text, AnalogSignal Signal)
        {
            AddComment(Text, Traces[Signal.Key].Points.Last().X, (float)Signal.Value, new Point(50, -100));
        }

        /// <summary>
        /// Adds a comment to the "Cycle Comment" column in the data plot AIO file.  These comments
        /// are not visible on-screen, but are available for analysis when viewing AIO files in Excel, for example.
        /// </summary>
        /// <param name="Text">Comment to be added</param>
        public void AddCycleComment(string Text)
        {
            if (Traces.Count == 0 || Traces[0].Points.Count == 0 || !IsRunning) return;

            graphControl1.GraphData.CycleComments
                .Add(new DataPlotCycleComment
                {
                    Time = Traces[0].Points.Last().Time,
                    Comment = Text
                });
        }

        /// <summary>
        /// Removes all comments from the Data Plot
        /// </summary>
        public void ClearComments()
        {
            graphControl1.ClearComments();
        }

        /// <summary>
        /// Opens the specified Data Plot file. The file can be in the
        /// compressed-XML .aiox file format, or the older .aio format.
        /// </summary>
        /// <param name="filename">Name of the file to open</param>
        public bool Open(string filename)
        {
            if (!_allowOpenFile) return false;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                _filename = filename;

                //this.Stop();    // make sure dataplot isn't collecting data

                this.SuspendLayout();

                //if (this.RecalledScan)
                //{
                //    Traces.Clear();
                //}
                //else
                //{
                //    savedTraces = new KeyedTraceCollection<DataPlotTraceType, DataPointType>();
                //    Traces.ForEach(T => savedTraces.Add(T));
                //    Traces.Clear();
                //}

                //this.RecalledScan = true;
                //this.RecalledFileName = filename;

                //ClearComments();

                switch (Path.GetExtension(filename).ToLower())
                {
                    case ".aiox":
                        graphControl1.Open(filename);
                        graphControl1.Traces.Changed += new EventHandler<TraceCollectionChangedEventArgs<DataPlotTraceType, DataPointType>>(Traces_Changed);
                        SetSettings();
                        break;

                    case ".aio":
                        graphControl1.Traces.Changed -= new EventHandler<TraceCollectionChangedEventArgs<DataPlotTraceType, DataPointType>>(Traces_Changed);
                        LoadAioFile(filename);
                        graphControl1.Traces.Changed += new EventHandler<TraceCollectionChangedEventArgs<DataPlotTraceType, DataPointType>>(Traces_Changed);
                        break;
                }

                SetCaption();

                // Hide "Live" menus for recalled traces
                toolStripButtonRun.Visible = false;
                toolStripButtonStop.Visible = false;
                toolStripSeparatorRunStop.Visible = false;
                toolStripButtonAutoRun1.Visible = false;
                toolStripButtonAutoRun2.Visible = false;
                toolStripSeparatorAutoRun.Visible = false;
                toolStripButtonAutoShowAll.Visible = false;
                toolStripSeparatorShowAll.Visible = false;
                toolStripButtonAutoShowEnd.Visible = false;

                runToolStripMenuItem.Visible = false;
                stopToolStripMenuItem.Visible = false;
                toolStripSeparatorRunStopMenu.Visible = false;
                autoRun1ToolStripMenuItem.Visible = false;
                autoRun2ToolStripMenuItem.Visible = false;
                toolStripSeparatorAutoRunMenu.Visible = false;
                autoShowAllToolStripMenuItem.Visible = false;
                toolStripSeparatorShowAllMenu.Visible = false;
                autoShowEndToolStripMenuItem.Visible = false;

                // Add overlay menus
                foreach (var trace in Traces)
                    if (trace.IsOverlay) AddRemoveOverlayMenuItem(trace);
                    else AddOverlayMenuItem(trace);

                this.ResumeLayout();

                this.Cursor = Cursors.Default;

                graphControl1.ReDrawGraph();

                return true;
            }
            catch (Exception e)
            {
                this.Cursor = Cursors.Default;

                VtiEvent.Log.WriteWarning(
                    string.Format("An error occurred attempting to open '{0}'.", filename),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());

                MessageBox.Show(
                    string.Format("An error occurred attempting to open '{0}'.", filename), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }
        }

        /// <summary>
        /// Saves the data from the current graph, including the traces, overlays, and settings,
        /// to the specified file. The file can be in the
        /// compressed-XML .aiox file format, or the older .aio format.
        /// </summary>
        /// <param name="filename">Name of the file to be saved.</param>
        public void Save(string filename)
        {
            _filename = filename;

            //timerCollectData.Interval = 60000; // effectively pauses data collection during file save
            switch (Path.GetExtension(filename).ToLower())
            {
                case ".aiox":
                    graphControl1.Save(filename);
                    break;

                case ".aio":
                    SaveDataPlotFile(filename, ".aio");
                    break;
                case ".csv":
                    SaveDataPlotFile(filename, ".csv");
                    break;
            }
            //timerCollectData.Interval = Convert.ToInt32(Settings.DataCollectionInterval * 1000);

            SetCaption();
        }

        /// <summary>
        /// Sets the caption of the Data Plot
        /// </summary>
        /// <param name="caption"></param>
        public void SetCaption(string caption)
        {
            if (this.InvokeRequired)
            {
                Action<string> d = new Action<string>(SetCaption);
                this.Invoke(d, caption);
            }
            else
            {
                toolStripStatusLabelCaption.Text = caption;
                _Caption = caption;
                OnCaptionChanged();
            }
        }

        /// <summary>
        /// Sets the caption for the Data Plot
        /// </summary>
        public void SetCaption()
        {
            String sTemp, sCaption, sNumPts = "";
            if (Traces.Count > 0 && Traces.First().Points.Count > 0)
            {
                sNumPts = ", " + Traces[0].Points.Count.ToString() + " pts";
                if (Settings.XAxisUnits == XAxisUnitsType.Seconds)
                    sTemp = String.Format("{0:00} seconds", Math.Round(Traces.First().Points.Last().X, 1));
                else
                    sTemp = String.Format("{0:0.00} minutes", Math.Round(Traces.First().Points.Last().X / (Double)Settings.XAxisUnits, 1));
            }
            else
                sTemp = string.Empty;
            if (!string.IsNullOrEmpty(_filename))
            {
                if (!string.IsNullOrEmpty(this._plotName))
                    sCaption = String.Format("Data Plot ({0}): {1}", _plotName, Path.GetFileName(_filename));
                else
                    sCaption = String.Format("Data Plot: {0}", Path.GetFileName(_filename));
            }
            else
            {
                if (_plotName != null && _plotName != string.Empty)
                {
                    sCaption = "Data Plot (" + _plotName + ")";
                }
                else
                {
                    sCaption = "Data Plot";
                }
                if (graphControl1.ShowingPlotCursor)
                {
                    if (Settings.XAxisUnits == XAxisUnitsType.Seconds)
                        sTemp = String.Format("{0:0.00} seconds", graphControl1.PlotCursorLocation);
                    else
                        sTemp = String.Format("{0:0.00} minutes", graphControl1.PlotCursorLocation / (Single)Settings.XAxisUnits);
                }
                /*
                        else
                        {
                          if (Traces.First().Points.Count > 0)
                          {
                            if (Settings.XAxisUnits == XAxisUnitsType.Seconds)
                              sTemp = String.Format("{0:00} seconds", Math.Round(Traces.First().Points.Last().X, 1));
                            else
                              sTemp = String.Format("{0:0.00} minutes", Math.Round(Traces.First().Points.Last().X / (Double)Settings.XAxisUnits, 1));
                          }
                          else
                            sTemp = string.Empty;
                        }
                */

                if (!string.IsNullOrEmpty(sTemp)) sCaption += ": " + sTemp;
                sCaption += sNumPts;
            }
            if (_Caption != sCaption) this.SetCaption(sCaption);
        }

        /// <summary>
        /// Shows the Properties window
        /// </summary>
        public void ShowProperties()
        {
            frmPlotProp1.ShowDialog(this);
            //ReDrawDataPlot();
            graphControl1.ReDrawGraph();
        }

        /// <summary>
        /// Starts the Data Plot running and collecting data
        /// </summary>
        public void Start()
        {
            if (this.InvokeRequired)
            {
                Action d = new Action(Start);
                this.BeginInvoke(d);
            }
            else
            {
                _filename = string.Empty;
                //Settings.XAxisUnits = XAxisUnitsType.Seconds;
                SetCaption();

                toolStripButtonRun.Checked = true;
                toolStripButtonStop.Checked = false;
                foreach (var trace in Traces.Where(T => !T.IsOverlay))
                    trace.Points.Clear();
                graphControl1.GraphData.CycleComments.Clear();
                //graphControl1.ClearComments();
                // remove any residual comments left over from last time this method was called
                graphControl1.GraphData.Comments.Where(T => !T.IsOverlay && T.Location.X == -1e10f).ForEach(T => graphControl1.GraphData.Comments.Remove(T));
                foreach (var graphComment in graphControl1.GraphData.Comments)
                {
                    if (!graphComment.IsOverlay)
                    {
                        PointF pf = new PointF(-1e10f, graphComment.Location.Y);
                        graphComment.Location = pf;
                    }
                }

                graphControl1.GraphData.IOStates.Clear();

                // Set initial state for monitored IO
                foreach (var digitalIO in _monitoredIO)
                {
                    graphControl1.GraphData.IOStates.Add(
                    new DataPlotIOState
                    {
                        Time = 0,
                        Name = digitalIO.Name,
                        Description = digitalIO.Description,
                        Enabled = digitalIO.Value
                    });
                }

                //if (Traces.Count > 0)
                //{
                this.stopWatchCollectData.Reset();
                this.stopWatchCollectData.Start();
                //if (Settings.DataCollectionInterval == 0)
                //{
                //    bCollectAllData = true;
                //    this.timerCollectData.Enabled = true;
                //    this.timerCollectData.Interval = 100;
                //    // trigger off the first analog signal only...
                //    //Traces[0].AnalogSignal.ValueChanged += new EventHandler(AnalogSignal_ValueChanged);
                //    Traces
                //        .Where(T => !T.IsOverlay && T.AnalogSignal != null)
                //        .ForEach(T =>
                //        {
                //            T.AnalogSignal.ValueChanged += new EventHandler(AnalogSignal_ValueChanged);
                //        });
                //}
                //else
                //{
                //    bCollectAllData = false;
                //    this.timerCollectData.Enabled = true;
                //    this.timerCollectData.Interval = Convert.ToInt32(Settings.DataCollectionInterval * 1000);
                //    // remove the analog signal trigger...
                //    //Traces[0].AnalogSignal.ValueChanged -= new EventHandler(AnalogSignal_ValueChanged);
                //    Traces
                //        .Where(T => !T.IsOverlay && T.AnalogSignal != null)
                //        .ForEach(T =>
                //        {
                //            T.AnalogSignal.ValueChanged -= new EventHandler(AnalogSignal_ValueChanged);
                //        });
                //}
                InitializeDataCollection();
                if (Settings.DataCollectionInterval == 0)
                {
                    timerCollectData.Enabled = false;
                }
                else
                {
                    timerCollectData.Enabled = true;
                    timerCollectData.Interval = Convert.ToInt32(Settings.DataCollectionInterval * 1000);
                }
                //}

                collectingData = true;

                graphControl1.ReDrawGraph();
            }
        }

        /// <summary>
        /// Stops the Data Plot from running and collecting data
        /// </summary>
        public void Stop()
        {
            if (this.InvokeRequired)
            {
                Action d = new Action(Stop);
                this.BeginInvoke(d);
            }
            else
            {
                toolStripButtonStop.Checked = true;
                toolStripButtonRun.Checked = false;
                this.stopWatchCollectData.Stop();
                this.timerCollectData.Enabled = false;
                collectingData = false;
            }
        }

        /// <summary>
        /// Default settings assigned to Data Plot at Cycle Start
        /// </summary>
        public void AssignDefaultCycleStartDataPlotSettings()
        {
            Settings.XAxisUnits = XAxisUnitsType.Seconds;
            Settings.XMin = 0;
            Settings.XMax = 30;
        }

        /// <summary>
        /// Assign Data Plot Dock Control
        /// </summary>
        ///
        public void AssignDataPlotDockControl(DataPlotDockControl dc)
        {
            _dataPlotDockControl = dc;
        }

        #endregion Public Methods
        #region Protected Methods (2)

        /// <summary>
        /// Raises the <see cref="CaptionChanged">CaptionChanged</see> event
        /// </summary>
        protected virtual void OnCaptionChanged()
        {
            if (CaptionChanged != null) CaptionChanged(this, null);
        }

        /// <summary>
        /// Raises the <see cref="Close">Close</see> event
        /// </summary>
        protected virtual void OnClose()
        {
            if (Close != null) Close(this, null);
        }

        #endregion Protected Methods
        #region Private Methods (53)

        private void _monitoredIO_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                _monitoredIO[e.NewIndex].ValueChanged += new EventHandler(MonitoredIO_ValueChanged);
            }
        }

        private void AddDataPoint()
        {
            foreach (var trace in Traces)
            {
                lock (((IList)trace.Points).SyncRoot)
                {
                    try
                    {
                        if (trace.AnalogSignal == null)
                            continue;
                        if (Settings.DecimationThreshold != "0")
                        {
                            if (Settings.AutoSave)
                            { // save data currently contained in DataPlot, then stop and restart the DataPlot
                                int nAutoSaveThrhld = Convert.ToInt32(Settings.DecimationThreshold);
                                if (trace.Points.Count > nAutoSaveThrhld)
                                {
                                    // make sure C:\DataPlot folder already exists
                                    string strDataPlotFolder = @"C:\DataPlot\";
                                    // create the DataPlot folder if it does not already exist
                                    if (!Directory.Exists(strDataPlotFolder))
                                    {
                                        Directory.CreateDirectory(strDataPlotFolder);
                                        DirectoryInfo di = new DirectoryInfo(strDataPlotFolder);
                                        di.Attributes = di.Attributes & ~FileAttributes.ReadOnly;
                                    }
                                    // stop the dataplot
                                    this.Stop();
                                    // save AIO file
                                    string strDataPlotFile = strDataPlotFolder + @"DataPlot" + string.Format("{0:yyyy-MM-dd HH-mm-ss}", DateTime.Now) + ".aio";
                                    SaveDataPlotFile(strDataPlotFile, ".aio");
                                    // start the dataplot
                                    this.Start();
                                }
                            }
                            else
                            {
                                int nDecThrhld = Convert.ToInt32(Settings.DecimationThreshold);
                                if (trace.Points.Count > nDecThrhld)
                                {
                                    Settings.IsDecimatingData = true;
                                    // throw half of the data away (every other data point)
                                    int ii, nNumPtsToKeep = trace.Points.Count / 2;
                                    for (ii = 0; ii < nNumPtsToKeep; ii++)
                                    {
                                        trace.Points.ElementAt(ii).X = trace.Points.ElementAt(2 * ii).X;
                                        trace.Points.ElementAt(ii).Y = trace.Points.ElementAt(2 * ii).Y;
                                    }
                                    trace.Points.RemoveRange(ii, trace.Points.Count - ii);
                                }
                            }
                        }
                        //if (trace.Points.Any(x => x.Y == null || x.Y == 0))
                        //{

                        //}
                        trace.Points.Add(
                          new DataPointType(stopWatchCollectData.ElapsedMilliseconds / 1000F, trace.Value, DateTime.Now));
                    }
                    catch (Exception ex)
                    {
                        // exceeded the maximum number of trace points
                        trace.Points.Clear();
                        trace.Points.Add(
                          new DataPointType(stopWatchCollectData.ElapsedMilliseconds / 1000F, trace.Value, DateTime.Now));
                    }
                }
            }

            CheckForGraphRescaling();
            Settings.IsDecimatingData = false;
        }

        private void CheckForGraphRescaling()
        {
            if (Settings.AutoShowAll && !graphControl1.ShowingPlotCursor)
            {
                if (Traces.Any(T => T.Points.Count > 0 &&
                      (T.Points.Last().X > Settings.XMax ||
                       T.Points.Last().X < Settings.XMin)))
                    graphControl1.ShowAllOfGraph();
                else if (Settings.IsDecimatingData)
                    graphControl1.ShowAllOfGraph();
            }
            if (Settings.AutoShowEnd && !graphControl1.ShowingPlotCursor)
            {
                if (Traces.Any(T => T.Points.Count > 0 &&
                      (T.Points.Last().X > Settings.XMax ||
                       T.Points.Last().X < Settings.XMin)))
                    graphControl1.ShowEndOfGraph(Settings.XWindow, (float)Settings.XWindowShiftPercent);
                else if (Settings.IsDecimatingData)
                    graphControl1.ShowEndOfGraph(Settings.XWindow, (float)Settings.XWindowShiftPercent);
            }
        }

        private void AddOverlayMenuItem(DataPlotTraceType trace)
        {
            ToolStripMenuItem overlayMenuItem = new ToolStripMenuItem();

            overlayMenuItem.Name = string.Format("{0}AddMenuStripItem", trace.Key);
            overlayMenuItem.Size = new System.Drawing.Size(152, 22);
            overlayMenuItem.Text = string.Format("{0} ({1})",
                trace.Label, trace.Color.Name);
            overlayMenuItem.Click += new EventHandler(addOverlayMenuItem_Click);
            overlayMenuItem.Tag = trace.Key;

            addOverlayToolStripMenuItem.DropDownItems.Add(overlayMenuItem);
        }

        private void addOverlayMenuItem_Click(object sender, EventArgs e)
        {
            graphControl1.OverlayTrace((sender as ToolStripMenuItem).Tag as string);
        }

        private void AddRemoveOverlayMenuItem(DataPlotTraceType trace)
        {
            ToolStripMenuItem removeOverlayMenuItem = new ToolStripMenuItem();

            removeOverlayMenuItem.Name = string.Format("{0}RemoveMenuStripItem", trace.Key);
            removeOverlayMenuItem.Size = new System.Drawing.Size(152, 22);
            removeOverlayMenuItem.Text = string.Format("{0} ({1})",
                trace.Label, trace.Color.Name);
            removeOverlayMenuItem.Click += new EventHandler(removeOverlayMenuItem_Click);
            removeOverlayMenuItem.Tag = trace.Key;

            clearOverlayToolStripMenuItem.DropDownItems.Add(removeOverlayMenuItem);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphControl1.RemoveAllOverlays();
        }

        private void allTracesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphControl1.OverlayAllTraces();
        }

        //void AnalogSignal_ValueChanged(object sender, EventArgs e)
        //{
        //    //if (bCollectAllData)
        //    //{
        //    //    AddDataPoint();
        //    //}
        //    AnalogSignal signal = sender as AnalogSignal;
        //    Traces.First(T => T.AnalogSignal == signal)
        //        .Points.Add(
        //            new DataPointType(stopWatchCollectData.ElapsedMilliseconds / 1000F, (float)signal.Value, DateTime.Now));
        //}
        private void autoRun1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.AutoRun1 = autoRun1ToolStripMenuItem.Checked;
            Settings.Save();
            SetSettings();
        }

        private void autoRun2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.AutoRun2 = autoRun2ToolStripMenuItem.Checked;
            Settings.Save();
            SetSettings();
        }

        private void autoShowAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.AutoShowAll = autoShowAllToolStripMenuItem.Checked;
            if (Settings.AutoShowAll)
            {
                Settings.AutoShowEnd = false;
                ShowAllOfDataPlot();
            }
            Settings.Save();
            SetSettings();
        }

        private void autoShowEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.AutoShowEnd = autoShowEndToolStripMenuItem.Checked;
            if (Settings.AutoShowEnd)
            {
                Settings.AutoShowAll = false;
                ShowEndOfDataPlot();
            }
            Settings.Save();
            SetSettings();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnClose();
        }

        private void closeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Hide();
            OnClose();
        }

        private DateTime GetDateTimeOfPoint(float PlotTime)
        {
            DateTime plotDateTime =
                Traces[0].Points
                    .Select(
                        P => new
                        {
                            distance = Math.Abs(P.X - PlotTime),
                            DateTime = P.DateTime
                        })
                       .OrderBy(P => P.distance)
                       .First().DateTime;

            return plotDateTime;
        }

        private string FormatPlotCursorAxisLabel(float PlotTime)
        {
            if (Settings.XAxisDisplayUnits == XAxisDisplayUnitsType.SecondsMinutes)
                return string.Format("{0:0.00}", PlotTime / (float)Settings.XAxisUnits);
            else
            {
                DateTime plotDateTime =
                    Traces[0].Points
                        .Select(
                            P => new
                            {
                                distance = Math.Abs(P.X - PlotTime),
                                DateTime = P.DateTime
                            })
                           .OrderBy(P => P.distance)
                           .First().DateTime;

                switch (Settings.XAxisDisplayUnits)
                {
                    case XAxisDisplayUnitsType.Time:
                        return string.Format("{0:h:mm:ss tt}", plotDateTime);

                    case XAxisDisplayUnitsType.Date:
                        return string.Format("{0:M/d/yy}", plotDateTime);

                    case XAxisDisplayUnitsType.DateTime:
                        return string.Format("{0:h:mm:ss tt}{1}{2:M/d/yy}",
                            plotDateTime, Environment.NewLine, plotDateTime);

                    default:
                        return string.Empty;
                }
            }
        }

        private string FormatXAxisTicLabel(float TicValue, float TicLabelInterval)
        { // tas 9/25/12 LOCKED IN TIC
            try
            {
                if (Settings.XAxisDisplayUnits == XAxisDisplayUnitsType.SecondsMinutes)
                    return string.Format("{0:0}", Math.Floor(TicValue / TicLabelInterval) * TicLabelInterval);
                else
                {
                    DateTime plotDateTime =
                        Traces[0].Points
                            .Select(
                                P => new
                                {
                                    distance = Math.Abs(P.X - TicValue * (float)Settings.XAxisUnits),
                                    DateTime = P.DateTime
                                })
                               .OrderBy(P => P.distance)
                               .First().DateTime;

                    switch (Settings.XAxisDisplayUnits)
                    {
                        case XAxisDisplayUnitsType.Time:
                            return string.Format("{0:h:mm:ss tt}", plotDateTime);

                        case XAxisDisplayUnitsType.Date:
                            return string.Format("{0:M/d/yy}", plotDateTime);

                        case XAxisDisplayUnitsType.DateTime:
                            return string.Format("{0:h:mm:ss tt}{1}{2:M/d/yy}",
                                plotDateTime, Environment.NewLine, plotDateTime);

                        default:
                            return string.Empty;
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                graphControl1.OverlayFile(openFileDialog1.FileName);
            }
        }

        //void graphControl1_GraphSettingChanged(object sender, DataPlotGraphControl.GraphSettingChangeEventArgs e)
        //{
        //    switch (e.ChangeType)
        //    {
        //        case DataPlotGraphControl.GraphSettingChangeType.AutoScaleYMaxExp:
        //            Settings.AutoScaleMax = graphControl1.AutoScaleYMaxExp;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.AutoScaleYMinExp:
        //            Settings.AutoScaleMin = graphControl1.AutoScaleYMinExp;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.CommentsVisible:
        //            Settings.CommentsVisible = graphControl1.CommentsVisible;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.LegendVisible:
        //            Settings.LegendVisible = graphControl1.LegendVisible;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.LegendWidth:
        //            Settings.LegendWidth = graphControl1.LegendWidth;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.PlotSemiLog:
        //            Settings.PlotSemiLog = graphControl1.PlotSemiLog;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.XMax:
        //            Settings.XMax = graphControl1.XMax;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.XMin:
        //            Settings.XMin = graphControl1.XMin;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.YMax:
        //            Settings.YMax = graphControl1.YMax;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.YMaxExp:
        //            Settings.YMaxExp = graphControl1.YMaxExp;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.YMin:
        //            Settings.YMin = graphControl1.YMin;
        //            break;
        //        case DataPlotGraphControl.GraphSettingChangeType.YMinExp:
        //            Settings.YMinExp = graphControl1.YMinExp;
        //            break;
        //    }

        //    Settings.Save();
        //}
        private void graphControl1_ShowProperties(object sender, EventArgs e)
        {
            ShowProperties();
        }

        private void InitializeDataCollection()
        {
            if (Settings.DataCollectionInterval == 0)
            {
                //bCollectAllData = true;
                //this.timerCollectData.Enabled = true;
                //this.timerCollectData.Interval = 100;
                // trigger off the first analog signal only...
                //Traces[0].AnalogSignal.ValueChanged += new EventHandler(AnalogSignal_ValueChanged);
                foreach (var trace in Traces.Where(T => !T.IsOverlay))
                    trace.ValueChanged += new EventHandler(Trace_ValueChanged);
            }
            else
            {
                //bCollectAllData = false;
                //this.timerCollectData.Enabled = true;
                this.timerCollectData.Interval = Convert.ToInt32(Settings.DataCollectionInterval * 1000);
                // remove the analog signal trigger...
                //Traces[0].AnalogSignal.ValueChanged -= new EventHandler(AnalogSignal_ValueChanged);
                foreach (var trace in Traces.Where(T => !T.IsOverlay))
                    trace.ValueChanged -= new EventHandler(Trace_ValueChanged);
            }
        }

        /// <summary>
        /// Load from AIO file into DataPlot
        /// </summary>
        /// <param name="filename"></param>
        public void LoadAioFile(string filename)
        {
            string sLine;
            string[] sData;
            string sHeader;
            int iNumTraces = 0, i, iDateTimeCol = 0;
            int iCycleCommentCol = 0, iCommentCol = 0;
            StreamReader sr = null;
            float time;
            DateTime plotDateTime;

            try
            {
                graphControl1.Filename = filename;
                Traces.Where(T => T.IsOverlay).ForEach(T => Traces.Remove(T));
                //Traces.Clear();
                ClearComments();
                int nPrevNumTraces = Traces.Count;
                sr = new StreamReader(filename);

                // Read the header line for the data
                sLine = sr.ReadLine();
                sData = sLine.Split('\t');

                // Count the number of traces
                for (i = 0; i < sData.Length; i++)
                {
                    if (sData[i].Replace('_', ' ').Equals("Date Time", StringComparison.CurrentCultureIgnoreCase))
                    {
                        iNumTraces = i - 1;
                        iDateTimeCol = i;
                    }
                    if (sData[i].Equals("Cycle Comments", StringComparison.CurrentCultureIgnoreCase))
                    {
                        iCycleCommentCol = i;
                    }
                }
                if (iCycleCommentCol != 0) iCommentCol = iCycleCommentCol + 1;
                else if (iDateTimeCol != 0) iCommentCol = iDateTimeCol + 1;

                if (iDateTimeCol == 0) iNumTraces = sData.Length - 1;   // old AIO files might not have a Date Time column

                // Get the X-Axis display units
                this.Settings.XAxisUnits = sData[0].ToLower().Contains("sec") ? XAxisUnitsType.Seconds : XAxisUnitsType.Minutes;

                sHeader = sLine; // save the header line in case we need it later
                                 // Create the Traces
                for (i = 1; i <= iNumTraces; i++)
                {
                    string traceName = sData[i].Replace(" ", "");
                    if (Traces.Contains(traceName))
                    {
                        int ii;
                        for (ii = 1; Traces.Contains(string.Format("{0}{1}", traceName, ii)); ii++) ;
                        traceName = string.Format("{0}{1}", traceName, ii);
                    }
                    this.Traces.Add(new DataPlotTraceType(traceName, sData[i], true));
                }

                // Read the data
                sLine = sr.ReadLine();
                while (!(sr.EndOfStream || sLine.StartsWith("Trace")))
                {
                    sData = sLine.Split('\t');
                    time = float.Parse(sData[0]) * (float)this.Settings.XAxisUnits;
                    if (iDateTimeCol != 0)
                        plotDateTime = Convert.ToDateTime(sData[iDateTimeCol]);
                    else
                        plotDateTime = DateTime.Now;

                    for (i = 0; i < iNumTraces; i++)
                        Traces[i + nPrevNumTraces].Points.Add(new DataPointType(time, float.Parse(sData[i + 1]), plotDateTime));

                    if (iCycleCommentCol != 0 && !string.IsNullOrEmpty(sData[iCycleCommentCol]))
                    {
                        sData[iCycleCommentCol].Split(',').ForEach(s =>
                            graphControl1.GraphData.CycleComments
                                .Add(new DataPlotCycleComment
                                {
                                    Time = time,
                                    Comment = s.Trim()
                                })
                            );
                    }

                    if ((iCommentCol != 0) && (sData.Length > iCommentCol))
                    {
                        // new style AIO file with tabs between comment parts
                        if (sData.Length > iCommentCol + 1)
                        {
                            AddComment(
                                sData[iCommentCol].Replace("\\n", "\n").Replace("\\r", "\r"),
                                new PointF(
                                    Convert.ToSingle(sData[0]),
                                    Convert.ToSingle(sData[iCommentCol + 1])),
                                new Point(
                                    Convert.ToInt32(sData[iCommentCol + 2]),
                                    Convert.ToInt32(sData[iCommentCol + 3])),
                                (sData.Length > iCommentCol + 4 ?
                                    Convert.ToBoolean(sData[iCommentCol + 4]) :
                                    true));
                            // this comment is from AIO file, so set IsOverlay = true
                            graphControl1.GraphData.Comments.Last().IsOverlay = true;
                        }
                        // old style AIO file
                        else
                        {
                            string sTemp;
                            sTemp = sData[iCommentCol];
                            if (sTemp != "")
                            {
                                try
                                {
                                    sTemp = sTemp.Replace(",,", "~~"); // do something with the old double-commas
                                    sData = sTemp.Split(','); // split the string on the commas
                                    AddComment(
                                        sData[0].Replace("<LF>", "\n").Replace("<CR>", "\r").Replace("~~", ","),  // restore the carriage returns, new lines, and commas
                                        new PointF(time, Convert.ToSingle(sData[1])),
                                        new Point(Convert.ToInt32(sData[2]), Convert.ToInt32(sData[3])),
                                        this.Settings.CommentsVisible);
                                    // this comment is from AIO file, so set IsOverlay = true
                                    graphControl1.GraphData.Comments.Last().IsOverlay = true;
                                }
                                catch
                                {
                                }
                            }
                        }
                    }

                    sLine = sr.ReadLine();
                }

                // Read the trace info
                if (!sr.EndOfStream)
                {
                    sLine = sr.ReadLine();
                    for (i = 0; i < iNumTraces && !sr.EndOfStream; i++)
                    {
                        sData = sLine.Split('\t');
                        try
                        {
                            Traces[i + nPrevNumTraces].Units = sData[1];
                            Traces[i + nPrevNumTraces].Format = sData[2];
                            Traces[i + nPrevNumTraces].IsOverlay = true;
                            string[] sColor = sData[4].Substring(7, sData[4].Length - 8).Split(',');
                            try
                            {
                                if (sColor.Length == 1)
                                    Traces[i + nPrevNumTraces].Color = Color.FromName(sColor[0]);
                                else
                                    Traces[i + nPrevNumTraces].Color = Color.FromArgb(Convert.ToInt32(sColor[0].Trim().Substring(2)),
                                        Convert.ToInt32(sColor[1].Trim().Substring(2)),
                                        Convert.ToInt32(sColor[2].Trim().Substring(2)),
                                        Convert.ToInt32(sColor[3].Trim().Substring(2)));
                            }
                            catch
                            {
                                Traces[i + nPrevNumTraces].Color = DefaultTraceColors.NextColor;
                            }
                        }
                        catch
                        {
                            try
                            {
                                Traces[i + nPrevNumTraces].Color = DefaultTraceColors.NextColor;
                            }
                            catch
                            {
                                MessageBox.Show(String.Format("An error occured opening file!"), "Data Plot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                sr.Close();
                                return;
                            }
                        }
                        sLine = sr.ReadLine();
                    }
                }
                // assuming old style AIO file, so guess about the traces...
                else
                {
                    double maxValue;
                    string sFormat;
                    sData = sHeader.Split('\t');
                    for (i = 0; i < iNumTraces; i++)
                    {
                        if (sData[i + 1] != "")
                        {
                            maxValue = Traces[i + nPrevNumTraces].Points.Max(P => P.Y);

                            if (maxValue < 1) sFormat = "0.00E+00";
                            else sFormat = "0.000";

                            Traces[i + nPrevNumTraces].Format = sFormat;

                            Traces[i + nPrevNumTraces].Color = DefaultTraceColors.NextColor;
                        }
                    }
                }

                // Read the the data plot settings
                if (!sr.EndOfStream)
                {
                    sLine = sr.ReadLine();
                    sData = sLine.Split('\t');

                    this.Settings.XMin = Convert.ToSingle(sData[0]);
                    this.Settings.XMax = Convert.ToSingle(sData[1]);
                    this.Settings.XAxisUnits = Enum<XAxisUnitsType>.Parse(sData[2]);
                    this.Settings.XAxisDisplayUnits = Enum<XAxisDisplayUnitsType>.Parse(sData[3]);
                    this.Settings.YMin = Convert.ToSingle(sData[4]);
                    this.Settings.YMinExp = Convert.ToInt32(sData[5]);
                    this.Settings.YMax = Convert.ToSingle(sData[6]);
                    this.Settings.YMaxExp = Convert.ToInt32(sData[7]);
                    this.Settings.PlotSemiLog = Convert.ToBoolean(sData[8]); ;
                    SetSettings();
                }

                sr.Close();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError(e.ToString(), VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error);
                MessageBox.Show(String.Format("An error occured opening file!"), "Data Plot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try
                {
                    if (sr != null) sr.Close();
                }
                catch
                {
                }
            }
        }

        private void MonitoredIO_ValueChanged(object sender, EventArgs e)
        {
            if (collectingData)
            {
                IDigitalIO digitalIO = sender as IDigitalIO;
                graphControl1.GraphData.IOStates.Add(
                    new DataPlotIOState
                    {
                        Time = stopWatchCollectData.ElapsedMilliseconds / 1000F,
                        Name = digitalIO.Name,
                        Description = digitalIO.Description,
                        Enabled = digitalIO.Value
                    });
            }
        }

        //private void openPlotFileToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.openFileDialog1.InitialDirectory = Application.StartupPath;
        //    this.openFileDialog1.Filter = "AIO Data Files (*.aio)|*.aio|All files (*.*)|*.*";
        //    this.openFileDialog1.FileName = string.Empty;
        //    this.openFileDialog1.FilterIndex = 1;
        //    if (sLastPath != string.Empty)
        //        this.openFileDialog1.InitialDirectory = sLastPath;

        //    if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        if (!File.Exists(this.openFileDialog1.FileName))
        //            MessageBox.Show("File does not exist!", "Data Plot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        else
        //        {
        //            try
        //            {
        //                Open(this.openFileDialog1.FileName);
        //                sLastPath = Path.GetDirectoryName(this.openFileDialog1.FileName);
        //            }
        //            catch
        //            {
        //                MessageBox.Show(String.Format("An error occured opening file: {0}", this.openFileDialog1.FileName), "Data Plot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
        //        }
        //    }
        //}
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphControl1.PageSetup();
        }

        private void pageSetupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            graphControl1.PageSetup();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphControl1.PrintWithDialog();
        }

        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            graphControl1.PrintWithDialog();
        }

        private void toggleAutoRunMenuItem_Click(object sender, EventArgs e)
        {
            toggleAutoRunMenuItem.Checked = !toggleAutoRunMenuItem.Checked;
            bool bState = toggleAutoRunMenuItem.Checked;
            AutoRun1Visible = bState;
            //AutoRun2Visible = bState;
        }

        private void assignYAxisPrintLabelMenuItem_Click(object sender, EventArgs e)
        {
            string input = "";
            ShowInputDialog(ref input);
            if (input.Length > 0)
                _yAxisPrintLabel = input;
        }

        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(300, 70);
            Form inputBox = new Form();
            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Y-Axis Print Label";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        private void toggleTimeUnitsMenuItem_Click(object sender, EventArgs e)
        {
            // toggle between Seconds and Minutes
            if (frmPlotProp1.RadioButtonMinutes.Checked)
            {
                frmPlotProp1.RadioButtonMinutes.Checked = false;
                frmPlotProp1.RadioButtonSeconds.Checked = true;
                Settings.XAxisUnits = XAxisUnitsType.Seconds;
            }
            else
            {
                frmPlotProp1.RadioButtonMinutes.Checked = true;
                frmPlotProp1.RadioButtonSeconds.Checked = false;
                Settings.XAxisUnits = XAxisUnitsType.Minutes;
            }
            Settings.Save();
            SetSettings();
            graphControl1.DrawAxes();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowProperties();
        }

        private void removeOverlayMenuItem_Click(object sender, EventArgs e)
        {
            graphControl1.RemoveOverlay((sender as ToolStripMenuItem).Tag as string);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start();
        }

        /// <summary>
        /// Saves the current data to the given AIO or CSV file.
        /// </summary>
        /// <param name="filename">Name of the AIO or CSV file</param>
        /// <param name="extension">File extension (.aio or .csv)</param>
        private void SaveDataPlotFile(string filename, string extension)
        {
            string cycleCommentString = "";
            float lastTimeValue = 0, fMinTimeDelta;
            string delimiter = "\t";
            if (extension == ".csv")
            {
                delimiter = ",";
            }
            //includes ms in DateTime
            //requires user to manually format cells in excel to "mm/dd/yyyy hh:mm:ss.000"
            //string dateTimeFormat = "MM/dd/yyyy hh:mm:ss.fff tt";
            string dateTimeFormat = "MM/dd/yyyy hh:mm:ss tt";
            //if (timerCollectData.Interval < 1000)
            //  fMinTimeDelta = (float)1.0; // save data no more frequently than once per second
            //else
            fMinTimeDelta = (float)timerCollectData.Interval / 1000;

            using (StreamWriter sw = new StreamWriter(filename))
            {
                // Write the header line for the data
                sw.Write("Time (" + (Settings.XAxisUnits == XAxisUnitsType.Seconds ? "SEC" : "MIN") + ")" + delimiter);

                foreach (var trace in Traces)
                {
                    sw.Write("{0}" + delimiter, trace.Label);
                }
                if (extension != ".csv")
                {
                    sw.WriteLine("Date Time" + delimiter + "Cycle Comments" + delimiter + "Comment" + delimiter + "Comment Signal" + delimiter + "Comment Offset X" + delimiter + "Comment Offset Y" + delimiter + "Comment Visible");
                }
                else
                {
                    sw.WriteLine("Date Time");
                }
                //new list which contains the Time and DateTime values for each point 
                var timeValues =
                    Traces.SelectMany(T => T.Points.Select(p => new { p.Time, p.DateTime }))
                    .OrderBy(a => a.Time)
                    .GroupBy(a => a.Time)
                    .Select(g => g.First());

                foreach (var timeValue in timeValues)
                {
                    if (timeValue.Time < lastTimeValue + fMinTimeDelta)
                    {
                        continue; // skip the to the next data point that occurs after fMinTimeDelta has elapsed
                    }
                    if (extension != ".csv")
                    {
                        if (graphControl1.GraphData.CycleComments.Any(C => C.Time > lastTimeValue && C.Time <= timeValue.Time))
                        {
                            cycleCommentString = string.Join(", ",
                                    graphControl1.GraphData.CycleComments
                                        .Where(C => C.Time > lastTimeValue && C.Time <= timeValue.Time)
                                        .Select(C => C.Comment).ToArray());
                        }
                        else
                        {
                            cycleCommentString = string.Empty;
                        }
                    }
                    //write data plot time value, divide by 60 if x-axis is in minutes
                    sw.Write("{0:F}" + delimiter, timeValue.Time / (Settings.XAxisUnits == XAxisUnitsType.Seconds ? 1 : 60));

                    foreach (var trace in Traces)
                    {
                        if (trace.Points.Any(p => p.Time > lastTimeValue && p.Time <= timeValue.Time))
                        {
                            sw.Write("{0}" + delimiter, trace.Points.First(p => p.Time > lastTimeValue && p.Time <= timeValue.Time).Signal);
                        }
                        else
                        {
                            sw.Write(string.Empty + delimiter);
                        }
                    }
                    if (extension != ".csv")
                    {
                        sw.Write("{0}" + delimiter + "{1}", timeValue.DateTime.ToString(dateTimeFormat), cycleCommentString);
                    }
                    else
                    {
                        sw.Write("{0}", timeValue.DateTime.ToString(dateTimeFormat));
                    }
                    if (extension != ".csv")
                    {
                        GraphComment graphComment =
                            graphControl1.GraphData.Comments.FirstOrDefault(
                                gc => gc.Location.X > lastTimeValue && gc.Location.X <= timeValue.Time);

                        if (graphComment != null)
                            sw.WriteLine(delimiter + "{0}" + delimiter + "{1}" + delimiter + "{2}" + delimiter + "{3}" + delimiter + "{4}",
                                graphComment.Text.Replace("\n", "\\n").Replace("\r", "\\r"), graphComment.Location.Y,
                              graphComment.CommentControl.Offset.X, graphComment.CommentControl.Offset.Y, graphComment.Visible.ToString());
                        else
                            sw.WriteLine();
                    }
                    else
                    {
                        sw.WriteLine();
                    }
                    lastTimeValue = timeValue.Time;
                }

                if (extension != ".csv")
                {
                    // Write the header line for the traces
                    sw.WriteLine("Trace" + delimiter + "Units" + delimiter + "Format" + delimiter + "Full Scale" + delimiter + "Color" + delimiter + "Visible");
                    // Write the trace details
                    foreach (var trace in Traces)
                    {
                        sw.WriteLine("{0}" + delimiter + "{1}" + delimiter + "{2}" + delimiter + "{3}" + delimiter + "{4}" + delimiter + "{5}", trace.Label, trace.Units, trace.Format, 0, trace.Color, trace.Visible);
                    }

                    // Write the the data plot settings
                    sw.WriteLine("X-Axis Min" + delimiter + "X-Axis Max" + delimiter + "X-Axis Units" + delimiter + "X-Axis Display Units" + delimiter + "Y-Axis Min" + delimiter + "Y-Axis Min Exp" + delimiter + "Y-Axis Max" + delimiter + "Y-Axis Max Exp" + delimiter + "Plot Log");
                    sw.WriteLine("{0}" + delimiter + "{1}" + delimiter + "{2}" + delimiter + "{3}" + delimiter + "{4}" + delimiter + "{5}" + delimiter + "{6}" + delimiter + "{7}" + delimiter + "{8}", this.Settings.XMin, this.Settings.XMax, this.Settings.XAxisUnits, this.Settings.XAxisDisplayUnits,
                        this.Settings.YMin, this.Settings.YMinExp, this.Settings.YMax, this.Settings.YMaxExp, this.Settings.PlotSemiLog);
                }
            }
        }

        private void SaveAs()
        {
            if (!string.IsNullOrEmpty(_filename)) saveFileDialog1.FileName = _filename;
            else saveFileDialog1.FileName = string.Empty;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Save(saveFileDialog1.FileName);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void savePlotAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_filename)) SaveAs();
            else Save(_filename);
        }

        private void ShowAllOfDataPlot()
        {
            if (Traces.Count > 0 || Traces[0].Points.Last().X > 1800)
            {
                Settings.XWindow = (float)Math.Round(Settings.XMax / 60, 0);
                //Settings.XAxisUnits = XAxisUnitsType.Minutes;
            }
            graphControl1.ShowAllOfGraph();
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.AutoShowEnd)
            {
                Settings.AutoShowEnd = false;
                Settings.Save();
                SetSettings();
            }

            this.ShowAllOfDataPlot();
        }

        private void ShowEndOfDataPlot()
        {
            graphControl1.ShowEndOfGraph(Settings.XWindow, (float)Settings.XWindowShiftPercent);
        }

        private void showEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.AutoShowAll)
            {
                Settings.AutoShowAll = false;
                Settings.Save();
                SetSettings();
            }

            this.ShowEndOfDataPlot();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void timerCollectData_Tick(object sender, EventArgs e)
        {
            AddDataPoint();
            graphControl1.DrawGraph();
            SetCaption();
        }

        private void toolStripButtonAutoRun1_Click(object sender, EventArgs e)
        {
            Settings.AutoRun1 = toolStripButtonAutoRun1.Checked;
            Settings.Save();
            SetSettings();
        }

        private void toolStripButtonAutoRun2_Click(object sender, EventArgs e)
        {
            Settings.AutoRun2 = toolStripButtonAutoRun2.Checked;
            Settings.Save();
            SetSettings();
        }

        private void toolStripButtonAutoShowAll_Click(object sender, EventArgs e)
        {
            //toolStripButtonShowAll.Checked = toolStripButtonAutoShowAll.Checked;
            Settings.AutoShowAll = toolStripButtonAutoShowAll.Checked;
            if (Settings.AutoShowAll)
            {
                Settings.AutoShowEnd = false;
                //toolStripButtonAutoShowEnd.Checked = false;
                //toolStripButtonShowEnd.Checked = false;
                //Settings.AutoShowEnd = false;
                ShowAllOfDataPlot();
            }
            Settings.Save();
            SetSettings();
        }

        private void toolStripButtonAutoShowEnd_Click(object sender, EventArgs e)
        {
            //toolStripButtonShowEnd.Checked = toolStripButtonAutoShowEnd.Checked;
            Settings.AutoShowEnd = toolStripButtonAutoShowEnd.Checked;
            if (Settings.AutoShowEnd)
            {
                Settings.AutoShowAll = false;
                //toolStripButtonAutoShowAll.Checked = false;
                //toolStripButtonShowAll.Checked = false;
                //Settings.AutoShowAll = false;
                ShowEndOfDataPlot();
            }
            Settings.Save();
            SetSettings();
        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            //this.Hide();
            OnClose();
        }

        private void toolStripButtonLinear_Click(object sender, EventArgs e)
        {
            //toolStripButtonLinear.Checked = true;
            //toolStripButtonLog.Checked = false;
            Settings.PlotSemiLog = false;
            Settings.Save();
            SetSettings();
            frmPlotProp1.DataPlot.YMaxUp.Visible = true;
            frmPlotProp1.DataPlot.YMaxDn.Visible = true;
            frmPlotProp1.DataPlot.YMinUp.Visible = true;
            frmPlotProp1.DataPlot.YMinDn.Visible = true;
            frmPlotProp1.DataPlot.YMaxExpUp.Visible = true;
            frmPlotProp1.DataPlot.YMaxExpDn.Visible = true;
            frmPlotProp1.DataPlot.YMinExpUp.Visible = true;
            frmPlotProp1.DataPlot.YMinExpDn.Visible = true;
            graphControl1.ReDrawGraph();
        }

        private void toolStripButtonLog_Click(object sender, EventArgs e)
        {
            //toolStripButtonLog.Checked = true;
            //toolStripButtonLinear.Checked = false;
            Settings.PlotSemiLog = true;
            Settings.Save();
            SetSettings();
            frmPlotProp1.DataPlot.YMaxUp.Visible = true;
            frmPlotProp1.DataPlot.YMaxDn.Visible = true;
            frmPlotProp1.DataPlot.YMinUp.Visible = true;
            frmPlotProp1.DataPlot.YMinDn.Visible = true;
            frmPlotProp1.DataPlot.YMaxExpUp.Visible = false;
            frmPlotProp1.DataPlot.YMaxExpDn.Visible = false;
            frmPlotProp1.DataPlot.YMinExpUp.Visible = false;
            frmPlotProp1.DataPlot.YMinExpDn.Visible = false;
            graphControl1.ReDrawGraph();
        }

        private void toolStripButtonRun_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void toolStripButtonShowAll_Click(object sender, EventArgs e)
        {
            if (Settings.AutoShowEnd)
            {
                Settings.AutoShowEnd = false;
                Settings.Save();
                SetSettings();
            }

            this.ShowAllOfDataPlot();
        }

        private void toolStripButtonShowEnd_Click(object sender, EventArgs e)
        {
            if (Settings.AutoShowAll)
            {
                Settings.AutoShowAll = false;
                Settings.Save();
                SetSettings();
            }

            this.ShowEndOfDataPlot();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Trace_ValueChanged(object sender, EventArgs e)
        {
            if (collectingData)
            {
                DataPlotTraceType trace = sender as DataPlotTraceType;
                lock (((IList)trace.Points).SyncRoot)
                {
                    trace.Points.Add(
                        new DataPointType(stopWatchCollectData.ElapsedMilliseconds / 1000F, trace.Value, DateTime.Now));
                }

                CheckForGraphRescaling();
            }
        }

        private void Traces_Changed(object sender, TraceCollectionChangedEventArgs<DataPlotTraceType, DataPointType> e)
        {
            DataPlotTraceType trace = e.ChangedItem;

            switch (e.ChangeType)
            {
                case TraceChangeType.Added:
                    if (trace.IsOverlay) AddRemoveOverlayMenuItem(trace);
                    else AddOverlayMenuItem(trace);
                    break;

                case TraceChangeType.Removed:
                    if (trace.IsOverlay)
                        clearOverlayToolStripMenuItem.DropDownItems.RemoveByKey(
                            string.Format("{0}RemoveMenuStripItem", trace.Key));
                    break;

                case TraceChangeType.Changed:
                    if (trace.IsOverlay)
                    {
                        if (clearOverlayToolStripMenuItem.DropDownItems.ContainsKey(
                            string.Format("{0}RemoveMenuStripItem", trace.Key)))
                        {
                            var menuitem = clearOverlayToolStripMenuItem.DropDownItems[
                                string.Format("{0}RemoveMenuStripItem", trace.Key)];

                            menuitem.Text = string.Format("{0} ({1})",
                                trace.Label, trace.Color.Name);
                        }
                    }
                    else
                    {
                        if (addOverlayToolStripMenuItem.DropDownItems.ContainsKey(
                            string.Format("{0}AddMenuStripItem", trace.Key)))
                        {
                            var menuitem = addOverlayToolStripMenuItem.DropDownItems[
                                string.Format("{0}AddMenuStripItem", trace.Key)];

                            menuitem.Text = string.Format("{0} ({1})",
                                trace.Label, trace.Color.Name);
                        }
                    }
                    break;
            }
        }

        private void yAxisLinearModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.PlotSemiLog = false;
            Settings.Save();
            SetSettings();
            graphControl1.ReDrawGraph();
        }

        private void dockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dataPlotDockControl.ShowDocked();
        }

        private void yAxisLogModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.PlotSemiLog = true;
            Settings.Save();
            SetSettings();
            graphControl1.ReDrawGraph();
        }

        #endregion Private Methods
        #region Internal Methods (1)

        //internal void CheckForRecalledScan()
        //{
        //    if (this.RecalledScan)
        //    {
        //        Traces.Clear();
        //        savedTraces.ForEach(T => Traces.Add(T));
        //        savedTraces.Clear();
        //        RecalledScan = false;
        //        this.RecalledScan = false;
        //    }
        //}
        public void SetSettings()
        {
            try
            {
                autoRun1ToolStripMenuItem.Checked =
                    toolStripButtonAutoRun1.Checked = Settings.AutoRun1;
                autoRun2ToolStripMenuItem.Checked =
                    toolStripButtonAutoRun2.Checked = Settings.AutoRun2;
                autoShowAllToolStripMenuItem.Checked =
                    toolStripButtonAutoShowAll.Checked = Settings.AutoShowAll;
                toolStripButtonShowAll.Checked = Settings.AutoShowAll;
                autoShowEndToolStripMenuItem.Checked =
                    toolStripButtonAutoShowEnd.Checked = Settings.AutoShowEnd;
                toolStripButtonShowEnd.Checked = Settings.AutoShowEnd;
                yAxisLinearModeToolStripMenuItem.Checked =
                    toolStripButtonLinear.Checked = !Settings.PlotSemiLog;
                yAxisLogModeToolStripMenuItem.Checked =
                    toolStripButtonLog.Checked = Settings.PlotSemiLog;

                for (int i = 0; i < Traces.Count; i++)
                {
                    if (Settings.TraceColors.Count > i)
                        Traces[i].Color = ColorTranslator.FromHtml(Settings.TraceColors[i]);
                    if (Settings.TraceVisibility.Count > i)
                        Traces[i].Visible = Settings.TraceVisibility[i];
                }

                ////graphControl1.GraphTypeName = _plotName;// Settings.Header;
                graphControl1.HeaderLeft = Settings.HeaderLeft;
                graphControl1.LegendForPrintedPlot = Settings.LegendForPrintedPlot;
                graphControl1.HeaderRight = Settings.HeaderRight;
                graphControl1.Header = Settings.Header;
                //graphControl1.Settings.LegendVisible = false;
                graphControl1.XAxisLabel =
            string.Format("Time ({0})", Settings.XAxisUnits.ToString());
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning("An error occured setting Data Plot settings.",
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());
            }
        }

        #endregion Internal Methods

        #endregion Methods

        private void YMaxUp_Click(object sender, EventArgs e)
        {
            if (Settings.PlotSemiLog)
            {
                frmPlotProp1.numericUpDownYMaxExp.Value = (decimal)Settings.YMaxExp;
                if (frmPlotProp1.numericUpDownYMaxExp.Value < frmPlotProp1.numericUpDownYMaxExp.Maximum)
                {
                    frmPlotProp1.numericUpDownYMaxExp.Value++;
                    Settings.YMaxExp = (int)frmPlotProp1.numericUpDownYMaxExp.Value;
                    frmPlotProp1.CrntControl = frmPlotProp1.numericUpDownYMaxExp;
                }
            }
            else
            { // Plot Linear
                SetYMaxLinear(true);
            }
            dtPrevYMaxClickTime = DateTime.Now;
        }

        private void YMaxDn_Click(object sender, EventArgs e)
        {
            if (Settings.PlotSemiLog)
            {
                frmPlotProp1.numericUpDownYMaxExp.Value = (decimal)Settings.YMaxExp;
                if (frmPlotProp1.numericUpDownYMaxExp.Value > frmPlotProp1.numericUpDownYMaxExp.Minimum)
                    if (frmPlotProp1.numericUpDownYMaxExp.Value > frmPlotProp1.numericUpDownYMinExp.Value + 1)
                    {
                        frmPlotProp1.numericUpDownYMaxExp.Value--;
                        Settings.YMaxExp = (int)frmPlotProp1.numericUpDownYMaxExp.Value;
                        frmPlotProp1.CrntControl = frmPlotProp1.numericUpDownYMaxExp;
                    }
            }
            else
            { // Plot Linear
                SetYMaxLinear(false);
            }
            dtPrevYMaxClickTime = DateTime.Now;
        }

        private void YMinUp_Click(object sender, EventArgs e)
        {
            if (Settings.PlotSemiLog)
            {
                frmPlotProp1.numericUpDownYMinExp.Value = (decimal)Settings.YMinExp;
                if (frmPlotProp1.numericUpDownYMinExp.Value < frmPlotProp1.numericUpDownYMaxExp.Maximum)
                    if (frmPlotProp1.numericUpDownYMinExp.Value < frmPlotProp1.numericUpDownYMaxExp.Value - 1)
                    {
                        frmPlotProp1.numericUpDownYMinExp.Value++;
                        Settings.YMinExp = (int)frmPlotProp1.numericUpDownYMinExp.Value;
                        frmPlotProp1.CrntControl = frmPlotProp1.numericUpDownYMaxExp;
                    }
            }
            else
            { // Plot Linear
                SetYMinLinear(true);
            }
            dtPrevYMinClickTime = DateTime.Now;
        }

        private void YMinDn_Click(object sender, EventArgs e)
        {
            if (Settings.PlotSemiLog)
            {
                frmPlotProp1.numericUpDownYMinExp.Value = (decimal)Settings.YMinExp;
                if (frmPlotProp1.numericUpDownYMinExp.Value > frmPlotProp1.numericUpDownYMinExp.Minimum)
                {
                    frmPlotProp1.numericUpDownYMinExp.Value--;
                    Settings.YMinExp = (int)frmPlotProp1.numericUpDownYMinExp.Value;
                    frmPlotProp1.CrntControl = frmPlotProp1.numericUpDownYMaxExp;
                }
            }
            else
            { // Plot Linear
                SetYMinLinear(false);
            }
            dtPrevYMinClickTime = DateTime.Now;
        }

        private void SetYMaxExpLinear(bool bIsUpClick)
        {
            try
            {
                string strYMax = Settings.YMax.ToString().ToLower();
                int ndxExponent = strYMax.IndexOf('e');
                if (ndxExponent == -1)
                {
                    if (bIsUpClick)
                        strYMax = Convert.ToSingle(strYMax + "E+01").ToString();
                    else
                        strYMax = Convert.ToSingle(strYMax + "E-01").ToString();
                }
                else
                {
                    bool bIsInteger = true;
                    int nVal = Convert.ToInt32(strYMax.Substring(ndxExponent + 1));
                    string strNVal = nVal.ToString(), strTemp = "", strComp = "0123456789-";
                    int ii, Len = strNVal.Length;
                    for (ii = 0; ii < Len; ii++)
                    {
                        strTemp = strNVal.Substring(ii, 1);
                        if (!strComp.Contains(strTemp))
                        {
                            bIsInteger = false;
                            break;
                        }
                    }
                    if (bIsInteger)
                    {
                        if (bIsUpClick)
                            nVal++;
                        else
                            nVal--;
                    }
                    if (nVal >= -15 && nVal <= 15)
                        strYMax = strYMax.Substring(0, ndxExponent) + "E" + nVal.ToString();
                }
                if (Convert.ToSingle(Settings.YMin) < Convert.ToSingle(strYMax))
                    Settings.YMax = Convert.ToSingle(strYMax);
            }
            catch (Exception ex)
            {
            }
        }

        private void SetYMinExpLinear(bool bIsUpClick)
        {
            try
            {
                string strYMin = Settings.YMin.ToString().ToLower();
                int ndxExponent = strYMin.IndexOf('e');
                if (ndxExponent == -1)
                {
                    if (bIsUpClick)
                        strYMin = Convert.ToSingle(strYMin + "E+01").ToString();
                    else
                        strYMin = Convert.ToSingle(strYMin + "E-01").ToString();
                }
                else
                {
                    bool bIsInteger = true;
                    int nVal = Convert.ToInt32(strYMin.Substring(ndxExponent + 1));
                    string strNVal = nVal.ToString(), strTemp = "", strComp = "0123456789-";
                    int ii, Len = strNVal.Length;
                    for (ii = 0; ii < Len; ii++)
                    {
                        strTemp = strNVal.Substring(ii, 1);
                        if (!strComp.Contains(strTemp))
                        {
                            bIsInteger = false;
                            break;
                        }
                    }
                    if (bIsInteger)
                    {
                        if (bIsUpClick)
                            nVal++;
                        else
                            nVal--;
                    }
                    if (nVal >= -15 && nVal <= 15)
                        strYMin = strYMin.Substring(0, ndxExponent) + "E" + nVal.ToString();
                }
                if (Convert.ToSingle(Settings.YMax) > Convert.ToSingle(strYMin))
                    Settings.YMin = Convert.ToSingle(strYMin);
            }
            catch (Exception ex)
            {
            }
        }

        // used only for Linear Plot
        private void YMaxExpUp_Click(object sender, EventArgs e)
        {
            if (!Settings.PlotSemiLog)
            {
                SetYMaxExpLinear(true);
            }
        }

        private void YMaxExpDn_Click(object sender, EventArgs e)
        {
            if (!Settings.PlotSemiLog)
            {
                SetYMaxExpLinear(false);
            }
        }

        private void YMinExpUp_Click(object sender, EventArgs e)
        {
            if (!Settings.PlotSemiLog)
            {
                SetYMinExpLinear(true);
            }
        }

        private void YMinExpDn_Click(object sender, EventArgs e)
        {
            if (!Settings.PlotSemiLog)
            {
                SetYMinExpLinear(false);
            }
        }

        private void SetYMaxLinear(bool bIsUpClick)
        {
            try
            {
                float retVal = 0;
                if (!float.TryParse(Settings.YMax.ToString(), out retVal))
                    return;
                string strYMax = string.Format("{0:0.000000000000000000}", retVal);
                string shortestNumericalFormat = graphControl1.GetShortestDistinctFormat(Settings.YMin, Settings.YMax, 10);
                string strYMaxPrev = graphControl1.AutoformatYValue(Settings.YMax, shortestNumericalFormat).ToLower();
                string strMantissa = "";
                string strExponent = strYMaxPrev.Substring(strYMaxPrev.IndexOf("e") + 1);
                int nDecimal = strYMaxPrev.IndexOf("."), nLen = strYMaxPrev.Length;
                float fDelta = 1.0f, fMantissa = 0, fYMax;
                if (strYMaxPrev.ToLower().Contains("e"))
                {
                    strMantissa = strYMaxPrev.Substring(0, strYMaxPrev.IndexOf("e"));
                    nLen = strMantissa.Length;
                    //if (strYMaxPrev.ToLower().Substring(0, strYMaxPrev.ToLower().IndexOf("e")) == "1" && !bIsUpClick)
                    if (strYMaxPrev.StartsWith("1.") && !bIsUpClick)
                    {
                        //if current YMax mantissa is 1 (1E-5, 1E-6, etc.) and is down arrow click
                        //if YMax is 1E-5 and down arrow is clicked, next value is 0.9E-5 (9E-6)
                        //fDelta = 0.9f;
                    }
                    if (nDecimal != -1)
                    {
                        //this was used before 5-27-21:
                        //// is decimal point
                        //fDelta = (float)Math.Pow(10.0f, -Convert.ToSingle(nLen - nDecimal - 1));

                        //NJ commented-out line above on 5-27-21 per GMS. 
                        //In linear mode, if YMax is 8.5E-5 and SetYMaxLinear button is up-clicked, change to 9.5E-5.
                        //If SetYMaxLinear button is down-clicked, change to 7.5E-5.
                        //(use fDelta default value of 1.0f)
                    }
                    if (!bIsUpClick)
                    {
                        fDelta = -fDelta;
                    }
                    //NJ 5-27-21
                    if (strYMaxPrev.StartsWith("1") && !bIsUpClick)
                    {
                        //down-click from 1.3E-5 -> set it to 9.3E-6
                        fMantissa = (Convert.ToSingle(strMantissa) + 8) / 10;
                    }
                    else if (strYMaxPrev.StartsWith("9") && bIsUpClick)
                    {
                        //up-click from 9.3E-6 -> set it to 1.3E-5
                        fMantissa = (Convert.ToSingle(strMantissa) - 8) * 10;
                    }
                    else
                    {
                        fMantissa = Convert.ToSingle(strMantissa) + fDelta;
                    }
                    fYMax = Convert.ToSingle(fMantissa.ToString() + "e" + strExponent);
                    if (fYMax > Convert.ToSingle(Settings.YMin))
                        Settings.YMax = Convert.ToSingle(fMantissa.ToString() + "e" + strExponent);
                    return;
                }
                else if (nDecimal == -1)
                { // no decimal point
                    if (!bIsUpClick)
                        fDelta = -fDelta;
                    fYMax = Convert.ToSingle(strYMaxPrev) + fDelta;
                    if (fYMax > Convert.ToSingle(Settings.YMin))
                        Settings.YMax = fYMax;
                    return;
                }
                else
                {
                    fDelta = (float)Math.Pow(10.0f, -Convert.ToSingle(nLen - nDecimal - 1));
                    if (!bIsUpClick)
                        fDelta = -fDelta;
                    fYMax = Convert.ToSingle(strYMaxPrev) + fDelta;
                    if (fYMax > Convert.ToSingle(Settings.YMin))
                        Settings.YMax = fYMax;
                    return;
                }

                bool bIsPositive = true;
                if (strYMax.StartsWith("-"))
                    bIsPositive = false;
                int nDecimalLocation = strYMax.IndexOf('.');
                // determine location of first significant digit
                int ii, Len = strYMax.Length, nFirstSignificantDigit = -1, nLastSignificantDigit = -1;
                string strTemp = "", strComp = "123456789";
                for (ii = 0; ii < Len; ii++)
                {
                    strTemp = strYMax.Substring(ii, 1);
                    if (strComp.Contains(strTemp) && nFirstSignificantDigit == -1)
                    {
                        nFirstSignificantDigit = ii;
                    }
                    if (nFirstSignificantDigit > -1 && nLastSignificantDigit == -1)
                    {
                        if (strYMax.Substring(ii, 3) == "000")
                        {
                            nLastSignificantDigit = ii - 1;
                            break;
                        }
                    }
                }
                if (nFirstSignificantDigit == -1)
                {
                    if (bIsUpClick)
                    {
                        strYMax = "1";
                        Settings.YMax = 1.0f;
                    }
                    else
                    {
                        strYMax = "-1";
                        Settings.YMax = -1.0f;
                    }
                    ndxOfPrevYMaxIncrDigit = 0;
                    return;
                }
                int ndxOfIncrDigit = nFirstSignificantDigit;
                int nDiff = nDecimalLocation - nFirstSignificantDigit;
                TimeSpan ts = DateTime.Now - dtPrevYMaxClickTime;
                int nSimpleDecimal = SimpleDecimal(nDecimalLocation, nFirstSignificantDigit, strYMax);
                bool bIsSimpleNum = IsSimpleNumber(nDecimalLocation, nFirstSignificantDigit, strYMax);
                if (nLastSignificantDigit - nFirstSignificantDigit <= 3 && nDecimalLocation > nFirstSignificantDigit && nDecimalLocation < nLastSignificantDigit)
                    ndxOfIncrDigit = nLastSignificantDigit;
                else if (nSimpleDecimal > -1 && !bIsSimpleNum)
                    ndxOfIncrDigit = nSimpleDecimal;
                else if (bIsSimpleNum)
                    ndxOfIncrDigit = nDecimalLocation - 1;
                else
                {
                    if (ts.TotalSeconds > 5)
                    { // it has been at least 5 seconds since the user last clicked a YMax Up/Dn button
                        if (nDiff > 0 && nDiff < 3)
                            ndxOfIncrDigit += 3;
                        else
                            ndxOfIncrDigit += 2;
                    }
                    else
                        ndxOfIncrDigit = ndxOfPrevYMaxIncrDigit;
                    if (!bIsPositive)
                        ndxOfIncrDigit++;
                }
                string strSignificantDigits = strYMax.Substring(nFirstSignificantDigit, ndxOfIncrDigit - nFirstSignificantDigit + 1);
                if (strSignificantDigits.Length == 0)
                    return;
                strIncrDigits(ref strSignificantDigits, bIsPositive, bIsUpClick);
                if (strSignificantDigits == "1000")
                {
                    strSignificantDigits = "100";
                    bIsSimpleNum = false;
                    nDiff++;
                }
                if (!bIsPositive)
                    strYMax = "-" + strSignificantDigits + "E";
                else
                    strYMax = strSignificantDigits + "E";
                if (bIsSimpleNum || nSimpleDecimal > -1)
                {
                    strYMax += "+0";
                }
                else if (nDiff - 3 >= 0)
                {
                    strYMax += string.Format("+{0:00}", Math.Abs(nDiff - 3));
                }
                else
                {
                    strYMax += string.Format("-{0:00}", Math.Abs(nDiff - 3));
                }
                if (Convert.ToSingle(Settings.YMin) < Convert.ToSingle(strYMax))
                    Settings.YMax = Convert.ToSingle(strYMax);
                ndxOfPrevYMaxIncrDigit = ndxOfIncrDigit;
            }
            catch (Exception ex)
            {
            }
        }

        private void SetYMinLinear(bool bIsUpClick)
        {
            try
            {
                float retVal = 0;
                if (!float.TryParse(Settings.YMin.ToString(), out retVal))
                    return;
                string strYMin = string.Format("{0:0.000000000000000000}", retVal);
                string shortestNumericalFormat = graphControl1.GetShortestDistinctFormat(Settings.YMin, Settings.YMax, 10);
                string strYMinPrev = graphControl1.AutoformatYValue(Settings.YMin, shortestNumericalFormat).ToLower();
                string strMantissa = "";
                string strExponent = strYMinPrev.Substring(strYMinPrev.IndexOf("e") + 1);
                int nDecimal = strYMinPrev.IndexOf("."), nLen = strYMinPrev.Length;
                float fDelta = 1.0f, fMantissa = 0, fYMin;
                if (strYMinPrev.ToLower().Contains("e"))
                {
                    strMantissa = strYMinPrev.Substring(0, strYMinPrev.IndexOf("e"));
                    nLen = strMantissa.Length;
                    if (nDecimal != -1)
                    {
                        //this was used before 5-27-21:
                        //// is decimal point
                        //fDelta = (float)Math.Pow(10.0f, -Convert.ToSingle(nLen - nDecimal - 1));

                        //NJ commented-out line above on 5-27-21 per GMS. 
                        //In linear mode, if YMin is 8.5E-5 and SetYMinLinear button is up-clicked, change to 9.5E-5.
                        //If SetYMinLinear button is down-clicked, change to 7.5E-5.
                    }
                    if (!bIsUpClick)
                    {
                        fDelta = -fDelta;
                    }
                    //NJ 5-27-21
                    if (strYMinPrev.StartsWith("1") && !bIsUpClick)
                    {
                        //down-click from 1.3E-5 -> set it to 9.3E-6
                        //down-click from 1E-6 -> set it to 9E-7
                        fMantissa = (Convert.ToSingle(strMantissa) + 8) / 10;
                    }
                    else if (strYMinPrev.StartsWith("9") && bIsUpClick)
                    {
                        //up-click from 9.3E-6 -> set it to 1.3E-5
                        //up-click from 9E-7 -> set it to 1E-6
                        fMantissa = (Convert.ToSingle(strMantissa) - 8) * 10;
                    }
                    else
                    {
                        fMantissa = Convert.ToSingle(strMantissa) + fDelta;
                    }
                    fYMin = Convert.ToSingle(fMantissa.ToString() + "e" + strExponent);
                    if (fYMin < Convert.ToSingle(Settings.YMax))
                        Settings.YMin = Convert.ToSingle(fMantissa.ToString() + "e" + strExponent);
                    return;
                }
                else if (nDecimal == -1)
                { // no decimal point
                    if (!bIsUpClick)
                        fDelta = -fDelta;
                    fYMin = Convert.ToSingle(strYMinPrev) + fDelta;
                    if (fYMin < Convert.ToSingle(Settings.YMax))
                        Settings.YMin = fYMin;
                    return;
                }
                else
                {
                    fDelta = (float)Math.Pow(10.0f, -Convert.ToSingle(nLen - nDecimal - 1));
                    if (!bIsUpClick)
                        fDelta = -fDelta;
                    fYMin = Convert.ToSingle(strYMinPrev) + fDelta;
                    if (fYMin < Convert.ToSingle(Settings.YMax))
                        Settings.YMin = fYMin;
                    return;
                }
                bool bIsPositive = true;
                if (strYMin.StartsWith("-"))
                    bIsPositive = false;
                int nDecimalLocation = strYMin.IndexOf('.');
                // determine location of first significant digit
                int ii, Len = strYMin.Length, nFirstSignificantDigit = -1, nLastSignificantDigit = -1;
                string strTemp = "", strComp = "123456789";
                for (ii = 0; ii < Len; ii++)
                {
                    strTemp = strYMin.Substring(ii, 1);
                    if (strComp.Contains(strTemp) && nFirstSignificantDigit == -1)
                    {
                        nFirstSignificantDigit = ii;
                    }
                    if (nFirstSignificantDigit > -1 && nLastSignificantDigit == -1)
                    {
                        if (strYMin.Substring(ii, 5) == "00000" || strYMin.Substring(ii, 5) == ".0000")
                        {
                            nLastSignificantDigit = ii - 1;
                            break;
                        }
                    }
                }
                if (nFirstSignificantDigit == -1)
                {
                    if (bIsUpClick)
                    {
                        strYMin = "1";
                        Settings.YMin = 1.0f;
                    }
                    else
                    {
                        strYMin = "-1";
                        Settings.YMin = -1.0f;
                    }
                    ndxOfPrevYMinIncrDigit = 0;
                    return;
                }
                int ndxOfIncrDigit = nFirstSignificantDigit;
                int nDiff = nDecimalLocation - nFirstSignificantDigit;
                TimeSpan ts = DateTime.Now - dtPrevYMinClickTime;
                int nSimpleDecimal = SimpleDecimal(nDecimalLocation, nFirstSignificantDigit, strYMin);
                bool bIsSimpleNum = IsSimpleNumber(nDecimalLocation, nFirstSignificantDigit, strYMin);
                if (nLastSignificantDigit - nFirstSignificantDigit <= 3 && nDecimalLocation > nFirstSignificantDigit && nDecimalLocation < nLastSignificantDigit)
                    ndxOfIncrDigit = nLastSignificantDigit;
                else if (nSimpleDecimal > -1 && !bIsSimpleNum)
                    ndxOfIncrDigit = nSimpleDecimal;
                else if (bIsSimpleNum)
                    ndxOfIncrDigit = nDecimalLocation - 1;
                else
                {
                    if (ts.TotalSeconds > 5)
                    { // it has been at least 5 seconds since the user last clicked a YMin Up/Dn button
                        if (nDiff > 0 && nDiff < 3)
                            ndxOfIncrDigit += 3;
                        else
                            ndxOfIncrDigit += 2;
                    }
                    else
                        ndxOfIncrDigit = ndxOfPrevYMinIncrDigit;
                    if (!bIsPositive)
                        ndxOfIncrDigit++;
                }
                string strSignificantDigits = strYMin.Substring(nFirstSignificantDigit, ndxOfIncrDigit - nFirstSignificantDigit + 1);
                if (strSignificantDigits.Length == 0)
                    return;
                strIncrDigits(ref strSignificantDigits, bIsPositive, bIsUpClick);
                if (strSignificantDigits == "1000")
                {
                    strSignificantDigits = "100";
                    bIsSimpleNum = false;
                    nDiff++;
                }
                if (!bIsPositive)
                    strYMin = "-" + strSignificantDigits + "E";
                else
                    strYMin = strSignificantDigits + "E";
                if (bIsSimpleNum || nSimpleDecimal > -1)
                {
                    strYMin += "+0";
                }
                else if (nDiff - 3 >= 0)
                {
                    strYMin += string.Format("+{0:00}", Math.Abs(nDiff - 3));
                }
                else
                {
                    strYMin += string.Format("-{0:00}", Math.Abs(nDiff - 3));
                }
                if (Convert.ToSingle(strYMin) < Convert.ToSingle(Settings.YMax))
                    Settings.YMin = Convert.ToSingle(strYMin);
                ndxOfPrevYMinIncrDigit = ndxOfIncrDigit;
            }
            catch (Exception ex)
            {
            }
        }

        private void strIncrDigits(ref string strDigits, bool bIsPositive, bool bIsUpClick)
        {
            int nDecimalLocation = strDigits.IndexOf('.');
            float fDigits = Convert.ToSingle(strDigits);
            switch (nDecimalLocation)
            {
                case 0:
                    if ((bIsPositive && bIsUpClick) || (!bIsPositive && !bIsUpClick))
                    {
                        fDigits += 0.001f;
                        if (fDigits > 0.999f)
                            fDigits = 0.999f;
                    }
                    else
                    {
                        fDigits -= 0.001f;
                        if (fDigits < -0.999f)
                            fDigits = -0.999f;
                    }
                    strDigits = string.Format("{0:0.000}", fDigits);
                    break;

                case 1:
                    if ((bIsPositive && bIsUpClick) || (!bIsPositive && !bIsUpClick))
                    {
                        fDigits += 0.01f;
                        if (fDigits > 9.99f)
                            fDigits = 9.99f;
                    }
                    else
                    {
                        fDigits -= 0.01f;
                        if (fDigits < -9.99f)
                            fDigits = -9.99f;
                    }
                    strDigits = string.Format("{0:0.00}", fDigits);
                    break;

                case 2:
                    if ((bIsPositive && bIsUpClick) || (!bIsPositive && !bIsUpClick))
                    {
                        fDigits += 0.1f;
                        if (fDigits > 99.9f)
                            fDigits = 99.9f;
                    }
                    else
                    {
                        fDigits -= 0.1f;
                        if (fDigits < -99.9f)
                            fDigits = -99.9f;
                    }
                    strDigits = string.Format("{0:0.0}", fDigits);
                    break;

                default:
                    if ((bIsPositive && bIsUpClick) || (!bIsPositive && !bIsUpClick))
                    {
                        fDigits += 1.0f;
                        //if (fDigits > 999.0f)
                        //  fDigits = 999.0f;
                    }
                    else
                    {
                        fDigits -= 1.0f;
                        //if (fDigits < -999.0f)
                        //  fDigits = -999.0f;
                    }
                    strDigits = string.Format("{0:0.}", fDigits);
                    break;
            }
        }

        private bool IsSimpleNumber(int nDecimalLocation, int nFirstSignificantDigit, string strY)
        {
            if (nDecimalLocation - nFirstSignificantDigit > 0 && nDecimalLocation - nFirstSignificantDigit <= 3)
                return true;
            return false;
        }

        private int SimpleDecimal(int nDecimalLocation, int nFirstSignificantDigit, string strY)
        {
            if (nDecimalLocation - nFirstSignificantDigit > 0 && nDecimalLocation - nFirstSignificantDigit <= 3)
            {
                if (strY.Substring(nFirstSignificantDigit + 1, 2) == "00")
                    return nFirstSignificantDigit;
                if (strY.Substring(nFirstSignificantDigit + 2, 2) == "00")
                    return nFirstSignificantDigit + 1;
                if (strY.Substring(nFirstSignificantDigit + 3, 2) == "00")
                    return nFirstSignificantDigit + 2;
            }
            return -1;
        }
    }
}