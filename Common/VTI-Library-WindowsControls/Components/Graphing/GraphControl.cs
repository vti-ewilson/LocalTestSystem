using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Graphing;
using VTIWindowsControlLibrary.Classes.Graphing.DataPlot;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;
using VTIWindowsControlLibrary.Classes.Graphing.Util;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Components.Graphing
{
    /// <summary>
    /// A generic graphing control which can be used to graph any type of data
    /// that meets the interfaces specified.  To be used, this control must be
    /// subclassed into a non-generic version with the generic types specified.
    /// </summary>
    /// <typeparam name="TData">Type of <see cref="VTIWindowsControlLibrary.Classes.Graphing.GraphData{TCollection, TTrace, TPoint, TSettings}">Graph Data</see></typeparam>
    /// <typeparam name="TCollection">Type of <see cref="KeyedTraceCollection{TTrace, TPoint}">Trace Collection</see></typeparam>
    /// <typeparam name="TTrace">Type of <see cref="IGraphTrace{TTrace, TPoint}">Trace</see></typeparam>
    /// <typeparam name="TPoint">Type of <see cref="IGraphPoint">Graph Point</see></typeparam>
    /// <typeparam name="TSettings">Type of <see cref="GraphSettings">Graph Settings</see></typeparam>
    public partial class GraphControl<TData, TCollection, TTrace, TPoint, TSettings> : UserControl
      where TData : GraphData<TCollection, TTrace, TPoint, TSettings>, new()
      where TCollection : KeyedTraceCollection<TTrace, TPoint>, new()
      where TTrace : class, IGraphTrace<TTrace, TPoint>, new()
      where TPoint : class, IGraphPoint, new()
      where TSettings : GraphSettings, new()
    {
        #region Fields (42)

        #region Public Fields (2)

        /// <summary>
        /// Optional function to format the Plot Cursor Axis Label when the plot cursor is displayed.
        /// </summary>
        public Func<float, string> FormatPlotCursorAxisLabel;

        /// <summary>
        /// Optional function to get the DateTime value of a point.
        /// </summary>
        public Func<float, DateTime> GetDateTimeOfPoint;

        /// <summary>
        /// Optional function to format the X-Axis tic labels
        /// </summary>
        public Func<float, float, string> FormatXAxisTicLabel;

        #endregion Public Fields
        #region Private Fields (40)

        private CommentControl _currentComment;
        private string _filename = string.Empty;
        private TData _graphData;
        private string _GraphTypeName = "Graph";
        private System.Drawing.Printing.PageSettings _pageSettings;
        private System.Drawing.Printing.PrinterSettings _printerSettings;
        private string _settingsSection;
        private bool _ShowingPlotCursor;
        private SolidBrush backgroundBrush;
        private SolidBrush brushGraph;
        private Cursor closedHandCursor;
        private object copyToScreenLock;
        private bool drawAgain = false;
        private bool drawingComplete = false;
        private bool bIsPrintingAxes = false;
        private object drawingLock;
        private BackgroundWorker drawingWorker;
        private object drawingWorkerLock;
        private Font fontAxes;
        private TTrace highlightedTrace;
        private Dictionary<TTrace, TPoint> lastPlotPoint;
        private int mouseDownX = -1, mouseMoveX = -1, mouseDownXRMB = -1;
        private int mouseDownY = -1, mouseMoveY = -1, mouseDownYRMB = -1;
        private Bitmap offScreenAxesBmp;
        private Graphics offscreenAxesDC;
        private Bitmap offScreenGraphBmp;
        private Bitmap offScreenGraphBmp1;
        private Graphics offscreenGraphDC;
        private Graphics offscreenGraphDC1;
        private Bitmap offscreenGridlinesBmp;
        private Graphics offscreenGridlinesDC;
        private Graphics onscreenAxesDC;
        private Graphics onscreenGraphDC;
        private Pen penAxes;
        private Pen penCallout;
        private Pen penGraph;
        private Pen penGridLines;
        private Pen penPlotCursor;
        private int plotCursorX;
        private bool redrawAgain = false;
        private bool resizing = false;
        private int plotCursorCalloutPrevYR, plotCursorCalloutPrevYL;
        private PlotPropForm _frmPlotProp1;
        private DataPlotControl _dataPlot;
        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">GraphControl</see>
        /// </summary>
        public GraphControl()
        {
            drawingLock = new object();
            copyToScreenLock = new object();
            drawingWorkerLock = new object();
            drawingWorker = new BackgroundWorker();
            drawingWorker.DoWork += new DoWorkEventHandler(drawingWorker_DoWork);
            drawingWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(drawingWorker_RunWorkerCompleted);

            lastPlotPoint = new Dictionary<TTrace, TPoint>();

            penGraph = new Pen(Color.Black, 1);
            brushGraph = new SolidBrush(Color.Black);
            penGridLines = new Pen(Color.FromArgb(220, 220, 220), 1);
            penCallout = new Pen(Color.FromArgb(180, Color.Black), 1);
            penAxes = new Pen(Color.Black, 2);
            fontAxes = new Font("Arial", 8);
            penPlotCursor = new Pen(Color.FromArgb(180, Color.Black), 2);
            closedHandCursor = new Cursor(
                Properties.Resources.closedhand2.GetHicon());
            backgroundBrush = new SolidBrush(SystemColors.Info);

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor,
                true);

            InitializeComponent();

            _graphData = new TData();
            _graphData.Traces = new TCollection();
            _graphData.Comments = new List<GraphComment>();
            _settingsSection = string.Format("{0}.Settings", this.GetType().ToString());
            _graphData.Settings = Activator.CreateInstance(typeof(TSettings), _settingsSection) as TSettings;
            _graphData.Settings.Load();
            InitGraphData();

            InitGraphics();
            ReDrawGraph();

            _pageSettings = new System.Drawing.Printing.PageSettings();
            _pageSettings.Landscape = true;
            _pageSettings.Margins.Left = 50;
            _pageSettings.Margins.Right = 50;
            _pageSettings.Margins.Top = 50;
            _pageSettings.Margins.Bottom = 50;
            _printerSettings = new System.Drawing.Printing.PrinterSettings();
            _pageSettings.PrinterSettings = _printerSettings;
            this.pageSetupDialog1.PageSettings = _pageSettings;
            this.printDialog1.PrinterSettings = _printerSettings;
            this.printDocument1.DefaultPageSettings = _pageSettings;
            this.printDocument1.PrinterSettings = _printerSettings;
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
        }

        #endregion Constructors

        #region Properties (18)

        /// <summary>
        /// Gets the lock object to be locked whenever the data is being modified.
        /// </summary>
        public object DrawingLock
        {
            get { return drawingLock; }
        }

        /// <summary>
        /// Gets or sets the filename associated with the graph.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// Gets or sets the data for the graph.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TData GraphData
        {
            get { return _graphData; }
            set
            {
                _graphData = value;
                InitGraphData();
            }
        }

        internal Size GraphSize
        {
            get { return panelGraph.Size; }
        }

        /// <summary>
        /// Gets or sets the name to be displayed at the top of the graph when printing.
        /// </summary>
        public string GraphTypeName
        {
            get
            {
                return _GraphTypeName;
            }
            set
            {
                _GraphTypeName = value;
            }
        }

        /// <summary>
        /// Gets or sets a header value to be displayed at the top-center of the graph when printing.
        /// </summary>
        public string Header { get; set; }

        public bool LegendForPrintedPlot { get; set; }

        /// <summary>
        /// Gets or sets a header value to be displayed at the top-left of the graph when printing.
        /// </summary>
        public string HeaderLeft { get; set; }

        /// <summary>
        /// Gets or sets a header value to be displayed at the top-right of the graph when printing.
        /// </summary>
        public string HeaderRight { get; set; }

        /// <summary>
        /// Gets or sets the page settings to be used when printing.
        /// </summary>
        public System.Drawing.Printing.PageSettings PageSettings { get { return _pageSettings; } }

        /// <summary>
        /// Gets the panel that contains the graphical data
        /// </summary>
        protected Panel PanelGraph
        {
            get { return panelGraph; }
        }

        /// <summary>
        /// Gets the current location in the X-Axis for the plot cursor
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float PlotCursorLocation
        {
            get { return ScreenXtoUser(plotCursorX); }
        }

        /// <summary>
        /// Gets or sets the Plot Name to be used when printing.
        /// </summary>
        public string PlotName { get; set; }

        /// <summary>
        /// Gets the printer settings to be used when printing.
        /// </summary>
        public System.Drawing.Printing.PrinterSettings PrinterSettings { get { return _printerSettings; } }

        /// <summary>
        /// Gets or sets the graph settings.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TSettings Settings
        {
            get { return _graphData.Settings; }
            set { _graphData.Settings = value; }
        }

        /// <summary>
        /// Gets or sets the name of the section in the user.config file to store the settings for this graph.
        /// </summary>
        public string SettingsSection
        {
            get { return _settingsSection; }
            set { _settingsSection = value; }
        }

        /// <summary>
        /// Gets a value to indicate whether or not the plot cursor is currently being shown.
        /// </summary>
        public bool ShowingPlotCursor
        {
            get { return _ShowingPlotCursor; }
        }

        /// <summary>
        /// Gets or sets the trace data for the graph.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TCollection Traces
        {
            get
            {
                return _graphData.Traces;
            }
            set
            {
                _graphData.Traces = value;
            }
        }

        /// <summary>
        /// Gets or sets the X-Axis label to be used when printing.
        /// </summary>
        public string XAxisLabel { get; set; }

        #endregion Properties

        #region Delegates and Events (1)

        #region Events (1)

        /// <summary>
        /// Occurs when the properties form should be shown.
        /// </summary>
        public event EventHandler ShowProperties;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (124)

        #region Public Methods (24)

        /// <summary>
        /// Adds a comment to the graph.
        /// </summary>
        /// <param name="text">Text of the comment</param>
        /// <param name="graphX">X coordinate of the location of the comment</param>
        /// <param name="graphY">Y coordinate of the location of the comment</param>
        /// <param name="offset">Offset from the location of the comment to the text box</param>
        /// <returns>The comment that was added.</returns>
        public GraphComment AddComment(String text, float graphX, float graphY, Point offset)
        {
            return AddComment(text, new PointF(graphX, graphY), offset);
        }

        /// <summary>
        /// Adds a comment to the graph
        /// </summary>
        /// <param name="text">Text of the comment</param>
        /// <param name="graphLocation">Location of the comment</param>
        /// <param name="offset">Offset from the location of the comment to the text box</param>
        /// <returns>The comment that was added.</returns>
        public GraphComment AddComment(String text, PointF graphLocation, Point offset)
        {
            return AddComment(text, graphLocation, offset, _graphData.Settings.CommentsVisible);
        }

        /// <summary>
        /// Adds a comment to the graph
        /// </summary>
        /// <param name="text">Text of the comment</param>
        /// <param name="graphLocation">Location of the comment</param>
        /// <param name="offset">Offset from the location of the comment to the text box</param>
        /// <param name="visible">Value to indicate whether or not the comment should be visible</param>
        /// <returns>The comment that was added.</returns>
        public GraphComment AddComment(String text, PointF graphLocation, Point offset, bool visible)
        {
            if (this.InvokeRequired)
                return
                    this.Invoke(new Func<string, PointF, Point, bool, GraphComment>(AddComment), text, graphLocation, offset, visible)
                        as GraphComment;
            else
            {
                GraphComment graphComment = new GraphComment();
                graphComment.Text = text;
                graphComment.Offset = offset;
                graphComment.Location = graphLocation;
                graphComment.Visible = visible;
                graphComment.CommentControl = AddCommentControl(graphComment);

                _graphData.Comments.Add(graphComment);

                return graphComment;
            }
        }

        /// <summary>
        /// Brings the trace to the front of the other traces.
        /// </summary>
        /// <param name="trace">Trace to bring to the front</param>
        public void BringTraceToFront(TTrace trace)
        {
            if (trace != null)
            {
                trace.ZOrder = Traces.Max(T => T.ZOrder) + 1;
            }
        }

        /// <summary>
        /// Clears the comments from the graph.
        /// </summary>
        public void ClearComments()
        {
            _graphData.Comments.ForEach(gc => panelGraph.Controls.Remove(gc.CommentControl));
            _graphData.Comments.Clear();
        }

        /// <summary>
        /// Draws the entire graph
        /// </summary>
        public virtual void DrawGraph()
        {
            lock (drawingWorkerLock)
            {
                if (drawingWorker.IsBusy) drawAgain = true;
                else drawingWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Opens the specified file.  The file must be of the compressed-XML type that
        /// is created by the <see cref="Save">Save</see> method.
        /// </summary>
        /// <param name="filename">Name of the file to open</param>
        public void Open(string filename)
        {
            _filename = filename;
            using (var filestream = File.OpenRead(filename))
            using (var zipstream = new GZipStream(filestream, CompressionMode.Decompress))
            {
                XmlSerializer x = new XmlSerializer(typeof(TData));
                GraphData =
                    x.Deserialize(zipstream) as TData;
            }
        }

        /// <summary>
        /// Creates overlays of all the traces on the graph (excluding other overlays)
        /// </summary>
        public void OverlayAllTraces()
        {
            Traces.Where(T => !T.IsOverlay).ForEach(T => OverlayTrace(T));
        }

        /// <summary>
        /// Opens the specified file and overlays all of the data onto the current graph.
        /// </summary>
        /// <param name="filename">Name of the file to be overlayed</param>
        public void OverlayFile(string filename)
        {
            TData overlayData = null;

            using (var filestream = File.OpenRead(filename))
                if (Path.GetExtension(filename).ToLower() == ".aiox")
                {
                    using (var zipstream = new GZipStream(filestream, CompressionMode.Decompress))
                    {
                        XmlSerializer x = new XmlSerializer(typeof(TData));
                        overlayData =
                            x.Deserialize(zipstream) as TData;
                    }
                    foreach (var trace in overlayData.Traces)
                        OverlayTrace(trace);
                    foreach (var comment in overlayData.Comments)
                        OverlayComment(comment);
                }
                else
                {
                    _dataPlot.LoadAioFile(filename);
                }
        }

        /// <summary>
        /// Creates a copy of the specified trace and overlays it on the current graph.
        /// </summary>
        /// <param name="key">Key of the trace to be copied</param>
        /// <returns>The overlay trace that was created.</returns>
        public TTrace OverlayTrace(string key)
        {
            if (Traces.Contains(key))
                return OverlayTrace(Traces[key]);
            else
                return null;
        }

        public void OverlayComment(GraphComment comment)
        {
            if (comment == null)
                return;
            PointF pf = new PointF(comment.Location.X, comment.Location.Y);
            Point pt = new Point(comment.Offset.X, comment.Offset.Y);
            AddComment(comment.Text, pf, pt);
            _graphData.Comments.Last().IsOverlay = true;
        }

        /// <summary>
        /// Creates a copy of the specified trace and overlays it on the current graph.
        /// </summary>
        /// <param name="trace">Trace to be copied</param>
        /// <returns>The overlay trace that was created</returns>
        public TTrace OverlayTrace(TTrace trace)
        {
            if (trace != null)
            {
                int overlayNum = 1;
                while (Traces.Contains(trace.Key + overlayNum.ToString()))
                    overlayNum++;
                string overlayKey = string.Format("{0}{1}", trace.Key, overlayNum);
                string overlayLabel = string.Format("{0} [{1}]", trace.Label, overlayNum);

                TTrace overlay = Activator.CreateInstance(typeof(TTrace), overlayKey, overlayLabel) as TTrace;

                overlay.IsOverlay = true;
                overlay.Color = DefaultTraceColors.NextColor;
                overlay.Format = trace.Format;
                overlay.Units = trace.Units;
                overlay.Points.AddRange(trace.Points);
                overlay.ZOrder = trace.ZOrder + 1;
                Traces.Add(overlay);
                ReDrawGraph();

                removeAllOverlaysToolStripMenuItem.Visible = true;
                toolStripMenuItemRemoveAllOverlaysLegend.Visible = true;

                return overlay;
            }
            else return null;
        }

        /// <summary>
        /// Shows the printing page setup form
        /// </summary>
        public void PageSetup()
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                printDialog1.PrinterSettings = pageSetupDialog1.PrinterSettings;
        }

        /// <summary>
        /// Prints the graph
        /// </summary>
        public void PrintGraph()
        {
            try
            {
                printDocument1.Print();
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteError(ex.Message + Environment.NewLine + ex.StackTrace);
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// Shows the print setup form in order to print the graph
        /// </summary>
        public void PrintWithDialog()
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        /// <summary>
        /// Redraws the entire graph
        /// </summary>
        public virtual void ReDrawGraph()
        {
            lock (drawingWorkerLock)
            {
                if (drawingWorker.IsBusy) redrawAgain = true;
                else
                {
                    foreach (var trace in Traces)
                        lastPlotPoint[trace] = null;

                    drawingWorker.RunWorkerAsync();
                    this.DrawAxes();
                    _graphData.Comments.ForEach(gc =>
                        gc.CommentControl.AnchorPoint =
                            new Point(
                                UserXtoScreen(gc.Location.X),
                                UserYtoScreen(gc.Location.Y)));
                }
            }
        }

        /// <summary>
        /// Removes all overlay traces from the graph
        /// </summary>
        public void RemoveAllOverlays()
        {
            Traces.Where(T => T.IsOverlay).ForEach(T => Traces.Remove(T));
            // remove any residual overlay comments left over from last time this method was called
            _graphData.Comments.Where(T => T.IsOverlay && T.Location.X == -1e10f).ForEach(T => _graphData.Comments.Remove(T));
            foreach (var graphComment in _graphData.Comments)
            {
                if (graphComment.IsOverlay)
                {
                    PointF pf = new PointF(-1e10f, graphComment.Location.Y);
                    graphComment.Location = pf;
                }
            }
            ReDrawGraph();
            removeAllOverlaysToolStripMenuItem.Visible = false;
            toolStripMenuItemRemoveAllOverlaysLegend.Visible = false;
        }

        /// <summary>
        /// Removes the specified comment control from the graph.
        /// </summary>
        /// <param name="comment">Comment control to be removed.</param>
        public void RemoveComment(CommentControl comment)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<CommentControl>(RemoveComment), comment);
            else
            {
                _graphData.Comments.RemoveAll(gc => gc.CommentControl == comment);
                panelGraph.Controls.Remove(comment);
                comment.Dispose();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Removes the specified overlay trace from the graph
        /// </summary>
        /// <param name="key">Key of the overlay trace to be removed</param>
        public void RemoveOverlay(string key)
        {
            if (Traces.Contains(key))
                RemoveOverlay(Traces[key]);
        }

        /// <summary>
        /// Removes the specified overlay trace from the graph
        /// </summary>
        /// <param name="overlay">Overlay trace to be removed</param>
        public void RemoveOverlay(TTrace overlay)
        {
            if (overlay != null && overlay.IsOverlay)
                Traces.Remove(overlay);

            if (Traces.Count(T => T.IsOverlay) == 0)
            {
                removeAllOverlaysToolStripMenuItem.Visible = false;
                toolStripMenuItemRemoveAllOverlaysLegend.Visible = false;
            }
        }

        /// <summary>
        /// Saves the data from the current graph, including the traces, overlays, and settings,
        /// to a compressed-XML file.
        /// </summary>
        /// <param name="filename">Name of the file to be saved.</param>
        public void Save(string filename)
        {
            _filename = filename;
            FileInfo fileInfo = new FileInfo(filename);
            using (var filestream = File.OpenWrite(filename))
            using (var zipstream = new GZipStream(filestream, CompressionMode.Compress))
            {
                new XmlSerializer(typeof(TData))
                    .Serialize(zipstream, _graphData);
            }
        }

        /// <summary>
        /// Places the trace behind all other traces on the graph.
        /// </summary>
        /// <param name="Trace">Trace to be sent to the back</param>
        public void SendTraceToBack(TTrace Trace)
        {
            if (Trace != null)
            {
                Trace.ZOrder = Traces.Min(T => T.ZOrder) - 1;
            }
        }

        /// <summary>
        /// Causes the graph to calculate new X-Axis values that will result in
        /// all of the data from the graph being displayed.
        /// </summary>
        public void ShowAllOfGraph()
        {
            float lastPlotTime;

            if (Traces.Count == 0 || Traces[0].Points.Count == 0) return;

            lastPlotTime = Traces[0].Points.Last().X / Settings.XAxisScaleFactor;
            Settings.XMin = 0;
            if (lastPlotTime > Settings.XMax)
            {
                if ((lastPlotTime - Settings.XMin) < 100)
                    Settings.XMax = Settings.XMin + (float)Math.Round((lastPlotTime - Settings.XMin) / 10 + 1) * 10;
                else if ((lastPlotTime - Settings.XMin) < 1000)
                    Settings.XMax = Settings.XMin + (float)Math.Round((lastPlotTime - Settings.XMin) / 100 + 1) * 100;
                else
                    Settings.XMax = Settings.XMin + (float)Math.Round((lastPlotTime - Settings.XMin) / 1000 + 1) * 1000;
            }
            ReDrawGraph();
        }

        /// <summary>
        /// Causes the graph to calculate new X-Axis values that will result in
        /// only the last part of the data being displayed.
        /// </summary>
        /// <param name="XWindow">Width of the window of data to be displayed.</param>
        /// <param name="ShiftPercent">Percent of the width of the graph to shift each time.</param>
        public void ShowEndOfGraph(float XWindow, float ShiftPercent)
        {
            //lock (offscreenGraphDC)
            {
                float lastPlotTime;

                if (Traces.Count == 0 || Traces[0].Points.Count == 0) return;

                lastPlotTime = Traces[0].Points.Last().X / Settings.XAxisScaleFactor;

                if (lastPlotTime > XWindow)
                {
                    if (lastPlotTime < Settings.XMax)
                    {
                        if (ShiftPercent == 0)
                            Settings.XMax = lastPlotTime;
                        else
                            Settings.XMax = (float)Math.Floor(lastPlotTime / (ShiftPercent / 100.0F)) * (ShiftPercent / 100.0F) + (XWindow * (ShiftPercent / 100.0F));
                        Settings.XMin = Settings.XMax - XWindow;
                        ReDrawGraph();
                    }
                    else
                    {
                        float OldXMax = Settings.XMax;
                        float OldXMin = Settings.XMin;
                        float dx;
                        if (ShiftPercent == 0)
                            dx = (float)Math.Ceiling(((lastPlotTime - Settings.XMax) / XWindow) * (float)panelGraph.Width);
                        else
                            dx = (float)Math.Ceiling((ShiftPercent / 100.0F) * (float)panelGraph.Width);

                        Settings.XMax += dx * XWindow / (float)panelGraph.Width;
                        Settings.XMin = Settings.XMax - XWindow;

                        // If we were already showing the right interval, translate it left and just draw the end data
                        if ((OldXMax - OldXMin == XWindow) && (dx < (0.1 * (float)panelGraph.Width)))
                        {
                            try
                            {
                                // Win32 BitBlt - runs about 15% cpu utilization
                                IntPtr hMemdc = offscreenGraphDC.GetHdc();
                                Win32Support.BitBlt(hMemdc, 0, 0, offScreenGraphBmp.Width, offScreenGraphBmp.Height,
                                    hMemdc, (int)dx, 0, Win32Support.TernaryRasterOperations.SRCCOPY);
                                offscreenGraphDC.ReleaseHdc(hMemdc);

                                DrawGraph();
                                DrawAxes();
                            }
                            catch (Exception ex)
                            {
                                VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            }
                        }
                        else
                            ReDrawGraph();
                    }
                }
                else
                {
                    Settings.XMin = 0;
                    Settings.XMax = XWindow;
                    ReDrawGraph();
                }
            }
        }

        /// <summary>
        /// Initializes the <see cref="GraphData">GraphData</see>
        /// object.  Used to add event handlers and initialize settings after the graph data
        /// has been retrieved from a file.
        /// </summary>
        protected virtual void InitGraphData()
        {
            if (string.IsNullOrEmpty(_graphData.Settings.SectionName))
                _graphData.Settings.SectionName = _settingsSection;

            _graphData.Traces.Changed += new EventHandler<TraceCollectionChangedEventArgs<TTrace, TPoint>>(Traces_Changed);
            _graphData.Settings.Changed += new EventHandler<GraphSettings.GraphSettingChangeEventArgs>(GraphSettings_Changed);
            panelGraph.Controls.Clear();
            _graphData.Comments.ForEach(gc => gc.CommentControl = AddCommentControl(gc));

            flowLayoutPanelLegend.Controls.Clear();
            _graphData.Traces.ForEach(t => SetupNewTrace(t));

            if (_graphData.Settings.LegendEnabled)
            {
                splitContainer1.Panel1Collapsed = !_graphData.Settings.LegendVisible;
                toolStripMenuItemShowLegend.Visible = true;
                toolStripSeparatorLegend.Visible = true;
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
                toolStripMenuItemShowLegend.Visible = false;
                toolStripSeparatorLegend.Visible = false;
            }

            toolStripMenuItemShowLegend.Checked = _graphData.Settings.LegendVisible;
            toolStripMenuItemShowLegend2.Checked = _graphData.Settings.LegendVisible;
            toolStripMenuItemShowLegendLegend.Checked = _graphData.Settings.LegendVisible;

            splitContainer1.SplitterDistance = _graphData.Settings.LegendWidth;
        }

        /// <summary>
        /// Called when the mouse is double-clicked in the graph panel.  Can be used
        /// by sub-classes to customize the double-click action.
        /// </summary>
        protected virtual void OnGraphDoubleClick()
        {
            OnShowProperties();
        }

        /// <summary>
        /// Called when the control is creating the off-screen bitmap just prior to painting
        /// it to the screen.  Sub-classes can use this to draw to the bitmap to add custom
        /// graphics.
        /// </summary>
        /// <param name="g"></param>
        protected virtual void OnPaintGraph(Graphics g)
        {
        }

        /// <summary>
        /// Raises the <see cref="ShowProperties">ShowProperties</see> event.
        /// </summary>
        protected virtual void OnShowProperties()
        {
            if (ShowProperties != null)
                ShowProperties(this, EventArgs.Empty);
        }

        /// <summary>
        /// Causes the control to copy the image from the off-screen bitmap to the screen.
        /// </summary>
        public void PaintGraph()
        {
            lock (copyToScreenLock)
            {
                using (Graphics clientDC = this.panelGraph.CreateGraphics())
                {
                    //Graphics clientDC = this.panelGraph.CreateGraphics();
                    IntPtr hdc;
                    IntPtr hMemdc1;
                    hMemdc1 = offscreenGraphDC1.GetHdc();
                    if (Monitor.TryEnter(drawingLock))
                    {
                        try
                        {
                            IntPtr hMemdc = IntPtr.Zero;
                            IntPtr hMemdcGridlines = IntPtr.Zero;

                            if (drawingComplete)
                            {
                                // BitBlt from gridlines to second bitmap
                                hMemdcGridlines = offscreenGridlinesDC.GetHdc();
                                Win32Support.BitBlt(hMemdc1, 0, 0, (int)((Double)this.panelGraph.Width * 1.1), this.panelGraph.Height,
                                    hMemdcGridlines, 0, 0, Win32Support.TernaryRasterOperations.SRCCOPY);
                                //var y = hMemdcGridlines.ToInt32();
                                //if (y > 0)
                                //{
                                    //100737879 not valid
                                    //1107364676 not valid
                                    //-738122403 is valid
                                    //906042295 is valid
                                //}
                                offscreenGridlinesDC.ReleaseHdc(hMemdcGridlines);

                                // BitBlt from primary bitmap to second bitmap
                                hMemdc = offscreenGraphDC.GetHdc();
                                Win32Support.BitBlt(hMemdc1, 0, 0, (int)((Double)this.panelGraph.Width * 1.1), this.panelGraph.Height,
                                    hMemdc, 0, 0, Win32Support.TernaryRasterOperations.SRCAND);
                                //var z = hMemdc.ToInt32();
                                offscreenGraphDC.ReleaseHdc(hMemdc);
                                try
                                {
                                    //var t = hMemdc1.ToInt32(); //-1358882061 is invalid
                                                               //-1375656584 is invalid
                                                               //-301915148 is invalid
                                                               //-1644094510 is invalid
                                    //if (t < 0)
                                    //{
                                    //}
                                    offscreenGraphDC1.ReleaseHdc(hMemdc1);
                                }
                                catch (Exception ex)
                                {
                                    VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                }

                                OnPaintGraph(offscreenGraphDC1);

                                if (_ShowingPlotCursor)
                                {
                                    DrawPlotCursor();
                                }
                                hMemdc1 = offscreenGraphDC1.GetHdc();
                            }
                        }
                        catch (Exception ex)
                        {
                            VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                        finally
                        {
                            Monitor.Exit(drawingLock);
                        }
                    }

                    // BitBlt from second bitmap to screen
                    hdc = clientDC.GetHdc();
                    Win32Support.BitBlt(hdc, 0, 0, (int)((Double)this.panelGraph.Width * 1.1), this.panelGraph.Height,
                        hMemdc1, 0, 0, Win32Support.TernaryRasterOperations.SRCCOPY);
                    clientDC.ReleaseHdc(hdc);
                    offscreenGraphDC1.ReleaseHdc(hMemdc1);
                    //System.Runtime.InteropServices.Marshal.FreeHGlobal(hMemdc1);
                }
            }
        }

        /// <summary>
        /// Scales a point from screen coordinates to the coordinates of the data
        /// </summary>
        /// <param name="ScreenPoint">Point relative to the graph panel</param>
        /// <returns>Point relative to the data</returns>
        protected PointF ScreenPointToDataPoint(Point ScreenPoint)
        {
            return new PointF(ScreenXtoUser(ScreenPoint.X), ScreenYtoUser(ScreenPoint.Y));
        }

        /// <summary>
        /// Scales the given X value from screen coordinates to user coordinates.
        /// </summary>
        /// <param name="ScreenXCoord">X coordinate in the Graph window</param>
        /// <returns>X value in user coordinates</returns>
        protected float ScreenXtoUser(int ScreenXCoord)
        {
            float xScaleFactor;
            int maxX;

            maxX = panelGraph.Width;
            if ((ScreenXCoord > maxX) || (ScreenXCoord < 0)) return -1;
            xScaleFactor = (float)maxX / (Settings.XMax - Settings.XMin);
            return (ScreenXCoord / xScaleFactor + Settings.XMin) * (float)Settings.XAxisScaleFactor;
        }

        /// <summary>
        /// Scales the given Y value from screen coordinates to user coordinates.
        /// </summary>
        /// <param name="ScreenYCoord">Y coordinate in the Graph window</param>
        /// <returns>Y value in user coordinates</returns>
        protected float ScreenYtoUser(int ScreenYCoord)
        {
            float yScaleFactor;
            int maxY;

            maxY = panelGraph.Height;

            if ((ScreenYCoord > maxY) || (ScreenYCoord < 0)) return -1;

            if (Settings.PlotSemiLog)
            {
                return (float)(Math.Pow(10, ((float)maxY - ScreenYCoord) /
                                     ((float)maxY /
                                      ((float)Settings.YMaxExp - (float)Settings.YMinExp))) *
                                     Math.Pow(10, Settings.YMinExp));
            }
            else
            {
                yScaleFactor = maxY / (Settings.YMax - Settings.YMin);
                return ((maxY - ScreenYCoord) / yScaleFactor + Settings.YMin);
            }
        }

        /// <summary>
        /// Scales the given X value from user coordinates to screen coordinates.
        /// </summary>
        /// <param name="UserXCoord">X coordinate relative to the data</param>
        /// <returns>X coordinate relative to the graph panel</returns>
        protected int UserXtoScreen(float UserXCoord)
        {
            return UserXtoScreen(UserXCoord, panelGraph.Bounds);
        }

        /// <summary>
        /// Scales the given X value from user coordinates to the bound of the specified rectangle.
        /// </summary>
        /// <param name="UserXCoord">X coordinate relative to the data</param>
        /// <param name="Bounds">Rectangle to calculate the relative coordinate</param>
        /// <returns>X coordinate relative to the rectangle</returns>
        protected int UserXtoScreen(float UserXCoord, Rectangle Bounds)
        {
            float xScaleFactor;

            xScaleFactor = Bounds.Width / (Settings.XMax - Settings.XMin);

            return (int)((UserXCoord / Settings.XAxisScaleFactor - Settings.XMin) * xScaleFactor);
        }

        /// <summary>
        /// Scales the given Y value from user coordinates to screen coordinates.
        /// </summary>
        /// <param name="UserYCoord">Y coordinate relative to the data</param>
        /// <returns>Y coordinate relative to the graph panel</returns>
        protected int UserYtoScreen(float UserYCoord)
        {
            return UserYtoScreen(UserYCoord, panelGraph.Bounds);
        }

        /// <summary>
        /// Scales the given Y value from user coordinates to the bound of the specified rectangle.
        /// </summary>
        /// <param name="UserYCoord">Y coordinate relative to the data</param>
        /// <param name="Bounds">Rectangle to calculate the relative coordinate</param>
        /// <returns>X coordinate relative to the rectangle</returns>
        protected int UserYtoScreen(float UserYCoord, Rectangle Bounds)
        {
            float yScaleFactor;
            int maxY;

            maxY = Bounds.Height;

            if (Settings.PlotSemiLog)
            {
                return (int)(maxY - Math.Log10(UserYCoord / Math.Pow(10, Settings.YMinExp)) * ((float)maxY / (float)(Settings.YMaxExp - Settings.YMinExp)));
            }
            else
            {
                yScaleFactor = maxY / (Settings.YMax - Settings.YMin);
                return (int)(maxY - (UserYCoord - Settings.YMin) * yScaleFactor);
            }
        }

        #endregion Protected Methods
        #region Private Methods (87)

        private CommentControl AddCommentControl(GraphComment graphComment)
        {
            if (this.InvokeRequired)
                return
                    this.Invoke(new Func<GraphComment, CommentControl>(AddCommentControl), graphComment)
                        as CommentControl;
            else
            {
                CommentControl commentControl = new CommentControl();
                graphComment.CommentControl = commentControl;
                commentControl.Text = graphComment.Text;
                commentControl.Offset = graphComment.Offset;
                commentControl.AnchorPoint = new Point(
                    UserXtoScreen(graphComment.Location.X),
                    UserYtoScreen(graphComment.Location.Y));
                commentControl.CommentVisible = graphComment.Visible;

                commentControl.CalloutRightClicked += new EventHandler(Comment_CalloutRightClicked);
                commentControl.AnchorPointChanged += new EventHandler(Comment_AnchorPointChanged);
                commentControl.TextChanged += new EventHandler(commentControl_TextChanged);

                panelGraph.Controls.Add(commentControl);

                if (commentControl.Visible)
                {
                    commentControl.BringToFront();
                    commentControl.Focus();
                }

                return commentControl;
            }
        }

        public string GetShortestDistinctFormat(float yminF, float ymaxF, int ticks)
        {
            if (yminF == ymaxF)
            {
                return "0";
            }
            decimal ymin = Convert.ToDecimal(yminF);
            decimal ymax = Convert.ToDecimal(ymaxF);
            List<decimal> tickValues = new List<decimal>();
            List<string> tickLabels = new List<string>();
            for (int i = 0; i < ticks; i++)
            {
                tickValues.Add((((ymax - ymin) / ticks * i) + ymin));
            }
            double logYmax = Math.Truncate(Math.Log10(Convert.ToDouble(ymax)));
            double logYmin = Math.Truncate(Math.Log10(Convert.ToDouble(ymin)));
            if ((Math.Abs(ymax) < 0.01m && Math.Abs(ymin) < 0.01m)
                || (Math.Abs(ymax) > 10000 && Math.Abs(ymin) > 10000)
                || ((logYmax - logYmin) >= 3 && logYmax >=0 && logYmin >=0))
            {
                string format = "0E0";
                while ((tickLabels.Count != tickLabels.Distinct().Count() || tickLabels.Count == 0) && tickValues.Distinct().Count() != 1)
                {
                    tickLabels.Clear();
                    if (format.Contains("."))
                    {
                        format = format.Insert(format.IndexOf("E"), "0");
                    }
                    else
                    {
                        format = format.Insert(format.IndexOf("E"), ".0");
                    }
                    foreach (decimal val in tickValues)
                    {
                        tickLabels.Add(val.ToString(format));
                    }
                }
                return format;
            }
            else
            {
                string format = "0";
                bool firstTime = true;
                while ((tickLabels.Count != tickLabels.Distinct().Count() || tickLabels.Count == 0) && tickValues.Distinct().Count() != 1)
                {
                    tickLabels.Clear();
                    if (format.Contains("."))
                    {
                        format = format + "0";
                    }
                    else if (!firstTime)
                    {
                        format = format + ".0";
                    }
                    foreach (decimal val in tickValues)
                    {
                        tickLabels.Add(val.ToString(format));
                    }
                    firstTime = false;
                }
                return format;
            }
        }

        //modified 10-20-2020 by NJ
        public string AutoformatYValue(float yValue, string numericalFormat)
        {
            string formattedValue = yValue.ToString(numericalFormat);//(GetShortestDistinctFormat(Settings.YMin, Settings.YMax, 10));
            if (formattedValue.EndsWith("E0"))
            {
                formattedValue = formattedValue.Replace("E0", "");
            }
            if (yValue == Settings.YMax || yValue == Settings.YMin)
            {
                if (formattedValue.Contains("E") && formattedValue.Contains("."))
                {
                    while (formattedValue.Substring(formattedValue.IndexOf("E") - 1, 1) == "0")
                    {
                        //there is a zero before the "E"
                        formattedValue = formattedValue.Remove(formattedValue.IndexOf("E") - 1, 1);
                    }
                    if (formattedValue.Substring(formattedValue.IndexOf("E") - 1, 1) == ".")
                    {
                        //if there is a decimal point before the "E", remove it
                        formattedValue = formattedValue.Remove(formattedValue.IndexOf("E") - 1, 1);
                    }
                }
            }
            return formattedValue;

            #region old
            //string strTemp = yValue.ToString();
            //if ((Math.Abs(Settings.YMax) < 0.01) && (Math.Abs(Settings.YMin) < 0.01))
            //{
            //    strTemp = yValue.ToString("0.0E+0");
            //    if (strTemp.Contains(".0"))
            //    {
            //        if (yValue < 0)
            //        {
            //            strTemp = strTemp.Substring(0, 2) + strTemp.Substring(4, strTemp.Length - 4);
            //        }
            //        else
            //        {
            //            strTemp = strTemp.Substring(0, 1) + strTemp.Substring(3, strTemp.Length - 3);
            //            if (strTemp == "0E+0")
            //            {
            //                strTemp = "0";
            //            }
            //        }
            //    }
            //}
            //else if ((Math.Abs(Settings.YMax) < 0.1) && (Math.Abs(Settings.YMin) < 0.1))
            //{
            //    strTemp = yValue.ToString("0.0000");
            //}
            //else if ((Math.Abs(Settings.YMax) < 1) && (Math.Abs(Settings.YMin) < 1))
            //{
            //    strTemp = yValue.ToString("0.000");
            //}
            //else if ((Math.Abs(Settings.YMax) < 10) && (Math.Abs(Settings.YMin) < 10))
            //{
            //    strTemp = yValue.ToString("0.000");
            //}
            //else if ((Math.Abs(Settings.YMax) < 100) && (Math.Abs(Settings.YMin) < 100))
            //{
            //    if (Math.Abs(Settings.YMax - Settings.YMin) % 10 == 0)
            //    {
            //        strTemp = yValue.ToString("0");
            //    }
            //    else
            //    {
            //        List<string> t = new List<string>();
            //        for (int i = 0; i < 10; i++)
            //        {
            //            t.Add((((Settings.YMax - Settings.YMin) / 10 * i) + Settings.YMin).ToString("0.0"));
            //        }
            //        if (t.Count != t.Distinct().Count())
            //        {
            //            strTemp = yValue.ToString("0.00");
            //        }
            //        else
            //        {
            //            strTemp = yValue.ToString("0.0");
            //        }
            //    }
            //}
            //else if ((Math.Abs(Settings.YMax) < 1000) && (Math.Abs(Settings.YMin) < 1000))
            //{
            //    if (Math.Abs(Settings.YMax - Settings.YMin) < 100)
            //    {
            //        strTemp = yValue.ToString("0.0");
            //    }
            //    else
            //    {
            //        strTemp = yValue.ToString("0");
            //    }
            //}
            //else
            //{
            //    strTemp = yValue.ToString("0.0E+0");
            //    if (strTemp.Contains(".0"))
            //    {
            //        if (yValue < 0)
            //        {
            //            strTemp = strTemp.Substring(0, 2) + strTemp.Substring(4, strTemp.Length - 4);
            //        }
            //        else
            //        {
            //            strTemp = strTemp.Substring(0, 1) + strTemp.Substring(3, strTemp.Length - 3);
            //        }
            //    }
            //}
            //while (strTemp != "0" && strTemp[strTemp.Length - 1] == '0' && ((yValue >= 0 && yValue < 10) || (yValue <= 0 && yValue > -10) || (strTemp[strTemp.Length - 2] == 'E')))
            //{
            //    strTemp = strTemp.Substring(0, strTemp.Length - 1);
            //}
            //if (strTemp[strTemp.Length - 1] == '.')
            //{
            //    strTemp = strTemp.Replace(".", "");
            //}
            //if (strTemp[strTemp.Length - 1] == '+')
            //{
            //    strTemp = strTemp.Replace("+", "");
            //}
            //if (strTemp[strTemp.Length - 1] == '-')
            //{
            //    strTemp = strTemp.Replace("-", "");
            //}
            //if (strTemp[strTemp.Length - 1] == 'E')
            //{
            //    strTemp = strTemp.Replace("E", "");
            //}
            //return strTemp;
            #endregion old
        }

        private string AutoformatYValue2(float yValue)
        {
            string strTemp;

            if (Settings.PlotSemiLog || (Math.Abs(Settings.YMax) < 0.01) && (Math.Abs(Settings.YMin) < 0.01))
                strTemp = yValue.ToString("0.00E+00");
            else if ((Math.Abs(Settings.YMax) < 0.1) && (Math.Abs(Settings.YMin) < 0.1))
                strTemp = yValue.ToString("0.00000");
            else if ((Math.Abs(Settings.YMax) < 1) && (Math.Abs(Settings.YMin) < 1))
                strTemp = yValue.ToString("0.0000");
            else if ((Math.Abs(Settings.YMax) < 10) && (Math.Abs(Settings.YMin) < 10))
                strTemp = yValue.ToString("0.0000");
            else if ((Math.Abs(Settings.YMax) < 100) && (Math.Abs(Settings.YMin) < 100))
                strTemp = yValue.ToString("0.000");
            else if ((Math.Abs(Settings.YMax) < 1000) && (Math.Abs(Settings.YMin) < 1000))
                if (Math.Abs(Settings.YMax - Settings.YMin) < 100)
                    strTemp = yValue.ToString("0.000");
                else
                    strTemp = yValue.ToString("0.00");
            else
                strTemp = yValue.ToString("0.00E+00");
            return strTemp;
        }

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentComment.BringToFront();
        }

        private void bringTraceToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BringTraceToFront(highlightedTrace);
        }

        private void Comment_AnchorPointChanged(object sender, EventArgs e)
        {
            CommentControl comment = (CommentControl)sender;
            if (comment.AnchorPoint.X >= 0 && comment.AnchorPoint.X < this.panelGraph.Width && comment.UserChanging)
            {
                GraphComment graphComment = _graphData.Comments.FirstOrDefault(gc => gc.CommentControl == comment);
                if (graphComment != null)
                    graphComment.Location = new PointF(
                        ScreenXtoUser(comment.AnchorPoint.X),
                        ScreenYtoUser(comment.AnchorPoint.Y));
            }
        }

        private void Comment_CalloutRightClicked(object sender, EventArgs e)
        {
            _currentComment = (CommentControl)sender;
            showCommentToolStripMenuItem.Visible = !_currentComment.CommentVisible;
            hideCommentToolStripMenuItem.Visible = _currentComment.CommentVisible;
            contextMenuStripComments.Show(Cursor.Position);
        }

        private void commentControl_TextChanged(object sender, EventArgs e)
        {
            CommentControl commentControl = sender as CommentControl;
            GraphComment graphComment = _graphData.Comments.FirstOrDefault(gc => gc.CommentControl == commentControl);
            if (graphComment != null)
                graphComment.Text = commentControl.Text;
        }

        private void commentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!toolStripMenuItemDisplayComments.Checked)
            {
                toolStripMenuItemDisplayComments.Checked = true;
                Settings.CommentsVisible = true;
                foreach (var comment in _graphData.Comments)
                {
                    comment.Visible = comment.CommentControl.Visible = Settings.CommentsVisible;
                }
            }
            AddComment(
                string.Empty,
                new PointF(
                        ScreenXtoUser(mouseDownXRMB),
                        ScreenYtoUser(mouseDownYRMB)),
                new Point(50, -50));
        }

        private void deleteCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentComment != null)
            {
                _graphData.Comments.RemoveAll(gc => gc.CommentControl == _currentComment);
                panelGraph.Controls.Remove(_currentComment);
                _currentComment.Dispose();
                _currentComment = null;
            }
        }

        /// <summary>
        /// Draws the X and Y Axes, tic marks, and labels
        /// </summary>
        public virtual void DrawAxes()
        {
            if (offscreenAxesDC != null)
            {
                try
                {
                    offscreenAxesDC.Clear(System.Drawing.SystemColors.Control);
                    DrawAxes(this.offscreenAxesDC, this.panelGraph.Bounds);
                    if (_ShowingPlotCursor)
                        DrawPlotCursorAxisLabel(offscreenAxesDC,
                            new Point(panelGraph.Left + plotCursorX + 1, panelGraph.Bottom + 8),
                            ScreenXtoUser(plotCursorX));
                    onscreenAxesDC.DrawImage(this.offScreenAxesBmp, 0, 0);
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
                    VtiEvent.Log.WriteVerbose(exp.Message + Environment.NewLine + exp.StackTrace);
                    InitGraphics();
                }
            }
        }

        /// <summary>
        /// DrawAxes
        ///
        /// Generalized to draw either on-screen or to the printer
        /// </summary>
        /// <param name="g">Graphics to be drawn on</param>
        /// <param name="GraphBounds">Rectangle that represents the bounds of the graph drawing area</param>
        protected virtual void DrawAxes(Graphics g, Rectangle GraphBounds)
        {
            int i, NumTicLabels, NumberOfTics;
            float TicSpacing, tempFloat;
            float TicValue, TicInterval, TicLabelInterval;
            float ltr, tr, llr, lr;
            float xsf;
            Point P1 = new Point(), P2 = new Point();
            string formattedYValueString;
            int TicSize = 10;
            int labelWidth = 35;

            // Draw the X and Y axes
            g.DrawLine(penAxes, GraphBounds.Left, GraphBounds.Top, GraphBounds.Left, GraphBounds.Bottom);
            g.DrawLine(penAxes, GraphBounds.Left - 1, GraphBounds.Bottom, GraphBounds.Right + 1, GraphBounds.Bottom);

            //*************************************************************************************
            // Y-Axis Tic Marks and Labels

            // Calculate the number of tic marks
            if (Settings.PlotSemiLog)
                NumberOfTics = (-Settings.YMinExp + Settings.YMaxExp);
            else
                NumberOfTics = 10;
            if (NumberOfTics == 0) NumberOfTics = 1;
            TicSpacing = (float)GraphBounds.Height / (float)NumberOfTics;

            // Draw the Y axis tic marks and labels
            penAxes.Width = 1;
            string shortestNumericalFormat = GetShortestDistinctFormat(Settings.YMin, Settings.YMax, NumberOfTics);
            for (i = 0; i <= NumberOfTics; i++)
            {
                P1.X = GraphBounds.Left;
                P2.X = GraphBounds.Left - TicSize;
                if (i < NumberOfTics)
                    P2.Y = P1.Y = GraphBounds.Bottom - (int)(Double)(i * TicSpacing);
                else
                    P2.Y = P1.Y = GraphBounds.Top;    // this keeps the top tick from jittering around the top of the chart

                g.DrawLine(penAxes, P1, P2);

                if (Settings.PlotSemiLog)
                {
                    formattedYValueString = String.Format("1 E{0:00}", (Settings.YMinExp + i));
                }
                else
                {
                    //gets y-axis tick value
                    tempFloat = ((Settings.YMax - Settings.YMin) / 10 * i) + Settings.YMin;
                    if (tempFloat < 1E-20f)
                    {
                        tempFloat = 1E-20f;
                    }
                    //gets format of every y-axis tick mark label (0.0, 0.00E0, etc.)
                    formattedYValueString = AutoformatYValue(tempFloat, shortestNumericalFormat);
                }
                //measure the size of the tick value label and then draw it with the appropriate offset from the edge of the plot
                labelWidth = (int)(g.MeasureString(formattedYValueString, fontAxes).Width);
                P1.X -= labelWidth + TicSize;
                P1.Y -= (int)(g.MeasureString(formattedYValueString, fontAxes).Height) / 2;
                g.DrawString(formattedYValueString, fontAxes, Brushes.Black, P1);
            }
            transparentPanelYAxis.Width = labelWidth - 5;

            //*************************************************************************************
            // X-Axis Tic Marks and Labels
            float fSecs = 1.0f, fRange;
            if (_frmPlotProp1.RadioButtonMinutes.Checked)
                fSecs = 60.0f;
            fRange = (Settings.XMax - Settings.XMin) / fSecs;
            if (fRange < 10)
            {
                NumTicLabels = Convert.ToInt32(fRange);
                NumberOfTics = Convert.ToInt32(fRange);
                TicLabelInterval = 1;
                TicInterval = 1;
            }
            else if (fRange <= 50)
            {
                //NumTicLabels = 10;
                NumTicLabels = Convert.ToInt32(fRange) / 5;  // Labels on the 5's
                NumberOfTics = Convert.ToInt32(fRange);      // Tics on the 1's
                TicLabelInterval = 5;
                TicInterval = 1;
            }
            else if (fRange <= 150)
            {
                //NumTicLabels = 10;
                NumTicLabels = Convert.ToInt32(fRange) / 10; // Labels on the 10's
                NumberOfTics = Convert.ToInt32(fRange);      // Tics on the 1's
                TicLabelInterval = 10;
                TicInterval = 1;
            }
            else if (fRange <= 300)
            {
                //NumTicLabels = 10;
                NumTicLabels = Convert.ToInt32(fRange) / 20; // Labels on the 10's
                NumberOfTics = Convert.ToInt32(fRange) / 2;      // Tics on the 2's
                TicLabelInterval = 20;
                TicInterval = 2;
            }
            else if (fRange <= 500)
            {
                NumTicLabels = Convert.ToInt32(fRange) / 50;  // Labels on the 50's
                NumberOfTics = Convert.ToInt32(fRange) / 10;  // Tics on the 10's
                TicLabelInterval = 50;
                TicInterval = 10;
            }
            else if (fRange <= 1000)
            {
                NumTicLabels = Convert.ToInt32(fRange) / 100; // Labels on the 100's
                NumberOfTics = Convert.ToInt32(fRange) / 10;  // Tics on the 10's
                TicLabelInterval = 100;
                TicInterval = 10;
            }
            else
            {
                NumTicLabels = 10;
                NumberOfTics = 100;
                TicInterval = Convert.ToInt32(Math.Floor((fRange) / 1000) * 1000 / (float)NumberOfTics);
                TicLabelInterval = Convert.ToInt32(Math.Floor((fRange) / 1000) * 1000 / (float)NumTicLabels);
            }
            if (NumberOfTics > 100) NumberOfTics = 100;
            if (NumberOfTics > 0)
            {
                TicSpacing = (float)GraphBounds.Width / (float)NumberOfTics;

                //xsf = ((float)Settings.XMax - (float)Settings.XMin) / (float)GraphBounds.Width;
                xsf = fRange / (float)GraphBounds.Width;
                //ltr = Math.Abs(Settings.XMin - xsf) % TicInterval;
                //llr = Math.Abs(Settings.XMin - xsf) % TicLabelInterval;
                ltr = Math.Abs(Settings.XMin / fSecs - xsf) % TicInterval;
                llr = Math.Abs(Settings.XMin / fSecs - xsf) % TicLabelInterval;

                for (i = 0; i <= GraphBounds.Width; i++)
                {
                    TicValue = (float)i * xsf + Settings.XMin / fSecs;
                    tr = TicValue % TicInterval;
                    lr = TicValue % TicLabelInterval;

                    // Draw Label & Tic Here
                    if (lr < llr)
                    {
                        P1.X = P2.X = GraphBounds.Left + i + 1;
                        P1.Y = GraphBounds.Bottom;
                        penAxes.Width = 2;
                        P2.Y = GraphBounds.Bottom + TicSize;
                        g.DrawLine(penAxes, P1, P2);

                        P1.Y += TicSize;
                        DrawXAxisTicLabel(g, P1, TicValue, TicLabelInterval);

                        i += (int)(TicSpacing * 0.8); // skip most of the blank space between tics
                    }
                    // Draw minor tic here
                    else if (tr < ltr)
                    {
                        P1.X = P2.X = GraphBounds.Left + i + 1;
                        P1.Y = GraphBounds.Bottom;
                        penAxes.Width = 1;
                        P2.Y = GraphBounds.Bottom + TicSize / 2;
                        g.DrawLine(penAxes, P1, P2);
                        i += (int)(TicSpacing * 0.8); // skip most of the blank space between tics
                    }
                    llr = lr;
                    ltr = tr;
                }
            }
            // draw units for X-axis
            if (!bIsPrintingAxes)
            {
                string strXAxisLabel = string.Format("Time ({0})", _frmPlotProp1.DataPlot.Settings.XAxisUnits.ToString());
                P1.X = (GraphBounds.Left + GraphBounds.Right) / 2 - (int)g.MeasureString(strXAxisLabel, fontAxes).Width / 2;
                P1.Y = GraphBounds.Bottom + 2 * TicSize;
                g.DrawString(strXAxisLabel, fontAxes, Brushes.Black, P1);
            }
        }

        /// <summary>
        /// Draws only the portion of the Data Plot graph specified by the DrawingRectangle.
        /// </summary>
        /// <param name="g">Graphics context</param>
        /// <param name="DCBounds">Bounds of the off-screen bitmap Device Context</param>
        /// <param name="GraphBounds">Bounds of the Graph</param>
        /// <param name="DrawingRect">Drawing rectangle specifying the portion of the Data Plot to draw</param>
        /// <param name="Printing">Value to indicate if the drawing is being done to a printer</param>
        protected virtual void DrawGraphRect(Graphics g, Rectangle DCBounds, Rectangle GraphBounds, Rectangle DrawingRect, Boolean Printing)
        {
            Point P1 = new Point(), P2 = new Point();
            float yRange, yScaleFactor, xScaleFactor;
            float factorA, factorB, factorC, factorD;
            bool largeTrace;
            int factorE;
            bool resizeMax, resizeMin;
            //try
            //{
            var TracesCopy = Traces;
            if (TracesCopy.Count > 0)
            {
                //lock (Traces)
                {
                    if (Settings.PlotSemiLog)
                    {
                        yRange = Settings.YMaxExp - Settings.YMinExp;
                        if (yRange == 0) yRange = 1;
                        yScaleFactor = GraphBounds.Height / yRange;
                    }
                    else
                    {
                        yRange = Settings.YMax - Settings.YMin;
                        if (yRange == 0) yRange = 1;
                        yScaleFactor = GraphBounds.Height / yRange;
                    }

                    //if (!Printing)
                    //{
                    //    if (!RecalledScan)
                    //    {
                    //        if (((PlotData[PlotData.Count - 1].PlotTime / (Double)Settings.XAxisUnits > Settings.XMax) ||
                    //             (PlotData[PlotData.Count - 1].PlotTime / (Double)Settings.XAxisUnits < Settings.XMin)) &&
                    //            (Settings.AutoShowAll) && (!ShowingPlotCursor))
                    //        {
                    //            this.ShowAllOfDataPlot();
                    //            return;
                    //        }
                    //        if (((PlotData[PlotData.Count - 1].PlotTime / (Double)Settings.XAxisUnits > Settings.XMax) ||
                    //             (PlotData[PlotData.Count - 1].PlotTime / (Double)Settings.XAxisUnits < Settings.XMin)) &&
                    //            (Settings.AutoShowEnd) && (!ShowingPlotCursor))
                    //        {
                    //            this.ShowEndOfDataPlot();
                    //            return;
                    //        }
                    //    }
                    //}

                    xScaleFactor = GraphBounds.Width / (Settings.XMax - Settings.XMin);

                    penGraph.Width = 1;
                    penGraph.EndCap = System.Drawing.Drawing2D.LineCap.Square;

                    factorA = (float)Math.Pow(10, Settings.YMinExp);
                    factorB = yScaleFactor / 2.30259F;

                    factorC = xScaleFactor / (float)Settings.XAxisScaleFactor;
                    factorD = Settings.XMin * xScaleFactor;

                    factorE = GraphBounds.Bottom + (int)(Settings.YMin * yScaleFactor);

                    //TempPlotIndex = LastPlotIndex;

                    resizeMax = resizeMin = false;

                    foreach (var trace in TracesCopy.Where(T => T.Visible).OrderBy(T => T.ZOrder))
                    {
                        penGraph.Color = trace.Color;
                        penGraph.Width = trace.LineWidth;
                        brushGraph.Color = trace.Color;
                        try
                        {
                            if (trace.Points.Count > 0)
                            {
                                TPoint firstPoint;

                                largeTrace = (trace.Points.Count > 10000);

                                //if (lastPlotIndex[trace.Key] == 0 || DrawingRect.Left != 0 || Printing)
                                if (lastPlotPoint[trace] == null || DrawingRect.Left != 0 || Printing || !TracesCopy[0].Visible)
                                {
                                    firstPoint = trace.Points.First(P => P.X >= ((float)DrawingRect.Left / xScaleFactor + Settings.XMin) * (float)Settings.XAxisScaleFactor);
                                }
                                else
                                {
                                    //firstPoint = trace.Points[lastPlotIndex[trace.Key]];
                                    firstPoint = lastPlotPoint[trace];
                                }

                                //if ((lastPlotIndex[trace.Key] == 0 || Printing) && trace.DisplayType == TraceDisplayType.Line)
                                if ((lastPlotPoint[trace] == null || Printing) && trace.DisplayType == TraceDisplayType.Line)
                                {
                                    P1.X = GraphBounds.Left;
                                }
                                else
                                {
                                    P1.X = GraphBounds.Left + (int)((firstPoint.X / (float)Settings.XAxisScaleFactor - Settings.XMin) * xScaleFactor);
                                }

                                // Calculate the Y coordinate
                                if (Settings.PlotSemiLog)
                                    if (firstPoint.Y > 0)
                                        P1.Y = GraphBounds.Bottom - (int)(Math.Log((firstPoint.Y + 0.000000000000001) / factorA) * factorB);
                                    else
                                        P1.Y = GraphBounds.Bottom;
                                else
                                    P1.Y = GraphBounds.Bottom - (int)((firstPoint.Y - Settings.YMin) * yScaleFactor);

                                // Flags to resize the Y Axis, and keep the Y coordinate on the graph
                                if (P1.Y <= GraphBounds.Top)
                                {
                                    resizeMax = true;
                                    if (P1.Y < GraphBounds.Top) P1.Y = GraphBounds.Top;
                                }
                                else if (P1.Y >= GraphBounds.Bottom)
                                {
                                    if (!Settings.PlotSemiLog || firstPoint.Y > 0) resizeMin = true;
                                    if (P1.Y > GraphBounds.Bottom) P1.Y = GraphBounds.Bottom;
                                }

                                // If drawing the complete graph, make the first line which starts from the left edge
                                //if ((lastPlotIndex[trace.Key] == 0 && trace.DisplayType == TraceDisplayType.Line) || Printing)
                                if ((lastPlotPoint[trace] == null && trace.DisplayType == TraceDisplayType.Line) || Printing)
                                {
                                    P2.X = GraphBounds.Left + (int)((firstPoint.X / (float)Settings.XAxisScaleFactor - Settings.XMin) * xScaleFactor);
                                    P2.Y = P1.Y;
                                    g.DrawLine(penGraph, P1, P2);
                                }
                                else
                                {
                                    // set P2 = P1 to start, so the bounds check works the first time in
                                    P2.X = P1.X;
                                    P2.Y = P1.Y;
                                }

                                // Draw log lines
                                if (Settings.PlotSemiLog)
                                {
                                    //for (j = trace.Points.IndexOf(firstPoint); (j < trace.Points.Count) && (P2.X < DCBounds.Right); j++)
                                    foreach (var point in trace.Points.Skip(trace.Points.IndexOf(firstPoint)))
                                    {
                                        try
                                        {
                                            //P2.X = GraphBounds.Left + (int)((trace.Points[j].X * factorC) - factorD);
                                            P2.X = GraphBounds.Left + (int)((point.X * factorC) - factorD);
                                            if (P2.X > DCBounds.Right) continue;
                                            // Skip drawing this point if it's a large trace and the x coordinate hasn't changed
                                            if (Printing || (!largeTrace) || (P2.X != P1.X))// && (P2.X < DCBounds.Right))
                                            {
                                                //if (trace.Points[j].Y > 0)
                                                if (point.Y > 0)
                                                    //P2.Y = GraphBounds.Bottom - (int)(Math.Log((trace.Points[j].Y + 0.000000000000001) / factorA) * factorB);
                                                    P2.Y = GraphBounds.Bottom - (int)(Math.Log((point.Y + 0.000000000000001) / factorA) * factorB);
                                                else
                                                    P2.Y = GraphBounds.Bottom;

                                                // Flags to resize the Y Axis, and keep the Y coordinate on the graph
                                                if (P2.Y <= GraphBounds.Top)
                                                {
                                                    resizeMax = true;
                                                    if (P2.Y < GraphBounds.Top) P2.Y = GraphBounds.Top;
                                                }
                                                else if (P2.Y >= GraphBounds.Bottom)
                                                {
                                                    //if (trace.Points[j].Y > 0) resizeMin = true;
                                                    if (point.Y > 0) resizeMin = true;
                                                    if (P2.Y > GraphBounds.Bottom) P2.Y = GraphBounds.Bottom;
                                                }

                                                DrawPoint(g, GraphBounds, trace, P1, P2);

                                                lastPlotPoint[trace] = point;

                                                P1.X = P2.X;
                                                P1.Y = P2.Y;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                                            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                        }
                                    }
                                    //j = trace.Points.Count;
                                }
                                // Draw "linear" lines
                                else
                                {
                                    //for (j = trace.Points.IndexOf(firstPoint) + 1; (j < trace.Points.Count) && (P2.X < DCBounds.Right); j++)
                                    foreach (var point in trace.Points.ToList().Skip(trace.Points.ToList().IndexOf(firstPoint))) //8/29/2022 added .ToList() to avoid InvalidOperationException due to collection being modified outside of this loop
                                    {
                                        try
                                        {
                                            //P2.X = GraphBounds.Left + (int)((trace.Points[j].X * factorC) - factorD);
                                            P2.X = GraphBounds.Left + (int)((point.X * factorC) - factorD);
                                            if (P2.X > DCBounds.Right) continue;
                                            // Skip drawing this point if it's a large trace and the x coordinate hasn't changed
                                            // This will do some funky stuff to tightly compressed data, but it seriously speeds things up
                                            // to the point that 10,000,000 datapoints are easy to handle.
                                            if (Printing || (!largeTrace) || (P2.X != P1.X))// && (P2.X < DCBounds.Right))
                                            {
                                                //P2.Y = factorE - (int)(trace.Points[j].Y * yScaleFactor);
                                                P2.Y = factorE - (int)(point.Y * yScaleFactor);

                                                // Flags to resize the Y Axis, and keep the Y coordinate on the graph
                                                if (P2.Y <= GraphBounds.Top)
                                                {
                                                    P2.Y = GraphBounds.Top;
                                                }
                                                else if (P2.Y >= GraphBounds.Bottom)
                                                {
                                                    P2.Y = GraphBounds.Bottom;
                                                }
                                                //g.DrawLine(penGraph, P1, P2);
                                                DrawPoint(g, GraphBounds, trace, P1, P2);

                                                lastPlotPoint[trace] = point;

                                                P1.X = P2.X;
                                                P1.Y = P2.Y;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                                            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                        }
                                    }
                                }

                                //lastPlotIndex[trace.Key] = j;
                                //lastPlotPoint[trace] = trace.Points.Last();
                            }
                        }
                        //catch (InvalidOperationException e)
                        //{
                        //    throw new Exception("Data changed while plotting.  Unable to continue.", e);
                        //}
                        catch (Exception ex)
                        {
                            VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            return;
                        }
                    }

                    if (Settings.PlotSemiLog || !Printing)
                    {
                        if (Settings.AutoScaleYMaxExp && resizeMax) Settings.YMaxExp++;
                        if (Settings.AutoScaleYMinExp & resizeMin) Settings.YMinExp--;
                        //if ((_AutoScaleYMaxExp && resizeMax) || (_AutoScaleYMinExp & resizeMin))
                        //    this.ReDrawGraph();
                    }
                }
            }
            //}
            //catch (Exception e)
            //{
            //    VtiEvent.Log.WriteError(e.ToString(), VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error);
            //}
        }

        /// <summary>
        /// Draws the horizontal grid lines on the graph
        /// </summary>
        protected virtual void DrawGridLines()
        {
            this.offscreenGridlinesDC.Clear(Color.White);
            DrawGridLines(this.offscreenGridlinesDC, new Rectangle(0, 0, this.panelGraph.Width, this.panelGraph.Height));
        }

        protected virtual void DrawGridLines(Graphics g, Rectangle GraphBounds)
        {
            int i, NumberOfTics;
            Double TicSpacing;
            Point P1 = new Point(), P2 = new Point();

            // Calculate the number of tic marks
            if (Settings.PlotSemiLog)
                NumberOfTics = (-Settings.YMinExp + Settings.YMaxExp);
            else
                NumberOfTics = 10;
            if (NumberOfTics == 0) NumberOfTics = 1;
            TicSpacing = (Double)GraphBounds.Height / (Double)NumberOfTics;

            P1.X = GraphBounds.Left;
            P2.X = GraphBounds.Right;
            penGridLines.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            for (i = 1; i < NumberOfTics; i++)
            {
                P1.Y = P2.Y = GraphBounds.Bottom - (int)(TicSpacing * i) - 1;
                g.DrawLine(penGridLines, P1, P2);
            }
        }

        private void drawingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (drawingLock)
            {
                if (this.offscreenGraphDC != null)
                {
                    try
                    {
                        drawingComplete = false;

                        // clear the offscreen graphics if drawing the whole plot
                        if (lastPlotPoint.Count == 0 || lastPlotPoint.First().Value == null) this.offscreenGraphDC.Clear(Color.White);

                        Actions.Retry(3,
                            delegate
                            {
                                this.DrawGraphRect(this.offscreenGraphDC,
                          new Rectangle(0, 0, this.offScreenGraphBmp.Width, this.offScreenGraphBmp.Height),
                          new Rectangle(0, 0, this.panelGraph.Width, this.panelGraph.Height),
                          new Rectangle(0, 0, this.panelGraph.Width, this.panelGraph.Height),
                          false);
                            });

                        this.DrawGridLines();
                        drawingComplete = true;
                        //this.SetFormCaption();
                        this.PaintGraph();
                    }
                    catch (Exception ex)
                    {
                        VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
            }
        }

        private void drawingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (drawAgain)
            {
                drawAgain = false;
                DrawGraph();
            }
            else if (redrawAgain)
            {
                redrawAgain = false;
                ReDrawGraph();
            }
        }

        protected virtual void DrawPlotCursor()
        {
            string sTemp;

            // Draw Plot Cursor
            offscreenGraphDC1.DrawLine(penPlotCursor, plotCursorX, 0, plotCursorX, offScreenGraphBmp1.Height);

            // Draw plot cursor call-outs
            if (Settings.DrawPlotCursorCallouts)
            {
                plotCursorCalloutPrevYR = 0;
                plotCursorCalloutPrevYL = 0;

                float plotCursorUserX = ScreenXtoUser(plotCursorX);

                // Call a virtual method to allow sub-classes to draw an initial callout
                OnDrawFirstPlotCursorCallout(plotCursorUserX);

                //// Find the nearest points on each side plot cursor and order the list by descending Y value
                //var cursorValues = Traces
                //    .Where(t => t.Visible && t.Points.Count > 0)
                //    .Select(
                //        t => t.Points
                //           .Select(
                //                p => new
                //                {
                //                    point = p,
                //                    distance = p.X - plotCursorUserX,
                //                    pt = new Point(UserXtoScreen(p.X), UserYtoScreen(p.Y)),
                //                    trace = t
                //                })
                //            .Where(p => p.distance < 0)
                //            .OrderBy(p => p.distance)
                //            .Last()
                //    )
                //    .Where(p => panelGraph.ClientRectangle.Contains(p.pt))
                //    .OrderByDescending(p => p.point.Y)
                //    .Select(a => new { trace = a.trace, left = a.point, right = a.trace.Points.FirstOrDefault(p => p.X > a.point.X) });

                //foreach (var cursorValue in cursorValues)
                //{
                //    // Interpolate the Y-Value between the two points
                //    float y;
                //    if (cursorValue.right == null)
                //        y = cursorValue.left.Y;
                //    else
                //        y = cursorValue.left.Y + (cursorValue.right.Y - cursorValue.left.Y) * (plotCursorUserX - cursorValue.left.X) / (cursorValue.right.X - cursorValue.left.X);

                var cursorValues = Traces.Where(t => t.Visible && t.Points.Count > 0)
                    .Select(t => new
                    {
                        trace = t,
                        y = t.Points.InterpolateNearestYValue(plotCursorUserX)
                    })
                    .Where(p => !float.IsNaN(p.y) && panelGraph.ClientRectangle.Contains(plotCursorX, UserYtoScreen(p.y)))
                    .OrderByDescending(p => p.y);

                foreach (var cursorValue in cursorValues)
                {
                    // Format the string
                    if (string.IsNullOrEmpty(cursorValue.trace.Format))
                    {
                        sTemp = string.Format("{0}: {1}",
                            cursorValue.trace.Label,
                            AutoformatYValue2(cursorValue.y));
                    }
                    else
                    {
                        sTemp = string.Format(
                            string.Format("{{0}}: {{1:{0}}} {{2}}", cursorValue.trace.Format),
                            cursorValue.trace.Label,
                            cursorValue.y,
                            cursorValue.trace.Units);
                    }

                    // Draw the callout
                    DrawPlotCursorCallout(sTemp, UserYtoScreen(cursorValue.y), cursorValue.trace.Color);
                }

                // Call a virtual method to allow sub-classes to draw a final callout
                OnDrawLastPlotCursorCallout(plotCursorUserX);
            }
        }

        /// <summary>
        /// Gets the value at plot cursor.
        /// </summary>
        /// <param name="traceKey">The trace key.</param>
        /// <returns></returns>
        public virtual float GetValueAtPlotCursor(string traceKey)
        {
            try
            {
                if (!Traces.Contains(traceKey))
                {
                    return 0;
                }
                TTrace trace = Traces[traceKey];
                if (trace == null) return 0;
                else
                {
                    float plotCursorUserX = ScreenXtoUser(plotCursorX);
                    float value = trace.Points.InterpolateNearestYValue(plotCursorUserX);
                    return value;
                }
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public void AssignPlotPropForm(PlotPropForm frmPlotProp1)
        {
            if (frmPlotProp1 != null)
                _frmPlotProp1 = frmPlotProp1;
        }

        /// <summary>
        /// Store a reference to the DataPlot object
        /// </summary>
        public void StoreDataPlot(DataPlotControl DataPlot)
        {
            _dataPlot = DataPlot;
        }

        /// <summary>
        /// Called when drawing the plot cursor prior to drawing any of the other callouts.
        /// Sub-classes can override this method to draw a custom plot cursor callout.
        /// </summary>
        /// <param name="x">Location of the plot cursor in the graph data</param>
        protected virtual void OnDrawFirstPlotCursorCallout(float x)
        {
        }

        /// <summary>
        /// Called when drawing the plot cursor after drawing all of the other callouts.
        /// Sub-classes can override this method to draw a custom plot cursor callout.
        /// </summary>
        /// <param name="x">Location of the plot cursor in the graph data</param>
        protected virtual void OnDrawLastPlotCursorCallout(float x)
        {
        }

        /// <summary>
        /// Draws the plot cursor callout.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="y">The y.</param>
        /// <param name="color">The color.</param>
        protected virtual void DrawPlotCursorCallout(string text, int y, Color color)
        {
            DrawPlotCursorCallout(text, y, color, true, true);
        }

        /// <summary>
        /// Draws a callout on the plot cursor.
        /// </summary>
        /// <param name="text">Text of the callout.</param>
        /// <param name="y">Vertical position of the callout.</param>
        /// <param name="color">Background color of the callout.</param>
        /// <param name="drawCalloutLine">Value indicating whether or not the callout line should be drawn.</param>
        /// <param name="preferRight">Value indicating whether or not the callout should have a right-hand preference.</param>
        protected virtual void DrawPlotCursorCallout(string text, int y, Color color, bool drawCalloutLine, bool preferRight)
        {
            SizeF size = offscreenGraphDC1.MeasureString(text, fontAxes);

            Rectangle rect;
            Point p1;
            Point p2;
            Point p3 = new Point();
            bool canGoR, canGoL, isR;

            p1 = new Point(plotCursorX, y);

            // determine if callout can fit left or right
            canGoR = (p1.X + 14 + size.Width < panelGraph.Width);
            canGoL = (p1.X - 14 - size.Width > 0);

            // if there is enough space, make the callout go up-right 5 pixels
            if (((canGoR && preferRight) || !canGoL) && (p1.Y - 9 > plotCursorCalloutPrevYR))// || plotCursorCalloutPrevYR == 0))
            {
                p3.Y = p1.Y - 5;
                isR = true;
                plotCursorCalloutPrevYR = p3.Y + (int)size.Height + 6;
            }
            else if (canGoL && (p1.Y - 9 > plotCursorCalloutPrevYL))// || plotCursorCalloutPrevYL == 0))
            {
                p3.Y = p1.Y - 5;
                isR = false;
                plotCursorCalloutPrevYL = p3.Y + (int)size.Height + 6;
            }
            // otherwise, make it fit
            else if (canGoR && (plotCursorCalloutPrevYR <= plotCursorCalloutPrevYL || !canGoL))
            {
                p3.Y = plotCursorCalloutPrevYR + 4;
                isR = true;
                plotCursorCalloutPrevYR = p3.Y + (int)size.Height + 6;
            }
            else
            {
                p3.Y = plotCursorCalloutPrevYL + 4;
                isR = false;
                plotCursorCalloutPrevYL = p3.Y + (int)size.Height + 6;
            }

            // if there is enough space, make the callout go to the right
            if (isR) // p1.X + 14 + size.Width < panelGraph.Width)
            {
                p3.X = p1.X + 10;
                p2 = new Point(p3.X, p3.Y + 2 + (int)size.Height / 2);
                rect = new Rectangle(p3,
                    new Size((int)size.Width + 4, (int)size.Height + 4));
            }
            // otherwise, make it go to the left
            else
            {
                p3.X = p1.X - 14 - (int)size.Width;
                p2 = new Point(p1.X - 10, p3.Y + 2 + (int)size.Height / 2);
                rect = new Rectangle(p3,
                    new Size((int)size.Width + 4, (int)size.Height + 4));
            }

            // draw the callout
            backgroundBrush.Color = Color.FromArgb(
                (255 - (color.R + color.G + color.B) / 3) / 3, color);
            offscreenGraphDC1.FillRectangle(backgroundBrush, rect);
            offscreenGraphDC1.DrawRectangle(penCallout, rect);
            if (drawCalloutLine) offscreenGraphDC1.DrawLine(penCallout, p1, p2);
            offscreenGraphDC1.DrawString(text, fontAxes, Brushes.Black, new PointF(p3.X + 2, p3.Y + 2));
        }

        private void DrawPlotCursorAxisLabel(Graphics G, Point TopCenterPoint, float GraphX)
        {
            string sTemp;

            if (FormatPlotCursorAxisLabel != null)
                sTemp = FormatPlotCursorAxisLabel(GraphX);
            else
                sTemp = string.Format("{0:0.00}", GraphX / (float)Settings.XAxisScaleFactor);

            // add DateTime in HH:mm:ss
            sTemp += Environment.NewLine + GetDateTimeOfPoint(GraphX).ToString("HH:mm:ss");

            SizeF strSize = offscreenAxesDC.MeasureString(sTemp, fontAxes);

            Rectangle rect =
                new Rectangle(
                    (int)(TopCenterPoint.X - strSize.Width / 2 - 2),
                    TopCenterPoint.Y,
                    (int)strSize.Width + 4, (int)strSize.Height + 4);

            backgroundBrush.Color = SystemColors.Info;
            G.FillRectangle(backgroundBrush, rect);
            G.DrawRectangle(penCallout, rect);
            G.DrawLine(penAxes, TopCenterPoint.X, TopCenterPoint.Y, TopCenterPoint.X, panelGraph.Bottom);
            G.DrawString(sTemp, fontAxes, Brushes.Black, new PointF(rect.Left + 2, rect.Top + 2));
        }

        private void DrawPoint(Graphics g, Rectangle graphBounds, TTrace trace, Point p1, Point p2)
        {
            switch (trace.DisplayType)
            {
                case TraceDisplayType.Line:
                    g.DrawLine(penGraph, p1, p2);
                    break;

                case TraceDisplayType.Fill:
                    g.FillPolygon(brushGraph, new Point[] {
                        new Point(p1.X-1, p1.Y), p2, new Point(p2.X, graphBounds.Bottom), new Point(p1.X - 1, graphBounds.Bottom) });
                    break;

                case TraceDisplayType.Histogram:
                    g.DrawLine(penGraph, p2.X, p2.Y, p2.X, graphBounds.Bottom);
                    break;

                case TraceDisplayType.Point:
                    g.DrawEllipse(penGraph,
                        new Rectangle(p2.X - (trace.PointSize / 2), p2.Y - (trace.PointSize / 2),
                            trace.PointSize, trace.PointSize));
                    break;

                case TraceDisplayType.Plus:
                    g.DrawLine(penGraph, p2.X - trace.PointSize, p2.Y, p2.X + trace.PointSize, p2.Y);
                    g.DrawLine(penGraph, p2.X, p2.Y - trace.PointSize, p2.X, p2.Y + trace.PointSize);
                    break;

                case TraceDisplayType.Cross:
                    g.DrawLine(penGraph,
                        p2.X - trace.PointSize, p2.Y - trace.PointSize,
                        p2.X + trace.PointSize, p2.Y + trace.PointSize);
                    g.DrawLine(penGraph,
                        p2.X - trace.PointSize, p2.Y + trace.PointSize,
                        p2.X + trace.PointSize, p2.Y - trace.PointSize);
                    break;
            }
        }

        private void DrawXAxisTicLabel(Graphics G, Point TopCenterPoint, float TicValue, float TicLabelInterval)
        {
            Point p1;
            string sTemp;

            if (FormatXAxisTicLabel != null)
                sTemp = FormatXAxisTicLabel(TicValue, TicLabelInterval);
            else
                sTemp = String.Format("{0:0}", Math.Floor(TicValue / TicLabelInterval) * TicLabelInterval);

            p1 = TopCenterPoint;
            p1.X -= (int)(G.MeasureString(sTemp, fontAxes).Width / 2);
            G.DrawString(sTemp, fontAxes, Brushes.Black, p1);
        }

        private void GraphSettings_Changed(object sender, GraphSettings.GraphSettingChangeEventArgs e)
        {
            switch (e.ChangeType)
            {
                case GraphSettings.GraphSettingChangeType.LegendEnabled:
                    if (_graphData.Settings.LegendEnabled)
                    {
                        splitContainer1.Panel1Collapsed = !_graphData.Settings.LegendVisible;
                        toolStripMenuItemShowLegend.Visible = true;
                        toolStripSeparatorLegend.Visible = true;
                    }
                    else
                    {
                        splitContainer1.Panel1Collapsed = true;
                        toolStripMenuItemShowLegend.Visible = false;
                        toolStripSeparatorLegend.Visible = false;
                    }
                    break;

                case GraphSettings.GraphSettingChangeType.LegendVisible:
                    splitContainer1.Panel1Collapsed = !_graphData.Settings.LegendVisible;
                    toolStripMenuItemShowLegend.Checked = _graphData.Settings.LegendVisible;
                    toolStripMenuItemShowLegend2.Checked = _graphData.Settings.LegendVisible;
                    toolStripMenuItemShowLegendLegend.Checked = _graphData.Settings.LegendVisible;
                    break;

                case GraphSettings.GraphSettingChangeType.YAxisButtonsVisible:
                    if (_graphData.Settings.YAxisButtonsVisible)
                    {
                        _frmPlotProp1.DataPlot.GraphControl.transparentPanelYAxis.Visible = true;
                        _frmPlotProp1.DataPlot.YMinExpUp.Visible = true;
                        _frmPlotProp1.DataPlot.YMinExpDn.Visible = true;
                        _frmPlotProp1.DataPlot.YMaxExpUp.Visible = true;
                        _frmPlotProp1.DataPlot.YMaxExpDn.Visible = true;
                        _frmPlotProp1.DataPlot.YMaxUp.Visible = true;
                        _frmPlotProp1.DataPlot.YMaxDn.Visible = true;
                        _frmPlotProp1.DataPlot.YMinUp.Visible = true;
                        _frmPlotProp1.DataPlot.YMinDn.Visible = true;
                        _frmPlotProp1.DataPlot.YMaxExpUpLbl.Visible = true;
                        _frmPlotProp1.DataPlot.YMinExpUpLbl.Visible = true;
                        _frmPlotProp1.DataPlot.YMinUpLbl.Visible = true;
                        _frmPlotProp1.DataPlot.YMaxUpLbl.Visible = true;
                    }
                    else
                    {
                        _frmPlotProp1.DataPlot.GraphControl.transparentPanelYAxis.Visible = false;
                        _frmPlotProp1.DataPlot.YMinExpUp.Visible = false;
                        _frmPlotProp1.DataPlot.YMinExpDn.Visible = false;
                        _frmPlotProp1.DataPlot.YMaxExpUp.Visible = false;
                        _frmPlotProp1.DataPlot.YMaxExpDn.Visible = false;
                        _frmPlotProp1.DataPlot.YMaxUp.Visible = false;
                        _frmPlotProp1.DataPlot.YMaxDn.Visible = false;
                        _frmPlotProp1.DataPlot.YMinUp.Visible = false;
                        _frmPlotProp1.DataPlot.YMinDn.Visible = false;
                        _frmPlotProp1.DataPlot.YMaxExpUpLbl.Visible = false;
                        _frmPlotProp1.DataPlot.YMinExpUpLbl.Visible = false;
                        _frmPlotProp1.DataPlot.YMinUpLbl.Visible = false;
                        _frmPlotProp1.DataPlot.YMaxUpLbl.Visible = false;
                    }
                    break;

                case GraphSettings.GraphSettingChangeType.LegendWidth:
                    splitContainer1.SplitterDistance = _graphData.Settings.LegendWidth;
                    break;
            }

            ReDrawGraph();
        }

        private void hideAllCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.CommentsVisible = false;
        }

        private void hideCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentComment != null)
            {
                _currentComment.CommentVisible = false;
            }
        }

        private void hideThisTraceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (highlightedTrace != null)
            {
                highlightedTrace.Visible = false;
            }
        }

        private void HideTrace(TTrace Trace)
        {
            if (Trace != null)
            {
                Trace.Visible = false;
            }
        }

        private void HighlightTrace(TTrace Trace)
        {
            if (highlightedTrace != null && highlightedTrace != Trace)
                RemoveTraceHighlight(highlightedTrace);

            Trace.LineWidth = 2;
            highlightedTrace = Trace;
            Label label = flowLayoutPanelLegend.Controls["label" + Trace.Key] as Label;
            label.Font = new Font(label.Font, FontStyle.Bold);

            if (Trace.IsOverlay)
            {
                overlayThisTracetoolStripMenuItem.Visible = false;
                hideThisTraceToolStripMenuItem.Visible = false;
                traceColorToolStripMenuItem.Visible = false;

                removeThisOverlayToolStripMenuItem.Visible = true;
                overlayColorToolStripMenuItem.Visible = true;

                toolStripMenuItemOverlayTraceLegend.Visible = false;
                toolStripMenuItemHideTraceLegend.Visible = false;
                toolStripMenuItemTraceColorLegend.Visible = false;

                toolStripMenuItemRemoveOverlayLegend.Visible = true;
                toolStripMenuItemOverlayColorLegend.Visible = true;
            }
            else
            {
                overlayThisTracetoolStripMenuItem.Visible = true;
                hideThisTraceToolStripMenuItem.Visible = true;
                traceColorToolStripMenuItem.Visible = true;

                removeThisOverlayToolStripMenuItem.Visible = false;
                overlayColorToolStripMenuItem.Visible = false;

                toolStripMenuItemOverlayTraceLegend.Visible = true;
                toolStripMenuItemHideTraceLegend.Visible = true;
                toolStripMenuItemTraceColorLegend.Visible = true;

                toolStripMenuItemRemoveOverlayLegend.Visible = false;
                toolStripMenuItemOverlayColorLegend.Visible = false;
            }

            bringTraceToFrontToolStripMenuItem.Visible = true;
            sendTraceToBackToolStripMenuItem.Visible = true;
            toolStripSeparatorTraceZOrder.Visible = true;

            toolStripMenuItemBringToFrontLegend.Visible = true;
            toolStripMenuItemSendToBackLegend.Visible = true;
            toolStripSeparatorTraceZOrderLegend.Visible = true;
        }

        private void InitGraphics()
        {
            try
            {

                // Initialize Win32 BitBlt

                // Offscreen Graph
                if (offScreenGraphBmp != null) offScreenGraphBmp.Dispose();
                offScreenGraphBmp = new Bitmap((int)(this.panelGraph.Width * 1.1), this.panelGraph.Height);
                Graphics clientDC = this.CreateGraphics();
                IntPtr hdc = clientDC.GetHdc();
                IntPtr memdc = Win32Support.CreateCompatibleDC(hdc);
                Win32Support.SelectObject(memdc, offScreenGraphBmp.GetHbitmap());
                offscreenGraphDC = Graphics.FromHdc(memdc);
                offscreenGraphDC.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                clientDC.ReleaseHdc(hdc);

                // 2nd Offscreen Graph (for Plot cursor)
                if (offScreenGraphBmp1 != null) offScreenGraphBmp1.Dispose();
                offScreenGraphBmp1 = new Bitmap((int)(this.panelGraph.Width * 1.1), this.panelGraph.Height);
                Graphics clientDC1 = this.CreateGraphics();
                IntPtr hdc1 = clientDC1.GetHdc();
                IntPtr memdc1 = Win32Support.CreateCompatibleDC(hdc1);
                Win32Support.SelectObject(memdc1, offScreenGraphBmp1.GetHbitmap());
                offscreenGraphDC1 = Graphics.FromHdc(memdc1);
                offscreenGraphDC1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                clientDC1.ReleaseHdc(hdc1);

                // 3rd Offscreen Graph (for Grid Lines)
                if (offscreenGridlinesBmp != null) offscreenGridlinesBmp.Dispose();
                offscreenGridlinesBmp = new Bitmap((int)(this.panelGraph.Width * 1.1), this.panelGraph.Height);
                Graphics clientDCGridlines = this.CreateGraphics();
                IntPtr hdcGridlines = clientDCGridlines.GetHdc();
                IntPtr memdcGridlines = Win32Support.CreateCompatibleDC(hdcGridlines);
                Win32Support.SelectObject(memdcGridlines, offscreenGridlinesBmp.GetHbitmap());
                offscreenGridlinesDC = Graphics.FromHdc(memdcGridlines);
                offscreenGridlinesDC.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                clientDCGridlines.ReleaseHdc(hdcGridlines);

                // Offscreen Axes
                if (offScreenAxesBmp != null)
                {
                    offScreenAxesBmp.Dispose();
                    offscreenAxesDC.Dispose();
                }
                if (offScreenAxesBmp != null) offScreenAxesBmp.Dispose();
                offScreenAxesBmp = new Bitmap(this.Width, this.Height);
                offscreenAxesDC = Graphics.FromImage(offScreenAxesBmp);

                // Onscreen Graph
                if (onscreenGraphDC != null) onscreenGraphDC.Dispose();
                onscreenGraphDC = this.panelGraph.CreateGraphics();
                onscreenGraphDC.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Onscreen Axes
                if (onscreenAxesDC != null) onscreenAxesDC.Dispose();
                onscreenAxesDC = this.panelAxes.CreateGraphics();
                onscreenAxesDC.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            TTrace trace = Traces[label.Name.Substring(5)];
            HighlightTrace(trace);
            ReDrawGraph();
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            //Label label = sender as Label;
            //IGraphTrace trace = Traces[label.Name.Substring(5)];
            //RemoveTraceHighlight(trace);
            //ReDrawGraph();
        }

        private void overlayAllTracesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverlayAllTraces();
        }

        private void overlayColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickTraceColor(highlightedTrace);
        }

        private void overlayThisTracetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverlayTrace(highlightedTrace);
        }

        private void panelAxes_Paint(object sender, PaintEventArgs e)
        {
            DrawAxes();
        }

        private void panelGraph_Click(object sender, EventArgs e)
        {
            panelGraph.Focus(); // remove focus from comments, to remove cursor
        }

        private void panelGraph_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.timerPlotCursor.Enabled)
            {
                this.timerPlotCursor.Enabled = false;
                this.panelGraph.Cursor = Cursors.Default;
            }

            OnGraphDoubleClick();
        }

        private void panelGraph_MouseDown(object sender, MouseEventArgs e)
        {
            // Left Mouse Button
            if (e.Button == MouseButtons.Left)
            {
                // If Plot Cursor is not visible, start 0.5 sec timer to display it
                if (!_ShowingPlotCursor)
                {
                    if ((Traces.Count > 0) && (Traces.First().Points.Count > 0) && (!this.timerPlotCursor.Enabled))
                    {
                        this.plotCursorX = e.X;
                        this.timerPlotCursor.Interval = 500;
                        this.timerPlotCursor.Enabled = true;
                    }
                }
                else if ((e.X > this.plotCursorX - 50) && (e.X < this.plotCursorX + 50))
                {
                    this.plotCursorX = e.X;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                mouseDownX = e.X;
                mouseDownY = e.Y;
                mouseDownXRMB = e.X;
                mouseDownYRMB = e.Y;
            }
        }

        private void panelGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > panelGraph.Width) return;
            if (e.Y > panelGraph.Height) return;
            if (e.X < 0) return;
            if (e.Y < 0) return;

            mouseMoveX = e.X;
            mouseMoveY = e.Y;
            mouseDownX = -1;
            mouseDownY = -1;

            // If Plot Cursor is visible...
            if (_ShowingPlotCursor)
            {
                // If Left Mouse Button is Pressed, and we were previously near the plot cursor, then follow it with the mouse
                if (e.Button == MouseButtons.Left)
                {
                    if (panelGraph.Cursor == Cursors.SizeWE)
                    {
                        plotCursorX = e.X;
                        timerPlotCursorUpdates.Enabled = true;
                    }
                }
                // If the left mouse button isn't pressed...
                else
                {
                    // ... and we're near the plot cursor, change the pointer to <->
                    if ((e.X > this.plotCursorX - 50) && (e.X < this.plotCursorX + 50))
                        panelGraph.Cursor = Cursors.SizeWE;
                    else
                        panelGraph.Cursor = Cursors.Default;
                }
            }

            timerTraceHighlight.Enabled = false;
            timerTraceHighlight.Interval = 50;
            timerTraceHighlight.Enabled = true;
        }

        private void panelGraph_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // If Plot Cursor hasn't yet been displayed, just stop the timer
                if (timerPlotCursor.Enabled)
                {
                    timerPlotCursor.Enabled = false;
                    panelGraph.Cursor = Cursors.Default;
                }
                // Otherwise, remove the plot cursor
                else
                {
                    if (_ShowingPlotCursor)
                    {
                        timerPlotCursorUpdates.Enabled = false;
                        plotCursorX = -1;
                        _ShowingPlotCursor = false;
                        panelGraph.Cursor = Cursors.Default;
                        ReDrawGraph();
                    }
                }
            }
        }

        private void panelGraph_Paint(object sender, PaintEventArgs e)
        {
            PaintGraph();
        }

        private void panelGraph_Resize(object sender, EventArgs e)
        {
            if (resizing == false)
            {
                resizing = true;
                timerResize.Interval = 50;
                timerResize.Enabled = true;
            }
        }

        private void PickTraceColor(TTrace Trace)
        {
            if (Trace != null)
            {
                colorDialog1.Color = Trace.Color;
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                    Trace.Color = colorDialog1.Color;
            }
        }

        //public string FileName
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int temp = 0;
            if (LegendForPrintedPlot)
            {
                temp = 185;
            }
            else
            {
                temp = 60;
            }
            try
            {
                Rectangle GraphBounds = new Rectangle(e.MarginBounds.Left + 35 - (int)e.PageSettings.HardMarginX, e.MarginBounds.Top + 25 - (int)e.PageSettings.HardMarginY, e.MarginBounds.Width - temp, e.MarginBounds.Height - 100);
                Font font10pt = new Font("Arial", 10);
                Font font8pt = new Font("Arial", 8);
                Pen myPen = new Pen(Brushes.Black, 1);
                String sTemp;

                DrawGridLines(e.Graphics, GraphBounds);
                Actions.Retry(3,
                    delegate
                    {
                        DrawGraphRect(e.Graphics, GraphBounds, GraphBounds, new Rectangle(0, 0, GraphBounds.Width, GraphBounds.Height), true);
                    });
                bIsPrintingAxes = true;
                DrawAxes(e.Graphics, GraphBounds);
                bIsPrintingAxes = false;
                e.Graphics.DrawLine(myPen, GraphBounds.Left, GraphBounds.Top, GraphBounds.Right, GraphBounds.Top);
                e.Graphics.DrawLine(myPen, GraphBounds.Right, GraphBounds.Top, GraphBounds.Right, GraphBounds.Bottom);

                // Print the Comments
                if (Settings.CommentsVisible)
                {
                    Point anchor;
                    Size commentboxsize;
                    foreach (var graphComment in _graphData.Comments)
                    {
                        // Calculate anchor point
                        anchor = new Point(GraphBounds.Left + UserXtoScreen(graphComment.Location.X, GraphBounds),
                          GraphBounds.Top + UserYtoScreen(graphComment.Location.Y, GraphBounds));
                        // Check to see if anchor point is inside the graph
                        if (anchor.X >= GraphBounds.Left && anchor.X <= GraphBounds.Right &&
                            anchor.Y >= GraphBounds.Top && anchor.Y <= GraphBounds.Bottom)
                        {
                            // Calculate the size of the comment box
                            commentboxsize = new Size(110, (int)e.Graphics.MeasureString(graphComment.Text, font8pt, 100).Height + 10);
                            //Rectangle commentrect = new Rectangle(anchor.X + graphComment.Offset.X - commentboxsize.Width / 2,
                            //    anchor.Y + graphComment.Offset.Y - commentboxsize.Height / 2,
                            //    commentboxsize.Width, commentboxsize.Height);
                            // using 8.5 x 11 printer paper
                            float fAspectX = (float)GraphSize.Width / (float)810, fAspectY = (float)GraphSize.Height / ((float)650);
                            Rectangle commentrect = new Rectangle(GraphBounds.Left + UserXtoScreen(graphComment.Location.X, GraphBounds) + (int)((float)graphComment.CommentControl.Offset.X / fAspectX) - commentboxsize.Width / 2,
                              GraphBounds.Top + UserYtoScreen(graphComment.Location.Y, GraphBounds) + (int)((float)graphComment.CommentControl.Offset.Y / fAspectY) - commentboxsize.Height / 2,
                              commentboxsize.Width, commentboxsize.Height);

                            // check to see if the entire comment box is inside the graph
                            //if (commentrect.Left >= GraphBounds.Left &&
                            //    commentrect.Right <= GraphBounds.Right &&
                            //    commentrect.Top >= GraphBounds.Top &&
                            //    commentrect.Bottom <= GraphBounds.Bottom) {
                            if ((commentrect.Left + commentrect.Right) / 2 >= GraphBounds.Left &&
                                (commentrect.Left + commentrect.Right) / 2 <= GraphBounds.Right &&
                                (commentrect.Bottom + commentrect.Top) / 2 >= GraphBounds.Top &&
                                (commentrect.Bottom + commentrect.Top) / 2 <= GraphBounds.Bottom)
                            {
                                // draw the comment
                                e.Graphics.FillEllipse(Brushes.Black, new Rectangle(anchor.X - 2, anchor.Y - 2, 4, 4));
                                //e.Graphics.DrawLine(myPen, anchor, new Point(anchor.X + graphComment.Offset.X, anchor.Y + graphComment.Offset.Y));
                                Point pt2 = new Point(GraphBounds.Left + UserXtoScreen(graphComment.Location.X, GraphBounds) + (int)((float)graphComment.CommentControl.Offset.X / fAspectX),
                                  GraphBounds.Top + UserYtoScreen(graphComment.Location.Y, GraphBounds) + (int)((float)graphComment.CommentControl.Offset.Y / fAspectY));
                                // Check to see if anchor point is inside the graph
                                //e.Graphics.DrawLine(myPen, anchor, new Point(UserXtoScreen(anchor.X + graphComment.Offset.X, GraphBounds),
                                //  UserYtoScreen(anchor.Y + graphComment.Offset.Y, GraphBounds)));
                                e.Graphics.DrawLine(myPen, anchor, pt2);
                                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(System.Drawing.KnownColor.Info)), commentrect);
                                e.Graphics.DrawRectangle(myPen, commentrect);
                                e.Graphics.DrawString(graphComment.Text, font8pt, Brushes.Black,
                                    new RectangleF(commentrect.X + 5, commentrect.Y + 5, commentrect.Width - 10, commentrect.Height - 10));
                            }
                        }
                    }
                }

                // Print X-Axis Caption
                //sTemp = String.Format("Time ({0})", Settings.XAxisUnits == XAxisUnitsType.Seconds ? "seconds" : "minutes");
                e.Graphics.DrawString(XAxisLabel, font10pt, Brushes.Black,
                    GraphBounds.Width / 2 + GraphBounds.Left - e.Graphics.MeasureString(XAxisLabel, font10pt).Width / 2,
                    e.MarginBounds.Bottom - e.PageSettings.HardMarginY - e.Graphics.MeasureString(XAxisLabel, font10pt).Height * 2);

                // Print Filename if recalled scan
                //if (!string.IsNullOrEmpty(_filename)) {
                //  e.Graphics.DrawString(_filename, font10pt, Brushes.Black,
                //      e.MarginBounds.Right - e.PageSettings.HardMarginX - e.Graphics.MeasureString(_filename, font10pt).Width,
                //      e.MarginBounds.Bottom - e.PageSettings.HardMarginY - e.Graphics.MeasureString(_filename, font10pt).Height);
                //}

                // Print Left Header
                if (Settings.HeaderLeft != "")
                {
                    sTemp = Settings.HeaderLeft;
                    e.Graphics.DrawString(sTemp, font10pt, Brushes.Black,
                        e.MarginBounds.Left - e.PageSettings.HardMarginX, e.MarginBounds.Top - e.PageSettings.HardMarginY);
                }

                // Print Right Header
                if (Settings.HeaderRight != "")
                {
                    sTemp = Settings.HeaderRight;
                    e.Graphics.DrawString(sTemp, font10pt, Brushes.Black,
                        e.MarginBounds.Right - e.PageSettings.HardMarginX - e.Graphics.MeasureString(sTemp, font10pt).Width,
                        e.MarginBounds.Top - e.PageSettings.HardMarginY);
                }

                // Print Main Header
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(Header))
                    sb.Append(string.Format("{0} - ", Header));
                sb.Append(_GraphTypeName);
                //if (!string.IsNullOrEmpty(PlotName))
                //  sb.Append(string.Format(" ({0})", PlotName));
                if (!string.IsNullOrEmpty(_dataPlot.YAxisPrintLabel))
                    sb.Append(string.Format(" - {0}", _dataPlot.YAxisPrintLabel));

                //if (PlotData.Count > 0)
                //{
                //    if (Settings.XAxisUnits == XAxisUnitsType.Seconds)
                //        sTemp += String.Format(" : {0:0.0} seconds", Math.Round(PlotData[PlotData.Count - 1].PlotTime, 1));
                //    else
                //        sTemp = String.Format(" : {0:0.0} minutes", Math.Round(PlotData[PlotData.Count - 1].PlotTime / (Double)Settings.XAxisUnits, 1));
                //}

                sTemp = sb.ToString();

                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                e.Graphics.DrawString(sTemp, headerFont, Brushes.Black,
                    GraphBounds.Left + GraphBounds.Width / 2 - e.Graphics.MeasureString(sTemp, headerFont).Width / 2,
                    e.MarginBounds.Top - e.PageSettings.HardMarginY);

                // Print the Legend
                if (LegendForPrintedPlot)
                {
                    Single iLegendHeight = e.Graphics.MeasureString("Legend", headerFont).Height + 20;
                    foreach (var trace in Traces)
                    {
                        if (trace.Visible)
                            iLegendHeight += e.Graphics.MeasureString(String.Format("{0} ({1})", trace.Label, trace.Units),
                                font10pt, 120).Height + 10;
                    }
                    e.Graphics.DrawRectangle(myPen, e.MarginBounds.Right - e.PageSettings.HardMarginX - 140,
                        e.MarginBounds.Top - e.PageSettings.HardMarginY + 75, 140, iLegendHeight);
                    e.Graphics.DrawString("Legend", headerFont, Brushes.Black,
                        e.MarginBounds.Right - e.PageSettings.HardMarginX - 130,
                        e.MarginBounds.Top - e.PageSettings.HardMarginY + 85);
                    iLegendHeight = e.MarginBounds.Top - e.PageSettings.HardMarginY + 95 + e.Graphics.MeasureString("Legend", headerFont).Height;
                    foreach (var trace in Traces)
                    {
                        if (trace.Visible)
                        {
                            if (string.IsNullOrEmpty(trace.Units)) sTemp = trace.Label;
                            else sTemp = string.Format("{0} ({1})", trace.Label, trace.Units);
                            e.Graphics.DrawString(sTemp,
                                font10pt, new System.Drawing.SolidBrush(trace.Color),
                                new RectangleF(e.MarginBounds.Right - e.PageSettings.HardMarginX - 130, iLegendHeight, 120,
                                e.Graphics.MeasureString(trace.Label, font10pt, 120).Height));
                            iLegendHeight += e.Graphics.MeasureString(trace.Label, font10pt, 120).Height + 10;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                VtiEvent.Log.WriteError("An error occurred attempting to print the graph.",
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    ee.ToString());
                MessageBox.Show("An error occurred attempting to print the graph.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            e.HasMorePages = false;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnShowProperties();
        }

        private void removeAllOverlaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllOverlays();
        }

        private void removeThisOverlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveOverlay(highlightedTrace);
        }

        private void RemoveTraceHighlight(TTrace Trace)
        {
            Trace.LineWidth = 1;
            if (highlightedTrace == Trace)
                highlightedTrace = null;
            Label label = flowLayoutPanelLegend.Controls["label" + Trace.Key] as Label;
            if (label != null)
                label.Font = new Font(label.Font, FontStyle.Regular);

            overlayThisTracetoolStripMenuItem.Visible = false;
            hideThisTraceToolStripMenuItem.Visible = false;
            traceColorToolStripMenuItem.Visible = false;

            removeThisOverlayToolStripMenuItem.Visible = false;
            overlayColorToolStripMenuItem.Visible = false;

            toolStripMenuItemOverlayTraceLegend.Visible = false;
            toolStripMenuItemHideTraceLegend.Visible = false;
            toolStripMenuItemTraceColorLegend.Visible = false;

            toolStripMenuItemRemoveOverlayLegend.Visible = false;
            toolStripMenuItemOverlayColorLegend.Visible = false;

            bringTraceToFrontToolStripMenuItem.Visible = false;
            sendTraceToBackToolStripMenuItem.Visible = false;
            toolStripSeparatorTraceZOrder.Visible = false;

            toolStripMenuItemBringToFrontLegend.Visible = false;
            toolStripMenuItemSendToBackLegend.Visible = false;
            toolStripSeparatorTraceZOrderLegend.Visible = false;
        }

        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentComment.SendToBack();
        }

        private void sendTraceToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendTraceToBack(highlightedTrace);
        }

        private void SetCommentVisibility(CommentControl Comment, bool Visible)
        {
            if (this.InvokeRequired)
            {
                Action<CommentControl, bool> d = new Action<CommentControl, bool>(SetCommentVisibility);
                this.Invoke(d, Comment, Visible);
            }
            else
            {
                Comment.Visible = Visible;
            }
        }

        private void SetupNewTrace(TTrace trace)
        {
            Label label1 = new Label();
            label1.AutoSize = true;
            label1.Margin = new System.Windows.Forms.Padding(3);
            label1.MinimumSize = new System.Drawing.Size(60, 13);
            label1.Name = "label" + trace.Key;
            label1.Size = new System.Drawing.Size(60, 13);
            label1.Text = trace.Label;
            label1.ForeColor = trace.Color;
            label1.Visible = trace.Visible;
            label1.MouseEnter += new EventHandler(label1_MouseEnter);
            label1.MouseLeave += new EventHandler(label1_MouseLeave);
            flowLayoutPanelLegend.Controls.Add(label1);
            label1.SendToBack();
            lastPlotPoint.Add(trace, null);
            trace.Changed += new EventHandler<TraceCollectionChangedEventArgs<TTrace, TPoint>>(trace_Changed);
        }

        //private void showAllCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //  Settings.CommentsVisible = true;
        //}

        private void showCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentComment != null)
            {
                _currentComment.CommentVisible = true;
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int yLoc;
            Settings.LegendWidth = splitContainer1.SplitterDistance;
            if (_frmPlotProp1 != null)
            {
                try
                {
                    yLoc = _frmPlotProp1.DataPlot.YMaxUpLbl.Location.Y;
                    _frmPlotProp1.DataPlot.YMaxUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMinUpLbl.Location.Y;
                    _frmPlotProp1.DataPlot.YMinUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMaxExpUpLbl.Location.Y;
                    _frmPlotProp1.DataPlot.YMaxExpUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMinExpUpLbl.Location.Y;
                    _frmPlotProp1.DataPlot.YMinExpUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMaxDn.Location.Y;
                    _frmPlotProp1.DataPlot.YMaxDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMaxUp.Location.Y;
                    _frmPlotProp1.DataPlot.YMaxUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMinDn.Location.Y;
                    _frmPlotProp1.DataPlot.YMinDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMinUp.Location.Y;
                    _frmPlotProp1.DataPlot.YMinUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMaxExpDn.Location.Y;
                    _frmPlotProp1.DataPlot.YMaxExpDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMaxExpUp.Location.Y;
                    _frmPlotProp1.DataPlot.YMaxExpUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMinExpDn.Location.Y;
                    _frmPlotProp1.DataPlot.YMinExpDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                    yLoc = _frmPlotProp1.DataPlot.YMinExpUp.Location.Y;
                    _frmPlotProp1.DataPlot.YMinExpUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                }
                catch (Exception ex)
                {
                    VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            //OnGraphSettingChanged(GraphSettingChangeType.LegendWidth);
        }

        private void timerPlotCursor_Tick(object sender, EventArgs e)
        {
            timerPlotCursor.Enabled = false;
            _ShowingPlotCursor = true;
            panelGraph.Cursor = Cursors.SizeWE;
            timerPlotCursorUpdates.Enabled = true;
        }

        private void timerPlotCursorUpdates_Tick(object sender, EventArgs e)
        {
            timerPlotCursorUpdates.Enabled = false;
            PaintGraph();
            //panelGraph.Invalidate();
            DrawAxes();
        }

        private void timerResize_Tick(object sender, EventArgs e)
        {
            timerResize.Enabled = false;
            InitGraphics();
            ReDrawGraph();
            resizing = false;
        }

        private void timerSlideXAxis_Tick(object sender, EventArgs e)
        {
            float dx;

            timerSlideXAxis.Enabled = false;
            if (Traces.First().Points.Count > 0)
            {
                dx = ((float)(mouseDownX - mouseMoveX)) * (Settings.XMax - Settings.XMin) / (float)this.panelGraph.Width;

                if (dx < 0)
                {
                    if (Settings.XMin + dx >= 0)
                    {
                        Settings.XMin += dx;
                        Settings.XMax += dx;
                        mouseDownX = mouseMoveX;
                        ReDrawGraph();
                        mouseMoveX = -1;
                    }
                }
                else
                {
                    if (Settings.XMax + dx <= Traces.First().Points.Last().X)
                    {
                        Settings.XMin += dx;
                        Settings.XMax += dx;
                        mouseDownX = mouseMoveX;
                        this.ReDrawGraph();
                        mouseMoveX = -1;
                    }
                }
            }
        }

        private void timerSlideYAxis_Tick(object sender, EventArgs e)
        {
            float dy;

            timerSlideYAxis.Enabled = false;
            if (Settings.PlotSemiLog)
            {
                //dy = (float)Math.Round(((float)(mouseMoveY - mouseDownY) / (float)this.panelGraph.Height) * (float)(Settings.YMaxExp - Settings.YMinExp), 0);

                //if (dy < 0)
                //{
                //    if (Settings.YMinExp + (int)dy >= (int)_frmPlotProp1.YMinExp().Minimum && Settings.YMaxExp + (int)dy >= (int)_frmPlotProp1.YMaxExp().Minimum)
                //    {
                //        Settings.YMinExp += (int)dy;
                //        Settings.YMaxExp += (int)dy;
                //    }
                //    mouseDownY = mouseMoveY;
                //    this.ReDrawGraph();
                //    mouseMoveY = -1;
                //}
                //else if (dy > 0)
                //{
                //    if (Settings.YMinExp + (int)dy <= (int)_frmPlotProp1.YMinExp().Maximum && Settings.YMaxExp + (int)dy <= (int)_frmPlotProp1.YMaxExp().Maximum)
                //    {
                //        Settings.YMinExp += (int)dy;
                //        Settings.YMaxExp += (int)dy;
                //    }
                //    mouseDownY = mouseMoveY;
                //    this.ReDrawGraph();
                //    mouseMoveY = -1;
                //}
            }
            else
            {
                //dy = (float)Math.Round(((float)(mouseMoveY - mouseDownY) / (float)this.panelGraph.Height), 2) * (Settings.YMax - Settings.YMin);

                //if (dy < 0)
                //{
                //    Settings.YMin += dy;
                //    Settings.YMax += dy;
                //    mouseDownY = mouseMoveY;
                //    this.ReDrawGraph();
                //    mouseMoveY = -1;
                //}
                //else
                //{
                //    Settings.YMin += dy;
                //    Settings.YMax += dy;
                //    mouseDownY = mouseMoveY;
                //    this.ReDrawGraph();
                //    mouseMoveY = -1;
                //}
            }
        }

        private void timerTraceHighlight_Tick(object sender, EventArgs e)
        {
            timerTraceHighlight.Enabled = false;
            bool redraw = false;
            TTrace trace = null;
            try
            {
                float ux = ScreenXtoUser(mouseMoveX);
                float uy = ScreenYtoUser(mouseMoveY);
                var traceNullCheck = Traces
                    .Where(T => T.Visible && T.Points.Count > 0)
                    .Select(T =>
                    {
                        var low = T.Points.LastOrDefault(P => P.X <= ux);
                        var high = T.Points.FirstOrDefault(P => P.X > ux);
                        if (low != null && high != null)
                        {
                            float hx = UserXtoScreen(high.X);
                            float hy = UserYtoScreen(high.Y);
                            float lx = UserXtoScreen(low.X);
                            float ly = UserYtoScreen(low.Y);
                            return new
                            {
                                distance =
                          (float)(Math.Abs((hx - lx) * (ly - mouseMoveY) - (lx - mouseMoveX) * (hy - ly)) /
                          Math.Sqrt((hx - lx) * (hx - lx) + (hy - ly) * (hy - ly))),
                                trace = T
                            };
                        }
                        else
                            return new
                            {
                                distance = (float)99999,
                                trace = T
                            };
                    })
                    .OrderBy(a => a.distance)
                    .ThenByDescending(a => a.trace.ZOrder)
                    .FirstOrDefault(a => a.distance < 10);
                if (traceNullCheck != null)
                {
                    trace = traceNullCheck.trace;
                }
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteVerbose(ex.Message + Environment.NewLine + ex.StackTrace);
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            if (trace != highlightedTrace)
            {
                if (highlightedTrace != null)
                {
                    //highlightedTrace.LineWidth = 1;
                    //highlightedTrace = null;
                    RemoveTraceHighlight(highlightedTrace);
                    redraw = true;
                }

                if (trace != null)
                {
                    //highlightedTrace.LineWidth = 2;
                    HighlightTrace(trace);
                    redraw = true;
                }
            }

            if (redraw) ReDrawGraph();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OnShowProperties();
        }

        private void toolStripMenuItemBringToFrontLegend_Click(object sender, EventArgs e)
        {
            BringTraceToFront(highlightedTrace);
        }

        private void toolStripMenuItemHideTraceLegend_Click(object sender, EventArgs e)
        {
            HideTrace(highlightedTrace);
        }

        private void toolStripMenuItemOverlayAllLegend_Click(object sender, EventArgs e)
        {
            OverlayAllTraces();
        }

        private void toolStripMenuItemOverlayColorLegend_Click(object sender, EventArgs e)
        {
            PickTraceColor(highlightedTrace);
        }

        private void toolStripMenuItemOverlayTraceLegend_Click(object sender, EventArgs e)
        {
            OverlayTrace(highlightedTrace);
        }

        private void toolStripMenuItemPropertiesLegend_Click(object sender, EventArgs e)
        {
            OnShowProperties();
        }

        private void toolStripMenuItemRemoveAllOverlaysLegend_Click(object sender, EventArgs e)
        {
            RemoveAllOverlays();
        }

        private void toolStripMenuItemRemoveOverlayLegend_Click(object sender, EventArgs e)
        {
            RemoveOverlay(highlightedTrace);
        }

        private void toolStripMenuItemSendToBackLegend_Click(object sender, EventArgs e)
        {
            SendTraceToBack(highlightedTrace);
        }

        private void toolStripToggleTimeUnits_Click(object sender, EventArgs e)
        {
            // toggle between Seconds and Minutes
            if (_frmPlotProp1.RadioButtonMinutes.Checked)
            {
                _frmPlotProp1.RadioButtonMinutes.Checked = false;
                _frmPlotProp1.RadioButtonSeconds.Checked = true;
                _dataPlot.Settings.XAxisUnits = XAxisUnitsType.Seconds;
            }
            else
            {
                _frmPlotProp1.RadioButtonMinutes.Checked = true;
                _frmPlotProp1.RadioButtonSeconds.Checked = false;
                _dataPlot.Settings.XAxisUnits = XAxisUnitsType.Minutes;
            }
            Settings.Save();
            _dataPlot.SetSettings();
            DrawAxes();
        }

        /// <summary>
        /// Assign the time units for the data plot
        /// </summary>
        public void AssignTimeUnits()
        {
            if (_dataPlot.Settings.XAxisUnits == XAxisUnitsType.Seconds)
            {
                _frmPlotProp1.RadioButtonSeconds.Checked = true;
                _frmPlotProp1.RadioButtonMinutes.Checked = false;
            }
            else
            {
                _frmPlotProp1.RadioButtonSeconds.Checked = false;
                _frmPlotProp1.RadioButtonMinutes.Checked = true;
            }
        }

        private void toolStripMenuItemShowLegend_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LegendVisible = (sender as ToolStripMenuItem).Checked;
        }

        private void toolStripMenuItemShowLegend2_Click(object sender, EventArgs e)
        {
            Settings.LegendVisible = (sender as ToolStripMenuItem).Checked;
        }

        private void toolStripMenuItemDisplayComments_CheckedChanged(object sender, EventArgs e)
        {
            Settings.CommentsVisible = (sender as ToolStripMenuItem).Checked;
            //foreach (GraphComment gc in _graphData.Comments) {
            //  gc.Visible = Settings.CommentsVisible;
            //}
            foreach (var comment in _graphData.Comments)
            {
                comment.Visible = comment.CommentControl.Visible = Settings.CommentsVisible;
            }
            //_dataPlot.Settings.Save();
            //_dataPlot.SetSettings();
        }

        private void toolStripMenuItemShowLegendLegend_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LegendVisible = (sender as ToolStripMenuItem).Checked;
            int yLoc;
            if (Settings.LegendVisible)
            {
                yLoc = _frmPlotProp1.DataPlot.YMaxUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMaxUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMinUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxExpUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMaxExpUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinExpUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMinExpUpLbl.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 3, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxDn.Location.Y;
                _frmPlotProp1.DataPlot.YMaxDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxUp.Location.Y;
                _frmPlotProp1.DataPlot.YMaxUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinDn.Location.Y;
                _frmPlotProp1.DataPlot.YMinDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinUp.Location.Y;
                _frmPlotProp1.DataPlot.YMinUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxExpDn.Location.Y;
                _frmPlotProp1.DataPlot.YMaxExpDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxExpUp.Location.Y;
                _frmPlotProp1.DataPlot.YMaxExpUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinExpDn.Location.Y;
                _frmPlotProp1.DataPlot.YMinExpDn.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinExpUp.Location.Y;
                _frmPlotProp1.DataPlot.YMinExpUp.Location = new System.Drawing.Point(_graphData.Settings.LegendWidth + 6, yLoc);
            }
            else
            {
                yLoc = _frmPlotProp1.DataPlot.YMaxUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMaxUpLbl.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMinUpLbl.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxExpUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMaxExpUpLbl.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinExpUpLbl.Location.Y;
                _frmPlotProp1.DataPlot.YMinExpUpLbl.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxDn.Location.Y;
                _frmPlotProp1.DataPlot.YMaxDn.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxUp.Location.Y;
                _frmPlotProp1.DataPlot.YMaxUp.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinDn.Location.Y;
                _frmPlotProp1.DataPlot.YMinDn.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinUp.Location.Y;
                _frmPlotProp1.DataPlot.YMinUp.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxExpDn.Location.Y;
                _frmPlotProp1.DataPlot.YMaxExpDn.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMaxExpUp.Location.Y;
                _frmPlotProp1.DataPlot.YMaxExpUp.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinExpDn.Location.Y;
                _frmPlotProp1.DataPlot.YMinExpDn.Location = new System.Drawing.Point(2, yLoc);
                yLoc = _frmPlotProp1.DataPlot.YMinExpUp.Location.Y;
                _frmPlotProp1.DataPlot.YMinExpUp.Location = new System.Drawing.Point(2, yLoc);
            }
        }

        private void toolStripMenuItemTraceColorLegend_Click(object sender, EventArgs e)
        {
            PickTraceColor(highlightedTrace);
        }

        private void trace_Changed(object sender, TraceCollectionChangedEventArgs<TTrace, TPoint> e)
        {
            Label label1 = (Label)(flowLayoutPanelLegend.Controls["label" + e.ChangedItem.Key]);
            label1.Text = e.ChangedItem.Label;
            label1.ForeColor = e.ChangedItem.Color;
            label1.Visible = e.ChangedItem.Visible;
        }

        private void traceColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickTraceColor(highlightedTrace);
        }

        private void Traces_Changed(object sender, TraceCollectionChangedEventArgs<TTrace, TPoint> e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<object, TraceCollectionChangedEventArgs<TTrace, TPoint>>(Traces_Changed), sender, e);
            else
            {
                Label label1;

                switch (e.ChangeType)
                {
                    case TraceChangeType.Added:
                        SetupNewTrace(e.ChangedItem);
                        break;

                    case TraceChangeType.Changed:
                        label1 = (Label)(flowLayoutPanelLegend.Controls["label" + e.ChangedItem.Key]);
                        label1.Text = e.ChangedItem.Label;
                        label1.ForeColor = e.ChangedItem.Color;
                        label1.Visible = e.ChangedItem.Visible;
                        break;

                    case TraceChangeType.Cleared:
                        flowLayoutPanelLegend.Controls.Clear();
                        lastPlotPoint.Clear();
                        break;

                    case TraceChangeType.Removed:
                        label1 = (Label)(flowLayoutPanelLegend.Controls["label" + e.ChangedItem.Key]);
                        flowLayoutPanelLegend.Controls.Remove(label1);
                        lastPlotPoint.Remove(e.ChangedItem);
                        break;

                    case TraceChangeType.Replaced:
                        label1 = (Label)(flowLayoutPanelLegend.Controls["label" + e.ChangedItem.Key]);
                        label1.Name = "label" + e.ReplacedWith.Key;
                        label1.Text = e.ReplacedWith.Label;
                        label1.ForeColor = e.ReplacedWith.Color;
                        label1.Visible = e.ReplacedWith.Visible;
                        lastPlotPoint.Remove(e.ChangedItem);
                        lastPlotPoint.Add(e.ReplacedWith, null);
                        break;
                }
            }
        }

        private void transparentPanelXAxis_MouseDown(object sender, MouseEventArgs e)
        {
            // If the whole plot isn't showing, allow user to slide it
            //if (Traces.First().Points.Count > 0 &&
            //    (Settings.XMin > 0 || Settings.XMax < Traces.First().Points.Last().X))
            //{
            //    //toolStripButtonAutoShowAll.Checked = false;
            //    //toolStripButtonShowAll.Checked = false;
            //    //toolStripButtonAutoShowEnd.Checked = false;
            //    //toolStripButtonShowEnd.Checked = false;
            //    transparentPanelXAxis.Cursor = closedHandCursor;
            //    mouseDownX = e.X;
            //    mouseMoveX = -1;
            //}
        }

        private void transparentPanelXAxis_MouseLeave(object sender, EventArgs e)
        {
            mouseDownX = -1;
            mouseMoveX = -1;
            timerSlideXAxis.Enabled = false;
        }

        private void transparentPanelXAxis_MouseMove(object sender, MouseEventArgs e)
        {
            if ((mouseDownX != -1) && (mouseMoveX == -1))
            {
                mouseMoveX = e.X;
                timerSlideXAxis.Enabled = false;
                timerSlideXAxis.Interval = 50;
                timerSlideXAxis.Enabled = true;
            }
        }

        private void transparentPanelXAxis_MouseUp(object sender, MouseEventArgs e)
        {
            transparentPanelXAxis.Cursor = Cursors.Hand;
            timerSlideXAxis.Enabled = false;
            mouseDownX = -1;
        }

        private void transparentPanelXAxis_Paint(object sender, PaintEventArgs e)
        {
            DrawAxes();
        }

        private void transparentPanelYAxis_MouseDown(object sender, MouseEventArgs e)
        {
            //transparentPanelYAxis.Cursor = closedHandCursor;
            //mouseDownY = e.Y;
            //mouseMoveY = -1;
        }

        private void transparentPanelYAxis_MouseLeave(object sender, EventArgs e)
        {
            timerSlideYAxis.Enabled = false;
            mouseDownY = -1;
            mouseMoveY = -1;
        }

        private void transparentPanelYAxis_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownY != -1)
            {
                mouseMoveY = e.Y;
                timerSlideYAxis.Enabled = false;
                timerSlideYAxis.Interval = 50;
                timerSlideYAxis.Enabled = true;
            }
        }

        private void transparentPanelYAxis_MouseUp(object sender, MouseEventArgs e)
        {
            //transparentPanelYAxis.Cursor = Cursors.Hand;
            timerSlideYAxis.Enabled = false;
            mouseDownY = -1;
        }

        private void transparentPanelYAxis_Paint(object sender, PaintEventArgs e)
        {
            DrawAxes();
        }

        #endregion Private Methods

        #endregion Methods
    }
}