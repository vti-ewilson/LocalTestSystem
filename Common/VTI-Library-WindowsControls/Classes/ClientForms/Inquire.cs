using System;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the <see cref="VTIWindowsControlLibrary.Forms.InquireForm">Inquire Form</see> of the client application
    /// </summary>
    /// <remarks>
    /// This class isn't the <see cref="VTIWindowsControlLibrary.Forms.InquireForm">Inquire Form</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class Inquire
    {
        private static InquireForm inquireForm;

        /// <summary>
        /// Initializes the static instance of the <see cref="VTIWindowsControlLibrary.Forms.InquireForm">Inquire Form</see> class
        /// </summary>
        static Inquire()
        {
            inquireForm = new InquireForm();
        }

        /// <summary>
        /// sets up event that needs to be called when the integrated scan button is pressed. Used when scanner requires software trigger to scan, such as with a mounted scanner.
        /// </summary>
        /// <param name="EventHandler">Event that will trigger when scan button is pressed in the inquire form.</param>
        /// <remarks>
        /// Example event:
        /// public virtual void ScanButtonClicked_InInquire(object sender, EventArgs e)
        /// {
        ///     Machine.Cycle[0].Scan.Start(); // the cycle step that triggers the scan
        /// }
        /// 
        /// Example binding:
        /// Inquire.SetScanEvent(ScanButtonClicked_InInquire);
        /// </remarks>
        public static void SetScanEvent(EventHandler handler)
        {
            inquireForm.SetScanButtonEvent(handler);
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.InquireForm">Inquire Form</see>
        /// </summary>
        public static void Show()
        {
            Show(null);
        }

        public static string GetName()
        {
            return inquireForm.Name;
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.InquireForm">Inquire Form</see>
        /// </summary>
        /// <param name="MdiParent">Form that will own this form.</param>
        public static void Show(Form MdiParent)
        {
            if (MdiParent != null)
                inquireForm.MdiParent = MdiParent;
            inquireForm.Show();
            inquireForm.BringToFront();
        }

        /// <summary>
        /// Hides the <see cref="VTIWindowsControlLibrary.Forms.InquireForm">Inquire Form</see>
        /// </summary>
        public static void Hide()
        {
            inquireForm.Hide();
        }

        /// <summary>
        /// Causes the inquire form to find all data related to the specified serial number.
        /// </summary>
        /// <param name="SerialNumber">Serial number to look up</param>
        public static void LookupSerialNumber(string SerialNumber)
        {
            inquireForm.LookupSerialNumber(SerialNumber);
        }
    }
}