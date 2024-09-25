using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Classes.CycleSteps
{
    /// <summary>
    /// Defines a cycle step for the system's test cycle.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each cycle step defines a single atomic operation in the test cycle.  The cycle step
    /// can have a fixed delay/timeout by providing a value for the <see cref="TimeDelay">TimeDelay</see> property, or it
    /// can run indefinitely if the <see cref="TimeDelay">TimeDelay</see> is not supplied.
    /// </para>
    /// <para>
    /// Each cycle step maintains it's own prompt in the operator prompt.  If the <see cref="Prompt">Prompt</see> property
    /// is given a value, the step will use this for it's prompt.  Otherwise, it will look for a string
    /// in the Localization resource with a Name equal to "[CycleStepName]_Prompt".  If no string is
    /// found, the step will not have a prompt.
    /// </para>
    /// <para>
    /// <h3>Time Delay Options</h3>
    /// </para>
    /// <para>
    /// If a cycle step has a prompt, then it will also display the <see cref="TimeRemaining">Time Remaining</see> for the step (if
    /// a <see cref="TimeDelay">TimeDelay</see> is given), along with the values of any <see cref="ProcessValue">ProcessValue</see>,
    /// <see cref="MinSetpoint">MinSetpoint</see>, <see cref="MaxSetpoint">MaxSetpoint</see>,
    /// <see cref="SignalsToDisplay">SignalsToDisplay</see>, or
    /// <see cref="ParametersToDisplay">ParametersToDisplay</see> that are specified.
    /// </para>
    /// <para><em>Note: If the <see cref="DisplayElapsedTime"/> property is set to true, the step will
    /// display the elapsed time rather than the time remaining.</em></para>
    /// <para>
    /// <h3>Pass/Fail Criteria</h3>
    /// </para>
    /// <para>
    /// If a cycle step has a <see cref="TimeDelay">TimeDelay</see>, a <see cref="ProcessValue">ProcessValue</see>, and either a
    /// <see cref="MinSetpoint">MinSetpoint</see>, <see cref="MaxSetpoint">MaxSetpoint</see>, or both,
    /// it will make a pass/fail determination when the <see cref="TimeDelay">TimeDelay</see> elapses.
    /// If the <see cref="ProcessValue">ProcessValue</see> is less than the <see cref="MinSetpoint">MinSetpoint</see>
    /// or greater than the <see cref="MaxSetpoint">MaxSetpoint</see>, then the step will call the <see cref="Failed">Failed</see> event.
    /// Otherwise, it will call the <see cref="Passed">Passed</see> event.
    /// </para>
    /// <para>The following is an example of a step using setpoints that would pass if the chamber evacuated correctly:</para>
    /// <example>
    /// <code>
    /// ChamberEvac = new CycleStep
    /// {
    ///     Sequence = new SequenceStep(Localization.SeqChamberEvac),
    ///     TimeDelay = Config.Time.ChamberEvacDelay,
    ///     ProcessValue = IO.Signals.ChamberPressure,
    ///     MaxSetpoint = Config.Pressure.ChamberEvacSetpoint
    /// }
    /// </code>
    /// </example>
    /// <para>
    /// If <see cref="Criteria">Criteria</see> function expression is given, it will override the function
    /// of the setpoints, and the pass/fail determination will be made based on the result of the function.
    /// </para>
    /// <para>The following is an example of a step using a criteria function (lamda expression in this case)
    /// that would pass if the chamber closed in the allowed amount of time:</para>
    /// <example>
    /// <code>
    /// CloseChamber = new CycleStep
    /// {
    ///     Sequence = new SequenceStep(Localization.SeqCloseChamber),
    ///     TimeDelay = Config.Time.ChamberMotionTimeout,
    ///     Criteria = () => IO.DIn.ChamberDown.Value
    /// };
    /// </code>
    /// </example>
    /// <para>
    /// <h3>Localization Strings</h3>
    /// </para>
    /// <para>Each cycle step can have several associated strings in the client application localization resource file.
    /// Each localization string causes the step to display a certain prompt, or record information to a log file or
    /// database in a particular format.  Each localization string should start with the name of the step and end
    /// with one of the following endings.
    /// </para>
    /// <para>
    /// <list type="table">
    /// <listheader><term>Localization String Ending</term><Description>Description</Description></listheader>
    /// <item>
    ///     <term><em>[No Ending]</em></term>
    ///     <description>A localization string with the exact spelling as the step name causes the system
    ///     to use the specified string value when referring to the step in log file or database entries.</description>
    /// </item>
    /// <item>
    ///     <term>_Prompt</term>
    ///     <description>Causes the specified string value to be displayed in the
    ///     operator prompt when the step is running.</description>
    /// </item>
    /// <item>
    ///     <term>_PromptPassed</term>
    ///     <description>Causes the specified string value to be displayed in the
    ///     operator prompt in GREEN if the step passes.</description>
    /// </item>
    /// <item>
    ///     <term>_PromptFailed</term>
    ///     <description>Causes the specified string value to be displayed in the
    ///     operator prompt in RED if the step fails.</description>
    /// </item>
    /// <item>
    ///     <term>_Passed</term>
    ///     <description>Causes the cycle step to log an entry to the
    ///     <see cref="VTIWindowsControlLibrary.Classes.VtiEvent">VTI Event Log</see>
    ///     with the specified string value for the description if the step passes.
    ///     </description>
    /// </item>
    /// <item>
    ///     <term>_Failed</term>
    ///     <description>Causes the cycle step to log an entry to the
    ///     <see cref="VTIWindowsControlLibrary.Classes.VtiEvent">VTI Event Log</see>
    ///     with the specified string value for the description if the step fails.</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// The following are some examples of localization strings that might be
    /// provided for a Chamber Evacuation cycle step named "ChamberEvac":
    /// </para>
    /// <para>
    /// <list type="table">
    /// <listheader><term>Name</term><description>Value</description></listheader>
    /// <item>
    ///     <term>ChamberEvac</term>
    ///     <description>Chamber Evacuation</description>
    /// </item>
    /// <item>
    ///     <term>ChamberEvac_Prompt</term>
    ///     <description>Chamber is being evacuated...</description>
    /// </item>
    /// <item>
    ///     <term>ChamberEvac_PromptFailed</term>
    ///     <description>NO TEST: Chamber failed to evacuate.  Press ACK to continue...</description>
    /// </item>
    /// <item>
    ///     <term>ChamberEvac_Failed</term>
    ///     <description>NO TEST: Chamber failed to evacuate</description>
    /// </item>
    /// <item>
    ///     <term>ChamberEvac_Passed</term>
    ///     <description>Chamber Evacuation Complete</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para><em>Note: There is no ChamberEvac_PromptPassed in this case, as the system would likely proceed
    /// to the next step with no additional prompt necessary.</em></para>
    /// </remarks>
    /// <seealso cref="CycleStepsBase"/>
    public class CycleStep
    {
        #region Fields (20)

        #region Private Fields (15)

        private Func<bool> _Criteria;
        private Expression<Func<bool>> _CriteriaExpression;
        private bool _DisplayElapsedTime = false;
        private bool _WriteUutRecordDetail = true;
        private Stopwatch _ElapsedTime = new Stopwatch();
        private Boolean _Enabled;
        private bool _LogToDataPlot = true;
        private double _PreElapsedTime;
        private string _Prompt = "";
        private CycleStepState _State;
        private List<NumericParameter> parametersToDisplay = new List<NumericParameter>();
        private List<TimeDelayParameter> timeParametersToDisplay = new List<TimeDelayParameter>();
        private List<AnalogSignal> signalsToDisplay = new List<AnalogSignal>();

        //private TimeDelayParameter _TimeDelay;
        //private double _TimeDelayValue;
        //private TimeDelayParameter _ValveDelay;
        //private TimeDelayParameter _SetpointDelay;
        private Boolean _ValveDelayFired;

        //private double _ValveDelayValue = 0.5;
        //private double _SetpointDelayValue = 0;
        private bool _passOnSetpoints = false;

        //private CycleStepTimeDelayUnits timeDelayUnits = CycleStepTimeDelayUnits.Seconds;

        private bool pvOverMinSp;
        private bool pvUnderMinSp;
        private bool pvOverMaxSp;
        private bool pvUnderMaxSp;

        #endregion Private Fields
        #region Internal Fields (5)

        internal string CycleCommentElapsed;
        internal string CycleCommentFailed;
        internal string CycleCommentPassed;
        internal string CycleCommentStarted;
        internal CycleStepsBase _CycleStepsBase;

        #endregion Internal Fields

        #endregion Fields

        #region Properties (23)

        /// <summary>
        /// Color to be used for the prompt for this step.  If null, the default color will be used.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Color to be used for the detail information for the prompt for this step.  If null, the default color will be used.
        /// </summary>
        public Color ColorDetails { get; set; }

        /// <summary>
        /// Optional function to be used to make a pass/fail determination when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// </summary>
        /// <example>
        /// <code>
        /// CycleStep step1 = new CycleStep
        /// {
        ///     Criteria = () => IO.Signals.SomeSignal > Config.Press.SomeSetpoint
        /// };
        /// </code>
        /// </example>
        public Expression<Func<bool>> Criteria
        {
            get { return _CriteriaExpression; }
            set
            {
                _CriteriaExpression = value;
                _Criteria = _CriteriaExpression.Compile();
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the cycle step details (process value, setpoints, etc.)
        /// should be displayed on a pass or fail.
        /// </summary>
        public bool DisplayDetailsOnPassFail { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate whether the <see cref="ElapsedTime">ElapsedTime</see> of
        /// the cycle step should be displayed in the operator prompt.
        /// </summary>
        public Boolean DisplayElapsedTime
        {
            get { return _DisplayElapsedTime; }
            set { _DisplayElapsedTime = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether a Uut Record Detail
        /// for the cycle step should be automatically created.
        /// </summary>
        public Boolean WriteUutRecordDetail
        {
            get { return _WriteUutRecordDetail; }
            set { _WriteUutRecordDetail = value; }
        }

        /// <summary>
        /// Elapsed time when the cycle step is active
        /// </summary>
        public TimeSpan ElapsedTime { get { return _ElapsedTime.Elapsed; } }

        /// <summary>
        /// Returns true if the cycle step is enabled (active).  Setting the value to true or false will start or stop the step respectively.
        /// </summary>
        public Boolean Enabled
        {
            get { return _Enabled; }
            set
            {
                if (_Enabled != value)
                {
                    _Enabled = value;
                    if (_Enabled)
                    {
                        _ElapsedTime.Reset();
                        _ElapsedTime.Start();
                        _ValveDelayFired = false;

                        if (ProcessValue != null)
                        {
                            if (MinSetpoint != null)
                            {
                                pvOverMinSp = (ProcessValue.Value > MinSetpoint.ProcessValue);
                                pvUnderMinSp = (ProcessValue.Value < MinSetpoint.ProcessValue);
                            }

                            if (MaxSetpoint != null)
                            {
                                pvOverMaxSp = (ProcessValue.Value > MaxSetpoint.ProcessValue);
                                pvUnderMaxSp = (ProcessValue.Value < MaxSetpoint.ProcessValue);
                            }
                        }

                        OnStarted();
                    }
                    else
                    {
                        _ElapsedTime.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Font to be used for the prompt for this step.  If null, the default font will be used.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Font to be used for the detail information for the prompt for this step.  If null, the default font minus 2 point sizes will be used.
        /// </summary>
        public Font FontDetails { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate if the cycle step's events should be logged
        /// to the data plot AIO file.  Defaults to true.
        /// </summary>
        public bool LogToDataPlot
        {
            get { return _LogToDataPlot; }
            set { _LogToDataPlot = value; }
        }

        /// <summary>
        /// <see cref="NumericParameter">NumericParameter</see> to be displayed in the operator prompt,
        /// and to be used for making a pass/fail determination when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// The <see cref="ProcessValue">ProcessValue</see> must be less than this setpoint for the step to pass.
        /// </summary>
        public NumericParameter MaxSetpoint { get; set; }

        /// <summary>
        /// <see cref="NumericParameter">NumericParameter</see> to be displayed in the operator prompt,
        /// and to be used for making a pass/fail determination when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// The <see cref="ProcessValue">ProcessValue</see> must be greater than this setpoint for the step to pass.
        /// </summary>
        public NumericParameter MinSetpoint { get; set; }

        /// <summary>
        /// Gets or sets the name of the cycle step.
        /// </summary>
        public String Name { get; internal set; }

        /// <summary>
        /// A list of additional <see cref="NumericParameter">NumbericParameters</see> to be displayed in the operator prompt.
        /// Usage example: 
        /// step.ParametersToDisplay.Add(Config.Pressure.BasePressureCheckLimit);
        /// </summary>
        public List<NumericParameter> ParametersToDisplay 
        {
            get 
            {
                //get rid of duplicates
                parametersToDisplay = parametersToDisplay.Distinct().ToList();
                return parametersToDisplay;
            }
        }

        /// <summary>
        /// A list of additional <see cref="TimeDelayParameter">NumbericParameters</see> to be displayed in the operator prompt.
        /// Usage example: 
        /// step.TimeParametersToDisplay.Add(Config.Time.PressureDecayFillMinTime);
        /// </summary>
        public List<TimeDelayParameter> TimeParametersToDisplay
        {
            get
            {
                //get rid of duplicates
                timeParametersToDisplay = timeParametersToDisplay.Distinct().ToList();
                return timeParametersToDisplay;
            }
        }

        /// <summary>
        /// <see cref="AnalogSignal">AnalogSignal</see> to be displayed in the operator prompt and compared to the
        /// <see cref="MinSetpoint">MinSetpoint</see> and/or <see cref="MaxSetpoint">MaxSetpoint</see>
        /// when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// </summary>
        public AnalogSignal ProcessValue { get; set; }

        /// <summary>
        /// SideName (Blue or White) for the cycle step
        /// </summary>
        public string SideName { get; set; }

        /// <summary>
        /// Prompt for the cycle step
        /// </summary>
        /// <remarks>
        /// If not specified, the <see cref="CycleStepsBase">CycleStepsBase</see> class will
        /// check in the client application's Localization file for a string with a Name
        /// matching <see cref="Name">Name</see> + "_Prompt".  If found, the value of this
        /// string will be used for the prompt.  If no prompt is specified and no matching string
        /// is found in the Localization file, the step will not have a prompt.
        /// </remarks>
        public string Prompt
        {
            get { return _Prompt; }
            set
            {
                String OldPrompt = _Prompt;
                _Prompt = value;
                OnPromptChanged(OldPrompt, _Prompt);
            }
        }

        /// <summary>
        /// Sequence step to be displayed on the operator form
        /// </summary>
        public SequenceStep Sequence { get; set; }

        /// <summary>
        /// A list of additional <see cref="AnalogSignal">AnalogSignals</see> to be displayed in the operator prompt.
        /// Usage example: 
        /// step.SignalsToDisplay.Add(Config.Pressure.BasePressureCheckLimit);
        /// </summary>
        public List<AnalogSignal> SignalsToDisplay
        {
            get
            {
                //get rid of duplicates
                signalsToDisplay = signalsToDisplay.Distinct().ToList();
                return signalsToDisplay;
            }
        }

        /// <summary>
        /// Current state of the cycle step
        /// </summary>
        public CycleStepState State
        {
            get { return _State; }
            internal set
            {
                _State = value;
                OnStateChanged();
            }
        }

        /// <summary>
        /// Time delay parameter
        /// </summary>
        /// <remarks>If omitted, the value of the <see cref="TimeDelay">TimeDelay</see> value will be used.</remarks>
        public TimeDelayParameter TimeDelay { get; set; }

        //{
        //    get { return _TimeDelay; }
        //    set
        //    {
        //        _TimeDelay = value;
        //        if (_TimeDelay != null)
        //        {
        //            _TimeDelay.ProcessValueChanged += new EventHandler(timeDelayParameter_ProcessValueChanged);
        //            //SetTimeDelayValue();
        //        }
        //    }
        //}

        ///// <summary>
        ///// Indicates whether the time delay of a cycle step should be in seconds, minutes, or hours.
        ///// </summary>
        //public CycleStepTimeDelayUnits TimeDelayUnits
        //{
        //    get { return timeDelayUnits; }
        //    set
        //    {
        //        timeDelayUnits = value;
        //        if (_TimeDelay != null)
        //            SetTimeDelayValue();
        //    }
        //}

        /// <summary>
        /// Time remaining when the cycle step is active
        /// </summary>
        public TimeSpan TimeRemaining { get; private set; }

        /// <summary>
        /// Amount of time after the cycle step starts before the <see cref="ValveDelayElapsed">ValveDelayElapsed</see> event is fired.
        /// </summary>
        /// <remarks>
        /// The default value is 0.5 seconds.
        /// </remarks>
        public TimeDelayParameter ValveDelay { get; set; }

        //{
        //    get { return _ValveDelay ?? (_ValveDelay = new TimeDelayParameter { ProcessValue = _ValveDelayValue }); }
        //    set
        //    {
        //        _ValveDelay = value;
        //        if (_ValveDelay != null)
        //        {
        //            _ValveDelay.ProcessValueChanged += new EventHandler(valveDelayParameter_ProcessValueChanged);
        //            _ValveDelayValue = _ValveDelay.ProcessValue;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets the delay until the setpoints will be checked, if
        /// <see cref="PassOnSetpoints">PassOnSetpoints</see> is enabled.
        /// </summary>
        /// <value>The setpoint delay.</value>
        public TimeDelayParameter SetpointDelay { get; set; }

        //{
        //    get { return _SetpointDelay ?? (_SetpointDelay = new TimeDelayParameter { ProcessValue = _SetpointDelayValue }); }
        //    set
        //    {
        //        _SetpointDelay = value;
        //        if (_SetpointDelay != null)
        //        {
        //            _SetpointDelay.ProcessValueChanged += new EventHandler(setpointDelayParameter_ProcessValueChanged);
        //            _SetpointDelayValue = _SetpointDelay.ProcessValue;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether the cycle step should pass
        /// as soon as the setpoints are satisfied, or wait for the entire time delay.
        /// </summary>
        /// <value><c>true</c> if the cycle step should pass
        /// as soon as the setpoints are satisfied; otherwise, <c>false</c>.</value>
        public bool PassOnSetpoints
        {
            get { return _passOnSetpoints; }
            set { _passOnSetpoints = value; }
        }

        #endregion Properties

        #region Delegates and Events (19)

        #region Delegates (2)

        /// <summary>
        /// Event handler for cycle step events
        /// </summary>
        /// <param name="step">CycleStep</param>
        /// <param name="e">CycleStepEventArgs</param>
        public delegate void CycleStepEventHandler(CycleStep step, CycleStepEventArgs e);

        /// <summary>
        /// Event handler for cycle step prompt events
        /// </summary>
        /// <param name="step">CycleStep</param>
        /// <param name="e">CycleStepPromptEventArgs</param>
        public delegate void CycleStepPromptEventHandler(CycleStep step, CycleStepPromptEventArgs e);

        #endregion Delegates
        #region Events (17)

        /// <summary>
        /// Occurs when the cycle step <see cref="TimeDelay">TimeDelay</see> elapses
        /// </summary>
        /// <remarks>
        /// If the <see cref="TimeDelay">TimeDelay</see> is null, this event will never occur.
        /// </remarks>
        public event CycleStepEventHandler Elapsed;

        /// <summary>
        /// Occurs if the step can make a fail determination when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the step has a <see cref="Criteria">Criteria</see> function, the fail determination will be made
        /// based on the result of that function.
        /// </para>
        /// <para>
        /// If the step doesn't have a <see cref="Criteria">Criteria</see> function, but has a
        /// <see cref="MinSetpoint">MinSetpoint</see> or a <see cref="MaxSetpoint">MaxSetpoint</see>,
        /// then the fail determination will be made based on the setpoint(s).
        /// </para>
        /// <para>
        /// If none of the <see cref="Criteria">Criteria</see>, <see cref="MinSetpoint">MinSetpoint</see>,
        /// or <see cref="MaxSetpoint">MaxSetpoint</see> properties are defined, then the step won't
        /// make a pass/fail determination, and the <see cref="Elapsed">Elapsed</see> event will be
        /// called when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// </para>
        /// </remarks>
        public event CycleStepEventHandler Failed;

        internal event CycleStepEventHandler IntElapsed;

        internal event CycleStepEventHandler IntFailed;

        internal event CycleStepEventHandler IntPassed;

        internal event CycleStepPromptEventHandler IntPromptChanged;

        internal event CycleStepEventHandler IntStarted;

        internal event CycleStepEventHandler IntStateChanged;

        internal event CycleStepEventHandler IntStopped;

        internal event CycleStepEventHandler IntTick;

        /// <summary>
        /// Occurs if the step can make a pass determination when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the step has a <see cref="Criteria">Criteria</see> function, the pass determination will be made
        /// based on the result of that function.
        /// </para>
        /// <para>
        /// If the step doesn't have a <see cref="Criteria">Criteria</see> function, but has a
        /// <see cref="MinSetpoint">MinSetpoint</see> or a <see cref="MaxSetpoint">MaxSetpoint</see>,
        /// then the pass determination will be made based on the setpoint(s).
        /// </para>
        /// <para>
        /// If none of the <see cref="Criteria">Criteria</see>, <see cref="MinSetpoint">MinSetpoint</see>,
        /// or <see cref="MaxSetpoint">MaxSetpoint</see> properties are defined, then the step won't
        /// make a pass/fail determination, and the <see cref="Elapsed">Elapsed</see> event will be
        /// called when the <see cref="TimeDelay">TimeDelay</see> elapses.
        /// </para>
        /// </remarks>
        public event CycleStepEventHandler Passed;

        /// <summary>
        /// Occurs when the <see cref="Prompt">Prompt</see> changes
        /// </summary>
        public event CycleStepPromptEventHandler PromptChanged;

        /// <summary>
        /// Occurs when the cycle step is started
        /// </summary>
        public event CycleStepEventHandler Started;

        /// <summary>
        /// Occurs when the <see cref="State">State</see> property changes
        /// </summary>
        public event CycleStepEventHandler StateChanged;

        /// <summary>
        /// Occurs if the cycle step is stopped with the <see cref="Stop">Stop()</see> method
        /// </summary>
        public event CycleStepEventHandler Stopped;

        /// <summary>
        /// Occurs every time through the cycle step processing loop
        /// </summary>
        public event CycleStepEventHandler Tick;

        /// <summary>
        /// Occurs shortly (typically 0.5 sec) after the cycle step starts
        /// </summary>
        /// <remarks>
        /// The delay is determined by the value of the <see cref="ValveDelay">ValveDelay</see> property, which defaults to 0.5 seconds.
        /// </remarks>
        public event CycleStepEventHandler ValveDelayElapsed;

        /// <summary>
        /// Occurs when <see cref="ProcessValue">process value</see> crossed over the
        /// <see cref="MinSetpoint">minimum setpoint</see>.
        /// </summary>
        public event CycleStepEventHandler OverMinSetpoint;

        /// <summary>
        /// Occurs when <see cref="ProcessValue">process value</see> crossed under the
        /// <see cref="MinSetpoint">minimum setpoint</see>.
        /// </summary>
        public event CycleStepEventHandler UnderMinSetpoint;

        /// <summary>
        /// Occurs when <see cref="ProcessValue">process value</see> crossed over the
        /// <see cref="MaxSetpoint">maximum setpoint</see>.
        /// </summary>
        public event CycleStepEventHandler OverMaxSetpoint;

        /// <summary>
        /// Occurs when <see cref="ProcessValue">process value</see> crossed under the
        /// <see cref="MaxSetpoint">maximum setpoint</see>.
        /// </summary>
        public event CycleStepEventHandler UnderMaxSetpoint;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (21)

        #region Public Methods (6)

        /// <summary>
        /// Disables the cycle step and raises the <see cref="Failed">Failed</see> event
        /// </summary>
        public void Fail()
        {
            this.Enabled = false;
            OnFailed();
        }

        /// <summary>
        /// Disables the cycle step and raises the <see cref="Passed">Passed</see> event
        /// </summary>
        public void Pass()
        {
            this.Enabled = false;
            OnPassed();
        }

        /// <summary>
        /// Resets the cycle step
        /// </summary>
        /// <remarks>
        /// This method disables the cycle step without calling any event handlers.  It sets
        /// the <see cref="SequenceStep.BackColor">BackColor</see> of the <see cref="SequenceStep">Sequence</see>
        /// to the <see cref="System.Drawing.SystemColors.Control">default control background color</see>.
        /// </remarks>
        public void Reset()
        {
            this.Enabled = false;
            if (Sequence != null)
                Sequence.BackColor = System.Drawing.SystemColors.Control;
            this.State = CycleStepState.Idle;
            Console.WriteLine(this.Name + " Reset " + this.SideName);
        }

        /// <summary>
        /// Starts the cycle step
        /// </summary>
        public void Start()
        {
            Start(0);
        }

        /// <summary>
        /// Starts or re-starts a cycle step with a given amount of "elapsed" time to be
        /// removed from the total cycle step delay.
        /// </summary>
        /// <param name="ElapsedTime">Amount of time to be removed from the cycle steps normal delay.</param>
        public void Start(double ElapsedTime)
        {
            _PreElapsedTime = ElapsedTime;
            if (_Enabled)
                this.Reset();  // Disable the step to remove the prompt, etc.
            this.Enabled = true;    // set the flag, which will cause the events to fire
            Console.WriteLine(this.Name + " Start " + this.SideName);
        }

        /// <summary>
        /// Stops the cycle step
        /// </summary>
        public void Stop()
        {
            this.Enabled = false;
            OnStopped();
        }

        #endregion Public Methods
        #region Protected Methods (9)

        /// <summary>
        /// Raises the <see cref="Elapsed">Elapsed</see> event
        /// </summary>
        protected virtual void OnElapsed()
        {
            try
            {
                if (_State == CycleStepState.Elapsed)
                    return;
                _State = CycleStepState.Elapsed;
                if (IntElapsed != null)
                    IntElapsed(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                if (Elapsed != null)
                    Elapsed(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("Elapsed", e);
            }
            catch (Exception e)
            {
                LogCycleError("Elapsed", e);
            }
            OnStateChanged();
        }

        /// <summary>
        /// Raises the <see cref="Failed">Failed</see> event
        /// </summary>
        protected virtual void OnFailed()
        {
            try
            {
                if (_State == CycleStepState.Failed)
                    return;
                _State = CycleStepState.Failed;
                if (IntFailed != null)
                    IntFailed(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                if (Failed != null)
                    Failed(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("Failed", e);
            }
            catch (Exception e)
            {
                LogCycleError("Failed", e);
            }
            OnStateChanged();
        }

        /// <summary>
        /// Raises the <see cref="Passed">Passed</see> event
        /// </summary>
        protected virtual void OnPassed()
        {
            try
            {
                if (_State == CycleStepState.Passed)
                    return;
                _State = CycleStepState.Passed;
                if (IntPassed != null)
                    IntPassed(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                if (Passed != null)
                    Passed(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("Passed", e);
            }
            catch (Exception e)
            {
                LogCycleError("Passed", e);
            }
            OnStateChanged();
        }

        /// <summary>
        /// Raises the <see cref="PromptChanged">PromptChanged</see> event
        /// </summary>
        /// <param name="OldPrompt">Old prompt</param>
        /// <param name="NewPrompt">New prompt</param>
        protected virtual void OnPromptChanged(String OldPrompt, String NewPrompt)
        {
            try
            {
                if (IntPromptChanged != null)
                    IntPromptChanged(this, new CycleStepPromptEventArgs(OldPrompt, _Prompt));
                if (PromptChanged != null)
                    PromptChanged(this, new CycleStepPromptEventArgs(OldPrompt, _Prompt));
            }
            catch (Exception e)
            {
                LogCycleError("PromptChanged", e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Started">Started</see> event
        /// </summary>
        protected virtual void OnStarted()
        {
            try
            {
                if (_State == CycleStepState.InProcess)
                    return;
                _State = CycleStepState.InProcess;
                if (IntStarted != null)
                    IntStarted(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                if (Started != null)
                    Started(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("Started", e);
            }
            catch (Exception e)
            {
                LogCycleError("Started", e);
            }
            OnStateChanged();
        }

        /// <summary>
        /// Raises the <see cref="StateChanged">StateChanged</see> event
        /// </summary>
        protected virtual void OnStateChanged()
        {
            try
            {
                if (IntStateChanged != null)
                    IntStateChanged(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                if (StateChanged != null)
                    StateChanged(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("StateChanged", e);
            }
            catch (Exception e)
            {
                LogCycleError("StateChanged", e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Stopped">Stopped</see> event
        /// </summary>
        protected virtual void OnStopped()
        {
            try
            {
                if (_State == CycleStepState.Idle)
                    return;
                _State = CycleStepState.Idle;
                if (IntStopped != null)
                    IntStopped(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                if (Stopped != null)
                    Stopped(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("Stopped", e);
            }
            catch (Exception e)
            {
                LogCycleError("Stopped", e);
            }
            OnStateChanged();
        }

        /// <summary>
        /// Raises the <see cref="Tick">Tick</see> event
        /// </summary>
        protected virtual void OnTick()
        {
            try
            {
                if (IntTick != null)
                    IntTick(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                if (Tick != null)
                    Tick(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("Tick", e);
            }
            catch (Exception e)
            {
                LogCycleError("Tick", e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ValveDelayElapsed">ValveDelayElapsed</see> event
        /// </summary>
        protected virtual void OnValveDelay()
        {
            try
            {
                if (ValveDelayElapsed != null)
                {
                    ValveDelayElapsed(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
                    _ValveDelayFired = true;
                }
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("ValveDelayElapsed", e);
            }
            catch (Exception e)
            {
                LogCycleError("ValveDelayElapsed", e);
            }
        }

        /// <summary>
        /// Raises the <see cref="OverMinSetpoint">OverMinSetpoint</see> event
        /// </summary>
        protected virtual void OnOverMinSetpoint()
        {
            try
            {
                if (OverMinSetpoint != null)
                    OverMinSetpoint(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("OverMinSetpoint", e);
            }
            catch (Exception e)
            {
                LogCycleError("OverMinSetpoint", e);
            }
        }

        /// <summary>
        /// Raises the <see cref="UnderMinSetpoint">UnderMinSetpoint</see> event
        /// </summary>
        protected virtual void OnUnderMinSetpoint()
        {
            try
            {
                if (UnderMinSetpoint != null)
                    UnderMinSetpoint(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("UnderMinSetpoint", e);
            }
            catch (Exception e)
            {
                LogCycleError("UnderMinSetpoint", e);
            }
        }

        /// <summary>
        /// Raises the <see cref="OverMaxSetpoint">OverMaxSetpoint</see> event
        /// </summary>
        protected virtual void OnOverMaxSetpoint()
        {
            try
            {
                if (OverMaxSetpoint != null)
                    OverMaxSetpoint(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("OverMaxSetpoint", e);
            }
            catch (Exception e)
            {
                LogCycleError("OverMaxSetpoint", e);
            }
        }

        /// <summary>
        /// Raises the <see cref="UnderMaxSetpoint">UnderMaxSetpoint</see> event
        /// </summary>
        protected virtual void OnUnderMaxSetpoint()
        {
            try
            {
                if (UnderMaxSetpoint != null)
                    UnderMaxSetpoint(this, new CycleStepEventArgs(_ElapsedTime.Elapsed));
            }
            catch (DigitalOutputChangeCanceledException e)
            {
                LogCycleOutputInterlockError("UnderMaxSetpoint", e);
            }
            catch (Exception e)
            {
                LogCycleError("UnderMaxSetpoint", e);
            }
        }

        #endregion Protected Methods
        #region Private Methods (5)

        private void LogCycleError(string EventName, Exception e)
        {
            VtiEvent.Log.WriteError(
                       String.Format("An error occurred in the {0} event of the '{1}' cycle step.", EventName, this.Name),
                       VtiEventCatType.Application_Error, e.ToString());
            try
            {
                _CycleStepsBase.CycleStop($"Cycle aborted due to an application error.  Please try again and contact VTI if problem persists. Date/Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.");
            }
            catch (Exception ee)
            {
                VtiEvent.Log.WriteError(
                    String.Format("An error occurred attempting to stop the cycle.", this.Name),
                    VtiEventCatType.Application_Error, ee.ToString());
            }
        }

        private void LogCycleOutputInterlockError(string EventName, DigitalOutputChangeCanceledException e)
        {
            VtiEvent.Log.WriteError(
                       String.Format("A Digital Output Interlock error occurred in the {0} event of the '{1}' cycle step.", EventName, this.Name),
                       VtiEventCatType.Application_Error, e.Reason, e.ToString());
            try
            {
                _CycleStepsBase.CycleStop("Cycle aborted due to a Digital Output Interlock error.  Please try again and contact VTI if problem persists.");
            }
            catch (Exception ee)
            {
                VtiEvent.Log.WriteError(
                    String.Format("An error occurred attempting to stop the cycle.", this.Name),
                    VtiEventCatType.Application_Error, ee.ToString());
            }
        }

        //private void SetTimeDelayValue()
        //{
        //    if (timeDelayUnits == CycleStepTimeDelayUnits.Hours)
        //        _TimeDelayValue = _TimeDelay.ProcessValue * 3600;
        //    else if (timeDelayUnits == CycleStepTimeDelayUnits.Minutes)
        //        _TimeDelayValue = _TimeDelay.ProcessValue * 60;
        //    else
        //        _TimeDelayValue = _TimeDelay.ProcessValue;
        //}

        //void timeDelayParameter_ProcessValueChanged(object sender, EventArgs e)
        //{
        //    SetTimeDelayValue();
        //}

        //void valveDelayParameter_ProcessValueChanged(object sender, EventArgs e)
        //{
        //    _ValveDelayValue = _ValveDelay.ProcessValue;
        //}

        //void setpointDelayParameter_ProcessValueChanged(object sender, EventArgs e)
        //{
        //    _SetpointDelayValue = _SetpointDelay.ProcessValue;
        //}

        #endregion Private Methods
        #region Internal Methods (1)

        internal void Process()
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //Debug.WriteLine("Step: " + this.Name + " started");

            // Fire the ValveDelay event
            if (!_ValveDelayFired &&
                (ValveDelay == null ?
                    _ElapsedTime.Elapsed.TotalSeconds >= 0.5 :
                    _ElapsedTime.Elapsed.TotalSeconds >= ValveDelay.Seconds))
            {
                OnValveDelay();
            }
            // Wait for VlaveDelayElaped event to complete before starting Tick event
            if (_ValveDelayFired || ValveDelayElapsed == null)
            {
                OnTick();
            }
            // Check for over/under the min/max setpoints
            if (ProcessValue != null &&
                (MinSetpoint != null || MaxSetpoint != null) &&
                (SetpointDelay == null || _ElapsedTime.Elapsed.TotalSeconds >= SetpointDelay.Seconds))
            {
                // Check for over/under the MinSetpoint
                if (MinSetpoint != null)
                {
                    if (!pvOverMinSp && ProcessValue.Value > MinSetpoint.ProcessValue)
                    {
                        pvOverMinSp = true;
                        pvUnderMinSp = false;

                        OnOverMinSetpoint();
                    }

                    if (!pvUnderMinSp && ProcessValue.Value < MinSetpoint.ProcessValue)
                    {
                        pvOverMinSp = false;
                        pvUnderMinSp = true;

                        OnUnderMinSetpoint();
                    }
                }

                // Check for over/under the MaxSetpoint
                if (MaxSetpoint != null)
                {
                    if (!pvOverMaxSp && ProcessValue.Value > MaxSetpoint.ProcessValue)
                    {
                        pvOverMaxSp = true;
                        pvUnderMaxSp = false;

                        OnOverMaxSetpoint();
                    }

                    if (!pvUnderMaxSp && ProcessValue.Value < MaxSetpoint.ProcessValue)
                    {
                        pvOverMaxSp = false;
                        pvUnderMaxSp = true;

                        OnUnderMaxSetpoint();
                    }
                }

                if (_passOnSetpoints &&
                    (pvOverMinSp || MinSetpoint == null) &&
                    (pvUnderMaxSp || MaxSetpoint == null))
                {
                    OnPassed();
                    return;
                }
            }

            // If step has a time delay, calculate the time remaining, check for pass/fail if time expired
            if ((TimeDelay != null) && (TimeDelay.Seconds > 0))
            {
                TimeRemaining = TimeSpan.FromSeconds(TimeDelay.Seconds - _PreElapsedTime) - _ElapsedTime.Elapsed;

                // Time has elapsed
                if (TimeRemaining.TotalSeconds <= 0)
                {
                    // Disable this step
                    this.Enabled = false;

                    // If CycleStep has a pass/fail Criteria function, use it to
                    // determine if the step passed or failed
                    if (_Criteria != null)
                    {
                        // Cycle Step Passed
                        if (_Criteria.Invoke())
                            OnPassed();
                        // Cycle Step Failed
                        else
                            OnFailed();
                    }
                    // No pass/fail criteria, so proceed based on setpoints
                    else
                    {
                        // If there is a Minimum setpoint and the ProcessValue is less than the setpoint, the step failed
                        if (MinSetpoint != null && ProcessValue < MinSetpoint)
                            OnFailed();
                        // If there is a Maximum setpoint and the ProcessValue is more than the setpoint, the step failed
                        else if (MaxSetpoint != null && ProcessValue > MaxSetpoint)
                            OnFailed();
                        // If either setpoint is defined and we weren't out of bounds, the step passed
                        else if (MinSetpoint != null || MaxSetpoint != null)
                            OnPassed();
                        // Nothing to determine pass/fail, so call the Elapsed event
                        else
                            OnElapsed();
                    }
                }
            }

            //if (sw.ElapsedMilliseconds > 50)
            //    Debug.WriteLine("Step: " + this.Name + " ended, elapsed == " + sw.ElapsedMilliseconds.ToString());
        }

        #endregion Internal Methods

        #endregion Methods

        #region Nested Classes (2)

        /// <summary>
        /// Event arguments for the any cycle step events
        /// </summary>
        public class CycleStepEventArgs
        {
            #region Fields (1)

            #region Private Fields (1)

            private TimeSpan _ElapsedTime;

            #endregion Private Fields

            #endregion Fields

            #region Constructors (1)

            /// <param name="ElapsedTime">Elapsed time of the cycle step</param>
            public CycleStepEventArgs(TimeSpan ElapsedTime)
            {
                _ElapsedTime = ElapsedTime;
            }

            #endregion Constructors

            #region Properties (1)

            /// <summary>
            /// Elapsed time of the cycle step
            /// </summary>
            public TimeSpan ElapsedTime
            {
                get { return _ElapsedTime; }
            }

            #endregion Properties
        }

        /// <summary>
        /// Event arguments for cycle step prompt events
        /// </summary>
        public class CycleStepPromptEventArgs
        {
            #region Constructors (1)

            /// <param name="OldPrompt">Prompt prior to the change</param>
            /// <param name="NewPrompt">New prompt</param>
            public CycleStepPromptEventArgs(String OldPrompt, String NewPrompt)
            {
                _OldPrompt = OldPrompt;
                _NewPrompt = NewPrompt;
            }

            #endregion Constructors

            #region Properties (2)

            /// <summary>
            /// New prompt
            /// </summary>
            public String NewPrompt
            {
                get { return _NewPrompt; }
            }

            /// <summary>
            /// Old prompt
            /// </summary>
            public String OldPrompt
            {
                get { return _OldPrompt; }
            }

            #endregion Properties

            private String _OldPrompt, _NewPrompt;
        }

        #endregion Nested Classes
    }
}