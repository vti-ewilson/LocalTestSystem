using System;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Displays a window similar to a <see cref="MessageBox">MessageBox</see>
    /// with a <see cref="TextBox">TextBox</see> for the user to enter a value
    /// </summary>
    public partial class InputBoxForm : Form
    {
        private string strReturnValue;
        private Point pntStartLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBoxForm">InputBoxForm</see>
        /// </summary>
        public InputBoxForm()
        {
            InitializeComponent();
            this.strReturnValue = "";
        }

        private void frmInputBox_Load(object sender, EventArgs e)
        {
            if (!this.pntStartLocation.IsEmpty)
            {
                this.Top = this.pntStartLocation.Y;
                this.Left = this.pntStartLocation.X;
            }
            this.txtResult.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.strReturnValue = this.txtResult.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Title to display on then input box
        /// </summary>
        public string Title
        {
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// Prompt to display in the input box
        /// </summary>
        public string Prompt
        {
            set
            {
                this.lblText.Text = value;
            }
        }

        /// <summary>
        /// Value entered by the user
        /// </summary>
        /// <remarks>
        /// If the user clicks the Cancel button, the value will be an empty string.
        /// </remarks>
        public string ReturnValue
        {
            get
            {
                return strReturnValue;
            }
        }

        /// <summary>
        /// Optional default response for the input box
        /// </summary>
        public string DefaultResponse
        {
            set
            {
                this.txtResult.Text = value;
                this.txtResult.SelectAll();
            }
        }

        /// <summary>
        /// Optional starting location for the input box window
        /// </summary>
        public Point StartLocation
        {
            set
            {
                this.pntStartLocation = value;
            }
        }
    }
}