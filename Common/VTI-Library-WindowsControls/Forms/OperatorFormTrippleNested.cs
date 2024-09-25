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
    public partial class OperatorFormTrippleNested : Form
    {
        private SequenceStepsControl.SequenceStepList[] _sequences = new SequenceStepsControl.SequenceStepList[3];
        private TestHistoryDockControl[] _testHistoryDockControl = new TestHistoryDockControl[3];
        private TestHistoryControl[] _testHistory = new TestHistoryControl[3];
        private DataPlotDockControl[] _dataPlotDockControl = new DataPlotDockControl[3];
        private DataPlotControl[] _dataPlot = new DataPlotControl[3];
        private RichTextPrompt[] _prompt = new RichTextPrompt[3];
        //private SystemSignalsControl[] _systemSignals = new SystemSignalsControl[1];

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorFormDualNested">OperatorFormDualNested</see> class
        /// </summary>
        /// <param name="PortNames">Names to be displayed for the ports</param>
        /// <param name="PortColors">Collection of strings containing the colors to be used for the ports.
        /// Note: Each string contains two color names for the foreground and background, such as "Blue, White".</param>
        /// <param name="MdiParent">Owner of this form.</param>
        public OperatorFormTrippleNested(StringCollection PortNames, StringCollection PortColors, Form MdiParent)
        {
            InitializeComponent();
            miniCommandControl1.Visible = false;
            dataPlotDockControl1.DataPlot.PlotName = PortNames[0];
            dataPlotDockControl2.DataPlot.PlotName = PortNames[1];
            dataPlotDockControl3.DataPlot.PlotName = PortNames[2];

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

            if (dataPlotDockControl3.DataPlot.Settings.CallUpgrade)
            {
                dataPlotDockControl3.DataPlot.Settings.Upgrade();
                dataPlotDockControl3.DataPlot.Settings.CallUpgrade = false;
                dataPlotDockControl3.DataPlot.Settings.Save();
            }

            dataPlotDockControl1.DataPlot.AutoRun1Visible = false;
            dataPlotDockControl1.DataPlot.AutoRun2Visible = false;
            dataPlotDockControl2.DataPlot.AutoRun1Visible = false;
            dataPlotDockControl2.DataPlot.AutoRun2Visible = false;
            dataPlotDockControl3.DataPlot.AutoRun1Visible = false;
            dataPlotDockControl3.DataPlot.AutoRun2Visible = false;
            dataPlotDockControl1.DataPlot.Close += new EventHandler(DataPlot1_Close);
            dataPlotDockControl2.DataPlot.Close += new EventHandler(DataPlot2_Close);
            dataPlotDockControl3.DataPlot.Close += new EventHandler(DataPlot3_Close);
            //dataPlotDockControl1.DataPlot.LegendVisible = dataPlotDockControl1.DataPlot.Settings.LegendVisible;
            //dataPlotDockControl2.DataPlot.LegendVisible = dataPlotDockControl1.DataPlot.Settings.LegendVisible;
            //dataPlotDockControl2.DataPlot.Traces = dataPlotDockControl1.DataPlot.Traces; // Tie traces together

            labelPortName1.Text = PortNames[0];
            labelPortName2.Text = PortNames[1];
            labelPortName3.Text = PortNames[2];

            tabPage1.Text = PortNames[0];
            tabPage2.Text = PortNames[1];
            tabPage3.Text = PortNames[2];

            //labelPortName1.Visible = ShowPortNames;
            //labelPortName2.Visible = ShowPortNames;
            labelPortName1.ForeColor = Color.FromName(PortColors[0].Split(',')[0]);
            labelPortName1.BackColor = Color.FromName(PortColors[0].Split(',')[1]);
            labelPortName2.ForeColor = Color.FromName(PortColors[1].Split(',')[0]);
            labelPortName2.BackColor = Color.FromName(PortColors[1].Split(',')[1]);
            labelPortName3.ForeColor = Color.FromName(PortColors[2].Split(',')[0]);
            labelPortName3.BackColor = Color.FromName(PortColors[2].Split(',')[1]);

            _sequences[0] = sequenceStepsControl1.Sequences;
            _sequences[1] = sequenceStepsControl2.Sequences;
            _sequences[2] = sequenceStepsControl3.Sequences;

            _testHistoryDockControl[0] = testHistoryDockControl1;
            _testHistoryDockControl[1] = testHistoryDockControl2;
            _testHistoryDockControl[2] = testHistoryDockControl3;
            _testHistory[0] = testHistoryDockControl1.TestHistory;
            _testHistory[1] = testHistoryDockControl2.TestHistory;
            _testHistory[2] = testHistoryDockControl3.TestHistory;
            testHistoryDockControl1.Caption = PortNames[0] + " Test History";
            testHistoryDockControl2.Caption = PortNames[1] + " Test History";
            testHistoryDockControl3.Caption = PortNames[2] + " Test History";
            testHistoryDockControl1.UndockFrameStartPosition = FormStartPosition.Manual;
            testHistoryDockControl1.UndockFrameLocation = new Point(50, Screen.PrimaryScreen.WorkingArea.Height - 200);
            testHistoryDockControl2.UndockFrameStartPosition = FormStartPosition.Manual;
            testHistoryDockControl2.UndockFrameLocation = new Point(225, Screen.PrimaryScreen.WorkingArea.Height - 200);
            testHistoryDockControl3.UndockFrameStartPosition = FormStartPosition.Manual;
            testHistoryDockControl3.UndockFrameLocation = new Point(400, Screen.PrimaryScreen.WorkingArea.Height - 200);

            _dataPlotDockControl[0] = dataPlotDockControl1;
            _dataPlotDockControl[1] = dataPlotDockControl2;
            _dataPlotDockControl[2] = dataPlotDockControl3;
            _dataPlot[0] = dataPlotDockControl1.DataPlot;
            _dataPlot[1] = dataPlotDockControl2.DataPlot;
            _dataPlot[2] = dataPlotDockControl3.DataPlot;

            _prompt[0] = richTextPrompt1;
            _prompt[1] = richTextPrompt2;
            _prompt[2] = richTextPrompt3;

            this.MdiParent = MdiParent;
            this.Dock = DockStyle.Fill;
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
        public MiniCommandControl.ManualCommandList Commands
        {
            get { return miniCommandControl1.Commands; }
        }

        /// <summary>
        /// Set the selected tab to #1
        /// </summary>
        [Browsable(true)]
        public Boolean Tab1Select
        {
            set { tabControl1.SelectedTab = tabPage1; }
        }

        /// <summary>
        /// Set the selected tab to #2
        /// </summary>
        [Browsable(true)]
        public Boolean Tab2Select
        {
            set { tabControl1.SelectedTab = tabPage2; }
        }

        /// <summary>
        /// Set the selected tab to #3
        /// </summary>
        [Browsable(true)]
        public Boolean Tab3Select
        {
            set { tabControl1.SelectedTab = tabPage3; }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="MiniCommandControl">MiniCommandControl</see> is visible.
        /// </summary>
        [Browsable(true)]
        public Boolean CommandWindowVisible
        {
            get { return miniCommandControl1.Visible; }
            set { miniCommandControl1.Visible = value; }
        }

        /// <summary>
        /// Gets the <see cref="SystemSignalsControl">SystemSignals</see> control for the form.
        /// </summary>
        [Browsable(false)]
        public SystemSignalsControl SystemSignals
        {
            get { return systemSignalsControl1; }
        }

        /// <summary>
        /// Gets an array of the <see cref="RichTextPrompt">Prompt</see> controls for the form.
        /// </summary>
        [Browsable(false)]
        public RichTextPrompt[] Prompt
        {
            get { return _prompt; }
        }

        private void systemSignalsControl1_VisibleChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();

            if (systemSignalsControl1.Visible)
                miniCommandControl1.Dock = DockStyle.Bottom;
            else
                miniCommandControl1.Dock = DockStyle.Fill;
        }

        private void miniCommandControl1_VisibleChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
        }

        private void CheckRightPanelVisible()
        {
            if (systemSignalsControl1.Visible || miniCommandControl1.Visible)
                panelRightPane.Width = 200;
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

        private void richTextPrompt3_TextChanged(object sender, EventArgs e)
        {
            // Select something other than the rich text box, to prevent the cursor from showing up
            // and to help control flicker
            if (richTextPrompt3.Focused) labelPortName3.Select();
        }

        private void dataPlotDockControl1_DockChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !dataPlotDockControl1.IsDocked;
        }

        private void dataPlotDockControl2_DockChanged(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !dataPlotDockControl2.IsDocked;
        }

        private void dataPlotDockControl3_DockChanged(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = !dataPlotDockControl3.IsDocked;
        }

        private void DataPlot1_Close(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = true;
        }

        private void DataPlot2_Close(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = true;
        }

        private void DataPlot3_Close(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = true;
        }

        private void OperatorFormTrippleNested_ResizeEnd(object sender, EventArgs e)
        {
            tabControl1.Size = new Size(splitContainerMain.Panel2.Width, splitContainerMain.Panel2.Height);
            tableLayoutPanel2.Size = new Size(splitContainerMain.Panel1.Width, splitContainerMain.Panel1.Height);
        }
    }
}