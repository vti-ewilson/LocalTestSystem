using System;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components.Configuration
{
    /// <summary>
    /// Implements a control which displays
    /// <see cref="VTIWindowsControlLibrary.Components.NumericSlider">numeric slider</see>
    /// for adjusting the value of
    /// <see cref="VTIWindowsControlLibrary.Classes.Configuration.NumericParameter">numeric parameters</see>.
    /// </summary>
    public partial class NumericParameterControl : UserControl
    {
        private string _Format = "0.00E-00";

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericParameterControl"/> class.
        /// </summary>
        public NumericParameterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericParameterControl"/> class.
        /// </summary>
        /// <param name="units">The units (ie. psi, torr, seconds, minutes, etc.)</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="largeChange">The amount the value should change for a large change.</param>
        /// <param name="smallChange">The amount the value should change for a small change.</param>
        /// <param name="format">The format to be used when displaying the value.</param>
        public NumericParameterControl(string units, string description, double value, double minimum, double maximum, double largeChange, double smallChange, string format)
        {
            InitializeComponent();
            Description = string.Format("({0}) {1}", units, description);
            _Format = format;
            Minimum = minimum;
            Maximum = maximum;
            LargeChange = largeChange;
            SmallChange = smallChange;
            if (value < minimum) Value = minimum;
            else if (value > maximum) Value = maximum;
            else Value = value;
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public double Minimum
        {
            get { return numericSlider1.Minimum; }
            set
            {
                numericSlider1.ThreadSafeAction(delegate { numericSlider1.Minimum = value; });
                labelMinValue.SetText(value.ToString(_Format));
            }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public double Maximum
        {
            get { return numericSlider1.Maximum; }
            set
            {
                numericSlider1.ThreadSafeAction(delegate { numericSlider1.Maximum = value; });
                labelMaxValue.SetText(value.ToString(_Format));
            }
        }

        /// <summary>
        /// Gets or sets the amount the value should change for a large change.
        /// </summary>
        /// <value>The amount the value should change for a large change.</value>
        public double LargeChange
        {
            get { return numericSlider1.LargeChange; }
            set
            {
                numericSlider1.ThreadSafeAction(delegate { numericSlider1.LargeChange = value; });
            }
        }

        /// <summary>
        /// Gets or sets the amount the value should change for a small change.
        /// </summary>
        /// <value>The amount the value should change for a small change.</value>
        public double SmallChange
        {
            get { return numericSlider1.SmallChange; }
            set
            {
                numericSlider1.ThreadSafeAction(delegate { numericSlider1.SmallChange = value; });
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get { return numericSlider1.Value; }
            set
            {
                numericSlider1.ThreadSafeAction(delegate { numericSlider1.Value = value; });
                labelProcessValue.SetText(value.ToString(_Format));
            }
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
        /// Occurs when value changes.
        /// </summary>
        public event EventHandler<EventArgs<double>> ValueChanged;

        /// <summary>
        /// Called when value changes.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void OnValueChanged(double value)
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs<double>(value));
        }

        private void numericSlider1_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(numericSlider1.Value);
        }
    }
}