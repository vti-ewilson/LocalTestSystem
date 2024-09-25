using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for a  Controller
    /// </summary>
    public class InficonVGC083 : SerialIOBase
    {
        #region Public Enum

        /// <summary>
        /// Enumeration of status register values
        ///       signal. FilamentStatus.Value = Convert.ToDouble(IO.SerialIn. Control.GetIonGaugeParameter((int) Mode.FilamentStatusAutofilament));
        /// </summary>
        //public enum TerraNova934StatusRegister
        //{
        //    Degas = 1, // 1 = on, 0 = off
        //    Filament, // (2) 1 = on, 0 = off
        //    IonGaugeEmission, // (3) 1 = OK, 0 = ff
        //    AutoFilament, // (4) 1 = enabled, 0 = disabled
        //    FilamentErrorCodes, // (5)
        //    LowVacuumGaugeAError, // (6) 1 = defective, 0 = OK
        //    LowVacuumGaugeBError, // (7) 1 = defective, 0 = OK
        //    FilamentStatusAutofilament = 10, // (10) 1 = filament on, 0 = filament off
        //    SetPoint1LowVacuumGauge, // (11) 1 = relay on, 0 = relay off
        //    SetPoint2LowVacuumGaugeB, // (12) 1 = relay on, 0 = relay off
        //    SetPoint3IonGauge, // (13) 1 = relay on, 0 = relay off
        //    SetPoint4IonGauge, // (14) 1 = relay on, 0 = relay off
        //    SetPointProtectionDisabled, // (15) 0 = protected, 1 = protection disabled
        //    IonGaugeConformityError, // (16) 1 = error, 2 = OK
        //    AutoFilamentSetPoint, // (17)
        //    GaugeASetPoint, // (18)
        //    GaugeBSetPoint, // (19)
        //    IonGaugeSetPoint, // (20)
        //    OptionSetPointIonGauge, // (21)
        //    GasFactor, // (22)
        //    IonGaugeSensitivity, // (23)
        //    IonGaugePressure, // (24)
        //    GaugeAPressure, // (25)
        //    GaugeBPressure // (26)
        //}

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
        private Double _pressure, _pressureIonGauge, _pressureSignalA, _pressureSignalB;
        private Single _min, _max;
        private String _units;
        private String _format = "0.00";

        //private int _decimalPlaces = 3;
        private AnalogSignal pressureIonGauge;

        private AnalogSignal pressureSignalA;
        private AnalogSignal pressureSignalB;

        //private Boolean _Idle;
        private String[] pressureUnitNames = { "mbar", "Pa", "atm", "Torr" };

        private Stopwatch _errorSW = new Stopwatch();

        // Command Process flags
        private Boolean _flagStartIonGauge = false, _flagStopIonGauge = false, _flagRead934Status = false, _flagStartIonGaugeDegas = false, _flagStartIonGaugeDegasOff = false;

        // Not all are implemented See status register Read934Status
        private int _filamentStatus, _ionGaugeEmission, _autoFilamentEnabled, _lowVacGaugeAerror, _lowVacGaugeBerror;

        private double _autoFilamentSetPoint, _gaugeASetPoint, _gaugeBSetPoint, _ionGaugeSetPoint, _gasFactor;
        #endregion Privates Vars



        #region Construction   // constructor name must match the class name

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public InficonVGC083(Single Min, Single Max, String Units)
            : base()
        {
            _min = Min;
            _max = Max;
            _units = Units;
            this.BaudRate = 19200;
            this.SerialPort.ReadTimeout = 100000;
            //pressureSignalA = new AnalogSignal("Gauge A Press", pressureUnitNames[3], "0.000E-0", 1000, false, true);
            pressureSignalA = new AnalogSignal("Gauge A Press", "Torr", "0.00E-0", 1000, false, true);
            pressureSignalB = new AnalogSignal("Gauge B Press", "Torr", "0.00E-0", 1000, false, true);
            pressureIonGauge = new AnalogSignal("Ion Gauge Press", "Torr", "0.00E-0", 1000, false, true);
            Format = "0.0E-0";
        }

        #endregion Construction   // constructor name must match the class name

        #region Private Methods

        /// <summary>
        /// Get Ion Gauge Pressure
        /// </summary>
        ///
        private static Int16 IGErrorReads = 0;

        private Double ReadIGPressure()
        {
            string strDummy = "", strCMD = "";
            strCMD = "#RDIG";
            double fRet = 1000;
            float fMan = 1;
            float fExp = 3;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                string strMan, strExp, strRet;
                try
                {
                    //fRet = Convert.ToSingle(strDummy);
                    Double PreviousRead = this._pressureIonGauge;
                    if (!strDummy.Contains("999"))
                    {
                        if (strDummy == "0" || strDummy == "") // || _filamentStatus == 0)
                        {
                            //_flagRead934Status = true;
                            //Console.WriteLine(" IG Error '" + strDummy + "'");
                            IGErrorReads++;
                            if (IGErrorReads > 6)
                                fRet = 1000;
                            else
                                fRet = Convert.ToSingle(PreviousRead);
                        }
                        else
                        {
                            if (!Double.TryParse(strDummy.Substring(1), out fRet))
                            {
                                fRet = 1000;
                            }
                        }
                        return fRet;
                    }
                    else
                    {
                        return PreviousRead;
                    }
                }
                catch (Exception e)
                {
                    _DoNotCall = false;
                    VtiEvent.Log.WriteError("Error parsing value in ReadPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                    return 1000f;
                    commError = true;
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in ReadPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                commError = true;
            }
            return 1000f;
        }

        /// <summary>
        /// Get Gauge A Pressure 
        /// </summary>
        private Double ReadGaugeAPressure()
        {
            string strDummy = "", strCMD = "";
            strCMD = "#RDCG1";
            double fRet = 1000;
            float fMan = 1;
            float fExp = 3;

            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                string strMan, strExp, strRet;
                try
                {
                    //fRet = Convert.ToSingle(strDummy.Substring(4));
                    Double PreviousRead = this._pressureSignalA;
                    if (!strDummy.Contains("999"))
                    {
                        if (strDummy != "")
                        {
                            if(!Double.TryParse(strDummy.Substring(1),out fRet))
                            {
                                fRet = 1000;
                            }
                        }
                        else { fRet = 1000; }
                        return fRet;
                    }
                    else
                    {
                        return PreviousRead;
                    }
                }
                catch (Exception e)
                {
                    _DoNotCall = false;
                    VtiEvent.Log.WriteError("Error parsing value in ReadGaugeAPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                    //commError = true;
                    return 1000f;
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in ReadGaugeAPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //commError = true;
            }
            return 1000f;
        }

        /// <summary>
        /// Get Gauge B Pressure 
        /// </summary>
        private Double ReadGaugeBPressure()
        {
            string strDummy = "", strCMD = "";
            strCMD = "#RDCG2";
            double fRet = 1000;
            float fMan = 1;
            float fExp = 3;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                string strMan, strExp, strRet;
                try
                {
                    //fRet = Convert.ToSingle(strDummy.Substring(4));
                    Double PreviousRead = this._pressureSignalB;
                    if (!strDummy.Contains("999"))
                    {
                        if (strDummy != "")
                        {
                            if (!Double.TryParse(strDummy.Substring(1), out fRet))
                            {
                                fRet = 1000;
                            }
                        }
                        else { fRet = 1000; }

                        return fRet;
                    }
                    else
                    {
                        return PreviousRead;
                    }
                }
                catch (Exception e)
                {
                    _DoNotCall = false;
                    VtiEvent.Log.WriteError("Error parsing value in ReadGaugeBPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                    //commError = true;

                    return 1000f;
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in ReadGaugeBPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //commError = true;
            }
            return 1000f;
        }

        /// <summary>
        /// Start Ion Gauge; remember to allow time for stabilization
        /// </summary>
        ///
        private string IonGaugeStart()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("#IG1");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside IonGaugeStart", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
            finally { _flagStartIonGauge = false; }
        }

        /// <summary>
        /// Start Ion Gauge Degas; remember to allow time for stabilization
        /// </summary>
        ///
        private string IonGaugeDegasON()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("C");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside IonGaugeDegasON", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
            finally { _flagStartIonGaugeDegas = false; }
        }

        /// <summary>
        /// Start Ion Gauge Degas; remember to allow time for stabilization
        /// </summary>
        ///
        private string IonGaugeDegasOff()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("D");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside IonGaugeDegasOff", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
            finally { _flagStartIonGaugeDegasOff = false; }
        }

        /// <summary>
        /// Stop Ion Gauge; remember to allow about 3 seconds to stop
        /// </summary>
        ///
        private string IonGaugeStop()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("#IG0");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside IonGaugeStop", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
            finally { _flagStopIonGauge = false; }
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
        private void Read934Status()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessRegisterAndReturnValues("E");
                sRet = strDummy;
                // divide string at /r into 26 parameters
                //string[] Reg = strDummy.Split(new char[] {'\r'});  May delete empty lines
                //List<string> Reg = strDummy.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();  // Environment.NewLine = /r/n
                List<string> Reg = strDummy.Split(new char[] { '\r' }, StringSplitOptions.None).ToList();

                //  Display values in console
                //Reg.ForEach(l => Console.WriteLine(l));
                //Console.WriteLine("Value26" + Reg[25].ToString());
                //Console.ReadLine();

                // Populate public status variables
                bool res = int.TryParse(Reg[2], out _filamentStatus);
                res = int.TryParse(Reg[3], out _ionGaugeEmission);
                if (_filamentStatus == 1)
                { Console.WriteLine("Filament is on"); }
                else
                { Console.WriteLine("Filament is off"); }
            }
            catch (Exception e)
            {
                //VtiEvent.Log.WriteError("Error inside Read934Status", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally { _flagRead934Status = false; }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the Terra934 Controller
        /// </summary>
        public override void Process()
        {
            if (Monitor.TryEnter(this.SerialLock, 2000))
            {
                try
                {
                    Thread.Sleep(300);
                    if (_DoNotCall == false)
                    {
                        // Constant delayed process
                        serialPort1.DiscardInBuffer();

                        _pressure = ReadIGPressure();
                        _pressureIonGauge = _pressure;
                        Thread.Sleep(50);
                        _pressureSignalA = ReadGaugeAPressure();
                        Thread.Sleep(50);
                        _pressureSignalB = ReadGaugeBPressure();

                        // Flagged processes (only one at a time)
                        if (_flagStartIonGauge)
                        {
                            this.IonGaugeStart();
                            Thread.Sleep(10000); // long delay when starting IG, if try to read ig, a or b during ig startup trash is returned
                            _flagRead934Status = true;
                        }
                        else if (_flagStopIonGauge)
                        {
                            this.IonGaugeStop();
                            Thread.Sleep(5000);
                            _flagRead934Status = true;
                        }
                        else if (_flagRead934Status)
                        {
                            this.Read934Status();
                        }
                        else if (_flagStartIonGaugeDegas)
                        {
                            this.IonGaugeDegasON();
                            Thread.Sleep(5000);
                            _flagRead934Status = true;
                        }
                        else if (_flagStartIonGaugeDegasOff)
                        {
                            this.IonGaugeDegasOff();
                            Thread.Sleep(5000);
                            _flagRead934Status = true;
                        }
                    }
                    else
                    {
                        //Console.Write("Skipped a read");
                    }

                    if (!this.backgroundWorker1.IsBusy)
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                }
                catch (Exception e)
                {
                    _pressure = Double.NaN;
                    _pressureIonGauge = _pressure;
                    _pressureSignalA = Double.NaN;
                    _pressureSignalB = Double.NaN;
                    VtiEvent.Log.WriteError("Error inside Process", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                    //commError = true;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Processes commands to the Controller
        /// </summary>
        private string ProcessCommandAndReturnValue(string strCommand)
        {
            // Send command
            _DoNotCall = true;
            string CommandSent = strCommand + "\r";
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                serialPort1.WriteLine(CommandSent);
            }
            catch (Exception e)
            {
                //VtiEvent.Log.WriteError("Error inside  ProcessCommandAndReturnValue, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
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
                    Thread.Sleep(200);
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
                //VtiEvent.Log.WriteError("Error inside   ProcessCommandAndReturnValue, Serial Port Read", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
                return "0"; // Read error
            }
            strRead = strRead.Replace("\r", "");
            strRead = strRead.Replace("\n", "");
            strRead = strRead.Replace(">", "");
            strRead = strRead.Trim();
            _DoNotCall = false;
            return strRead; // no error
        }

        /// <summary>
        /// Processes Status Register of the Terra934 Controller
        /// </summary>
        private string ProcessRegisterAndReturnValues(string strCommand)
        {
            // Send command
            _DoNotCall = true;
            string CommandSent = strCommand + "\r";
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                serialPort1.WriteLine(CommandSent);
            }
            catch (Exception e)
            {
                //VtiEvent.Log.WriteError("Error inside  ProcessRegisterAndReturnValues, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
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
                //VtiEvent.Log.WriteError("Error inside   ProcessRegisterAndReturnValues, Serial Port Read", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
                return strRead; // Read error
            }
            //strRead = strRead.Replace("\r", "");
            //strRead = strRead.Replace("\n", "");
            //strRead = strRead.Replace(">", "");
            //strRead = strRead.Trim();
            _DoNotCall = false;
            return strRead; // no error
        }

        /// <summary>
        /// Send Manual Command
        /// </summary>
        public string SendManualCommand(string strCommand)
        {
            string strDummy = "";
            String sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCommand);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside   SendManualCommand", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "";
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

        /// <summary>
        /// Sets a flag to start the ion gauge, check IonGaugeStatus value
        /// </summary>
        public bool StartIonGauge { set { _flagStartIonGauge = true; } }

        /// <summary>
        /// Sets a flag to stop the ion gauge, check IonGaugeStatus value
        /// </summary>
        public bool StopIonGauge { set { _flagStopIonGauge = true; } }

        /// <summary>
        /// Read 934 status register, values can be read from public properties
        /// See private method Read934Status
        /// </summary>
        public bool Read934StatusReg { set { _flagRead934Status = true; } }

        /// <summary>
        /// Fil Status, 1 - on, 0 - off
        /// </summary>
        public int FilamentStatus { get { return _filamentStatus; } internal set { _filamentStatus = value; } }

        /// <summary>
        /// IG Emission, 1 - on, 0 - off
        /// </summary>
        public int IonGaugeEmission { get { return _ionGaugeEmission; } internal set { _ionGaugeEmission = value; } }

        /// <summary>
        /// Sets a flag to start the ion gauge degas, check IonGaugeStatus value
        /// </summary>
        public bool StartIGDegas { set { _flagStartIonGaugeDegas = true; } }

        /// <summary>
        /// Sets a flag to start the ion gauge degas, check IonGaugeStatus value
        /// </summary>
        public bool StopIGDegas { set { _flagStartIonGaugeDegasOff = true; } }

        /// <summary>
        /// Value (pressure) of the Terra934 Controllers IG
        /// </summary>
        public override double Value
        {
            get
            {
                return _pressure;
            }
            internal set
            {
                _pressure = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Value (pressure) of the Terra934 Controllers GaugeA
        /// </summary>
        public double ValueGaugeA
        {
            get
            {
                return _pressureSignalA;
            }
            internal set
            {
                _pressureSignalA = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Value (pressure) of the Terra934 Controllers GaugeB
        /// </summary>
        public double ValueGaugeB
        {
            get
            {
                return _pressureSignalB;
            }
            internal set
            {
                _pressureSignalB = value;
                OnValueChanged();
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
        /// Name for the Terra934 Controller
        /// </summary>
        public override string Name
        {
            get { return "Terra934 Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the Terra934 Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the Terra934 controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the Terra934 controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the Terra934 controller
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