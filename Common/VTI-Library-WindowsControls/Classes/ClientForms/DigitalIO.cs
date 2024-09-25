using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the <see cref="VTIWindowsControlLibrary.Forms.DigitalIOForm">Digital I/O Form</see> of the client application
    /// </summary>
    /// <remarks>
    /// This class isn't the <see cref="VTIWindowsControlLibrary.Forms.DigitalIOForm">Digital I/O Form</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class DigitalIO
    {
        private static DigitalIOForm digitalIO;

        /// <summary>
        /// Initializes the static instance of the <see cref="VTIWindowsControlLibrary.Forms.DigitalIOForm">Digital I/O Form</see> class
        /// </summary>
        static DigitalIO()
        {
            digitalIO = new DigitalIOForm();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.DigitalIOForm">Digital I/O Form</see>
        /// </summary>
        public static void Show()
        {
            Show(null);
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.DigitalIOForm">Digital I/O Form</see>
        /// </summary>
        /// <param name="MdiParent">Form that will own this form.</param>
        public static void Show(Form MdiParent)
        {
            if (MdiParent != null) digitalIO.MdiParent = MdiParent;
            digitalIO.Show();
            digitalIO.BringToFront();
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.DigitalIOForm">Digital I/O Form</see>
        /// </summary>
        /// <param name="Active">Value to indicate if the outputs should be active.</param>
        public static void Show(Boolean Active)
        {
            Show(null, Active);
        }

        /// <summary>
        /// Shows the <see cref="VTIWindowsControlLibrary.Forms.DigitalIOForm">Digital I/O Form</see>
        /// </summary>
        /// <param name="MdiParent">Form that will own this form.</param>
        /// <param name="Active">Value to indicate if the outputs should be active.</param>
        public static void Show(Form MdiParent, Boolean Active)
        {
            if (MdiParent != null) digitalIO.MdiParent = MdiParent;
            digitalIO.Show(Active);
            digitalIO.BringToFront();
        }

        /// <summary>
        /// Changes the language of the <c>Form</c> to the currently selected language.
        /// </summary>
        public static void ChangeLanguage()
        {
            digitalIO.ChangeLanguage();
        }

        #region Public Methods

        /// <summary>
        /// Set width and height of form
        /// </summary>
        public static void SetSize(int w, int h)
        {
            int wBias = 97, hBias = 37;
            int xForm = digitalIO.Size.Width;
            //int yForm = touchScreenButtonForm.Size.Height;
            digitalIO.Size = new Size(w + wBias, h + hBias);
            //touchScreenButtonForm.Size = new Size(w + wBias, h + hBias);
            digitalIO.flowLayoutPanelDigitalInputs.Size = new Size(w, h);
            digitalIO.flowLayoutPanelDigitalInputs.MaximumSize = new Size(w, h);
            digitalIO.flowLayoutPanelDigitalOutputs.Size = new Size(w, h);
            digitalIO.flowLayoutPanelDigitalOutputs.MaximumSize = new Size(w, h);
            int xPanel1 = digitalIO.flowLayoutPanel1.Location.X;
            int yPanel1 = digitalIO.flowLayoutPanel1.Location.Y;
            digitalIO.flowLayoutPanel1.Location = new Point(xPanel1 + w + wBias - xForm, yPanel1);
            //touchScreenButtonForm.flowLayoutPanel1.Size = new Size(w, h);
            //touchScreenButtonForm.flowLayoutPanel1.MaximumSize = new Size(w, h);
            //int xPanel1 = touchScreenButtonForm.panel1.Location.X;
            //int yPanel1 = touchScreenButtonForm.panel1.Location.Y;
            //touchScreenButtonForm.panel1.Location = new Point(xPanel1 + w + wBias - xForm, yPanel1);
        }

        #endregion Public Methods

        public static bool ManualMode
        {
            get { return DigitalIOForm.ManualMode; }
            set { DigitalIOForm.ManualMode = value; }
        }
    }
}