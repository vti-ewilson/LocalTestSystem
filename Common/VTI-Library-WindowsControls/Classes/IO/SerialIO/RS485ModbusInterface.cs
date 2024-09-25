using System;
using System.ComponentModel;
using System.IO.Ports;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Implements a very basic ASCII-only RS-485 Modbus Interface.
    /// </summary>
    public partial class RS485ModbusInterface : Component
    {
        #region Enums

        /// <summary>
        /// RS-485 Function Codes
        /// </summary>
        public enum FunctionCodes
        {
#pragma warning disable 1591
            ReadCoilStatus = 1,
            ReadInputStatus = 2,
            ReadHoldingRegisters = 3,
            ReadInputRegisters = 4,
            WriteCoil = 5,
            WriteRegister = 6,
            WriteMultipleCoils = 15,
            WriteMultipleRegisters = 16
#pragma warning restore 1591
        }

        #endregion Enums

        #region Globals

        private System.Text.ASCIIEncoding asciiEnc = new System.Text.ASCIIEncoding();
        private SerialPortParameter serialPortParameter;
        private SerialPort serialPort1;
        private Object serialLock = new Object();

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RS485ModbusInterface">RS485ModbusInterface</see>.
        /// </summary>
        public RS485ModbusInterface()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RS485ModbusInterface">RS485ModbusInterface</see>.
        /// </summary>
        /// <param name="container">Container for the control</param>
        public RS485ModbusInterface(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion Construction

        #region Private Members

        /// <summary>
        /// Calculates the LRC check sum for a message
        /// </summary>
        /// <param name="message">Message to use to calculate the check sum</param>
        /// <returns>Check sum for the message</returns>
        private byte CalcCheckSum(string message)
        {
            int lrc = 0;
            for (int i = 0; i < message.Length / 2; i++)
                lrc += byte.Parse(message.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            lrc = (lrc & 0xFF ^ 0xFF) + 1;

            return (byte)lrc;
        }

        /// <summary>
        /// Sends a message over the RS-485 bus and returns the result
        /// </summary>
        /// <param name="slaveAddress">Address of the device</param>
        /// <param name="functionCode">Function Code</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="data">Data to send</param>
        /// <returns>Data returned from the device</returns>
        private byte[] SendMessage(byte slaveAddress, FunctionCodes functionCode, ushort startAddress, byte[] data)
        {
            string message, response = String.Empty;
            string strData = String.Empty;
            foreach (byte b in data) strData += (char)b;
            //foreach (byte b in data) strData += (char)b; //b.ToString("X2");
            //string strData = new string(data.Select(b => (char)b).ToArray());
            //string strData = new string(Encoding.Unicode.GetChars(data));
            message = String.Format("{0:X2}{1:X2}{2:X4}{3}", slaveAddress, (byte)functionCode, startAddress, strData);
            message = String.Format(":{0}{1:X2}", message, CalcCheckSum(message));
            lock (this.SerialPort)
            {
                Actions.Retry(3, delegate
                {
                    this.SerialPort.Write(message + "\r\n");
                    response = this.SerialPort.ReadTo("\r\n");
                });
            }
            if (String.IsNullOrEmpty(response)) throw new Exception("No data returned");
            // Check the returned checksum
            if (CalcCheckSum(response.Substring(1, response.Length - 3))
                != byte.Parse(response.Substring(response.Length - 2, 2), System.Globalization.NumberStyles.HexNumber))
                throw new Exception("Checksum error in response");
            // Check returned function code for error status
            byte retFunctionCode = byte.Parse(response.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            if (retFunctionCode == (byte)functionCode + 128)
                throw new Exception(String.Format("Error code returned by device: {0}", response.Substring(5, 2)));
            // create the array of returned data
            int i;
            int j = (response.Length - 3) / 2;  // half the length of the string ignoring the initial colon and the LRC
            byte[] retData = new byte[j];
            for (i = 0; i < j; i++)
                retData[i] = byte.Parse(response.Substring(i * 2 + 1, 2), System.Globalization.NumberStyles.HexNumber);
            return retData;
        }

        #endregion Private Members

        #region Public Properties

        /// <summary>
        /// Lock object to be used any time that serial I/O operations are performed
        /// </summary>
        public Object SerialLock
        {
            get { return serialLock; }
            set { serialLock = value; }
        }

        /// <summary>
        /// Serial port to use for the RS-485 connection
        /// </summary>
        /// <remarks>
        /// The serial port must be an RS-485 port or have an external RS-485 adapter installed.
        /// </remarks>
        public SerialPort SerialPort
        {
            get { return serialPort1; }
            set { serialPort1 = value; }
        }

        /// <summary>
        /// Serial port parameter to use to configure the serial port
        /// </summary>
        /// <remarks>
        /// Any changes to the SerialPortParameter will cause the serial port to automatically be updated.
        /// </remarks>
        public SerialPortParameter SerialPortParameter
        {
            get { return serialPortParameter; }
            set
            {
                if (SerialPort == null) SerialPort = new SerialPort();
                serialPortParameter = value;
                if (serialPortParameter.ProcessValue == null)
                {
                    //serialPortParameter.ProcessValue =
                    //  new SerialPortSettings(value.DisplayName,
                    //    value.BaudRate,
                    //    value.Parity,
                    //    value.DataBits,
                    //    value.StopBits,
                    //    value.Handshake);
                }
                serialPortParameter.ProcessValueChanged += new EventHandler(serialPortParameter_ProcessValueChanged);
                serialPortParameter_ProcessValueChanged(this, null);
            }
        }

        private void serialPortParameter_ProcessValueChanged(object sender, EventArgs e)
        {
            if (!serialPortParameter.ProcessValue.Equals(serialPort1))
            {
                try
                {
                    if (serialPort1.IsOpen) serialPort1.Close();
                    serialPortParameter.ProcessValue.CopyTo(serialPort1);
                    serialPort1.Open();
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteWarning(String.Format("Unable to open serial port {0} for the RS485 Modbus Interface.", serialPort1.PortName),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                        ee.ToString());
                }
            }
        }

        #endregion Public Properties

        #region Public Members

        /// <summary>
		/// Read from 1 to 2000 contiguous coils status.
		/// </summary>
		/// <param name="slaveAddress">Address of device to read values from.</param>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of coils to read.</param>
		/// <returns>Coils status</returns>
        public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            byte[] bytes = SendMessage(slaveAddress, FunctionCodes.ReadCoilStatus, startAddress,
                asciiEnc.GetBytes(numberOfPoints.ToString("X4")));
            bool[] retData = new bool[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
                retData[i] = ((bytes[i / 8 + 3] & (1 << i)) != 0);
            return retData;
        }

        /// <summary>
        /// Read from 1 to 2000 contiguous discrete input status.
        /// </summary>
        /// <param name="slaveAddress">Address of device to read values from.</param>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of discrete inputs to read.</param>
        /// <returns>Discrete inputs status</returns>
        public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            byte[] bytes = SendMessage(slaveAddress, FunctionCodes.ReadInputStatus, startAddress,
                asciiEnc.GetBytes(numberOfPoints.ToString("X4")));
            bool[] retData = new bool[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
                retData[i] = ((bytes[i / 8 + 3] & (1 << i)) != 0);
            return retData;
        }

        /// <summary>
        /// Read contiguous block of holding registers.
        /// </summary>
        /// <param name="slaveAddress">Address of device to read values from.</param>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of holding registers to read.</param>
        /// <returns>Holding registers status</returns>
        public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            byte[] bytes = SendMessage(slaveAddress, FunctionCodes.ReadHoldingRegisters, startAddress,
                asciiEnc.GetBytes(numberOfPoints.ToString("X4")));
            ushort[] retData = new ushort[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
                retData[i] = (ushort)(bytes[i + 3] * 256 + bytes[i + 4]);
            return retData;
        }

        /// <summary>
        /// Read contiguous block of input registers.
        /// </summary>
        /// <param name="slaveAddress">Address of device to read values from.</param>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of holding registers to read.</param>
        /// <returns>Input registers status</returns>
        public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            byte[] bytes = SendMessage(slaveAddress, FunctionCodes.ReadInputRegisters, startAddress,
                asciiEnc.GetBytes(numberOfPoints.ToString("X4")));
            ushort[] retData = new ushort[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
                retData[i] = (ushort)(bytes[i + 3] * 256 + bytes[i + 4]);
            return retData;
        }

        /// <summary>
        /// Write a single coil value.
        /// </summary>
        /// <param name="slaveAddress">Address of the device to write to.</param>
        /// <param name="coilAddress">Address to write value to.</param>
        /// <param name="value">Value to write.</param>
        public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
        {
            SendMessage(slaveAddress, FunctionCodes.WriteCoil, coilAddress,
                asciiEnc.GetBytes(value ? "FF00" : "0000"));
        }

        /// <summary>
        /// Write a single holding register.
        /// </summary>
        /// <param name="slaveAddress">Address of the device to write to.</param>
        /// <param name="registerAddress">Address to write.</param>
        /// <param name="value">Value to write.</param>
        public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
        {
            SendMessage(slaveAddress, FunctionCodes.WriteRegister, registerAddress,
                asciiEnc.GetBytes(value.ToString("X4")));
        }

        ///// <summary>
        ///// Write a block of 1 to 123 contiguous registers.
        ///// </summary>
        ///// <param name="slaveAddress">Address of the device to write to.</param>
        ///// <param name="startAddress">Address to begin writing values.</param>
        ///// <param name="data">Values to write.</param>
        //public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
        //{
        //}

        ///// <summary>
        ///// Force each coil in a sequence of coils to a provided value.
        ///// </summary>
        ///// <param name="slaveAddress">Address of the device to write to.</param>
        ///// <param name="startAddress">Address to begin writing values.</param>
        ///// <param name="data">Values to write.</param>
        //public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
        //{
        //}

        ///// <summary>
        ///// Performs a combination of one read operation and one write operation in a single Modbus transaction.
        ///// The write operation is performed before the read.
        ///// </summary>
        ///// <param name="slaveAddress">Address of device to read values from.</param>
        ///// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
        ///// <param name="numberOfPointsToRead">Number of registers to read.</param>
        ///// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
        ///// <param name="writeData">Register values to write.</param>
        //public ushort[] ReadWriteMultipleRegisters(byte slaveAddress, ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData)
        //{
        //}

        #endregion Public Members
    }
}