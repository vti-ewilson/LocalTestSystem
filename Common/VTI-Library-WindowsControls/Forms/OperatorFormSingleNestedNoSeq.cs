using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents an Operator Form for a Single-Port system that contains a nested Data Plot control.
    /// This form is presents a user interface that has fewer overlapping windows.
    /// </summary>
    public partial class OperatorFormSingleNestedNoSeq : Form
    {
        //private Action<string> setTextCallback;
        //private Action<bool> setFlowRateTextVisibleCallback;
        private int _signalPanelWidth = 200;

        private int _dockButtonSpacing = 50;
        private int _testHistoryRowCountUndocked = 5;
        private int _testHistoryColumnCountUndocked = 3;
        private BackgroundWorker formDisplayWorker;
        private bool showStatePending = false;

        public void StartAsyncShow()
        {
            showStatePending = true;
            formDisplayWorker.RunWorkerAsync();
        }

        public void StartAsyncHide()
        {
            showStatePending = false;
            formDisplayWorker.RunWorkerAsync();
        }

        private void FormDisplayWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
        }

        private void FormDisplayWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (showStatePending != Visible)
            {
                Invoke(new MethodInvoker(delegate () { Show(); }));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorFormSingleNestedNoSeq">OperatorFormSingleNestedNoSeq</see> class
        /// </summary>
        /// <param name="MdiParent">Form that will be the owner of this form</param>
        public OperatorFormSingleNestedNoSeq(Form MdiParent)
          : this(String.Empty, String.Empty, MdiParent) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorFormSingleNestedNoSeq">OperatorFormSingleNestedNoSeq</see> class
        /// </summary>
        /// <param name="PortName">Name of the port to be displayed on the form</param>
        /// <param name="PortColor">Colors to be used to display the port name. Note: this is a
        /// string containing two color names for the foreground and background, such as "Blue, White".</param>
        /// <param name="MdiParent">Owner of this form</param>
        public OperatorFormSingleNestedNoSeq(String PortName, String PortColor, Form MdiParent)
        {
            InitializeComponent();
            formDisplayWorker = new BackgroundWorker();
            formDisplayWorker.DoWork += new DoWorkEventHandler(FormDisplayWorker_DoWork);
            formDisplayWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FormDisplayWorker_RunWorkerCompleted);

            miniCommandControl1.Visible = false;
            labelPortName.Text = PortName;
            labelPortName.Visible = !String.IsNullOrEmpty(PortName);
            if (!String.IsNullOrEmpty(PortColor) && PortColor.Contains(","))
            {
                labelPortName.ForeColor = Color.FromName(PortColor.Split(',')[0]);
                labelPortName.BackColor = Color.FromName(PortColor.Split(',')[1]);
            }

            this.MdiParent = MdiParent;
            this.Dock = DockStyle.Fill;

            testHistoryDockControl1.UndockFrameStartPosition = FormStartPosition.Manual;
            testHistoryDockControl1.UndockFrameLocation = new Point(Properties.Settings.Default.OpFormSingleTestHistoryX, Properties.Settings.Default.OpFormSingleTestHistoryY);

            valvesPanelDockControl1.UndockFrameStartPosition = FormStartPosition.Manual;
            valvesPanelDockControl1.UndockFrameLocation = new Point(Properties.Settings.Default.OpFormSingleValvesPanelX, Properties.Settings.Default.OpFormSingleValvesPanelY);
            miniCommandControl1.Visible = Properties.Settings.Default.OpFormSingleCommandControlVisible;
            signalIndicatorControl1.Visible = Properties.Settings.Default.OpFormSingleFlowRateIndicatorVisible;
            panelRightPane.MinimumSize = new Size(_signalPanelWidth, 4);

            systemSignalsControl1.Visible = Properties.Settings.Default.OpFormSingleSystemSignalsVisible;
            testHistoryDockControl1.IsDocked = Properties.Settings.Default.OpFormSingleTestHistoryIsDocked;
            //valvesPanelDockControl1.IsDocked = Properties.Settings.Default.OpFormSingleValvesPanelIsDocked;
            valvesPanelDockControl1.Hide();
            dataPlotDockControl1.IsDocked = Properties.Settings.Default.OpFormSingleDataPlotIsDocked;

            if (dataPlotDockControl1.DataPlot.Settings.CallUpgrade)
            {
                dataPlotDockControl1.DataPlot.Settings.Upgrade();
                dataPlotDockControl1.DataPlot.Settings.CallUpgrade = false;
                dataPlotDockControl1.DataPlot.Settings.Save();
            }

            CheckRightPanelVisible();
            SetTestHistorySize();
            SetValvesPanelSize();
        }

        /// <summary>
        /// Gets the <see cref="SequenceStepsControl.SequenceStepList">List</see> of <see cref="VTIWindowsControlLibrary.Classes.SequenceStep">SequenceSteps</see> for the form.
        /// </summary>
        //[Browsable(false)]
        //public SequenceStepsControl.SequenceStepList Sequences
        //{
        //    //get { return sequenceStepsControl1.Sequences; }
        //}

        /// <summary>
        /// Gets the <see cref="TestHistoryDockControl">TestHistoryDockControl</see> for the form.
        /// </summary>
        [Browsable(false)]
        public TestHistoryDockControl TestHistoryDockControl
        {
            get { return testHistoryDockControl1; }
        }

        /// <summary>
        /// Gets the <see cref="TestHistoryControl">TestHistory</see> control for the form.
        /// </summary>
        [Browsable(false)]
        public TestHistoryControl TestHistory
        {
            get { return testHistoryDockControl1.TestHistory; }
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

        /// Gets the <see cref="ValvesPanelDockControl">ValvesPanelDockControl</see> for the form.
        /// </summary>
        [Browsable(false)]
        public ValvesPanelDockControl ValvesPanelDockControl
        {
            get { return valvesPanelDockControl1; }
        }

        /// <summary>
        /// Gets the <see cref="ValvesPanelControl">ValvesPanel</see> control for the form.
        /// </summary>
        [Browsable(false)]
        public ValvesPanelControl ValvesPanel
        {
            get { return valvesPanelDockControl1.ValvesPanel; }
        }

        /// <summary>
        /// Gets the <see cref="DataPlotDockControl">DataPlotDockControl</see> for the form.
        /// </summary>
        [Browsable(false)]
        public DataPlotDockControl DataPlotDockControl
        {
            get { return dataPlotDockControl1; }
        }

        /// <summary>
        /// Gets the <see cref="DataPlotControl">DataPlot</see> for the form.
        /// </summary>
        [Browsable(false)]
        public DataPlotControl DataPlot
        {
            get { return dataPlotDockControl1.DataPlot; }
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
        /// Gets or sets a value that indicates if the <see cref="MiniCommandControl">MiniCommandControl</see> is visible.
        /// </summary>
        [Browsable(true)]
        public Boolean CommandWindowVisible
        {
            get { return miniCommandControl1.Visible; }
            set { miniCommandControl1.Visible = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the port name should be visible at the top of the operator prompt.
        /// </summary>
        [Browsable(true)]
        public bool PortNameVisible
        {
            get { return labelPortName.Visible; }
            set { labelPortName.Visible = value; }
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
            }
        }

        /// <summary>
        /// Gets the <see cref="SignalIndicatorControl">Signal Indicator</see> control for the form.
        /// </summary>
        public SignalIndicatorControl SignalIndicator
        {
            get { return signalIndicatorControl1; }
        }

        /// <summary>
        /// Gets or sets the text for the flow rate display in the operator prompt.
        /// </summary>
        public string FlowRateText
        {
            get { return labelFlowRate.Text; }
            set { labelFlowRate.SetText(value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the flow rate display should be visible in the operator prompt.
        /// </summary>
        public Boolean FlowRateVisible
        {
            get { return labelFlowRate.Visible; }
            set { labelFlowRate.Visible = value; }
        }

        /// <summary>
        /// Gets the <see cref="RichTextPrompt">Prompt</see> control for the form.
        /// </summary>
        [Browsable(false)]
        public RichTextPrompt Prompt
        {
            get { return richTextPrompt1; }
        }

        private void systemSignalsControl1_VisibleChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetTestHistorySize();
            SetValvesPanelSize();
        }

        private void SetTestHistorySize()
        {
            if (testHistoryDockControl1.IsDocked)
            {
                testHistoryDockControl1.Columns = 1;
                if (systemSignalsControl1.Visible)
                {
                    testHistoryDockControl1.Rows = 10;
                    testHistoryDockControl1.Dock = DockStyle.Bottom;
                    testHistoryDockControl1.Caption = "Test History";
                }
                else
                {
                    testHistoryDockControl1.Rows = 40;
                    testHistoryDockControl1.Dock = DockStyle.Fill;
                    testHistoryDockControl1.Caption = "Test History";
                }
            }
            else
            {
                testHistoryDockControl1.Rows = _testHistoryRowCountUndocked;
                testHistoryDockControl1.Columns = _testHistoryColumnCountUndocked;

                // Set DockButtonSpacing in settings.setttings
                // Dock should be located just to the left of the window close button
                testHistoryDockControl1.Caption = "Test History";
                //testHistoryDockControl1.Caption = string.Concat(testHistoryDockControl1.Caption, new String(' ', _dockButtonSpacing), " Dock >>");

                // attempts to set dock button location automatically regardless of size of test history control - FAIL 8/2/18
                //Size textSize0 = TextRenderer.MeasureText("-", testHistoryDockControl1.Font);
                //Size textSize1 = TextRenderer.MeasureText("Test History", testHistoryDockControl1.Font);
                //Size textSize2 = TextRenderer.MeasureText("Dock", testHistoryDockControl1.Font);
                //int SizeOfCaption = testHistoryDockControl1.Size.Width;
                //int SpacesWidth = SizeOfCaption;
                //int NumberOfSpaces = SpacesWidth / textSize0.Width;
                //String Spaces = new String('-', NumberOfSpaces);
                //Size textSize3 = TextRenderer.MeasureText(Spaces, testHistoryDockControl1.Font);
            }
        }

        private void SetValvesPanelSize()
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

        private void testHistoryDockControl1_VisibleChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetTestHistorySize();
        }

        private void valvesPanelDockControl1_VisibleChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetValvesPanelSize();
        }

        private void CheckRightPanelVisible()
        {
            if (systemSignalsControl1.Visible ||
                (testHistoryDockControl1.IsDocked && testHistoryDockControl1.Visible))
                panelRightPane.Width = 150;
            else
                panelRightPane.Width = 0;
        }

        private void dataPlotDockControl1_DockChanged(object sender, EventArgs e)
        {
            //splitContainer2.Panel2Collapsed = !dataPlotDockControl1.IsDocked;// && dataPlotDockControl1.Visible);
        }

        private void dataPlotDockControl1_VisibleChanged(object sender, EventArgs e)
        {
            //splitContainer2.Panel2Collapsed = !(dataPlotDockControl1.IsDocked && dataPlotDockControl1.Visible);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Select something other than the rich text box, to prevent the cursor from showing up
            // and to help control flicker
            if (richTextPrompt1.Focused) labelPortName.Select();
        }

        private void OperatorFormSingleNestedNoSeq_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.OpFormSingleHorizSplitter =
                splitContainer1.SplitterDistance;
            Properties.Settings.Default.OpFormSingleCommandControlVisible =
                miniCommandControl1.Visible;
            Properties.Settings.Default.OpFormSingleFlowRateIndicatorVisible =
                signalIndicatorControl1.Visible;
            Properties.Settings.Default.OpFormSingleSystemSignalsVisible =
                systemSignalsControl1.Visible;
            Properties.Settings.Default.OpFormSingleTestHistoryIsDocked =
                testHistoryDockControl1.IsDocked;

            Properties.Settings.Default.OpFormSingleValvesPanelIsDocked =
                valvesPanelDockControl1.IsDocked;

            Properties.Settings.Default.OpFormSingleDataPlotIsDocked =
                dataPlotDockControl1.IsDocked;
            Properties.Settings.Default.OpFormSingleTestHistoryX =
                testHistoryDockControl1.UndockFrameLocation.X;
            Properties.Settings.Default.OpFormSingleTestHistoryY =
                testHistoryDockControl1.UndockFrameLocation.Y;

            Properties.Settings.Default.OpFormSingleValvesPanelX =
                valvesPanelDockControl1.UndockFrameLocation.X;
            Properties.Settings.Default.OpFormSingleValvesPanelY =
                valvesPanelDockControl1.UndockFrameLocation.Y;

            Properties.Settings.Default.Save();

            dataPlotDockControl1.DataPlot.Settings.Save();
        }

        private void testHistoryDockControl1_DockChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetTestHistorySize();
        }

        private void valvesPanelDockControl1_DockChanged(object sender, EventArgs e)
        {
            CheckRightPanelVisible();
            SetValvesPanelSize();
        }

        //private void SetFlowRateText(string value)
        //{
        //    if (labelFlowRate.InvokeRequired)
        //        labelFlowRate.Invoke(setTextCallback, value);
        //    else
        //        labelFlowRate.Text = value;
        //}

        //private void SetFlowRateTextVisible(bool value)
        //{
        //    if (labelFlowRate.InvokeRequired)
        //        labelFlowRate.Invoke(setFlowRateTextVisibleCallback, value);
        //    else
        //        labelFlowRate.Visible = value;

        //}
    }
}