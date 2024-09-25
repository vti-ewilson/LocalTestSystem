using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents an Operator Form for a Dual-Port system that contains a nested Data Plot control.
    /// This form is presents a user interface that has fewer overlapping windows.
    /// </summary>
    public partial class OperatorFormDualNested2 : Form
    {
        private int _signalPanelWidth = 200;
        private int _dockButtonSpacing = 50;
        private int _testHistoryRowCountUndocked = 5;
        private int _testHistoryColumnCountUndocked = 3;

        private SequenceStepsControl.SequenceStepList[] _sequences = new SequenceStepsControl.SequenceStepList[2];
        private TestHistoryDockControl[] _testHistoryDockControl = new TestHistoryDockControl[2];
        private TestHistoryControl[] _testHistory = new TestHistoryControl[2];
        private ValvesPanelDockControl[] _valvesPanelDockControl = new ValvesPanelDockControl[2];
        private ValvesPanelControl[] _valvesPanel = new ValvesPanelControl[2];
        private DataPlotDockControl[] _dataPlotDockControl = new DataPlotDockControl[2];
        private DataPlotControl[] _dataPlot = new DataPlotControl[2];
        private RichTextPrompt[] _prompt = new RichTextPrompt[2];
        private SystemSignalsControl[] _systemSignals = new SystemSignalsControl[2];
        private MiniCommandControl.ManualCommandList[] _commands = new MiniCommandControl.ManualCommandList[2];

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorFormDualNested">OperatorFormDualNested</see> class
        /// </summary>
        /// <param name="PortNames">Names to be displayed for the ports</param>
        /// <param name="PortColors">Collection of strings containing the colors to be used for the ports.
        /// Note: Each string contains two color names for the foreground and background, such as "Blue, White".</param>
        /// <param name="MdiParent">Owner of this form.</param>
        public OperatorFormDualNested2(StringCollection PortNames, StringCollection PortColors, Form MdiParent)
        {
            InitializeComponent();
            miniCommandControl1.Visible = false;
            miniCommandControl2.Visible = false;
            dataPlotDockControl1.DataPlot.PlotName = PortNames[0];
            dataPlotDockControl2.DataPlot.PlotName = PortNames[1];

            if (dataPlotDockControl1.DataPlot.Settings.CallUpgrade)
            {
                dataPlotDockControl1.DataPlot.Settings.Upgrade();
                dataPlotDockControl1.DataPlot.Settings.CallUpgrade = false;
                dataPlotDockControl1.DataPlot.Settings.Save();
            }

            if (dataPlotDockControl2.DataPlot.Settings.CallUpgrade)
            {
                dataPlotDockControl2.DataPlot.Settings.Upgrade();
                dataPlotDockControl2.DataPlot.Settings.CallUpgrade = false;
                dataPlotDockControl2.DataPlot.Settings.Save();
            }

            dataPlotDockControl1.DataPlot.AutoRun1Visible = false;
            dataPlotDockControl1.DataPlot.AutoRun2Visible = false;
            dataPlotDockControl2.DataPlot.AutoRun1Visible = false;
            dataPlotDockControl2.DataPlot.AutoRun2Visible = false;
            dataPlotDockControl1.DataPlot.Close += new EventHandler(DataPlot1_Close);
            dataPlotDockControl2.DataPlot.Close += new EventHandler(DataPlot2_Close);
            //dataPlotDockControl1.DataPlot.LegendVisible = dataPlotDockControl1.DataPlot.Settings.LegendVisible;
            //dataPlotDockControl2.DataPlot.LegendVisible = dataPlotDockControl1.DataPlot.Settings.LegendVisible;
            //dataPlotDockControl2.DataPlot.Traces = dataPlotDockControl1.DataPlot.Traces; // Tie traces together

            labelPortName1.Text = PortNames[0];
            labelPortName2.Text = PortNames[1];
            //labelPortName1.Visible = ShowPortNames;
            //labelPortName2.Visible = ShowPortNames;
            labelPortName1.ForeColor = Color.FromName(PortColors[0].Split(',')[0]);
            labelPortName1.BackColor = Color.FromName(PortColors[0].Split(',')[1]);
            labelPortName2.ForeColor = Color.FromName(PortColors[1].Split(',')[0]);
            labelPortName2.BackColor = Color.FromName(PortColors[1].Split(',')[1]);

            _sequences[0] = sequenceStepsControl1.Sequences;
            _sequences[1] = sequenceStepsControl2.Sequences;
            //foreach (String str in Sequences)
            //{
            //    _sequences[0].Add(new SequenceStepsControl.SequenceStep(str));
            //    _sequences[1].Add(new SequenceStepsControl.SequenceStep(str));
            //}
            panelLeftMid.Height = Sequences[0].Count * 20 + 5;
            panelRightMid.Height = Sequences[1].Count * 20 + 5;
            _sequences[0].ItemAdded += new SequenceStepsControl.SequenceStepList.SequenceStepEventHandler(OperatorFormDualNested_SequencesChanged);
            _sequences[0].ItemChanged += new SequenceStepsControl.SequenceStepList.SequenceStepEventHandler(OperatorFormDualNested_SequencesChanged);
            _sequences[0].ItemRemoved += new SequenceStepsControl.SequenceStepList.SequenceStepEventHandler(OperatorFormDualNested_SequencesChanged);
            _sequences[0].Cleared += new EventHandler(OperatorFormDualNested_SequencesCleared);
            _sequences[1].ItemAdded += new SequenceStepsControl.SequenceStepList.SequenceStepEventHandler(OperatorFormDualNested_SequencesChanged);
            _sequences[1].ItemChanged += new SequenceStepsControl.SequenceStepList.SequenceStepEventHandler(OperatorFormDualNested_SequencesChanged);
            _sequences[1].ItemRemoved += new SequenceStepsControl.SequenceStepList.SequenceStepEventHandler(OperatorFormDualNested_SequencesChanged);
            _sequences[1].Cleared += new EventHandler(OperatorFormDualNested_SequencesCleared);

            _testHistoryDockControl[0] = testHistoryDockControl1;
            _testHistoryDockControl[1] = testHistoryDockControl2;
            _testHistory[0] = testHistoryDockControl1.TestHistory;
            _testHistory[1] = testHistoryDockControl2.TestHistory;
            testHistoryDockControl1.Caption = PortNames[0] + " Test History";
            testHistoryDockControl2.Caption = PortNames[1] + " Test History";

            _valvesPanelDockControl[0] = valvesPanelDockControl1;
            _valvesPanelDockControl[1] = valvesPanelDockControl2;
            _valvesPanel[0] = valvesPanelDockControl1.ValvesPanel;
            _valvesPanel[1] = valvesPanelDockControl2.ValvesPanel;
            valvesPanelDockControl1.Caption = PortNames[0] + " Valves Panel";
            valvesPanelDockControl2.Caption = PortNames[1] + " Valves Panel";

            _dataPlotDockControl[0] = dataPlotDockControl1;
            _dataPlotDockControl[1] = dataPlotDockControl2;
            _dataPlot[0] = dataPlotDockControl1.DataPlot;
            _dataPlot[1] = dataPlotDockControl2.DataPlot;

            _dataPlot[0].Caption = string.Format("Data Plot ({0})", PortNames[0]);
            _dataPlot[1].Caption = string.Format("Data Plot ({0})", PortNames[1]);

            _prompt[0] = richTextPrompt1;
            _prompt[1] = richTextPrompt2;

            _systemSignals[0] = systemSignalsControl1;
            _systemSignals[1] = systemSignalsControl2;

            _commands[0] = miniCommandControl1.Commands;
            _commands[1] = miniCommandControl2.Commands;

            systemSignalsControl1.BringToFront();
            systemSignalsControl2.BringToFront();

            this.MdiParent = MdiParent;
            this.Dock = DockStyle.Fill;

            testHistoryDockControl1.UndockFrameStartPosition = FormStartPosition.Manual;
            testHistoryDockControl1.UndockFrameLocation = new Point(Properties.Settings.Default.OpFormDualTestHistory1X, Properties.Settings.Default.OpFormDualTestHistory1Y);
            testHistoryDockControl2.UndockFrameStartPosition = FormStartPosition.Manual;
            testHistoryDockControl2.UndockFrameLocation = new Point(Properties.Settings.Default.OpFormDualTestHistory2X, Properties.Settings.Default.OpFormDualTestHistory2Y);

            valvesPanelDockControl1.UndockFrameStartPosition = FormStartPosition.Manual;
            valvesPanelDockControl1.UndockFrameLocation = new Point(Properties.Settings.Default.OpFormDualValvesPanel1X, Properties.Settings.Default.OpFormDualValvesPanel1Y);
            valvesPanelDockControl2.UndockFrameStartPosition = FormStartPosition.Manual;
            valvesPanelDockControl2.UndockFrameLocation = new Point(Properties.Settings.Default.OpFormDualValvesPanel2X, Properties.Settings.Default.OpFormDualValvesPanel2Y);

            splitContainer1.SplitterDistance = Properties.Settings.Default.OpFormDualHorizSplitter1;
            splitContainer2.SplitterDistance = Properties.Settings.Default.OpFormDualHorizSplitter2;
            miniCommandControl1.Visible = Properties.Settings.Default.OpFormDualCommandControl1Visible;
            miniCommandControl2.Visible = Properties.Settings.Default.OpFormDualCommandControl2Visible;
            systemSignalsControl1.Visible = Properties.Settings.Default.OpFormDualSystemSignals1Visible;
            systemSignalsControl2.Visible = Properties.Settings.Default.OpFormDualSystemSignals2Visible;
            testHistoryDockControl1.IsDocked = Properties.Settings.Default.OpFormDualTestHistory1IsDocked;
            testHistoryDockControl2.IsDocked = Properties.Settings.Default.OpFormDualTestHistory2IsDocked;
            //valvesPanelDockControl1.IsDocked = Properties.Settings.Default.OpFormDualValvesPanel1IsDocked;
            valvesPanelDockControl1.Hide();
            //valvesPanelDockControl2.IsDocked = Properties.Settings.Default.OpFormDualValvesPanel2IsDocked;
            valvesPanelDockControl2.Hide();
            dataPlotDockControl1.IsDocked = Properties.Settings.Default.OpFormDualDataPlot1IsDocked;
            dataPlotDockControl2.IsDocked = Properties.Settings.Default.OpFormDualDataPlot2IsDocked;

            CheckLeftPanelVisible();
            CheckRightPanelVisible();
            SetLeftTestHistorySize();
            SetRightTestHistorySize();
            SetLeftValvesPanelSize();
            SetRightValvesPanelSize();
        }

        private void OperatorFormDualNested_SequencesCleared(object sender, EventArgs e)
        {
            SetPanelHeights();
        }

        private void OperatorFormDualNested_SequencesChanged(object sender, SequenceStepsControl.SequenceStepList.SequenceStepEventArgs e)
        {
            SetPanelHeights();
        }

        private void SetPanelHeights()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(SetPanelHeights));
            else
            {
                panelLeftMid.Height = Sequences[0].Count * 20 + 5;
                panelRightMid.Height = Sequences[1].Count * 20 + 5;
            }
        }

        /// <summary>
        /// Gets an array of <see cref="SequenceStepsControl.SequenceStepList">Lists</see> of <see cref="VTIWindowsControlLibrary.Classes.SequenceStep">SequenceSteps</see> for the form.
        /// </summary>
        [Browsable(false)]
        public SequenceStepsControl.SequenceStepList[] Sequences
        {
            get { return _sequences; }
        }

        /// <summary>
        /// Gets an array of the <see cref="TestHistoryDockControl">TestHistoryDockControls</see> for the form.
        /// </summary>
        [Browsable(false)]
        public TestHistoryDockControl[] TestHistoryDockControl
        {
            get { return _testHistoryDockControl; }
        }

        /// <summary>
        /// Gets an array of the <see cref="TestHistoryControl">TestHistory</see> controls for the form.
        /// </summary>
        [Browsable(false)]
        public TestHistoryControl[] TestHistory
        {
            get { return _testHistory; }
        }

        /// <summary>
        public int TestHistoryRowCountUndocked
        {
            get { return _testHistoryRowCountUndocked; }
            set
            {
                _testHistoryRowCountUndocked = value;
            }
        }

        public int TestHistoryColumnCountUndocked
        {
            get { return _testHistoryColumnCountUndocked; }
            set
            {
                _testHistoryColumnCountUndocked = value;
            }
        }

        /// <summary>
        /// Gets an array of the <see cref="ValvesPanelDockControl">ValvesPanelDockControls</see> for the form.
        /// </summary>
        [Browsable(false)]
        public ValvesPanelDockControl[] ValvesPanelDockControl
        {
            get { return _valvesPanelDockControl; }
        }

        /// <summary>
        /// Gets an array of the <see cref="ValvesPanelControl">ValvesPanel</see> controls for the form.
        /// </summary>
        [Browsable(false)]
        public ValvesPanelControl[] ValvesPanel
        {
            get { return _valvesPanel; }
        }

        /// <summary>
        /// Gets an array of the <see cref="DataPlotDockControl">DataPlotDockControls</see> for the form.
        /// </summary>
        [Browsable(false)]
        public DataPlotDockControl[] DataPlotDockControl
        {
            get { return _dataPlotDockControl; }
        }

        /// <summary>
        /// Gets an array of the <see cref="DataPlotControl">DataPlot</see> controls for the form.
        /// </summary>
        [Browsable(false)]
        public DataPlotControl[] DataPlot
        {
            get { return _dataPlot; }
        }

        /// <summary>
        /// Gets the <see cref="MiniCommandControl.ManualCommandList">List</see> of Manual Commands that are in the
        /// <see cref="MiniCommandControl">MiniCommandControl</see> on the form.
        /// </summary>
        [Browsable(false)]
        public MiniCommandControl.ManualCommandList[] Commands
        {
            get { return _commands; }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="MiniCommandControl">MiniCommandControl</see> is visible.
        /// </summary>
        [Browsable(true)]
        public Boolean CommandWindowsVisible
        {
            get { return miniCommandControl1.Visible; }
            set
            {
                miniCommandControl1.Visible = value;
                miniCommandControl2.Visible = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="SystemSignalsControl">SystemSignals</see> control for the form.
        /// </summary>
        [Browsable(false)]
        public SystemSignalsControl[] SystemSignals
        {
            get { return _systemSignals; }
        }

        /// <summary>
        /// Gets or sets the Signal Panel Width for the control.
        /// </summary>
        public int DockButtonSpacing
        {
            get { return _dockButtonSpacing; }
            set
            {
                _dockButtonSpacing = value;
            }
        }

        /// <summary>
        /// Gets or sets the Signal Panel Width for the control.
        /// </summary>
        public int SystemSignalPanelWidth
        {
            get { return _signalPanelWidth; }
            set
            {
                _signalPanelWidth = value;
                panelRightPane.MinimumSize = new Size(_signalPanelWidth, 4);
                panelLeftPane.MinimumSize = new Size(_signalPanelWidth, 4);
            }
        }

        /// <summary>
        /// Gets an array of the <see cref="RichTextPrompt">Prompt</see> controls for the form.
        /// </summary>
        [Browsable(false)]
        public RichTextPrompt[] Prompt
        {
            get { return _prompt; }
        }

        /// <summary>
        /// Sets the port name label on the operator form.
        /// </summary>
        /// <param name="Port">Port whose name is to be set.</param>
        /// <param name="Text">Name to be displayed.</param>
        public void SetPortName(int Port, string Text)
        {
            if (Port == 0)
                labelPortName1.Text = Text;
            else
                labelPortName2.Text = Text;
        }

        private void systemSignalsControl1_VisibleChanged(object sender, EventArgs e)
        {
            CheckLeftPanelVisible();
            SetLeftTestHistorySize();
            SetLeftValvesPanelSize();
        }

        private void systemSignalsControl2_VisibleChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetRightTestHistorySize();
            SetRightValvesPanelSize();
        }

        private void miniCommandControl1_VisibleChanged(object sender, EventArgs e)
        {
            CheckLeftPanelVisible();
        }

        private void miniCommandControl2_VisibleChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
        }

        private void CheckLeftPanelVisible()
        {
            if (systemSignalsControl1.Visible || miniCommandControl1.Visible ||
                (testHistoryDockControl1.Visible && testHistoryDockControl1.IsDocked))
            {
                panelLeftPane.Width = 150;
                systemSignalsControl1.BringToFront();
            }
            else
                panelLeftPane.Width = 0;
        }

        private void CheckRightPanelVisible()
        {
            if (systemSignalsControl2.Visible || miniCommandControl2.Visible ||
                (testHistoryDockControl2.Visible && testHistoryDockControl2.IsDocked))
            {
                panelRightPane.Width = 150;
                systemSignalsControl2.BringToFront();
            }
            else
                panelRightPane.Width = 0;
        }

        private void richTextPrompt1_TextChanged(object sender, EventArgs e)
        {
            // Select something other than the rich text box, to prevent the cursor from showing up
            // and to help control flicker
            if (richTextPrompt1.Focused) labelPortName1.Select();
        }

        private void richTextPrompt2_TextChanged(object sender, EventArgs e)
        {
            // Select something other than the rich text box, to prevent the cursor from showing up
            // and to help control flicker
            if (richTextPrompt2.Focused) labelPortName2.Select();
        }

        private void dataPlotDockControl1_DockChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !dataPlotDockControl1.IsDocked;
        }

        private void dataPlotDockControl2_DockChanged(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !dataPlotDockControl2.IsDocked;
        }

        private void DataPlot1_Close(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = true;
        }

        private void DataPlot2_Close(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = true;
        }

        private void testHistoryDockControl1_DockChanged(object sender, EventArgs e)
        {
            CheckLeftPanelVisible();
            SetLeftTestHistorySize();
        }

        private void testHistoryDockControl2_DockChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetRightTestHistorySize();
        }

        private void valvesPanelDockControl1_DockChanged(object sender, EventArgs e)
        {
            CheckLeftPanelVisible();
            SetLeftValvesPanelSize();
        }

        private void valvesPanelDockControl2_DockChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetRightValvesPanelSize();
        }

        private void SetLeftTestHistorySize()
        {
            if (testHistoryDockControl1.IsDocked)
            {
                testHistoryDockControl1.Columns = 1;
                if (systemSignalsControl1.Visible)
                {
                    testHistoryDockControl1.Rows = 10;
                    testHistoryDockControl1.Dock = DockStyle.Bottom;
                    testHistoryDockControl1.Caption = "Blue Port Test History";
                }
                else
                {
                    testHistoryDockControl1.Rows = 40;
                    testHistoryDockControl1.Dock = DockStyle.Fill;
                    testHistoryDockControl1.Caption = "Blue Port Test History";
                }
            }
            else
            {
                testHistoryDockControl1.Rows = _testHistoryRowCountUndocked;
                testHistoryDockControl1.Columns = _testHistoryColumnCountUndocked;
                testHistoryDockControl1.Caption = "Blue Port Test History";
                //testHistoryDockControl1.Caption = string.Concat(testHistoryDockControl1.Caption, new String(' ', _dockButtonSpacing), " Dock >>");
            }
        }

        private void SetRightTestHistorySize()
        {
            if (testHistoryDockControl2.IsDocked)
            {
                testHistoryDockControl2.Columns = 1;
                if (systemSignalsControl2.Visible)
                {
                    testHistoryDockControl2.Rows = 10;
                    testHistoryDockControl2.Dock = DockStyle.Bottom;
                    testHistoryDockControl2.Caption = "White Port Test History";
                }
                else
                {
                    testHistoryDockControl2.Rows = 40;
                    testHistoryDockControl2.Dock = DockStyle.Fill;
                    testHistoryDockControl2.Caption = "White Port Test History";
                }
            }
            else
            {
                testHistoryDockControl2.Rows = _testHistoryRowCountUndocked;
                testHistoryDockControl2.Columns = _testHistoryColumnCountUndocked;
                testHistoryDockControl2.Caption = "White Port Test History";
                //testHistoryDockControl2.Caption = string.Concat(testHistoryDockControl2.Caption, new String(' ', _dockButtonSpacing), " Dock >>");
            }
        }

        private void SetLeftValvesPanelSize()
        {
            if (valvesPanelDockControl1.IsDocked)
            {
                valvesPanelDockControl1.Columns = 1;
                if (systemSignalsControl1.Visible)
                {
                    valvesPanelDockControl1.Rows = 10;
                    valvesPanelDockControl1.Dock = DockStyle.Bottom;
                }
                else
                {
                    valvesPanelDockControl1.Rows = 40;
                    valvesPanelDockControl1.Dock = DockStyle.Fill;
                }
            }
            else
            {
                valvesPanelDockControl1.Rows = 5;
                valvesPanelDockControl1.Columns = 3;
            }
        }

        private void SetRightValvesPanelSize()
        {
            if (valvesPanelDockControl2.IsDocked)
            {
                valvesPanelDockControl2.Columns = 1;
                if (systemSignalsControl2.Visible)
                {
                    valvesPanelDockControl2.Rows = 10;
                    valvesPanelDockControl2.Dock = DockStyle.Bottom;
                }
                else
                {
                    valvesPanelDockControl2.Rows = 40;
                    valvesPanelDockControl2.Dock = DockStyle.Fill;
                }
            }
            else
            {
                valvesPanelDockControl2.Rows = 5;
                valvesPanelDockControl2.Columns = 3;
            }
        }

        private void OperatorFormDualNested2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.OpFormDualHorizSplitter1 =
                splitContainer1.SplitterDistance;
            Properties.Settings.Default.OpFormDualHorizSplitter2 =
                splitContainer2.SplitterDistance;
            Properties.Settings.Default.OpFormDualCommandControl1Visible =
                miniCommandControl1.Visible;
            Properties.Settings.Default.OpFormDualCommandControl2Visible =
                miniCommandControl2.Visible;
            Properties.Settings.Default.OpFormDualSystemSignals1Visible =
                systemSignalsControl1.Visible;
            Properties.Settings.Default.OpFormDualSystemSignals2Visible =
                systemSignalsControl2.Visible;
            Properties.Settings.Default.OpFormDualTestHistory1IsDocked =
                testHistoryDockControl1.IsDocked;
            Properties.Settings.Default.OpFormDualTestHistory2IsDocked =
                testHistoryDockControl2.IsDocked;

            Properties.Settings.Default.OpFormDualValvesPanel1IsDocked =
                valvesPanelDockControl1.IsDocked;
            Properties.Settings.Default.OpFormDualValvesPanel2IsDocked =
                valvesPanelDockControl2.IsDocked;

            Properties.Settings.Default.OpFormDualDataPlot1IsDocked =
                dataPlotDockControl1.IsDocked;
            Properties.Settings.Default.OpFormDualDataPlot2IsDocked =
                dataPlotDockControl2.IsDocked;
            Properties.Settings.Default.OpFormDualTestHistory1X =
                testHistoryDockControl1.UndockFrameLocation.X;
            Properties.Settings.Default.OpFormDualTestHistory1Y =
                testHistoryDockControl1.UndockFrameLocation.Y;
            Properties.Settings.Default.OpFormDualTestHistory2X =
                testHistoryDockControl2.UndockFrameLocation.X;
            Properties.Settings.Default.OpFormDualTestHistory2Y =
                testHistoryDockControl2.UndockFrameLocation.Y;

            Properties.Settings.Default.OpFormDualValvesPanel1X =
                valvesPanelDockControl1.UndockFrameLocation.X;
            Properties.Settings.Default.OpFormDualValvesPanel1Y =
                valvesPanelDockControl1.UndockFrameLocation.Y;
            Properties.Settings.Default.OpFormDualValvesPanel2X =
                valvesPanelDockControl2.UndockFrameLocation.X;
            Properties.Settings.Default.OpFormDualValvesPanel2Y =
                valvesPanelDockControl2.UndockFrameLocation.Y;

            Properties.Settings.Default.Save();

            dataPlotDockControl1.DataPlot.Settings.Save();
            dataPlotDockControl2.DataPlot.Settings.Save();
        }
    }
}