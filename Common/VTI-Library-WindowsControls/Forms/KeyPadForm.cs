using System;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents a form that contains a complete alpha-numeric keypad for
    /// use on a touch screen.
    /// </summary>
    public partial class KeyPadForm : Form
    {
        private int AlphaNumButtonWidth;
        private int AlphaNumButtonHeight;

        /// <summary>
        /// Sets a value to indicate that the characters should be displayed as
        /// asterisks (*) for entering a password.
        /// </summary>
        public Boolean Password
        {
            set
            {
                if (value) textBox1.PasswordChar = Convert.ToChar("*");
                else textBox1.PasswordChar = Convert.ToChar(0);
            }
        }

        /// <summary>
        /// Gets or sets the text of the form
        /// </summary>
        public string KeyPadText
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        /// <summary>
        /// Initializes Button Width for nums and alpha
        /// </summary>
        public int KeyPadButtonWidth
        {
            set
            {
                AlphaNumButtonWidth = value;
            }
        }

        /// <summary>
        /// Initializes Button Height for nums and alpha
        /// </summary>
        public int KeyPadButtonHeight
        {
            set
            {
                AlphaNumButtonHeight = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPadForm">KeyPadForm</see>
        /// </summary>
        public KeyPadForm()
        {
            InitializeComponent();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPadForm_KeyDown);
            this.KeyPreview = true;
        }

        private void frmKeyPad_Load(object sender, EventArgs e)
        {
            String KeyPadButtons = "1234567890!QWERTYUIOPASDFGHJKL@ZXCVBNM ";
            Button button;
            int i = 1;
            this.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            foreach (Char c in KeyPadButtons)
            {
                button = new Button();
                button.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button.Location = new System.Drawing.Point(3, 3);
                if (c.ToString() == "!")
                {
                    button.Name = "buttonBksp";
                    button.Text = "BkSp";
                    button.Size = new System.Drawing.Size(120, 60);// buttonBksp button size
                    button.Click += new System.EventHandler(this.buttonBksp_Click);
                    button.ForeColor = Color.Orange;
                }
                else if (c.ToString() == "@")
                {
                    button.Name = "buttonOK";
                    button.Text = "OK";
                    button.Size = new System.Drawing.Size(120, 60);// buttonOK button size
                    button.Margin = new Padding(69, 3, 3, 3);
                    button.Click += new System.EventHandler(this.buttonOK_Click);
                    button.ForeColor = Color.DarkGreen;
                    this.AcceptButton = button;
                }
                else if (c.ToString() == " ")
                {
                    button.Name = "buttonSpace";
                    button.Text = " ";
                    button.Size = new System.Drawing.Size(120, 60);// buttonSpace button size
                    button.Click += new System.EventHandler(this.button_Click);
                }
                else
                {
                    button.Name = "button" + c.ToString();
                    button.Text = c.ToString();
                    //button.Size = new System.Drawing.Size(60, 60); // button size for 1234567890!QWERTYUIOPASDFGHJKL@ZXCVBNM
                    button.Size = new System.Drawing.Size(AlphaNumButtonWidth, AlphaNumButtonHeight); // button size for 1234567890!QWERTYUIOPASDFGHJKL@ZXCVBNM

                    button.Click += new System.EventHandler(this.button_Click);
                }
                if (c.ToString() == "Q" || c.ToString() == "Z")
                    button.Margin = new Padding(33, 3, 3, 3);
                if (c.ToString() == "P")
                    button.Margin = new Padding(3, 3, 63, 3);
                button.TabIndex = i;
                button.UseVisualStyleBackColor = true;
                this.flowLayoutPanel1.Controls.Add(button);
                button.SendToBack();
                i++;
            }
            this.flowLayoutPanel1.ResumeLayout();
            this.ResumeLayout();
            textBox1.Select();
            Application.DoEvents();
        }

        private void button_Click(object sender, EventArgs e)
        {
            textBox1.Text += ((Button)sender).Text;
        }

        private void buttonBksp_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmKeyPad_Activated(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Select();
        }

        private void KeyPadForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}