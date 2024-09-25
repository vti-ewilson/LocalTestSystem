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
    /// Represents the <see cref="VTIWindowsControlLibrary.Forms.ParamChangeLogForm">ParamChangeLogForm Form</see> of the client application
    /// </summary>
    /// /// <remarks>
    /// This class isn't the <see cref="VTIWindowsControlLibrary.Forms.ParamChangeLogForm">ParamChangeLogForm Form</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class ParamChangeLog
    {
        private static ParamChangeLogForm paramChangeLogForm;

        /// <summary>
        /// Initializes the static instance of the <see cref="ParamChangeLog">ParamChangeLog</see> class
        /// </summary>
        static ParamChangeLog()
        {
            paramChangeLogForm = new ParamChangeLogForm();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ParamChangeLogForm">ParamChangeLog Form</see>
        /// </summary>
        public static void Show()
        {
            ParamChangeLog.Show(null);
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ParamChangeLogForm">ParamChangeLog Form</see>
        /// </summary>
        /// <param name="MdiParent">Form that will own this form.</param>
        public static void Show(Form MdiParent)
        {
            if (MdiParent != null) paramChangeLogForm.MdiParent = MdiParent;
            paramChangeLogForm.Show();
            paramChangeLogForm.BringToFront();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.ParamChangeLogForm">ParamChangeLog Form</see>
        /// as a modal dialog box.
        /// </summary>
        public static void ShowDialog()
        {
            paramChangeLogForm.ShowDialog();
        }
    }
}