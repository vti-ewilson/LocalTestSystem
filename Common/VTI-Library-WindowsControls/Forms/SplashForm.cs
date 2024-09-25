using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents the splash screen of the client application
    /// </summary>
    public partial class SplashForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SplashForm">SplashForm</see>
        /// </summary>
        public SplashForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the Product Name to be displayed on the splash screen.
        /// </summary>
        public new string ProductName
        {
            get { return labelProductName.Text; }
            set { labelProductName.Text = value; }
        }

        /// <summary>
        /// Gets or sets the Message Font to be displayed on the splash screen.
        /// </summary>
        public new Font MessageFont
        {
            get { return labelMessage.Font; }
            set { labelMessage.Font = value; }
        }

        /// <summary>
        /// Gets or sets the software version to be displayed on the splash screen.
        /// </summary>
        public string Version
        {
            get { return labelVersion.Text; }
            set { labelVersion.Text = value; }
        }

        /// <summary>
        /// Gets or sets the Copyright string to be displayed on the splash screen.
        /// </summary>
        public string Copyright
        {
            get { return labelCopyright.Text; }
            set { labelCopyright.Text = value; }
        }

        /// <summary>
        /// Gets or sets the message text to be displayed on the splash screen.
        /// </summary>
        public string Message
        {
            get { return labelMessage.Text; }
            set
            {
                labelMessage.Text = value;
                this.BringToFront();
                labelMessage.Refresh();
                Application.DoEvents();
            }
        }
    }
}