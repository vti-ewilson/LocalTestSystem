using System;
using System.Diagnostics;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an TorrConII Controller
    /// </summary>
    public class TorrConIIController : SerialIOBase
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
        private Single _min = 0, _max = 1000;
        private String _units = "Torr";
        private String _format = "0.000";
        private Boolean _sp1Latch = true, _sp2Latch = true, _sp1ActiveHigh = true, _sp2ActiveHigh = false;
        private int _decimalPlaces = 3;

        private Stopwatch _errorSW = new Stopwatch();

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PVGC5Controller">PVGC5Controller</see> class
        /// </summary>
        public TorrConIIController()
          : base()
        {
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
            get { return "TorrConII Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        ///

        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the TorrCon II Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the Torr Con II controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the TorrCon II controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the TorrConII controller
        /// </summary>
        public override string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Setpoint 1 of the TorrCon II controller
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
                    VtiEvent.Log.WriteError("Error reading Setpoint 1 from TorrCon II Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
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
                    throw new Exception("Error setting Setpoint 1 on TorrCon II Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Setpoint 2 of the TorrCon II controller
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
                    throw new Exception("Error reading Setpoint 2 from TorrCon II Controller on port " + serialPort1.PortName);
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
                    throw new Exception("Error setting Setpoint 2 on TorrCon II Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        #endregion Public Properties
    }
}