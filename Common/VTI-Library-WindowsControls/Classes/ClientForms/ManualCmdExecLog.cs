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
    /// Represents the <see cref="VTIWindowsControlLibrary.Forms.ManualCmdExecLogForm">ManualCmdExecLogForm Form</see> of the client application
    /// </summary>
    /// /// <remarks>
    /// This class isn't the <see cref="VTIWindowsControlLibrary.Forms.ManualCmdExecLogForm">ManualCmdExecLogForm Form</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class ManualCmdExecLog
    {
        private static ManualCmdExecLogForm ManualCmdExecLogForm;

        /// <summary>
        /// Initializes the static instance of the <see cref="ManualCmdExecLog">ManualCmdExecLog</see> class
        /// </summary>
        static ManualCmdExecLog()
        {
            ManualCmdExecLogForm = new ManualCmdExecLogForm();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ManualCmdExecLogForm">ManualCmdExecLog Form</see>
        /// </summary>
        public static void Show()
        {
            ManualCmdExecLog.Show(null);
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ManualCmdExecLogForm">ManualCmdExecLog Form</see>
        /// </summary>
        /// <param name="MdiParent">Form that will own this form.</param>
        public static void Show(Form MdiParent)
        {
            if (MdiParent != null) ManualCmdExecLogForm.MdiParent = MdiParent;
            ManualCmdExecLogForm.Show();
            ManualCmdExecLogForm.BringToFront();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ManualCmdExecLogForm">ManualCmdExecLog Form</see>
        /// as a modal dialog box.
        /// </summary>
        public static void ShowDialog()
        {
            ManualCmdExecLogForm.ShowDialog();
        }
    }
}