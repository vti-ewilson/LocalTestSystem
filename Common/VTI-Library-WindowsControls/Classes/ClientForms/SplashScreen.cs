using System.Drawing;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the Splash Screen of the client application
    /// </summary>
    /// <remarks>
    /// This class isn't the <see cref="SplashForm">SplashForm</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class SplashScreen
    {
        private static SplashForm splashForm;

        /// <summary>
        /// Initializes the static instance of the <see cref="SplashScreen">SplashScreen</see> class
        /// </summary>
        static SplashScreen()
        {
            splashForm = new SplashForm();
        }

        /// <summary>
        /// Shows the <see cref="SplashForm">SplashForm</see>
        /// </summary>
        /// <param name="ProductName">Product name to be displayed on the form</param>
        /// <param name="Version">Version number to be displayed on the form</param>
        /// <param name="Copyright">Copyright to be displayed on the form</param>
        public static void Show(string ProductName, string Version, string Copyright)
        {
            splashForm.ProductName = ProductName;
            splashForm.Version = "Version " + Version;
            splashForm.Copyright = "Copyright © " + System.DateTime.Now.Year.ToString() + " Vacuum Technology Inc";//Copyright;
            splashForm.Show();
            splashForm.BringToFront();
            splashForm.Refresh();
        }

        public static void Show(string ProductName, string Version, string Copyright, Font MessageFont)
        {
            splashForm.ProductName = ProductName;
            splashForm.Version = "Version " + Version;
            splashForm.Copyright = "Copyright © " + System.DateTime.Now.Year.ToString() + " Vacuum Technology Inc";//Copyright;
            splashForm.MessageFont = MessageFont;
            splashForm.Show();
            splashForm.BringToFront();
            splashForm.Refresh();
        }

        /// <summary>
        /// Gets or sets the message to be displayed on the form
        /// </summary>
        public static string Message
        {
            get { return splashForm.Message; }
            set { splashForm.Message = value; }
        }

        /// <summary>
        /// Hides the form
        /// </summary>
        public static void Hide()
        {
            //VtiLib.MuteParamChangeLog = false;
            splashForm.Hide();
        }
    }
}