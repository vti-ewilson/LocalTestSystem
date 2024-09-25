using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO.Ports;
using System.Threading;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.FormatProviders;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;
using VTIWindowsControlLibrary.Classes.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an Messkonzept FTC320 Helium Purity Monitor
    /// </summary>
    public partial class MesskonzeptFTC320 : SerialIOBase
    {
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

        #endregion

        #region Globals

        private double ppm = double.NaN;
        public static double MinimumCalibrationDelay = 10;
        
        private string format = "0.000E-0";

        private string _Units = "ppm";

        private bool _startOffsetCalibration;
        private bool _waitingOnOffsetCalibration;
        private bool _setOffset;
        private bool _getOffset;
        private bool _startGainCalibration;
        private bool _waitingOnGainCalibration;
        private bool _setGain;
        private bool _getGain;

        private double _setGainTo = double.NaN;
        private double _gain = double.NaN;

        private double _setOffsetTo = double.NaN;
        private double _offset = double.NaN;
        public DateTime? LastCalibratedOffset { get; set; } = null;
        public DateTime? LastCalibratedGain { get; set; } = null;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MesskonzeptFTC320">MesskonzeptFTC320</see> class
        /// </summary>
        public MesskonzeptFTC320()
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MesskonzeptFTC320">MesskonzeptFTC320</see> class
        /// </summary>
        public MesskonzeptFTC320(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MesskonzeptFTC320">MesskonzeptFTC320</see> class
        /// </summary>
        public MesskonzeptFTC320(SerialPortParameter SerialPortParameter)
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
            this.SerialPortParameter = SerialPortParameter;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Thread for reading the PPM of the MesskonzeptFTC320 Purity Monitor
        /// </summary>
        public override void Process()
        {
            string retVal;
            //Single tempPress;

            if (Monitor.TryEnter(this.SerialLock, 1500))
            {
                try
                {
                    bool readOffset = false;
                    bool readGain = false;
                    bool startedCalibration = false;
                    bool gotResult = false;

                    if (_startGainCalibration)
                    {
                        retVal = ProcessCommandAndReturnValue("P12=F251");
                        startedCalibration = true;
                        _startGainCalibration = false;
                        _waitingOnGainCalibration = true;
                    }
                    else if (_startOffsetCalibration)
                    {
                        retVal = ProcessCommandAndReturnValue("P12=F250");
                        startedCalibration = true;
                        _startOffsetCalibration = false;
                        _waitingOnOffsetCalibration = true;
                    }
                    else if (_setGain)
                    {
                        retVal = ProcessCommandAndReturnValue($"P399=F{_setGainTo.ToString("F1")}");
                        _setGain = false;
                        return;
                    }
                    else if (_getGain)
                    {
                        retVal = ProcessCommandAndReturnValue("P402?");
                        readGain = true;
                    }
                    else if (_setOffset)
                    {
                        retVal = ProcessCommandAndReturnValue($"P398=F{_setOffsetTo.ToString("F1")}");
                        _setOffset = false;
                        return;
                    }
                    else if (_getOffset)
                    {
                        retVal = ProcessCommandAndReturnValue("P401?");
                        readOffset = true;
                    }
                    else 
                    {
                        retVal = ProcessCommandAndReturnValue("P408?");
                    }

                    if (!string.IsNullOrWhiteSpace(retVal) && !startedCalibration)
                    {
                        char[] delimiter = { '=', ':' };
                        string[] vals = retVal.Split(delimiter);
                        if (vals.Length == 4)
                        {
                            retVal = vals[1];
                            if (retVal.StartsWith("F"))
                                retVal = retVal.Substring(1);
                            gotResult = true;
                        }
                    }

                    if (readOffset)
                    {
                        _getOffset = false;
                        if (gotResult && double.TryParse(retVal, out double offset))
                        {
                            _offset = offset;
                            _waitingOnOffsetCalibration = false;
                        }
                    }
                    else if (readGain)
                    {
                        _getGain = false;
                        if (gotResult && double.TryParse(retVal, out double gain))
                        {
                            _gain = gain;
                            _waitingOnGainCalibration = false;
                        }
                    }
                    else if (!startedCalibration && gotResult && double.TryParse(retVal, out double _ppm)) // Concentration return
                    {
                        try
                        {
                            Value = _ppm;
                            if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
                        }
                        catch
                        {

                        }
                    }


                }
                catch (Exception ex) 
                {
                    commError = true;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

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
                VtiEvent.Log.WriteError("Error inside MesskonzeptFTC320 ProcessCommandAndReturnValue, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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
                return strRead; // Read error
            }
            return strRead; // no error
        }

        #endregion

        #region Events

        /// <summary>
        /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
        }

        #endregion

        #region Public Properties

        public double PPM
        {
            get
            {
                return ppm;
            }
        }

        /// <summary>
        /// Name for the MesskonzeptFTC320 Controller
        /// </summary>
        public override string Name
        {
            get { return "MesskonzeptFTC320 on port " + serialPort1.PortName; }
        }

        /// <summary>
        /// RawValue (Voltage) from the MesskonzeptFTC320 Controller
        /// </summary>
        /// <remarks>
        /// This property is not implemented!
        /// </remarks>
        public override double RawValue
        {
            get { return this.ppm; }
        }

        /// <summary>
        /// Minimum value for the MesskonzeptFTC320 Controller
        /// </summary>
        /// <value>1.0E-3</value>
        public override double Min
        {
            get { return 1e-3; }
        }

        /// <summary>
        /// Maximum value for the MesskonzeptFTC320 Controller
        /// </summary>
        /// <value>1.0E3</value>
        public override double Max
        {
            get { return 1e6; }
        }

        /// <summary>
        /// Units for the value for the MesskonzeptFTC320 Controller
        /// </summary>
        /// <value>torr</value>
        public override string Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        /// <summary>
        /// Format string for the value for the MesskonzeptFTC320 Controller
        /// </summary>
        public override string Format
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Value of the TorrConII Controller formatted to match the display on the controller
        /// </summary>
        public override string FormattedValue
        {
            get
            {
                if (double.IsNaN(ppm))
                {
                    return "ERROR";
                }
                else if (_waitingOnGainCalibration)
                {
                    return "Cal Gain";
                }
                else if (_waitingOnOffsetCalibration)
                {
                    return "Cal Offset";
                }
                else
                    return $"{ppm} {Units}";
            }
        }

        /// <summary>
        /// Value (pressure) for the MesskonzeptFTC320 Controller
        /// </summary>
        public override double Value
        {
            get { return this.ppm; }
            internal set
            {
                ppm = (double)value;
                OnValueChanged();
            }
        }

        public bool ReadOffset
        {
            get { return _getOffset; }
            set { _getOffset = value; }
        }
        public bool ReadGain
        {
            get { return _getGain; }
            set { _getGain = value; }
        }
        public double Offset
        {
            get { return _offset; }
            set 
            { 
                _setOffsetTo = value;
                _setOffset = true;
            }
        }
        public double Gain
        {
            get { return _gain; }
            set
            {
                _setGainTo = value;
                _setGain = true;
            }
        }
        #endregion

        public bool CalibrateOffset
        {
            get { return _waitingOnOffsetCalibration; }
            set
            {
                _startOffsetCalibration = value;
            }
        }

        public bool CalibrateGain
        {
            get { return _waitingOnGainCalibration; }
            set
            {
                _startGainCalibration = value;
            }
        }
    }
}
