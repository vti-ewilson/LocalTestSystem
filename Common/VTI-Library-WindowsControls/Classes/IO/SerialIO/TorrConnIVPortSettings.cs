using System;
using System.IO.Ports;
using System.Xml;
using System.Xml.Serialization;

namespace VTIWindowsControlLibrary.Classes.IO.SErialIO
{
    /// <summary>
    /// A class for saving settings for serial ports.
    /// </summary>
    [Serializable]
    public class TorrconIVSerialPortSettings
    {
        private string _PortName = "COM1";
        private string _TorrConIVName = "Evacuation1";

        /// <summary>
        /// Gets or sets the name o
        /// </summary>
        /// <value>The name of the 
        [XmlElement("TorrConIVName")]
        public string TorrConIVName
        {
            get { return _TorrConIVName; }
            set { _TorrConIVName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the port.
        /// </summary>
        /// <value>The name of the port.</value>
        [XmlElement("PortName")]
        public string PortName
        {
            get { return _PortName; }
            set { _PortName = value; }
        }

        private int _BaudRate = 57600;

        /// <summary>
        /// Gets or sets the baud rate.
        /// </summary>
        /// <value>The baud rate.</value>
        [XmlElement("BaudRate")]
        public int BaudRate
        {
            get
            {
                return _BaudRate;
            }
            set
            {
                _BaudRate = value;
            }
        }

        private Parity _Parity = Parity.None;

        /// <summary>
        /// Gets or sets the parity.
        /// </summary>
        /// <value>The parity.</value>
        [XmlElement("Parity")]
        public Parity Parity
        {
            get
            {
                return _Parity;
            }
            set
            {
                _Parity = value;
            }
        }

        private int _DataBits = 8;

        /// <summary>
        /// Gets or sets the data bits.
        /// </summary>
        /// <value>The data bits.</value>
        [XmlElement("DataBits")]
        public int DataBits
        {
            get
            {
                return _DataBits;
            }
            set
            {
                _DataBits = value;
            }
        }

        private StopBits _StopBits = StopBits.One;

        /// <summary>
        /// Gets or sets the stop bits.
        /// </summary>
        /// <value>The stop bits.</value>
        [XmlElement("StopBits")]
        public StopBits StopBits
        {
            get
            {
                return _StopBits;
            }
            set
            {
                _StopBits = value;
            }
        }

        private Handshake _Handshake = Handshake.None;

        /// <summary>
        /// Gets or sets the handshake option.
        /// </summary>
        /// <value>The handshake option.</value>
        [XmlElement("Handshake")]
        public Handshake Handshake
        {
            get
            {
                return _Handshake;
            }
            set
            {
                _Handshake = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrconIVSerialPortSettings"/> class.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        /// <param name="torrConIVname">Name of the port.</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">The parity.</param>
        /// <param name="dataBits">The data bits.</param>
        /// <param name="stopBits">The stop bits.</param>
        /// <param name="handshake">The handshake.</param>
        public TorrconIVSerialPortSettings(string portName, string torrConIVname, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake)
        {
            _PortName = portName;
            _TorrConIVName = torrConIVname;
            _BaudRate = baudRate;
            _Parity = parity;
            _DataBits = dataBits;
            _StopBits = stopBits;
            _Handshake = handshake;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrconIVSerialPortSettings"/> class.
        /// </summary>
        public TorrconIVSerialPortSettings()
        {
        }

        /// <summary>
        /// Compares two instances of the <see cref="TorrconIVSerialPortSettings"/> class to
        /// determine if they are equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>A value indicating whether the two instances of the
        /// <see cref="TorrconIVSerialPortSettings"/> class are equal.</returns>
        public static bool operator ==(TorrconIVSerialPortSettings left, TorrconIVSerialPortSettings right)
        {
            object n = null;

            //  Both are null.
            if (object.ReferenceEquals(n, left) && object.ReferenceEquals(n, right))
                return true;

            //  Neither are null and process values are equal.
            return (!object.ReferenceEquals(n, left) && !object.ReferenceEquals(n, right) &&
                left.PortName == right.PortName &&
                left.TorrConIVName == right.TorrConIVName &&
                left.BaudRate == right.BaudRate &&
                left.Parity == right.Parity &&
                left.DataBits == right.DataBits &&
                left.StopBits == right.StopBits &&
                left.Handshake == right.Handshake);
        }

        /// <summary>
        /// Compares two instances of the <see cref="TorrconIVSerialPortSettings"/> class to
        /// determine if they are not equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>A value indicating whether the two instances of the
        /// <see cref="TorrconIVSerialPortSettings"/> class are not equal.</returns>
        public static bool operator !=(TorrconIVSerialPortSettings left, TorrconIVSerialPortSettings right)
        {
            return (!(left == right));
        }

        /// <summary>
        /// Copies the serial port settings to the specified serial port.
        /// </summary>
        /// <param name="serialPort">The serial port.</param>
        public void CopyTo(TorrconIVSerialPortSettings serialPort)
        {
            serialPort.PortName = _PortName;
            serialPort.TorrConIVName = _TorrConIVName;
            serialPort.BaudRate = _BaudRate;
            serialPort.Parity = _Parity;
            serialPort.DataBits = _DataBits;
            serialPort.StopBits = _StopBits;
            serialPort.Handshake = _Handshake;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _PortName.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is TorrconIVSerialPortSettings)
            {
                TorrconIVSerialPortSettings tcIVPort = obj as TorrconIVSerialPortSettings;
                return (
                    tcIVPort.PortName == _PortName &&
                    tcIVPort.TorrConIVName == _TorrConIVName &&
                    tcIVPort.BaudRate == _BaudRate &&
                    tcIVPort.Parity == _Parity &&
                    tcIVPort.DataBits == _DataBits &&
                    tcIVPort.StopBits == _StopBits &&
                    tcIVPort.Handshake == _Handshake);
            }
            else if (obj is TorrconIVSerialPortSettings)
            {
                TorrconIVSerialPortSettings TorrconIVSerialPortSettings = obj as TorrconIVSerialPortSettings;
                return (
                    TorrconIVSerialPortSettings.PortName == _PortName &&
                    TorrconIVSerialPortSettings.TorrConIVName == _TorrConIVName &&
                    TorrconIVSerialPortSettings.BaudRate == _BaudRate &&
                    TorrconIVSerialPortSettings.Parity == _Parity &&
                    TorrconIVSerialPortSettings.DataBits == _DataBits &&
                    TorrconIVSerialPortSettings.StopBits == _StopBits &&
                    TorrconIVSerialPortSettings.Handshake == _Handshake);
            }
            else return base.Equals(obj);
        }
    }
}