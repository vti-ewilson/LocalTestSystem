using iText.StyledXmlParser.Node;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using static VTIWindowsControlLibrary.Classes.IO.EthernetIO.PolyScienceAD15R30;

namespace VTIWindowsControlLibrary.Classes.IO.EthernetIO
{
    /// <summary>
    /// Serial Interface for a Controller
    /// </summary>
    public class PolyScienceAD15R30 : EthernetIOBase
    {
        #region Private Vars

        private bool _stopProcessing = true;
        private int _processDelay = 100;
        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();

        private EthernetPortParameter _socketPortParameter;
        private List<EthernetPortParameter> _targetPortParameters;

        private UdpClient _socket;
        private List<IPEndPoint> _targets = new List<IPEndPoint>();
        private List<DateTime> _lastUpdated = new List<DateTime>();
        private int _receiveTimeout = 100;
        private Byte[] _byteMessage;
        private string _message;

        private int _password = 100;
        private String _format = "0.00";
        private String _units;
        private float _min, _max;

        private List<double> _tempValues = new List<double>();
        private List<bool> _isExternalTemp = new List<bool>();
        private List<double> _setPoint = new List<double>();
        private List<bool> _isOn = new List<bool>();
        private List<int> _highAlarm = new List<int>();
        private List<int> _lowAlarm = new List<int>();
        private List<int> _pumpSpeed = new List<int>();
        private List<bool> _isFaulted = new List<bool>();
        private List<bool> _isRestarting = new List<bool>();
        private List<bool> _isPowered = new List<bool>();

        private List<bool> _getSetPoint = new List<bool>();
        private List<bool> _setSetPoint = new List<bool>();
        private List<bool> _getIsOn = new List<bool>();
        private List<bool> _setIsOn = new List<bool>();
        private List<bool> _getUnits = new List<bool>();
        private List<bool> _setUnits = new List<bool>();
        private List<bool> _getHighAlarm = new List<bool>();
        private List<bool> _setHighAlarm = new List<bool>();
        private List<bool> _getLowAlarm = new List<bool>();
        private List<bool> _setLowAlarm = new List<bool>();
        private List<bool> _getPumpSpeed = new List<bool>();
        private List<bool> _setPumpSpeed = new List<bool>();
        private List<bool> _getIsFaulted = new List<bool>();
        private List<bool> _setIsRestarting = new List<bool>();
        private List<bool> _getIsPowered = new List<bool>();

        #endregion Private Vars

        #region Event Handlers

        public override event EventHandler ValueChanged;
        public override event EventHandler RawValueChanged;

        #endregion Event Handlers

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PolyScienceAD15R30">PolyScienceAD15R30</see> class
        /// </summary>
        public PolyScienceAD15R30()
        {

        }

        #endregion

        #region Methods

        public override void Start()
        {
            for (int i = 0; i < _targetPortParameters.Count; i++)
            {
                if (i == 0)
                {
                    try
                    {
                        VtiEvent.Log.WriteVerbose(
                            String.Format($"Starting the processing thread for Ethernet PolyScience AD15R-30. Port: {_targetPortParameters[0].ProcessValue.Port}."),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);

                        backgroundWorker.WorkerReportsProgress = true;
                        backgroundWorker.DoWork += backgroundWorker_DoWork;

                        Connect();

                        backgroundWorker.RunWorkerAsync();
                    }
                    catch (Exception e)
                    {
                        VtiEvent.Log.WriteWarning(String.Format("Unable to start the PolyScience AD15R-30."),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO,
                            e.ToString());
                    }
                }

                addIpEndPoint();
            }
        }

        private void Connect()
        {
            _stopProcessing = false;

            _socket = new UdpClient(Int32.Parse(_socketPortParameter.ProcessValue.Port));
        }

        private void addIpEndPoint()
        {
            _targets.Add(new IPEndPoint(IPAddress.Parse(_targetPortParameters[_targets.Count].ProcessValue.IPAddress), Convert.ToInt32(_targetPortParameters[_targets.Count].ProcessValue.Port)));
            _lastUpdated.Add(DateTime.MinValue);
            
            _tempValues.Add(0.0);
            _isExternalTemp.Add(false);
            _setPoint.Add(0.0);
            _isOn.Add(false);
            _highAlarm.Add(0);
            _lowAlarm.Add(0);
            _pumpSpeed.Add(0);
            _isFaulted.Add(false);
            _isRestarting.Add(false);
            _isPowered.Add(false);

            _getSetPoint.Add(false);
            _setSetPoint.Add(false);
            _getIsOn.Add(false);
            _setIsOn.Add(false);
            _getUnits.Add(false);
            //_setUnits.Add(false);
            _getHighAlarm.Add(false);
            _setHighAlarm.Add(false);
            _getLowAlarm.Add(false);
            _setLowAlarm.Add(false);
            _getPumpSpeed.Add(false);
            _setPumpSpeed.Add(false);
            _getIsFaulted.Add(false);
            _setIsRestarting.Add(false);
            _getIsPowered.Add(false);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_stopProcessing)
            {
                for (int i = 0; i  < _targets.Count; i++)
                {
                    Thread.Sleep(_processDelay);

                    #region Get Temperature

                    #region Set Internal/External

                    try
                    {
                        Console.WriteLine($"Set Internal/External: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                        _message = SendReceiveMessage($"SJ{(_isExternalTemp[i] ? "1" : "0")}P{_password}\r", i);

                        if (!_message.Equals("!"))
                        {
                            Disconnect();
                            Connect();
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to Set Internal/External: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                    }

                    #endregion

                    #region Get Temp
                    try
                    {
                        Console.WriteLine($"Get Temperature: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                        if (_isExternalTemp[i])
                        {
                            _message = SendReceiveMessage("RR\r", i);
                        }
                        else
                        {
                            _message = SendReceiveMessage("RT\r", i);
                        }

                        if (_message.Equals("?") || _message.Equals("timed out"))
                        {
                            Disconnect();
                            Connect();
                            continue;
                        }
                        else
                        {
                            _tempValues[i] = Convert.ToDouble(_message);
                            _lastUpdated[i] = DateTime.Now;
                            if (i == 0)
                            {
                                this.Value = _tempValues[i];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to Get Temperature: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                    }
                    #endregion

                    #endregion

                    #region Read Set Point

                    if (_getSetPoint[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get Set Point: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RS\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setPoint[i] = Convert.ToDouble(_message);
                                _getSetPoint[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get Set Point: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Set Set Point

                    if (_setSetPoint[i])
                    {
                        try
                        {
                            Console.WriteLine($"Set Set Point to {_setPoint[i]}: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage($"SS{_setPoint[i]}P{_password}\r", i);

                            if (!_message.Equals("!"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setSetPoint[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Set Set Point: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Read On Off

                    if (_getIsOn[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get Operating Status: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RO\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _isOn[i] = Convert.ToBoolean(Int32.Parse(_message));
                                _getIsOn[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get Operating Status: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Set On Off

                    if (_setIsOn[i])
                    {
                        try
                        {
                            Console.WriteLine($"Set On Off to {_isOn[i]}: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage($"SO{(_isOn[i] ? "1" : "0")}P{_password}\r", i);

                            if (!_message.Equals("!"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setIsOn[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Set On Off: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Get Units

                    if (_getUnits[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get Units: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RU\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _units = _message;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get Units: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Set Units

                    if (_setUnits[i])
                    {
                        try
                        {
                            Console.WriteLine($"Set Units to {_units}: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage($"SU{_units}P{_password}\r", i);

                            if (!_message.Equals("!"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setUnits[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Set Units: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Read High Alarm

                    if (_getHighAlarm[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get High Alarm: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RH\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _highAlarm[i] = Int32.Parse(_message);
                                _getHighAlarm[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get High Alarm: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Set High Alarm

                    if (_setHighAlarm[i])
                    {
                        try
                        {
                            Console.WriteLine($"Set High Alarm to {_highAlarm[i]}: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage($"SH{_highAlarm[i]}P{_password}\r", i);

                            if (!_message.Equals("!"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setHighAlarm[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Set High Alarm: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Read Low Alarm

                    if (_getLowAlarm[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get Low Alarm: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RL\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _lowAlarm[i] = Int32.Parse(_message);
                                _getLowAlarm[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get Low Alarm: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Set Low Alarm

                    if (_setLowAlarm[i])
                    {
                        try
                        {
                            Console.WriteLine($"Set Low Alarm to {_lowAlarm[i]}: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage($"SL{_lowAlarm[i]}P{_password}\r", i);

                            if (!_message.Equals("!"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setLowAlarm[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Set Low Alarm: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Read Pump Speed

                    if (_getPumpSpeed[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get Pump Speed: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RM\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _pumpSpeed[i] = Int32.Parse(_message);
                                _getPumpSpeed[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get Pump Speed: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Set Pump Speed

                    if (_setPumpSpeed[i])
                    {
                        try
                        {
                            Console.WriteLine($"Set Pump Speed to {_pumpSpeed[i]}: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage($"SM{_pumpSpeed[i]}P{_password}\r", i);

                            if (!_message.Equals("!"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setPumpSpeed[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Set Pump Speed: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Read Alarm Status

                    if (_getIsFaulted[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get Alarm Status: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RF\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _isFaulted[i] = Convert.ToBoolean(Int32.Parse(_message));
                                _getIsFaulted[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get Alarm Status: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Set Restart Power

                    if (_setIsRestarting[i])
                    {
                        try
                        {
                            Console.WriteLine($"Set Restart Power to {_isRestarting[i]}: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage($"SW{(_isRestarting[i] ? "1" : "0")}P{_password}\r", i);

                            if (!_message.Equals("!"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _setIsRestarting[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Set Restart Power: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion

                    #region Read Power Status

                    if (_getIsPowered[i])
                    {
                        try
                        {
                            Console.WriteLine($"Get Power Status: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}");

                            _message = SendReceiveMessage("RW\r", i);

                            if (_message.Equals("?") || _message.Equals("timed out"))
                            {
                                Disconnect();
                                Connect();
                                continue;
                            }
                            else
                            {
                                _isPowered[i] = Convert.ToBoolean(Int32.Parse(_message));
                                _getIsPowered[i] = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to Get Power Status: {_targetPortParameters[i].ProcessValue.IPAddress}:{_targetPortParameters[i].ProcessValue.Port}, Exception: {ex}");
                        }
                    }

                    #endregion
                }
            }

            Disconnect();
        }

        private string SendReceiveMessage(string toSend, int targetIndex)
        {
            IPEndPoint endPoint = _targets[targetIndex];
            _byteMessage = Encoding.ASCII.GetBytes(toSend);
            _socket.Send(_byteMessage, _byteMessage.Length, endPoint);

            try
            {
                _socket.Client.ReceiveTimeout = _receiveTimeout;
                _byteMessage = _socket.Receive(ref endPoint);

                _message = Encoding.ASCII.GetString(_byteMessage);
                _message = _message.Remove(_message.Length - 1);
            }
            catch (Exception ex)
            {
                _message = "timed out";
                Console.WriteLine($"Operation Timed Out: {_targetPortParameters[targetIndex].ProcessValue.IPAddress}:{_targetPortParameters[targetIndex].ProcessValue.Port}, Exception: {ex}");
            }

            return _message;
        }

        private void Disconnect()
        {
            try
            {
                _socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Closed Socket: {_socketPortParameter.ProcessValue.Port}, Exception: {ex}");
            }
        }

        public override void Stop()
        {
            try
            {
                VtiEvent.Log.WriteVerbose(
                    String.Format("Stopping the processing thread for PolyScience AD15R-30"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
                _stopProcessing = true;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(String.Format("An error occurred stopping the PolyScience AD15R-30"),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO,
                    e.ToString());
            }
        }

        protected override void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        protected override void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Temperature of the Water Bath
        /// </summary>
        public override double Value
        {
            get
            {
                //required to trigger ValueChanged event
                return _tempValues[0];
            }
            internal set
            {
                _tempValues[0] = value;
                OnValueChanged();
            }
        }

        public double getValue(int targetIndex)
        {
            return _tempValues[targetIndex];
        }

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
        /// Formatted value including the units
        /// </summary>
        public override string Name
        {
            get { return "PolyScience AD15R-30"; }
        }

        /// <summary>
        /// No raw value for this device
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// MAximum value
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the temperature
        /// Either F or C
        /// </summary>
        public override string Units
        {
            get
            {
                return _units;
            }
            set
            {
                _units = value;
                for (int i = 0; i <  _targetPortParameters.Count; i++)
                {
                    _setUnits[i] = true;
                }
            }
        }

        /// <summary>
        /// Format string for the temperature
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

        public int getLength()
        {
            return _targets.Count();
        }

        public DateTime getLastUpdated(int targetIndex)
        {
            return _lastUpdated[targetIndex];
        }

        /// <summary>
        /// The IP address and port of the computer's Ethernet
        /// </summary>
        public EthernetPortParameter SocketPortParameter
        {
            get { return _socketPortParameter; }
            set
            {
                _socketPortParameter = value;
            }
        }

        /// <summary>
        /// The IP addresses and ports of each water bath's Ethernets
        /// </summary>
        public List<EthernetPortParameter> TargetPortParameters
        {
            get { return _targetPortParameters; }
            set
            {
                _targetPortParameters = value;
                for (int i = 0; i < _targetPortParameters.Count; i++)
                {
                    _setUnits.Add(false);
                }
            }
        }

        /// <summary>
        /// Whether the controller is using the internal or external tem.
        /// 1 if using External
        /// 0 if using Internal
        /// </summary>
        public bool getIsExternalTemp(int targetIndex)
        {
            return _isExternalTemp[targetIndex];
        }

        public void setIsExternalTemp(bool value, int targetIndex)
        {
            _isExternalTemp[targetIndex] = value;
        }

        /// <summary>
        /// Set Point of the controller. The desired temperature of the water bath
        /// </summary>
        public double getSetPoint(int targetIndex)
        {
            _getSetPoint[targetIndex] = true;
            DateTime start = DateTime.Now;
            while (_getSetPoint[targetIndex])
            {
                if ((DateTime.Now - start).TotalSeconds > 2)
                {
                    return Double.MinValue;
                }
            }
            return _setPoint[targetIndex];
        }

        public void setSetPoint(double value, int targetIndex)
        {
            if ((value >= -99.99) && (value <= 999.99))
            {
                _setPoint[targetIndex] = Math.Round(value, 2);
                _setSetPoint[targetIndex] = true;
            }
            else
            {
                VtiEvent.Log.WriteWarning(String.Format("PolyScience AD15R-30: Set Point out of range (-99.99-999.99). Could not be set."),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
            }
        }

        /// <summary>
        /// Whether or not the controller is on (running) or off (standby)
        /// 1 if On
        /// 0 if Off
        /// </summary>
        public bool getOnOff(int targetIndex)
        {
            _getIsOn[targetIndex] = true;
            while (_getIsOn[targetIndex]) ;
            return _isOn[targetIndex];
        }

        public void setOnOff(bool value, int targetIndex)
        {
            _isOn[targetIndex] = value;
            _setIsOn[targetIndex] = true;
        }


        /// <summary>
        /// Limits how high the Set Point Temperature can be set. Should be at least 5 degrees higher than the Set Point. 
        /// </summary>
        public int getHighAlarm(int targetIndex)
        {
            _getHighAlarm[targetIndex] = true;
            while (_getHighAlarm[targetIndex]) ;
            return _highAlarm[targetIndex];
        }

        public void setHighAlarm(int value, int targetIndex)
        {
            if ((value >= -99) && (value <= 999))
            {
                _highAlarm[targetIndex] = value;
                _setHighAlarm[targetIndex] = true;
            }
            else
            {
                VtiEvent.Log.WriteWarning(String.Format("PolyScience AD15R-30: High Alarm out of range (-99-999). Could not be set."),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
            }
        }

        /// <summary>
        /// Limits how low the Set Point Temperature can be set. Can be negative. Should be at least 5 degrees lower than the Set Point. 
        /// </summary>
        public int getLowAlarm(int targetIndex)
        {
            _getLowAlarm[targetIndex] = true;
            while (_getLowAlarm[targetIndex]) ;
            return _lowAlarm[targetIndex];
        }

        public void setLowAlarm(int value, int targetIndex)
        {
            if ((value >= -99) && (value <= 999))
            {
                _lowAlarm[targetIndex] = value;
                _setLowAlarm[targetIndex] = true;
            }
            else
            {
                VtiEvent.Log.WriteWarning(String.Format("PolyScience AD15R-30: Low Alarm out of range (-99-999). Could not be set."),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
            }
        }

        /// <summary>
        /// Speed of the pump
        /// </summary>
        public int getPumpSpeed(int targetIndex)
        {
            _getPumpSpeed[targetIndex] = true;
            while (_getPumpSpeed[targetIndex]) ;
            return _pumpSpeed[targetIndex];
        }

        public void setPumpSpeed(int value, int targetIndex)
        {
            if ((value >= -99) && (value <= 999))
            {
                _pumpSpeed[targetIndex] = value;
                _setPumpSpeed[targetIndex] = true;
            }
            else
            {
                VtiEvent.Log.WriteWarning(String.Format("PolyScience AD15R-30: Pump Speed out of range (5-100) or not a multiple of 5. Could not be set."),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Ethernet_IO);
            }
        }

        /// <summary>
        /// Indicates whether there are faults in the system.
        /// 1 if there are Faults
        /// 0 if there are No Faults
        /// </summary>
        public bool getAlarmStatus(int targetIndex)
        {
            _getIsFaulted[targetIndex] = true;
            while (_getIsFaulted[targetIndex]) ;
            return _isFaulted[targetIndex];
        }

        /// <summary>
        /// Sets how the unit will begin operating after a disruption in electrical power.
        /// 1 to automatically begin running when power is restored
        /// 0 to sit in Standby mode when power is restored
        /// </summary>
        public void setPumpSpeed(bool value, int targetIndex)
        {
            _isRestarting[targetIndex] = value;
            _setIsRestarting[targetIndex] = true;
        }

        /// <summary>
        /// Whether or not there is power to the machine.
        /// 1 if there is power
        /// No response if no power
        /// </summary>
        public bool getPowerStatus(int targetIndex)
        {
            _getIsPowered[targetIndex] = true;
            while (_getIsPowered[targetIndex]) ;
            return _isPowered[targetIndex];
        }

        #endregion Public Properties
    }
}