namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for Analog Outputs
    /// </summary>
    public interface IAnalogOutput : IAnalogIO
    {
        ///// <summary>
        ///// Name of the analog output
        ///// </summary>
        //string Name { get; }
        /// <summary>
        /// Value of the analog output
        /// </summary>
        double Value { get; set; }

        ///// <summary>
        ///// Minimum value of the analog output
        ///// </summary>
        //double Min { get; }
        ///// <summary>
        ///// Maximum value of the analog output
        ///// </summary>
        //double Max { get; }
        ///// <summary>
        ///// Units (i.e. psi, Torr, etc.) of the analog output
        ///// </summary>
        //string Units { get; }
        ///// <summary>
        ///// Format string to be used when displaying the value of the analog output
        ///// </summary>
        //string Format { get; set; }
        ///// <summary>
        ///// Indicates that the analog output was located in the <see cref="IOInterface">IOInterface</see>
        ///// </summary>
        //bool IsAvailable { get; }
    }
}