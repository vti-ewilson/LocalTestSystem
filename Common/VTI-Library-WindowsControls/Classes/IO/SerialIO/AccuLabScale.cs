using System;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an AccuLabScale
    /// </summary>
    public class AccuLabScale : SerialIOBase
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

        private Single _max;
        private int _decimals;
        private String _units;
        private Single _weight;
        private Boolean _weightReady;
        private String _format;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AccuLabScale">AccuLabScale</see> class
        /// </summary>
        /// <param name="Max">Maximum weight for the scale</param>
        /// <param name="Decimals">Number of decimal places reported by the scale</param>
        /// <param name="Units">Units for the weight measurement</param>
        public AccuLabScale(Single Max, int Decimals, String Units)
            : base()
        {
            _max = Max;
            _decimals = Decimals;
            _units = Units;
            _format = String.Format("0.{0:D" + _decimals.ToString() + "}", 0);
            this.serialPort1.BaudRate = 1200;
            this.serialPort1.Parity = System.IO.Ports.Parity.Odd;
            this.serialPort1.DataBits = 7;
            this.serialPort1.StopBits = System.IO.Ports.StopBits.One;
            this.serialPort1.DtrEnable = true;
        }

        #endregion Construction

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> (position) of the AccuLabScale
        /// </summary>
        public override void Process()
        {
            String s;
            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                try
                {
                    serialPort1.Write(String.Format("{0}P", (char)27));
                    s = serialPort1.ReadLine();
                    if (s.Contains(" g"))
                    {
                        _weight = Convert.ToSingle(s.Substring(0, s.IndexOf(" g")).Trim().Replace(" ", ""));
                        _weightReady = true;
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                    }
                }
                catch
                {
                    _weightReady = false;
                    //commError = true;
                }
                finally
                {
                    Monitor.Exit(SerialLock);
                }
            }
        }

        /// <summary>
        /// Method for processing events inside the <see cref="Process">Process</see> thread.
        /// This method runs outside of the <see cref="Process">Process</see> thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
        }

        /// <summary>
        /// Sends a command to zero the scale
        /// </summary>
        public void ZeroScale()
        {
            Monitor.Enter(SerialLock);
            try
            {
                serialPort1.Write(String.Format("{0}T", (char)27));
            }
            catch
            {
            }
            finally
            {
                Monitor.Exit(SerialLock);
            }
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Value (weight) reported by the scale
        /// </summary>
        public override double Value
        {
            get
            {
                if (_weightReady)
                    return _weight;
                else
                    return Single.NaN;
            }
            internal set
            {
                _weight = (Single)value;
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
                if (commError) return "ERROR";
                else if (_weightReady) return _weight.ToString(_format);
                else return "NOT READY";
            }
        }

        /// <summary>
        /// Name for the AccuLabScale
        /// </summary>
        public override string Name
        {
            get { return "AccuLab Scale Controller on port " + this.PortName; }
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value of the AccuLabScale
        /// </summary>
        /// <remarks>Always returns zero</remarks>
        public override double Min
        {
            get { return 0; }
        }

        /// <summary>
        /// Maximum value of the AccuLabScale
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the AccuLabScale
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// String format for displaying the value of the AccuLabScale
        /// </summary>
        public override string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Indicates if the weight value is ready
        /// </summary>
        public Boolean WeightReady
        {
            get { return _weightReady; }
        }

        #endregion Public Properties
    }
}