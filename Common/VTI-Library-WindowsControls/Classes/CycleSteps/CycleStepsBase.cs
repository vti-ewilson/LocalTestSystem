using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Transactions;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Components.Graphing;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Enums;
using System.Text.RegularExpressions;

namespace VTIWindowsControlLibrary.Classes.CycleSteps
{
    /// <summary>
    /// Used as a base for any CycleSteps class.
    /// Automatically handles displaying, updating, and removing information
    /// about the individual cycle steps in the operator prompt.
    /// Contains a thread that processes all cycle steps every 50ms.
    /// </summary>
    /// <seealso cref="CycleStep"/>
    public abstract class CycleStepsBase
    {
        #region Fields (16)

        #region Protected Fields (1)

        /// <summary>
        /// Indicates whether a <c>NewLine</c> should be appended to the operator prompt after
        /// the prompt for the step.
        /// </summary>
        public bool NewlineAfterPrompt = false;

        #endregion Protected Fields
        #region Private Fields (15)

        private List<CycleStep> _CycleSteps;
        private DataPlotControl _DataPlotControl;
        private int _DetailFontSizeReduction = 2;
        private List<CycleStep> _EnabledSteps;
        private EventWaitHandle _ExitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private Color _FailedColor;
        private Font _FontDetails;
        private Color _PassedColor;

        // Thread object for processing Cycles
        private RichTextPrompt _Prompt;

        private List<CycleStep> _StepsToDisable;
        private List<CycleStep> _StepsToEnable;
        private Boolean _StopProcessing;
        private Color _TestingColor;
        private bool bDoNotDisplayRepeatedPrompts;

        // ThreadStart object for processing Cycles
        private Thread _Thread;

        private int _WaitDelay = 100;//50
        private TestHistoryControl _TestHistory;
        private Object stepLock = new Object();

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <param name="Prompt">Prompt in the operator form for this instance of the cycle steps</param>
        /// <param name="DataPlot">Data plot associated with this cycle</param>
        public CycleStepsBase(RichTextPrompt Prompt, DataPlotControl DataPlot, TestHistoryControl TestHistory)
        {
            _CycleSteps = new List<CycleStep>();
            _EnabledSteps = new List<CycleStep>();
            _StepsToEnable = new List<CycleStep>();
            _StepsToDisable = new List<CycleStep>();
            _Prompt = Prompt;
            _TestingColor = Properties.Settings.Default.SequenceTestingColor;
            _PassedColor = Properties.Settings.Default.SequenceGoodColor;
            _FailedColor = Properties.Settings.Default.SequenceBadColor;
            _DataPlotControl = DataPlot;
            _TestHistory = TestHistory;
            bDoNotDisplayRepeatedPrompts = false; // displays all cycle step prompts whether or not they are repeated
        }

        public CycleStepsBase(RichTextPrompt Prompt, DataPlotControl DataPlot)//, TestHistoryControl TestHistory)
        {
            _CycleSteps = new List<CycleStep>();
            _EnabledSteps = new List<CycleStep>();
            _StepsToEnable = new List<CycleStep>();
            _StepsToDisable = new List<CycleStep>();
            _Prompt = Prompt;
            _TestingColor = Properties.Settings.Default.SequenceTestingColor;
            _PassedColor = Properties.Settings.Default.SequenceGoodColor;
            _FailedColor = Properties.Settings.Default.SequenceBadColor;
            _DataPlotControl = DataPlot;
            //_TestHistory = TestHistory;
            bDoNotDisplayRepeatedPrompts = false; // displays all cycle step prompts whether or not they are repeated
        }

        #endregion Constructors

        #region Properties (5)

        /// <summary>
        /// List of <see cref="CycleStep">CycleSteps</see>
        /// </summary>
        public List<CycleStep> CycleSteps
        {
            get { return _CycleSteps; }
        }

        /// <summary>
        /// List of enabled <see cref="CycleStep">CycleSteps</see>
        /// </summary>
        public List<CycleStep> EnabledSteps
        {
            get { return _EnabledSteps; }
        }

        /// <summary>
        /// List of ToEnable <see cref="CycleStep">CycleSteps</see>
        /// </summary>
        public List<CycleStep> StepsToEnable
        {
            get { return _StepsToEnable; }
        }

        /// <summary>
        /// List of ToDisable <see cref="CycleStep">CycleSteps</see>
        /// </summary>
        public List<CycleStep> StepsToDisable
        {
            get { return _StepsToDisable; }
        }

        /// <summary>
        /// Gets or sets a value that indicates how much smaller the cycle step details are displayed
        /// in the operator prompt than the main cycle step prompt.
        /// </summary>
        public int DetailFontSizeReduction
        {
            get { return _DetailFontSizeReduction; }
            set
            {
                _DetailFontSizeReduction = value;
                Font fontDetails = new Font(_Prompt.DefaultFont.FontFamily, _Prompt.DefaultFont.Size - _DetailFontSizeReduction, _Prompt.DefaultFont.Style);
            }
        }

        /// <summary>
        /// Port name for this instance of the cycle steps
        /// </summary>
        /// <remarks>
        /// If supplied, the PortName will be added to event log entries to identify which port the events belong to.
        /// </remarks>
        public String PortName { get; set; }

        /// <summary>
        /// Gets the thread that processes the cycle steps.
        /// </summary>
        public Thread ProcessThread
        {
            get { return _Thread; }
        }

        /// <summary>
        /// Indicates whether
        /// <see cref="VtiEventLog">CycleStepsBase</see>
        /// should not display repeated prompts
        /// </summary>
        public bool DoNotDisplayRepeatedPrompts
        {
            get { return bDoNotDisplayRepeatedPrompts; }
            set { bDoNotDisplayRepeatedPrompts = value; }
        }

        /// <summary>
        /// <see cref="VTIWindowsControlLibrary.Data.UutRecord">UutRecord</see> to be used during the cycle
        /// </summary>
        /// <remarks>
        /// Any <see cref="CycleStep">CycleStep</see> that results in a <see cref="CycleStep.Passed">Passed</see> or
        /// <see cref="CycleStep.Failed">Failed</see> event will get a
        /// <see cref="VTIWindowsControlLibrary.Data.UutRecordDetail">UutRecordDetail</see> record
        /// added to this <see cref="VTIWindowsControlLibrary.Data.UutRecord">UutRecord</see>.
        /// </remarks>
        public UutRecord UutRecord { get; set; }

        #endregion Properties

        #region Methods (18)

        #region Public Methods (7)

        /// <summary>
        /// Method for starting the machine cycle.
        /// </summary>
        public abstract void CycleStart();

        /// <summary>
        /// Stops the machine cycle and displays a message in the operator prompt.
        /// </summary>
        /// <param name="Prompt">Message to be displayed.</param>
        public void CycleStop(string Prompt)
        {
            this.CycleStop();
            _Prompt.AppendText(Environment.NewLine + Prompt + Environment.NewLine, Color.Red);
        }

        /// <summary>
        /// Method for stopping the machine cycle.
        /// </summary>
        public abstract void CycleStop();

        public virtual void CycleNoTest(CycleStep step)
        {
            string testResult = VtiLib.Localization.GetString(
                string.Format("{0}_Failed", step.Name));
            if (string.IsNullOrEmpty(testResult))
                testResult = string.Format(VtiLibLocalization.StepFailed, step.Name);

            string testHistory = VtiLib.Localization.GetString(
                string.Format("{0}_FailedTH", step.Name));
            if (string.IsNullOrEmpty(testHistory))
                testHistory = testResult;

            this.RecordCycleResults(testResult, testHistory, Color.Black, Color.Yellow);
        }

        public virtual void CycleFail(CycleStep step)
        {
            string testResult = VtiLib.Localization.GetString(
                string.Format("{0}_Failed", step.Name));
            if (string.IsNullOrEmpty(testResult))
                testResult = string.Format(VtiLibLocalization.StepFailed, step.Name);

            string testHistory = VtiLib.Localization.GetString(
                string.Format("{0}_FailedTH", step.Name));
            if (string.IsNullOrEmpty(testHistory))
                testHistory = testResult;

            this.RecordCycleResults(testResult, testHistory, Color.Yellow, Color.Red);
        }

        public virtual void CyclePass(CycleStep step)
        {
            string testResult = VtiLib.Localization.GetString(
                string.Format("{0}_Passed", step.Name));
            if (string.IsNullOrEmpty(testResult))
                testResult = string.Format(VtiLibLocalization.StepFailed, step.Name);

            string testHistory = VtiLib.Localization.GetString(
                string.Format("{0}_PassedTH", step.Name));
            if (string.IsNullOrEmpty(testHistory))
                testHistory = testResult;

            this.RecordCycleResults(testResult, testHistory, Color.Black, Color.LawnGreen);
        }

        public virtual void RecordCycleResults(string TestResult, string TestHistoryEntry, Color TestHistoryForeground, Color TestHistoryBackground)
        {
            // Add entry to Test History window
            _TestHistory.AddEntry(TestHistoryEntry, TestHistoryForeground, TestHistoryBackground);

            // Set the test result and write the records
            if (this.UutRecord != null)
            {
                LeftStr(ref TestResult, 50);
                this.UutRecord.TestResult = TestResult;
                this.RecordResults();
            }
        }

        /// <summary>
        /// Initializes the CycleStepBase class.  Needs to be called from the constructor
        /// of the sub-class after all of the CycleStep objects have been initialized.
        /// </summary>
        public void Init()
        {
            CycleStep cycleStep;
            _FontDetails = new Font(_Prompt.DefaultFont.FontFamily, _Prompt.DefaultFont.Size - _DetailFontSizeReduction, _Prompt.DefaultFont.Style);
            HSL hslColorDetails = HSL.FromRGB(_Prompt.DefaultColor);
            hslColorDetails.Luminance = hslColorDetails.Luminance * 0.8F; // 80% as bright
            Color colorDetails = hslColorDetails.RGB;

            // Interate through all of the fields in the object, and build
            // an array list of only the CycleStep fields
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                // Add any cycle steps to the array list
                if (field.FieldType == typeof(CycleStep))
                {
                    // Get the value of the CycleStep
                    cycleStep = field.GetValue(this) as CycleStep;
                    if (cycleStep != null)
                    {
                        cycleStep.Name = field.Name;
                        // Add event handlers for the Started, Elapsed, and Stopped events
                        cycleStep.IntStateChanged += new CycleStep.CycleStepEventHandler(_CycleStep_StateChanged);
                        cycleStep.IntPromptChanged += new CycleStep.CycleStepPromptEventHandler(_CycleStep_PromptChanged);
                        cycleStep.IntPassed += new CycleStep.CycleStepEventHandler(_CycleStep_Passed);
                        cycleStep.IntFailed += new CycleStep.CycleStepEventHandler(_CycleStep_Failed);
                        // Set the font and color for the prompt, if not specified
                        if (cycleStep.Font == null) cycleStep.Font = _Prompt.DefaultFont;
                        if (cycleStep.FontDetails == null) cycleStep.FontDetails = _FontDetails;
                        if (cycleStep.Color == Color.Empty) cycleStep.Color = _Prompt.DefaultColor;
                        if (cycleStep.ColorDetails == Color.Empty) cycleStep.ColorDetails = colorDetails;
                        // If no prompt was supplier, check in the Localization resource for a string
                        // that matches the step name.  If found, set that for the prompt.
                        if (string.IsNullOrEmpty(cycleStep.Prompt))
                            cycleStep.Prompt = VtiLib.Localization.GetString(cycleStep.Name + "_Prompt");
                        cycleStep._CycleStepsBase = this;

                        cycleStep.CycleCommentStarted =
                            string.Format("{0} {1}",
                            VtiLib.Localization.GetString(cycleStep.Name) ?? cycleStep.Name,
                            VtiLib.Localization.GetString("Started") ?? "Started");

                        cycleStep.CycleCommentElapsed =
                            string.Format("{0} {1}",
                            VtiLib.Localization.GetString(cycleStep.Name) ?? cycleStep.Name,
                            VtiLib.Localization.GetString("Elapsed") ?? "Elapsed");

                        cycleStep.CycleCommentPassed =
                            string.Format("{0} {1}",
                            VtiLib.Localization.GetString(cycleStep.Name) ?? cycleStep.Name,
                            VtiLib.Localization.GetString("Passed") ?? "Passed");

                        cycleStep.CycleCommentFailed =
                            string.Format("{0} {1}",
                            VtiLib.Localization.GetString(cycleStep.Name) ?? cycleStep.Name,
                            VtiLib.Localization.GetString("Failed") ?? "Failed");

                        // Add the CycleStep to the list
                        _CycleSteps.Add(cycleStep);
                    }
                    else
                        VtiEvent.Log.WriteError(
                            String.Format("Cycle step '{0}' is not initialized!", field.Name),
                            VtiEventCatType.Application_Error);
                }
            }
        }

        /// <summary>
        /// Records the test results to the UutRecords and UutRecordDetails tables in the database
        /// </summary>
        public (long localID, long remoteID) RecordResults()
        {
            long localID = 0;
            long remoteID = 0;
            if (Properties.Settings.Default.UsesVtiDataDatabase)
            {
                try
                {
                    if (this.UutRecord != null)
                    {
                        using (VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString))
                        {
                            // Record has been build, just add it to the database
                            using (TransactionScope ts = new TransactionScope())
                            {
                                db.DeferredLoadingEnabled = false;
                                db.UutRecords.InsertOnSubmit(this.UutRecord);
                                db.SubmitChanges();
                                ts.Complete();
                                //this.UutRecord = null;
                            }
                        }
                        localID = UutRecord.ID;
                    }
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError(
                        String.Format("An error occurred writing a UutRecord."),
                        VtiEventCatType.Database,
                        e.ToString());
                }

                try
                {
                    if (this.UutRecord != null)
                    {
                        if (VtiLib.ConnectionString2 != "")
                        {
                            using (VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString2))
                            {
                                // Record has been build, just add it to the database
                                using (TransactionScope ts = new TransactionScope())
                                {
                                    db.DeferredLoadingEnabled = false;
                                    db.UutRecords.InsertOnSubmit(this.UutRecord);
                                    db.SubmitChanges();
                                    ts.Complete();
                                }
                            }
                            remoteID = UutRecord.ID;
                        }
                        this.UutRecord = null;
                    }
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError(
                        String.Format("An error occurred writing a UutRecord2."),
                        VtiEventCatType.Database,
                        e.ToString());
                }
            }
            return (localID, remoteID);
        }

        /// <summary>
        /// Starts the thread that processes the cycle steps
        /// </summary>
        public void Start()
        {
            _Thread = new Thread(new ThreadStart(ProcessCycles));
            _Thread.Start();
            _Thread.IsBackground = true;
            _Thread.Name = "CycleStep Process";
        }

        /// <summary>
        /// Stops the thread the processes the cycle steps
        /// </summary>
        public void Stop()
        {
            //if (thrd != null)
            //    thrd.Abort();
            _StopProcessing = true;
            _ExitEvent.Set();
        }

        /// <summary>
        /// Lock object to be used any time that serial I/O operations are performed
        /// </summary>
        public Object StepLock
        {
            get { return stepLock; }
            set { stepLock = value; }
        }

        #endregion Public Methods
        #region Private Methods (11)

        private void _CycleStep_Failed(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            //Actions.QueueAction(
            //    delegate
            //{
            // Add UutRecordDetail
            if (UutRecord != null && step.WriteUutRecordDetail)
            {
                string strTest = VtiLib.Localization.GetString(step.Name) ?? step.Name;
                string strResult = "Failed";//VtiLib.Localization.GetString("Failed") ?? "Failed";
                string strValueName = step.ProcessValue != null ? step.ProcessValue.Label : String.Empty;
                string strMinSetpointName = step.MinSetpoint != null ? step.MinSetpoint.DisplayName : String.Empty;
                string strMaxSetpointName = step.MaxSetpoint != null ? step.MaxSetpoint.DisplayName : String.Empty;
                string strUnits = step.ProcessValue != null ? step.ProcessValue.Units : String.Empty;
                LeftStr(ref strTest, 50); // UutRecordDetails limits this to 50 test characters
                LeftStr(ref strResult, 50);
                LeftStr(ref strValueName, 50);
                LeftStr(ref strMinSetpointName, 50);
                LeftStr(ref strMaxSetpointName, 50);
                LeftStr(ref strUnits, 50);
                UutRecord.UutRecordDetails.Add(
                    new UutRecordDetail
                    {
                        DateTime = DateTime.Now,
                        Test = strTest,
                        Result = strResult,
                        ValueName = strValueName,
                        Value = step.ProcessValue != null ? step.ProcessValue.Value : 0,
                        MinSetpointName = strMinSetpointName,
                        MinSetpoint = step.MinSetpoint != null ? step.MinSetpoint.ProcessValue : 0,
                        MaxSetpointName = strMaxSetpointName,
                        MaxSetpoint = step.MaxSetpoint != null ? step.MaxSetpoint.ProcessValue : 0,
                        Units = strUnits,
                        ElapsedTime = Math.Round(step.ElapsedTime.TotalSeconds, 2)
                    });
            }

            // Log the details
            if (!String.IsNullOrEmpty(VtiLib.Localization.GetString(step.Name + "_Failed")))
                VtiEvent.Log.WriteError(
                    String.Format("{0}{1}",
                        string.IsNullOrEmpty(PortName) ? "" : "(" + PortName + ") ",
                        VtiLib.Localization.GetString(step.Name + "_Failed")),
                    VtiEventCatType.Test_Cycle,
                    step.TimeDelay, step.MinSetpoint, step.MaxSetpoint, step.ProcessValue);

            this.RemoveMainPrompt(step);
            this.RemovePassPrompt(step);
            this.RemoveFailedPrompt(step);

            // If there is a failed prompt, append it to the operator prompt
            if (!String.IsNullOrEmpty(VtiLib.Localization.GetString(step.Name + "_PromptFailed")))
            {
                _Prompt.AppendText(VtiLib.Localization.GetString(step.Name + "_PromptFailed") + Environment.NewLine,
                    step.Font, _FailedColor);

                if (step.DisplayDetailsOnPassFail)
                    DisplaySetpointsAndSignals(step);

                if (NewlineAfterPrompt)
                    _Prompt.AppendText(Environment.NewLine);
            }
            //});
        }

        private void _CycleStep_Passed(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            //Actions.QueueAction(
            //    delegate
            //{
            // Add UutRecordDetail
            if (UutRecord != null && step.WriteUutRecordDetail)
            {
                string strTest = VtiLib.Localization.GetString(step.Name) ?? step.Name;
                string strResult = "Passed";//VtiLib.Localization.GetString("Passed") ?? "Passed";
                string strValueName = step.ProcessValue != null ? step.ProcessValue.Label : String.Empty;
                string strMinSetpointName = step.MinSetpoint != null ? step.MinSetpoint.DisplayName : String.Empty;
                string strMaxSetpointName = step.MaxSetpoint != null ? step.MaxSetpoint.DisplayName : String.Empty;
                string strUnits = step.ProcessValue != null ? step.ProcessValue.Units : String.Empty;
                LeftStr(ref strTest, 50); // UutRecordDetails limits this to 50 test characters
                LeftStr(ref strResult, 50);
                LeftStr(ref strValueName, 50);
                LeftStr(ref strMinSetpointName, 50);
                LeftStr(ref strMaxSetpointName, 50);
                LeftStr(ref strUnits, 50);
                UutRecord.UutRecordDetails.Add(
                    new UutRecordDetail
                    {
                        DateTime = DateTime.Now,
                        Test = strTest,
                        Result = strResult,
                        ValueName = strValueName,
                        Value = step.ProcessValue != null ? step.ProcessValue.Value : 0,
                        MinSetpointName = strMinSetpointName,
                        MinSetpoint = step.MinSetpoint != null ? step.MinSetpoint.ProcessValue : 0,
                        MaxSetpointName = strMaxSetpointName,
                        MaxSetpoint = step.MaxSetpoint != null ? step.MaxSetpoint.ProcessValue : 0,
                        Units = strUnits,
                        ElapsedTime = Math.Round(step.ElapsedTime.TotalSeconds, 2)
                    });
            }

            // Log the details
            if (!String.IsNullOrEmpty(VtiLib.Localization.GetString(step.Name + "_Passed")))
                VtiEvent.Log.WriteInfo(
                    String.Format("{0}{1}",
                        string.IsNullOrEmpty(PortName) ? "" : "(" + PortName + ") ",
                        VtiLib.Localization.GetString(step.Name + "_Passed")),
                    VtiEventCatType.Test_Cycle,
                    step.TimeDelay, step.MinSetpoint, step.MaxSetpoint, step.ProcessValue);

            this.RemoveMainPrompt(step);
            this.RemovePassPrompt(step);
            this.RemoveFailedPrompt(step);

            // If there is a passed prompt, append it to the operator prompt
            if (!String.IsNullOrEmpty(VtiLib.Localization.GetString(step.Name + "_PromptPassed")))
            {
                _Prompt.AppendText(VtiLib.Localization.GetString(step.Name + "_PromptPassed") + Environment.NewLine,
                    step.Font, _PassedColor);

                if (step.DisplayDetailsOnPassFail)
                    DisplaySetpointsAndSignals(step);

                if (NewlineAfterPrompt)
                    _Prompt.AppendText(Environment.NewLine);
            }
            //});
        }

        private void _CycleStep_PromptChanged(CycleStep step, CycleStep.CycleStepPromptEventArgs e)
        {
            if (step.Enabled)
            {
                if (!String.IsNullOrEmpty(e.OldPrompt))
                    _Prompt.ReplaceRegex(@"(?<=[\r\n])\b" + e.OldPrompt.Replace("\r\n", @"\s+").Replace("\n", @"\s").Replace("\r", @"\s"), e.NewPrompt);
                else
                    this.DisplayPrompt(step);
            }
        }

        private void _CycleStep_StateChanged(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            switch (step.State)
            {
                case CycleStepState.Idle:
                    if (_EnabledSteps.Contains(step) && !_StepsToDisable.Contains(step))
                        lock (StepLock)
                        {
                            //VtiEvent.Log.WriteInfo("case Idle, adding " + step.Name + " to _stepsToDisable list.", VtiEventCatType.Test_Cycle);
                            _StepsToDisable.Add(step);
                        }
                    //_StepsToDisable.Add(step);
                    //Actions.QueueAction( // commented out by JRT - 2012-09-19
                    //    delegate
                    //    {
                    this.RemoveMainPrompt(step);
                    this.RemovePassPrompt(step);
                    this.RemoveFailedPrompt(step);
                    //});
                    break;

                case CycleStepState.InProcess:
                    if (!_EnabledSteps.Contains(step) && !_StepsToEnable.Contains(step))
                        lock (StepLock)
                        {
                            //VtiEvent.Log.WriteInfo("case InProcess, adding " + step.Name + " to _stepsToEnable list.", VtiEventCatType.Test_Cycle);
                            _StepsToEnable.Add(step);
                        }
                    //_StepsToEnable.Add(step);
                    //Actions.QueueAction( // commented out by JRT - 2012-09-19
                    //delegate
                    //{
                    this.DisplayPrompt(step);
                    if (step.Sequence != null) step.Sequence.BackColor = _TestingColor;
                    if (step.LogToDataPlot && _DataPlotControl != null)
                        _DataPlotControl.AddCycleComment(step.CycleCommentStarted);
                    //});
                    break;

                case CycleStepState.Passed:
                    if (_EnabledSteps.Contains(step) && !_StepsToDisable.Contains(step))
                        lock (StepLock)
                        {
                            //VtiEvent.Log.WriteInfo("case Passed, adding " + step.Name + " to _stepsToDisable list.", VtiEventCatType.Test_Cycle);
                            _StepsToDisable.Add(step);
                        }
                    //_StepsToDisable.Add(step);
                    //Actions.QueueAction( // commented out by JRT - 2012-09-19
                    //    delegate
                    //    {
                    if (step.Sequence != null) step.Sequence.BackColor = _PassedColor;
                    if (step.LogToDataPlot && _DataPlotControl != null)
                        _DataPlotControl.AddCycleComment(step.CycleCommentPassed);
                    //});
                    break;

                case CycleStepState.Elapsed:
                    if (_EnabledSteps.Contains(step) && !_StepsToDisable.Contains(step))
                        lock (StepLock)
                        {
                            //VtiEvent.Log.WriteInfo("case Elapsed, adding " + step.Name + " to _stepsToDisable list.", VtiEventCatType.Test_Cycle);
                            _StepsToDisable.Add(step);
                        }
                    //_StepsToDisable.Add(step);
                    //Actions.QueueAction( // commented out by JRT - 2012-09-19
                    //    delegate
                    //    {
                    this.RemoveMainPrompt(step);
                    this.RemovePassPrompt(step);
                    this.RemoveFailedPrompt(step);
                    if (step.Sequence != null) step.Sequence.BackColor = _PassedColor;
                    if (step.LogToDataPlot && _DataPlotControl != null)
                        _DataPlotControl.AddCycleComment(step.CycleCommentElapsed);
                    //});
                    break;

                case CycleStepState.Failed:
                    if (_EnabledSteps.Contains(step) && !_StepsToDisable.Contains(step))
                        lock (StepLock)
                        {
                            //VtiEvent.Log.WriteInfo("case Failed, adding " + step.Name + " to _stepsToDisable list.", VtiEventCatType.Test_Cycle);
                            _StepsToDisable.Add(step);
                        }
                    //_StepsToDisable.Add(step);
                    //Actions.QueueAction( // commented out by JRT - 2012-09-19
                    //    delegate
                    //    {
                    if (step.Sequence != null) step.Sequence.BackColor = _FailedColor;
                    if (step.LogToDataPlot && _DataPlotControl != null)
                        _DataPlotControl.AddCycleComment(step.CycleCommentPassed);
                    //});
                    break;

                default:
                    if (_EnabledSteps.Contains(step) && !_StepsToDisable.Contains(step))
                        lock (StepLock)
                        {
                            VtiEvent.Log.WriteInfo("default, adding " + step.Name + " to _stepsToDisable list.", VtiEventCatType.Test_Cycle);
                            _StepsToDisable.Add(step);
                        }
                    //_StepsToDisable.Add(step);
                    break;
            }
        }

        private void DisplayPrompt(CycleStep step)
        {
            // If a prompt was specified, display it
            if (!String.IsNullOrEmpty(step.Prompt))
            {
                try
                {
                    if (!bDoNotDisplayRepeatedPrompts)
                    { // display all prompts
                        if (step.TimeDelay == null && !step.DisplayElapsedTime)
                            _Prompt.AppendText(step.Prompt + Environment.NewLine, step.Font, step.Color);
                    }
                    else
                    { // only display non-repeated prompts - JRT 2014-03-05
                        if (step.TimeDelay == null && !step.DisplayElapsedTime &&
                          (!_Prompt.Text.Contains(step.Prompt) || _Prompt.Text.Length <= 10))
                            _Prompt.AppendText(step.Prompt + Environment.NewLine, step.Font, step.Color);
                    }
                }
                catch (Exception ex)
                {
                }
                // If cycle step has a time delay, display the time remaining
                if (step.TimeDelay != null && !step.DisplayElapsedTime)
                    if (step.TimeRemaining.TotalHours >= 1)
                    {
                        _Prompt.AppendText(
                            String.Format("{0}  ",
                                step.Prompt),
                            step.Font, step.Color);
                        if (NewlineAfterPrompt) _Prompt.AppendText(Environment.NewLine);
                        _Prompt.AppendText(
                            String.Format("{0}: {1:0} {2} {3:0} {4} {5:0} {6}{7}",
                                VtiLibLocalization.TimeRemaining,
                                Math.Floor(step.TimeRemaining.TotalHours),
                                VtiLibLocalization.hours,
                                step.TimeRemaining.Minutes,
                                VtiLibLocalization.minutes,
                                step.TimeRemaining.Seconds + 1,
                                VtiLibLocalization.seconds, Environment.NewLine),
                            step.FontDetails, step.ColorDetails);
                    }
                    else if (step.TimeRemaining.Minutes > 0)
                    {
                        _Prompt.AppendText(
                            String.Format("{0}  ",
                                step.Prompt),
                            step.Font, step.Color);
                        if (NewlineAfterPrompt) _Prompt.AppendText(Environment.NewLine);
                        _Prompt.AppendText(
                            String.Format("{0}: {1:0} {2} {3:0} {4}{5}",
                                VtiLibLocalization.TimeRemaining,
                                step.TimeRemaining.Minutes,
                                VtiLibLocalization.minutes,
                                step.TimeRemaining.Seconds + 1,
                                VtiLibLocalization.seconds, Environment.NewLine),
                            step.FontDetails, step.ColorDetails);
                    }
                    else
                    {
                        _Prompt.AppendText(
                            String.Format("{0}  ",
                                step.Prompt),
                            step.Font, step.Color);
                        if (NewlineAfterPrompt) _Prompt.AppendText(Environment.NewLine);
                        _Prompt.AppendText(
                            String.Format("{0}: {1:0} {2}{3}",
                                VtiLibLocalization.TimeRemaining,
                                step.TimeRemaining.Seconds + 1,
                                VtiLibLocalization.seconds, Environment.NewLine),
                            step.FontDetails, step.ColorDetails);
                    }

                // Display Elapsed Time
                if (step.DisplayElapsedTime)
                    if (step.ElapsedTime.TotalHours >= 1)
                    {
                        _Prompt.AppendText(
                            String.Format("{0}  ",
                                step.Prompt),
                            step.Font, step.Color);
                        if (NewlineAfterPrompt) _Prompt.AppendText(Environment.NewLine);
                        _Prompt.AppendText(
                            String.Format("{0}: {1:0} {2} {3:0} {4} {5:0} {6}{7}",
                                VtiLibLocalization.ElapsedTime,
                                Math.Floor(step.ElapsedTime.TotalHours),
                                VtiLibLocalization.hours,
                                step.ElapsedTime.Minutes,
                                VtiLibLocalization.minutes,
                                step.ElapsedTime.Seconds + 1,
                                VtiLibLocalization.seconds, Environment.NewLine),
                            step.FontDetails, step.ColorDetails);
                    }
                    else if (step.ElapsedTime.Minutes > 0)
                    {
                        _Prompt.AppendText(
                            String.Format("{0}  ",
                                step.Prompt),
                            step.Font, step.Color);
                        if (NewlineAfterPrompt) _Prompt.AppendText(Environment.NewLine);
                        _Prompt.AppendText(
                            String.Format("{0}: {1:0} {2} {3:0} {4}{5}",
                                VtiLibLocalization.ElapsedTime,
                                step.ElapsedTime.Minutes,
                                VtiLibLocalization.minutes,
                                step.ElapsedTime.Seconds + 1,
                                VtiLibLocalization.seconds, Environment.NewLine),
                            step.FontDetails, step.ColorDetails);
                    }
                    else
                    {
                        _Prompt.AppendText(
                            String.Format("{0}  ",
                                step.Prompt),
                            step.Font, step.Color);
                        if (NewlineAfterPrompt) _Prompt.AppendText(Environment.NewLine);
                        _Prompt.AppendText(
                            String.Format("{0}: {1:0} {2}{3}",
                                VtiLibLocalization.ElapsedTime,
                                step.ElapsedTime.Seconds + 1,
                                VtiLibLocalization.seconds, Environment.NewLine),
                            step.FontDetails, step.ColorDetails);
                    }
                DisplaySetpointsAndSignals(step);

                _Prompt.AppendText(Environment.NewLine);
            }
        }

        private void DisplaySetpointsAndSignals(CycleStep step)
        {
            // display the MinSetpoint parameter
            if (step.MinSetpoint != null)
            {
                _Prompt.AppendText(string.Format("{0}: {1} {2}{3}", step.MinSetpoint.DisplayName,
                    step.MinSetpoint.ProcessValue.ToString(step.MinSetpoint.StringFormat), step.MinSetpoint.Units, Environment.NewLine),
                    step.FontDetails, step.ColorDetails);
            }
            // display the MaxSetpoint parameter
            if (step.MaxSetpoint != null)
                _Prompt.AppendText(string.Format("{0}: {1} {2}{3}", step.MaxSetpoint.DisplayName,
                    step.MaxSetpoint.ProcessValue.ToString(step.MaxSetpoint.StringFormat), step.MaxSetpoint.Units, Environment.NewLine),
                    step.FontDetails, step.ColorDetails);
            // display any other numeric parameters
            if (step.ParametersToDisplay != null)
                foreach (var parameter in step.ParametersToDisplay)
                    _Prompt.AppendText(string.Format("{0}: {1} {2}{3}", parameter.DisplayName,
                        parameter.ProcessValue.ToString(parameter.StringFormat), parameter.Units, Environment.NewLine),
                        step.FontDetails, step.ColorDetails);

            // display any other time delay parameters
            if (step.TimeParametersToDisplay != null)
                foreach (var parameter in step.TimeParametersToDisplay)
                    _Prompt.AppendText(string.Format("{0}: {1} {2}{3}", parameter.DisplayName,
                        parameter.ProcessValue.ToString(parameter.StringFormat), parameter.Units.ToString(), Environment.NewLine),
                        step.FontDetails, step.ColorDetails);

            // display the ProcessValue
            if (step.ProcessValue != null)
                _Prompt.AppendText(string.Format("{0}: {1} {2}{3}", step.ProcessValue.Label,
                    step.ProcessValue.Value.ToString(step.ProcessValue.Format), step.ProcessValue.Units, Environment.NewLine),
                    step.FontDetails, step.ColorDetails);
            // display any other analog signals
            if (step.SignalsToDisplay != null)
                foreach (var signal in step.SignalsToDisplay)
                    _Prompt.AppendText(string.Format("{0}: {1} {2}{3}", signal.Label,
                        signal.Value.ToString(signal.Format), signal.Units, Environment.NewLine),
                        step.FontDetails, step.ColorDetails);
        }

        /// <summary>
        /// ProcessCycles
        ///
        /// This method runs as a separate thread.
        /// It cycles through all of the CycleSteps, and processes any steps that are enabled.
        /// It updates the operator prompt for the time remaining in the step, and
        /// the value of any analog signals in the AnalogSignalsToDisplay list
        /// </summary>
        private void ProcessCycles()
        {
            // Infinite Loop for the thread
            // Thread can be stopped with thrd.Abort()
            while (!_StopProcessing)
            {
                if (_StepsToDisable.Count > 0)
                {
                    //VtiEvent.Log.WriteInfo("calling foreach(_StepsToDisable)", VtiEventCatType.Test_Cycle);
                    lock (StepLock)
                    {
                        foreach (var step in _StepsToDisable.ToList())
                            if (_EnabledSteps.Contains(step))
                            {
                                //VtiEvent.Log.WriteInfo("removing " + step.Name + " from _EnabledSteps", VtiEventCatType.Test_Cycle);
                                _EnabledSteps.Remove(step);
                            }
                            else
                                VtiEvent.Log.WriteWarning("attempted to disable already disabled step " + step.Name, VtiEventCatType.Application_Error);

                        _StepsToDisable.Clear();
                    }
                }

                if (_StepsToEnable.Count > 0)
                {
                    //VtiEvent.Log.WriteInfo("calling foreach(_StepsToEnable)", VtiEventCatType.Test_Cycle);
                    lock (StepLock)
                    {
                        foreach (var step in _StepsToEnable.ToList())
                            if (!_EnabledSteps.Contains(step))
                            {
                                //VtiEvent.Log.WriteInfo("adding " + step.Name + " to _EnabledSteps", VtiEventCatType.Test_Cycle);
                                _EnabledSteps.Add(step);
                            }
                            else
                                VtiEvent.Log.WriteWarning("attempted to enable already enabled step " + step.Name, VtiEventCatType.Application_Error);
                        _StepsToEnable.Clear();
                    }
                }

                // Interate through the cycle steps
                //foreach (var step in _CycleSteps)
                foreach (var step in _EnabledSteps.ToList())
                {
                    // Process any enabled steps
                    if (step.Enabled)   // still need to check, in case step was disabled from when the list was updated
                    {
                        // Process the step
                        step.Process();

                        // Update the operator prompt for the Time Remaining
                        if (!String.IsNullOrEmpty(step.Prompt))
                        {
                            if (step.TimeDelay != null)
                            {
								string promptExp = Regex.Escape(step.Prompt);
								if(step.TimeRemaining.TotalHours >= 1)
                                    // replace minutes/seconds in time remaining part of operator prompt
                                    _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @"\s+" + VtiLibLocalization.TimeRemaining + @":\s+).*(?=\s\b" + VtiLibLocalization.seconds + @")",
                                        String.Format("{0:0} {1} {2:0} {3} {4:0}",
                                            Math.Floor(step.TimeRemaining.TotalHours),
                                            VtiLibLocalization.hours,
                                            step.TimeRemaining.Minutes,
                                            VtiLibLocalization.minutes,
                                            step.TimeRemaining.Seconds + 1),
                                        step.FontDetails, step.ColorDetails);
                                else if (step.TimeRemaining.Minutes > 0)
                                    // replace minutes/seconds in time remaining part of operator prompt
                                    _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @"\s+" + VtiLibLocalization.TimeRemaining + @":\s+).*(?=\s\b" + VtiLibLocalization.seconds + @")",
                                        String.Format("{0:0} {1} {2:0}",
                                            step.TimeRemaining.Minutes,
                                            VtiLibLocalization.minutes,
                                            step.TimeRemaining.Seconds + 1),
                                        step.FontDetails, step.ColorDetails);
                                else
                                    // replace seconds in time remaining part of operator prompt
                                    _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @"\s+" + VtiLibLocalization.TimeRemaining + @":\s+).*(?=\s\b" + VtiLibLocalization.seconds + @")",
                                        String.Format("{0:0}", step.TimeRemaining.Seconds + 1),
                                        step.FontDetails, step.ColorDetails);
                            }

                            // Update Elapsed Time
                            if (step.DisplayElapsedTime)
                            {
                                string promptExp = Regex.Escape(step.Prompt);
                                if(step.ElapsedTime.TotalHours >= 1) {
									// replace minutes/seconds in time remaining part of operator prompt
									_Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @"\s+" + VtiLibLocalization.ElapsedTime + @":\s+).*(?=\s\b" + VtiLibLocalization.seconds + @")",
                                        String.Format("{0:0} {1} {2:0} {3} {4:0}",
                                            Math.Floor(step.ElapsedTime.TotalHours),
                                            VtiLibLocalization.hours,
                                            step.ElapsedTime.Minutes,
                                            VtiLibLocalization.minutes,
                                            step.ElapsedTime.Seconds + 1),
                                        step.FontDetails, step.ColorDetails);
                                } else if(step.ElapsedTime.Minutes > 0) {
                                    // replace minutes/seconds in time remaining part of operator prompt
                                    _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @"\s+" + VtiLibLocalization.ElapsedTime + @":\s+).*(?=\s\b" + VtiLibLocalization.seconds + @")",
                                        String.Format("{0:0} {1} {2:0}",
                                            step.ElapsedTime.Minutes,
                                            VtiLibLocalization.minutes,
                                            step.ElapsedTime.Seconds + 1),
                                        step.FontDetails, step.ColorDetails);
                                } else {
                                    // replace seconds in time remaining part of operator prompt
                                    _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @"\s+" + VtiLibLocalization.ElapsedTime + @":\s+).*(?=\s\b" + VtiLibLocalization.seconds + @")",
                                        String.Format("{0:0}", step.ElapsedTime.Seconds + 1),
                                        step.FontDetails, step.ColorDetails);
                                }
                            }
                        }

                        // Update the value of any analog signals in the operator prompt
                        if (step.ProcessValue != null)
                            _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + step.ProcessValue.Label + @":\s+).*(?=\s+(?<=\s)(?!\s)" + step.ProcessValue.Units + @")",
                                step.ProcessValue.Value.ToString(step.ProcessValue.Format),
                                step.FontDetails, step.ColorDetails);
                        if (step.SignalsToDisplay != null)
                            foreach (var signal in step.SignalsToDisplay)
                                _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + Regex.Escape(signal.Label) + @":\s+).*(?=\s+(?<=\s)(?!\s)" + Regex.Escape(signal.Units) + @")",
                                    signal.Value.ToString(signal.Format),
                                    step.FontDetails, step.ColorDetails);
                    }
                    Thread.Sleep(0);
                }

                // Sleep for 50ms (to prevent thread from consuming too much CPU time)
                //Thread.Sleep(50);
                _ExitEvent.WaitOne(_WaitDelay);
            }
        }

        private void RemoveFailedPrompt(CycleStep step)
        {
            //if (step.State == VTIWindowsControlLibrary.Enum.CycleStepState.Failed &&
            //    !String.IsNullOrEmpty(VtiLib.Localization.GetString(step.Name + "_PromptFailed")))
            RemovePrompt(step, VtiLib.Localization.GetString(step.Name + "_PromptFailed"));
        }

        private void RemoveMainPrompt(CycleStep step)
        {
            RemovePrompt(step, step.Prompt);
        }

        private void RemovePassPrompt(CycleStep step)
        {
            //if (step.State == VTIWindowsControlLibrary.Enum.CycleStepState.Passed &&
            //    !String.IsNullOrEmpty(VtiLib.Localization.GetString(step.Name + "_PromptPassed")))
            RemovePrompt(step, VtiLib.Localization.GetString(step.Name + "_PromptPassed"));
        }

        private void RemovePrompt(CycleStep step, string prompt)
        {
            // use Regex to find and remove the prompt for this step from the operator prompt
            // without disrupting the rest of the prompt
            if (!String.IsNullOrEmpty(prompt))
            {
                string promptExp = Regex.Escape(prompt);

				// Remove the Time Remaining part
                _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
					@"\s+\b" + VtiLibLocalization.TimeRemaining + @":\s+.*(?<=\s)(?!\s)" + VtiLibLocalization.seconds,
					"");

				// Remove the Elapsed Time part
				_Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
					@"\s+\b" + VtiLibLocalization.ElapsedTime + @":\s+.*(?<=\s)(?!\s)" + VtiLibLocalization.seconds,
					"");

				// use Regex to find and remove the MinSetpoint parameter
				if(step.MinSetpoint != null) {
					_Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
						@"\s+\b" + step.MinSetpoint.DisplayName + @":\s+.*(?<=\s)(?!\s)" + (!string.IsNullOrEmpty(step.MinSetpoint.Units) ? Regex.Escape(step.MinSetpoint.Units) : ""),
						"");
				}
				// use Regex to find and remove the MaxSetpoint parameter
				if(step.MaxSetpoint != null) {
					_Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
						@"\s+\b" + step.MaxSetpoint.DisplayName + @":\s+.*(?<=\s)(?!\s)" + (!string.IsNullOrEmpty(step.MaxSetpoint.Units) ? Regex.Escape(step.MaxSetpoint.Units) : ""),
						"");
				}
				// remove any other numeric parameters from the display
				if(step.ParametersToDisplay != null)
					foreach(var parameter in step.ParametersToDisplay)
						_Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
							@"\s+\b" + parameter.DisplayName + @":\s+.*(?<=\s)(?!\s)" + (!string.IsNullOrEmpty(parameter.Units) ? Regex.Escape(parameter.Units) : ""),
							"");

                // remove any other TimeDelay parameters from the display
                if (step.TimeParametersToDisplay != null)
                    foreach (var parameter in step.TimeParametersToDisplay)
                        _Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
                            @"\s+\b" + parameter.DisplayName + @":\s+.*(?<=\s)(?!\s)" + (!string.IsNullOrEmpty(parameter.Units.ToString()) ? Regex.Escape(parameter.Units.ToString()) : ""),
                            "");

                // use Regex to find and remove the ProcessValue
                if (step.ProcessValue != null) {
					_Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
						@"\s+\b" + Regex.Escape(step.ProcessValue.Label) + @":\s+.*(?<=\s)(?!\s)" + (!string.IsNullOrEmpty(step.ProcessValue.Units) ? Regex.Escape(step.ProcessValue.Units) : ""),
						"");
				}
				// remove any other signals from the display
				if(step.SignalsToDisplay != null)
					foreach(var signal in step.SignalsToDisplay)
						_Prompt.ReplaceRegex(@"(?<=[\r\n]\b" + promptExp + @")" +
							@"\s+\b" + Regex.Escape(signal.Label) + @":\s+.*(?<=\s)(?!\s)" + (!string.IsNullOrEmpty(signal.Units) ? Regex.Escape(signal.Units) : ""),
							"");

				// Finally remove the prompt itself
				_Prompt.ReplaceRegex(@"(?<=[\r\n])\b" + promptExp + @"[\r\n]+", "");
            }
        }

        protected void LeftStr(ref string str, int len)
        {
            try
            {
                if (str == null)
                    return;
                if (str.Length <= len)
                    return;
                str = str.Substring(0, len);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}