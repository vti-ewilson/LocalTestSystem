using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO.Ports;
using System.Threading;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.FormatProviders;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for a Barcode Scanner
    /// </summary>
    public partial class RFIDeasBadgeReader : SerialIOBase
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

        #endregion

        #region Globals

        private string _units = "";
        private string _format = "0";

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RFIDeasBadgeReader">RFIDeas Badge Reader</see> class
        /// </summary>
        public RFIDeasBadgeReader()
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RFIDeasBadgeReader">RFIDea sBadge Reader</see> class
        /// </summary>
        public RFIDeasBadgeReader(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.serialPort1.BaudRate = 19200;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RFIDeasBadgeReader">TorrConIV</see> class
        /// </summary>
        public RFIDeasBadgeReader(SerialPortParameter SerialPortParameter)
        {
            InitializeComponent();
            this.SerialPortParameter = SerialPortParameter;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Required thread - empty
        /// </summary>
        public override void Process()
        {
            
        }

        #endregion

        #region Events

        /// <summary>
        /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
        }

        #endregion

        #region Public Properties


        /// <summary>
        /// Name for the RFIDeas Badge Reader
        /// </summary>
        public override string Name
        {
            get { return "Badge Reader on port " + serialPort1.PortName; }
        }

        /// <summary>
        /// RawValue (Voltage) from the RFIDeasBadgeReader
        /// </summary>
        /// <remarks>
        /// This property is not implemented!
        /// </remarks>
        public override double RawValue
        {
            get { return 0; }
        }

        /// <summary>
        /// Minimum value for the RFIDeasBadgeReader
        /// </summary>
        /// <value>1.0E-3</value>
        public override double Min
        {
            get { return 0; }
        }

        /// <summary>
        /// Maximum value for the RFIDeasBadgeReader
        /// </summary>
        /// <value>1.0E3</value>
        public override double Max
        {
            get { return 0; }
        }

        /// <summary>
        /// Units for the value for the RFIDeasBadgeReader
        /// </summary>
        /// <value>torr</value>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the RFIDeasBadgeReader
        /// </summary>
        public override string Format
        {
            get { return _format; }
            set 
            { 
                _format = value;
                
            }
        }

        /// <summary>
        /// Value of the RFIDeasBadgeReader formatted to match the display on the controller
        /// </summary>
        public override String FormattedValue
        {
            get
            {
                if (commError)
                {
                    return "COM ERROR";
                }
                else
                {
                    return "IDLE";
                }
            }
        }

        public override double Value
        {
            get { return 0; }
            internal set
            {
                OnValueChanged();
            }
        }

        #endregion
    }
}
