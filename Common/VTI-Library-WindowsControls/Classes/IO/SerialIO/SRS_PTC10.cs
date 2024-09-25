using System;
using System.Diagnostics;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for a SRS Controller
    /// </summary>
    public partial class SRSPTC10 : SerialIOBase
    {
        #region Public Enum

        /// <summary>
        /// Enumeration of status register values for PTC10_StatusRegister
        ///      see example signal.TerraNova934FilamentStatus.Value = Convert.ToDouble(IO.SerialIn.TerraNova934Control.GetIonGaugeParameter((int)TerraNova934Mode.FilamentStatusAutofilament));
        /// </summary>
        public enum PTC10_StatusRegister
        {
        }

        #endregion Public Enum

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

        #region Privates Vars

        private Boolean _DoNotCall;
        private Double _Temperature1A, _Temperature1B, _Temperature1C, _Temperature1D;
        private Double _Temperature2A, _Temperature2B, _Temperature2C, _Temperature2D;

        private Double _V1PIDTemperatureSetpoint, _V2PIDTemperatureSetpoint, _V3PIDTemperatureSetpoint;
        private string _V1PIDOn, _V2PIDOn, _V3PIDOn;
        private bool _ReadPIDValues;

        private Single _min, _max;
        private String _units;
        private String _format;

        //private AnalogSignal _temperature1A, _temperature1B, _temperature1C, _temperature1D;
        //private AnalogSignal _temperature2A, _temperature2B, _temperature2C, _temperature2D;
        //private Boolean _Idle;
        private String[] _temperatureUnitNames = { "F" };

        private Stopwatch _errorSW = new Stopwatch();
        private String[] _ManualCommandQueue = new String[10];
        private int QueueCount;

        // Command Process flags
        private Boolean _flagReadSRSStatus = false;

        private string _SendManualCommandResult;

        // Not all are implemented See status register Read934Status
        //private int

        #endregion Privates Vars

        #region Private Properties

        private bool _StopV1PIDControlClearValue { get; set; }
        private bool _StopV2PIDControlClearValue { get; set; }
        private bool _StopV3PIDControlClearValue { get; set; }
        private bool _SendManualCommand { get; set; }
        private string _ManualCommandString { get; set; }

        #endregion Private Properties

        #region Construction   // constructor name must match the class name

        /// <summary>
        /// Initializes a new instance of the <see cref="TerraNova934">TerraNova934</see> class
        /// </summary>
        public SRSPTC10(Single Min, Single Max, String Units, String format)
            : base()
        {
            try
            {
                _min = Min;
                _max = Max;
                _units = Units;
                Format = format;

                this.SerialPort.ReadTimeout = 100000;

                //_temperature1A = new AnalogSignal("Temp 1A", _temperatureUnitNames[1], "0.00", 100, false, false);
                //_temperature1B = new AnalogSignal("Temp 1B", _temperatureUnitNames[1], "0.00", 100, false, false);
                //_temperature1C = new AnalogSignal("Temp 1C", _temperatureUnitNames[1], "0.00", 100, false, false);
                //_temperature1D = new AnalogSignal("Temp 1D", _temperatureUnitNames[1], "0.00", 100, false, false);
                //_temperature2A = new AnalogSignal("Temp 2A", _temperatureUnitNames[1], "0.00", 100, false, false);
                //_temperature2B = new AnalogSignal("Temp 2B", _temperatureUnitNames[1], "0.00", 100, false, false);
                //_temperature2C = new AnalogSignal("Temp 2C", _temperatureUnitNames[1], "0.00", 100, false, false);
                //_temperature2D = new AnalogSignal("Temp 2D", _temperatureUnitNames[1], "0.00", 100, false, false);
            }
            catch (Exception exc)
            {
                VtiEvent.Log.WriteError("Error initializing SRSPTC10. ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, exc.Message);
            }
        }

        #endregion Construction   // constructor name must match the class name

        #region Private Methods

        /// <summary>
        /// Get Ion Gauge Pressure from SRS
        /// </summary>
        private Double ReadTemperature(string InputName)
        {
            string strDummy = "", strCMD = "";
            strCMD = InputName;
            Single fRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD + "?");
                if (!strDummy.Contains("Error"))
                {
                    fRet = Convert.ToSingle(strDummy);
                    return fRet;
                }
                return 0;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in ReadPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                commError = true;
                return 0;
            }
        }

        /// <summary>
        ///  Sends the manual command to the PTC10
        /// </summary>
        private String SendManualCommandString()
        {
            string strDummy = "", strCMD = "";
            strCMD = _ManualCommandString;

            try
            {
                strDummy = ProcessCommandAndReturnValue(_ManualCommandString);
                return strDummy;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in SendManualCommandString", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                commError = true;
                return "Error: Unknown";
            }
        }

        /// <summary>
        /// Processes commands to the SRS Controller
        /// </summary>
        private string ProcessCommandAndReturnValue(string strCommand)
        {
            // Send command
            //_DoNotCall = true;
            string CommandSent = strCommand + "\r";
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                serialPort1.WriteLine(CommandSent);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SRS ProcessCommandAndReturnValue, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //_DoNotCall = false;
                return "-1"; // write error
            }
            // Read back response
            DateTime dt = DateTime.Now;
            long dataLength = 13; double timeout = 2;
            string strRead = ""; // CLEAR VALUE
            try
            {
                DateTime dtStart = DateTime.Now;
                TimeSpan ts;
                int numReads = 0, prevStrReadLength, numEmptyReads = 0;
                do
                {
                    Thread.Sleep(100);
                    prevStrReadLength = strRead.Length;

                    strRead += serialPort1.ReadExisting();

                    //strRead = serialPort1.ReadLine();
                    //Console.WriteLine( strRead);
                    if (strRead.Length == prevStrReadLength)
                        numEmptyReads++;
                    else
                        numEmptyReads = 0;
                    if (strRead.Length >= dataLength)
                        break;
                    numReads++;
                    ts = DateTime.Now - dtStart;
                }
                while (numEmptyReads < 3 && ts.TotalSeconds < timeout);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside  SRS ProcessCommandAndReturnValue, Serial Port Read", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //_DoNotCall = false;
                return "0"; // Read error
            }
            strRead = strRead.Replace("\r", "");
            strRead = strRead.Replace("\n", "");
            strRead = strRead.Replace(">", "");
            strRead = strRead.Trim();
            //_DoNotCall = false;
            return strRead; // no error
        }

        /// <summary>
        /// Processes Status Register of the SRS Controller
        /// </summary>
        private string ProcessRegisterAndReturnValues(string strCommand)
        {
            // Send command
            //_DoNotCall = true;
            string CommandSent = strCommand + "\r";
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                serialPort1.WriteLine(CommandSent);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SRS ProcessRegisterAndReturnValues, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //_DoNotCall = false;
                return "-1"; // write error
            }
            // Read back response
            DateTime dt = DateTime.Now;
            long dataLength = 152; double timeout = 2;
            string strRead = ""; // CLEAR VALUE
            try
            {
                DateTime dtStart = DateTime.Now;
                TimeSpan ts;
                int numReads = 0, prevStrReadLength, numEmptyReads = 0;
                do
                {
                    Thread.Sleep(50);
                    prevStrReadLength = strRead.Length;
                    //strRead += serialPort1.ReadExisting();
                    strRead += serialPort1.ReadLine();
                    //Console.WriteLine(strRead);
                    if (strRead.Length == prevStrReadLength)
                        numEmptyReads++;
                    else
                        numEmptyReads = 0;
                    if (strRead.Length >= dataLength)
                        break;
                    numReads++;
                    ts = DateTime.Now - dtStart;
                }
                while (numEmptyReads < 5 && ts.TotalSeconds < timeout);
                //Console.Write(numEmptyReads);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SRS ProcessRegisterAndReturnValues, Serial Port Read", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //_DoNotCall = false;
                return strRead; // Read error
            }
            //strRead = strRead.Replace("\r", "");
            //strRead = strRead.Replace("\n", "");
            //strRead = strRead.Replace(">", "");
            //strRead = strRead.Trim();
            //_DoNotCall = false;
            return strRead; // no error
        }

        /// <summary>
        /// Reads V1, V2 or V3 PID Temperature set point.
        /// </summary>
        private Single ReadVirtualChannelSetPoint(string InputName)
        {
            string strDummy = "", strCMD = "";
            strCMD = InputName + ".PID.SetPoint?";
            Single fRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                if (!strDummy.Contains("Error"))
                {
                    fRet = Convert.ToSingle(strDummy);
                    return fRet;
                }
                return 0;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in ReadVirtualChannelSetPoint", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                commError = true;
                return 0;
            }
        }

        /// <summary>
        /// Reads V1, V2 or V3 PID State as on or off
        /// </summary>
        private string ReadVirtualChannelPIDState(string InputName)
        {
            string strDummy = "", strCMD = "";
            strCMD = InputName + ".PID.Mode?";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                if (!strDummy.Contains("Error"))
                {
                    sRet = strDummy;
                    return sRet;
                }
                return "Error";
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in ReadVirtualChannelPIDState", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                commError = true;
                return "Error";
            }
        }

        /// <summary>
        /// E= status register dump; entries separated by carriage returns
        ///(1) = degas; 1 = on, 0 = off
        ///(2) = filament; 1 = on, 0 = off
        ///(3) = ion gauge emission; 1 = OK, 0 = off
        ///(4) = auto filament; 1 = enabled, 0 = disabled
        ///(5) = filament error codes (see Table 3.3, above)
        ///(6) = low-vacuum gauge A error; 1 = defective, 0 = OK
        ///(7) = low-vacuum gauge B error; 1 = defective, 0 = OK
        ///(10) = filament status, auto filament; 1 = filament on, 0 = filament off
        ///(11) = set point 1/low-vacuum gauge A; 1 = relay on, 0 = relay off
        ///(12) = set point 2/low-vacuum gauge B; 1 = relay on, 0 = relay off
        ///(13) = set point 3/ion gauge; 1 = relay on, 0 = relay off
        ///(14) = set point 4/ion gauge; 1 = relay on, 0 = relay off
        ///(15) = set point protection disabled, via secret switch;
        ///0 = protected, 1 = protection disabled
        ///(16) = ion gauge conformity error; 1 = error, 2 = OK
        ///(17) = Auto filament set point; -xyz, where pressure = yz*10-x Torr
        ///(18) = gauge A set point; -xyz, where pressure = yz*10-x Torr
        ///(19) = gauge B set point; -xyz, where pressure = yz*10-x Torr
        ///(20) = ion gauge set point
        ///(21) = option set point (ion gauge)
        ///(22) = gas factor; shown as 50 to 150 for values of 0.50 to 1.50
        ///(23) = ion gauge sensitivity; shown as 50 to 150 for values of 5.0 to15.0
        ///(24) = ion gauge pressure; -xyz, where pressure = yz*10-x Torr.
        ///(25) = gauge A pressure; -xyz, where pressure = yz*10-x Torr;
        ///pressure in range of 10 to 99 Torr shown as yz Torr
        ///(26) = gauge B pressure; -xyz, where pressure = yz*10-x Torr;
        ///pressure in range of 10 to 99 Torr shown as yz Torr
        /// </summary>
        //private void Read934Status()
        //{
        //    string strDummy = "";
        //    string sRet;
        //    try
        //    {
        //        strDummy = ProcessRegisterAndReturnValues("E");
        //        sRet = strDummy;
        //        // divide string at /r into 26 parameters
        //        //string[] Reg = strDummy.Split(new char[] {'\r'});  May delete empty lines
        //        //List<string> Reg = strDummy.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();  // Environment.NewLine = /r/n
        //        List<string> Reg = strDummy.Split(new char[] { '\r' }, StringSplitOptions.None).ToList();

        //        //  Display values in console
        //        //Reg.ForEach(l => Console.WriteLine(l));
        //        //Console.WriteLine("Value26" + Reg[25].ToString());
        //        //Console.ReadLine();

        //        // Populate public status variables
        //        bool res = int.TryParse(Reg[2], out _filamentStatus);
        //        res = int.TryParse(Reg[3], out _ionGaugeEmission);
        //        if (_filamentStatus == 1)
        //        { Console.WriteLine("Filament is on"); }
        //        else
        //        { Console.WriteLine("Filament is off"); }
        //    }
        //    catch (Exception e)
        //    {
        //        VtiEvent.Log.WriteError("Error inside Read934Status", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
        //    }
        //    finally { _flagRead934Status = false; }
        //}

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the SRS Controller
        /// </summary>
        public override void Process()
        {
            if (Monitor.TryEnter(this.SerialLock, 2000))
            {
                try
                {
                    Thread.Sleep(50);

                    // Constant delayed process
                    serialPort1.DiscardInBuffer();
                    Temperature1A = ReadTemperature("1A");
                    Temperature1B = ReadTemperature("1B");
                    Temperature1C = ReadTemperature("1C");
                    Temperature1D = ReadTemperature("1D");
                    Temperature2A = ReadTemperature("2A");
                    Temperature2B = ReadTemperature("2B");
                    Temperature2C = ReadTemperature("2C");
                    Temperature2D = ReadTemperature("2D");

                    // Flagged processes (only one at a time)
                    if (_SendManualCommand)
                    {
                        _SendManualCommand = false;
                        _SendManualCommandResult = SendManualCommandString();
                        Console.WriteLine("_SendManualCommandResult = " + _SendManualCommandResult);
                        _ManualCommandString = "";
                    }
                    if (QueueCount > 0)
                    {
                        for (int QuecheckCount = 0; QuecheckCount < 9; QuecheckCount++)
                        {
                            //QuecheckCount += QuecheckCount;
                            if (_ManualCommandQueue[QuecheckCount] != null)
                            {
                                _ManualCommandString = _ManualCommandQueue[QuecheckCount];
                                _SendManualCommandResult = SendManualCommandString();
                                Console.WriteLine("_SendManualCommandResult = " + _SendManualCommandResult);
                                _ManualCommandString = "";
                                _ManualCommandQueue[QuecheckCount] = null;
                            }
                        }
                        QueueCount = 0;
                    }
                    if (_StopV1PIDControlClearValue)
                    {
                        Thread.Sleep(1000);
                    }

                    if (_ReadPIDValues)
                    {
                        _V1PIDTemperatureSetpoint = ReadVirtualChannelSetPoint("V1");
                        _V1PIDOn = ReadVirtualChannelPIDState("V1");
                        _V2PIDTemperatureSetpoint = ReadVirtualChannelSetPoint("V2");
                        _V2PIDOn = ReadVirtualChannelPIDState("V2");
                        _V3PIDTemperatureSetpoint = ReadVirtualChannelSetPoint("V3");
                        _V3PIDOn = ReadVirtualChannelPIDState("V3");
                        _ReadPIDValues = false;
                    }

                    if (!this.backgroundWorker1.IsBusy)
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError("Error inside SRS | Process", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Sets V1, V2 or V3 Temperature set point.
        /// Set PID mode to On to start controlling temperature
        /// </summary>
        public void SetVirtualChannelSetPoint(string Channel, Double SetPointTemp)
        {
            try
            {
                SendManualCommandFlag(Channel + ".PID.SetPoint = " + SetPointTemp.ToString());
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SetVirtualChannelSetPoint", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { }
        }

        /// <summary>
        /// Set Flag to Turn off PID control or tuning and clears value for Virtual Channel V1
        /// </summary>
        public void StopV1PIDControlClearsValue()
        {
            try
            {
                SendManualCommandFlag("V1.Off");
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StopV1PIDControlClearsValue", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { }
        }

        /// <summary>
        /// Set Flag to Turn off PID control or tuning and clears value for Virtual Channel V2
        /// </summary>
        public void StopV2PIDControlClearsValue()
        {
            try
            {
                SendManualCommandFlag("V2.Off");
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StopV2PIDControlClearsValue", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { }
        }

        /// <summary>
        /// Set Flag to Turn off PID control or tuning and clears value for Virtual Channel V3
        /// </summary>
        public void StopV3PIDControlClearsValue()
        {
            try
            {
                SendManualCommandFlag("V3.Off");
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StopV3PIDControlClearsValue", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { }
        }

        /// <summary>
        /// Set Flag to Turn On PID control for Virtual Channel V1
        /// </summary>
        public void StartV1PIDControl()
        {
            try
            {
                SendManualCommandFlag("V1.PID.Mode = On");
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StartV1PIDControl", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { }
        }

        /// <summary>
        /// Set Flag to Turn On PID control for Virtual Channel V2
        /// </summary>
        public void StartV2PIDControl()
        {
            try
            {
                SendManualCommandFlag("V2.PID.Mode = On");
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StartV2PIDControl", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { }
        }

        /// <summary>
        /// Set Flag to Turn On PID control for Virtual Channel V3
        /// </summary>
        public void StartV3PIDControl()
        {
            try
            {
                SendManualCommandFlag("V3.PID.Mode = On");
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StartV3PIDControl", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { }
        }

        /// <summary>
        /// Send Manual Command
        /// </summary>
        public void SendManualCommandFlag(string strCommand)
        {
            try
            {
                if (_SendManualCommand)
                {
                    // If command is already in que do no add it again
                    for (int QuecheckCount = 0; QuecheckCount < 9; QuecheckCount++)
                    {
                        //QuecheckCount += QuecheckCount;
                        if (_ManualCommandQueue[QuecheckCount] == strCommand)
                            return;
                    }
                    _ManualCommandQueue[QueueCount] = strCommand;
                    QueueCount = QueueCount + 1;
                }
                else
                {
                    _ManualCommandString = strCommand;
                    _SendManualCommand = true;
                    //_DoNotCall = true;
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SRS SendManualCommand", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }

        public void RefreshForm(object sender, EventArgs e)
        {
            this._flagReadSRSStatus = true;
        }

        #endregion Public Methods

        #region Events

        /// <summary>
        /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
        }

        #endregion Events

        #region Public Properties

        public string SendManualCommandResult
        {
            get
            {
                return _SendManualCommandResult;
            }
            internal set
            {
                _SendManualCommandResult = value;
            }
        }

        /// <summary>
        /// Read 934 status register, values can be read from public properties
        /// See private method Read934Status
        /// </summary>
        public bool Read934StatusReg { set { _flagReadSRSStatus = true; } }

        /// <summary>
        /// Set to "true" Read V# PID values of the SRS Controllers
        /// </summary>
        public bool ReadPIDValues
        {
            get
            {
                return _ReadPIDValues;
            }
            set
            {
                _ReadPIDValues = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public override double Value
        {
            get
            {
                return Temperature1A;
            }
            internal set
            {
                _Temperature1A = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature1A
        {
            get
            {
                return _Temperature1A;
            }
            internal set
            {
                _Temperature1A = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature1B
        {
            get
            {
                return _Temperature1B;
            }
            internal set
            {
                _Temperature1B = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature1C
        {
            get
            {
                return _Temperature1C;
            }
            internal set
            {
                _Temperature1C = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature1D
        {
            get
            {
                return _Temperature1D;
            }
            internal set
            {
                _Temperature1D = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature2A
        {
            get
            {
                return _Temperature2A;
            }
            internal set
            {
                _Temperature2A = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature2B
        {
            get
            {
                return _Temperature2B;
            }
            internal set
            {
                _Temperature2B = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature2C
        {
            get
            {
                return _Temperature2C;
            }
            internal set
            {
                _Temperature2C = value;
            }
        }

        /// <summary>
        /// Value (temp) of the SRS Controllers
        /// </summary>
        public double Temperature2D
        {
            get
            {
                return _Temperature2D;
            }
            internal set
            {
                _Temperature2D = value;
            }
        }

        /// <summary>
        /// Value (V1 PID Temperature Set Point) of the SRS Controllers
        /// </summary>
        public double V1PIDTemperatureSetpoint
        {
            get
            {
                return _V1PIDTemperatureSetpoint;
            }
            set
            {
                _V1PIDTemperatureSetpoint = value;
                SetVirtualChannelSetPoint("V1", value);
            }
        }

        /// <summary>
        /// Value (V2 PID Temperature Set Point) of the SRS Controllers
        /// </summary>
        public double V2PIDTemperatureSetpoint
        {
            get
            {
                return _V2PIDTemperatureSetpoint;
            }
            set
            {
                _V2PIDTemperatureSetpoint = value;
                SetVirtualChannelSetPoint("V2", value);
            }
        }

        /// <summary>
        /// Value (V3 PID Temperature Set Point) of the SRS Controllers
        /// </summary>
        public double V3PIDTemperatureSetpoint
        {
            get
            {
                return _V3PIDTemperatureSetpoint;
            }
            set
            {
                _V3PIDTemperatureSetpoint = value;
                SetVirtualChannelSetPoint("V3", value);
            }
        }

        /// <summary>
        /// Value (V1 PID State "on" or "off") of the SRS Controllers
        /// </summary>
        public string V1PIDState
        {
            get
            {
                return _V1PIDOn;
            }
            set
            {
                _V1PIDOn = value;
                SendManualCommandFlag("V1.PID.Mode = " + value);
            }
        }

        /// <summary>
        /// Value (V2 PID State on or off) of the SRS Controllers
        /// </summary>
        public string V2PIDState
        {
            get
            {
                return _V2PIDOn;
            }
            set
            {
                _V2PIDOn = value;
                SendManualCommandFlag("V2.PID.Mode = " + value);
            }
        }

        /// <summary>
        /// Value (V3 PID State on or off) of the SRS Controllers
        /// </summary>
        public string V3PIDState
        {
            get
            {
                return _V3PIDOn;
            }
            set
            {
                _V3PIDOn = value;
                SendManualCommandFlag("V3.PID.Mode = " + value);
            }
        }

        /// <summary>
        /// Formatted value including the <see cref="Units">Units</see>
        /// </summary>
        public override string FormattedValue
        {
            get
            {
                if (Double.IsNaN(this.Value))
                    return "ERROR";
                else
                    return this.Value.ToString(_format) + " " + this.Units;
            }
        }

        /// <summary>
        /// Name for the SRS Controller
        /// </summary>
        public override string Name
        {
            get { return "SRS Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the SRS Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the SRS controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the SRS controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the SRS controller
        /// </summary>
        public override string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        #endregion Public Properties
    }
}