namespace VTIWindowsControlLibrary.Enums
{
    /// <summary>
    /// Categories for writing events to the
    /// <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
    /// </summary>
    /// <remarks>
    /// Enumerated type that corresponds with the messages in the VtiEventCatMsgs.dll file
    /// If VtiEventCatMsgs.dll is kept up-to-date, then the system Event Viewer can display the category names.
    /// Category names within the library are derived directly from this enumeration, so VtiEventCatMsgs.dll
    /// isn't necessary to display the categories within the VTI software.
    /// </remarks>
    public enum VtiEventCatType : short
    {
        /// <summary>
        /// Indicates that no category was given
        /// </summary>
        None,

        /// <summary>
        /// Indicates that an Application Error occurred
        /// </summary>
        /// <remarks>
        /// Application errors are typically programming and/or data induced errors
        /// such as divide-by-zero that are trapped by the global
        /// <see cref="VTIWindowsControlLibrary.Classes.VtiExceptionHandler">VtiExceptionHandler</see>.
        /// </remarks>
        Application_Error,

        /// <summary>
        /// Indicates that an event associated with a user logon or logoff has occurred
        /// </summary>
        Login,

        /// <summary>
        /// Indicates that a manual command was executed
        /// </summary>
        Manual_Command,

        /// <summary>
        /// Indicates that a paramter update was performed
        /// </summary>
        Parameter_Update,

        /// <summary>
        /// Indicates an event log entry related to a test cycle
        /// </summary>
        Test_Cycle,

        /// <summary>
        /// Indicates an event log entry related to a calibration process
        /// </summary>
        Calibration,

        /// <summary>
        /// Indicates an event log entry related to a Serial I/O process
        /// </summary>
        Serial_IO,

        /// <summary>
        /// Indicates an event log entry related to a Digital I/O process
        /// </summary>
        Digital_IO,

        /// <summary>
        /// Indicates an event log entry related to an Analog I/O process
        /// </summary>
        Analog_IO,

        /// <summary>
        /// Indicates an event log entry related to a database operation
        /// </summary>
        Database,

        /// <summary>
        /// Indicates an event log entry related to a ethernet device operation
        /// </summary>
        Ethernet_IO
    }
}