namespace VTIWindowsControlLibrary.Interfaces
{
    /// <summary>
    /// Interface which must be implemented by the Config class of the client application
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Gets the operator ID
        /// </summary>
        /// <value>The operator ID.</value>
        string _OpID { get; }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        void _Save();

        /// <summary>
        /// Gets the IO instance.
        /// </summary>
        /// <value>The IO instance.</value>
        IIOSettings IOInstance { get; }
    }
}