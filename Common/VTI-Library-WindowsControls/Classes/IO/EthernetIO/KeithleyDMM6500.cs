using System;
using System.Diagnostics;
using System.Net;

//using VTIWindowsControlLibrary.Classes.IO.EthernetIO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.EthernetIO
{
    /// <summary>
    /// Serial Interface for a Controller
    /// </summary>
    public class KeithleyDMM6500 : EthernetIOBase
    {
        private Thread thrd;
        private Object serialLock = new Object();
        private Boolean isAvailable = false;

        //private /*SerialPortParameter serialPortParameter*/;
        private bool _stopProcessing = true;

        private EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private int waitDelay = 100;
        private int retries = 0;

        private Socket clientSocket;
        private byte[] buffer;

        /// <summary>
        /// Value to indicate that a communications error has occurred.
        /// </summary>
        protected Boolean commError = false;

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

        #region Private Vars

        private Boolean _DoNotCall;
        private string ParseValue;
        private IPAddress _ipaddressOfDevice;

        private Double _signal;
        private Single _min, _max;
        private String _units;
        private String _format = "0.00000E+00";
        private String[] SignalUnitNames = { "VDC", "Amps" };
        private AnalogSignal DMMSignal;

        private Boolean _flagReadCurrent = false;
        private Boolean _flagReturntoContinuousReadMode = false;
        private Stopwatch _errorSW = new Stopwatch();

        #endregion Private Vars

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="KeithleyDMM6500">KeithleyDMM6500</see> class
        /// </summary>
        public KeithleyDMM6500(Single Min, Single Max, String Units)
        {
            _min = Min;
            _max = Max;
            _units = Units;

            //this.EthernetPort.ReadTimeout = 100000;
            DMMSignal = new AnalogSignal("DMM Signal", "VDC", "0.000000E+00", 1000, false, true);
            Format = "0.00000E+00";
        }

        #endregion Construction

        /// <summary>
        /// Opens the port and starts the
        /// <see cref="Process">Process</see> thread
        /// </summary>
        public override void Start()
        {
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Starting the processing thread for {0}", "KeithleyIO"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);

                Connect();

                _stopProcessing = false;
                thrd = new Thread(new ThreadStart(ProcessThrd));
                thrd.Start();
                thrd.IsBackground = true;
                thrd.Name = "KeithleyIO";
                isAvailable = true;
                //}
            }
            catch (Exception e)
            {
                try
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to start the Keithley"),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                        e.ToString());

                    //MessageBox.Show(String.Format("Unable to start the {0}.  Check Parameters for: {1}", this.Name, this.SerialPortParameter.DisplayName), Application.ProductName,
                    //        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch { }
            }
        }

        /// <summary>
        /// Returns true if the device is available
        /// </summary>
        public Boolean IsAvailable
        {
            get { return isAvailable; }
        }

        private void ProcessThrd()
        {
            while (!_stopProcessing)
            {
                try
                {
                    if (commError)
                    {
                        if (++retries > 5)
                        {
                            VtiEvent.Log.WriteError(
                                    String.Format("An error has occurred communicating with Keithley"),
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
                        String.Format("An error has occurred in the thread processing the Keithley"),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        e.ToString());
                }
            }
            isAvailable = false;
            //serialPort1.Close();
            this.Value = Double.NaN;
        }

        //public override bool UpdateIPOnDevice()
        //{
        //    return true;
        //}

        // <summary>
        /// Stops the <see cref="Process">Process</see> thread
        /// </summary>
        public override void Stop()
        {
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Stopping the processing thread for Keithley"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                _stopProcessing = true;
                if (exitEvent != null)
                    exitEvent.Set();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(String.Format("An error occurred stopping the Keithley"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());
            }
            //if (thrd != null)
            //    thrd.Abort();
            //thrd = null;
        }

        #region Private Methods

        private void Connect()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Connect to the specified host.
                var endPoint = new IPEndPoint(_ipaddressOfDevice, 5025);
                clientSocket.BeginConnect(endPoint, ConnectCallback, null);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndConnect(AR);
                // Update flag connected .... Must create

                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = clientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }

                string message = Encoding.ASCII.GetString(buffer);
                bool DidIt = Double.TryParse(message.Trim(), out double result);
                if (DidIt) { Value = result; } else Value = -100;

                // Start receiving data again.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            // Avoid Pokemon exception handling in cases like these.
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Processes commands to the Controller
        /// </summary>
        private string ProcessCommandAndReturnValue(string strCommand)
        {
            // Send command
            _DoNotCall = true;

            try
            {
                string command = strCommand + "\n";
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                // Send the command

                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
            }
            catch (SocketException ex)
            {
                VtiEvent.Log.WriteError("Error inside DMM ProcessCommandAndReturnValue, Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, ex.Message);
                _DoNotCall = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //UpdateControlStates(false);
                return "-1"; // write error
            }
            catch (ObjectDisposedException ex)
            {
                VtiEvent.Log.WriteError("Error inside DMM ProcessCommandAndReturnValue, Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, ex.Message);
                _DoNotCall = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //UpdateControlStates(false);
                return "-1"; // write error
            }
            // Read back response
            //strRead = strRead.Replace("\r", "");
            //strRead = strRead.Replace("\n", "");
            //strRead = strRead.Replace(">", "");
            //strRead = strRead.Trim();
            _DoNotCall = false;
            return "ok"; // no error
        }

        /// <summary>
        ///  read the baratron pressure
        /// </summary>
        public double ReadSignal()
        {
            string strDummy = "", strCMD = "";
            //strCMD = "@020";
            //try
            //{
            //    strDummy = ProcessCommandAndReturnValue(strCMD + "?");

            //    Double PreviousRead = this._signal;
            //    float parseResult;

            //    if (strDummy.Contains(strCMD))
            //    {
            //        int nCRLocation;
            //        nCRLocation = strDummy.IndexOf("\r");
            //        if (nCRLocation > 0)
            //            strDummy = strDummy.Substring(0, nCRLocation);

            //        int nOverRange = strDummy.IndexOf("#");
            //        if (nOverRange > 0)
            //        { // Set overrange flag
            //            if (strDummy.Substring(0, 5) == "@020#")
            //                strDummy = strDummy.Substring(5);
            //            if (float.TryParse(strDummy, out parseResult))
            //                return parseResult;
            //            else { return _max; }
            //        }
            //        else
            //        { // Parse Value
            //            if (strDummy.Substring(0, 4) == "@020")
            //                strDummy = strDummy.Substring(5);
            //            if (float.TryParse(strDummy, out parseResult))
            //                return parseResult;
            //            else
            //            { return _max; }
            //        }
            //    }
            //    else
            { return _max; }

            //}
            //catch (Exception e)
            //{
            //    VtiEvent.Log.WriteError("Error processing command in Read Signal", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            //}
            //return _max;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the Controller
        /// </summary>
        public void Process()
        {
            try
            {
                //serialPort1.DiscardInBuffer();
                Thread.Sleep(300);

                ParseValue = ProcessCommandAndReturnValue(":READ?");

                _signal = Value;

                if (_flagReadCurrent)
                {
                }

                if (_flagReturntoContinuousReadMode)
                {
                    //SetReadMode
                }
            }
            catch (Exception e)
            {
                DMMSignal.Value = Double.NaN;
                //commError = true;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Sets a flag to start the current read, false if voltage
        /// </summary>
        public bool StartReadCurrent { set { _flagReadCurrent = true; } }

        /// <summary>
        /// Returns the DMM to continuous read mode
        /// </summary>
        public bool ReturnDMMToContinuousReadMode { set { _flagReturntoContinuousReadMode = true; } }

        public enum DMMResponseCodes
        {
            //Unknown = -1,
            //CommandAccepted = 0,
            //ParameterValueUnrecognized = 1,
            //DataFieldValueInvalid = 2,
            //CommandIsInappropriate = 3
        }

        /// <summary>
        /// set the read mode  ' Code incomplete
        /// </summary>
        public int SetReadMode()
        {
            //try {
            //  int status, CommIn;
            //  string strDummy;
            //  CommIn = 0;
            //  while (true) {
            //    strDummy = "@0100" + "\r";
            //    //if (status == -1 || status == 3)
            //    //  break;
            //    if (CommIn > 3)
            //      break;
            //    CommIn++;
            //  }
            //  if (status == 0) {
            //    return (int)MKSResponse.Unknown; // error occurred inside SendCommand
            //  }
            //  Thread.Sleep(50);
            //  // read back the acknowledge
            //  CommIn = 0;
            //  strDummy = "";
            //  while (true) {
            //    //strDummy = WaitForAcknowledge(1, 5, 2);
            //    if (strDummy != "")
            //      break;
            //    if (CommIn > 3)
            //      break;
            //    CommIn++;
            //  }
            //  if (strDummy != "") {
            //    string strPrefix = strDummy.Substring(0, 1);
            //    if (strPrefix == "@")
            //      return (int)MKSResponse.CommandAccepted;
            //    else if (strPrefix == ">")
            //      return (int)MKSResponse.ParameterValueUnrecognized;
            //    else if (strPrefix == "?")
            //      return (int)MKSResponse.DataFieldValueInvalid;
            //    else if (strPrefix == "=")
            //      return (int)MKSResponse.CommandIsInappropriate;
            //    else
            //      return (int)MKSResponse.Unknown;
            //  }
            //}
            //catch (Exception e) {
            //  VtiEvent.Log.WriteError("Error inside SetReadMode", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            //}
            return -1;
        }

        #endregion Public Methods

        #region Events

        /// <summary>
        /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public void BackgroundProcess()
        {
            OnValueChanged();
        }

        #endregion Events

        #region Public Properties

        //public override bool StopProcessing
        //{
        //    get { return _stopProcessing; }
        //}

        public IPAddress IPAddressOfDevice
        {
            get { return _ipaddressOfDevice; }
            set { _ipaddressOfDevice = value; }
        }

        /// <summary>
        /// Value (pressure) of the Controller
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
                    return this.Value.ToString(_format) + " " + _units;
            }
        }

        /// <summary>
        /// Units for the value for the controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the controller
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

        /// <summary>
        /// Name for the Controller
        /// </summary>
        public override string Name
        {
            get { return "Keithley DMM"; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        #endregion Public Properties
    }
}