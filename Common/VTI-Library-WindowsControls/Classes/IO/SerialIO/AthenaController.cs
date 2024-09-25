using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an Athena Controller
    /// </summary>
    public class AthenaController : SerialIOBase
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

        #endregion Event Handlers

        #region Globals

        private Double _temperature;
        private Single _min, _max;
        private String _units;
        private String _format = "0.00";
        private Boolean _sp1Latch = true, _sp2Latch = true, _sp1ActiveHigh = true, _sp2ActiveHigh = false;
        private int _decimalPlaces = 3;
        private byte channelID = 1;
        private char[] charStr = new char[1000];

        private Stopwatch _errorSW = new Stopwatch();

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaController">AthenaController</see> class
        /// </summary>
        ///
        public AthenaController(Single Min, Single Max, String Units)
          : base()
        {
            _min = Min;
            _max = Max;
            _units = Units;
            this.BaudRate = 9600;
            _format = "0.0";
            serialPort1.NewLine = System.Char.ConvertFromUtf32(13);
        }

        public AthenaController(Single Min, Single Max, String Units, String Format)
        {
            _min = Min;
            _max = Max;
            _units = Units;
            this.BaudRate = 9600;
            _format = Format;
            serialPort1.NewLine = System.Char.ConvertFromUtf32(13);
        }

        #endregion Construction

        #region Private Methods

        private void SendWithDelay(String Text)
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            for (int i = 0; i < Text.Length; i++)
            {
                serialPort1.Write(Text.Substring(i, 1));
                Thread.Sleep(25);
            }
        }

        private String RecvWithDelay()
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            Array.Clear(charStr, 0, charStr.Length - 1);
            for (int ii = 0; ii < 100; ii++)
            {
                serialPort1.Read(charStr, ii, 1);
                Thread.Sleep(25);
            }
            return charStr.ToString();
        }

        private void SendConfig()
        {
            int iConfig;
            String sConfig;

            iConfig = 1 << _decimalPlaces;
            if (_sp1Latch) iConfig += 256;
            if (!_sp1ActiveHigh) iConfig += 512;
            if (_sp2Latch) iConfig += 1024;
            if (!_sp2ActiveHigh) iConfig += 2048;
            sConfig = String.Format(">{0:0}\r", iConfig);
            Monitor.Enter(this.SerialLock);
            try
            {
                serialPort1.DiscardInBuffer();
                SendWithDelay(sConfig);
                String s = serialPort1.ReadLine(); // throw away the return value
            }
            catch (Exception e)
            {
                commError = true;
                VtiEvent.Log.WriteError("Error sending configuration to Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
                Monitor.Exit(this.SerialLock);
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the Athena Controller
        /// </summary>
        public override void Process()
        {
            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                string recvStr;
                try
                {
                    serialPort1.DiscardInBuffer();
                    string sendStr = "0" + String.Format("{0}", channelID) + "01R05";
                    string crcStr = GetAthenaCheckSum(sendStr);
                    if (crcStr == "0" || crcStr == null)
                        return;
                    string writeStr = "$" + sendStr + crcStr.Trim() + Char.ConvertFromUtf32(13);
                    serialPort1.Write(writeStr);
                    recvStr = serialPort1.ReadLine();
                    string dblStr = recvStr.Substring(8, 7);
                    double dbl = Convert.ToDouble(dblStr);
                    // check for negative Athena temperature
                    if (recvStr.Contains("r"))
                        dbl = -dbl;
                    if (dbl != 0)
                    {
                        if (_units.Contains("K"))
                            _temperature = dbl + 273.15;
                        else if (_units.Contains("R"))
                            _temperature = dbl + 459.67;
                        else if (_units.Contains("C") || _units.Contains("F"))
                            _temperature = dbl;
                    }
                    else
                        _temperature = 0;
                    if (!this.backgroundWorker1.IsBusy)
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                }
                catch (Exception e)
                {
                    _temperature = Double.NaN;
                    commError = true;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Compute Check Sum for command being sent to Athena Controller
        /// </summary>
        protected string GetAthenaCheckSum(string str)
        {
            string retStr = "";
            int asciiSum = 0, strLength = str.Length;
            if (str != null)
                for (int ii = 0; ii < strLength; ii++)
                {
                    asciiSum += (int)Encoding.ASCII.GetBytes(str.Substring(ii, 1))[0];
                }
            if (asciiSum > 0)
            {
                int mod = asciiSum % 256;
                string tempStr = mod.ToString(), tempStr2;
                if (tempStr.Length > 2)
                {
                    tempStr2 = tempStr.Substring(0, 2);
                    int offset = 55 + Convert.ToInt32(tempStr2);
                    char tempChar = Convert.ToChar(offset);
                    tempStr2 = Convert.ToString(tempChar);
                }
                else
                    tempStr2 = null;
                retStr = tempStr2 + tempStr.Substring(tempStr.Length - 1, 1);
            }
            else
                retStr = "0";
            return retStr;
        }

        /// <summary>
        /// Resets the setpoint relays
        /// </summary>
        public void ResetRelays()
        {
            Monitor.Enter(this.SerialLock);
            try
            {
                if (!serialPort1.IsOpen) serialPort1.Open();
                serialPort1.Write("R");
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error reseting relays on Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
                Monitor.Exit(this.SerialLock);
            }
        }

        /// <summary>
        /// Calculates a new <see cref="Offset">Offset</see> based on the current <see cref="Value">Value</see>
        /// </summary>
        public void AutoZero()
        {
            this.Offset = this.Offset - (Single)this.Value / this.Gain;
        }

        /// <summary>
        /// Sends raw text <see cref="Offset">Text</see> out the serial port
        /// </summary>
        public void SendRawTextWithDelay(String Text)
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            for (int i = 0; i < Text.Length; i++)
            {
                serialPort1.Write(Text.Substring(i, 1));
                Thread.Sleep(25);
            }
        }

        #endregion Public Methods

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
        /// Value (Temperature) of the Athena Controller
        /// </summary>
        public override double Value
        {
            get
            {
                return _temperature;
            }
            internal set
            {
                _temperature = value;
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
                    return this.Value.ToString(_format) + " " + this.Units;
            }
        }

        /// <summary>
        /// Name for the Athena Controller
        /// </summary>
        public override string Name
        {
            get { return "Athena Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the Athena Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the Athena controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the Athena controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the Athena controller
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
        /// Setpoint 1 of the Athena controller
        /// </summary>
        public Single Setpoint1
        {
            get
            {
                Single temp = Single.NaN;
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("1");
                    temp = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error reading Setpoint 1 from Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return temp;
            }
            set
            {
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    SendWithDelay(string.Format("!{0:0.000}\r", value));
                    String s = serialPort1.ReadLine(); // throw away the return value
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error setting Setpoint 1 on Athena Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Setpoint 2 of the Athena controller
        /// </summary>
        public Single Setpoint2
        {
            get
            {
                Single temp;
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("2");
                    temp = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error reading Setpoint 2 from Athena Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return temp;
            }
            set
            {
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    SendWithDelay(string.Format("@{0:0.000}\r", value));
                    String s = serialPort1.ReadLine(); // throw away the return value
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error setting Setpoint 2 on Athena Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Gain setting for the Athena controller
        /// </summary>
        public Single Gain
        {
            get
            {
                Single Value = Single.NaN;
                Monitor.Enter(this.SerialLock);
                try
                {
                    //if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("g");
                    Value = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error reading Gain from Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }

                return Value;
            }
            set
            {
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    SendWithDelay(string.Format("G{0:0.000}\r", value));
                    String s = serialPort1.ReadLine(); // throw away the return value
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error setting Gain on Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Offset setting for the Athena controller
        /// </summary>
        public Single Offset
        {
            get
            {
                Single temp = Single.NaN;
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("o");
                    temp = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error reading Offset from Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return temp;
            }
            set
            {
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    SendWithDelay(string.Format("O{0:0.000}\r", value));
                    String s = serialPort1.ReadLine(); // throw away the return value
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error setting Offset on Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Dac setting for the Athena controller
        /// </summary>
        public Single Dac
        {
            get
            {
                Single Value = Single.NaN;
                Monitor.Enter(this.SerialLock);
                try
                {
                    //if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("d");
                    Value = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error reading Dac from Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }

                return Value;
            }
            set
            {
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    SendWithDelay(string.Format("D{0:0}\r", value));
                    String s = serialPort1.ReadLine(); // throw away the return value
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error setting Dac on Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// RelayStatus of the Athena Controller
        /// </summary>
        /// <remarks>
        /// The Relay Status is a two character string with each character being either a
        /// 0 or a 1 to indicate the status of each relay.
        /// </remarks>
        public String RelayStatus
        {
            get
            {
                String s = string.Empty;
                Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("r");
                    s = serialPort1.ReadLine();
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error reading Relay Status from Athena Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
                return s;
            }
        }

        /// <summary>
        /// Indicates whether or not Setpoint 1 will latch when hit
        /// </summary>
        public Boolean SP1Latch
        {
            get { return _sp1Latch; }
            set
            {
                _sp1Latch = value;
                if (!this.DesignMode) this.SendConfig();
            }
        }

        /// <summary>
        /// Indicates whether or not Setpoint 2 will latch when hit
        /// </summary>
        public Boolean SP2Latch
        {
            get { return _sp2Latch; }
            set
            {
                _sp2Latch = value;
                if (!this.DesignMode) this.SendConfig();
            }
        }

        /// <summary>
        /// Indicates if Setpoint 1 is active when the presure is above the setpoint
        /// </summary>
        /// <remarks>
        /// <para>If <see cref="SP1ActiveHigh">SP1ActiveHigh</see> is true, the Setpoint 1 relay will
        /// turn on if the pressure is above the setpoint.</para>
        /// <para>If <see cref="SP1ActiveHigh">SP1ActiveHigh</see> is false, the Setpoint 1 relay will
        /// turn on if the pressure is below the setpoint.</para>
        /// </remarks>
        public Boolean SP1ActiveHigh
        {
            get { return _sp1ActiveHigh; }
            set
            {
                _sp1ActiveHigh = value;
                if (!this.DesignMode) this.SendConfig();
            }
        }

        /// <summary>
        /// Indicates if Setpoint 2 is active when the presure is above the setpoint
        /// </summary>
        /// <remarks>
        /// <para>If <see cref="SP2ActiveHigh">SP2ActiveHigh</see> is true, the Setpoint 2 relay will
        /// turn on if the pressure is above the setpoint.</para>
        /// <para>If <see cref="SP2ActiveHigh">SP2ActiveHigh</see> is false, the Setpoint 2 relay will
        /// turn on if the pressure is below the setpoint.</para>
        /// </remarks>
        public Boolean SP2ActiveHigh
        {
            get { return _sp2ActiveHigh; }
            set
            {
                _sp2ActiveHigh = value;
                if (!this.DesignMode) this.SendConfig();
            }
        }

        /// <summary>
        /// Number of decimal places for the Athena controller to display
        /// </summary>
        public int DecimalPlaces
        {
            get { return _decimalPlaces; }
            set
            {
                _decimalPlaces = value;
                if (_decimalPlaces < 0) _decimalPlaces = 0;
                if (_decimalPlaces > 3) _decimalPlaces = 3;
                if (!this.DesignMode) this.SendConfig();
            }
        }

        /// <summary>
        /// channel ID Address for the athena
        /// </summary>
        public byte ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        #endregion Public Properties
    }
}