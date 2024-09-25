namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface which must be implemented for all Analog and Digital I/O
    /// </summary>
    public interface IAnalogDigitalIO
    {
        /// <summary>
        /// Gets the name of the I/O.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether this I/O instance is available.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this I/O instance is available; otherwise, <c>false</c>.
        /// </value>
        bool IsAvailable { get; }
    }
}