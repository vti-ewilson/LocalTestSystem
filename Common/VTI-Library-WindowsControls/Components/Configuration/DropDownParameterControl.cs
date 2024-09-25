using System;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components.Configuration
{
    /// <summary>
    /// Implements a control which displays a drop-down list for
    /// <see cref="VTIWindowsControlLibrary.Classes.Configuration.EnumParameter{T}">enumerated parameters</see>, and
    /// <see cref="VTIWindowsControlLibrary.Classes.Configuration.StringParameter">string parameters</see> with
    /// <see cref="VTIWindowsControlLibrary.Classes.Configuration.StringSourceAttribute">string source attributes</see>.
    /// </summary>
    public partial class DropDownParameterControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownParameterControl"/> class.
        /// </summary>
        public DropDownParameterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownParameterControl"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="items">The items.</param>
        public DropDownParameterControl(string description, string value, string[] items)
        {
            InitializeComponent();
            Description = description;
            Items.AddRange(items);
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return comboBoxProcessValue.Text; }
            set { comboBoxProcessValue.SetText(value); }
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

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public ComboBox.ObjectCollection Items
        {
            get { return comboBoxProcessValue.Items; }
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

        private void comboBoxProcessValue_TextChanged(object sender, EventArgs e)
        {
            OnValueChanged(comboBoxProcessValue.Text);
        }
    }
}