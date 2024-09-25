using System;
using System.ComponentModel;
using System.Threading;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// RS-485 Interface for an AutomationDirect SOLO Temperature Controller
    /// </summary>
    public class SoloTempController : RS485IOBase //Component, IFormattedAnalogInput
    {
        #region Nested Classes and Enums

        /// <summary>
        /// Parameter Address for the SOLO Temperature Controller
        /// </summary>
        public enum ParameterAddresses
        {
#pragma warning disable 1591
            ProcessValue = 0x1000,
            SetPointValue,
            InputRangeHigh,
            InputRangeLow,
            InputType,
            ControlMode,
            HeatingCooling,
            Output1Period,
            Output2Period,
            ProportionBand,
            IntegralTime,
            DerivativeTime,
            IntegralOffset,
            PDControlOffset,
            ProportionBandCoefficient,
            DeadBand,
            HeatingHysterisis,
            CoolingHysterisis,
            Output1Level,
            Output2Level,
            AnalogHighAdjustment,
            AnalogLowAdjustment,
            PVOffset,
            DecimalPointPosition,
            PIDParameterGroup = 0x101C,
            TargetSV,
            Alarm1 = 0x1020,
            Alarm2,
            Alarm3,
            SystemAlarm,
            Alarm1HighLimit,
            Alarm1LowLimit,
            Alarm2HighLimit,
            Alarm2LowLimit,
            Alarm3HighLimit,
            Alarm3LowLimit,
            LEDStatus,
            PushbuttonStatus,
            LockMode,
            FirwareVersion = 0x102F,
            StartingRampSoakPattern = 0x1030,
            LastStepNumber = 0x1040,
            AdditionalCycles = 0x1050,
            NextPatternNumber = 0x1060,
            RampSoakSV = 0x2000,
            RampSoakTime = 0x2080,

            AT_LEDstatus = 0x0800,
            Output1_LEDstatus,
            Output2_LEDstatus,
            Alarm1_LEDstatus,
            F_LEDstatus,
            C_LEDstatus,
            Alarm2_LEDstatus,
            Alarm3_LEDstatus,
            SetKeyStatus,
            FunctionKeyStatus,
            UpKeyStatus,
            DownKeyStatus,
            Event1InputStatus,
            Event2InputStatus,
            SystemAlarmStatus,
            RampSoakControlStatus,
            OnLineConfigurationEnabled,
            TempUnitSelection,
            DecimalPointDisplaySelection,
            AutoTuning,
            RunMode,
            RampSoakStop,
            RampSoakHold
#pragma warning restore 1591
        }

        /// <summary>
        /// Status codes for the SOLO Temperature Controller
        /// </summary>
        public enum StatusCodes
        {
#pragma warning disable 1591
            Ready = 0,
            Initializing = 0x8002,
            TemperatureSensorNotConnected = 0x8003,
            TemperatureSensorInputError = 0x8004,
            ADCInputError = 0x8006,
            MemoryReadWriteError = 0x8007
#pragma warning restore 1591
        }

        /// <summary>
        /// Thermo-couple types for the SOLO Temperature Controller
        /// </summary>
        public enum ThermoCoupleTypes
        {
#pragma warning disable 1591
            Type_K = 0,
            Type_J,
            Type_T,
            Type_E,
            Type_N,
            Type_R,
            Type_S,
            Type_B,
            Type_L,
            Type_U,
            Type_TXK
#pragma warning restore 1591
        }

        /// <summary>
        /// Control modes for the SOLO Temperature Controller
        /// </summary>
        public enum ControlModes
        {
#pragma warning disable 1591
            PID = 0,
            OnOff,
            Manual,
            RampSoak
#pragma warning restore 1591
        }

        /// <summary>
        /// Heating and Cooling output options for the SOLO Temperature Controller
        /// </summary>
        public enum HeatingCoolingOptions
        {
            /// <summary>
            /// First output is Heating, Second output is Disabled
            /// </summary>
            Heating = 0,

            /// <summary>
            /// First output is Cooling, Second output is Disabled
            /// </summary>
            Cooling,

            /// <summary>
            /// First output is Heating, Second output is Cooling
            /// </summary>
            Heating_Cooling,

            /// <summary>
            /// First output is Cooling, Second output is Heating
            /// </summary>
            Cooling_Heating
        }

        /// <summary>
        /// Alarm codes for the SOLO Temperature Controller
        /// </summary>
        public enum AlarmCodes
        {
#pragma warning disable 1591
            AlarmFunctionDisabled = 0,
            DeviationUpperAndLowerLimit,
            DeviationUpperLimit,
            DeviationLowerLimit,
            ReverseDeviationUpperAndLowerLimit,
            AbsoluteValueUpperAndLowerLimit,
            AbsoluteValueUpperLimit,
            AbsoluteValueLowerLimit,
            DeviationUpperAndLowerLimitWithStandbySequence,
            DeviationUpperLimitWithStandbySequence,
            DeviationLowerLimitWithStandbySequence,
            HysteresisUpperLimitAlarmOutput,
            HysteresisLowerLimitAlarmOutput,
            NotApplicable,
            RampSoakProgramHasEnded,
            ProgramInRampUpStatus,
            ProgramInRampDownStatus,
            ProgramInSoakStatus,
            ProgramInRunStatus
#pragma warning restore 1591
        }

        /// <summary>
        /// Indicates the status of the LED lights on the SOLO Temperature Controller
        /// </summary>
        public class LEDstatus
        {
            private SoloTempController soloTempController;

            /// <summary>
            /// Initializes a new instance of the <see cref="LEDstatus">LEDstatus</see> class
            /// </summary>
            /// <param name="controller">SOLO temperature controller for this class</param>
            public LEDstatus(SoloTempController controller)
            {
                soloTempController = controller;
            }

            /// <summary>
            /// AT LED status
            /// </summary>
            [Browsable(false)]
            public bool AT
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.AT_LEDstatus); }
            }

            /// <summary>
            /// Output1 LED status
            /// </summary>
            [Browsable(false)]
            public bool Output1
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.Output1_LEDstatus); }
            }

            /// <summary>
            /// Output2 LED status
            /// </summary>
            [Browsable(false)]
            public bool Output2
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.Output2_LEDstatus); }
            }

            /// <summary>
            /// Alarm1 LED status
            /// </summary>
            [Browsable(false)]
            public bool Alarm1
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.Alarm1_LEDstatus); }
            }

            /// <summary>
            /// Degrees F LED status
            /// </summary>
            [Browsable(false)]
            public bool F
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.F_LEDstatus); }
            }

            /// <summary>
            /// Degrees C LED status
            /// </summary>
            [Browsable(false)]
            public bool C
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.C_LEDstatus); }
            }

            /// <summary>
            /// Alarm2 LED status
            /// </summary>
            [Browsable(false)]
            public bool Alarm2
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.Alarm2_LEDstatus); }
            }

            /// <summary>
            /// Alarm3 LED status
            /// </summary>
            [Browsable(false)]
            public bool Alarm3
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.Alarm3_LEDstatus); }
            }
        }

        /// <summary>
        /// Represents the "Digital Inputs" available on a <see cref="SoloTempController">SoloTempController</see>
        /// </summary>
        public class TempControllerDigitalInputs
        {
            private TempControllerDigitalInput output1, output2, alarm1, alarm2, alarm3;

            /// <summary>
            /// Initializes a new instance of the <see cref="TempControllerDigitalInputs">TempControllerDigitalInputs</see> class.
            /// </summary>
            public TempControllerDigitalInputs()
            {
                output1 = new TempControllerDigitalInput("Output 1", "Output 1");
                output2 = new TempControllerDigitalInput("Output 2", "Output 2");
                alarm1 = new TempControllerDigitalInput("Alarm 1", "Alarm 1");
                alarm2 = new TempControllerDigitalInput("Alarm 2", "Alarm 2");
                alarm3 = new TempControllerDigitalInput("Alarm 3", "Alarm 3");
            }

            /// <summary>
            /// Gets the <see cref="TempControllerDigitalInput">Digital Input</see>
            /// associated with Output 1 of the <see cref="SoloTempController">SoloTempController</see>
            /// </summary>
            public TempControllerDigitalInput Output1
            {
                get { return output1; }
            }

            /// <summary>
            /// Gets the <see cref="TempControllerDigitalInput">Digital Input</see>
            /// associated with Output 2 of the <see cref="SoloTempController">SoloTempController</see>
            /// </summary>
            public TempControllerDigitalInput Output2
            {
                get { return output2; }
            }

            /// <summary>
            /// Gets the <see cref="TempControllerDigitalInput">Digital Input</see>
            /// associated with Alarm 1 of the <see cref="SoloTempController">SoloTempController</see>
            /// </summary>
            public TempControllerDigitalInput Alarm1
            {
                get { return alarm1; }
            }

            /// <summary>
            /// Gets the <see cref="TempControllerDigitalInput">Digital Input</see>
            /// associated with Alarm 2 of the <see cref="SoloTempController">SoloTempController</see>
            /// </summary>
            public TempControllerDigitalInput Alarm2
            {
                get { return alarm2; }
            }

            /// <summary>
            /// Gets the <see cref="TempControllerDigitalInput">Digital Input</see>
            /// associated with Alarm 3 of the <see cref="SoloTempController">SoloTempController</see>
            /// </summary>
            public TempControllerDigitalInput Alarm3
            {
                get { return alarm3; }
            }
        }

        /// <summary>
        /// Indicates the status of the inputs to the SOLO Temperature Controller
        /// </summary>
        public class InputStatus
        {
            private SoloTempController soloTempController;

            /// <summary>
            /// Initializes a new instance of the <see cref="InputStatus">InputStatus</see> class
            /// </summary>
            /// <param name="controller">SOLO temperature controller for this class</param>
            public InputStatus(SoloTempController controller)
            {
                soloTempController = controller;
            }

            /// <summary>
            /// Indicates whether the Set key is pressed.
            /// </summary>
            [Browsable(false)]
            public bool SetKey
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.SetKeyStatus); }
            }

            /// <summary>
            /// Indicates whether the Function key is pressed.
            /// </summary>
            [Browsable(false)]
            public bool FunctionKey
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.FunctionKeyStatus); }
            }

            /// <summary>
            /// Indicates whether the Up key is pressed.
            /// </summary>
            [Browsable(false)]
            public bool UpKey
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.UpKeyStatus); }
            }

            /// <summary>
            /// Indicates whether the Down key is pressed.
            /// </summary>
            [Browsable(false)]
            public bool DownKey
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.DownKeyStatus); }
            }

            /// <summary>
            /// Indicates whether the Event1 input is on.
            /// </summary>
            [Browsable(false)]
            public bool Event1
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.Event1InputStatus); }
            }

            /// <summary>
            /// Indicates whether the Event2 input is on.
            /// </summary>
            [Browsable(false)]
            public bool Event2
            {
                get { return soloTempController.ReadBitRegister(ParameterAddresses.Event2InputStatus); }
            }
        }

        /// <summary>
        /// Represents a digital input for the <see cref="SoloTempController">SoloTempController</see> class.
        /// </summary>
        /// <remarks>
        /// Implements the <see cref="IDigitalInput">IDigitalInput</see> interface.
        /// </remarks>
        public class TempControllerDigitalInput : IDigitalInput
        {
            private bool value;

            /// <summary>
            /// Initializes a new instance of the <see cref="TempControllerDigitalInput">TempControllerDigitalInput</see> class.
            /// </summary>
            /// <param name="Name">Name of the digital input.</param>
            /// <param name="Description">Description of the digital input.</param>
            public TempControllerDigitalInput(String Name, String Description)
            {
                this.Name = Name;
                this.Description = Description;
            }

            #region IDigitalIO Members

            /// <summary>
            /// Gets the name of the digital input.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the description of the digital input
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the value of the digital input
            /// </summary>
            public bool Value
            {
                get { return value; }
                set
                {
                    this.value = value;
                    OnValueChanged();
                }
            }

            /// <summary>
            /// Gets a value to indicate that this is a digital input.
            /// </summary>
            public bool IsInput
            {
                get { return true; }
            }

            /// <summary>
            /// Gets a value to indicate that this digital input is available.
            /// </summary>
            public bool IsAvailable
            {
                get { return true; }
            }

            /// <summary>
            /// Occurs when the <see cref="Value">Value</see> changes.
            /// </summary>
            public event EventHandler ValueChanged;

            /// <summary>
            /// Raises the <see cref="ValueChanged">ValueChanged</see> event.
            /// </summary>
            public void OnValueChanged()
            {
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }

            #endregion IDigitalIO Members
        }

        #endregion Nested Classes and Enums

        #region Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> changes
        /// </summary>
        public override event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        protected override void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> changes
        /// </summary>
        public override event EventHandler RawValueChanged;

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        protected override void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        /// <summary>
        /// Occurs when the <see cref="SystemAlarmStatus">SystemAlarmStatus</see> changes.
        /// </summary>
        public event EventHandler SystemAlarmChanged;

        /// <summary>
        /// Raises the <see cref="SystemAlarmChanged">SystemAlarmChanged</see> event.
        /// </summary>
        protected void OnSystemAlarmChanged()
        {
            if (SystemAlarmChanged != null)
                SystemAlarmChanged(this, null);
        }

        #endregion Event Handlers

        #region Globals

        private String format = string.Empty;
        private String units = string.Empty;
        private Double processValue;
        private StatusCodes status;
        private LEDstatus ledStatus;
        private InputStatus inputs;

        //private Boolean commError;
        private Double min = Double.NaN, max = Double.NaN;

        //private int retries = 0;
        private Boolean systemAlarmStatus = false;

        private Boolean systemAlarmStatusTemp = false;
        private TempControllerDigitalInputs digitalInputs;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PVGC5Controller">PVGC5Controller</see> class
        /// </summary>
        public SoloTempController()
          : base()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PVGC5Controller">PVGC5Controller</see> class
        /// </summary>
        /// <param name="container">Container for this object</param>
        public SoloTempController(IContainer container)
          : base(container)
        {
            Init();
        }

        private void Init()
        {
            ledStatus = new LEDstatus(this);
            inputs = new InputStatus(this);
            digitalInputs = new TempControllerDigitalInputs();
        }

        #endregion Construction

        #region Events

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BackgroundProcess();
        }

        #endregion Events

        #region Private Methods

        private ushort ReadParameter(ParameterAddresses Address)
        {
            ushort retVal = 0;
            if (this.IsAvailable && !commError)
            {
                if (Monitor.TryEnter(this.ModbusInterface.SerialLock, 500))
                {
                    try
                    {
                        ushort[] registers = this.ModbusInterface.ReadHoldingRegisters(this.SlaveID, (ushort)Address, 1);
                        retVal = registers[0];
                    }
                    catch (Exception e)
                    {
                        commError = true;
                        VtiEvent.Log.WriteError(
                            String.Format("Error communicating with {0}.", this.Name),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            e.ToString());
                    }
                    finally
                    {
                        Monitor.Exit(this.ModbusInterface.SerialLock);
                    }
                }
            }
            return retVal;
        }

        public void WriteParameter(ParameterAddresses Address, ushort Value)
        {
            if (this.IsAvailable && !commError)
            {
                //if (Monitor.TryEnter(this.ModbusInterface.SerialLock, 500))
                {
                    try
                    {
                        this.ModbusInterface.WriteSingleRegister(this.SlaveID, (ushort)Address, Value);
                    }
                    catch (Exception e)
                    {
                        commError = true;
                        VtiEvent.Log.WriteError(
                            String.Format("Error communicating with {0}.", this.Name),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            e.ToString());
                    }
                    finally
                    {
                        //Monitor.Exit(this.ModbusInterface.SerialLock);
                    }
                }
            }
        }

        private bool ReadBitRegister(ParameterAddresses Address)
        {
            bool retVal = false;
            if (this.IsAvailable && !commError)
            {
                if (Monitor.TryEnter(this.ModbusInterface.SerialLock, 500))
                {
                    try
                    {
                        bool[] registers = this.ModbusInterface.ReadInputs(this.SlaveID, (ushort)Address, 1);
                        retVal = registers[0];
                    }
                    catch (Exception e)
                    {
                        commError = true;
                        VtiEvent.Log.WriteError(
                            String.Format("Error communicating with {0}.", this.Name),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            e.ToString());
                    }
                    finally
                    {
                        Monitor.Exit(this.ModbusInterface.SerialLock);
                    }
                }
            }
            return retVal;
        }

        private void WriteBitRegister(ParameterAddresses Address, bool Value)
        {
            if (this.IsAvailable && !commError)
            {
                if (Monitor.TryEnter(this.ModbusInterface.SerialLock, 500))
                {
                    try
                    {
                        this.ModbusInterface.WriteSingleCoil(this.SlaveID, (ushort)Address, Value);
                    }
                    catch (Exception e)
                    {
                        commError = true;
                        VtiEvent.Log.WriteError(
                            String.Format("Error writing to {0}.", this.Name),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            e.ToString());
                    }
                    finally
                    {
                        Monitor.Exit(this.ModbusInterface.SerialLock);
                    }
                }
            }
        }

        private double ParamToDouble(ushort val, int DecimalPlaces)
        {
            short tmp2 = (short)val;
            return (double)tmp2 / Math.Pow(10, DecimalPlaces);
        }

        private ushort DoubleToParam(double val, int DecimalPlaces)
        {
            short tmp = (short)(val * Math.Pow(10, DecimalPlaces));
            return (ushort)tmp;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the SOLO Temperature Controller
        /// </summary>
        public override void Process()
        {
            try
            {
                ushort tmp = ReadParameter(ParameterAddresses.ProcessValue);
                if (commError)
                {
                    processValue = Double.NaN;
                }
                else
                {
                    // Error code returned
                    // 0x8000 or greater is a negative temperature
                    //  and is OK for Solo Temperature Controller
                    //if (tmp >= 0x8000) {
                    //  status = (StatusCodes)tmp;
                    //  processValue = Double.NaN;
                    //} else {
                    processValue = ParamToDouble(tmp, 1);
                    //}
                }

                Thread.Sleep(10);
                systemAlarmStatus = ReadBitRegister(ParameterAddresses.SystemAlarmStatus);

                Thread.Sleep(10);
                digitalInputs.Output1.Value = ledStatus.Output1;

                Thread.Sleep(10);
                digitalInputs.Output2.Value = ledStatus.Output2;

                Thread.Sleep(10);
                digitalInputs.Alarm1.Value = ledStatus.Alarm1;

                Thread.Sleep(10);
                digitalInputs.Alarm2.Value = ledStatus.Alarm2;

                Thread.Sleep(10);
                digitalInputs.Alarm3.Value = ledStatus.Alarm3;

                if (!backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
            }
            catch { }
        }

        /// <summary>
        /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
        /// method on the main thread.  Also invokes the <see cref="OnSystemAlarmChanged">OnSystemAlarmChanged</see> method.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
            if (systemAlarmStatus != systemAlarmStatusTemp)
            {
                systemAlarmStatusTemp = systemAlarmStatus;
                OnSystemAlarmChanged();
            }
        }

        //public void ResetCommError()
        //{
        //    commError = false;
        //}

        #endregion Public Methods

        #region Public Properties

        //public Boolean CommError
        //{
        //    get { return commError; }
        //}

        /// <summary>
        /// Process Value (temperature)
        /// </summary>
        public override double Value
        {
            get { return processValue; }
            internal set { processValue = value; }
        }

        /// <summary>
        /// Status code
        /// </summary>
        public StatusCodes Status
        {
            get { return status; }
        }

        /// <summary>
        /// Current Set Point Value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double SetPointValue
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.SetPointValue), 1); }
            set { WriteParameter(ParameterAddresses.SetPointValue, DoubleToParam(value, 1)); }
        }

        /// <summary>
        /// Thermo-couple type
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ThermoCoupleTypes ThermoCoupleType
        {
            get { return (ThermoCoupleTypes)ReadParameter(ParameterAddresses.InputType); }
            set { WriteParameter(ParameterAddresses.InputType, (ushort)value); }
        }

        /// <summary>
        /// Current control mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ControlModes ControlMode
        {
            get { return (ControlModes)ReadParameter(ParameterAddresses.ControlMode); }
            set { WriteParameter(ParameterAddresses.ControlMode, (ushort)value); }
        }

        /// <summary>
        /// Heating and Cooling output options
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HeatingCoolingOptions HeatingCooling
        {
            get { return (HeatingCoolingOptions)ReadParameter(ParameterAddresses.HeatingCooling); }
            set { WriteParameter(ParameterAddresses.HeatingCooling, (ushort)value); }
        }

        /// <summary>
        /// Output 1 Period (0.5 sec, 1 - 99 sec)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Output1Period
        {
            get
            {
                ushort tmp = ReadParameter(ParameterAddresses.Output1Period);
                if (tmp == 0) return 0.5;
                else return tmp;
            }
            set
            {
                if (value < 1) WriteParameter(ParameterAddresses.Output1Period, 0);
                else if (value > 99) WriteParameter(ParameterAddresses.Output1Period, 99);
                else WriteParameter(ParameterAddresses.Output1Period, (ushort)value);
            }
        }

        /// <summary>
        /// Output 2 Period (0.5 sec, 1 - 99 sec)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Output2Period
        {
            get
            {
                ushort tmp = ReadParameter(ParameterAddresses.Output2Period);
                if (tmp == 0) return 0.5;
                else return tmp;
            }
            set
            {
                if (value < 1) WriteParameter(ParameterAddresses.Output2Period, 0);
                else if (value > 99) WriteParameter(ParameterAddresses.Output2Period, 99);
                else WriteParameter(ParameterAddresses.Output2Period, (ushort)value);
            }
        }

        /// <summary>
        /// Proportion Band (0.1 - 99.9)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ProportionBand
        {
            get { return (double)ReadParameter(ParameterAddresses.ProportionBand) / 10; }
            set { WriteParameter(ParameterAddresses.ProportionBand, (ushort)(value * 10)); }
        }

        /// <summary>
        /// Integral Time (0 - 9999)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort IntegralTime
        {
            get { return ReadParameter(ParameterAddresses.IntegralTime); }
            set { WriteParameter(ParameterAddresses.IntegralTime, value); }
        }

        /// <summary>
        /// Derivative Time (0 - 9999)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort DerivativeTime
        {
            get { return ReadParameter(ParameterAddresses.DerivativeTime); }
            set { WriteParameter(ParameterAddresses.DerivativeTime, value); }
        }

        /// <summary>
        /// Integral Offset (0.0 - 100.0%)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double IntegralOffset
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.IntegralOffset), 1); }
            set { WriteParameter(ParameterAddresses.IntegralOffset, DoubleToParam(value, 1)); }
        }

        /// <summary>
        /// PD Control Offset (0.0 - 100.0%)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double PDControlOffset
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.PDControlOffset), 1); }
            set { WriteParameter(ParameterAddresses.PDControlOffset, DoubleToParam(value, 1)); }
        }

        /// <summary>
        /// Proportion Band Coefficient (0.01 - 99.99)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ProportionBandCoefficient
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.ProportionBandCoefficient), 2); }
            set { WriteParameter(ParameterAddresses.ProportionBandCoefficient, DoubleToParam(value, 2)); }
        }

        /// <summary>
        /// Dead Band (-999 - 9999)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short DeadBand
        {
            get { return (short)ReadParameter(ParameterAddresses.DeadBand); }
            set { WriteParameter(ParameterAddresses.DeadBand, (ushort)value); }
        }

        /// <summary>
        /// Heating Hysteresis (0 - 9999)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort HeatingHysterisis
        {
            get { return ReadParameter(ParameterAddresses.HeatingHysterisis); }
            set
            {
                if (value < 0 || value > 9999) throw new Exception("HeatingHysterisis value out of range");
                WriteParameter(ParameterAddresses.HeatingHysterisis, value);
            }
        }

        /// <summary>
        /// Cooling Hysteresis (0 - 9999)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort CoolingHysterisis
        {
            get { return ReadParameter(ParameterAddresses.CoolingHysterisis); }
            set
            {
                if (value < 0 || value > 9999) throw new Exception("CoolingHysterisis value out of range");
                WriteParameter(ParameterAddresses.CoolingHysterisis, value);
            }
        }

        /// <summary>
        /// <para>Output 1 Level</para>
        /// <para>Unit is 0.1%, write operation is valid under manual tuning mode only.</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Output1Level
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.Output1Level), 1); }
            set
            {
                if (value < 0 || value > 100) throw new Exception("Output1Level value out of range");
                WriteParameter(ParameterAddresses.Output1Level, DoubleToParam(value, 1));
            }
        }

        /// <summary>
        /// <para>Output 2 Level</para>
        /// <para>Unit is 0.1%, write operation is valid under manual tuning mode only.</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Output2Level
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.Output2Level), 1); }
            set
            {
                if (value < 0 || value > 100) throw new Exception("Output2Level value out of range");
                WriteParameter(ParameterAddresses.Output2Level, DoubleToParam(value, 1));
            }
        }

        /// <summary>
        /// <para>Analog High Adjustment</para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>1 Unit = 2.8 uA (Current Output)</description>
        ///     </item>
        ///     <item>
        ///         <description>1 Unit = 1.3 mV (Linear Voltage Output)</description>
        ///     </item>
        /// </list>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short AnalogHighAdjustment
        {
            get { return (short)ReadParameter(ParameterAddresses.AnalogHighAdjustment); }
            set { WriteParameter(ParameterAddresses.AnalogHighAdjustment, (ushort)value); }
        }

        /// <summary>
        /// <para>Analog Low Adjustment</para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>1 Unit = 2.8 uA (Current Output)</description>
        ///     </item>
        ///     <item>
        ///         <description>1 Unit = 1.3 mV (Linear Voltage Output)</description>
        ///     </item>
        /// </list>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short AnalogLowAdjustment
        {
            get { return (short)ReadParameter(ParameterAddresses.AnalogLowAdjustment); }
            set { WriteParameter(ParameterAddresses.AnalogLowAdjustment, (ushort)value); }
        }

        /// <summary>
        /// PV Offset (-999 - 999)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short PVOffset
        {
            get { return (short)ReadParameter(ParameterAddresses.PVOffset); }
            set
            {
                if (value < -999 || value > 999) throw new Exception("PVOffset value out of range");
                WriteParameter(ParameterAddresses.PVOffset, (ushort)value);
            }
        }

        /// <summary>
        /// PID Parameter Group
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort PIDParameterGroup
        {
            get { return ReadParameter(ParameterAddresses.PIDParameterGroup); }
            set
            {
                if (value > 4) throw new Exception("PIDParameterGroup value out of range");
                WriteParameter(ParameterAddresses.PIDParameterGroup, value);
            }
        }

        /// <summary>
        /// <para>Target SV</para>
        /// <para>Only valid within available range, unit: 0.1 scale</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort TargetSV
        {
            get { return ReadParameter(ParameterAddresses.TargetSV); }
            set { WriteParameter(ParameterAddresses.TargetSV, value); }
        }

        /// <summary>
        /// Alarm 1
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlarmCodes Alarm1
        {
            get { return (AlarmCodes)ReadParameter(ParameterAddresses.Alarm1); }
            set { WriteParameter(ParameterAddresses.Alarm1, (ushort)value); }
        }

        /// <summary>
        /// Alarm 2
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlarmCodes Alarm2
        {
            get { return (AlarmCodes)ReadParameter(ParameterAddresses.Alarm2); }
            set { WriteParameter(ParameterAddresses.Alarm2, (ushort)value); }
        }

        /// <summary>
        /// Alarm 3
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlarmCodes Alarm3
        {
            get { return (AlarmCodes)ReadParameter(ParameterAddresses.Alarm3); }
            set { WriteParameter(ParameterAddresses.Alarm3, (ushort)value); }
        }

        /// <summary>
        /// <para>System Alarm</para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>0 = System Alarm is disabled. (default)</description>
        ///     </item>
        ///     <item>
        ///         <description>1 - 3 = Alarm number to also be used as system alarm.</description>
        ///     </item>
        /// </list>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort SystemAlarm
        {
            get { return ReadParameter(ParameterAddresses.SystemAlarm); }
            set
            {
                if (value > 3) throw new Exception("SystemAlarm value out of range");
                WriteParameter(ParameterAddresses.SystemAlarm, value);
            }
        }

        /// <summary>
        /// Alarm 1 High Limit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Alarm1HighLimit
        {
            get { return (short)ReadParameter(ParameterAddresses.Alarm1HighLimit); }
            set { WriteParameter(ParameterAddresses.Alarm1HighLimit, (ushort)value); }
        }

        /// <summary>
        /// Alarm 1 Low Limit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Alarm1LowLimit
        {
            get { return (short)ReadParameter(ParameterAddresses.Alarm1LowLimit); }
            set { WriteParameter(ParameterAddresses.Alarm1LowLimit, (ushort)value); }
        }

        /// <summary>
        /// Alarm 2 High Limit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Alarm2HighLimit
        {
            get { return (short)ReadParameter(ParameterAddresses.Alarm2HighLimit); }
            set { WriteParameter(ParameterAddresses.Alarm2HighLimit, (ushort)value); }
        }

        /// <summary>
        /// Alarm 2 Low Limit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Alarm2LowLimit
        {
            get { return (short)ReadParameter(ParameterAddresses.Alarm2LowLimit); }
            set { WriteParameter(ParameterAddresses.Alarm2LowLimit, (ushort)value); }
        }

        /// <summary>
        /// Alarm 3 High Limit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Alarm3HighLimit
        {
            get { return (short)ReadParameter(ParameterAddresses.Alarm3HighLimit); }
            set { WriteParameter(ParameterAddresses.Alarm3HighLimit, (ushort)value); }
        }

        /// <summary>
        /// Alarm 3 Low Limit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Alarm3LowLimit
        {
            get { return (short)ReadParameter(ParameterAddresses.Alarm3LowLimit); }
            set { WriteParameter(ParameterAddresses.Alarm3LowLimit, (ushort)value); }
        }

        /// <summary>
        /// <para>Pushbutton Status</para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>Bit 0 = SET</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 1 = Rotate</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 2 = Up</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 3 = Down</description>
        ///     </item>
        /// </list>
        /// <para>If the button is pressed, the bit is off.</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort PushbuttonStatus
        {
            get { return ReadParameter(ParameterAddresses.PushbuttonStatus); }
        }

        /// <summary>
        /// <para>LED Status</para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>Bit 0 = ALM3</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 1 = ALM2</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 2 = Degrees F</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 3 = Degrees C</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 4 = ALM1</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 5 = OUT2</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 6 = OUT1</description>
        ///     </item>
        ///     <item>
        ///         <description>Bit 7 = AT</description>
        ///     </item>
        /// </list>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort LEDStatus
        {
            get { return ReadParameter(ParameterAddresses.LEDStatus); }
        }

        /// <summary>
        /// <para>Lock Mode</para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>0 = OFF</description>
        ///     </item>
        ///     <item>
        ///         <description>1 = Lock Mode 1</description>
        ///     </item>
        ///     <item>
        ///         <description>11 = Lock Mode 2</description>
        ///     </item>
        /// </list>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort LockMode
        {
            get { return ReadParameter(ParameterAddresses.LockMode); }
            set { WriteParameter(ParameterAddresses.LockMode, value); }
        }

        /// <summary>
        /// Firmware Version
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FirmwareVersion
        {
            get
            {
                ushort tmp = ReadParameter(ParameterAddresses.FirwareVersion);
                return String.Format("V{0:0.00}", (double)tmp / 100);
            }
        }

        /// <summary>
        /// Starting Ramp / Soak Pattern (0 - 7)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort StartingRampSoakPattern
        {
            get { return ReadParameter(ParameterAddresses.StartingRampSoakPattern); }
            set
            {
                if (value > 7) throw new Exception("StartingRampSoakPattern value out of range");
                WriteParameter(ParameterAddresses.StartingRampSoakPattern, value);
            }
        }

        /// <summary>
        /// <para>Last Step Number</para>
        /// <para>0 - 7 = The last step number of the pattern</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort LastStepNumber
        {
            get { return ReadParameter(ParameterAddresses.LastStepNumber); }
            set
            {
                if (value > 7) throw new Exception("LastStepNumber value out of range");
                WriteParameter(ParameterAddresses.LastStepNumber, value);
            }
        }

        /// <summary>
        /// Additional Cycles (0 - 199)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort AdditionalCycles
        {
            get { return ReadParameter(ParameterAddresses.AdditionalCycles); }
            set
            {
                if (value > 199) throw new Exception("AdditionalCycles value out of range");
                WriteParameter(ParameterAddresses.AdditionalCycles, value);
            }
        }

        /// <summary>
        /// <para>Next Pattern Number</para>
        /// <para>0 - 7 = Next pattern number</para>
        /// <para>8 = There is no next pattern.</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort NextPatternNumber
        {
            get { return ReadParameter(ParameterAddresses.NextPatternNumber); }
            set
            {
                if (value > 7) throw new Exception("NextPatternNumber value out of range");
                WriteParameter(ParameterAddresses.NextPatternNumber, value);
            }
        }

        /// <summary>
        /// Ramp / Soak SV (-999 - 9999)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short RampSoakSV
        {
            get { return (short)ReadParameter(ParameterAddresses.RampSoakSV); }
            set
            {
                if (value < -999 || value > 9999) throw new Exception("RampSoakSV value out of range");
                WriteParameter(ParameterAddresses.RampSoakSV, (ushort)value);
            }
        }

        /// <summary>
        /// <para>Ramp / Soak Time</para>
        /// <para>0 - 1500 (15 hours 0 minutes)</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort RampSoakTime
        {
            get { return ReadParameter(ParameterAddresses.RampSoakTime); }
            set
            {
                if (value > 1500) throw new Exception("RampSoakTime value out of range");
                if (value % 100 > 60) throw new Exception("RampSoakTime value is invalid"); // must be HHMM, so if the last 2 digits are > 60, there's a problem
                WriteParameter(ParameterAddresses.RampSoakTime, value);
            }
        }

        /// <summary>
        /// LED Status
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LEDstatus LEDs
        {
            get { return ledStatus; }
        }

        /// <summary>
        /// Input Status
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InputStatus Inputs
        {
            get { return inputs; }
        }

        /// <summary>
        /// System Alarm Status
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SystemAlarmStatus
        {
            get { return systemAlarmStatus; }
        }

        /// <summary>
        /// Ramp Soak Control Status
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RampSoakControlStatus
        {
            get { return ReadBitRegister(ParameterAddresses.RampSoakControlStatus); }
        }

        /// <summary>
        /// On-Line Configuration Enabled
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OnLineConfigurationEnabled
        {
            get { return ReadBitRegister(ParameterAddresses.OnLineConfigurationEnabled); }
            set { WriteBitRegister(ParameterAddresses.OnLineConfigurationEnabled, value); }
        }

        /// <summary>
        /// Temperature Display in Degrees C
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TemperatureDisplayInC
        {
            get { return ReadBitRegister(ParameterAddresses.TempUnitSelection); }
            set
            {
                WriteBitRegister(ParameterAddresses.TempUnitSelection, value);
                units = string.Empty;
            }
        }

        /// <summary>
        /// Display 10ths digit decimal
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DecimalPointDisplayTenths
        {
            get { return ReadBitRegister(ParameterAddresses.DecimalPointDisplaySelection); }
            set { WriteBitRegister(ParameterAddresses.DecimalPointDisplaySelection, value); }
        }

        /// <summary>
        /// Auto Tuning Enabled
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoTuning
        {
            get { return ReadBitRegister(ParameterAddresses.AutoTuning); }
            set { WriteBitRegister(ParameterAddresses.AutoTuning, value); }
        }

        /// <summary>
        /// Run Mode Enabled
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RunMode
        {
            get { return ReadBitRegister(ParameterAddresses.RunMode); }
            set { WriteBitRegister(ParameterAddresses.RunMode, value); }
        }

        /// <summary>
        /// Stop the Ramp / Soak Control
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RampSoakStop
        {
            get { return ReadBitRegister(ParameterAddresses.RampSoakStop); }
            set { WriteBitRegister(ParameterAddresses.RampSoakStop, value); }
        }

        /// <summary>
        /// Hold the Ramp / Soak Control
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RampSoakHold
        {
            get { return ReadBitRegister(ParameterAddresses.RampSoakHold); }
            set { WriteBitRegister(ParameterAddresses.RampSoakHold, value); }
        }

        /// <summary>
        /// Value formatted to match the front panel display including the temperature units
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string FormattedValue
        {
            get
            {
                if (Double.IsNaN(this.Value))
                    return "ERROR";
                else
                    return this.Value.ToString(this.Format) + " " + this.Units;
            }
        }

        /// <summary>
        /// Name for the device
        /// </summary>
        public override string Name
        {
            get { return "SOLO Temperature Controller on port " + this.ModbusInterface.SerialPort.PortName + " with ID " + this.SlaveID; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        [Browsable(false)]
        public override double RawValue
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Minimum value
        /// </summary>
        /// <remarks>Identical to <see cref="InputRangeLow">InputRangeLow</see></remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Min
        {
            get
            {
                if (Double.IsNaN(min))
                    min = InputRangeLow;

                return min;
            }
        }

        /// <summary>
        /// <para>Input Range Low</para>
        /// <para>The data content should not be lower that the temperature range.</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double InputRangeLow
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.InputRangeLow), 1); }
            set { WriteParameter(ParameterAddresses.InputRangeLow, DoubleToParam(value, 1)); }
        }

        /// <summary>
        /// Maximum value
        /// </summary>
        /// <remarks>Identical to <see cref="InputRangeHigh">InputRangeHigh</see></remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Max
        {
            get
            {
                if (Double.IsNaN(max))
                    max = InputRangeHigh;

                return max;
            }
        }

        /// <summary>
        /// <para>Input Range High</para>
        /// <para>The data content should not be higher that the temperature range.</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double InputRangeHigh
        {
            get { return ParamToDouble(ReadParameter(ParameterAddresses.InputRangeHigh), 1); }
            set { WriteParameter(ParameterAddresses.InputRangeHigh, DoubleToParam(value, 1)); }
        }

        /// <summary>
        /// Temperature Units (C or F)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Units
        {
            get
            {
                // only reads once (gets reset if C/F changed programmatically, but not if changed by the front panel)
                if (String.IsNullOrEmpty(units))
                    units = ReadBitRegister(ParameterAddresses.TempUnitSelection) ? "°C" : "°F";
                return units;
            }
        }

        /// <summary>
        /// Decimal Point Position (0 - 3)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort DecimalPointPosition
        {
            get { return ReadParameter(ParameterAddresses.DecimalPointPosition); }
            set
            {
                WriteParameter(ParameterAddresses.DecimalPointPosition, value);
                format = string.Empty;
            }
        }

        /// <summary>
        /// Display format for the temperature
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Format
        {
            get
            {
                if (String.IsNullOrEmpty(format))
                {
                    ushort tmp = DecimalPointPosition;
                    if (tmp == 0) format = "0";
                    else format = "0." + new String('0', tmp);
                }
                return format;
            }
            set
            {
                switch (value)
                {
                    case "0":
                        format = value;
                        DecimalPointPosition = 0;
                        break;

                    case "0.0":
                        format = value;
                        DecimalPointPosition = 1;
                        break;

                    case "0.00":
                        format = value;
                        DecimalPointPosition = 2;
                        break;

                    case "0.000":
                        format = value;
                        DecimalPointPosition = 3;
                        break;

                    default:
                        format = string.Empty;
                        throw new Exception("Invalid format");
                }
            }
        }

        /// <summary>
        /// Gets the digital inputs associated with the <see cref="SoloTempController">SoloTempController</see>.
        /// </summary>
        public TempControllerDigitalInputs DigitalInputs { get { return digitalInputs; } }

        #endregion Public Properties
    }
}