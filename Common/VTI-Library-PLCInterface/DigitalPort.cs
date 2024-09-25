using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
//using MccDaq;

namespace VTIPLCInterface
{
    /// <summary>
    /// DigitalPort Class
    /// 
    /// Represents a port on a Measurement Computing digital board
    /// Properties: DigitalPortType (FirstPortA, SecondPortCL, etc.)
    ///             DigitalPortDirection (DigitalIn, DigitalOut)
    /// </summary>
    public class DigitalPort
    {
        #region Globals

        private VTIPLCInterface.MccDaq.DigitalPortType _DigitalPortType;
        private VTIPLCInterface.MccDaq.DigitalPortDirection _DigitalPortDirection;
        private List<DigitalPortBit> _DigitalPortBits;
        private DigitalBoard _Board;
        // Bit values for each of the digital outputs
        private ushort _value;
        // Bit mask for the number of outputs for this port
        // 0xFF for 8-bit ports (A & B), 0xF for 4-bit ports (CH & CL)
        private ushort _bitMask;
        internal System.Diagnostics.Stopwatch errorTimer;

        #endregion

        #region Internal Properties

        internal DigitalBoard Board
        {
            get { return _Board; }
            set { _Board = value; }
        }

        internal ushort Value
        {
            get { return _value; }
            set
            {
                _value = value;
                try
                {
                    if (_Board.BoardNum >= 10)
                    {
                        //set dig out by TCP IP
                        if (_DigitalPortType == MccDaq.DigitalPortType.FirstPortA)
                        {
                            byte[] byteData = new byte[13];
                            byteData[0] = 0;//transaction number msb
                            byteData[1] = 1;//transaction number lsb
                            byteData[2] = 0;//0 for modbus tcp
                            byteData[3] = 0;//0 for modbus tcp
                            byteData[4] = 0;//number of remaining bytes in this frame msb
                            byteData[5] = 6;//number of remaining bytes in this fram lsb
                            byteData[6] = 255;//slave address, 255 if not used
                            byteData[7] = 15;//function code 15 to write multiple coils
                            byteData[8] = 0x20;//start address msb modbus hex addressing
                            byteData[9] = 0x00;//start address lsb modbus hex addressing
                            byteData[10] = 0x00;//number of data point msb
                            byteData[11] = 0x08;//number of data points lsb
                            byteData[12] = (byte)_value;

                            //Send(_PLC, byteData);
                            //sendDone.WaitOne();

                            //ReadAndParseSocket(_PLC);
                        }
                    }
                    else
                    {
                        //_Board.MccBoard.DOut(_DigitalPortType, _value);
                    }
                }
                catch
                {

                }
            }
        }

        internal ushort BitMask
        {
            get { return _bitMask; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of the digital port.
        /// </summary>
        /// <value>The type of the digital port.</value>
        public VTIPLCInterface.MccDaq.DigitalPortType DigitalPortType
        {
            get { return _DigitalPortType; }
            set
            {
                _DigitalPortType = value;
                String s = _DigitalPortType.ToString();
                if (s.EndsWith("H") || s.EndsWith("L"))
                    _bitMask = 0xF;
                else
                    _bitMask = 0xFF;
            }
        }

        /// <summary>
        /// Gets or sets the digital port direction.
        /// </summary>
        /// <value>The digital port direction.</value>
        public VTIPLCInterface.MccDaq.DigitalPortDirection DigitalPortDirection
        {
            get { return _DigitalPortDirection; }
            set { _DigitalPortDirection = value; }
        }

        /// <summary>
        /// Gets or sets the digital port bits.
        /// </summary>
        /// <value>The digital port bits.</value>
        public List<DigitalPortBit> DigitalPortBits
        {
            get { return _DigitalPortBits; }
            set { _DigitalPortBits = value; }
        }

        #endregion
    }
}
