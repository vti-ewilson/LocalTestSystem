using System;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a text box with a horizontal scroll bar which can be presented
    /// on either side of the text box.  This creates a slider control similar to the
    /// <see cref="NumericUpDown">NumbericUpDown</see> control, but with a larger, horizontal slider.
    /// </summary>
    public partial class NumericSlider : UserControl
    {
        #region Fields (7)

        #region Private Fields (7)

        private double _LargeChange = 10;
        private double _Maximum = 100;
        private double _Minimum = 0;
        private bool _sliderOnLeft = true;
        private double _SmallChange = 1;
        private double _Value = double.NaN; // set to NaN to force initial setting of text box if initial value is zero
        private string formatString = "0";

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref= "NumericSlider">NumericSlider</see> control.
        /// </summary>
        public NumericSlider()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (8)

        /// <summary>
        ///  Gets or sets the value that indicates how much the value should change
        /// when the horizontal scroll bar is moved by a large distance.
        /// </summary>
        public double LargeChange
        {
            get { return _LargeChange; }
            set
            {
                //if (value <= _SmallChange) throw new ArgumentOutOfRangeException("Large Change must be greater than the Small Change.");
                _LargeChange = value;
                SetHScroll();
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the control.
        /// </summary>
        public double Maximum
        {
            get { return _Maximum; }
            set
            {
                _Maximum = value;
                SetHScroll();
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the control.
        /// </summary>
        public double Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;
                SetHScroll();
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the horizontal scroll bar
        /// should appear on the left side of the text box.
        /// </summary>
        public bool SliderOnLeft
        {
            get { return _sliderOnLeft; }
            set
            {
                _sliderOnLeft = value;
                if (_sliderOnLeft)
                {
                    if (hScrollBar1.Parent == splitContainer1.Panel2)
                    {
                        splitContainer1.Panel1.Controls.Remove(textBox1);
                        splitContainer1.Panel2.Controls.Remove(hScrollBar1);
                        splitContainer1.Panel1.Controls.Add(hScrollBar1);
                        splitContainer1.Panel2.Controls.Add(textBox1);
                    }
                }
                else
                {
                    if (hScrollBar1.Parent == splitContainer1.Panel1)
                    {
                        splitContainer1.Panel1.Controls.Remove(hScrollBar1);
                        splitContainer1.Panel2.Controls.Remove(textBox1);
                        splitContainer1.Panel1.Controls.Add(textBox1);
                        splitContainer1.Panel2.Controls.Add(hScrollBar1);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that indicates how much the value should change
        /// when the user clicks on the arrows on either end of the horizontal scroll bar.
        /// </summary>
        public double SmallChange
        {
            get { return _SmallChange; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Small Change must be greater than zero.");
                _SmallChange = value;
                SetHScroll();
                if (Math.Log10(_SmallChange) >= 0) formatString = "0";
                else
                {
                    int decimals = (int)Math.Ceiling(-Math.Log10(_SmallChange));
                    formatString = "0." + new string('0', decimals);
                }
            }
        }

        /// <summary>
        /// Gets or sets the location of the split between the horizontal scroll
        /// bar and the text box, in pixels, from the left edge of the control.
        /// </summary>
        public int SplitterDistance
        {
            get { return splitContainer1.SplitterDistance; }
            set { splitContainer1.SplitterDistance = value; }
        }

        /// <summary>
        /// Gets or sets the current text of the control.
        /// </summary>
        public new string Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        /// <summary>
        /// Gets or sets the current value of the control.
        /// </summary>
        public double Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    try
                    {
                        hScrollBar1.ThreadSafeAction(
                            delegate { hScrollBar1.Value = (int)Math.Round(_Value / _SmallChange); });
                    }
                    catch (Exception ex)
                    {
                        VtiEvent.Log.WriteWarning("hScrollbar assignment out of range", VtiEventCatType.Application_Error);
                    }
                    textBox1.SetText(value.ToString(formatString));
                    OnValueChanged();
                    //SetTextBox();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the text box.
        /// </summary>
        /// <value>The color of the text box.</value>
        public Color TextBoxColor
        {
            get { return textBox1.ForeColor; }
            set { textBox1.ForeColor = value; }
        }

        private static Font _defaultFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Drawing.Font"/> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"/> property.
        /// </returns>
        public new Font Font
        {
            get
            {
                return textBox1.Font;
            }
            set
            {
                textBox1.Font = value;
            }
        }

        private new void ResetFont()
        {
            Font = _defaultFont;
        }

        private bool ShouldSerializeFont()
        {
            return (!Font.Equals(_defaultFont));
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>The text alignment.</value>
        public HorizontalAlignment TextAlign
        {
            get { return textBox1.TextAlign; }
            set { textBox1.TextAlign = value; }
        }

        #endregion Properties

        #region Methods (5)

        #region Private Methods (5)

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            Value = (double)hScrollBar1.Value * _SmallChange;
            //if (_Value != (double)hScrollBar1.Value * _SmallChange)
            //{
            //    Value = (double)hScrollBar1.Value * _SmallChange;
            //    //_Value = (double)hScrollBar1.Value * _SmallChange;
            //    //textBox1.Text = _Value.ToString(formatString);
            //    //OnValueChanged();
            //}
        }

        private void SetHScroll()
        {
            hScrollBar1.SuspendLayout();
            try
            {
                hScrollBar1.Minimum = (int)Math.Round(_Minimum / _SmallChange);
                hScrollBar1.Maximum = (int)Math.Round(_Maximum / _SmallChange);
                hScrollBar1.LargeChange = (int)Math.Round(_LargeChange / _SmallChange);
            }
            catch { }   // Empty try-catch so Minimum and Maximum can be set in either order without blowing up
            hScrollBar1.ResumeLayout();
        }

        private void SetValueFromTextBox()
        {
            double tempval;
            if (double.TryParse(textBox1.Text, out tempval) &&
                tempval >= _Minimum && tempval <= _Maximum)
            {
                Value = tempval;
                //if (Value != tempval)
                //{
                //    //_Value = tempval;
                //    //hScrollBar1.Value = (int)(_Value / _SmallChange);
                //    Value = tempval;
                //}
            }
            else textBox1.SetText(_Value.ToString(formatString));
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SetValueFromTextBox();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            SetValueFromTextBox();
        }

        #endregion Private Methods

        #endregion Methods

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }
    }
}