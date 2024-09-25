using System;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Classes.ManualCommands
{
    /// <summary>
    /// When applied to a method, the ManualCommand attribute converts the method into
    /// a "Manual Command", by placing it on the Manual Commands form, and intercepting
    /// calls to the method to handle the VTI security
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ManualCommandAttribute : System.Attribute
    {
        #region Fields (3)

        #region Private Fields (3)

        private CommandPermissionType commandPermission;
        private string commandText;
        private Boolean visible;
        //show in Permissions form by default
        private bool showInPermissionsForm = true;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// ManualCommandAttribute converts a method into a manual command, causing it to appear
        /// in the Manual Commands window, enabling barcode scanning of the CommandText, and
        /// enforcing permissions regardless of what methos is used to invoke the method.
        /// </summary>
        /// <param name="CommandText">Command Text to appear in the Manual Commands window and to
        /// be scanned by the barcode scanner.  If the Command Text appears in the Localization resource,
        /// the string value from the Localization resource will be displayed in the Manual Commands window.</param>
        /// <param name="Visible">If true, the command will appear in the Manual Commands window.
        /// If false, the command won't appear in the Manual Commands window, but will still be available
        /// to invoke programmatically or via the barcode scanner.</param>
        /// <param name="Permission">Command permission type</param>
        public ManualCommandAttribute(
          string CommandText,
          Boolean Visible,
          CommandPermissionType Permission)
        {
            this.commandText = CommandText;
            this.visible = Visible;
            this.commandPermission = Permission;
        }

        /// <summary>
        /// ManualCommandAttribute converts a method into a manual command, causing it to appear
        /// in the Manual Commands window, enabling barcode scanning of the CommandText, and
        /// enforcing permissions regardless of what methos is used to invoke the method.
        /// </summary>
        /// <param name="CommandText">Command Text to appear in the Manual Commands window and to
        /// be scanned by the barcode scanner.  If the Command Text appears in the Localization resource,
        /// the string value from the Localization resource will be displayed in the Manual Commands window.</param>
        /// <param name="Visible">If true, the command will appear in the Manual Commands window.
        /// If false, the command won't appear in the Manual Commands window, but will still be available
        /// to invoke programmatically or via the barcode scanner.</param>
        /// <param name="Permission">Command permission type</param>
        /// /// <param name="ShowInPermissionsForm">True by default. If false, it will not be visible in the Permissions form.</param>
        public ManualCommandAttribute(
          string CommandText,
          Boolean Visible,
          CommandPermissionType Permission,
          bool ShowInPermissionsForm
          ) {
            this.commandText = CommandText;
            this.visible = Visible;
            this.commandPermission = Permission;
            this.showInPermissionsForm = ShowInPermissionsForm;
        }

        #endregion Constructors

        #region Properties (3)

        /// <summary>
        /// Gets the Command Text of the manual command
        /// </summary>
        public string CommandText
        { get { return commandText; } }

        /// <summary>
        /// Gets the type of permission required to execute the command
        /// </summary>
        public CommandPermissionType Permission
        { get { return commandPermission; } }

        /// <summary>
        /// Gets a value that indicates if the command is visible in the Permissions form
        /// </summary>
        public bool ShowInPermissionsForm {
            get { return showInPermissionsForm; }
            set { showInPermissionsForm = value; }
        }

        /// <summary>
        /// Gets a value that indicates if the command is visible in the Manual Command form
        /// </summary>
        public Boolean Visible
        { 
            get { return visible; }
            set { visible = value; }
        }

        #endregion Properties
    }
}