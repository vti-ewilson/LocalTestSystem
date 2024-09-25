using System;
using System.ComponentModel;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// RS-485 Interface for a Pfeiffer Turbo Controller
    /// </summary>
    public partial class PfeifferTurboController : PfeifferInterface
    {
        #region Enums

#pragma warning disable 1591

        public enum GasModes
        {
            HeavyInertGases = 0,
            OtherGases = 1
        }

        public enum HeatingOutputModes
        {
            HeatOperationMode = 0,
            SealingGasValveControl = 1
        }

        public enum HeatingTypes
        {
            CaseHeating = 0,
            TMSHeating,
            Cooling
        }

        public enum VentModes
        {
            AutomaticVenting = 0,
            DoNotVent = 1,
            VentingON = 2
        }

        public enum BackingPumpModes
        {
            NonStop = 0,
            Intermittent = 1,
            SwitchOnDelayed = 2
        }

        public enum AccessoryOutputModes
        {
            AirCooling = 0,
            VentingValve = 1,
            Heating = 2,
            BackingPump = 3
        }

        public enum K1OutputModes
        {
            SpeedAttained = 0,
            TemperatureAttained = 1
        }

        public enum AnalogOutputModes
        {
            RotationSpeedSignal = 0,
            PowerInputSignal = 1,
            CurrentSupplySignal = 2
        }

        public enum ParameterSetTypes
        {
            BasicParameterSet = 0,
            ExtendedParameterSet = 1
        }

#pragma warning restore 1591

        #endregion Enums

        #region Globals

        private String format = "0";
        private String units = "";
        private double min = 0, max = 100;
        private Double rotationSpeed;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeifferTurboController">PfeifferTurboController</see>
        /// </summary>
        public PfeifferTurboController()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeifferTurboController">PfeifferTurboController</see>
        /// </summary>
        /// <param name="container">Container for the control</param>
        public PfeifferTurboController(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion Construction

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> (Rotation Speed) of the Pfeiffer Turbo Controller
        /// </summary>
        public override void Process()
        {
            //while (true)
            //{
            rotationSpeed = RotationSpeed;

            if (!backgroundWorker1.IsBusy && !this.CommError)
                backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method

            //    Thread.Sleep(250);
            //}
        }

        /// <summary>
        /// When called, this method invokes the <see cref="PfeifferInterface.OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
        }

        /// <summary>
        /// Sends an Error Acknowledgement to the Pfeiffer Turbo Controller
        /// </summary>
        public void AcknowledgeError()
        {
            SetBooleanParameter(9, true);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Name for the Pfeiffer Turbo Controller
        /// </summary>
        public override string Name
        {
            get { return "Pfeiffer Turbo Controller on port " + this.SerialPort.PortName + " with ID " + this.Address; }
        }

        /// <summary>
        /// Rotation Speed (%)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Value
        {
            get { return rotationSpeed; }
            internal set { rotationSpeed = value; }
        }

        /// <summary>
        /// Formatted value
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
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Format string for the displayed value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Format
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Units for the displayed value (%)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Units
        {
            get { return units; }
            set { units = value; }
        }

        /// <summary>
        /// Minimum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// Maximum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Max
        {
            get { return max; }
            set { max = value; }
        }

        #region Run-up time and rotation speed switchpoint

        /// <summary>
        /// Run-up time monitoring
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RunUpTimeMonitoring
        {
            get { return GetBooleanParameter(4); }
            set { SetBooleanParameter(4, value); }
        }

        /// <summary>
        /// Maximum run-up time (in minutes)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint MaximumRunUpTime
        {
            get { return GetIntegerParameter(700); }
            set { SetIntegerParameter(700, value); }
        }

        /// <summary>
        /// Run-up ratation speed switchpoint (in %)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint RunUpRotationSpeedSwitchpoint
        {
            get { return GetIntegerParameter(701); }
            set { SetIntegerParameter(701, value); }
        }

        #endregion Run-up time and rotation speed switchpoint

        #region General Operating Information

        /// <summary>
        /// Final rotation speed (in Hz)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint FinalRotationSpeed
        {
            get { return GetIntegerParameter(315); }
        }

        /// <summary>
        /// Motor current (in A)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single MotorCurrent
        {
            get { return GetSingleParameter(310); }
        }

        /// <summary>
        /// Motor voltage (in V)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single MotorVoltage
        {
            get { return GetSingleParameter(313); }
        }

        /// <summary>
        /// Motor power (in W)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint MotorPower
        {
            get { return GetIntegerParameter(316); }
        }

        /// <summary>
        /// Operating hours
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint OperatingHours
        {
            get { return GetIntegerParameter(311); }
        }

        #endregion General Operating Information

        #region Operating Adjustments

        /// <summary>
        /// Standby ON/OFF
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Standby
        {
            get { return GetBooleanParameter(2); }
            set { SetBooleanParameter(2, value); }
        }

        /// <summary>
        /// Pumping station ON/OFF
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PumpingStation
        {
            get { return GetBooleanParameter(10); }
            set { SetBooleanParameter(10, value); }
        }

        /// <summary>
        /// Motor Turbopump ON/OFF
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MotorTurbopump
        {
            get { return GetBooleanParameter(23); }
            set { SetBooleanParameter(23, value); }
        }

        /// <summary>
        /// Enables the controlled rotation speed setting mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RotationSpeedSettingMode
        {
            get { return GetShortIntegerParameter(26) == 1; }
            set { SetShortIntegerParameter(26, (ushort)(value ? 1 : 0)); }
        }

        /// <summary>
        /// Gas mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GasModes GasMode
        {
            get { return (GasModes)GetShortIntegerParameter(27); }
            set { SetShortIntegerParameter(27, (ushort)value); }
        }

        /// <summary>
        /// Enables the reduced power consumption mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReducedPowerMode
        {
            get { return GetBooleanParameter(29); }
            set { SetBooleanParameter(29, value); }
        }

        /// <summary>
        /// Rotation speed setpoint (in Hz)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint RotationSpeedSetpoint
        {
            get { return GetIntegerParameter(308); }
        }

        /// <summary>
        /// Actual rotation speed in Hz
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint RotationSpeed
        {
            get { return GetIntegerParameter(309); }
        }

        /// <summary>
        /// Rotation speed set value at standby operations (in %)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort RotationSpeedSetpoint_Standby
        {
            get { return GetShortIntegerParameter(717); }
            set { SetShortIntegerParameter(717, value); }
        }

        /// <summary>
        /// Rotation speed set value in rotation speed setting operations (in %)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort RotationSpeedSetpoint_RotationSpeedSettingMode
        {
            get { return GetShortIntegerParameter(707); }
            set { SetShortIntegerParameter(707, value); }
        }

        #endregion Operating Adjustments

        #region Heating/cooling turbopump

        /// <summary>
        /// Heating ON/OFF
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Heating
        {
            get { return GetBooleanParameter(1); }
            set { SetBooleanParameter(1, value); }
        }

        /// <summary>
        /// Configuration for the heating output (Out4)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HeatingOutputModes HeatingOutputConfiguration
        {
            get { return (HeatingOutputModes)GetShortIntegerParameter(32); }
            set { SetShortIntegerParameter(32, (ushort)value); }
        }

        /// <summary>
        /// Actual temperature (in C)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint TMSActualTemperature
        {
            get { return GetIntegerParameter(331); }
        }

        /// <summary>
        /// TMS temperature attained?
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TMSsteady
        {
            get { return GetBooleanParameter(333); }
        }

        /// <summary>
        /// Maximum temperature occured (in C)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint TMSMaximumTemperature
        {
            get { return GetIntegerParameter(334); }
        }

        /// <summary>
        /// Heating type
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HeatingTypes HeatType
        {
            get { return (HeatingTypes)GetShortIntegerParameter(335); }
        }

        /// <summary>
        /// TMS heating temperature set point (in C)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint HeatingSetpoint
        {
            get { return GetIntegerParameter(704); }
            set { SetIntegerParameter(704, value); }
        }

        #endregion Heating/cooling turbopump

        #region Vent valve control

        /// <summary>
        /// Venting enable ON/OFF
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool VentingEnable
        {
            get { return GetBooleanParameter(12); }
            set { SetBooleanParameter(12, value); }
        }

        /// <summary>
        /// Venting mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VentModes VentMode
        {
            get { return (VentModes)GetShortIntegerParameter(30); }
            set { SetShortIntegerParameter(30, (ushort)value); }
        }

        /// <summary>
        /// Venting frequency (as a % of the final rotation speed of the TMP)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort VentFrequency
        {
            get { return GetShortIntegerParameter(720); }
            set { SetShortIntegerParameter(720, value); }
        }

        /// <summary>
        /// Venting time (in S) Min: 6  Max: 3600
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint VentTime
        {
            get { return GetIntegerParameter(721); }
            set { SetIntegerParameter(721, value); }
        }

        #endregion Vent valve control

        #region Pumping station control

        /// <summary>
        /// Operation mode of the backing pump
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BackingPumpModes BackingPumpMode
        {
            get { return (BackingPumpModes)GetShortIntegerParameter(25); }
            set { SetShortIntegerParameter(25, (ushort)value); }
        }

        /// <summary>
        /// Configuration of Accessory Output 1
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AccessoryOutputModes AccessoryOutput1Configuration
        {
            get { return (AccessoryOutputModes)GetShortIntegerParameter(35); }
            set { SetShortIntegerParameter(35, (ushort)value); }
        }

        /// <summary>
        /// Configuration of Accessory Output 2
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AccessoryOutputModes AccessoryOutput2Configuration
        {
            get { return (AccessoryOutputModes)GetShortIntegerParameter(36); }
            set { SetShortIntegerParameter(36, (ushort)value); }
        }

        /// <summary>
        /// Actual pressure value (in mbar)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Pressure
        {
            get { return GetExpoParameter(340); }
        }

        /// <summary>
        /// Vacuum pressure gauge type
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PressureGaugeType
        {
            get { return GetStringParameter(738); }
            set { SetStringParameter(738, value); }
        }

        /// <summary>
        /// Pmin for backing-pump interval operations (in W)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint BackingPumpPmin
        {
            get { return GetIntegerParameter(710); }
            set { SetIntegerParameter(710, value); }
        }

        /// <summary>
        /// Pmax for backing-pump interval operations (in W)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint BackingPumpPmax
        {
            get { return GetIntegerParameter(711); }
            set { SetIntegerParameter(711, value); }
        }

        #endregion Pumping station control

        #region Other parameters

        /// <summary>
        /// Braking ON/OFF
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BrakeEnable
        {
            get { return GetBooleanParameter(13); }
            set { SetBooleanParameter(13, value); }
        }

        /// <summary>
        /// Configuration of the K1 output
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public K1OutputModes K1OutputMode
        {
            get { return (K1OutputModes)GetShortIntegerParameter(24); }
            set { SetShortIntegerParameter(24, (ushort)value); }
        }

        /// <summary>
        /// Configuration of the Analog Output
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogOutputModes AnalogOutputMode
        {
            get { return (AnalogOutputModes)GetShortIntegerParameter(55); }
            set { SetShortIntegerParameter(55, (ushort)value); }
        }

        /// <summary>
        /// Actual error code ("no Err", "Errxxx", or "Wrnxxx")
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ErrorCode
        {
            get { return GetStringParameter(303); }
        }

        /// <summary>
        /// Software version of the electronic drive unit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SoftwareVersion_DriveUnit
        {
            get { return GetStringParameter(312); }
        }

        /// <summary>
        /// Software version of the Display an dControl Unit
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SoftwareVersion_DisplayAndControlUnit
        {
            get { return GetStringParameter(351); }
        }

        /// <summary>
        /// Parameter set
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ParameterSetTypes ParameterSet
        {
            get { return (ParameterSetTypes)GetShortIntegerParameter(794); }
            set { SetShortIntegerParameter(794, (ushort)value); }
        }

        /// <summary>
        /// Insert Service Line
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort ServiceLine
        {
            get { return GetShortIntegerParameter(795); }
            set { SetShortIntegerParameter(795, value); }
        }

        /// <summary>
        /// Unit address
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint UnitAddress
        {
            get { return GetIntegerParameter(797); }
            set { SetIntegerParameter(797, value); }
        }

        #endregion Other parameters

        #region Table of failures

        /// <summary>
        /// Previous Errors
        /// </summary>
        /// <param name="ErrorNumber">Error Number (1 - 10)</param>
        /// <returns>Error Code</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PastError(ushort ErrorNumber)
        {
            if (ErrorNumber < 1 || ErrorNumber > 10) throw new Exception("Invalid ErrorNumber");
            return GetStringParameter((ushort)(360 + ErrorNumber - 1));
        }

        #endregion Table of failures

        #endregion Public Properties
    }
}