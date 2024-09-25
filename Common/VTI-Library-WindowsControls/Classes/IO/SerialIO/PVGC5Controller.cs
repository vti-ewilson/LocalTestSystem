using System;
using System.Diagnostics;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an PVGC5 Controller
    /// </summary>
    public class PVGC5Controller : SerialIOBase
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

        private Double _pressure;
        private Single _min, _max;
        private String _units;
        private String _format = "0.00";
        private Boolean _sp1Latch = true, _sp2Latch = true, _sp1ActiveHigh = true, _sp2ActiveHigh = false;
        private int _decimalPlaces = 3;

        private Stopwatch _errorSW = new Stopwatch();

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PVGC5Controller">PVGC5Controller</see> class
        /// </summary>
        public PVGC5Controller(Single Min, Single Max, String Units)
          : base()
        {
            _min = Min;
            _max = Max;
            _units = Units;
            this.BaudRate = 2400;
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
                VtiEvent.Log.WriteError("Error sending configuration to PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
                Monitor.Exit(this.SerialLock);
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the PVGC5 Controller
        /// </summary>
        public override void Process()
        {
            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                try
                {
                    serialPort1.DiscardInBuffer();
                    Thread.Sleep(300);
                    serialPort1.Write("p");
                    _pressure = Convert.ToDouble(serialPort1.ReadLine());
                    if (!this.backgroundWorker1.IsBusy)
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                }
                catch (Exception e)
                {
                    _pressure = Double.NaN;
                    commError = true;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Resets the setpoint relays
        /// </summary>
        public void ResetRelays()
        {
            //            Monitor.Enter(this.SerialLock);
            //            try
            //            {
            //                if (!serialPort1.IsOpen) serialPort1.Open();
            //                serialPort1.Write("R");
            //            }
            //            catch (Exception e)
            //            {
            //                VtiEvent.Log.WriteError("Error reseting relays on PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            //            }
            //            finally
            //            {
            //                Monitor.Exit(this.SerialLock);
            //            }
        }

        /// <summary>
        /// Calculates a new <see cref="Offset">Offset</see> based on the current <see cref="Value">Value</see>
        /// </summary>
        public void AutoZero()
        {
            //            this.Offset = this.Offset - (Single)this.Value / this.Gain;
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
        /// Value (pressure) of the PVGC5 Controller
        /// </summary>
        public override double Value
        {
            get
            {
                //lock (SerialLock)
                //{
                //    if (_pressureReady)
                //    {
                return _pressure;
                //}
                //else
                //{
                //    try
                //    {
                //        if (!serialPort1.IsOpen) serialPort1.Open();
                //        serialPort1.Write("p");
                //        return Convert.ToDouble(serialPort1.ReadLine().Substring(2));
                //    }
                //    catch (Exception e)
                //    {
                //        if (_errorSW.IsRunning == false || _errorSW.Elapsed.Minutes >= 5)
                //        {
                //            VtiEvent.Log.WriteError("Error reading Pressure from PVGC5 on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //            if (_errorSW.IsRunning) _errorSW.Reset();
                //            _errorSW.Start();
                //        }
                //        return Double.NaN;
                //    }
                //}
                //}
            }
            internal set
            {
                _pressure = value;
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
        /// Name for the PVGC5 Controller
        /// </summary>
        public override string Name
        {
            get { return "PVGC5 Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the PVGC5 Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the PVGC5 controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the PVGC5 controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the PVGC5 controller
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
        /// Setpoint 1 of the PVGC5 controller
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
                    VtiEvent.Log.WriteError("Error reading Setpoint 1 from PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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
                    throw new Exception("Error setting Setpoint 1 on PVGC5 Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Setpoint 2 of the PVGC5 controller
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
                    throw new Exception("Error reading Setpoint 2 from PVGC5 Controller on port " + serialPort1.PortName);
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
                    throw new Exception("Error setting Setpoint 2 on PVGC5 Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Gain setting for the PVGC5 controller
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
                    VtiEvent.Log.WriteError("Error reading Gain from PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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
                    VtiEvent.Log.WriteError("Error setting Gain on PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Offset setting for the PVGC5 controller
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
                    VtiEvent.Log.WriteError("Error reading Offset from PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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
                    VtiEvent.Log.WriteError("Error setting Offset on PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Dac setting for the PVGC5 controller
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
                    VtiEvent.Log.WriteError("Error reading Dac from PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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
                    VtiEvent.Log.WriteError("Error setting Dac on PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// RelayStatus of the PVGC5 Controller
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
                    VtiEvent.Log.WriteError("Error reading Relay Status from PVGC5 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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
        /// Number of decimal places for the PVGC5 controller to display
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

        #endregion Public Properties
    }
}