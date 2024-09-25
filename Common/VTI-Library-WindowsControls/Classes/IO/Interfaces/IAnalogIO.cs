namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface which must be implemented by all Analog I/O
    /// </summary>
    public interface IAnalogIO : IAnalogDigitalIO
    {
        /// <summary>
        /// Gets the minimum value of the analog I/O.
        /// </summary>
        double Min { get; }

        /// <summary>
        /// Gets the maximum value of the analog I/O
        /// </summary>
        double Max { get; }

        /// <summary>
        /// Gets the units of the value (i.e. volts, mA, etc.)
        /// </summary>
        string Units { get; }

        /// <summary>
        /// Gets the format string to be used for displaying the value of the analog I/O
        /// </summary>
        string Format { get; set; }
    }
}