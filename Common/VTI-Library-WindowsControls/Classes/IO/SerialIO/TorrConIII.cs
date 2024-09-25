using System;
using System.ComponentModel;
using System.Threading;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.FormatProviders;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an AEROVAC TorrConIII Convection Gauge Controller
    /// </summary>
    public partial class TorrConIII : SerialIOBase
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

        ///// <summary>
        ///// Occurs when the <see cref="AutoRead">AutoRead</see> changes
        ///// </summary>
        //public event EventHandler AutoReadChanged;
        ///// <summary>
        ///// Raises the <see cref="AutoReadChanged">AutoReadChanged</see> event
        ///// </summary>
        //protected void OnAutoReadChanged()
        //{
        //    if (AutoReadChanged != null)
        //        AutoReadChanged(this, null);
        //}

        #endregion Event Handlers

        #region Globals

        private Single pressure = Single.NaN;//, _voltage;
        private Boolean pressureReady;//, _voltageReady;
        private String format = "0.000E-0";
        private TorrConFormatProvider _formatProvider = new TorrConFormatProvider();

        //private Boolean autoRead = true;
        private String errCode;

        private String _Units = "Torr";

        private double _volt;
        private double _offset;
        private double _gain;
        private double _setpoint1;
        private double _setpoint2;
        private string _macaddress;
        private string _ipaddress;
        private string _subnetmask;
        private string _gatewayaddress;
        private int _ipport;
        private string _unitsstring;
        private double _unitsgain;
        private double _unitsoffset;
        private int _convectionflag;
        private int _numofdispchar;

        private bool _readVoltage;
        private bool _readOffset;
        private bool _readGain;
        private bool _setOffset;
        private bool _setGain;

        private bool _readSetPoint1;
        private bool _setSetPoint1;
        private bool _readSetPoint2;
        private bool _setSetPoint2;
        private bool _readMacAddress;
        private bool _setMacAddress;
        private bool _readIPAddress;
        private bool _setIPAddress;
        private bool _readSubnetMask;
        private bool _setSubnetMask;
        private bool _readGatewayAddress;
        private bool _setGatewayAddress;
        private bool _readIPPort;
        private bool _setIPPort;
        private bool _readUnitsString;
        private bool _setUnitsString;
        private bool _readUnitsGain;
        private bool _setUnitsGain;
        private bool _readUnitsOffset;
        private bool _setUnitsOffset;
        private bool _readConvectionFlag;
        private bool _setConvectionFlag;
        private bool _readNumOfDispChar;
        private bool _setNumOfDispChar;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TorrConIII()
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TorrConIII(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.serialPort1.BaudRate = 2400;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TorrConIII(SerialPortParameter SerialPortParameter)
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
            this.SerialPortParameter = SerialPortParameter;
        }

        #endregion Construction

        #region Private Methods

        /// <summary>
        /// Thread for reading the <see cref="Pressure">Pressure</see> of the TorrConII Controller
        /// </summary>
        public override void Process()
        {
            String retVal;
            Single tempPress;

            if (Monitor.TryEnter(this.SerialLock, 1500))
            {
                try
                {
                    serialPort1.DiscardInBuffer();
                    bool breadOffset = false;
                    bool breadGain = false;

                    if (_setGain)
                    {
                        serialPort1.WriteLine(string.Format("G{0:0.0000}", _gain));
                        _setGain = false;
                        return;
                    }
                    else if (_readGain)
                    {
                        serialPort1.Write("g");
                        breadGain = true;
                    }
                    else if (_setOffset)
                    {
                        serialPort1.WriteLine(string.Format("O{0:0.0000}", _offset));
                        _setOffset = false;
                        return;
                    }
                    else if (_readOffset)
                    {
                        serialPort1.Write("o");
                        breadOffset = true;
                    }
                    else
                    {
                        serialPort1.Write("p");
                    }
                    retVal = serialPort1.ReadLine();
                    if (breadOffset)
                    {
                        _readOffset = false;
                        double tempOffset;
                        Double.TryParse(retVal, out tempOffset);
                        _offset = tempOffset;
                    }
                    else if (breadGain)
                    {
                        _readGain = false;
                        double tempGain;
                        Double.TryParse(retVal, out tempGain);
                        _gain = tempGain;
                    }
                    else
                    {
                        if (retVal.StartsWith("0.0000-"))
                        {
                            pressure = 0.000F;
                            errCode = String.Empty;
                            pressureReady = true;
                            if (!backgroundWorker1.IsBusy)
                                backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                        }
                        else if (Single.TryParse(retVal, out tempPress))
                        {
                            pressure = tempPress;
                            errCode = String.Empty;
                            pressureReady = true;
                            if (!backgroundWorker1.IsBusy)
                                backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                        }
                        else if (retVal.StartsWith("HHH") ||
                            retVal.StartsWith("EPL") ||
                            retVal.StartsWith("EPH") ||
                            retVal.StartsWith("CBL"))
                        {
                            pressureReady = false;
                            pressure = Single.NaN;
                            errCode = retVal.Substring(0, 3);
                        }
                        else
                        {
                            pressureReady = false;
                            pressure = Single.NaN;
                            errCode = String.Empty;
                        }
                    }
                }
                catch
                {
                    pressureReady = false;
                    commError = true;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        #endregion Private Methods

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
        /// Pressure (in torr) of the TorrConIII Controller
        /// </summary>
        public Single Pressure
        {
            get
            {
                Single value = Single.NaN;
                if (pressureReady)
                {
                    //_pressureReady = false;
                    return pressure;
                }
                else
                {
                    if (this.IsAvailable && !commError)
                    {
                        Monitor.Enter(this.SerialLock);
                        try
                        {
                            if (!serialPort1.IsOpen) serialPort1.Open();
                            serialPort1.Write("p");
                            value = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
                        }
                        catch (ThreadAbortException te)
                        {
                            throw te;
                        }
                        catch
                        {
                            commError = true;
                            //throw new Exception("Error reading Pressure from TorrCon III on port " + serialPort1.PortName);
                        }
                        finally
                        {
                            Monitor.Exit(this.SerialLock);
                        }
                    }
                    return value;
                }
            }
        }

        /// <summary>
        /// Voltage reading from the TorrConIII Controller
        /// </summary>
        public Single Voltage
        {
            get
            {
                Single retVal = Single.NaN;
                if (Monitor.TryEnter(this.SerialLock, 500))
                {
                    try
                    {
                        if (!serialPort1.IsOpen) serialPort1.Open();
                        serialPort1.Write("v");
                        retVal = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
                    }
                    catch (ThreadAbortException te)
                    {
                        throw te;
                    }
                    catch
                    {
                        commError = true;
                        throw new Exception("Error reading Voltage from TorrCon III on port " + serialPort1.PortName);
                    }
                    finally
                    {
                        Monitor.Exit(this.SerialLock);
                    }
                }
                return retVal;
            }
        }

        /// <summary>
        /// Setpoint 1 of the TorrConIII Controller
        /// </summary>
        public Single Setpoint1
        {
            get
            {
                Single value = Single.NaN;
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.Write("1");
                    value = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
                }
                catch (ThreadAbortException te)
                {
                    throw te;
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error reading Setpoint 1 from TorrCon III on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return value;
            }
        }

        /// <summary>
        /// Setpoint 2 of the TorrConIII Controller
        /// </summary>
        public Single Setpoint2
        {
            get
            {
                Single value = Single.NaN;
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.Write("2");
                    value = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
                }
                catch (ThreadAbortException te)
                {
                    throw te;
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error reading Setpoint 2 from TorrCon II on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return value;
            }
        }

        /// <summary>
        /// RelayStatus of the TorrConIII Controller
        /// </summary>
        /// <remarks>
        /// The Relay Status is a two character string with each character being either a
        /// 0 or a 1 to indicate the status of each relay.
        /// </remarks>
        public String RelayStatus
        {
            get
            {
                String value = string.Empty;
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.Write("r");
                    value = serialPort1.ReadLine();
                }
                catch (ThreadAbortException te)
                {
                    throw te;
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error reading Relay Status from TorrCon II on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return value;
            }
        }

        /// <summary>
        /// Name for the TorrConIII Controller
        /// </summary>
        public override string Name
        {
            get { return "TorrConIII on port " + serialPort1.PortName; }
        }

        /// <summary>
        /// RawValue (Voltage) from the TorrConIII Controller
        /// </summary>
        /// <remarks>
        /// This property is not implemented!
        /// </remarks>
        public override double RawValue
        {
            get { return this.Voltage; }
        }

        /// <summary>
        /// Minimum value for the TorrConIII Controller
        /// </summary>
        /// <value>1.0E-3</value>
        public override double Min
        {
            get { return 1e-3; }
        }

        /// <summary>
        /// Maximum value for the TorrConIII Controller
        /// </summary>
        /// <value>1.0E3</value>
        public override double Max
        {
            get { return 1e3; }
        }

        /// <summary>
        /// Units for the value for the TorrConIII Controller
        /// </summary>
        /// <value>torr</value>
        public override string Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        /// <summary>
        /// Format string for the value for the TorrConIII Controller
        /// </summary>
        public override string Format
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Value of the TorrConII Controller formatted to match the display on the controller
        /// </summary>
        public override String FormattedValue
        {
            get
            {
                if (Single.IsNaN(pressure))
                {
                    if (errCode != String.Empty)
                        return errCode;
                    else
                        return "ERROR";
                }
                else
                    return String.Format("{0} {1}", String.Format(_formatProvider, "{0}", pressure), (pressure >= 1 ? "Torr" : "mTorr"));
            }
        }

        /// <summary>
        /// Value (pressure) for the TorrConIII Controller
        /// </summary>
        public override double Value
        {
            get { return this.Pressure; }
            internal set
            {
                pressure = (Single)value;
                OnValueChanged();
            }
        }

        public Boolean ReadVoltage
        {
            get { return _readVoltage; }
            set { _readVoltage = value; }
        }

        public Boolean ReadOffset
        {
            get { return _readOffset; }
            set { _readOffset = value; }
        }

        public Boolean ReadGain
        {
            get { return _readGain; }
            set { _readGain = value; }
        }

        public Boolean SetOffset
        {
            get { return _setOffset; }
            set { _setOffset = value; }
        }

        public Boolean SetGain
        {
            get { return _setGain; }
            set { _setGain = value; }
        }

        public Boolean ReadSetPoint1
        {
            get { return _readSetPoint1; }
            set { _readSetPoint1 = value; }
        }

        public Boolean SetSetPoint1
        {
            get { return _setSetPoint1; }
            set { _setSetPoint1 = value; }
        }

        public Boolean ReadSetPoint2
        {
            get { return _readSetPoint2; }
            set { _readSetPoint2 = value; }
        }

        public Boolean SetSetPoint2
        {
            get { return _setSetPoint2; }
            set { _setSetPoint2 = value; }
        }

        public Boolean ReadMACAddress
        {
            get { return _readMacAddress; }
            set { _readMacAddress = value; }
        }

        public Boolean SetMACAddress
        {
            get { return _setMacAddress; }
            set { _setMacAddress = value; }
        }

        public Boolean ReadIPAddress
        {
            get { return _readIPAddress; }
            set { _readIPAddress = value; }
        }

        public Boolean SetIPAddress
        {
            get { return _setIPAddress; }
            set { _setIPAddress = value; }
        }

        public Boolean ReadSubnetMask
        {
            get { return _readSubnetMask; }
            set { _readSubnetMask = value; }
        }

        public Boolean SetSubnetMask
        {
            get { return _setSubnetMask; }
            set { _setSubnetMask = value; }
        }

        public Boolean ReadGatewayAddress
        {
            get { return _readGatewayAddress; }
            set { _readGatewayAddress = value; }
        }

        public Boolean SetGatewayAddress
        {
            get { return _setGatewayAddress; }
            set { _setGatewayAddress = value; }
        }

        public Boolean ReadIPPort
        {
            get { return _readIPPort; }
            set { _readIPPort = value; ; }
        }

        public Boolean SetIPPort
        {
            get { return _setIPPort; }
            set { _setIPPort = value; }
        }

        public Boolean ReadUnitsString
        {
            get { return _readUnitsString; }
            set { _readUnitsString = value; }
        }

        public Boolean SetUnitsString
        {
            get { return _setUnitsString; }
            set { _setUnitsString = value; }
        }

        public Boolean ReadUnitsGain
        {
            get { return _readUnitsGain; }
            set { _readUnitsGain = value; }
        }

        public Boolean SetUnitsGain
        {
            get { return _setUnitsGain; }
            set { _setUnitsGain = value; }
        }

        public Boolean ReadUnitsOffset
        {
            get { return _readUnitsOffset; }
            set { _readUnitsOffset = value; }
        }

        public Boolean SetUnitsOffset
        {
            get { return _setUnitsOffset; }
            set { _setUnitsOffset = value; }
        }

        public Boolean ReadConvectionFlag
        {
            get { return _readConvectionFlag; }
            set { _readConvectionFlag = value; }
        }

        public Boolean SetConvectionFlag
        {
            get { return _setConvectionFlag; }
            set { _setConvectionFlag = value; }
        }

        public Boolean ReadNumOfCharToDisp
        {
            get { return _readNumOfDispChar; }
            set { _readNumOfDispChar = value; }
        }

        public Boolean SetNumOfCharToDisp
        {
            get { return _setNumOfDispChar; }
            set { _setNumOfDispChar = value; }
        }

        public double Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public double Gain
        {
            get { return _gain; }
            set { _gain = value; }
        }

        public double Volt
        {
            get { return _volt; }
            set { _volt = value; }
        }

        public double SetPoint1
        {
            get { return _setpoint1; }
            set { _setpoint1 = value; }
        }

        public double SetPoint2
        {
            get { return _setpoint2; }
            set { _setpoint2 = value; }
        }

        public string MACAddress
        {
            get { return _macaddress; }
            set { _macaddress = value; }
        }

        public string IPAddress
        {
            get { return _ipaddress; }
            set { _ipaddress = value; }
        }

        public string SubnetMask
        {
            get { return _subnetmask; }
            set { _subnetmask = value; }
        }

        public string GatewayAddress
        {
            get { return _gatewayaddress; }
            set { _gatewayaddress = value; }
        }

        public int IPPort
        {
            get { return _ipport; }
            set { _ipport = value; }
        }

        public string UnitsString
        {
            get { return _unitsstring; }
            set { _unitsstring = value; }
        }

        public double UnitsGain
        {
            get { return _unitsgain; }
            set { _unitsgain = value; }
        }

        public double UnitsOffset
        {
            get { return _unitsoffset; }
            set { _unitsoffset = value; }
        }

        public int ConvectionFlag
        {
            get { return _convectionflag; }
            set { _convectionflag = value; }
        }

        public int NumOfDisplayChar
        {
            get { return _numofdispchar; }
            set { _numofdispchar = value; }
        }

        ///// <summary>
        ///// Gets or sets a value to indicate if the TorrCon should automatically read the pressure
        ///// or if the reading has been interrupted by a front-panel keypress.
        ///// </summary>
        //public Boolean AutoRead
        //{
        //    get { return autoRead; }
        //    set { autoRead = value; }
        //}

        #endregion Public Properties
    }
}