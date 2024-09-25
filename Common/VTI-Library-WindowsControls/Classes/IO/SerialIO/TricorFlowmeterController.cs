using System;
using System.ComponentModel;
using System.Threading;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.FormatProviders;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an Tricor Flowmeter Controller
    /// </summary>
    public partial class TricorFlowmeterController : SerialIOBase
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
        private Single temperature = Single.NaN;
        private Single charge = Single.NaN;
        private Single flow = Single.NaN;
        private Boolean pressureReady;//, _voltageReady;
        private String format = "{0:0.000 }";
        private TorrConFormatProvider _formatProvider = new TorrConFormatProvider();

        //private Boolean autoRead = true;
        private String errCode;

        private String _Units = "oz/s";

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TricorFlowmeterController()
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TricorFlowmeterController(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.serialPort1.BaudRate = 2400;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrConII">TorrConII</see> class
        /// </summary>
        public TricorFlowmeterController(SerialPortParameter SerialPortParameter)
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
            Single processValue = 20.0F;

            //charge = -1.0F;

            byte[] dataByte;
            dataByte = new Byte[50];

            byte[] InputData;
            InputData = new Byte[50];

            try
            {
                if (commError)
                {
                }
                else
                {
                    //commError = true;
                    processValue = 0.0F;

                    int i, NUMDATA;

                    byte Crc_HByte, Crc_LByte;
                    ushort Crc;

                    NUMDATA = 6;

                    //Mass flow rate
                    dataByte[0] = 0x01;
                    dataByte[1] = 0x03;

                    dataByte[2] = (byte)(((ushort)20000 & (ushort)0xFF00) / ((ushort)256));
                    dataByte[3] = (byte)((ushort)20000 & (ushort)0x00FF);

                    dataByte[4] = 0x00;
                    dataByte[5] = 0x02;

                    Crc = (ushort)0XFFFF;

                    for (i = 0; i < NUMDATA; i++)
                    {
                        Crc = CRC16(Crc, dataByte[i]);
                    }

                    Crc_LByte = (byte)(Crc & (ushort)0x00FF);
                    Crc_HByte = (byte)((Crc & (ushort)0xFF00) / ((ushort)256));

                    dataByte[NUMDATA] = Crc_LByte;
                    dataByte[NUMDATA + 1] = Crc_HByte;

                    this.SerialPort.Write(dataByte, 0, NUMDATA + 2);

                    //Thread.Sleep(300);
                    Thread.Sleep(50);

                    int BytesToRead = this.SerialPort.BytesToRead;
                    if (BytesToRead > 0)
                    {
                        this.SerialPort.Read(InputData, 0, BytesToRead);
                    }
                    processValue = BitConverter.ToSingle(new byte[] { InputData[5], InputData[6], InputData[4], InputData[3] }, 0);

                    //Mass Batch Total
                    dataByte[0] = 0x01;
                    dataByte[1] = 0x03;

                    dataByte[2] = (byte)(((ushort)20002 & (ushort)0xFF00) / ((ushort)256));
                    dataByte[3] = (byte)((ushort)20002 & (ushort)0x00FF);

                    dataByte[4] = 0x00;
                    dataByte[5] = 0x02;

                    Crc = (ushort)0XFFFF;

                    for (i = 0; i < NUMDATA; i++)
                    {
                        Crc = CRC16(Crc, dataByte[i]);
                    }

                    Crc_LByte = (byte)(Crc & (ushort)0x00FF);
                    Crc_HByte = (byte)((Crc & (ushort)0xFF00) / ((ushort)256));

                    dataByte[NUMDATA] = Crc_LByte;
                    dataByte[NUMDATA + 1] = Crc_HByte;

                    this.SerialPort.Write(dataByte, 0, NUMDATA + 2);

                    //Thread.Sleep(300);
                    Thread.Sleep(50);

                    BytesToRead = this.SerialPort.BytesToRead;
                    if (BytesToRead > 0)
                    {
                        this.SerialPort.Read(InputData, 0, BytesToRead);
                    }
                    charge = BitConverter.ToSingle(new byte[] { InputData[5], InputData[6], InputData[4], InputData[3] }, 0);
                }
            }
            catch
            {
            }
            //this.ModbusInterface.SerialPort.r

            temperature = processValue;
            pressure = processValue;
            flow = processValue;
        }

        //CRC16 calculation
        private ushort CRC16(ushort crc, ushort data)
        {
            ushort Poly16 = 0xA001;
            ushort LSB, i;
            ushort FFOO = 0xFF00;
            ushort OOFF = 0x00FF;
            ushort OOO1 = 0x0001;
            ushort lOOO = 0x1000;
            crc = (ushort)((ushort)((ushort)(crc ^ data) | FFOO) & (ushort)(crc | OOFF));
            for (i = 0; i < 8; i++)
            {
                LSB = (ushort)(crc & OOO1);
                crc = (ushort)(crc / 2);
                if (LSB >= (ushort)0x1)
                {
                    crc = (ushort)(crc ^ Poly16);
                }
            }
            return (crc);
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
        /// Flow (in oz/s) of the TricorFlowmeter Controller
        /// </summary>
        public Single Flow
        {
            get
            {
                return flow;
            }
        }

        /// <summary>
        /// Charge (in oz) of the TricorFlowmeter Controller
        /// </summary>
        public Single Charge
        {
            get
            {
                return charge;
            }
        }

        /// <summary>
        /// Pressure (in torr) of the TricorFlowmeter Controller
        /// </summary>
        public Single Pressure
        {
            get
            {
                return pressure;
            }
        }

        /// <summary>
        /// Voltage reading from the TricorFlowmeter Controller
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
        /// Setpoint 1 of the TricorFlowmeter Controller
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
        /// Setpoint 2 of the TricorFlowmeter Controller
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
        /// RelayStatus of the TricorFlowmeter Controller
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
        /// Name for the TricorFlowmeter Controller
        /// </summary>
        public override string Name
        {
            get { return "TricorFlowmeter on port " + serialPort1.PortName; }
        }

        /// <summary>
        /// RawValue (Voltage) from the TricorFlowmeter Controller
        /// </summary>
        /// <remarks>
        /// This property is not implemented!
        /// </remarks>
        public override double RawValue
        {
            get { return this.Voltage; }
        }

        /// <summary>
        /// Minimum value for the TricorFlowmeter Controller
        /// </summary>
        /// <value>1.0E-3</value>
        public override double Min
        {
            get { return 1e-3; }
        }

        /// <summary>
        /// Maximum value for the TricorFlowmeter Controller
        /// </summary>
        /// <value>1.0E3</value>
        public override double Max
        {
            get { return 1e3; }
        }

        /// <summary>
        /// Units for the value for the TricorFlowmeter Controller
        /// </summary>
        /// <value>torr</value>
        public override string Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        /// <summary>
        /// Format string for the value for the TricorFlowmeter Controller
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
                //if (errCode != String.Empty)
                //{
                //    return errCode;
                //}
                //else
                {
                    return String.Format("{0} {1}", String.Format(format, temperature), Units);
                }
            }
        }

        /// <summary>
        /// Value (pressure) for the TricorFlowmeter Controller
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