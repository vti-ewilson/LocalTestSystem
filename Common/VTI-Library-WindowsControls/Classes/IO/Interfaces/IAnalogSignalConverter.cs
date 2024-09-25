using System;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for Analog Signal Converters
    /// </summary>
    public interface IAnalogSignalConverter
    {
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
        Double Convert(Double RawValue);

        /// <summary>
        /// Indicates if the
        /// <see cref="IAnalogSignalConverter">AnalogSignalConverter</see>
        /// is linear
        /// </summary>
        Boolean IsLinear { get; }
    }
}