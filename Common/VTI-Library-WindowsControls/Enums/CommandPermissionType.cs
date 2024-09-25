namespace VTIWindowsControlLibrary.Enums
{
    /// <summary>
    /// Controls access to methods marked with the
    /// <see cref="VTIWindowsControlLibrary.Classes.ManualCommands.ManualCommandAttribute">ManualCommandAttribute</see>
    /// </summary>
    public enum CommandPermissionType
    {
        /// <summary>
        /// Indicates that no permission is required to execute the command
        /// </summary>
        /// <remarks>
        /// Note: Even logged off users can execute commands with the permission set to None
        /// </remarks>
        None,

        /// <summary>
        /// Indicates that any logged in user can execute the command
        /// </summary>
        AnyLoggedInUser,

        /// <summary>
        /// Indicates that the permissions for the current user will be checked before executing the command
        /// </summary>
        /// <remarks>
        /// Note: If the user does not have permission to execute the command, no warning will be given.
        /// The command will simply not execute.
        /// </remarks>
        CheckPermission,

        /// <summary>
        /// Indicates that the permissions for the current user will be checked before executing the command
        /// </summary>
        /// <remarks>
        /// Note: If the user does not have permission to execute the command, a warning message will be displayed.
        /// </remarks>
        CheckPermissionWithWarning
    }
}