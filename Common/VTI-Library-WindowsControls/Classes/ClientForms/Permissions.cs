using System;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.ManualCommands;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Enums;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the <see cref="VTIWindowsControlLibrary.Forms.PermissionsForm">Permissions Form</see> of the client application
    /// </summary>
    /// /// <remarks>
    /// This class isn't the <see cref="VTIWindowsControlLibrary.Forms.PermissionsForm">Permissions Form</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class Permissions
    {
        private static PermissionsForm permissionsForm;

        /// <summary>
        /// Initializes the static instance of the <see cref="Permissions">Permissions</see> class
        /// </summary>
        static Permissions()
        {
            permissionsForm = new PermissionsForm();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.PermissionsForm">Permissions Form</see>
        /// </summary>
        public static void Show()
        {
            Permissions.Show(null);
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.PermissionsForm">Permissions Form</see>
        /// </summary>
        /// <param name="MdiParent">Form that will own this form.</param>
        public static void Show(Form MdiParent)
        {
            if (MdiParent != null) permissionsForm.MdiParent = MdiParent;
            permissionsForm.Show();
            permissionsForm.BringToFront();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.PermissionsForm">Permissions Form</see>
        /// as a modal dialog box.
        /// </summary>
        public static void ShowDialog()
        {
            permissionsForm.ShowDialog();
        }

        /// <summary>
        /// Grants permission to all commands for a specific group
        /// </summary>
        /// <param name="GroupNumber">Group to which to grant permissions.</param>
        public static void GrantAllCommandsToGroup(int GroupNumber)
        {
            using (VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString))
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    // retrieve list of commands directly from the Machine class
                    foreach (MethodInfo method in VtiLib.ManualCommands.GetType().GetMethods(BindingFlags.Public |
                                BindingFlags.Instance | BindingFlags.Static |
                                BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy))
                        foreach (ManualCommandAttribute attribute in method.GetCustomAttributes(typeof(ManualCommandAttribute), false))
                            if ((attribute.Permission == CommandPermissionType.CheckPermission) ||
                                (attribute.Permission == CommandPermissionType.CheckPermissionWithWarning))
                                db.GroupCommands.InsertOnSubmit(
                                    new GroupCommand
                                    {
                                        GroupID = String.Format("GROUP{0:00}", GroupNumber),
                                        CommandID = attribute.CommandText
                                    });
                    db.SubmitChanges();
                    ts.Complete();
                }
            }
        }
    }
}