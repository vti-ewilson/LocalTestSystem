using System;
using System.Data.Linq;
using System.Threading;

/// <summary>
/// Developed for VTI VGMS 1/2020
/// Todd A. Scott
/// Revisions: 0
///
/// </summary>
namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for a MKS 670 Controller
    /// </summary>
    public class MKS670 : SerialIOBase
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MKS670">MKS670</see> class
        /// </summary>
        public MKS670(Single Min, Single Max, String Units, int maxRetries = 2)
            : base()
        {
            _min = Min;
            _max = Max;
            _units = Units;
            this.SerialPort.ReadTimeout = 100000;
            pressureSignal = new AnalogSignal("MKS670", pressureUnitNames[3], "##0.00E+0", 1000, false, true);
            Format = "##0.00E+0";
            MaxRetries = maxRetries;
        }

        #endregion Construction

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

        #region Private Fields

        private Boolean _DoNotCall = false;  // Uses serial lock
        private MKSChannel _channel;
        private Double _pressure;
        private Single _min, _max;
        private String _units;
        private String _format = "##0.00E+0";
        private String[] pressureUnitNames = { "mbar", "Pa", "atm", "Torr" };
        private AnalogSignal pressureSignal;

        // Process flags and values
        private Boolean _flagStartZeroBaratron = false;

        private Boolean _flagReturntoReadMode = false;
        private MKSMode _currentMode = MKSMode.Pressure;

        private bool _setChannelFlag = false;
        private MKSRange _range;
        private bool _setRangeFlag = false;
        private bool _setModeFlag = false;
        private bool _setAveragingFlag = false;
        private MKSResponseTime _responseTime = MKSResponseTime.OneMiliSecond;
        private bool _setResponseTimeFlag = true;
        private int _averagingValue;
        private bool _flagEnableRS232Averaging = false;
        private bool _averagingEnabled = false;
        private bool _calibrating = false;
        private bool _startCalibrate = false;

        private MKSResponse _commandResonseStatus { get; set; }

        #endregion Private Fields

        #region Public Properties

        public int MaxRetries = 2;

        /// <summary>
        /// Value (pressure) of the MKS 670 Controller
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
        /// Name for the MKS 670 Controller
        /// </summary>
        public override string Name
        {
            get { return "MKS670 Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Last value recorded for channel, check channel to see if value is current
        /// </summary>
        public double Channel1_LastValue { get; private set; }

        /// <summary>
        /// Last value recorded for channel, check channel to see if value is current
        /// </summary>
        public double Channel2_LastValue { get; private set; }

        /// <summary>
        /// Last value recorded for channel, check channel to see if value is current
        /// </summary>
        public double Channel3_LastValue { get; private set; }

        /// <summary>
        /// Minimum value for the MKS 670 Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the MKS 670 controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the MKS670 controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Change or get currently selected channel 0=1, 1=2, 2=3
        /// </summary>
        public MKSChannel Channel
        {
            get
            {
                return _channel;
            }
            set
            {
                _channel = value;
                _setChannelFlag = true;
            }
        }

        /// <summary>
        /// Set response time for the selected channel, see MKSResponseTime enum
        /// </summary>
        public MKSResponseTime ResponseTime
        {
            get
            {
                return (MKSResponseTime)_responseTime;
            }
            set
            {
                _responseTime = value;
                _setResponseTimeFlag = true;
            }
        }

        /// <summary>
        /// Set the gain range for the selected channel
        /// </summary>
        public MKSRange GainRange
        {
            get
            {
                return _range;
            }
            set
            {
                _range = value;
                _setRangeFlag = true;
            }
        }

        /// <summary>
        /// Set number of values to average for selected channel when RS232 averaging is enabled
        /// </summary>
        public int RS232AveragingCount
        {
            get
            {
                return _averagingValue;
            }
            set
            {
                _averagingValue = value;
                _setAveragingFlag = true;
            }
        }

        private bool _enableZeroSelectedChannel;
        private static System.Timers.Timer _delayTimer;
        private MKSChannel _targetChannel;
        private MKSChannel _currentChannel;
        private bool _DoNotChangeChannelsYet;

        /// <summary>
        /// Current Mode of MKSMode Enum; Pressure, Zero, ...
        /// </summary>
        public MKSMode Mode
        {
            get
            {
                return _currentMode;
            }
            set
            {
                _setModeFlag = true;
                _currentMode = value;
            }
        }

        public bool CalibrationInProgress { get { return _calibrating; } }

        /// <summary>
        /// Indicates current channel is over range
        /// </summary>
        public bool OverRange { get; private set; } = false;

        /// <summary>
        /// Indicates current channel is Under range
        /// </summary>
        public bool UnderRange { get; private set; } = false;

        /// <summary>
        /// Sets a flag to start the zero calibrate procedure
        /// </summary>
        public bool ZeroSelectedChannelStart { set { _flagStartZeroBaratron = true; } }

        /// <summary>
        /// Returns the 670 to read mode after zero calibrate
        /// </summary>
        public bool ReturnBaratronToReadMode { set { _flagReturntoReadMode = true; } }

        /// <summary>
        /// Set to true to enable RS232 Averaging or false to disable
        /// </summary>
        public bool EnabledRS232Averaging
        {
            get
            {
                return _averagingEnabled;
            }
            set
            {
                _averagingEnabled = value;
                _flagEnableRS232Averaging = true;
            }
        }

        /// <summary>
        /// Cycles through all enabled channels in process
        /// Set response time = 1mSec and Averaging = 1 for best response time
        /// </summary>
        public bool CycleThroughEnabledChannels { get; set; } = true;

        /// <summary>
        /// Enabled/Disable channelOne for cycle through all enabled channels
        /// Set channel command disables CycleThroughEnabledChannels
        /// </summary>
        public bool EnableChannelOne { get; set; } = true;

        /// <summary>
        /// Enabled/Disable channel 2 for cycle through all enabled channels
        /// Set channel command disables CycleThroughEnabledChannels
        /// </summary>
        public bool EnableChannelTwo { get; set; } = true;

        /// <summary>
        /// Enabled/Disable channel 3 for cycle through all enabled channels
        /// Set channel command disables CycleThroughEnabledChannels
        /// </summary>
        public bool EnableChannelThree { get; set; } = true;

        /// <summary>
        /// Format string for the value for the MKS670 controller
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

        #region Private Methods

        /// <summary>
        ///  read the baratron pressure
        ///  add indication of which channel is overranged or underranged
        /// </summary>
        private double ReadPressure()
        {
            string strDummy = "", strCMD = "";
            strCMD = "@020";
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD + "?");
                if (strDummy == "")
                {
                    Thread.Sleep(500);
                    strDummy = ProcessCommandAndReturnValue(strCMD + "?");
                }
                Double PreviousRead = this._pressure;
                float parseResult;
                if (strDummy.Contains(strCMD))  // returned a valid command
                {
                    int nCRLocation;
                    nCRLocation = strDummy.IndexOf("\r");
                    if (nCRLocation > 0)
                        strDummy = strDummy.Substring(0, nCRLocation);

                    int nOverRange = strDummy.IndexOf("#");
                    int nUnderRange = strDummy.IndexOf("\"");
                    int nCalibrating = strDummy.IndexOf("!");
                    if (nCalibrating > 0)
                        _calibrating = true;
                    else if (_currentMode != MKSMode.Pressure)
                    {
                        ReturnBaratronToReadMode = true;
                        _calibrating = false;
                    }

                    if (nOverRange > 0)
                    { // Set overrange flag
                        if (strDummy.Substring(0, 5) == "@020#")
                            strDummy = strDummy.Substring(5);
                        OverRange = true;
                        if (float.TryParse(strDummy, out parseResult))
                            return parseResult;
                        else { return _max; }
                    }
                    else if (nUnderRange > 0)
                    {
                        UnderRange = true;
                        if (strDummy.Substring(0, 5) == "@020#")
                            strDummy = strDummy.Substring(5);

                        if (float.TryParse(strDummy, out parseResult))
                            return parseResult;
                        else { return _max; }
                    }
                    else
                    { // Parse Value
                        OverRange = false;
                        UnderRange = false;
                        if (strDummy.Substring(0, 4) == "@020")
                            strDummy = strDummy.Substring(5);

                        if (float.TryParse(strDummy, out parseResult))
                            return parseResult;
                        else
                        { return _max; }
                    }
                }
                else
                { return _max; }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command in MKS 670 ReadPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return _max;
        }

        /// <summary>
        /// set the gain range see MKSRange enum
        /// </summary>
        private MKSResponse SetGainRangeOnCurrentChannel(MKSRange Range)
        {
            string strCMD;
            string response;
            strCMD = "@17" + (int)_channel + (int)Range + "\r";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                if (response != "")
                {
                    _commandResonseStatus = CommandResponse(response);
                    GetGainRange();
                    if (_range == Range) _setRangeFlag = false;
                }
                return _commandResonseStatus;
            }
            catch (Exception e)
            {
                _setRangeFlag = false;
                VtiEvent.Log.WriteError("Error processing command SetGainRangeOnCurrentChannel in MKS 670 ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        /// set the Response time on current channel
        /// send MKSResponseTime enum 0 = 1mSec, 1 = 40mSec, 2 = 400mSec
        /// </summary>
        private MKSResponse SetResponseTimeOnCurrentChannel(MKSResponseTime ResponseTime)
        {
            string strCMD;
            string response;
            strCMD = "@11" + (int)_channel + (int)ResponseTime + "\r";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                _commandResonseStatus = CommandResponse(response);
                if (_commandResonseStatus == MKSResponse.CommandAccepted)
                {
                }
                _setResponseTimeFlag = false;
                return _commandResonseStatus;
            }
            catch (Exception e)
            {
                _setResponseTimeFlag = false;
                VtiEvent.Log.WriteError("Error processing command SetResponseTimeOnCurrentChannel in MKS 670 ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        /// Set the instrument channel
        /// returns error number if failed
        /// </summary>
        private MKSResponse SelectChannel(MKSChannel Ch)
        {
            string response;
            string strCMD;
            strCMD = "@000" + (int)Ch + "\r";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                _commandResonseStatus = CommandResponse(response);
                if (_commandResonseStatus == MKSResponse.CommandAccepted)
                {
                    _setChannelFlag = false;
                    //if (_responseTime == MKSResponseTime.FourHundredMiliSeconds) Thread.Sleep(500);
                }
                return _commandResonseStatus;
            }
            catch (Exception e)
            {
                _setChannelFlag = false;
                VtiEvent.Log.WriteError("Error processing command Select channel in MKS 670 ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        /// Returns currently selected channel or -1 if error processing command
        /// </summary>
        /// <returns></returns>
        public MKSChannel GetSelectedChannel()
        {
            string response;
            string strCMD;
            strCMD = "@000?" + "\r";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                _commandResonseStatus = CommandResponse(response);
                if (_commandResonseStatus == MKSResponse.CommandAccepted)
                {
                    bool OK = int.TryParse(response.Substring(4, 1), out int result);
                    if (OK) _channel = (MKSChannel)result;
                    return _channel;
                }
                return _channel;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command Select channel in MKS 670 ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return _channel;
            }
        }

        private MKSRange GetGainRange()
        {
            string response;
            string strCMD;
            strCMD = "@17" + (int)_channel + "?" + "\r";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                _commandResonseStatus = CommandResponse(response);
                if (_commandResonseStatus == MKSResponse.CommandAccepted)
                {
                    bool OK = int.TryParse(response.Substring(4, 1), out int result);
                    if (OK) _range = (MKSRange)result;
                    return _range;
                }
                return MKSRange.time1;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command get gain range in MKS 670 ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSRange.time1;
            }
        }

        private MKSResponse CommandResponse(string response)
        {
            if (response != "")
            {
                string strPrefix = response.Substring(0, 1);
                if (strPrefix == "@")
                    return MKSResponse.CommandAccepted;
                else if (strPrefix == ">")
                    return MKSResponse.ParameterValueUnrecognized;
                else if (strPrefix == "?")
                    return MKSResponse.DataFieldValueInvalid;
                else if (strPrefix == "=")
                    return MKSResponse.CommandIsInappropriate;
                else
                    return MKSResponse.NoResponse;
            }
            return MKSResponse.NoResponse;
        }

        /// <summary>
        /// Use in place of thread.sleep if you want to continue
        /// processes on the same thread and need a delay
        /// </summary>
        /// <param name="Time_delay"></param>
        private static void delay(int Time_delay)
        {
            int i = 0;
            //  ameTir = new System.Timers.Timer();
            _delayTimer = new System.Timers.Timer();
            _delayTimer.Interval = Time_delay;
            _delayTimer.AutoReset = false; //so that it only calls the method once
            _delayTimer.Elapsed += (s, args) => i = 1;
            _delayTimer.Start();
            while (i == 0) { };
        }

        /// <summary>
        /// Processes commands to the MKS SRG Controller
        /// </summary>
        private string ProcessCommandAndReturnValue(string strCommand)
        {
            // Send command
            _DoNotCall = true;
            string CommandSent = strCommand + "\r";
            try
            {
                serialPort1.DiscardInBuffer();
                serialPort1.WriteLine(CommandSent);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside MKS 670 ProcessCommandAndReturnValue, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
                return "-1"; // write error
            }

            // Read back response
            DateTime dt = DateTime.Now;
            long dataLength = 20; double timeout = 3;
            string strRead = ""; // CLEAR VALUE
            try
            {
                DateTime dtStart = DateTime.Now;
                TimeSpan ts;
                int numReads = 0, prevStrReadLength, numEmptyReads = 0;

                int readDelay = 300; // mSec
                if (_averagingValue < 3) { readDelay = 200; }
                if (_averagingValue > 3 && _averagingValue < 10) { readDelay = 250; }
                if (_averagingValue > 10) { readDelay = 1000; timeout = 3; }
                if (_averagingValue > 50) { readDelay = 2000; timeout = 6; }

                do
                {
                    /*delay*/
                    Thread.Sleep(readDelay);
                    prevStrReadLength = strRead.Length;
                    strRead += serialPort1.ReadExisting();
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
                VtiEvent.Log.WriteError("Error inside ProcessCommandAndReturnValue, Serial Port Read", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
                return strRead; // Read error
            }
            _DoNotCall = false;
            return strRead; // no error
        }

        /// <summary>
        ///  Return the baratron to read mode
        ///  Needs called after Zero calibate completes
        /// </summary>
        private MKSResponse ReturntoReadMode()
        {
            string strCMD = "";
            string response;
            strCMD = "@0100";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                _commandResonseStatus = CommandResponse(response);
                if (_commandResonseStatus == MKSResponse.CommandAccepted)
                {
                    _flagReturntoReadMode = false;
                    _currentMode = MKSMode.Pressure;
                }
                return _commandResonseStatus;
            }
            catch (Exception e)
            {
                _flagReturntoReadMode = false;
                VtiEvent.Log.WriteError("Error processing command in ReturnToReadMode", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        ///  Zero the selected channels CDG
        ///  // Needs work
        /// </summary>
        private void ZeroSelectedChannel()
        {
            try
            {
                _enableZeroSelectedChannel = true;
                Mode = MKSMode.Zero;
                _startCalibrate = true;
                _flagStartZeroBaratron = false;
                // wait for calibrate to complete before reading pressure
            }
            catch (Exception e)
            {
                _flagStartZeroBaratron = false;
                VtiEvent.Log.WriteError("Error processing command in ZeroSelectedChannel", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }

        /// <summary>
        /// Enable remote zero
        /// Pass in true to enable and failse to disable
        /// </summary>
        private MKSResponse EnableZeroOnSelectedChannel(bool TrueFalse)
        {
            string strDummy = "", strCMD = "";
            try
            {
                if (TrueFalse)
                    strCMD = "@18" + (int)_channel + "1";
                else strCMD = "@18" + (int)_channel + "0";
                _enableZeroSelectedChannel = false;
                strDummy = ProcessCommandAndReturnValue(strCMD);
                return CommandResponse(strDummy);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command Enable Zero On MKS670", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        /// Enable RS232 Averaging
        /// Pass in true to enable and failse to disable
        /// </summary>
        private MKSResponse EnableRS232Averaging(bool TrueFalse)
        {
            string strDummy = "", strCMD = "";
            try
            {
                if (TrueFalse)
                    strCMD = "@040" + "1";
                else strCMD = "@040" + "0";

                strDummy = ProcessCommandAndReturnValue(strCMD);
                _flagEnableRS232Averaging = false;
                return CommandResponse(strDummy);
            }
            catch (Exception e)
            {
                _flagEnableRS232Averaging = false;
                VtiEvent.Log.WriteError("Error processing command Enable RS-232 Averaging On MKS670", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        /// Send number of readings to average for RS232 Averaging
        /// </summary>
        private MKSResponse SetAveragingOnCurrentChannel(int One_To_OneHundred)
        {
            string response;
            string strCMD;
            strCMD = "@16" + (int)_channel + (int)One_To_OneHundred + "\r";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                _commandResonseStatus = CommandResponse(response);
                if (_commandResonseStatus == MKSResponse.CommandAccepted)
                {
                    _setAveragingFlag = false;
                }
                return _commandResonseStatus;
            }
            catch (Exception e)
            {
                _setChannelFlag = false;
                VtiEvent.Log.WriteError("Error processing command Set Averaging in MKS 670 ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        ///  Send Calibrate to selected channel
        ///  Set calibration mode first
        /// </summary>
        private MKSResponse SetMode(MKSMode mode)
        {
            string strDummy = "", strCMD = "";
            try
            {
                strCMD = "@010" + (int)mode;
                strDummy = ProcessCommandAndReturnValue(strCMD);
                _setModeFlag = false;
                return CommandResponse(strDummy);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command SetMode On MKS670", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _setModeFlag = false;
                return MKSResponse.NoResponse;
            }
        }

        /// <summary>
        ///  Send Calibrate to selected channel
        ///  Set calibration mode first
        /// </summary>
        private MKSResponse Calibrate()
        {
            string strDummy = "", strCMD = "";
            try
            {
                strCMD = "@030"; // Calibrate Command
                strDummy = ProcessCommandAndReturnValue(strCMD);
                _startCalibrate = false;
                return CommandResponse(strDummy);
            }
            catch (Exception e)
            {
                _startCalibrate = false;
                VtiEvent.Log.WriteError("Error processing command in ZeroBaratron", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return MKSResponse.NoResponse;
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the MKS670 Controller
        /// </summary>
        public override void Process()
        {
            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                try
                {
                    serialPort1.DiscardInBuffer();
                    // Check que for commands
                    // for a que flags become counts, _channelChangeFlag must check target channel and que commands for channel

                    delay(300);
                    //if ((_setAveragingFlag || _setModeFlag || _setRangeFlag || _setResponseTimeFlag ||
                    //    _startCalibrate || _enableZeroSelectedChannel || _flagStartZeroBaratron || _flagReturntoReadMode) && (_setChannelFlag))
                    //{
                    //    _targetChannel = _channel;
                    //    _currentChannel = GetSelectedChannel();
                    //    if (_targetChannel != _currentChannel)
                    //    {
                    //        _channel = _targetChannel;
                    //    }
                    //    else
                    //_DoNotChangeChannelsYet = true;
                    //}
                    //else
                    //{
                    _DoNotChangeChannelsYet = false;
                    //}

                    if (_setChannelFlag && _DoNotChangeChannelsYet == false)
                    {
                        SelectChannel(_channel);
                        /*delay*/
                        delay(500);
                    }
                    else if (_flagReturntoReadMode)
                    {
                        this.ReturntoReadMode();
                    }
                    else if (_setResponseTimeFlag)
                    {
                        SetResponseTimeOnCurrentChannel(_responseTime);
                    }
                    else if (_flagEnableRS232Averaging)
                    {
                        EnableRS232Averaging(_averagingEnabled);
                    }
                    else if (_setAveragingFlag)
                    {
                        SetAveragingOnCurrentChannel(_averagingValue);
                    }
                    else if (_setRangeFlag)
                    {
                        SetGainRangeOnCurrentChannel(_range);
                    }
                    else if (_flagStartZeroBaratron)
                    {
                        this.ZeroSelectedChannel();
                    }
                    else if (_enableZeroSelectedChannel)
                    {
                        EnableZeroOnSelectedChannel(true);
                    }
                    else if (_setModeFlag)
                    {
                        SetMode(Mode);
                    }
                    else if (_startCalibrate)
                    {
                        Calibrate();
                        delay(300);
                    }
                    else
                    {
                        bool doneReading = false;
                        int attemps = 0;
                        // if there is a change greater than 1000% or if the reading is > 999, read again to see if it was a fluke
                        while (!doneReading)
                        {
                            _pressure = ReadPressure();
                            if (_pressure != 1000)
                            {
                                switch (_channel)
                                {
                                    case MKSChannel.one:
                                        Channel1_LastValue = _pressure;
                                        doneReading = true;
                                        break;
                                    case MKSChannel.two:
                                        Channel2_LastValue = _pressure;
                                        doneReading = true;
                                        break;
                                    case MKSChannel.three:
                                        Channel3_LastValue = _pressure;
                                        doneReading = true;
                                        break;
                                }
                            }
                            
                            attemps++;
                            if (!doneReading)
                                delay(300);
                        }

                        if (CycleThroughEnabledChannels)
                        {
                            SelectNextChannel();
                        }
                    }

                    if (!this.backgroundWorker1.IsBusy)
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                }
                catch (Exception e)
                {
                    pressureSignal.Value = Double.NaN;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        ///  If no channels are enabled MKS670 defaults to channel 3 which is typically the 1000 Torr CDG
        /// </summary>
        private void SelectNextChannel()
        {
            if (EnableChannelOne && _channel == MKSChannel.one)
            {
                if (EnableChannelTwo)
                {
                    _channel = MKSChannel.two;
                    _setChannelFlag = true;
                }
                else if (EnableChannelThree)
                {
                    _channel = MKSChannel.three;
                    _setChannelFlag = true;
                }
            }
            else if (EnableChannelTwo && _channel == MKSChannel.two)
            {
                if (EnableChannelThree)
                {
                    _channel = MKSChannel.three;
                    _setChannelFlag = true;
                }
                else if (EnableChannelOne)
                {
                    _channel = MKSChannel.one;
                    _setChannelFlag = true;
                }
            }
            else if (EnableChannelThree && _channel == MKSChannel.three)
            {
                if (EnableChannelOne)
                {
                    _channel = MKSChannel.one;
                    _setChannelFlag = true;
                }
                else if (EnableChannelTwo)
                {
                    _channel = MKSChannel.two;
                    _setChannelFlag = true;
                }
            }
            else
            {
                if (_channel == MKSChannel.three && EnableChannelThree == false) _channel = MKSChannel.two;
                if (_channel == MKSChannel.two && EnableChannelTwo == false) _channel = MKSChannel.one;
                if (_channel == MKSChannel.one && EnableChannelOne == false) _channel = MKSChannel.three;
                _setChannelFlag = true;
            }
        }

        /// <summary>
        /// determine the range setting for the pressure value
        /// </summary>
        public int DetermineRange(float Press, float XOpt01PressHighLimit, float XOpt1PressHighLimit)
        {
            if (XOpt01PressHighLimit != 0f && Press <= XOpt01PressHighLimit)
                return 2; // x0.01
            else if (XOpt1PressHighLimit != 0 && Press <= XOpt1PressHighLimit)
                return 1; // x0.1
            else
                return 0; // x1
        }

        #endregion Public Methods

        #region enums

        public enum MKSRange
        {
            time1 = 0,
            timept1 = 1,
            timept01 = 2,
            AutomaticRange = 3
        }

        public enum MKSChannel
        {
            one = 0,
            two,
            three
        }

        public enum MKSMode
        {
            Pressure = 0,
            Zero,
            Null,
            FullScale,
            SystemCheck
        }

        public enum MKSResponseTime
        {
            OneMiliSecond = 0,
            FourtyMiliSeconds,
            FourHundredMiliSeconds
        }

        public enum MKSResponse
        {
            NoResponse = -1,
            CommandAccepted = 0,
            ParameterValueUnrecognized = 1,
            DataFieldValueInvalid = 2,
            CommandIsInappropriate = 3
        }

        #endregion enums

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
    }
}