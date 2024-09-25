using System;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// A UserControl that presents a numerical keypad on-screen.
    /// </summary>
    /// <remarks>Intended for touch-screen use.</remarks>
    public partial class NumberPadControl : UserControl
    {
        private Double _CurrentSetting;

        /// <summary>
        /// Minimum value that the control will generate.
        /// </summary>
        public Double MinValue = Double.MinValue;

        /// <summary>
        /// Maximum value that the control will generate.
        /// </summary>
        public Double MaxValue = Double.MaxValue;

        /// <summary>
        /// Occurs when the value of the Numerical Keypad changes
        /// </summary>
        public event CurrentSettingChangedEventHandler CurrentSettingChanged;

        /// <summary>
        /// Raises the <see cref="CurrentSettingChanged">CurrentSettingChanged</see> event
        /// </summary>
        /// <param name="e">Event arguments for the <see cref="CurrentSettingChanged">CurrentSettingChanged</see> event</param>
        protected virtual void OnCurrentSettingChanged(CurrentSettingChangedEventArgs e)
        {
            CurrentSettingChanged(this, e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberPadControl">NumberPadControl</see>
        /// </summary>
        public NumberPadControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "9";
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += ".";
        }

        private void button0_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "0";
        }

        private void buttonMinus_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "-";
        }

        private void buttonE_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text += "E";
        }

        private void buttonCE_Click(object sender, EventArgs e)
        {
            this.textBoxCurrentSetting.Text = string.Empty;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            Double newValue;
            try
            {
                newValue = Double.Parse(this.textBoxCurrentSetting.Text);
                if ((newValue <= this.MaxValue) && (newValue >= this.MinValue))
                {
                    _CurrentSetting = newValue;
                    this.textBoxCurrentSetting.Text = string.Empty;
                    OnCurrentSettingChanged(new CurrentSettingChangedEventArgs(_CurrentSetting));
                }
                else
                    MessageBox.Show("Value entered is out of range!", "Numeric Keypad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch
            {
                MessageBox.Show("Please enter a valid numerical value!", "Numeric Keypad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonBksp_Click(object sender, EventArgs e)
        {
            if (this.textBoxCurrentSetting.Text.Length > 0)
                this.textBoxCurrentSetting.Text = this.textBoxCurrentSetting.Text.Substring(0, this.textBoxCurrentSetting.Text.Length - 1);
        }

        /// <summary>
        /// Gets or sets the current value of the NumberPadControl
        /// </summary>
        public Double CurrentSetting
        {
            get
            {
                return this._CurrentSetting;
            }
            set
            {
                _CurrentSetting = value;
                //if ((value == 0) || (Math.Abs(value) > 0.01))
                //    this.textBoxCurrentSetting.Text = value.ToString("0.00");
                //else
                //    this.textBoxCurrentSetting.Text = value.ToString("0.00E-00");
            }
        }
    }

    /// <summary>
    /// Event arguments for the <see cref="NumberPadControl.CurrentSettingChanged">NumberPadControl.CurrentSettingChanged</see> event
    /// </summary>
    public class CurrentSettingChangedEventArgs : EventArgs
    {
        private Double currentSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSettingChangedEventArgs">CurrentSettingChangedEventArgs</see>
        /// </summary>
        /// <param name="CurrentSetting">New value of the NumberPad control</param>
        public CurrentSettingChangedEventArgs(Double CurrentSetting)
        {
            this.currentSetting = CurrentSetting;
        }

        /// <summary>
        /// Gets the new value of the NumberPad control
        /// </summary>
        public Double CurrentSetting
        {
            get
            {
                return currentSetting;
            }
            set
            {
                currentSetting = value;
            }
        }
    }

    /// <summary>
    /// Delegate for calling the <see cref="CurrentSettingChangedEventArgs">CurrentSettingChangedEventArgs</see> event
    /// </summary>
    /// <param name="sender">Object calling the event</param>
    /// <param name="e">Event arguments</param>
    public delegate void CurrentSettingChangedEventHandler(object sender, CurrentSettingChangedEventArgs e);
}