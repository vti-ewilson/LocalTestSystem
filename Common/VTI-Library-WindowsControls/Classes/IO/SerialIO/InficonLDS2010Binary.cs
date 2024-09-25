using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Implements the binary communication protocol to an Inficon LDS2010 Leak Detector
    /// </summary>
    public partial class InficonLDS2010Binary : SerialIOBase
    {
        #region Enums

#pragma warning disable 1591

        public enum PressureUnitsType
        {
            mbar = 0,
            Pa = 1,
            atm = 2,
            Torr = 3
        }

        public enum CalFactorModes
        {
            Vacuum = 0,
            Sniff = 1
        }

        public enum CalModes
        {
            Internal = 0,
            External = 1
        }

        public enum CalStates
        {
            Inactive = 0,
            Active = 1,
            WaitForCalLeakClose = 2
        }

        public enum LeakRateUnitsType
        {
            mbar_l_s = 0,
            Pa_m3_s = 1,
            atm_cc_s = 2,
            torr_l_s = 3,
            ppm = 4,
            g_a = 5
        }

        public enum OpModes
        {
            Vacuum = 0,
            Sniff = 1,
            ByPlcInput = 2
        }

        public enum MeasurementModes
        {
            Standby = 0,
            Measurement = 1
        }

        public enum States
        {
            Standby = 0,
            Error = 1,
            Cal = 2,
            Runup = 3,
            Ready = 4,
            EmissionOff = 5
        }

        public enum GasBallastStates
        {
            Off = 0,
            On = 1,
            FailSafeOn = 2
        }

        public enum AudioModes
        {
            Analog = 0,
            Trigger = 1
        }

        public enum ExtCalModes
        {
            Normal = 0,
            Dynamic = 1,
            ByPlcInput = 2
        }

        public enum ZeroModes
        {
            Decades2to3 = 0,
            Decades1to2 = 1,
            Decades2 = 3,
            Decades3to4 = 4,
            Value = 2
        }

        public enum ControlModes
        {
            FrontPanel = 0,
            RS232 = 1,
            RS485 = 2
        }

#pragma warning restore 1591

        #endregion Enums

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

        #endregion Event Handlers

        #region Globals

        private String format = "0.00E-00";
        private double min = 1E-15, max = 1E0;
        private Double leakRate, pressure;
        private LeakRateUnitsType? leakRateUnits = 0;
        private PressureUnitsType? pressureUnits;
        private String[] leakRateUnitNames = { "mbar-l/s", "Pa-m^3/s", "atm-cc/s", "Torr-l/s", "ppm", "g/a" };
        private String[] pressureUnitNames = { "mbar", "Pa", "atm", "Torr" };

        private String _Units;
        //private Boolean commError = false;
        //private int retries = 0;

        private AnalogSignal leakRateSignal, pressureSignal;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010Binary">InficonLDS2010Binary</see> class
        /// </summary>
        public InficonLDS2010Binary()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010Binary">InficonLDS2010Binary</see> class
        /// </summary>
        /// <param name="container">Container for the object</param>
        public InficonLDS2010Binary(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            leakRateSignal = new AnalogSignal("LD Signal", leakRateUnitNames[0], "0.0E-0", 1E-8F, false, true);
            pressureSignal = new AnalogSignal("LD Inlet Press", pressureUnitNames[0], "0.0E-0", 1000, false, true);
        }

        #endregion Construction

        #region Private Methods

        private void CalculateCheckSumAndSend(String Message)
        {
            String message;
            message = String.Format("{0}{1}", Message, (char)(Message.ToCharArray().Sum(c => (int)c) % 256));
            this.serialPort1.DiscardInBuffer();
            this.serialPort1.Write(message);
        }

        private bool SendMessage(String Message)
        {
            bool retVal = false;
            if (this.IsAvailable && !commError)
            {
                if (Monitor.TryEnter(this.SerialLock, 3000))
                {
                    try
                    {
                        Actions.Retry(3,
                            delegate
                            {
                                CalculateCheckSumAndSend(Message);
                                retVal = true;
                            }, 500);
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
                        Monitor.Exit(this.SerialLock);
                    }
                }
            }
            return retVal;
        }

        private byte[] SendMessageWithResponse(String Message)
        {
            byte[] retData = null;
            byte len;
            if (this.IsAvailable && !commError && Monitor.TryEnter(this.SerialLock, 3000))
            {
                try
                {
                    Actions.Retry(3,
                        delegate
                        {
                            CalculateCheckSumAndSend(Message);
                            Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 0);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);
                        });
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
                    Monitor.Exit(this.SerialLock);
                }
            }
            return retData;
        }

        private Single ParseFloatResponse(byte[] Response)
        {
            Single retVal = Single.NaN;
            if (Response != null && Response.Length >= 6)
                retVal = BitConverter.ToSingle(new byte[] { Response[4], Response[5], Response[3], Response[2] }, 0);
            return retVal;
        }

        private byte[] SingleToInficonBytes(Single Value)
        {
            byte[] bytesIn = BitConverter.GetBytes(Value);
            byte[] bytesOut = new byte[] { bytesIn[3], bytesIn[2], bytesIn[0], bytesIn[1] };
            return bytesOut;
        }

        private Single GetFloatParameter(byte Command, byte Param1, byte Param2)
        {
            return ParseFloatResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)6, (char)Command, (char)Param1, (char)Param2)));
        }

        private Single GetFloatParameter(byte Command, byte Param1)
        {
            return ParseFloatResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}", (char)0x05, (char)5, (char)Command, (char)Param1)));
        }

        private Single GetFloatParameter(byte Command)
        {
            return ParseFloatResponse(
                SendMessageWithResponse(
                          String.Format("{0}{1}{2}{3}", (char)0x05, (char)4, (char)Command)));
        }

        private bool SetFloatParameter(byte Command, Single Value, byte Param1, byte Param2)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}{5}", (char)0x05, (char)10, (char)Command, (char)Param1, (char)Param2, SingleToInficonBytes(Value)));
        }

        private bool SetFloatParameter(byte Command, Single Value, byte Param1)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)9, (char)Command, (char)Param1, SingleToInficonBytes(Value)));
        }

        private bool SetFloatParameter(byte Command, Single Value)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)8, (char)Command, SingleToInficonBytes(Value)));
        }

        private Byte ParseByteResponse(byte[] Response)
        {
            if (Response != null && Response.Length >= 3)
                return Response[2];
            else
                return 0;
        }

        private Byte GetByteParameter(byte Command, byte Param1, byte Param2)
        {
            return ParseByteResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)6, (char)Command, (char)Param1, (char)Param2)));
        }

        private Byte GetByteParameter(byte Command, byte Param1)
        {
            return ParseByteResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}", (char)0x05, (char)5, (char)Command, (char)Param1)));
        }

        private Byte GetByteParameter(byte Command)
        {
            return ParseByteResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}", (char)0x05, (char)4, (char)Command)));
        }

        private bool SetByteParameter(byte Command, Byte Value, byte Param1, byte Param2)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}{5}", (char)0x05, (char)7, (char)Command, (char)Param1, (char)Param2, (char)Value));
        }

        private bool SetByteParameter(byte Command, Byte Value, byte Param1)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)6, (char)Command, (char)Param1, (char)Value));
        }

        private bool SetByteParameter(byte Command, Byte Value)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}", (char)0x05, (char)5, (char)Command, (char)Value));
        }

        private Boolean GetBoolParameter(byte Command)
        {
            return GetByteParameter(Command) == 1;
        }

        private Boolean GetBoolParameter(byte Command, byte Param1)
        {
            return GetByteParameter(Command, Param1) == 1;
        }

        private Boolean GetBoolParameter(byte Command, byte Param1, byte Param2)
        {
            return GetByteParameter(Command, Param1, Param2) == 1;
        }

        private bool SetBoolParameter(byte Command, Boolean Value)
        {
            return SetByteParameter(Command, Value ? (byte)1 : (byte)0);
        }

        private bool SetBoolParameter(byte Command, Boolean Value, byte Param1)
        {
            return SetByteParameter(Command, Value ? (byte)1 : (byte)0, Param1);
        }

        private bool SetBoolParameter(byte Command, Boolean Value, byte Param1, byte Param2)
        {
            return SetByteParameter(Command, Value ? (byte)1 : (byte)0, Param1, Param2);
        }

        private bool SetVoidParameter(byte Command)
        {
            return SendMessage(
                String.Format("{0}{1}{2}", (char)0x05, (char)4, (char)Command));
        }

        private string GetStringParameter(byte Command)
        {
            byte[] retData = SendMessageWithResponse(
                    String.Format("{0}{1}{2}", (char)0x05, (char)4, (char)Command));
            if (retData == null) return String.Empty;
            else return retData.ToString();
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="LeakRate">Leak Rate</see> and <see cref="Pressure">Pressure</see> of the Leak Detector
        /// </summary>
        public override void Process()
        {
            //leakRate = GetFloatParameter(99, (byte)leakRateUnits);
            leakRate = GetFloatParameter(97);
            Thread.Sleep(25);
            //if (!commError) pressure = GetFloatParameter(2, (byte)pressureUnits);

            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
        }

        /// <summary>
        /// When called, this method invokes the <see cref="InficonLDS2010Binary.OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            leakRateSignal.Value = leakRate;
            pressureSignal.Value = pressure;
            OnValueChanged();
        }

        /// <summary>
        /// Starts in Internal Calibration
        /// </summary>
        public void StartInternalCal()
        {
            SetByteParameter(55, 1, 0);
        }

        /// <summary>
        /// Stops the Internal Calibration
        /// </summary>
        public void StopInternalCal()
        {
            SetByteParameter(55, 0, 0);
        }

        /// <summary>
        /// Starts an External Calibration
        /// </summary>
        public void StartExternalCal()
        {
            SetByteParameter(55, 1, 1);
        }

        /// <summary>
        /// Stops the External Calibration
        /// </summary>
        public void StopExternalCal()
        {
            SetByteParameter(55, 0, 1);
        }

        /// <summary>
        /// Indicates that the External Leak has been closed and the External Calibration can continue
        /// </summary>
        public void ExternalCalLeakClosed()
        {
            SetByteParameter(55, 2, 1);
        }

        /// <summary>
        /// Clears any error on the Leak Detector
        /// </summary>
        public void ClearError()
        {
            SetVoidParameter(63);
        }

        /// <summary>
        /// Cancels any ongoing calibration
        /// </summary>
        public void CancelCalibration()
        {
            SetVoidParameter(63);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets the current Leak Rate
        /// </summary>
        public override double Value
        {
            get { return leakRate; }
            internal set
            {
                leakRate = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Gets the Leak Rate formatted in scientific notation and including the units
        /// </summary>
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
        /// Name for the class
        /// </summary>
        public override string Name
        {
            get { return "Inficon LDS2010 Leak Detector on port " + this.SerialPort.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Minimum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Min
        {
            get { return min; }
        }

        /// <summary>
        /// Maximum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Max
        {
            get { return max; }
        }

        /// <summary>
        /// Units for the displayed value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Units
        {
            get { return leakRateUnitNames[(int)this.leakRateUnits]; }
            set { _Units = value; }
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
        /// Gets the current Inlet Pressure of the Leak Detector
        /// </summary>
        public Double Pressure
        {
            get
            {
                return pressure; // GetFloatParameter(2, 1, (byte)pressureUnits, 0);
            }
        }

        /// <summary>
        /// Gets or sets the units for the <see cref="Pressure">Pressure</see> reading
        /// </summary>
        public PressureUnitsType PressureUnits
        {
            get
            {
                if (pressureUnits == null)
                    pressureUnits = (PressureUnitsType)GetByteParameter(92, 2);
                return (PressureUnitsType)pressureUnits;
            }
            set
            {
                pressureUnits = value;
                pressureSignal.Units = pressureUnitNames[(int)pressureUnits];
                SetByteParameter(93, (byte)value, 2);
            }
        }

        /// <summary>
        /// Gets the current Leak Rate of the Leak Detector
        /// </summary>
        public Double LeakRate
        {
            get
            {
                return leakRate; // GetFloatParameter(99, 1, (byte)leakRateUnits, 0);
            }
        }

        /// <summary>
        /// Gets or Sets the units for the <see cref="LeakRate">Leak Rate</see>
        /// </summary>
        public LeakRateUnitsType LeakRateUnits
        {
            get
            {
                if (leakRateUnits == null)
                    leakRateUnits = (LeakRateUnitsType)GetByteParameter(92, 0);
                return (LeakRateUnitsType)leakRateUnits;
            }
            set
            {
                leakRateUnits = value;
                leakRateSignal.Units = leakRateUnitNames[(int)this.leakRateUnits];
                SetByteParameter(93, (byte)value, 0);
            }
        }

        /// <summary>
        /// Leak Rate as an AnalogSignal
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogSignal LeakRateSignal
        {
            get { return leakRateSignal; }
        }

        /// <summary>
        /// Foreline pressure as an AnalogSignal
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogSignal PressureSignal
        {
            get { return pressureSignal; }
        }

        /// <summary>
        /// Gets or sets a value indicating the state of the Gas Ballast setting
        /// </summary>
        public GasBallastStates GasBallastState
        {
            get { return (GasBallastStates)GetByteParameter(8); }
            set { SetByteParameter(9, (byte)value); }
        }

        /// <summary>
        /// Gets or sets the value of the Calibration Factor for Vacuum Mode
        /// </summary>
        public Single CalFactorVacuum
        {
            get { return GetFloatParameter(36, 0); }
            set { SetFloatParameter(37, value, 0); }
        }

        /// <summary>
        /// Gets or sets the value of the Calibration Factor for Sniff Mode
        /// </summary>
        public Single CalFactorSniff
        {
            get { return GetFloatParameter(36, 1); }
            set { SetFloatParameter(37, value, 1); }
        }

        /// <summary>
        /// Gets or sets a value indicating the atomic mass setting
        /// </summary>
        public Byte Mass
        {
            get { return GetByteParameter(40); }
            set { SetByteParameter(41, Math.Max(Math.Min(value, (byte)2), (byte)4)); }
        }

        /// <summary>
        /// Gets or sets a value to enable the zero (background suppression)
        /// </summary>
        public Boolean Zero
        {
            get { return GetByteParameter(50) == 1; }
            set { SetByteParameter(51, value ? (byte)1 : (byte)0); }
        }

        /// <summary>
        /// Gets or sets a value indicating if the Display Range should be between the
        /// <see cref="LowLimit">Low Limit</see> and <see cref="HighLimit">High Limit</see>
        /// </summary>
        public Boolean Manual
        {
            get { return GetBoolParameter(52); }
            set { SetBoolParameter(53, value); }
        }

        /// <summary>
        /// Gets the state of the internal calibration
        /// </summary>
        public CalStates InternalCalState
        {
            get { return (CalStates)GetByteParameter(54, (byte)CalModes.Internal); }
        }

        /// <summary>
        /// Gets the state of the external calibration
        /// </summary>
        public CalStates ExternalCalState
        {
            get { return (CalStates)GetByteParameter(54, (byte)CalModes.External); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 1
        /// </summary>
        public Single Trigger1
        {
            get { return GetFloatParameter(56, 1, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 1, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 2
        /// </summary>
        public Single Trigger2
        {
            get { return GetFloatParameter(56, 2, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 2, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 3
        /// </summary>
        public Single Trigger3
        {
            get { return GetFloatParameter(56, 3, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 3, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 4
        /// </summary>
        public Single Trigger4
        {
            get { return GetFloatParameter(56, 4, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 4, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the current operating mode
        /// </summary>
        public OpModes OpMode
        {
            get { return (OpModes)GetByteParameter(58); }
            set { SetByteParameter(59, (byte)value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the current measurement mode
        /// </summary>
        public MeasurementModes MeasureMode
        {
            get { return (MeasurementModes)GetByteParameter(60); }
            set { SetByteParameter(61, (byte)value); }
        }

        /// <summary>
        /// Gets the current Error Code (0 = no error)
        /// </summary>
        public byte ErrorCode
        {
            get { return GetByteParameter(62); }
        }

        /// <summary>
        /// Gets or sets a value indicating the Low Limit
        /// </summary>
        public Single LowLimit
        {
            get { return GetFloatParameter(64, 0, (byte)leakRateUnits); }
            set { SetFloatParameter(65, value, 0, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets a value indicating the High Limit
        /// </summary>
        public Single HighLimit
        {
            get { return GetFloatParameter(64, 1, (byte)leakRateUnits); }
            set { SetFloatParameter(65, value, 1, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the leak rate value of the Internal Calibrated Leak
        /// </summary>
        public Single InternalCalLeak
        {
            get { return GetFloatParameter(66, 0, (byte)leakRateUnits); }
            set { SetFloatParameter(67, value, 0, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the leak rate value of the External Calibrated Leak
        /// </summary>
        public Single ExternalCalLeak
        {
            get { return GetFloatParameter(66, 1, (byte)leakRateUnits); }
            set { SetFloatParameter(67, value, 1, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the leak rate value of the Sniffer Calibrated Leak
        /// </summary>
        public Single SnifferCalLeak
        {
            get { return GetFloatParameter(66, 2, (byte)leakRateUnits); }
            set { SetFloatParameter(67, value, 2, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets a value indicating the leak rate for switching the averaging time
        /// </summary>
        public Single FilterSetPoint
        {
            get { return GetFloatParameter(68, (byte)leakRateUnits); }
            set { SetFloatParameter(69, value, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets the Serial Number of the device
        /// </summary>
        public string SerialNumber
        {
            get { return GetStringParameter(70); }
        }

        /// <summary>
        /// Gets the current operating state
        /// </summary>
        public States State
        {
            get { return (States)GetByteParameter(72); }
        }

        /// <summary>
        /// Gets a value indicating if the averaging filter is active
        /// </summary>
        public Boolean FilterActive
        {
            get { return GetBoolParameter(73); }
        }

        /// <summary>
        /// Gets the operating hours of the device
        /// </summary>
        public uint OpHours
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the software version (Byte 0: Main-Version, Byte 1: Sub-Version)
        /// </summary>
        public ushort SoftwareVersion
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the value of the Machine calibration factor
        /// </summary>
        public Single MachineFactor
        {
            get { return GetFloatParameter(78); }
            set { SetFloatParameter(79, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the External Calibration Mode
        /// </summary>
        public ExtCalModes ExtCalMode
        {
            get { return (ExtCalModes)GetByteParameter(80); }
            set { SetByteParameter(80, (byte)value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the type of Zero Mode
        /// </summary>
        public ZeroModes ZeroMode
        {
            get { return (ZeroModes)GetByteParameter(82); }
            set { SetByteParameter(83, (byte)value); }
        }

        /// <summary>
        /// Gets or sets the audio volume
        /// </summary>
        public byte AudioVolume
        {
            get { return GetByteParameter(88); }
            set { SetByteParameter(89, Math.Max(Math.Min(value, (byte)1), (byte)15)); }
        }

        /// <summary>
        /// Gets or sets the audio mode
        /// </summary>
        public AudioModes AudioMode
        {
            get { return (AudioModes)GetByteParameter(90); }
            set { SetByteParameter(91, (byte)value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the control mode
        /// </summary>
        public ControlModes ControlMode
        {
            get { return (ControlModes)GetByteParameter(94); }
            set { SetByteParameter(95, (byte)value); }
        }

        #endregion Public Properties
    }
}