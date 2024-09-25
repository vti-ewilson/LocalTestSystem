using System;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components.Configuration
{
    /// <summary>
    /// Implements a control which displays an Enable/Disable button for
    /// <see cref="VTIWindowsControlLibrary.Classes.Configuration.BooleanParameter">boolean parameters</see>.
    /// </summary>
    public partial class BooleanParameterControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanParameterControl"/> class.
        /// </summary>
        public BooleanParameterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanParameterControl"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">Value to indicate if the Enable/Disable button should be enabled.</param>
        public BooleanParameterControl(string description, bool value)
        {
            InitializeComponent();
            Description = description;
            Value = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BooleanParameterControl"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Value
        {
            get { return checkBoxEnable.Checked; }
            set { checkBoxEnable.SetChecked(value); }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return textBoxDescription.Text; }
            set { textBoxDescription.SetText(value); }
        }

        private void checkBoxEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnable.Checked)
            {
                checkBoxEnable.Image = VTIWindowsControlLibrary.Properties.Resources.GreenCheck;
                checkBoxEnable.Text = "ENABLED";
            }
            else
            {
                checkBoxEnable.Image = VTIWindowsControlLibrary.Properties.Resources.Red_X;
                checkBoxEnable.Text = "DISABLED";
            }
        }

        /// <summary>
        /// Occurs when the value changes.
        /// </summary>
        public event EventHandler<EventArgs<bool>> ValueChanged;

        /// <summary>
        /// Called when value changes.
        /// </summary>
        /// <param name="value">Value indicating if the Enable/Disable button is enabled.</param>
        protected virtual void OnValueChanged(bool value)
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs<bool>(value));
        }

        private void checkBoxEnable_Leave(object sender, EventArgs e)
        {
            OnValueChanged(checkBoxEnable.Checked);
        }
    }
}