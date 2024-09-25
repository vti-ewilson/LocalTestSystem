using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Enums;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace VTIWindowsControlLibrary.Components.Configuration
{
    public partial class TimeDelayParameterControl : UserControl
    {
        private string _Format = "0.00";
        private CycleStepTimeDelayUnits _units;
        private TimeDelayParameter _timeDelayParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDelayParameterControl"/> class.
        /// </summary>
        public TimeDelayParameterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDelayParameterControl"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="largeChange">The amount the value should change for a large change.</param>
        /// <param name="smallChange">The amount the value should change for a small change.</param>
        public TimeDelayParameterControl(TimeDelayParameter timeDelayParameter)
        {
            InitializeComponent();
            _timeDelayParameter = timeDelayParameter;
            _timeDelayParameter.NewValueChanged += new EventHandler<EventArgs<double>>(_timeDelayParameter_NewValueChanged);
            _timeDelayParameter.NewUnitsChanged += new EventHandler<EventArgs<CycleStepTimeDelayUnits>>(_timeDelayParameter_NewUnitsChanged);
            SetControlValues();
            Value = _timeDelayParameter.ProcessValue;
        }

        void _timeDelayParameter_NewValueChanged(object sender, EventArgs<double> e)
        {
            Value = e.Value;
        }

        void _timeDelayParameter_NewUnitsChanged(object sender, EventArgs<CycleStepTimeDelayUnits> e)
        {
            Units = e.Value;
        }

        private void SetControlValues()
        {
            Units = _timeDelayParameter.Units;
            Description = _timeDelayParameter.DisplayName;
            Minimum = _timeDelayParameter.MinValue;
            Maximum = _timeDelayParameter.MaxValue;
            LargeChange = _timeDelayParameter.LargeStep;
            SmallChange = _timeDelayParameter.SmallStep;
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

        public CycleStepTimeDelayUnits Units
        {
            get
            {
                return _units;
            }
            set 
            {
                _units = value;
                comboBoxUnits.Text = value.ToString();
            }
        }

        private void numericSlider1_Leave(object sender, EventArgs e)
        {
            OnValueChanged(numericSlider1.Value);
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

        public event EventHandler<EventArgs<CycleStepTimeDelayUnits>> UnitsChanged;

        protected virtual void OnUnitsChanged(CycleStepTimeDelayUnits units)
        {
            if (UnitsChanged != null)
                UnitsChanged(this, new EventArgs<CycleStepTimeDelayUnits>(units));
        }

        private void comboBoxUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            CycleStepTimeDelayUnits newUnits = Enum<CycleStepTimeDelayUnits>.Parse(comboBoxUnits.Text);
            double value = Value;
            Minimum = Minimum * ((double)_units / (double)newUnits);
            Maximum = Maximum * ((double)_units / (double)newUnits);
            LargeChange = LargeChange * ((double)_units / (double)newUnits);
            SmallChange = SmallChange * ((double)_units / (double)newUnits);
            Value = value * ((double)_units / (double)newUnits);
            _units = newUnits;
            OnValueChanged(Value);
            OnUnitsChanged(_units);
        }
    }
}
