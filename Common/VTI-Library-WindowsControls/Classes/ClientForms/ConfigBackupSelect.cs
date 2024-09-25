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
    /// Represents the <see cref="VTIWindowsControlLibrary.Forms.ConfigBackupSelectForm">ConfigBackupSelectForm Form</see> of the client application
    /// </summary>
    /// /// <remarks>
    /// This class isn't the <see cref="VTIWindowsControlLibrary.Forms.ConfigBackupSelectForm">ConfigBackupSelectForm Form</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class ConfigBackupSelect
    {
        private static ConfigBackupSelectForm ConfigBackupSelectForm;

        /// <summary>
        /// Initializes the static instance of the <see cref="ConfigBackupSelect">ConfigBackupSelect</see> class
        /// </summary>
        static ConfigBackupSelect()
        {
            ConfigBackupSelectForm = new ConfigBackupSelectForm();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ConfigBackupSelectForm">ConfigBackupSelect Form</see>
        /// </summary>
        public static void Show()
        {
            ConfigBackupSelect.Show(null);
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ConfigBackupSelectForm">ConfigBackupSelect Form</see>
        /// </summary>
        /// <param name="MdiParent">Form that will own this form.</param>
        public static void Show(Form MdiParent)
        {
            if (MdiParent != null) ConfigBackupSelectForm.MdiParent = MdiParent;
            ConfigBackupSelectForm.Show();
            ConfigBackupSelectForm.BringToFront();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ConfigBackupSelectForm">ConfigBackupSelect Form</see>
        /// as a modal dialog box.
        /// </summary>
        public static void ShowDialog()
        {
            ConfigBackupSelectForm.ShowDialog();
        }
    }
}