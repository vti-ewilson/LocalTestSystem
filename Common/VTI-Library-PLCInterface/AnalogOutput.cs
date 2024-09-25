using System;
using System.Collections.Generic;
using System.Text;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIPLCInterface
{
    /// <summary>
    /// AnalogOutput Class
    /// 
    /// Implements the IAnalogOutput interface
    /// 
    /// Takes a value in arbitrary units and calculates an integer
    /// value to pass to the MccDaq AOut method.
    /// </summary>
    public class AnalogOutput : IAnalogOutput
    {
        #region Globals

        private String _Name;
        private String _Format;
        private AnalogChannel _AnalogChannel;
        private Double _Min;
        private Double _Max;
        private int _NumberOfBits;
        private String _Units;
        private Double _value;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogOutput"/> class.
        /// </summary>
        /// <param name="Channel">The channel.</param>
        public AnalogOutput(AnalogChannel Channel)
        {
            _Name = Channel.Name;
            _AnalogChannel = Channel;

            // Process the Range property to determine the Units, Min, and Max
            String sRange = _AnalogChannel.Range.ToString();
            // Range is milliamps (ex. Ma4To20)
            if (sRange.Substring(0, 2) == "Ma")
            {
                _Units = "mA";
                // Strip off the "Ma" part
                sRange = sRange.Substring(2);
                // Split the string around the "To" part
                String[] result = sRange.Split(new String[] { "to" }, StringSplitOptions.None);
                // Get the Min and Max, converting "Pt" to "." (ex 2Pt5 == 2.5)
                _Max = Convert.ToDouble(result[0].Replace("Pt", "."));
                _Min = Convert.ToDouble(result[1].Replace("Pt", "."));
            }
            // Range is volts
            else
            {
                _Units = "V";
                // Get the Max by stripping out the "Bip", "Uni", and "Volts" bits, and replacing "Pt" with "."
                _Max = Convert.ToDouble(sRange.Replace("Bip", "").Replace("Uni", "").Replace("Volts", "").Replace("Pt", "."));
                // If range is bipolar, Min == -Max, else Min = 0
                if (sRange.Substring(0, 3) == "Bip")
                    _Min = -_Max;
                else
                    _Min = 0;
            }
            // Get the number of bits
            _NumberOfBits = _AnalogChannel.Board.NumberOfBits;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the I/O.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Gets the analog channel.
        /// </summary>
        /// <value>The analog channel.</value>
        public AnalogChannel AnalogChannel
        {
            get { return _AnalogChannel; }
        }

        /// <summary>
        /// Gets the format string to be used for displaying the value of the analog I/O
        /// </summary>
        /// <value></value>
        public string Format
        {
            get { return _Format; }
            set { _Format = value; }
        }

        /// <summary>
        /// Gets the maximum value of the analog I/O
        /// </summary>
        /// <value></value>
        public double Max
        {
            get { return _Max; }
        }

        /// <summary>
        /// Gets the minimum value of the analog I/O.
        /// </summary>
        /// <value></value>
        public double Min
        {
            get { return _Min; }
        }

        /// <summary>
        /// Gets the units of the value (i.e. volts, mA, etc.)
        /// </summary>
        /// <value></value>
        public string Units
        {
            get { return _Units; }
        }

        /// <summary>
        /// Value of the analog output
        /// </summary>
        /// <value></value>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _AnalogChannel.Value = (float)value;
                _value = value;
                if (_AnalogChannel.Board.BoardNum < 10)
                {
                    //lock (MccConfig.MccLock)
                    //{
                    //    _AnalogChannel.Board.MccBoard.AOut(_AnalogChannel.Channel, _AnalogChannel.Range,
                    //        Convert.ToInt16(((_value - _Min) / (_Max - _Min)) * (1 << _NumberOfBits)));
                    //}
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this I/O instance is available.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this I/O instance is available; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailable
        {
            get { return true; }
        }

        #endregion
    }
}
