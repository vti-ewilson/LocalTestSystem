using System;
using System.ComponentModel;
using System.Threading;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Base class for RS-485 I/O devices
    /// </summary>
    public abstract partial class RS485IOBase : Component, IFormattedAnalogInput
    {
        #region Globals

        private Thread thrd;
        private RS485ModbusInterface modbusInterface;
        private byte slaveID = 1;
        private Boolean isAvailable = false;
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
        /// Initializes a new instance of the <see cref="RS485IOBase">RS485IOBase</see> class
        /// </summary>
        public RS485IOBase()
        {
            InitializeComponent();

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RS485IOBase">RS485IOBase</see> class
        /// </summary>
        public RS485IOBase(IContainer container)
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
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Starting the processing thread for {0}", this.Name),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                this.modbusInterface.SerialPort.ReadTimeout = 500;
                this.modbusInterface.SerialPort.WriteTimeout = 500;
                if (!this.modbusInterface.SerialPort.IsOpen)
                    this.modbusInterface.SerialPort.Open();
                if (this.modbusInterface.SerialPort.IsOpen)
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

        /// <summary>
        /// Stops the <see cref="Process">Process</see> thread
        /// </summary>
        public void Stop()
        {
            VtiEvent.Log.WriteVerbose(
                String.Format("Stopping the processing thread for {0}", this.Name),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
            stopProcessing = true;
            if (exitEvent != null)
                exitEvent.Set();

            //isAvailable = false;
            //this.Value = Single.NaN;
            //if (thrd != null)
            //    thrd.Abort();
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Slave ID Address for the device
        /// </summary>
        public byte SlaveID
        {
            get { return slaveID; }
            set { slaveID = value; }
        }

        /// <summary>
        /// RS-485 Modbus Interface for the device
        /// </summary>
        public RS485ModbusInterface ModbusInterface
        {
            get { return modbusInterface; }
            set { modbusInterface = value; }
        }

        /// <summary>
        /// Returns true if the RS-485 Modbus Interface is available
        /// </summary>
        public Boolean IsAvailable
        {
            get { return isAvailable; }
        }

        /// <summary>
        /// Returns the BackgroundWorker object
        /// </summary>
        protected BackgroundWorker BackgroundWorker
        {
            get { return backgroundWorker1; }
        }

        /// <summary>
        /// Gets the thread that reads the process value from the serial device.
        /// </summary>
        public Thread ProcessThread
        {
            get { return thrd; }
        }

        #endregion Public Properties

        #region Abstract Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> of the RS-485 I/O device changes
        /// </summary>
        public abstract event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        abstract protected void OnValueChanged();

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> of the RS-485 I/O device changes
        /// </summary>
        public abstract event EventHandler RawValueChanged;

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        abstract protected void OnRawValueChanged();

        #endregion Abstract Event Handlers

        #region Abstract Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the RS-485 I/O device
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
        /// Formatted Value of the RS-485 I/O device
        /// </summary>
        public abstract string FormattedValue { get; }

        /// <summary>
        /// Name of the RS-485 I/O device
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Raw Value of the RS-485 I/O device
        /// </summary>
        public abstract double RawValue { get; }

        /// <summary>
        /// Minimum value of the RS-485 I/O device
        /// </summary>
        public abstract double Min { get; }

        /// <summary>
        /// Maximum value of the RS-485 I/O device
        /// </summary>
        public abstract double Max { get; }

        /// <summary>
        /// Units for the value of the RS-485 I/O device
        /// </summary>
        public abstract string Units { get; }

        /// <summary>
        /// Format string for the value of the RS-485 I/O device
        /// </summary>
        public abstract string Format { get; set; }

        #endregion Abstract Properties
    }
}