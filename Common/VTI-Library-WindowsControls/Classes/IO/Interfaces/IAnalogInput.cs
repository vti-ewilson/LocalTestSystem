using System;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for Analog Inputs
    /// </summary>
    public interface IAnalogInput : IAnalogIO
    {
        /// <summary>
        /// Raw Value (i.e. volts, amps, etc.) of the analog input
        /// </summary>
        double RawValue { get; }

        /// <summary>
        /// Occurs when the raw value of the analog input changes
        /// </summary>
        event EventHandler RawValueChanged;
    }
}