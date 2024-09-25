using System;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for a Log-Linear Signal Converter
    /// </summary>
    /// <remarks>
    /// A Log-Linear Signal Converter converts the
    /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
    /// <see cref="IAnalogInput">AnalogInput</see> to the
    /// <see cref="AnalogSignal.Value">Value</see> of the
    /// <see cref="AnalogSignal">AnalogSignal</see>
    /// using a log-linear conversion.
    /// </remarks>
    public interface ILogLinearSignalConverter : IAnalogSignalConverter
    {
        /// <summary>
        /// Linear span factor that is applied to the
        /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// prior to the log-linear conversion
        /// </summary>
        Double LinearFactor { get; set; }

        /// <summary>
        /// Linear offset that is applied to the
        /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// prior to the log-linear conversion
        /// </summary>
        Double LinearOffset { get; set; }

        /// <summary>
        /// Volts per Decade for the log-linear conversion
        /// </summary>
        Double VoltsPerDecade { get; set; }

        /// <summary>
        /// Minimum exponent for the log-linear conversion
        /// </summary>
        Double MinExponent { get; set; }

        /// <summary>
        /// Maximum exponent for the log-linear conversion
        /// </summary>
        Double MaxExponent { get; set; }
    }
}