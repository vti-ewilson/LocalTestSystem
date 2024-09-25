using System;
using System.Collections.Generic;
using System.Text;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using AdvancedHMIDrivers;
//using MccDaq;

namespace VTIPLCInterface
{
    /// <summary>
    /// AnalogInput Class
    /// 
    /// Implementes the IAnalogInput interface.
    /// 
    /// Takes the integer value returned by the MccDaq AIn method and
    /// calculates the value in the arbitrary units of the AnalogInput
    /// </summary>
    public class AnalogInput : IAnalogInput
    {
        #region Globals

        private String _Name;
        private String _Format;
        private AnalogChannel _AnalogChannel;
        private Double _Min;
        private Double _Max;
        private int _NumberOfBits;
        private String _Units;
        private Double _RawValue;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogInput"/> class.
        /// </summary>
        /// <param name="Channel">The channel.</param>
        public AnalogInput(AnalogChannel Channel)
        {
            _Name = Channel.Name;
            _AnalogChannel = Channel;

            // Create an event handler to calculate the RawValue (volts or mA) when the
            // value from the analog channel changes
            _AnalogChannel.ValueChanged += new EventHandler(_AnalogChannel_ValueChanged);

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

        #region Event Handlers

        /// <summary>
        /// Occurs when the raw value of the analog input changes
        /// </summary>
        public event EventHandler RawValueChanged;

        #endregion

        #region Events

        /// <summary>
        /// AnalogChannel_ValueChanged event
        /// 
        /// Calculates the RawValue (volts or mA) from the analog channel value
        /// The Analog Channel value is an integer from 0 to 2^NumBits-1 (ie, a 12-bit input is 0..4095)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _AnalogChannel_ValueChanged(object sender, EventArgs e)
        {
            // Calculate the RawValue (volts or mA)
            _RawValue = ((Double)_AnalogChannel.Value / (Double)(1 << _NumberOfBits)) * (_Max - _Min) + _Min;
            // If there is an event attached to the RawValueChanged event handler, then call the event
            if (RawValueChanged != null) RawValueChanged(this, null);
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
            set { _Name = value; }
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
        /// Raw Value (i.e. volts, amps, etc.) of the analog input
        /// </summary>
        /// <value></value>
        public double RawValue
        {
            get { return _RawValue; }
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
