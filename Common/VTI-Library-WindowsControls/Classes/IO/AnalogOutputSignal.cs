using System;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Represents an analog output.  The Value is given in arbitrary
    /// units and is converted to the units of the analog output.
    /// The units of the analog output are determined by the IO Interface
    /// configuration and are application independent.
    /// </summary>
    public class AnalogOutputSignal
    {
        #region Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> changes
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        #endregion Event Handlers

        #region Globals

        private String _key;
        private String _label;
        private String _units = string.Empty;
        private String _format = "0.00";
        private Double _fullscale;
        private Double _offset;
        private Double _value;
        private IAnalogOutput _analogOutput;

        #endregion Globals

        #region Construction

        /// <param name="Label">Label for display purposes</param>
        /// <param name="Units">Units of the analog output (i.e. psi, Torr, etc.)</param>
        /// <param name="Format">Format string to be used when displaying the value</param>
        /// <param name="Fullscale">Full scale range of the analog output signal</param>
        /// <param name="Offset">Zero offset of the analog output signal</param>
        /// <param name="AnalogOutput">Raw <see cref="IAnalogOutput">AnalogOutput</see> that this signal is related to</param>
        public AnalogOutputSignal(String Label, String Units, String Format, Double Fullscale, Double Offset, IAnalogOutput AnalogOutput)
        {
            _key = Label.Replace(" ", string.Empty);
            _label = Label;
            _units = Units;
            _format = Format;
            _fullscale = Fullscale;
            _offset = Offset;
            _analogOutput = AnalogOutput;
        }

        #endregion Construction

        #region Public Properties

        /// <summary>
        /// Key value is calculated internally to be equal to the <see cref="Label">Label</see> without any spaces
        /// </summary>
        public String Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Display Label
        /// </summary>
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        /// <summary>
        /// Units (i.e. psi, Torr, etc.) of the analog output signal
        /// </summary>
        public String Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Display format string for the analog output signal
        /// </summary>
        public String Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Full scale range of the analog output signal
        /// </summary>
        public Double Fullscale
        {
            get { return _fullscale; }
            set { _fullscale = value; }
        }

        /// <summary>
        /// Zero offset of the analog output signal
        /// </summary>
        public Double Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// Value of the analog output signal.  When set, the value of the raw
        /// <see cref="IAnalogOutput">AnalogOutput</see> is calculated and set, and the
        /// <see cref="ValueChanged">ValueChanged</see> event is fired.
        /// </summary>
        public Double Value
        {
            get { return _value; }
            set
            {
                _value = value;

                // Scale _value (in arbitrary units) to the units of the analog output (i.e. volts, mA, etc)
                _analogOutput.Value = ((_value - _offset) / _fullscale) *
                    (_analogOutput.Max - _analogOutput.Min) + _analogOutput.Min;
                OnValueChanged();
            }
        }

        #endregion Public Properties
    }
}