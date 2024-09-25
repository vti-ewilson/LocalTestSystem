using System;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.SignalConverters
{
    /// <summary>
    /// General purpose Log-Linear Signal Converter
    /// </summary>
    public class LogLinearSignal : ILogLinearSignalConverter
    {
        #region Globals

        private Double _LinearFactor = 1;
        private Double _LinearOffset = 0;
        private Double _VoltsPerDecade;
        private Double _MinExponent;
        private Double _MaxExponent;

        #endregion Globals

        #region IAnalogSignalConverter Members

        /// <summary>
        /// Converts the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see> to the
        /// <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        /// <param name="RawValue"><see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see></param>
        /// <returns>Double (<see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>)</returns>
        public double Convert(double RawValue)
        {
            return (_LinearFactor * Math.Pow(10, ((RawValue / _VoltsPerDecade) + _MinExponent)) - _LinearOffset);
        }

        /// <summary>
        /// Returns false to indicate that this is not a linear signal converter
        /// </summary>
        public bool IsLinear
        {
            get { return false; }
        }

        #endregion IAnalogSignalConverter Members

        #region Construction

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LogLinearSignal">LogLinearSignal</see> class
        ///// </summary>
        ///// <param name="VoltsPerDecade">Volts per decade of the analog signal</param>
        ///// <param name="MinExponent">Minimum exponent of the analog signal</param>
        ///// <param name="MaxExponent">Maximum exponent of the analog signal</param>
        //public LogLinearSignal(Double VoltsPerDecade, Double MinExponent, Double MaxExponent)
        //    : this(VoltsPerDecade, MinExponent, MaxExponent, 1, 0)
        //{ }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LogLinearSignal">LogLinearSignal</see> class
        ///// </summary>
        ///// <param name="VoltsPerDecade">Volts per decade of the analog signal</param>
        ///// <param name="MinExponent">Minimum exponent of the analog signal</param>
        ///// <param name="MaxExponent">Maximum exponent of the analog signal</param>
        ///// <param name="LinearFactor">Linear factor for the analog input</param>
        ///// <param name="LinearOffset">Linear offset for the analog input</param>
        //public LogLinearSignal(Double VoltsPerDecade, Double MinExponent, Double MaxExponent, Double LinearFactor, Double LinearOffset)
        //{
        //    _VoltsPerDecade = VoltsPerDecade;
        //    _MinExponent = MinExponent;
        //    _MaxExponent = MaxExponent;
        //    _LinearFactor = LinearFactor;
        //    _LinearOffset = LinearOffset;
        //}

        #endregion Construction

        /// <summary>
        /// Volts per Decade for the log-linear conversion
        /// </summary>
        public Double VoltsPerDecade
        {
            get { return _VoltsPerDecade; }
            set { _VoltsPerDecade = value; }
        }

        /// <summary>
        /// Minimum exponent for the log-linear conversion
        /// </summary>
        public Double MinExponent
        {
            get { return _MinExponent; }
            set { _MinExponent = value; }
        }

        /// <summary>
        /// Maximum exponent for the log-linear conversion
        /// </summary>
        public Double MaxExponent
        {
            get { return _MaxExponent; }
            set { _MaxExponent = value; }
        }

        /// <summary>
        /// Linear span factor that is applied to the
        /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// prior to the log-linear conversion
        /// </summary>
        public Double LinearFactor
        {
            get { return _LinearFactor; }
            set { _LinearFactor = value; }
        }

        /// <summary>
        /// Linear offset that is applied to the
        /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// prior to the log-linear conversion
        /// </summary>
        public Double LinearOffset
        {
            get { return _LinearOffset; }
            set { _LinearOffset = value; }
        }
    }
}