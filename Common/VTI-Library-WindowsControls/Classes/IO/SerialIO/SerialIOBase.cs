using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Base class for Serial I/O devices
    /// </summary>
    /// <remarks>
    /// This base class takes care of many of the Serial Port properties,
    /// making development of Serial I/O device interfaces simpler.
    /// </remarks>
    public abstract partial class SerialIOBase : Component, IFormattedAnalogInput
    {
        #region Globals

        private Thread thrd;
        private Object serialLock = new Object();
        private Boolean isAvailable = false;
        private SerialPortParameter serialPortParameter;
        private Boolean stopProcessing;
        private EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private int waitDelay = 100;
        private int retries = 0;

        /// <summary>
        /// Value to indicate that a communications error has occurred.
        /// </summary>
        protected Boolean commError = false;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialIOBase">SerialIOBase</see> class
        /// </summary>
        public SerialIOBase()
        {
            InitializeComponent();

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialIOBase">SerialIOBase</see> class
        /// </summary>
        public SerialIOBase(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }

        #endregion Construction

        #region Events

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BackgroundProcess();
        }

        #endregion Events

        #region Private Methods

        private void ProcessThrd()
        {
            //Stopwatch sw = new Stopwatch();
            while (!stopProcessing)
            {
                //sw.Start();
                try
                {
                    if (commError)
                    {
                        if (++retries > 5)
                        {
                            VtiEvent.Log.WriteError(
                                    String.Format("An error has occurred communicating with {0}", this.Name),
                                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                            this.Stop();
                            isAvailable = false;
                            this.Value = Double.NaN;
                            return;
                        }
                        else commError = false;
                    }
                    else retries = 0;
                    if (this.IsAvailable)
                    {
                        this.Process();
                    }
                    //exitEvent.WaitOne(waitDelay, true);
                    //changed to Thread.Sleep() to drastically improve CPU usage
                    Thread.Sleep(waitDelay);
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError(
                        String.Format("An error has occurred in the thread processing the {0}", this.Name),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        e.ToString());
                }
                //sw.Stop();
                //Console.WriteLine("Serial device process ms: " + sw.ElapsedMilliseconds);
                //sw.Reset();
            }
            isAvailable = false;
            this.Value = Double.NaN;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Opens the serial port and starts the
        /// <see cref="Process">Process</see> thread
        /// </summary>
        public void Start()
        {
            Stop();
            Thread.Sleep(50);

            int retries = 0;
            int maxRetries = 10;
            while (retries < maxRetries)
            {
                try
                {
                    VtiEvent.Log.WriteVerbose(
                        String.Format("Starting the processing thread for {0}", this.Name),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                    serialPort1.ReadTimeout = 1500;
                    serialPort1.WriteTimeout = 1500;
                    if (!serialPort1.IsOpen)
                    {
                        serialPort1.Open();
                    }
                    if (serialPort1.IsOpen)
                    {
                        commError = false;
                        stopProcessing = false;
                        thrd = new Thread(new ThreadStart(ProcessThrd));
                        thrd.Start();
                        thrd.IsBackground = true;
                        thrd.Name = this.Name; // "Serial IO";
                        isAvailable = true;
                    }
                    retries = maxRetries;
                }
                catch (UnauthorizedAccessException uae)
                {
                    retries++;
                    this.Stop();
                    Thread.Sleep(50);
                    if (retries == maxRetries)
                    {
                        ShowSerialStartErrorMessage(uae);
                    }
                }
                catch (Exception e)
                {
                    ShowSerialStartErrorMessage(e);
                    retries = maxRetries;
                }
            }
        }

        public void ShowSerialStartErrorMessage(Exception e)
        {
            try
            {
                if (this.SerialPortParameter != null)
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to start the {0}.  Check Parameters for: {1}.", this.Name, this.SerialPortParameter.DisplayName),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                        e.Message);

                    MessageBox.Show(String.Format("Unable to start the {0}.  Check Parameters for: {1}.", this.Name, this.SerialPortParameter.DisplayName), Application.ProductName,
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch { }
        }

        /// <summary>
        /// Stops the <see cref="Process">Process</see> thread
        /// </summary>
        public void Stop()
        {
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Stopping the processing thread for {0}", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                stopProcessing = true;
                if (exitEvent != null)
                {
                    exitEvent.Set();
                }
                if (serialPort1.IsOpen)
                {
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                }
                //need to do this in separate thread
                Thread threadToClosePort = new Thread(new System.Threading.ThreadStart(Close));
                threadToClosePort.IsBackground = true;
                threadToClosePort.Start();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(String.Format("An error occurred stopping the {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());
                Dispose(true);
                GC.SuppressFinalize(this);
            }


            //if (thrd != null)
            //    thrd.Abort();
            //thrd = null;
        }

        /// <summary>
        /// Closes the serial port (called in seperate standalone thread).
        /// </summary>
        public void Close()
        {
            try
            {
                serialPort1.Close();
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Error stopping the processing thread for {0}: " + ex.Message, this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
            }
        }

        /// <summary>
        /// Opens the serial port
        /// </summary>
        public void Open()
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.Open();
            }
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Name of the serial port for this device (i.e. COM1, COM2, etc.)
        /// </summary>
        public string PortName
        {
            get { return serialPort1.PortName; }
            set { serialPort1.PortName = value; }
        }

        /// <summary>
        /// Baud rate for this device (i.e. 9600, 19200, etc.)
        /// </summary>
        public int BaudRate
        {
            get { return serialPort1.BaudRate; }
            set { serialPort1.BaudRate = value; }
        }

        /// <summary>
        /// Parity for this device
        /// </summary>
        public Parity Parity
        {
            get { return serialPort1.Parity; }
            set { serialPort1.Parity = value; }
        }

        /// <summary>
        /// Data bits for this device
        /// </summary>
        public int DataBits
        {
            get { return serialPort1.DataBits; }
            set { serialPort1.DataBits = value; }
        }

        /// <summary>
        /// Stop bits for this device
        /// </summary>
        public StopBits StopBits
        {
            get { return serialPort1.StopBits; }
            set { serialPort1.StopBits = value; }
        }

        /// <summary>
        /// Handshaking for this device
        /// </summary>
        public Handshake Handshake
        {
            get { return serialPort1.Handshake; }
            set { serialPort1.Handshake = value; }
        }

        /// <summary>
        /// Lock object to be used any time that serial I/O operations are performed
        /// </summary>
        public Object SerialLock
        {
            get { return serialLock; }
            set { serialLock = value; }
        }

        /// <summary>
        /// Returns true if the Serial I/O device is available
        /// </summary>
        public Boolean IsAvailable
        {
            get { return isAvailable; }
        }

        /// <summary>
        /// Serial Port contained within the control
        /// </summary>
        public SerialPort SerialPort
        {
            get { return serialPort1; }
            set { serialPort1 = value; }
        }

        /// <summary>
        /// BackgroundWorker thread for the control
        /// </summary>
        protected BackgroundWorker BackgroundWorker
        {
            get { return backgroundWorker1; }
        }

        /// <summary>
        /// Serial port parameter to use to configure the serial port
        /// </summary>
        /// <remarks>
        /// Any changes to the SerialPortParameter will cause the serial port to automatically be updated.
        /// </remarks>
        public SerialPortParameter SerialPortParameter
        {
            get { return serialPortParameter; }
            set
            {
                serialPortParameter = value;
                serialPortParameter.ProcessValueChanged += new EventHandler(serialPortParameter_ProcessValueChanged);
                serialPortParameter_ProcessValueChanged(this, null);
            }
        }

        private void serialPortParameter_ProcessValueChanged(object sender, EventArgs e)
        {
            if (serialPortParameter.ProcessValue != null &&
                !serialPortParameter.ProcessValue.Equals(serialPort1))
            {
                try
                {
                    if (serialPort1.IsOpen) serialPort1.Close();
                    serialPortParameter.ProcessValue.CopyTo(serialPort1);
                    serialPort1.Open();
                }
                catch
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to open serial port {0} for {1}", serialPort1.PortName, this.Name));
                }
            }
        }

        /// <summary>
        /// Gets the thread that processes the serial I/O
        /// </summary>
        public Thread ProcessThread
        {
            get { return thrd; }
        }

        #endregion Public Properties

        #region Abstract Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> of the Serial I/O device changes
        /// </summary>
        public abstract event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        abstract protected void OnValueChanged();

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> of the Serial I/O device changes
        /// </summary>
        public abstract event EventHandler RawValueChanged;

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        abstract protected void OnRawValueChanged();

        #endregion Abstract Event Handlers

        #region Abstract Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the Serial I/O device
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Method for processing events inside the <see cref="Process">Process</see> thread.
        /// This method runs outside of the <see cref="Process">Process</see> thread.
        /// </summary>
        public abstract void BackgroundProcess();

        #endregion Abstract Methods

        #region Abstract Properties

        /// <summary>
        /// Value of the Serial I/O device
        /// </summary>
        public abstract double Value { get; internal set; }

        /// <summary>
        /// Formatted Value of the Serial i/O device
        /// </summary>
        public abstract string FormattedValue { get; }

        /// <summary>
        /// Name of the Serial I/O device
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Raw Value of the Serial I/O device
        /// </summary>
        public abstract double RawValue { get; }

        /// <summary>
        /// Minimum value of the Serial I/O device
        /// </summary>
        public abstract double Min { get; }

        /// <summary>
        /// Maximum value of the Serial I/O device
        /// </summary>
        public abstract double Max { get; }

        /// <summary>
        /// Units for the value of the Serial I/O device
        /// </summary>
        public abstract string Units { get; set; }

        /// <summary>
        /// Format string for the value of the Serial I/O device
        /// </summary>
        public abstract string Format { get; set; }

        #endregion Abstract Properties
    }
}