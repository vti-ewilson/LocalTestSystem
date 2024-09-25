using System;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for Formatted Analog Inputs
    /// </summary>
    public interface IFormattedAnalogInput : IAnalogInput
    {
        /// <summary>
        /// Value of the analog input
        /// </summary>
        Double Value { get; }

        /// <summary>
        /// Returns the <see cref="Value">Value</see> formatted properly and followed by the Units.
        /// </summary>
        String FormattedValue { get; }

        /// <summary>
        /// Occurs when the value of the analog input changes
        /// </summary>
        event EventHandler ValueChanged;
    }
}