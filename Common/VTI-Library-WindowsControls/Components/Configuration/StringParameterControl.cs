using System;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components.Configuration
{
    /// <summary>
    /// Implements a control which displays a text box to enter the value for
    /// <see cref="VTIWindowsControlLibrary.Classes.Configuration.StringParameter">string parameters</see>.
    /// </summary>
    public partial class StringParameterControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringParameterControl"/> class.
        /// </summary>
        public StringParameterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringParameterControl"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        public StringParameterControl(string description, string value)
        {
            InitializeComponent();
            Description = description;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return textBoxProcessValue.Text; }
            set { textBoxProcessValue.SetText(value); }
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

        private void textBoxProcessValue_Leave(object sender, EventArgs e)
        {
            OnValueChanged(textBoxProcessValue.Text);
        }

        /// <summary>
        /// Occurs when value changes.
        /// </summary>
        public event EventHandler<EventArgs<string>> ValueChanged;

        /// <summary>
        /// Called when value changes.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void OnValueChanged(string value)
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs<string>(value));
        }
    }
}