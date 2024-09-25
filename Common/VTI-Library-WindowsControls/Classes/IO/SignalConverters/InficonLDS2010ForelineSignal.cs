using System;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.SignalConverters
{
    /// <summary>
    /// Signal converter for the Foreline Signal of an Inficon LDS2010 Leak Detector
    /// </summary>
    public class InficonLDS2010ForelineSignal : ILogLinearSignalConverter
    {
        #region Globals

        private LogLinearSignal _logLinearSignal1;
        private LogLinearSignal _logLinearSignal2;

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
            return (RawValue < 3.5 ? _logLinearSignal1.Convert(RawValue) : _logLinearSignal2.Convert(RawValue));
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

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010ForelineSignal">InficonLDS2010ForelineSignal</see> class
        /// </summary>
        public InficonLDS2010ForelineSignal()
        {
            _logLinearSignal1 = new LogLinearSignal
            {
                VoltsPerDecade = 0.5,
                MinExponent = -5,
                MaxExponent = 2,
                LinearFactor = 760,
                LinearOffset = 0
            };
            _logLinearSignal2 = new LogLinearSignal
            {
                VoltsPerDecade = 0.5,
                MinExponent = -13,
                MaxExponent = -5,
                LinearFactor = 760,
                LinearOffset = 0
            };
        }

        #endregion Construction

        #region ILogLinearSignalConverter Members

        /// <summary>
        /// Linear span factor that is applied to the
        /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// prior to the log-linear conversion
        /// </summary>
        public double LinearFactor
        {
            get
            {
                return _logLinearSignal1.LinearFactor;
            }
            set
            {
                _logLinearSignal1.LinearFactor = value;
                _logLinearSignal2.LinearFactor = value;
            }
        }

        /// <summary>
        /// Linear offset that is applied to the
        /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// prior to the log-linear conversion
        /// </summary>
        public double LinearOffset
        {
            get
            {
                return _logLinearSignal1.LinearOffset;
            }
            set
            {
                _logLinearSignal1.LinearOffset = value;
                _logLinearSignal1.LinearOffset = value;
            }
        }

        /// <summary>
        /// Volts per Decade for the log-linear conversion
        /// </summary>
        public double VoltsPerDecade
        {
            get
            {
                return 0.5;
            }
            set
            {
                throw new Exception("VoltsPerDecade property of InficonLDS2010ForelineSignal is read-only.");
            }
        }

        /// <summary>
        /// Minimum exponent for the log-linear conversion
        /// </summary>
        public double MinExponent
        {
            get
            {
                return -13;
            }
            set
            {
                throw new Exception("MinExponent property of InficonLDS2010ForelineSignal is read-only.");
            }
        }

        /// <summary>
        /// Maximum exponent for the log-linear conversion
        /// </summary>
        public double MaxExponent
        {
            get
            {
                return 2;
            }
            set
            {
                throw new Exception("MaxExponent property of InficonLDS2010ForelineSignal is read-only.");
            }
        }

        #endregion ILogLinearSignalConverter Members
    }
}