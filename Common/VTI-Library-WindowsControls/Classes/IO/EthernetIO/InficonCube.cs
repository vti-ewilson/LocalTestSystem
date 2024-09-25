using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.EthernetIO
{
    /// <summary>
    /// Interface for a Controller
    /// </summary>
    public class InficonCube : IFormattedAnalogInput
    {
        private Thread thrd;

        //private Object serialLock = new Object();
        private Boolean isAvailable = false;

        //private /*SerialPortParameter serialPortParameter*/;
        private Boolean stopProcessing;

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

        #region Private Vars

        private Boolean _DoNotCall;
        private string ParseValue;
        private IPAddress _ipaddressOfDevice;

        private Double _signal;
        private Single _min, _max;
        private String _units;
        private String _format = "0.00000E+00";
        private String[] SignalUnitNames = { "VDC", "Amps" };
        private AnalogSignal CDGSignal;

        private Boolean _ZeroCDG = false;
        private Stopwatch _errorSW = new Stopwatch();
        private string _Name = "InficonCube";
        private string _errorNum = "0";

        #endregion Private Vars

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonCube">InficonCube</see> class
        /// </summary>
        public InficonCube(Single Min, Single Max, String Units)
        {
            _min = Min;
            _max = Max;
            _units = Units;

            CDGSignal = new AnalogSignal("Cube Signal", "VDC", "0.000000E+00", 1000, false, true);
            Format = "0.00000E+00";
        }

        #endregion Construction

        /// <summary>
        /// Opens the port and starts the
        /// <see cref="Process">Process</see> thread
        /// </summary>
        public void Start()
        {
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Starting the processing thread for {0}", "InficonCube"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);

                Connect();

                stopProcessing = false;
                thrd = new Thread(new ThreadStart(ProcessThrd));
                thrd.Start();
                thrd.IsBackground = true;
                thrd.Name = "InficonCube";
                isAvailable = true;
                //}
            }
            catch (Exception e)
            {
                try
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to start the InficonCube"),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                        e.ToString());

                    MessageBox.Show(String.Format("Unable to start the {0}.  Check Parameters for: {1}", this.Name), Application.ProductName,
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
            while (!stopProcessing)
            {
                try
                {
                    if (commError)
                    {
                        if (++retries > 5)
                        {
                            VtiEvent.Log.WriteError(
                                    String.Format("An error has occurred communicating with InficonCube"),
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
                        String.Format("An error has occurred in the thread processing the InficonCube"),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        e.ToString());
                }
            }
            isAvailable = false;
            //serialPort1.Close();
            this.Value = Double.NaN;
        }

        // <summary>
        /// Stops the <see cref="Process">Process</see> thread
        /// </summary>
        public void Stop()
        {
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Stopping the processing thread for InficonCube"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                stopProcessing = true;
                if (exitEvent != null)
                    exitEvent.Set();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(String.Format("An error occurred stopping the InficonCube"),
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
                var endPoint = new IPEndPoint(_ipaddressOfDevice, 8087);
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

        //private void SendCallback(IAsyncResult AR)
        //{
        //    try
        //    {
        //        clientSocket.EndSend(AR);
        //    }
        //    catch (SocketException ex)
        //    {
        //        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (ObjectDisposedException ex)
        //    {
        //        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        /// <summary>
        /// Processes commands to the Controller
        /// </summary>
        private string ProcessCommandAndReturnValue(string strCommand)
        {
            // Send command
            string message = "";
            _DoNotCall = true;
            try
            {
                var client = new RestClient("http://" + _ipaddressOfDevice + ":8087/1/cmd/");

                // Create a request
                var request = new RestRequest(strCommand);

                // Send GET request
                var response = client.Get(request);
                message = response.Content;

                // Check for errors in the response
                if (response.ErrorException != null || response.ResponseStatus == RestSharp.ResponseStatus.Error)
                {
                    message = "Error retrieving response. " + response.Content + ": Rest Status Code = " + response.StatusCode.ToString();

                    var RestException = new ApplicationException(message, response.ErrorException);
                    throw RestException;
                }

                // Send another request to check extended error
                request = new RestRequest("EXE");
                response = client.Get(request);

                if (response.Content != "0")
                {
                    _errorNum = response.Content;
                }
                else
                {
                    _errorNum = "0";
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _DoNotCall = false;
            }
            return message;
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

                if (!_DoNotCall) ParseValue = ProcessCommandAndReturnValue("PRE");
                if (Double.TryParse(ParseValue, out double result))
                {
                    _signal = Value = result;
                }
                else
                {
                    _signal = 1000;
                    VtiEvent.Log.WriteError(String.Format(ParseValue + " {0}", this.Name), VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO, _signal);
                }

                if (_ZeroCDG)
                {
                    try
                    {
                        if (!_DoNotCall) ParseValue = ProcessCommandAndReturnValue("PRE");
                    }
                    catch (Exception e)
                    {
                        VtiEvent.Log.WriteError(String.Format("Error processing ZAD Zero command to Inficon CDG. {0}", this.Name), VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO, e.Message);
                    }
                    finally
                    {
                        _ZeroCDG = false;
                    }
                }
            }
            catch (Exception e)
            {
                CDGSignal.Value = Double.NaN;
                //commError = true;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Sets a flag to start the current read, false if voltage
        /// </summary>
        public bool StartZeroCDG { set { _ZeroCDG = true; } }

        /// <summary>
        /// set the read mode  ' Code incomplete
        /// </summary>
        public int SetReadMode()
        {
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

        public IPAddress IPAddressOfDevice
        {
            get { return _ipaddressOfDevice; }
            set { _ipaddressOfDevice = value; }
        }

        /// <summary>
        /// Value (pressure) of the Controller
        /// </summary>
        public double Value
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
        /// ExtendedError EXE uint16 R Extended Errors, coded as 16bit uint.
        ///        High byte: Bit 0 PT1000 fault
        ///Bit 1 Heater block overtemperature
        ///Bit 2 Electronic overtemperature
        ///Bit 3 Zero adjust error
        ///Low byte: Bit 0 Atm.pressure out of range
        ///Bit 1 Temperature out of range
        ///Bit 4 Cal.mode wrong
        ///Bit 5 Pressure underflow
        ///Bit 6 Pressure overflow
        ///Bit 7 Zero adjust warning
        /// </summary>
        public string ErrorNumber
        {
            get { return _errorNum; }
        }

        /// <summary>
        /// Formatted value including the <see cref="Units">Units</see>
        /// </summary>
        public string FormattedValue
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
        public string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the controller
        /// </summary>
        public string Format
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
        public string Name
        {
            get { return "InficonCube " + this.IPAddressOfDevice + " - " + _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the Controller
        /// </summary>
        public double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the controller
        /// </summary>
        public double Max
        {
            get { return _max; }
        }

        #endregion Public Properties
    }
}