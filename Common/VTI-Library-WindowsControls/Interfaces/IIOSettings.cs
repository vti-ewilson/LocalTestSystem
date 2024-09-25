using VTIWindowsControlLibrary.Classes.IO;

namespace VTIWindowsControlLibrary.Interfaces
{
    /// <summary>
    /// Interface which must be implemented by the IOSettings class of the client application.
    /// </summary>
    public interface IIOSettings
    {
        /// <summary>
        /// Gets or sets the I/O interface.
        /// </summary>
        /// <value>The I/O interface.</value>
        IOInterface Interface { get; set; }
    }
}