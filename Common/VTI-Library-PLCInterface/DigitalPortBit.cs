using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
//using MccDaq;

namespace VTIPLCInterface
{
    /// <summary>
    /// DigitalPortBit Class
    /// 
    /// Represents a Digital I/O Bit on a Measurement Computing Digital Port
    /// Properties: Name, Bit (number 0..7)
    /// </summary>
    public class DigitalPortBit
    {
        #region Globals

        private String _Name;
        private String _Description;
        private ushort _Bit;
        private ushort _BitValue; // 2 raised to the _Bit power
        private Boolean _Value;
        private DigitalPort _Port;

        #endregion

        #region Event Handlers

        internal event EventHandler ValueChanged;

        #endregion

        #region Internal Properties

        internal DigitalPort Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        /// Gets or sets the bit.
        /// </summary>
        /// <value>The bit.</value>
        public ushort Bit
        {
            get { return _Bit; }
            set 
            { 
                _Bit = value;
                _BitValue = (ushort)(1 << _Bit);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DigitalPortBit"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        internal Boolean Value
        {
            get { return _Value; }
            set {
                // Test if value changed from previous value
                Boolean bValueChanged = (_Value != value);
                // Assign the value to the private value
                _Value = value;
                // If the port is an output port, use the Measurement Computing DBitOut method to write the bit
                //if (_Port.DigitalPortDirection == DigitalPortDirection.DigitalOut)
                {
                    lock (MccConfig.MccLock)
                    {
                        // update the bit value at the port level, since the DBitOut method in MccDaq doesn't work right
                        if (_Port.Board.InvertedLogic ? !_Value : _Value)
                            _Port.Value = (ushort)(_Port.Value | _BitValue); // ON
                        else
                            _Port.Value = (ushort)(_Port.Value & (_Port.BitMask ^ _BitValue)); // OFF
                    }
                }

                // If there is an event attached to the ValueChanged event handler, call the event
                if (bValueChanged && (ValueChanged != null))
                    ValueChanged(this, null);
            }
        }

        #endregion
    }
}
