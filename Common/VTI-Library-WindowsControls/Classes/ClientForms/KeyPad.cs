using System;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the alpha-numeric touchscreen <see cref="KeyPadForm">KeyPadForm</see> of the client application
    /// </summary>
    /// <remarks>
    /// This class isn't the <see cref="KeyPadForm">KeyPadForm</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public static class KeyPad
    {
        /// <summary>
        /// Shows the <see cref="KeyPadForm">KeyPadForm</see>
        /// </summary>
        /// <param name="Password">Value to indicate if the characters should be replaced with asterisks (*) for entering a password.</param>
        /// <returns>Value entered by the user.</returns>
        public static string Show(Boolean Password)
        {
            return Show(string.Empty, Password);
        }

        /// <summary>
        /// Shows the <see cref="KeyPadForm">KeyPadForm</see>
        /// </summary>
        /// <param name="Caption">Caption to be displayed on the form</param>
        /// <param name="Password">Value to indicate if the characters should be replaced with asterisks (*) for entering a password.</param>
        /// <param name="ButtHeight">AlfaNumeric button width</param>
        /// <param name="ButtWidth">AlfaNumeric button Height.</param>
        /// <param name="FormWidth">Form width</param>
        /// <param name="FormHeight">Form Height.</param>
        /// <returns>Value entered by the user.</returns>
        public static string Show(String Caption, Boolean Password, int ButtHeight, int ButtWidth, int FormWidth, int FormHeight)
        {
            KeyPadForm frmKeyPad1 = new KeyPadForm();
            frmKeyPad1.KeyPadButtonHeight = ButtHeight;
            frmKeyPad1.KeyPadButtonWidth = ButtWidth;
            frmKeyPad1.Width = FormWidth;
            frmKeyPad1.Height = FormHeight;
            frmKeyPad1.Text = Caption;
            frmKeyPad1.Password = Password;
            frmKeyPad1.ShowDialog();
            return frmKeyPad1.KeyPadText;
        }

        public static string Show(String Caption, Boolean Password)
        {
            KeyPadForm frmKeyPad1 = new KeyPadForm();
            frmKeyPad1.KeyPadButtonHeight = 60;
            frmKeyPad1.KeyPadButtonWidth = 60;
            frmKeyPad1.Width = 825;
            frmKeyPad1.Text = Caption;
            frmKeyPad1.Password = Password;
            frmKeyPad1.ShowDialog();
            return frmKeyPad1.KeyPadText;
        }
    }
}