using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.FormatProviders;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;
using VTIWindowsControlLibrary.Classes.Configuration;
using System.IO;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    public partial class KeyenceScanner : SerialIOBase
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
        private const Byte STX = 0x02;
        private const Byte ETX = 0x03;
        private const Byte CR = 0x0d;
        private const int RECV_DATA_MAX = 10240;
        private const int ReadTimeout = 100;
        public static bool ScannerOK = false;

        private string _units = "";
        private string _format = "0";
        private string StartReading = "\x02LON\x03";
        private string StopReading = "\x02LOFF\x03"; // used to cancel the read, such as in a timeout
        private string ClearBuffer = "\x02" + "BCLR\x03";
        private string FocusAdjust = "\x02" + "FTUNE\x03";
        private string Version = "\x02KEYENCE\x03";
        private string Tune = "\x02TUNE,01\x03";
        private string TuneQuit = "\x02QTUNE\x03";
        private string _data = "";

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyenceScanner">Keyence Barcode Scanner</see> class
        /// </summary>
        public KeyenceScanner()
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 115200;
            this.serialPort1.DataBits = 8;
            this.serialPort1.Parity = Parity.Even;
            this.serialPort1.StopBits = StopBits.One;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyenceScanner">Keyence Barcode Scanner</see> class
        /// </summary>
        public KeyenceScanner(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.serialPort1.BaudRate = 115200;
            this.serialPort1.DataBits = 8;
            this.serialPort1.Parity = Parity.Even;
            this.serialPort1.StopBits = StopBits.One;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyenceScanner">Keyence Barcode Scanner</see> class
        /// </summary>
        public KeyenceScanner(SerialPortParameter SerialPortParameter)
        {
            InitializeComponent();
            this.SerialPortParameter = SerialPortParameter;
            this.serialPort1.BaudRate = SerialPortParameter.ProcessValue.BaudRate;
            this.serialPort1.DataBits = SerialPortParameter.ProcessValue.DataBits;
            this.serialPort1.Parity = SerialPortParameter.ProcessValue.Parity;
            this.serialPort1.StopBits = SerialPortParameter.ProcessValue.StopBits;
            this.serialPort1.Handshake = SerialPortParameter.ProcessValue.Handshake;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Required thread - empty
        /// </summary>
        public override void Process()
        {

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

        #region public methods
        public void CheckVersion()
        {
            try
            {
                if (!serialPort1.IsOpen) serialPort1.Open();
                serialPort1.ReadTimeout = ReadTimeout;
                byte[] StartReadingBytes = ASCIIEncoding.ASCII.GetBytes(Version);
                serialPort1.Write(StartReadingBytes, 0, StartReadingBytes.Length);
                Thread.Sleep(200);
                string strRead = "";
                DateTime dtStart = DateTime.Now;
                TimeSpan ts;
                int numReads = 0, prevStrReadLength, numEmptyReads = 0;
                do
                {
                    Thread.Sleep(50);
                    prevStrReadLength = strRead.Length;
                    strRead += serialPort1.ReadExisting();
                    if (strRead.StartsWith("\x02"))
                    {
                        
                    }

                    if (strRead.Length == prevStrReadLength)
                        numEmptyReads++;
                    else
                        numEmptyReads = 0;
                    if (strRead.EndsWith("\x0d"))
                    {
                        break;
                    }
                    numReads++;
                    ts = DateTime.Now - dtStart;
                }
                while (numEmptyReads < 3 && ts.TotalMilliseconds < 200);

                if (strRead.Contains("KEYENCE"))
                {
                    // started ok
                    ScannerOK = true;
                }
                else
                {
                    VtiEvent.Log.WriteWarning("Cant verify Scanner version on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, strRead);
                }
            }
            catch (IOException e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error verifying Scanner version on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }
        public void Scan(int timeout = 0)
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            serialPort1.ReadTimeout = ReadTimeout;
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                byte[] StartReadingBytes = ASCIIEncoding.ASCII.GetBytes(StartReading);
                serialPort1.Write(StartReadingBytes, 0, StartReadingBytes.Length);
            }
            catch (IOException e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error sending start command to Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }
        public void TuneStart(int timeout = 0)
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            serialPort1.ReadTimeout = ReadTimeout;
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                byte[] StartReadingBytes = ASCIIEncoding.ASCII.GetBytes(Tune);
                serialPort1.Write(StartReadingBytes, 0, StartReadingBytes.Length);
            }
            catch (IOException e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error sending tune command to Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }
        public void TuneEnd(int timeout = 0)
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            serialPort1.ReadTimeout = ReadTimeout;
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                byte[] StartReadingBytes = ASCIIEncoding.ASCII.GetBytes(TuneQuit);
                serialPort1.Write(StartReadingBytes, 0, StartReadingBytes.Length);
            }
            catch (IOException e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error sending quit tuning command to Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }
        public void Clear()
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                byte[] StartReadingBytes = ASCIIEncoding.ASCII.GetBytes(ClearBuffer);
                serialPort1.Write(StartReadingBytes, 0, StartReadingBytes.Length);
            }
            catch (IOException e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error sending clear command to Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }
        public void Focus()
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                byte[] StartReadingBytes = ASCIIEncoding.ASCII.GetBytes(FocusAdjust);
                serialPort1.Write(StartReadingBytes, 0, StartReadingBytes.Length);
            }
            catch (IOException e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error sending stop command to Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }
        public void Abort()
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            try
            {
                serialPort1.DiscardInBuffer();
                Thread.Sleep(75);
                byte[] StartReadingBytes = ASCIIEncoding.ASCII.GetBytes(StopReading);
                serialPort1.Write(StartReadingBytes, 0, StartReadingBytes.Length);
            }
            catch (IOException e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error sending stop command to Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
        }
        public int Read(int timeout = 200)
        {
            int res = 0;
            string strRead = "";
            try
            {
                DateTime dtStart = DateTime.Now;
                TimeSpan ts;
                int numReads = 0, prevStrReadLength, numEmptyReads = 0;
                do
                {
                    Thread.Sleep(50);
                    prevStrReadLength = strRead.Length;
                    strRead += serialPort1.ReadExisting();
                    if (strRead.StartsWith("\x02"))
                    {
                        strRead = "";
                        prevStrReadLength = 0;
                        numEmptyReads = 0;
                    }
                        
                    if (strRead.Length == prevStrReadLength)
                        numEmptyReads++;
                    else
                        numEmptyReads = 0;
                    if (strRead.EndsWith("\x0d"))
                    {
                        break;
                    }
                    numReads++;
                    ts = DateTime.Now - dtStart;
                }
                while (numEmptyReads < 3 && ts.TotalMilliseconds < timeout);
            }
            catch (Exception ex)
            {
                res = 1;
                VtiEvent.Log.WriteError("Error reading from Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, ex.Message);
            }

            Data = strRead;

            return res;
            /*

            byte[] recieved = new byte[RECV_DATA_MAX];
            int size;

            for (; ; )
            {
                try
                {
                    size = readDataSub(recieved, serialPort1);
                }
                catch (IOException e)
                {
                    res = 1;
                    commError = true;
                    VtiEvent.Log.WriteError("Error reading from Scanner on " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                    break;
                }
                if (size == 0)
                {
                    res = 1;
                    Data = "";
                    break;
                }
                if (recieved[0] == STX)
                {
                    //
                    // Skip if command response.
                    //
                    continue;
                }
                else
                {
                    //
                    // Show the receive data after converting the receive data to Shift-JIS.
                    // Terminating null to handle as string.
                    //
                    recieved[size] = 0;
                    Data = Encoding.GetEncoding("Shift_JIS").GetString(recieved);
                    break;
                }
            }

            return res;*/
        }

        private bool checkDataSize(byte[] recvBytes, int recvSize)
        {
            const int dataSizeLen = 4;

            if (recvSize < dataSizeLen)
            {
                return false;
            }

            int dataSize = 0;
            int mul = 1;
            for (int i = 0; i < dataSizeLen; i++)
            {
                dataSize += (recvBytes[dataSizeLen - 1 - i] - '0') * mul;
                mul *= 10;
            }

            return (dataSize + 1 == recvSize);
        }

        private int readDataSub(byte[] recvBytes, SerialPort serialPortInstance)
        {
            int recvSize = 0;
            bool isCommandRes = false;
            byte d;

            //
            // Distinguish between command response and read data.
            //
            try
            {
                d = (byte)serialPortInstance.ReadByte();
                recvBytes[recvSize++] = d;
                if (d == STX)
                {
                    isCommandRes = true;    // Distinguish between command response and read data.
                }
            }
            catch (TimeoutException)
            {
                return 0;   //  No data received.
            }

            //
            // Receive data until the terminator character.
            //
            for (; ; )
            {
                try
                {
                    d = (byte)serialPortInstance.ReadByte();
                    recvBytes[recvSize++] = d;

                    if (isCommandRes && (d == ETX))
                    {
                        break;  // Command response is received completely.
                    }
                    else if (d == CR)
                    {
                        if (checkDataSize(recvBytes, recvSize))
                        {
                            break;  // Read data is received completely.
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    return 0;
                }
            }

            return recvSize;
        }
        #endregion

        #region Public Properties

        public string Data
        {
            get { return _data; }
            internal set
            {
                _data = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Name for the Keyence Barcode Scanner
        /// </summary>
        public override string Name
        {
            get { return "Keyence Barcode Scanner on port " + serialPort1.PortName; }
        }

        /// <summary>
        /// RawValue (Voltage) from the Keyence Barcode Scanner
        /// </summary>
        /// <remarks>
        /// This property is not implemented!
        /// </remarks>
        public override double RawValue
        {
            get { return 0; }
        }

        /// <summary>
        /// Minimum value for the Keyence Barcode Scanner
        /// </summary>
        /// <value>1.0E-3</value>
        public override double Min
        {
            get { return 0; }
        }

        /// <summary>
        /// Maximum value for the Keyence Barcode Scanner
        /// </summary>
        /// <value>1.0E3</value>
        public override double Max
        {
            get { return 0; }
        }

        /// <summary>
        /// Units for the value for the Keyence Barcode Scanner
        /// </summary>
        /// <value>torr</value>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the Keyence Barcode Scanner
        /// </summary>
        public override string Format
        {
            get { return _format; }
            set
            {
                _format = value;

            }
        }

        /// <summary>
        /// Value of the Keyence Barcode Scanner formatted to match the display on the controller
        /// </summary>
        public override String FormattedValue
        {
            get
            {
                if (commError)
                {
                    return "COM ERROR";
                }
                else
                {
                    return "IDLE";
                }
            }
        }
        private double v = 0;
        public override double Value { get => 0; internal set => v = value; }

        #endregion
    }
}
