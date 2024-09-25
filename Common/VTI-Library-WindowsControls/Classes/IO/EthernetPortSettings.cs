using System;
using System.IO.Ports;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace VTIWindowsControlLibrary.Classes.IO.EthernetIO
{
    /// <summary>
    /// A class for saving settings for Ethernet ports.
    /// </summary>
    [Serializable]
    public class EthernetPortSettings
    {
        private string _PortName = "DeviceName";

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

        private string _IPAddress = "127.0.0.0";

        /// <summary>
        /// Gets or sets the IPAddress.
        /// </summary>
        /// <value>The IPAddress.</value>
        [XmlElement("IPAddress")]
        public string IPAddress
        {
            get
            {
                return _IPAddress;
            }
            set
            {
                _IPAddress = value;
            }
        }

        private string _Port = "80";

        /// <summary>
        /// Gets or sets the Port.
        /// </summary>
        /// <value>The Port.</value>
        [XmlElement("Port")]
        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthernetPortSettings"/> class.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        /// <param name="ipAddress">The ipAddress.</param>
        /// <param name="port">The port.</param>
        public EthernetPortSettings(string portName, string ipAddress, string port)
        {
            _PortName = portName;
            _IPAddress = ipAddress;
            _Port = port;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthernetPortSettings"/> class.
        /// </summary>
        public EthernetPortSettings()
        {
        }

        /// <summary>
        /// Compares two instances of the <see cref="EthernetPortSettings"/> class to
        /// determine if they are equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>A value indicating whether the two instances of the
        /// <see cref="EthernetPortSettings"/> class are equal.</returns>
        public static bool operator ==(EthernetPortSettings left, EthernetPortSettings right)
        {
            object n = null;

            //  Both are null.
            if (object.ReferenceEquals(n, left) && object.ReferenceEquals(n, right))
                return true;

            //  Neither are null and process values are equal.
            return (!object.ReferenceEquals(n, left) && !object.ReferenceEquals(n, right) &&
                left.PortName == right.PortName &&
                left.IPAddress == right.IPAddress &&
                left.Port == right.Port);
        }

        /// <summary>
        /// Compares two instances of the <see cref="EthernetPortSettings"/> class to
        /// determine if they are not equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>A value indicating whether the two instances of the
        /// <see cref="EthernetPortSettings"/> class are not equal.</returns>
        public static bool operator !=(EthernetPortSettings left, EthernetPortSettings right)
        {
            return (!(left == right));
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
            if (obj is SerialPort)
            {
                IPAddress ipaddress = System.Net.IPAddress.Parse(_IPAddress);
                int resultPort = 80;
                bool b = int.TryParse(_Port, out resultPort);
                System.Net.IPEndPoint ethernetPort = obj as System.Net.IPEndPoint;
                return (
                    ethernetPort.Address == ipaddress &&
                    ethernetPort.Port == resultPort);
            }
            else if (obj is EthernetPortSettings)
            {
                EthernetPortSettings ethernetPortSettings = obj as EthernetPortSettings;
                return (
                    ethernetPortSettings.PortName == _PortName &&
                    ethernetPortSettings.IPAddress == _IPAddress &&
                    ethernetPortSettings.Port == _Port);
            }
            else return base.Equals(obj);
        }
    }
}