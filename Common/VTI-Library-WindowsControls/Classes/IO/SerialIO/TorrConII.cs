using System;
using System.ComponentModel;
using System.Threading;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.FormatProviders;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an AEROVAC TorrConII Convection Gauge Controller
    /// </summary>
    public partial class TorrConII : SerialIOBase
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

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TorrConII()
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 2400;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TorrConII(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.serialPort1.BaudRate = 2400;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TorrConII(SerialPortParameter SerialPortParameter)
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 2400;
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

            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                try
                {
                    serialPort1.Write("p");
                    retVal = serialPort1.ReadLine();
                    if (Single.TryParse(retVal, out tempPress))
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
        /// Pressure (in torr) of the TorrConII Controller
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
                            //throw new Exception("Error reading Pressure from TorrCon II on port " + serialPort1.PortName);
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
        /// Voltage reading from the TorrConII Controller
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
                        throw new Exception("Error reading Voltage from TorrCon II on port " + serialPort1.PortName);
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
        /// Setpoint 1 of the TorrConII Controller
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
                    throw new Exception("Error reading Setpoint 1 from TorrCon II on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return value;
            }
        }

        /// <summary>
        /// Setpoint 2 of the TorrConII Controller
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
        /// RelayStatus of the TorrConII Controller
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
        /// Name for the TorrConII Controller
        /// </summary>
        public override string Name
        {
            get { return "TorrConII on port " + serialPort1.PortName; }
        }

        /// <summary>
        /// RawValue (Voltage) from the TorrConII Controller
        /// </summary>
        /// <remarks>
        /// This property is not implemented!
        /// </remarks>
        public override double RawValue
        {
            get { return this.Voltage; }
        }

        /// <summary>
        /// Minimum value for the TorrConII Controller
        /// </summary>
        /// <value>1.0E-3</value>
        public override double Min
        {
            get { return 1e-3; }
        }

        /// <summary>
        /// Maximum value for the TorrConII Controller
        /// </summary>
        /// <value>1.0E3</value>
        public override double Max
        {
            get { return 1e3; }
        }

        /// <summary>
        /// Units for the value for the TorrConII Controller
        /// </summary>
        /// <value>torr</value>
        public override string Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        /// <summary>
        /// Format string for the value for the TorrConII Controller
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
        /// Value (pressure) for the TorrConII Controller
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