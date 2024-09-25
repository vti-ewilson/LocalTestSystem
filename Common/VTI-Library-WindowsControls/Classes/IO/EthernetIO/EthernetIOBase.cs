using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.EthernetIO
{
    /// <summary>
    /// Base class for Ethernet I/O devices
    /// </summary>
    /// <remarks>
    /// This base class takes care of many of the Ethernet Port properties,
    /// making development of Ethernet I/O device interfaces simpler.
    /// </remarks>
    public abstract partial class EthernetIOBase : Component, IFormattedAnalogInput
    {
        #region Globals

        private Thread thrd;
        private Object ethernetLock = new Object();
        private Boolean isAvailable = false;
        private EthernetPortParameter ethernetPortParameter;
        private EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private int waitDelay = 100;
        private int retries = 0;
        //private EthernetPort ethernetPort1 = new EthernetPort();

        //private class EthernetPort
        //{
        //    public string PortName;
        //    public string IPAddress;
        //    public string Port;
        //}

        /// <summary>
        /// Value to indicate that a communications error has occurred.
        /// </summary>
        protected Boolean commError = false;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EthernetIOBase">EthernetIOBase</see> class
        /// </summary>
        public EthernetIOBase()
        {
            InitializeComponent();
            System.Net.IPAddress ip = IPAddress.Parse("192.168.0.11");
            ethernetPort1 = new System.Net.IPEndPoint(ip, 502);

            //backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthernetIOBase">EthernetIOBase</see> class
        /// </summary>
        public EthernetIOBase(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            //backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }

        #endregion Construction

        #region Events

        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //this.BackgroundProcess();
        //}

        #endregion Events

        #region Private Methods

        //private void ProcessThrd()
        //{
        //    //while (!stopProcessing)
        //    //{
        //    //    try
        //    //    {
        //    //        if (commError)
        //    //        {
        //    //            if (++retries > 5)
        //    //            {
        //    //                VtiEvent.Log.WriteError(
        //    //                        String.Format("An error has occurred communicating with {0}", this.Name),
        //    //                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
        //    //                this.Stop();
        //    //                isAvailable = false;
        //    //                this.Value = Double.NaN;
        //    //                return;
        //    //            }
        //    //            else commError = false;
        //    //        }
        //    //        else retries = 0;
        //    //        if (this.IsAvailable)
        //    //        {
        //    //            this.Process();
        //    //        }
        //    //        //exitEvent.WaitOne(waitDelay, true);
        //    //        //changed to Thread.Sleep() to drastically improve CPU usage
        //    //        Thread.Sleep(waitDelay);
        //    //    }
        //    //    catch (Exception e)
        //    //    {
        //    //        VtiEvent.Log.WriteError(
        //    //            String.Format("An error has occurred in the thread processing the {0}", this.Name),
        //    //            VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO,
        //    //            e.ToString());
        //    //    }
        //    //}
        //    //isAvailable = false;
        //    //this.Value = Double.NaN;
        //}

        #endregion Private Methods

        #region Public Methods

        ///// <summary>
        ///// Opens the ethernet port and starts the
        ///// <see cref="Process">Process</see> thread
        ///// </summary>
        //public void Start()
        //{
        //    try
        //    {
        //        VtiEvent.Log.WriteVerbose(
        //            String.Format("Starting the processing thread for {0}", "KeithleyIO"),
        //            VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);

        //        backgroundWorker.WorkerReportsProgress = true;
        //        backgroundWorker.DoWork += backgroundWorker_DoWork;

        //        Connect();

        //        backgroundWorker.RunWorkerAsync();

        //        stopProcessing = false;
        //        //thrd = new Thread(new ThreadStart(ProcessThrd));
        //        //thrd.Start();
        //        //thrd.IsBackground = true;
        //        //thrd.Name = "KeithleyIO";
        //        isAvailable = true;
        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        try
        //        {
        //            VtiEvent.Log.WriteWarning(String.Format("Unable to start the TorrconIV."),
        //                VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
        //                e.ToString());
        //        }
        //        catch { }
        //    }
        //}

        //public void ShowEthernetStartErrorMessage(Exception e)
        //{
        //    try
        //    {
        //        //if (this.EthernetPortParameter != null)
        //        //{
        //        //    VtiEvent.Log.WriteInfo(String.Format("Unable to start the {0}.  Check Parameters for: {1}.", this.Name, this.EthernetPortParameter.DisplayName),
        //        //        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
        //        //        e.Message);

        //        //    MessageBox.Show(String.Format("Unable to start the {0}.  Check Parameters for: {1}.", this.Name, this.EthernetPortParameter.DisplayName), Application.ProductName,
        //        //            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        //}
        //    }
        //    catch { }
        //}

        ///// <summary>
        ///// Stops the <see cref="Process">Process</see> thread
        ///// </summary>
        //public void Stop()
        //{
        //    //try
        //    //{
        //    //    VtiEvent.Log.WriteVerbose(
        //    //        String.Format("Stopping the processing thread for {0}", this.Name),
        //    //        VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
        //    //    stopProcessing = true;
        //    //    if (exitEvent != null)
        //    //    {
        //    //        exitEvent.Set();
        //    //    }
        //    //    ethernetPort1.DiscardInBuffer();
        //    //    ethernetPort1.DiscardOutBuffer();
        //    //    //need to do this in separate thread
        //    //    Thread threadToClosePort = new Thread(new System.Threading.ThreadStart(Close));
        //    //    threadToClosePort.IsBackground = true;
        //    //    threadToClosePort.Start();
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    VtiEvent.Log.WriteWarning(String.Format("An error occurred stopping the {0}.", this.Name),
        //    //        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
        //    //        e.ToString());
        //    //    Dispose(true);
        //    //    GC.SuppressFinalize(this);
        //    //}
        //}

        ///// <summary>
        ///// Closes the ethernet port (called in seperate standalone thread).
        ///// </summary>
        //public void Close()
        //{
        //    //try
        //    //{
        //    //    ethernetPort1.Close();
        //    //    Dispose(true);
        //    //    GC.SuppressFinalize(this);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    VtiEvent.Log.WriteVerbose(
        //    //        String.Format("Error stopping the processing thread for {0}: " + ex.Message, this.Name),
        //    //        VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
        //    //}
        //}

        ///// <summary>
        ///// Opens the ethernet port
        ///// </summary>
        //public void Open()
        //{
        //    //if (!ethernetPort1.IsOpen)
        //    //{
        //    //    ethernetPort1.Open();
        //    //}
        //}

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Ethernet port parameter to use to configure the ethernet port
        /// </summary>
        /// <remarks>
        /// Any changes to the EthernetPortParameter will cause the ethernet port to automatically be updated.
        /// </remarks>
        public EthernetPortParameter EthernetPortParameter
        {
            get { return ethernetPortParameter; }
            set
            {
                ethernetPortParameter = value;
                ethernetPortParameter.ProcessValueChanged += new EventHandler(ethernetPortParameter_ProcessValueChanged);
                ethernetPortParameter_ProcessValueChanged(this, null);
            }
        }

        private void ethernetPortParameter_ProcessValueChanged(object sender, EventArgs e)
        {
            //IPAddress ip;
            //ushort prt;
            //if (IPAddress.TryParse(ethernetPortParameter.ProcessValue.IPAddress, out ip) && ushort.TryParse(ethernetPortParameter.ProcessValue.Port, out prt))
            //{
            //    IP_Address = ip;
            //    Port = prt;
            //    if (!StopProcessing)
            //    {
            //        while (!UpdateIPOnDevice())
            //        {

            //        }
            //        //Stop();
            //        //Start();
            //    }
            //}
            
            //if (ethernetPortParameter.ProcessValue != null &&
            //    !ethernetPortParameter.ProcessValue.Equals(ethernetPort1))
            //{
            //    try
            //    {
            //        ethernetPortParameter.ProcessValue.CopyTo(ethernetPort1);
            //    }
            //    catch
            //    {
            //        VtiEvent.Log.WriteWarning(String.Format("Unable to open ethernet port for {0}", EthernetPortParameter.DisplayName));
            //    }
            //}
        }

        //public string PortName
        //{
        //    get { return ethernetPort1.PortName; }
        //    set { ethernetPort1.PortName = value; }
        //}

        public int Port
        {
            get { return ethernetPort1.Port; }
            set { ethernetPort1.Port = value; }
        }

        public System.Net.IPAddress IP_Address
        {
            get { return ethernetPort1.Address; }
            set { ethernetPort1.Address = value; }
        }

        ///// <summary>
        ///// Lock object to be used any time that Ethernet I/O operations are performed
        ///// </summary>
        //public Object EthernetLock
        //{
        //    get { return ethernetLock; }
        //    set { ethernetLock = value; }
        //}

        /// <summary>
        /// Returns true if the Ethernet I/O device is available
        /// </summary>
        public Boolean IsAvailable
        {
            get { return isAvailable; }
        }

        ///// <summary>
        ///// Ethernet Port contained within the control
        ///// </summary>
        //public EthernetPort EthernetPort
        //{
        //    get { return ethernetPort1; }
        //    set { ethernetPort1 = value; }
        //}

        ///// <summary>
        ///// BackgroundWorker thread for the control
        ///// </summary>
        //protected BackgroundWorker BackgroundWorker
        //{
        //    get { return backgroundWorker1; }
        //}

        ///// <summary>
        ///// Gets the thread that processes the Ethernet I/O
        ///// </summary>
        //public Thread ProcessThread
        //{
        //    get { return thrd; }
        //}

        #endregion Public Properties

        #region Abstract Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> of the Ethernet I/O device changes
        /// </summary>
        public abstract event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        abstract protected void OnValueChanged();

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> of the Ethernet I/O device changes
        /// </summary>
        public abstract event EventHandler RawValueChanged;

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        abstract protected void OnRawValueChanged();

        #endregion Abstract Event Handlers

        #region Abstract Methods

        //public abstract bool UpdateIPOnDevice();
        public abstract void Stop();
        public abstract void Start();

        ///// <summary>
        ///// Thread for reading the <see cref="Value">Value</see> of the Ethernet I/O device
        ///// </summary>
        //public abstract void Process();

        ///// <summary>
        ///// Method for processing events inside the <see cref="Process">Process</see> thread.
        ///// This method runs outside of the <see cref="Process">Process</see> thread.
        ///// </summary>
        //public abstract void BackgroundProcess();

        #endregion Abstract Methods

        #region Abstract Properties

        //public abstract bool StopProcessing { get; }

        /// <summary>
        /// Value of the Ethernet I/O device
        /// </summary>
        public abstract double Value { get; internal set; }

        /// <summary>
        /// Formatted Value of the Ethernet I/O device
        /// </summary>
        public abstract string FormattedValue { get; }

        /// <summary>
        /// Name of the Ethernet I/O device
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Raw Value of the Ethernet I/O device
        /// </summary>
        public abstract double RawValue { get; }

        /// <summary>
        /// Minimum value of the Ethernet I/O device
        /// </summary>
        public abstract double Min { get; }

        /// <summary>
        /// Maximum value of the Ethernet I/O device
        /// </summary>
        public abstract double Max { get; }

        /// <summary>
        /// Units for the value of the Ethernet I/O device
        /// </summary>
        public abstract string Units { get; set; }

        /// <summary>
        /// Format string for the value of the Ethernet I/O device
        /// </summary>
        public abstract string Format { get; set; }

        #endregion Abstract Properties
    }
}