using System;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    public class Keithley2000scan : SerialIOBase
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="Keithley2000">Keithley2000</see> class
        /// </summary>
        public Keithley2000scan(Single Min, Single Max, String Units)
            : base()
        {
            _min = Min;
            _max = Max;
            _units = Units;
            this.SerialPort.ReadTimeout = 100000;
            pressureSignal = new AnalogSignal("Keithley2000", pressureUnitNames[3], "##0.00E+0", 1000, false, true);
            Format = "##0.00E+0";
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

        private int _channel;
        private Double _signal;
        private Single _min, _max;
        private String _units;
        private String _format = "##0.00E+0";
        private String[] pressureUnitNames = { "vDC", "vAC", "IDC", "IAC" };
        private AnalogSignal pressureSignal;

        // Process flags
        private bool _setChannelFlag = false;

        //private KeithleyResponse _commandResonseStatus;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Value (pressure) of the Keithley 2000 Controller
        /// </summary>
        public override double Value
        {
            get
            {
                return _signal;
            }
            internal set
            {
                _signal = value;
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
        /// Name for the Keithley 2000 Controller
        /// </summary>
        public override string Name
        {
            get { return "Keithley Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the Keithley 2000 Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the Keithley 2000 controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the Keithley2000 controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Change or get currently selected channel 0=1, 1=2, 2=3
        /// </summary>
        public int Channel
        {
            get { return _channel; }
            set
            {
                _channel = value;
                _setChannelFlag = true;
            }
        }

        /// <summary>
        /// Format string for the value for the Keithley2000 controller
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

        public double Channel1_LastValue { get; private set; }
        public double Channel2_LastValue { get; private set; }
        public double Channel3_LastValue { get; private set; }
        public double Channel4_LastValue { get; private set; }
        public double Channel5_LastValue { get; private set; }
        public double Channel6_LastValue { get; private set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Processes commands to the MKS SRG Controller
        /// </summary>
        private string ProcessCommandAndReturnValue(string strCommand)
        {
            // Send command
            string CommandSent = strCommand + "\r\n";
            try
            {
                serialPort1.DiscardInBuffer();
                serialPort1.WriteLine(CommandSent);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside Keithley 2000 ProcessCommandAndReturnValue, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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

                int readDelay = 250; // mSec
                do
                {
                    KeithleyCmdDelay(readDelay);
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
                return strRead; // Read error
            }
            return strRead; // no error
        }

        /// <summary>
        ///  Requests the latest reading without triggering.
        /// </summary>
        private double FETCH()
        {
            string strDummy = "", strCMD = "";
            strCMD = ":FETCH?";
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);

                Double PreviousRead = this.Value;

                if (_channel == 1)
                    PreviousRead = Channel1_LastValue;
                if (_channel == 2)
                    PreviousRead = Channel2_LastValue;
                if (_channel == 3)
                    PreviousRead = Channel3_LastValue;
                if (_channel == 4)
                    PreviousRead = Channel4_LastValue;
                if (_channel == 5)
                    PreviousRead = Channel5_LastValue;
                if (_channel == 6)
                    PreviousRead = Channel6_LastValue;


                double parseResult;
                if (strDummy.Contains("\r"))  // returned a valid command
                {
                    int nCRLocation;
                    nCRLocation = strDummy.IndexOf("\r");
                    if (nCRLocation > 0)
                        strDummy = strDummy.Substring(0, nCRLocation);

                    strDummy = strDummy.Replace("\u0011", "");
                    strDummy = strDummy.Replace("\u0013", "");

                    if (double.TryParse(strDummy, out parseResult))
                    {
                        if (parseResult == -1 || parseResult == 100) // -1 or 100 are errors
                        {
                            return PreviousRead;
                        }
                        return parseResult;
                    }
                    else
                    { return PreviousRead; }
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command FETCH? in Keithley", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return _max;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the Keithley2000 Controller
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

                    KeithleyCmdDelay(300);
                    if (_setChannelFlag)
                    {
                        SelectChannel(_channel);
                        KeithleyCmdDelay(500);
                    }
                    else
                    {
                        _signal = FETCH();

                        if (_channel == 1)
                            Channel1_LastValue = _signal;
                        if (_channel == 2)
                            Channel2_LastValue = _signal;
                        if (_channel == 3)
                            Channel3_LastValue = _signal;
                        if (_channel == 4)
                            Channel4_LastValue = _signal;
                        if (_channel == 5)
                            Channel5_LastValue = _signal;
                        if (_channel == 6)
                            Channel6_LastValue = _signal;

                        //if (CycleThroughEnabledChannels)
                        //{
                        //    SelectNextChannel();
                        //}
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

        private string SelectChannel(int channel)
        {
            string response;
            string strCMD;
            // Status = fnSendCommand(":route:open:all" & vbCrLf, InstrumentNum)
            //Status = fnSendCommand(":route:close:State?" & vbCrLf, InstrumentNum)
            strCMD = ":route:open:all";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error processing command Open All channels in Keithley ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }

            //":route:close (@" & channelnum & ")" & vbCrLf'
            strCMD = ":route:close (@" + (int)channel + ")";
            try
            {
                response = ProcessCommandAndReturnValue(strCMD);
                //_commandResonseStatus = CommandResponse(response);
                //if (_commandResonseStatus == KeithleyResponse.CommandAccepted)
                //{
                _setChannelFlag = false;
                //if (_responseTime == MKSResponseTime.FourHundredMiliSeconds) Thread.Sleep(500);
                //}
                return "done"; // _commandResonseStatus;
            }
            catch (Exception e)
            {
                _setChannelFlag = false;
                VtiEvent.Log.WriteError("Error processing command Select channel in Keithley ", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "error"; // KeithleyResponse.NoResponse;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void SelectNextChannel()
        {
        }

        #endregion Public Methods

        /// <summary>
        ///  Do not call from multiple locations this is a static instance, if active does not reset, create new instance for each use
        /// </summary>
        /// <param name="Time_delay"></param>
        private static void KeithleyCmdDelay(int Time_delay)
        {
            int i = 0;
            System.Timers.Timer
            _delayTimer = new System.Timers.Timer();
            _delayTimer.Interval = Time_delay;
            _delayTimer.AutoReset = false; //so that it only calls the method once
            _delayTimer.Elapsed += (s, args) => i = 1;
            _delayTimer.Start();
            while (i == 0) { };
        }

        /// <summary>
        /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
        }
    }
}