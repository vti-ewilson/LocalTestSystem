using System;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for a Linear Signal Converter
    /// </summary>
    /// <remarks>
    /// A Linear Signal Converter converts the
    /// <see cref="IAnalogInput.RawValue">RawValue</see> of the
    /// <see cref="IAnalogInput">AnalogInput</see> to the
    /// <see cref="AnalogSignal.Value">Value</see> of the
    /// <see cref="AnalogSignal">AnalogSignal</see>
    /// using a linear conversion.
    /// </remarks>
    public interface ILinearSignalConverter : IAnalogSignalConverter
    {
        /// <summary>
        /// Minimum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see> to the
        /// </summary>
        Double InputMinimum { get; set; }

        /// <summary>
        /// Maximum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see> to the
        /// </summary>
        Double InputMaximum { get; set; }

        /// <summary>
        /// Full scale range of the <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        Double FullScale { get; set; }

        /// <summary>
        /// Zero offset of the <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        Double Offset { get; set; }
    }
}