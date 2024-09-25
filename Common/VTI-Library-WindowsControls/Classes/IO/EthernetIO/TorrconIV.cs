using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.FormatProviders;

namespace VTIWindowsControlLibrary.Classes.IO.EthernetIO
{
    /// <summary>
    /// Serial Interface for a Controller
    /// </summary>
    public class TorrconIV : EthernetIOBase
    {
        #region Private Vars
        private double disconnectLimitSeconds = 2;
        private double disconnectedGaugeValue = 999;
        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private float _min, _max;
        private string _format = "0.000";
        private TorrConFormatProvider _formatProvider = new TorrConFormatProvider();
        private bool _stopProcessing = true;
        private Socket clientSocket;
        private int failedConnectionAttempts = 0;
        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();
        private bool _setModbusData = false;
        private DateTime DisconnectStartTime = DateTime.Now;
        private bool ReadPLC = false;

        private byte[] _modbusWriteArray = new byte[200];

        private bool _writeGauge1Gain = false;
        private bool _writeGauge1Offset = false;
        private bool _writeGauge1UnitGain = false;
        private bool _writeGauge1UnitOffset = false;
        private bool _writeGauge1Name = false;
        private bool _writeGauge1Units = false;

        private bool _writeGauge2Gain = false;
        private bool _writeGauge2Offset = false;
        private bool _writeGauge2UnitGain = false;
        private bool _writeGauge2UnitOffset = false;
        private bool _writeGauge2Name = false;
        private bool _writeGauge2Units = false;

        private bool _writeGauge1Enabled = false;
        private bool _writeGauge1IsConvection = false;
        private bool _writeGauge2Enabled = false;
        private bool _writeGauge2OutputTypeIs4To20mA = false;
        private bool _writeSetpoint1 = false;
        private bool _writeSetpoint2 = false;
        private bool _writeSetpoint1IsGauge1 = false;
        private bool _writeSetpoint2IsGauge1 = false;
        private bool _writeScreenRotationIsDIN_Rail = false;
        private bool _writeGauge1Average = false;
        private bool _writeGauge2Average = false;
        private bool _writePassword = false;
        private bool _writeControllerName = false;
        private bool _writeReturnCharacterIsCR_Only = false;
        private bool _writeDHCP_Enabled = false;
        private bool _writeIP_Address = false;
        private bool _writeSubnetMask = false;
        private bool _writeGatewayAddress = false;
        private bool _writeModbusID = false;
        private bool _writeSocketPort = false;


        private int _processDelay = 100;
        private string _controllerName;
        private double _setpoint1;
        private double _setpoint2;
        private string _read_IP_Address;
        private string _IP_AddressToWrite;

        private string _gatewayAddress;
        private byte _modbusID;
        private string _subnetMask;
        private ushort _readSocketPort;
        private ushort _socketPortToWrite;
        private bool _DHCP_Enabled;
        private bool _screenRotationIsDIN_Rail;
        private bool _returnCharacterIsCR_Only;
        private bool _setpoint1IsGauge1;
        private bool _setpoint2IsGauge1;
        private string _password;

        private bool _gauge1_Enabled;
        private string _gauge1_Name;
        private string _gauge1_Units;
        private double _gauge1_Voltage;
        private double _gauge1_Pressure = 999;
        private double _gauge1_Gain;
        private double _gauge1_Offset;
        private double _gauge1_UnitGain;
        private double _gauge1_UnitOffset;
        private bool _gauge1_IsConvection;
        private byte _gauge1_Average;

        private bool _gauge2_Enabled;
        private string _gauge2_Name;
        private string _gauge2_Units;
        private double _gauge2_Voltage;
        private double _gauge2_Pressure = 999;
        private double _gauge2_Gain;
        private double _gauge2_Offset;
        private double _gauge2_UnitGain;
        private double _gauge2_UnitOffset;
        private byte _gauge2_Average;
        private bool _gauge2_OutputTypeIs4To20mA;

        //private Gauge _gauge1 = new Gauge();
        //private Gauge _gauge2 = new Gauge();

        #endregion Private Vars

        #region Event Handlers
        private EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

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

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrconIV">TorrconIV</see> class
        /// </summary>
        public TorrconIV()
        {
            //DMMSignal = new AnalogSignal("DMM Signal", "VDC", "0.000", 1000, false, true);
            //Format = "0.000";
        }

        //public class Gauge
        //{
        //    public bool Enabled;
        //    public string Name;
        //    public string Units;
        //    public double Voltage;
        //    public double Pressure;
        //    public double Gain;
        //    public double Offset;
        //    public double UnitGain;
        //    public double UnitOffset;
        //    public bool ConvectionFlag;
        //    public double Average;
        //    public string OutputType;
        //}

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
                    String.Format($"Starting the processing thread for Ethernet TorrconIV. Port name: {EthernetPortParameter.ProcessValue.PortName}."),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);

                //_stopProcessing = true;
                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.DoWork += backgroundWorker_DoWork;

                Connect();

                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception e)
            {
                try
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to start the TorrconIV."),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                        e.ToString());
                }
                catch { }
            }
        }

        private void Disconnect()
        {
            DisconnectStartTime = DateTime.Now;
            try
            {
                //clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception e)
            {
                string t = e.Message;
            }
        }

        private void Connect()
        {
            _stopProcessing = true;

            //create the socket
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // specify the IP address and port to connect to
            IPAddress ipAddress = IPAddress.Parse(EthernetPortParameter.ProcessValue.IPAddress);
            int port = Convert.ToInt32(EthernetPortParameter.ProcessValue.Port);

            // set the maximum number of attempts to connect to the socket
            int maxAttempts = 5;
            int attemptCount = 0;

            while (!clientSocket.Connected && attemptCount < maxAttempts)
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    ClientForms.SplashScreen.Message = $"Connecting to {EthernetPortParameter.ProcessValue.PortName} on {EthernetPortParameter.ProcessValue.IPAddress}";
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                }
                try
                {
                    // attempt to connect to the socket
                    //clientSocket.Connect(ipAddress, port);

                    IAsyncResult result = clientSocket.BeginConnect(ipAddress, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(1000, true);

                    if (clientSocket.Connected)
                    {
                        clientSocket.EndConnect(result);
                    }
                    else
                    {
                        // NOTE, MUST CLOSE THE SOCKET
                        clientSocket.Close();
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"Error connecting to TorrconIV on IP Address {EthernetPortParameter.ProcessValue.IPAddress}. Exception message: " + e.Message);
                    // if the connection attempt fails, wait for a short time before trying again
                    System.Threading.Thread.Sleep(500);
                }

                // increment the attempt counter
                attemptCount++;
            }

            // check if the socket is connected
            if (clientSocket.Connected)
            {
                _stopProcessing = false;
            }
            else
            {
                MessageBox.Show($"Unable to connect to {EthernetPortParameter.ProcessValue.PortName} on {EthernetPortParameter.ProcessValue.IPAddress}. Verify the IP Address on the device matches the IP Address in Edit Cycle -> Common Control Parameters and then restart the VTI app. Otherwise, press the push button in the middle the Torrcon IV board to restart it.");
            }


            //try
            //{
            //    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    // Connect to the specified host.
            //    IPAddress IP = System.Net.IPAddress.Parse(EthernetPortParameter.ProcessValue.IPAddress);
            //    int sock = Convert.ToInt32(EthernetPortParameter.ProcessValue.Port);
            //    var endPoint = new IPEndPoint(IP, sock);
            //    clientSocket.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), clientSocket);
            //    connectDone.WaitOne(2000);
            //    if (!clientSocket.Connected)
            //    {
            //        Console.WriteLine("Torrcon IV Connection Fail. IP: " + EthernetPortParameter.ProcessValue.IPAddress);
            //        failedConnectionAttempts++;
            //        //Thread.Sleep(1000);
            //        //Stop();
            //        //Start();
            //    }
            //    else
            //    {
            //        Console.WriteLine("Torrcon IV Connection Success. IP: " + EthernetPortParameter.ProcessValue.IPAddress);
            //        failedConnectionAttempts = 0;
            //    }
            //}
            //catch (SocketException ex)
            //{
            //    Console.WriteLine("TorrconIV SocketException: " + ex.Message);
            //}
            //catch (ObjectDisposedException ex)
            //{
            //    Console.WriteLine("TorrconIV ObjectDisposedException: " + ex.Message);
            //}
        }

        //public override bool UpdateIPOnDevice()
        //{
        //    _IP_Address_to_write = EthernetPortParameter.ProcessValue.IPAddress;
        //    _writeIP_Address = true;
        //    _socketPort = Convert.ToUInt16(EthernetPortParameter.ProcessValue.Port);
        //    _writeSocketPort = true;
        //    _setModbusData = true;
        //    while (_setModbusData && clientSocket.Connected)
        //    {

        //    }
        //    return true;
        //}

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Stopwatch sw = new Stopwatch();
            while (!_stopProcessing)
            {
                Thread.Sleep(_processDelay);
                //if (failedConnectionAttempts >= 5)
                //{
                //    string msg = "Unable to connect to " + this.Name + " (" + EthernetPortParameter.ProcessValue.IPAddress + ":" + EthernetPortParameter.ProcessValue.Port + ") after " + failedConnectionAttempts + " attempts.";
                //    VtiEvent.Log.WriteError(msg);
                //    MessageBox.Show(msg);
                //    //_stopProcessing = true;
                //    backgroundWorker.Dispose();
                //    return;
                //}
                //if (_IP_Address != EthernetPortParameter.ProcessValue.IPAddress)
                //{

                //}
                //if (_socketPort != Convert.ToUInt16(EthernetPortParameter.ProcessValue.Port))
                //{

                //}

                if (_setModbusData)
                {
                    #region assign _modbusWriteArray

                    if (_writeGauge1Gain)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(0, _gauge1_Gain);
                    }
                    else if (_writeGauge1Offset)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(2, _gauge1_Offset);
                    }
                    else if (_writeGauge1UnitGain)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(4, _gauge1_UnitGain);
                    }
                    else if (_writeGauge1UnitOffset)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(6, _gauge1_UnitOffset);
                    }
                    else if (_writeGauge1Name)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForStringValue(8, 10, _gauge1_Name);
                    }
                    else if (_writeGauge1Units)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForStringValue(13, 4, _gauge1_Units);
                    }
                    else if (_writeGauge2Gain)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(16, _gauge2_Gain);
                    }
                    else if (_writeGauge2Offset)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(18, _gauge2_Offset);
                    }
                    else if (_writeGauge2UnitGain)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(20, _gauge2_UnitGain);
                    }
                    else if (_writeGauge2UnitOffset)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(22, _gauge2_UnitOffset);
                    }
                    else if (_writeGauge2Name)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForStringValue(24, 10, _gauge2_Name);
                    }
                    else if (_writeGauge2Units)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForStringValue(29, 4, _gauge2_Units);
                    }
                    else if (_writeGauge1Enabled)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(32, _gauge1_Enabled);
                    }
                    else if (_writeGauge1IsConvection)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(33, _gauge1_IsConvection);
                    }
                    else if (_writeGauge2Enabled)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(34, _gauge2_Enabled);
                    }
                    else if (_writeGauge2OutputTypeIs4To20mA)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(35, _gauge2_OutputTypeIs4To20mA);
                    }
                    else if (_writeSetpoint1)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(36, _setpoint1);
                    }
                    else if (_writeSetpoint2)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForFloatValue(38, _setpoint2);
                    }
                    else if (_writeSetpoint1IsGauge1)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(40, !_setpoint1IsGauge1); //default is Gauge 1 (false)
                    }
                    else if (_writeSetpoint2IsGauge1)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(41, !_setpoint2IsGauge1); //default is Gauge 1 (false)
                    }
                    else if (_writeScreenRotationIsDIN_Rail)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(42, !_screenRotationIsDIN_Rail); //default = DIN Rail = false
                    }
                    else if (_writeGauge1Average)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForIntValue(43, _gauge1_Average);
                    }
                    else if (_writeGauge2Average)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForIntValue(44, _gauge2_Average);
                    }
                    else if (_writePassword)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForStringValue(45, 4, _password);
                    }
                    else if (_writeControllerName)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForStringValue(47, 10, _controllerName);
                    }
                    else if (_writeReturnCharacterIsCR_Only)
                    {
                        //_modbusWriteArray = GetModbusWriteArrayForByteValue(53, _returnCharacterIsCR_Only);
                    }
                    else if (_writeDHCP_Enabled)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForBoolValue(54, _DHCP_Enabled);
                    }
                    if (_writeIP_Address)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForIPAddressValue(55, _IP_AddressToWrite);
                    }
                    else if (_writeSubnetMask)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForIPAddressValue(57, _subnetMask);
                    }
                    else if (_writeGatewayAddress)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForIPAddressValue(59, _gatewayAddress);
                    }
                    else if (_writeModbusID)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForIntValue(61, _modbusID);
                    }
                    if (_writeSocketPort)
                    {
                        _modbusWriteArray = GetModbusWriteArrayForIntValue(62, _socketPortToWrite);
                    }

                    #endregion

                    #region clear write flags
                    _setModbusData = false;
                    _writeGauge1Gain = false;
                    _writeGauge1Offset = false;
                    _writeGauge1UnitGain = false;
                    _writeGauge1UnitOffset = false;
                    _writeGauge1Name = false;
                    _writeGauge1Units = false;

                    _writeGauge2Gain = false;
                    _writeGauge2Offset = false;
                    _writeGauge2UnitGain = false;
                    _writeGauge2UnitOffset = false;
                    _writeGauge2Name = false;
                    _writeGauge2Units = false;

                    _writeGauge1Enabled = false;
                    _writeGauge1IsConvection = false;
                    _writeGauge2Enabled = false;
                    _writeGauge2OutputTypeIs4To20mA = false;
                    _writeSetpoint1 = false;
                    _writeSetpoint2 = false;
                    _writeSetpoint1IsGauge1 = false;
                    _writeSetpoint2IsGauge1 = false;
                    _writeScreenRotationIsDIN_Rail = false;
                    _writeGauge1Average = false;
                    _writeGauge2Average = false;
                    _writePassword = false;
                    _writeControllerName = false;
                    _writeReturnCharacterIsCR_Only = false;
                    _writeDHCP_Enabled = false;
                    _writeIP_Address = false;
                    _writeSubnetMask = false;
                    _writeGatewayAddress = false;
                    _writeModbusID = false;
                    _writeSocketPort = false;
                    #endregion

                    #region send _modbusWriteArray

                    try
                    {
                        int TempInt = 0;
                        clientSocket.SendTimeout = 1000;
                        TempInt = clientSocket.Send(_modbusWriteArray, _modbusWriteArray.Length, SocketFlags.None);

                        if (TempInt == _modbusWriteArray.Length)//send correct number of bytes
                        {
                            try
                            {
                                int TempReadInt = 0;
                                byte[] byteReadData = new byte[600];
                                clientSocket.ReceiveTimeout = 1000;
                                TempReadInt = clientSocket.Receive(byteReadData, 12, SocketFlags.None);
                                if (TempReadInt != 12)
                                {
                                    Console.WriteLine("Error writing TorrconIV value");
                                }
                            }
                            catch (Exception ex)
                            {
                                var t = ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var t = ex.Message;
                    }

                    #endregion
                }
                else if (ReadPLC)
                {
                    try
                    {
                        //read analog inputs by TCPIP
                        // Send test data to the remote device.
                        byte[] byteData = new byte[12];
                        byteData[0] = (byte)0;
                        byteData[1] = (byte)1;
                        byteData[2] = (byte)0;
                        byteData[3] = (byte)0;
                        byteData[4] = (byte)0;
                        byteData[5] = (byte)6;
                        byteData[6] = (byte)255;
                        byteData[7] = (byte)3;
                        byteData[8] = (byte)0;
                        byteData[9] = (byte)65;//gauge value 1 as a float
                        byteData[10] = (byte)0;
                        byteData[11] = (byte)2;//number of words to read

                        //bytesSentValue = 0;

                        try
                        {
                            int TempInt = 0;
                            clientSocket.SendTimeout = 1000;
                            TempInt = clientSocket.Send(byteData, 12, SocketFlags.None);

                            if (TempInt == 12)//send correct number of bytes
                            {
                                try
                                {
                                    int TempReadInt = 0;
                                    byte[] byteReadData = new byte[60];

                                    clientSocket.ReceiveTimeout = 1000;

                                    TempReadInt = clientSocket.Receive(byteReadData, 13, SocketFlags.None);//read 4 bytes, 2 words

                                    if (TempReadInt > 12)//24 for 8
                                    {
                                        DisconnectStartTime = DateTime.Now;
                                        //convert the data here
                                        float fltValue = BitConverter.ToSingle(new byte[] { byteReadData[9], byteReadData[10], byteReadData[11], byteReadData[12] }, 0);
                                        //_gauge1_Pressure = (Double)fltValue;
                                        _gauge1_Pressure = (Double)fltValue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("TorrconIV ReadPLC receive timeout.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime;
                                    if (DisconnectTime.TotalSeconds > disconnectLimitSeconds)
                                    {
                                        _gauge1_Pressure = _gauge2_Pressure = disconnectedGaugeValue;
                                        Value = disconnectedGaugeValue;
                                        Disconnect();
                                        Connect();
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("TorrconIV ReadPLC send timeout.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("TorrconIV ReadPLC exception 2: " + ex.Message);
                            TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime;
                            if (DisconnectTime.TotalSeconds > disconnectLimitSeconds)
                            {
                                _gauge1_Pressure = _gauge2_Pressure = disconnectedGaugeValue;
                                Value = disconnectedGaugeValue;
                                Disconnect();
                                Connect();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("TorrconIV ReadPLC exception: " + ex.Message);
                    }
                }
                else
                {
                    #region read all values from TorrconIV

                    try
                    {
                        //read analog inputs by TCPIP
                        // Send test data to the remote device.
                        byte[] byteData = new byte[12];
                        byteData[0] = (byte)0;
                        byteData[1] = (byte)1;
                        byteData[2] = (byte)0;
                        byteData[3] = (byte)0;
                        byteData[4] = (byte)0;
                        byteData[5] = (byte)6;
                        byteData[6] = (byte)255;
                        byteData[7] = (byte)3;
                        byteData[8] = (byte)0;
                        byteData[9] = (byte)0;
                        byteData[10] = (byte)0;
                        byteData[11] = (byte)80;//number of words to read

                        //bytesSentValue = 0;

                        if (clientSocket.Connected)
                        {
                            try
                            {
                                int TempInt = 0;
                                clientSocket.SendTimeout = 50;
                                TempInt = clientSocket.Send(byteData, 12, SocketFlags.None);
                                if (TempInt == 12)//send correct number of bytes
                                {
                                    try
                                    {
                                        int TempReadInt = 0;
                                        byte[] byteReadData = new byte[600];
                                        clientSocket.ReceiveTimeout = 50;
                                        TempReadInt = clientSocket.Receive(byteReadData, 169, SocketFlags.None);//13 is read 4 bytes, 2 words,149  is 160 bytes 80 words

                                        if (TempReadInt > 168)//12 for 2 words, 24 for 8 words, 168 is for 80 words
                                        {
                                            DisconnectStartTime = DateTime.Now;

                                            //convert the data here
                                            float fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[9], byteReadData[10], byteReadData[12], byteReadData[11] }, 0);
                                            _gauge1_Gain = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[14], byteReadData[13], byteReadData[16], byteReadData[15] }, 0);
                                            _gauge1_Offset = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[18], byteReadData[17], byteReadData[20], byteReadData[19] }, 0);
                                            _gauge1_UnitGain = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[22], byteReadData[21], byteReadData[24], byteReadData[23] }, 0);
                                            _gauge1_UnitOffset = (Double)fltValue;

                                            _gauge1_Average = byteReadData[96];

                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[42], byteReadData[41], byteReadData[44], byteReadData[43] }, 0);
                                            _gauge2_Gain = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[46], byteReadData[45], byteReadData[48], byteReadData[47] }, 0);
                                            _gauge2_Offset = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[50], byteReadData[49], byteReadData[52], byteReadData[51] }, 0);
                                            _gauge2_UnitGain = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[54], byteReadData[53], byteReadData[56], byteReadData[55] }, 0);
                                            _gauge2_UnitOffset = (Double)fltValue;

                                            _gauge2_Average = byteReadData[98];

                                            _modbusID = byteReadData[132];
                                            _readSocketPort = ((byte)(256.0 * byteReadData[133] + byteReadData[134]));

                                            //setpoint values
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[82], byteReadData[81], byteReadData[84], byteReadData[83] }, 0);
                                            _setpoint1 = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[86], byteReadData[85], byteReadData[88], byteReadData[87] }, 0);
                                            _setpoint2 = (Double)fltValue;

                                            //current pressure values
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[150], byteReadData[149], byteReadData[152], byteReadData[151] }, 0);
                                            Value = _gauge1_Pressure = (Double)fltValue; //assign to Value to trigger ValueChanged event
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[154], byteReadData[153], byteReadData[156], byteReadData[155] }, 0);
                                            _gauge2_Pressure = (Double)fltValue;

                                            //current voltage values
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[158], byteReadData[157], byteReadData[160], byteReadData[159] }, 0);
                                            _gauge1_Voltage = (Double)fltValue;
                                            fltValue = BitConverter.ToSingle(new byte[] { byteReadData[162], byteReadData[161], byteReadData[164], byteReadData[163] }, 0);
                                            _gauge2_Voltage = (Double)fltValue;

                                            //_Gauge 1 name and units
                                            string strValue = "";
                                            if (byteReadData[26] > 0) strValue += (char)byteReadData[26];
                                            if (byteReadData[25] > 0) strValue += (char)byteReadData[25];
                                            if (byteReadData[28] > 0) strValue += (char)byteReadData[28];
                                            if (byteReadData[27] > 0) strValue += (char)byteReadData[27];
                                            if (byteReadData[30] > 0) strValue += (char)byteReadData[30];
                                            if (byteReadData[29] > 0) strValue += (char)byteReadData[29];
                                            if (byteReadData[32] > 0) strValue += (char)byteReadData[32];
                                            if (byteReadData[31] > 0) strValue += (char)byteReadData[31];
                                            if (byteReadData[34] > 0) strValue += (char)byteReadData[34];
                                            if (byteReadData[33] > 0) strValue += (char)byteReadData[33];

                                            _gauge1_Name = strValue;
                                            strValue = "";

                                            if (byteReadData[36] > 0) strValue += (char)byteReadData[36];
                                            if (byteReadData[35] > 0) strValue += (char)byteReadData[35];
                                            if (byteReadData[38] > 0) strValue += (char)byteReadData[38];
                                            if (byteReadData[37] > 0) strValue += (char)byteReadData[37];

                                            _gauge1_Units = strValue;
                                            //gauge 2 name and units
                                            strValue = "";
                                            if (byteReadData[58] > 0) strValue += (char)byteReadData[58];
                                            if (byteReadData[57] > 0) strValue += (char)byteReadData[57];
                                            if (byteReadData[60] > 0) strValue += (char)byteReadData[60];
                                            if (byteReadData[59] > 0) strValue += (char)byteReadData[59];
                                            if (byteReadData[62] > 0) strValue += (char)byteReadData[62];
                                            if (byteReadData[61] > 0) strValue += (char)byteReadData[61];
                                            if (byteReadData[64] > 0) strValue += (char)byteReadData[64];
                                            if (byteReadData[63] > 0) strValue += (char)byteReadData[63];
                                            if (byteReadData[66] > 0) strValue += (char)byteReadData[66];
                                            if (byteReadData[65] > 0) strValue += (char)byteReadData[65];

                                            _gauge2_Name = strValue;
                                            strValue = "";
                                            //for (int i = 67; i < 71; i++)
                                            //{
                                            //    if (byteReadData[i] > 0)
                                            //    {
                                            //        strValue += (char)byteReadData[i];
                                            //    }
                                            //}
                                            if (byteReadData[68] > 0) strValue += (char)byteReadData[68];
                                            if (byteReadData[67] > 0) strValue += (char)byteReadData[67];
                                            if (byteReadData[70] > 0) strValue += (char)byteReadData[70];
                                            if (byteReadData[69] > 0) strValue += (char)byteReadData[69];

                                            _gauge2_Units = strValue;
                                            //Controller Name
                                            strValue = "";
                                            //for (int i = 103; i < 113; i++)
                                            //{
                                            //    if (byteReadData[i] > 0)
                                            //    {
                                            //        strValue += (char)byteReadData[i];
                                            //    }
                                            //}
                                            if (byteReadData[104] > 0) strValue += (char)byteReadData[104];
                                            if (byteReadData[103] > 0) strValue += (char)byteReadData[103];
                                            if (byteReadData[106] > 0) strValue += (char)byteReadData[106];
                                            if (byteReadData[105] > 0) strValue += (char)byteReadData[105];
                                            if (byteReadData[108] > 0) strValue += (char)byteReadData[108];
                                            if (byteReadData[107] > 0) strValue += (char)byteReadData[107];
                                            if (byteReadData[110] > 0) strValue += (char)byteReadData[110];
                                            if (byteReadData[109] > 0) strValue += (char)byteReadData[109];
                                            if (byteReadData[112] > 0) strValue += (char)byteReadData[112];
                                            if (byteReadData[111] > 0) strValue += (char)byteReadData[111];

                                            _controllerName = strValue;

                                            //ipaddress
                                            strValue = "";
                                            strValue += byteReadData[120].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[119].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[122].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[121].ToString();
                                            _read_IP_Address = strValue;
                                            //subnet mask
                                            strValue = "";
                                            strValue += byteReadData[124].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[123].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[126].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[125].ToString();
                                            _subnetMask = strValue;
                                            //gateway address
                                            strValue = "";
                                            strValue += byteReadData[128].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[127].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[130].ToString();
                                            strValue += ".";
                                            strValue += byteReadData[129].ToString();
                                            _gatewayAddress = strValue;

                                            //DHCP_Enabled
                                            if (byteReadData[118] == 0)
                                            {
                                                _DHCP_Enabled = false;
                                            }
                                            else
                                            {
                                                _DHCP_Enabled = true;
                                            }
                                            //Screen Rotation
                                            if ((byteReadData[94]) == 0)
                                            {
                                                _screenRotationIsDIN_Rail = true; //DIN RAIL
                                            }
                                            else
                                            {
                                                _screenRotationIsDIN_Rail = false; //BOX
                                            }
                                            //Return Char
                                            //var t = (char)byteReadData[116];
                                            //var y = Encoding.ASCII.GetBytes(Environment.NewLine);
                                            if (byteReadData[116] == 10)
                                            {
                                                _returnCharacterIsCR_Only = true;//"CR"
                                            }
                                            else
                                            {
                                                _returnCharacterIsCR_Only = false;//"CR & LF";
                                            }
                                            //Guage 1 Enabled
                                            if (byteReadData[74] == 0)
                                            {
                                                _gauge1_Enabled = false;
                                            }
                                            else
                                            {
                                                _gauge1_Enabled = true;
                                            }
                                            //Guage 1 Convection Flag
                                            if (byteReadData[76] == 0)
                                            {
                                                _gauge1_IsConvection = false;
                                            }
                                            else
                                            {
                                                _gauge1_IsConvection = true;
                                            }
                                            //Guage 2 Enabled
                                            if (byteReadData[78] == 0)
                                            {
                                                _gauge2_Enabled = false;
                                            }
                                            else
                                            {
                                                _gauge2_Enabled = true;
                                            }
                                            //Guage 2 Type Flag
                                            if (byteReadData[80] == 0)
                                            {
                                                _gauge2_OutputTypeIs4To20mA = false; //"VOLTAGE";
                                            }
                                            else
                                            {
                                                _gauge2_OutputTypeIs4To20mA = true; //"4-20 MA";
                                            }
                                            //SP1 _Gauge
                                            if ((byteReadData[90]) == 0)
                                            {
                                                _setpoint1IsGauge1 = true; //gauge 1
                                            }
                                            else
                                            {
                                                _setpoint1IsGauge1 = false; //gauge 2
                                            }
                                            //SP2 _Gauge
                                            if ((byteReadData[92]) == 0)
                                            {
                                                _setpoint2IsGauge1 = true; //gauge 1
                                            }
                                            else
                                            {
                                                _setpoint2IsGauge1 = false; //gauge 2
                                            }
                                        }
                                        else
                                        {
                                            //if (clientSocket.RemoteEndPoint != null)
                                            //{
                                            //    Console.WriteLine("TorrconIV Receive Timeout. IP:" + clientSocket.RemoteEndPoint.ToString());
                                            //}
                                            //else
                                            //{
                                            //    Console.WriteLine("TorrconIV Receive Timeout.");
                                            //}
                                            Console.WriteLine("TorrconIV (IP: " + EthernetPortParameter.ProcessValue.IPAddress + ") Receive Timeout.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //if (clientSocket.RemoteEndPoint != null)
                                        //{
                                        //    Console.WriteLine("TorrconIV (IP: " + clientSocket.RemoteEndPoint.ToString() + ") exception: " + ex.Message);
                                        //}
                                        //else
                                        //{
                                        //    Console.WriteLine("TorrconIV exception: " + ex.Message);
                                        //}
                                        Console.WriteLine("TorrconIV (IP: " + EthernetPortParameter.ProcessValue.IPAddress + ") exception: " + ex.Message);

                                        TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime;
                                        if (DisconnectTime.TotalSeconds > disconnectLimitSeconds)
                                        {
                                            _gauge1_Pressure = _gauge2_Pressure = disconnectedGaugeValue;
                                            Value = disconnectedGaugeValue;
                                            Disconnect();
                                            Connect();
                                        }
                                    }
                                }
                                else
                                {
                                    //if (clientSocket.RemoteEndPoint != null)
                                    //{
                                    //    Console.WriteLine("TorrconIV Send Timeout. IP:" + clientSocket.RemoteEndPoint.ToString());
                                    //}
                                    //else
                                    //{
                                    //    Console.WriteLine("TorrconIV Send Timeout.");
                                    //}
                                    Console.WriteLine("TorrconIV (IP: " + EthernetPortParameter.ProcessValue.IPAddress + ") Send Timeout.");
                                }
                            }
                            catch (Exception ex)
                            {
                                //if (clientSocket.RemoteEndPoint != null)
                                //{
                                //    Console.WriteLine("TorrconIV (IP: " + clientSocket.RemoteEndPoint.ToString() + ") exception: " + ex.Message);
                                //}
                                //else
                                //{
                                //    Console.WriteLine("TorrconIV exception: " + ex.Message);
                                //}
                                Console.WriteLine("TorrconIV (IP: " + EthernetPortParameter.ProcessValue.IPAddress + ") exception: " + ex.Message);
                                TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime;
                                if (DisconnectTime.TotalSeconds > disconnectLimitSeconds)
                                {
                                    _gauge1_Pressure = _gauge2_Pressure = disconnectedGaugeValue;
                                    Value = disconnectedGaugeValue;
                                    Disconnect();
                                    Connect();
                                }
                            }
                        }
                        else
                        {
                            //Console.WriteLine("TorrconIV (IP: " + EthernetPortParameter.ProcessValue.IPAddress + ") socket is not connected.");
                            TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime;
                            if (DisconnectTime.TotalSeconds > disconnectLimitSeconds)
                            {
                                _gauge1_Pressure = _gauge2_Pressure = disconnectedGaugeValue;
                                Value = disconnectedGaugeValue;
                                Disconnect();
                                Connect();
                            }
                        }
                    }
                    catch (System.Net.Sockets.SocketException se)
                    {
                        //A request to send or receive data was disallowed because the socket is not connected and (when sending on a datagram socket using a sendto call) no address was supplied
                        string t = se.Message;
                    }
                    catch (Exception ex)
                    {
                        //if (clientSocket.RemoteEndPoint != null)
                        //{
                        //    Console.WriteLine("TorrconIV (IP: " + clientSocket.RemoteEndPoint.ToString() + ") exception: " + ex.Message);
                        //}
                        //else
                        //{
                        //    Console.WriteLine("TorrconIV exception: " + ex.Message);
                        //}
                        Console.WriteLine("TorrconIV (IP: " + EthernetPortParameter.ProcessValue.IPAddress + ") exception: " + ex.Message);
                    }

                    #endregion
                }
                //sw.Stop();
                //if (sw.ElapsedMilliseconds > 13)
                //{
                //    Console.WriteLine("sw ms elapsed: " + sw.ElapsedMilliseconds);
                //}
                //sw.Reset();
            }
        }

        #region GetModbusArray

        private byte[] GetModbusWriteArrayForFloatValue(int address, double value)
        {
            byte[] byteFloat = new byte[4];
            float dataFloat = 0;
            dataFloat = Convert.ToSingle(value);
            byteFloat = BitConverter.GetBytes(dataFloat);

            byte[] modbusWriteArray = new byte[17];
            //load the Modbus data array
            modbusWriteArray[0] = (byte)0;//constant
            modbusWriteArray[1] = (byte)1;//constant
            modbusWriteArray[2] = (byte)0;//constant
            modbusWriteArray[3] = (byte)0;//constant
            modbusWriteArray[4] = (byte)0;//constant
            modbusWriteArray[5] = (byte)11; //sending 11 bytes (11 lines below this line)
            modbusWriteArray[6] = (byte)255;//constant
            modbusWriteArray[7] = (byte)16;//constant
            modbusWriteArray[8] = (byte)0;//constant, addmsb
            modbusWriteArray[9] = (byte)address; //first address number for the target variable
            modbusWriteArray[10] = (byte)0;//num datapts msb
            modbusWriteArray[11] = (byte)2;//number datapts lsb - number of words (2 bytes = 1 word)
            modbusWriteArray[12] = (byte)4;//number of bytes
            modbusWriteArray[13] = (byte)byteFloat[1]; //data to send
            modbusWriteArray[14] = (byte)byteFloat[0]; //data to send
            modbusWriteArray[15] = (byte)byteFloat[3]; //data to send
            modbusWriteArray[16] = (byte)byteFloat[2]; //data to send

            return modbusWriteArray;
        }

        private byte[] GetModbusWriteArrayForStringValue(int address, int targetVariableSize, string value)
        {
            byte[] valueByteArray = new byte[targetVariableSize];
            for (int i = 0; i < targetVariableSize; i++)
            {
                if (i < value.Length)
                {
                    valueByteArray[i] = Convert.ToByte(value[i]);
                }
                else
                {
                    valueByteArray[i] = (byte)0;
                }
            }

            byte[] modbusWriteArray;
            if (targetVariableSize == 4)
            {
                modbusWriteArray = new byte[17];
            }
            else //if (targetVariableSize == 10)
            {
                modbusWriteArray = new byte[23];
            }
            //load the Modbus data array
            modbusWriteArray[0] = (byte)0;//constant
            modbusWriteArray[1] = (byte)1;//constant
            modbusWriteArray[2] = (byte)0;//constant
            modbusWriteArray[3] = (byte)0;//constant
            modbusWriteArray[4] = (byte)0;//constant
            modbusWriteArray[5] = (byte)(7 + targetVariableSize); //sending (7 + variable size) bytes ((7 + variable size) lines below this line)
            modbusWriteArray[6] = (byte)255;//constant
            modbusWriteArray[7] = (byte)16;//constant
            modbusWriteArray[8] = (byte)0;//constant, addmsb
            modbusWriteArray[9] = (byte)address; //first address number for the target variable
            modbusWriteArray[10] = (byte)0;//num datapts msb
            if (targetVariableSize == 4)
            {
                modbusWriteArray[11] = (byte)2;//number datapts lsb - number of words (2 bytes = 1 word)
                modbusWriteArray[12] = (byte)4;//number of bytes
            }
            else if (targetVariableSize == 10)
            {
                modbusWriteArray[11] = (byte)5;//number datapts lsb - number of words (2 bytes = 1 word)
                modbusWriteArray[12] = (byte)10;//number of bytes
            }
            modbusWriteArray[13] = (byte)valueByteArray[1]; //data to send
            modbusWriteArray[14] = (byte)valueByteArray[0]; //data to send
            modbusWriteArray[15] = (byte)valueByteArray[3]; //data to send
            modbusWriteArray[16] = (byte)valueByteArray[2]; //data to send
            if (targetVariableSize == 10)
            {
                modbusWriteArray[17] = (byte)valueByteArray[5]; //data to send
                modbusWriteArray[18] = (byte)valueByteArray[4]; //data to send
                modbusWriteArray[19] = (byte)valueByteArray[7]; //data to send
                modbusWriteArray[20] = (byte)valueByteArray[6]; //data to send
                modbusWriteArray[21] = (byte)valueByteArray[9]; //data to send
                modbusWriteArray[22] = (byte)valueByteArray[8]; //data to send
            }

            return modbusWriteArray;
        }

        private byte[] GetModbusWriteArrayForBoolValue(int address, bool value)
        {

            byte[] modbusWriteArray = new byte[15];
            //load the Modbus data array
            modbusWriteArray[0] = (byte)0;//constant
            modbusWriteArray[1] = (byte)1;//constant
            modbusWriteArray[2] = (byte)0;//constant
            modbusWriteArray[3] = (byte)0;//constant
            modbusWriteArray[4] = (byte)0;//constant
            modbusWriteArray[5] = (byte)9; //sending 9 bytes (9 lines below this line)
            modbusWriteArray[6] = (byte)255;//constant
            modbusWriteArray[7] = (byte)16;//constant
            modbusWriteArray[8] = (byte)0;//constant, addmsb
            modbusWriteArray[9] = (byte)address; //first address number for the target variable
            modbusWriteArray[10] = (byte)0;//num datapts msb
            modbusWriteArray[11] = (byte)1;//number datapts lsb - number of words (2 bytes = 1 word)
            modbusWriteArray[12] = (byte)2;//number of bytes
            modbusWriteArray[13] = (byte)0; //data to send - lsb
            modbusWriteArray[14] = (byte)Convert.ToByte(value); //data to send - msb

            return modbusWriteArray;
        }

        private byte[] GetModbusWriteArrayForIntValue(int address, int value)
        {
            // "word" = "byte" in C# - Unsigned 8-bit integer, values 0-255
            // uint16_t = ushort in C#, 0 - 65,535

            byte[] byteInt = new byte[2];
            byteInt = BitConverter.GetBytes(value);

            byte[] modbusWriteArray = new byte[15];
            //load the Modbus data array
            modbusWriteArray[0] = (byte)0;//constant
            modbusWriteArray[1] = (byte)1;//constant
            modbusWriteArray[2] = (byte)0;//constant
            modbusWriteArray[3] = (byte)0;//constant
            modbusWriteArray[4] = (byte)0;//constant
            modbusWriteArray[5] = (byte)9; //sending 9 bytes (9 lines below this line)
            modbusWriteArray[6] = (byte)255;//constant
            modbusWriteArray[7] = (byte)16;//constant
            modbusWriteArray[8] = (byte)0;//constant, addmsb
            modbusWriteArray[9] = (byte)address; //first address number for the target variable
            modbusWriteArray[10] = (byte)0;//num datapts msb
            modbusWriteArray[11] = (byte)1;//number datapts lsb - number of words (2 bytes = 1 word)
            modbusWriteArray[12] = (byte)2;//number of bytes
            modbusWriteArray[13] = (byte)(byteInt[1]); //data to send - lsb
            modbusWriteArray[14] = (byte)(byteInt[0]); //data to send - msb

            return modbusWriteArray;
        }

        private byte[] GetModbusWriteArrayForIPAddressValue(int address, string IPAddress)
        {
            string[] IPAddressSplit = IPAddress.Split('.');

            byte[] valueByteArray = new byte[4];
            valueByteArray[0] = Convert.ToByte(IPAddressSplit[0]);
            valueByteArray[1] = Convert.ToByte(IPAddressSplit[1]);
            valueByteArray[2] = Convert.ToByte(IPAddressSplit[2]);
            valueByteArray[3] = Convert.ToByte(IPAddressSplit[3]);

            byte[] modbusWriteArray = new byte[17];
            //load the Modbus data array
            modbusWriteArray[0] = (byte)0;//constant
            modbusWriteArray[1] = (byte)1;//constant
            modbusWriteArray[2] = (byte)0;//constant
            modbusWriteArray[3] = (byte)0;//constant
            modbusWriteArray[4] = (byte)0;//constant
            modbusWriteArray[5] = (byte)11; //sending (7 + variable size) bytes ((7 + variable size) lines below this line)
            modbusWriteArray[6] = (byte)255;//constant
            modbusWriteArray[7] = (byte)16;//constant
            modbusWriteArray[8] = (byte)0;//constant, addmsb
            modbusWriteArray[9] = (byte)address; //first address number for the target variable
            modbusWriteArray[10] = (byte)0;//num datapts msb
            modbusWriteArray[11] = (byte)2;//number datapts lsb - number of words (2 bytes = 1 word)
            modbusWriteArray[12] = (byte)4;//number of bytes
            modbusWriteArray[13] = (byte)valueByteArray[1]; //data to send
            modbusWriteArray[14] = (byte)valueByteArray[0]; //data to send
            modbusWriteArray[15] = (byte)valueByteArray[3]; //data to send
            modbusWriteArray[16] = (byte)valueByteArray[2]; //data to send

            return modbusWriteArray;
        }

        #endregion

        // <summary>
        /// Stops the <see cref="Process">Process</see> thread
        /// </summary>
        public override void Stop()
        {
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Stopping the processing thread for TorrconIV"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO);
                _stopProcessing = true;
                if (exitEvent != null)
                    exitEvent.Set();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(String.Format("An error occurred stopping the TorrconIV"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.

            string IP = "(Inaccessible)";
            Socket client;
            try
            {
                client = (Socket)ar.AsyncState;
                IP = EthernetPortParameter.ProcessValue.IPAddress;//client.RemoteEndPoint.ToString();
                // Complete the connection.
                if (client.Connected)
                {
                    client.EndConnect(ar);
                    Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());
                }
                

                // Signal that the connection has been made.
                connectDone.Set();
            }
            catch (ObjectDisposedException ode)
            {
                Console.WriteLine("TorrconIV: Error completing the connection for IP: " + IP);
                Console.WriteLine(ode.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("TorrconIV: Error completing the connection for IP: " + IP);
                Console.WriteLine(e.Message);
            }
        }

        public List<string> GetAllConnectedTorrconIPs()
        {
            bool retry = true;
            Stopwatch sw = new Stopwatch();
            List<string> IP_List = new List<string>();
            sw.Start();
            var ListOfIPsToTest = new List<string>();
            for (int i = 0; i < 31; i++)
            {
                ListOfIPsToTest.Add("192.168.0." + i.ToString());
            }
            while (retry)
            {
                try
                {
                    Parallel.For(0, 31, (i, loopState) =>
                    {
                        Ping ping = new Ping();
                        //Stopwatch sw2 = new Stopwatch();
                        //sw2.Start();
                        PingReply pingReply = ping.Send(ListOfIPsToTest[i], 5);//"192.168.0." + i.ToString(), 5);
                        //sw2.Stop();
                        //Console.WriteLine("Single Ping ms: " + sw2.ElapsedMilliseconds);
                        if (pingReply.Status == IPStatus.Success)
                        {
                            IP_List.Add(ListOfIPsToTest[i]);//"192.168.0." + i.ToString());
                        }
                    });

                    #region alternative - similar performance

                    //var degreeOfParallelism = Environment.ProcessorCount;
                    //var tasks = new Task[degreeOfParallelism];

                    //for (int taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
                    //{
                    //    // capturing taskNumber in lambda wouldn't work correctly
                    //    int taskNumberCopy = taskNumber;

                    //    tasks[taskNumber] = Task.Factory.StartNew(
                    //        () =>
                    //        {
                    //            var max = ListOfIPsToTest.Count * (taskNumberCopy + 1) / degreeOfParallelism;
                    //            for (int i = ListOfIPsToTest.Count * taskNumberCopy / degreeOfParallelism; i < max; i++)
                    //            {
                    //                Ping ping = new Ping();
                    //                PingReply pingReply = ping.Send(ListOfIPsToTest[i], 5);
                    //                if (pingReply.Status == IPStatus.Success)
                    //                {
                    //                    IP_List.Add(ListOfIPsToTest[i]);
                    //                }
                    //            }
                    //        });
                    //}

                    //Task.WaitAll(tasks);
                    #endregion

                    retry = false;
                }
                catch
                {
                    IP_List.Clear();
                }
            }
            sw.Stop();
            Console.WriteLine("IP total ping time (sec): " + Math.Round(sw.ElapsedMilliseconds / 1000f, 1));
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i=0; i<40; i++)
            //{
            //    Ping ping = new Ping();
            //    PingReply pingReply = ping.Send("192.168.0." + i.ToString());
            //    if (pingReply.Status == IPStatus.Success)
            //    {
            //        IP_List.Add("192.168.0." + i.ToString());
            //    }
            //}
            //sw.Stop();
            //Console.WriteLine("Elapsed sec ip Ping: " + sw.ElapsedMilliseconds / 1000);

            //Thread.Sleep(1);
            //if (IP_List.Contains("192.168.0.7"))
            //{
            //    IP_List.Remove("192.168.0.7");
            //}

            List<string> IPsToRemove = new List<string>();

            Stopwatch sw3 = new Stopwatch();
            sw3.Start();
            //IPsToRemove = IP_List.Where(x => !GetMacAddress(x).Contains("c0-")).ToList();
            IP_List = VerifyIPsByMACAddress(IP_List);
            sw3.Stop();
            Console.WriteLine("MAC check ms: " + sw3.ElapsedMilliseconds);

            //foreach (string item in IPsToRemove)
            //{
            //    IP_List.Remove(item);
            //}
            //var t = Convert.ToInt32(IP_List[0].LastIndexOf('.') + 1);

            IP_List = IP_List
                .OrderBy(x => Convert.ToInt32(x.Substring(x.LastIndexOf('.') + 1)))
                .ToList();

            return IP_List;




            ////string command = "/c ipconfig";
            //ProcessStartInfo cmdsi = new ProcessStartInfo("cmd.exe");
            ////cmdsi.Arguments = command;
            //cmdsi.RedirectStandardOutput = true;
            //cmdsi.RedirectStandardInput = true;
            //cmdsi.UseShellExecute = false;
            //Process cmd = Process.Start(cmdsi);
            //using (StreamWriter sw = cmd.StandardInput)
            //{
            //    if (sw.BaseStream.CanWrite)
            //    {
            //        sw.WriteLine("ipconfig");
            //        sw.WriteLine("arp -a");
            //    }
            //}

            //string output = cmd.StandardOutput.ReadToEnd();
            //cmd.WaitForExit();

            //string All_IPs = output.Substring(output.LastIndexOf("Interface: 192.168.0."));
            //string Local_IPs_Table = All_IPs.Substring(0, All_IPs.IndexOf("ff-ff-ff-ff-ff-ff"));
            //string Local_IPs = Local_IPs_Table.Substring(Local_IPs_Table.IndexOf("Type") + 6);

            //List<string> IP_List = new List<string>();

            //using (System.IO.StringReader reader = new System.IO.StringReader(Local_IPs))
            //{
            //    string t;
            //    while ((t = reader.ReadLine()) != null && t.Contains("c0-")) //c0- identifies it as a Torrcon for now
            //    {
            //        string line = t.Trim();
            //        IP_List.Add(line.Substring(0, line.IndexOf(" ")));
            //    }
            //}

            //#region connect to each IP and send test data
            ////List<string> TorrconIPList = new List<string>();
            ////foreach (string IP in IP_List)
            ////{
            ////    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////    // Connect to the specified host.
            ////    IPAddress iPAddress = System.Net.IPAddress.Parse(IP);
            ////    int sock = 502;
            ////    var endPoint = new IPEndPoint(iPAddress, sock);
            ////    clientSocket.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), s);
            ////    connectDone.WaitOne(5000);
            ////    if (clientSocket.Connected)
            ////    {
            ////        //read analog inputs by TCPIP
            ////        // Send test data to the remote device.
            ////        byte[] byteData = new byte[12];
            ////        byteData[0] = (byte)0;
            ////        byteData[1] = (byte)1;
            ////        byteData[2] = (byte)0;
            ////        byteData[3] = (byte)0;
            ////        byteData[4] = (byte)0;
            ////        byteData[5] = (byte)6;
            ////        byteData[6] = (byte)255;
            ////        byteData[7] = (byte)3;
            ////        byteData[8] = (byte)0;
            ////        byteData[9] = (byte)0;
            ////        byteData[10] = (byte)0;
            ////        byteData[11] = (byte)80;//number of words to read
            ////        try
            ////        {
            ////            int TempInt = 0;
            ////            s.SendTimeout = 50;
            ////            TempInt = s.Send(byteData, 12, SocketFlags.None);
            ////            if (TempInt == 12)
            ////            {
            ////                TorrconIPList.Add(IP);
            ////            }
            ////        }
            ////        catch (Exception e)
            ////        {
            ////            string z = e.Message;
            ////        }
            ////    }
            ////}
            //#endregion

            //return IP_List;
        }

        public List<string> VerifyIPsByMACAddress(List<string> ipAddresses)
        {
            List<string> TorrconIPs = new List<string>();

            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            strOutput = strOutput.Substring(strOutput.IndexOf("Type"));
            strOutput = strOutput.Substring(0, strOutput.IndexOf("ff-ff-ff-ff-ff-ff"));
            foreach (string ip in ipAddresses)
            {
                if (ip != null && strOutput.IndexOf(ip) != -1)
                {
                    string newStart = strOutput.Substring(strOutput.IndexOf(ip) + ip.Length);
                    int whiteSpaces = newStart.TakeWhile(c => char.IsWhiteSpace(c)).Count();
                    string macAddress = newStart.Substring(whiteSpaces, 17);
                    if (macAddress[0] == 'c' && macAddress[1] == '0') //c0 identifies it as a Torrcon for now
                    {
                        TorrconIPs.Add(ip);
                    }
                }
            }

            return TorrconIPs;
        }

        #region Public Properties

        /// <summary>
        /// Value of the TorrConIV Controller  (Convection gauge 1 pressure) formatted to match the display on the controller
        /// </summary>
        public override String FormattedValue
        {
            get
            {
                if (double.IsNaN(_gauge1_Pressure))
                {
                    return "ERROR";
                }
                else
                {
                    return string.Format("{0} {1}", string.Format(_formatProvider, "{0}", _gauge1_Pressure), (_gauge1_Pressure >= 1 ? "Torr" : "mTorr"));
                }
            }
        }

        //public Gauge Gauge1
        //{
        //    get { return _gauge1; }
        //}

        //public Gauge Gauge2
        //{
        //    get { return _gauge2; }
        //}

        #region Gauge 1 Public Properties

        public double Gauge1_Voltage
        {
            get { return _gauge1_Voltage; }
        }

        public double Gauge1_Pressure
        {
            get { return _gauge1_Pressure; }
        }

        public double Gauge1_Gain
        {
            get { return _gauge1_Gain; }
            set
            {
                _gauge1_Gain = value;
                _setModbusData = true;
                _writeGauge1Gain = true;
            }
        }

        public double Gauge1_Offset
        {
            get { return _gauge1_Offset; }
            set
            {
                _gauge1_Offset = value;
                _setModbusData = true;
                _writeGauge1Offset = true;
            }
        }

        public double Gauge1_UnitGain
        {
            get { return _gauge1_UnitGain; }
            set
            {
                _gauge1_UnitGain = value;
                _setModbusData = true;
                _writeGauge1UnitGain = true;
            }
        }

        public double Gauge1_UnitOffset
        {
            get { return _gauge1_UnitOffset; }
            set
            {
                _gauge1_UnitOffset = value;
                _setModbusData = true;
                _writeGauge1UnitOffset = true;
            }
        }

        public string Gauge1_Name
        {
            get { return _gauge1_Name; }
            set
            {
                if (value.Length != 0)
                {
                    if (value.Length <= 10)
                    {
                        _gauge1_Name = value;
                    }
                    else
                    {
                        _gauge1_Name = value.Substring(0, 10);
                    }
                    _setModbusData = true;
                    _writeGauge1Name = true;
                }
            }
        }

        public string Gauge1_Units
        {
            get { return _gauge1_Units; }
            set
            {
                if (value.Length != 0)
                {
                    if (value.Length <= 4)
                    {
                        _gauge1_Units = value;
                    }
                    else
                    {
                        _gauge1_Units = value.Substring(0, 4);
                    }
                    _setModbusData = true;
                    _writeGauge1Units = true;
                }
            }
        }

        public bool Gauge1_Enabled
        {
            get { return _gauge1_Enabled; }
            set
            {
                _gauge1_Enabled = value;
                _setModbusData = true;
                _writeGauge1Enabled = true;
            }
        }

        public bool Gauge1_IsConvection
        {
            get { return _gauge1_IsConvection; }
            set
            {
                _gauge1_IsConvection = value;
                _setModbusData = true;
                _writeGauge1IsConvection = true;
            }
        }

        public double Gauge1_Average
        {
            get { return _gauge1_Average; }
            set
            {
                if (value > 0 && value <= 255)
                {
                    _gauge1_Average = Convert.ToByte(value);
                    _setModbusData = true;
                    _writeGauge1Average = true;
                }
            }
        }

        #endregion

        #region Gauge 2 Public Properties

        public double Gauge2_Voltage
        {
            get { return _gauge2_Voltage; }
        }

        public double Gauge2_Pressure
        {
            get { return _gauge2_Pressure; }
        }

        public double Gauge2_Gain
        {
            get { return _gauge2_Gain; }
            set
            {
                _gauge2_Gain = value;
                _setModbusData = true;
                _writeGauge2Gain = true;
            }
        }

        public double Gauge2_Offset
        {
            get { return _gauge2_Offset; }
            set
            {
                _gauge2_Offset = value;
                _setModbusData = true;
                _writeGauge2Offset = true;
            }
        }

        public double Gauge2_UnitGain
        {
            get { return _gauge2_UnitGain; }
            set
            {
                _gauge2_UnitGain = value;
                _setModbusData = true;
                _writeGauge2UnitGain = true;
            }
        }

        public double Gauge2_UnitOffset
        {
            get { return _gauge2_UnitOffset; }
            set
            {
                _gauge2_UnitOffset = value;
                _setModbusData = true;
                _writeGauge2UnitOffset = true;
            }
        }

        public string Gauge2_Name
        {
            get { return _gauge2_Name; }
            set
            {
                if (value.Length != 0)
                {
                    if (value.Length <= 10)
                    {
                        _gauge2_Name = value;
                    }
                    else
                    {
                        _gauge2_Name = value.Substring(0, 10);
                    }
                    _setModbusData = true;
                    _writeGauge2Name = true;
                }
            }
        }

        public string Gauge2_Units
        {
            get { return _gauge2_Units; }
            set
            {
                if (value.Length != 0)
                {
                    if (value.Length <= 4)
                    {
                        _gauge2_Units = value;
                    }
                    else
                    {
                        _gauge2_Units = value.Substring(0, 4);
                    }
                    _setModbusData = true;
                    _writeGauge2Units = true;
                }
            }
        }

        public bool Gauge2_Enabled
        {
            get { return _gauge2_Enabled; }
            set
            {
                _gauge2_Enabled = value;
                _setModbusData = true;
                _writeGauge2Enabled = true;
            }
        }

        /// <summary>
        /// False is Voltage, True is 4-20 mA
        /// </summary>
        public bool Gauge2_OutputTypeIs4To20mA
        {
            get { return _gauge2_OutputTypeIs4To20mA; }
            set
            {
                _gauge2_OutputTypeIs4To20mA = value;
                _setModbusData = true;
                _writeGauge2OutputTypeIs4To20mA = true;
            }
        }

        public double Gauge2_Average
        {
            get { return _gauge2_Average; }
            set
            {
                if (value > 0 && value <= 255)
                {
                    _gauge2_Average = Convert.ToByte(value);
                    _setModbusData = true;
                    _writeGauge2Average = true;
                }
            }
        }

        #endregion

        #region Controller Public Properties

        public double SetPoint1
        {
            get { return _setpoint1; }
            set
            {
                _setpoint1 = value;
                _setModbusData = true;
                _writeSetpoint1 = true;
            }
        }

        public double SetPoint2
        {
            get { return _setpoint2; }
            set
            {
                _setpoint2 = value;
                _setModbusData = true;
                _writeSetpoint2 = true;
            }
        }

        /// <summary>
        /// True is Gauge 1, False is Gauge 2
        /// </summary>
        public bool Setpoint1IsGauge1
        {
            get { return _setpoint1IsGauge1; }
            set
            {
                _setpoint1IsGauge1 = value;
                _setModbusData = true;
                _writeSetpoint1IsGauge1 = true;
            }
        }

        /// <summary>
        /// True is Gauge 1, False is Gauge 2
        /// </summary>
        public bool Setpoint2IsGauge1
        {
            get { return _setpoint2IsGauge1; }
            set
            {
                _setpoint2IsGauge1 = value;
                _setModbusData = true;
                _writeSetpoint2IsGauge1 = true;
            }
        }

        /// <summary>
        /// True is screen rotation for DIN Rail mounting configuration.
        /// False is screen rotation for Torrcon-in-box configuration.
        /// </summary>
        public bool ScreenRotationIsDIN_Rail
        {
            get { return _screenRotationIsDIN_Rail; }
            set
            {
                _screenRotationIsDIN_Rail = value;
                _setModbusData = true;
                _writeScreenRotationIsDIN_Rail = true;
            }
        }

        //public string Password
        //{
        //    get { return _password; }
        //    //set
        //    //{
        //    //    if (_password.Length > 0 && _password.Length <= 4)
        //    //    {
        //    //        _password = value;
        //    //        _setModbusData = true;
        //    //        _writePassword = true;
        //    //    }
        //    //}
        //}

        public string ControllerName
        {
            get { return _controllerName; }
            set
            {
                if (value.Length != 0)
                {
                    if (value.Length <= 10)
                    {
                        _controllerName = value;
                    }
                    else
                    {
                        _controllerName = value.Substring(0, 10);
                    }
                    _setModbusData = true;
                    _writeControllerName = true;
                }
            }
        }

        /// <summary>
        /// True - return character is CR
        /// False - return character is CR plus LF
        /// </summary>
        public bool ReturnCharacterIsCR_Only
        {
            get
            {
                return _returnCharacterIsCR_Only;
            }
            //set
            //{
            //    if (value == true)
            //    {
            //        _returnCharacter = 10;
            //    }
            //    else
            //    {
            //        //_returnCharacterIsCR_Only = 10;
            //    }
            //    _setModbusData = true;
            //    _writeReturnCharacterIsCR_Only = true;
            //}
        }

        public bool DHCP_Enabled
        {
            get { return _DHCP_Enabled; }
            set
            {
                _DHCP_Enabled = value;
                _setModbusData = true;
                _writeDHCP_Enabled = true;
            }
        }

        public ushort SocketPortReadFromTorrcon
        {
            get { return _readSocketPort; }
        }

        public ushort SocketPortToWrite
        {
            set
            {
                try
                {
                    _socketPortToWrite = value;
                    _writeSocketPort = true;
                    _setModbusData = true;
                }
                catch (Exception e)
                {
                    string t = e.Message;
                }
            }
        }

        public string IP_AddressReadFromTorrcon
        {
            get { return _read_IP_Address; }
        }

        public string IP_AddressToWrite
        {
            set
            {
                try
                {
                    System.Net.IPAddress newIPAddress;
                    if (System.Net.IPAddress.TryParse(value, out newIPAddress) && value.Substring(0, 10) == "192.168.0.")
                    {
                        int hostID = Convert.ToInt32(value.Substring(value.LastIndexOf('.')));
                        if (hostID >= 0 && hostID < 31)
                        {
                            _IP_AddressToWrite = value;
                            _writeIP_Address = true;
                            _setModbusData = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    string t = e.Message;
                }
            }
        }

        //public string IP_Address
        //{
        //    //get { return System.Net.IPAddress.Parse(EthernetPortParameter.ProcessValue.IPAddress); }
        //    get { return _IP_Address; }
        //    set
        //    {
        //        System.Net.IPAddress newIPAddress;
        //        if (System.Net.IPAddress.TryParse(value, out newIPAddress))
        //        {
        //            _IP_Address = value;
        //            _setModbusData = true;
        //            _writeIP_Address = true;
        //        }
        //    }
        //}

        public string SubnetMask
        {
            get { return _subnetMask; }
            set
            {
                System.Net.IPAddress test;
                if (System.Net.IPAddress.TryParse(value, out test))
                {
                    _subnetMask = value;
                    _setModbusData = true;
                    _writeSubnetMask = true;
                }
            }
        }

        public string GatewayAddress
        {
            get { return _gatewayAddress; }
            set
            {
                System.Net.IPAddress test;
                if (System.Net.IPAddress.TryParse(value, out test))
                {
                    _gatewayAddress = value;
                    _setModbusData = true;
                    _writeGatewayAddress = true;
                }
            }
        }

        public double ModbusID
        {
            get { return _modbusID; }
            set
            {
                if (value > 0 && value <= 255)
                {
                    _modbusID = Convert.ToByte(value);
                    _setModbusData = true;
                    _writeModbusID = true;
                }
            }
        }

        //public double SocketPort
        //{
        //    get { return _socketPort; }
        //    set
        //    {
        //        if (value > 0 && value <= 255)
        //        {
        //            _socketPort = Convert.ToByte(value);
        //            _setModbusData = true;
        //            _writeSocketPort = true;
        //        }
        //    }
        //}

        #endregion

        //public override bool StopProcessing 
        //{ 
        //    get { return _stopProcessing; }
        //}

        /// <summary>
        /// Value (pressure) of the Controller
        /// </summary>
        public override double Value
        {
            get
            {
                //required to trigger ValueChanged event
                return _gauge1_Pressure;
            }
            internal set
            {
                //_signal = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Units for the value for the controller
        /// </summary>
        public override string Units { get; set; }

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
            get { return "TorrconIV"; }
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

        ///// <summary>
        ///// Returns true if the device is available
        ///// </summary>
        //public bool IsAvailable
        //{
        //    get { return isAvailable; }
        //}

        #endregion Public Properties
    }
}