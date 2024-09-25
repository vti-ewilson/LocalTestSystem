using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Implements an interface to a device that communicates via the Pfeiffer protocol
    /// </summary>
    public abstract partial class PfeifferInterface : Component, IFormattedAnalogInput
    {
        #region Enums

        /// <summary>
        /// Communication Action
        /// </summary>
        protected enum Action
        {
            /// <summary>
            /// Read Parameter
            /// </summary>
            Read = 0,

            /// <summary>
            /// Write Parameter
            /// </summary>
            Write = 10
        }

        #endregion Enums

        #region Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> changes
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        protected void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> changes
        /// </summary>
        public event EventHandler RawValueChanged;

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        protected void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        #endregion Event Handlers

        #region Globals

        private Thread thrd;
        private byte address = 1;
        private bool isAvailable;
        private SerialPortParameter serialPortParameter;
        private bool commError;
        private bool stopProcessing;
        private EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private int waitDelay = 100;
        private int retries = 0;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeifferInterface">PfeifferInterface</see>
        /// </summary>
        public PfeifferInterface()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfeifferInterface">PfeifferInterface</see>
        /// </summary>
        /// <param name="container">Container for this object</param>
        public PfeifferInterface(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }

        #endregion Construction

        #region Events

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BackgroundProcess();
        }

        #endregion Events

        #region Protected Methods

        /// <summary>
        /// Sends a message to the device and returns the resulting message
        /// </summary>
        /// <param name="Action">Read/Write Action</param>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Data">Data to be sent</param>
        /// <returns>Data returned from the device</returns>
    private static bool bErrorMsg = false;
        protected string SendMessage(Action Action, ushort ParameterNumber, string Data)
        {
            string message, response = String.Empty;
            message = String.Format("{0:000}{1:00}{2:000}{3:00}{4}", address, (ushort)Action, ParameterNumber, Data.Length, Data);
            message = String.Format("{0}{1:000}", message, message.ToCharArray().Sum(c => (int)c) % 256);
        try
        {
            lock (this.SerialPort)
            {
                Actions.Retry(3, delegate
                {
                    this.SerialPort.Write(message + "\r");
            response = this.SerialPort.ReadTo("\r");
                });
            }
        }
        catch (Exception et) 
        {
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    et.ToString());
                return "error";
        }
        Thread.Sleep(200);
            if (String.IsNullOrEmpty(response)) throw new Exception("No data returned");
            // Check the returned checksum
            if (response.Substring(0, response.Length - 3).ToCharArray().Sum(c => c) % 256 != int.Parse(response.Substring(response.Length - 3, 3)))
                throw new Exception("Checksum error in response");
            if (response.Length == 19 && response.Substring(10, 6).Equals("NO-DEF"))
                throw new Exception("Parameter does not exist");
            if (response.Length == 19 && response.Substring(10, 6).Equals("-RANGE"))
                throw new Exception("Transmitted data are outside the permitted range");
            if (response.Length == 19 && response.Substring(10, 6).Equals("-LOGIC"))
                throw new Exception("Logic error");
            return response;
        }

        /// <summary>
        /// Writes a boolean parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be set</param>
        /// <returns>Value returned after writing the value</returns>
        protected bool SetBooleanParameter(ushort ParameterNumber, bool Value)
        {
            try
            {
                return SendMessage(Action.Write, ParameterNumber, Value ? "111111" : "000000").Substring(10, 6).Equals("111111");
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Reads a boolean parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected bool GetBooleanParameter(ushort ParameterNumber)
        {
            try
            {
                return SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 6).Equals("111111");
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Writes a boolean parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be set</param>
        /// <returns>Value returned be the device</returns>
        protected bool SetBooleanNewParameter(ushort ParameterNumber, bool Value)
        {
            try
            {
                return SendMessage(Action.Write, ParameterNumber, Value ? "1" : "0").Substring(10, 1).Equals("1");
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Reads a boolean parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected bool GetBooleanNewParameter(ushort ParameterNumber)
        {
            try
            {
                return SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 1).Equals("1");
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Writes an integer parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be written</param>
        /// <returns>Value returned by the device</returns>
        protected uint SetIntegerParameter(ushort ParameterNumber, uint Value)
        {
            try
            {
                return uint.Parse(SendMessage(Action.Write, ParameterNumber, Value.ToString("000000")).Substring(10, 6));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Reads an integer parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected uint GetIntegerParameter(ushort ParameterNumber)
        {
            try
            {
                return uint.Parse(SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 6));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Writes an short integer parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be written</param>
        /// <returns>Value returned by the device</returns>
        protected ushort SetShortIntegerParameter(ushort ParameterNumber, ushort Value)
        {
            try
            {
                return ushort.Parse(SendMessage(Action.Write, ParameterNumber, Value.ToString("000")).Substring(10, 6));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Reads an short integer parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected ushort GetShortIntegerParameter(ushort ParameterNumber)
        {
            try
            {
                return ushort.Parse(SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 3));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Writes an floating-point parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be written</param>
        /// <returns>Value returned by the device</returns>
        protected Single SetSingleParameter(ushort ParameterNumber, Single Value)
        {
            try
            {
                return Single.Parse(SendMessage(Action.Write, ParameterNumber, (Value * 100).ToString("000000")).Substring(10, 6)) / 100;
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Reads an floating-point parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected Single GetSingleParameter(ushort ParameterNumber)
        {
            try
            {
                return Single.Parse(SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 6)) / 100;
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Writes an floating-point parameter in exponential format
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be written</param>
        /// <returns>Value returned by the device</returns>
        protected Single SetExpoParameter(ushort ParameterNumber, Single Value)
        {
            try
            {
                return Single.Parse(SendMessage(Action.Write, ParameterNumber, Value.ToString("0.0E-0").PadLeft(6, '0')).Substring(10, 6));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Reads an floating-point parameter in exponential format
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected Single GetExpoParameter(ushort ParameterNumber)
        {
            try
            {
                return Single.Parse(SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 6));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Writes an floating-point parameter in exponential format
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be written</param>
        /// <returns>Value returned by the device</returns>
        protected Single SetExpoNewParameter(ushort ParameterNumber, Single Value)
        {
            try
            {
                string s = Value.ToString("0.000E-00");
                int mantissa = (int)(Single.Parse(s.Substring(0, 5)) * 1000F);
                int exponent = Int32.Parse(s.Substring(7)) + 20;
                string ret = SendMessage(Action.Write, ParameterNumber, String.Format("{0:0000}{1:00}", mantissa, exponent)).Substring(10, 6);
                return Single.Parse(String.Format("{0}E{1}", Single.Parse(ret.Substring(0, 4)) / 1000F, Single.Parse(ret.Substring(4, 2)) - 20));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Reads an floating-point parameter in exponential format
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected Single GetExpoNewParameter(ushort ParameterNumber)
        {
            try
            {
                string ret = SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 6);
                return Single.Parse(String.Format("{0}E{1}", Single.Parse(ret.Substring(0, 4)) / 1000F, Single.Parse(ret.Substring(4, 2)) - 20));
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Writes a 6-character string parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be written</param>
        /// <returns>Value returned by the device</returns>
        protected String SetStringParameter(ushort ParameterNumber, String Value)
        {
            try
            {
                return SendMessage(Action.Write, ParameterNumber, Value.PadLeft(6)).Substring(10, 6);
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return String.Empty;
            }
        }

        /// <summary>
        /// Reads a 6-character string parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected String GetStringParameter(ushort ParameterNumber)
        {
            try
            {
                return SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 6);
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return String.Empty;
            }
        }

        /// <summary>
        /// Writes a 16-character string parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <param name="Value">Value to be written</param>
        /// <returns>Value returned by the device</returns>
        protected String SetString16Parameter(ushort ParameterNumber, String Value)
        {
            try
            {
                return SendMessage(Action.Write, ParameterNumber, Value.PadLeft(16)).Substring(10, 16);
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return String.Empty;
            }
        }

        /// <summary>
        /// Reads a 16-character string parameter
        /// </summary>
        /// <param name="ParameterNumber">Parameter Number</param>
        /// <returns>Value returned by the device</returns>
        protected String GetString16Parameter(ushort ParameterNumber)
        {
            try
            {
                return SendMessage(Action.Read, ParameterNumber, "=?").Substring(10, 16);
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError(
                    String.Format("Error communicating with {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    e.ToString());
                return String.Empty;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void ProcessThrd()
        {
            while (!stopProcessing)
            {
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
                            return;
                        }
                        else commError = false;
                    }
                    else retries = 0;
                    if (this.IsAvailable)
                        this.Process();
                    exitEvent.WaitOne(waitDelay, true);
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError(
                        String.Format("An error has occurred in the thread processing the {0}", this.Name),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        e.ToString());
                }
            }
            isAvailable = false;
            //serialPort1.Close();
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
            if (this.SerialPort != null)
            {
                try
                {
                    VtiEvent.Log.WriteVerbose(
                        String.Format("Starting the processing thread for {0}", this.Name),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                    this.SerialPort.ReadTimeout = 500;
                    this.SerialPort.WriteTimeout = 500;
                    if (!this.SerialPort.IsOpen) this.SerialPort.Open();
                    if (this.SerialPort.IsOpen)
                    {
                        commError = false;
                        stopProcessing = false;
                        thrd = new Thread(new ThreadStart(ProcessThrd));
                        thrd.Start();
                        //thrd.IsBackground = true;
                        thrd.Name = this.Name; // "Serial IO";
                        isAvailable = true;
                    }
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to start the {0}.", this.Name),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                        e.ToString());
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="Process">Process</see> thread
        /// </summary>
        public void Stop()
        {
            //if (thrd != null)
            //    thrd.Abort();
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Stopping the processing thread for {0}", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                stopProcessing = true;
                if (exitEvent != null)
                    exitEvent.Set();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(String.Format("An error occurred stopping the {0}.", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());
            }
        }

        /// <summary>
        /// Resets the <see cref="CommError">CommError</see> flag
        /// </summary>
        public void ResetCommError()
        {
            commError = false;
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// <para>Serial Port for the Pfeiffer Interface</para>
        /// <para>More than one device can share a serial port using RS-485.</para>
        /// </summary>
        public SerialPort SerialPort { get; set; }

        /// <summary>
        /// Attaches the device to a SerialPortParameter in the Config.cs class so that
        /// any changes to the SerialPortParameter are automatically changed for the device
        /// </summary>
        public SerialPortParameter SerialPortParameter
        {
            get { return serialPortParameter; }
            set
            {
                if (SerialPort == null) SerialPort = new SerialPort();
                serialPortParameter = value;
                serialPortParameter.ProcessValueChanged += new EventHandler(serialPortParameter_ProcessValueChanged);
                serialPortParameter_ProcessValueChanged(this, null);
            }
        }

        private void serialPortParameter_ProcessValueChanged(object sender, EventArgs e)
        {
            if (!serialPortParameter.ProcessValue.Equals(SerialPort))
            {
                try
                {
                    if (SerialPort.IsOpen) SerialPort.Close();
                    serialPortParameter.ProcessValue.CopyTo(SerialPort);
                    SerialPort.Open();
                }
                catch
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to open serial port {0} for {1}", SerialPort.PortName, this.Name));
                }
            }
        }

        /// <summary>
        /// RS-485 Address for the device
        /// </summary>
        public byte Address
        {
            get { return address; }
            set { address = value; }
        }

        /// <summary>
        /// Indicates if the <see cref="SerialPort">SerialPort</see> is available
        /// </summary>
        public bool IsAvailable
        {
            get { return isAvailable; }
        }

        /// <summary>
        /// Gets a value to indicate if a communications error has occurred.
        /// </summary>
        public bool CommError
        {
            get { return commError; }
        }

        /// <summary>
        /// Gets the thread that processes the serial I/O for the Pfeiffer Interface.
        /// </summary>
        public Thread ProcessThread
        {
            get { return thrd; }
        }

        #endregion Public Properties

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
        public abstract double Min { get; set; }

        /// <summary>
        /// Maximum value of the Serial I/O device
        /// </summary>
        public abstract double Max { get; set; }

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