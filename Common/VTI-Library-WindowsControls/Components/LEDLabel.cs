﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a label with an LED (round red light) to the left of the text
    /// </summary>
    public partial class LEDLabel : UserControl
    {
        /// <summary>
        /// Creates an instance of the <see cref="LEDLabel">LEDLabel</see> control.
        /// </summary>
        public LEDLabel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically resized
        /// to display its entire contents.
        /// </summary>
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                label1.AutoSize = true;
            }
        }

        /// <summary>
        /// Raises the SizeChanged event.
        /// </summary>
        /// <param name="e">Eventargs</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            //ovalShape1.Top = (this.Height - this.Margin.Top - this.Margin.Bottom - ovalShape1.Height) / 2;
        }

        /// <summary>
        /// Gets or sets a value to indicate the size in pixels of the LED
        /// </summary>
        public int LEDSize
        {
            get { return ovalControl1.Height; }
            set
            {
                ovalControl1.Height = value;
                ovalControl1.Width = value;
                label1.Left = this.Margin.Left + ovalControl1.Width + 4;
            }
        }

        /// <summary>
        /// Gets or sets the text for the label
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return label1.Text; }
            set
            {
                label1.Text = value;
            }
        }

        private Color _LEDColor = Color.Red;

        /// <summary>
        /// Gets or sets the color of the LED
        /// </summary>
        public Color LEDColor
        {
            get { return _LEDColor; }
            set
            {
                _LEDColor = value;

                HSL hslDim = HSL.FromRGB(value);
                hslDim.Luminance *= 0.25F;
                colorDim = hslDim.RGB;

                SetLEDColor();
            }
        }

        private Color colorDim = Color.DarkRed;

        private bool _Lit = false;

        /// <summary>
        /// Gets or sets a value to indicate if the LED is lit (bright) or not lit (dim)
        /// </summary>
        public bool Lit
        {
            get { return _Lit; }
            set
            {
                if (_Lit != value)
                {
                    _Lit = value;
                    SetLEDColor();
                }
            }
        }

        private void SetLEDColor()
        {
            if (_Lit)
            {
                ovalControl1.FillColor = _LEDColor;
            }
            else
            {
                ovalControl1.FillColor = colorDim;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the label
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                label1.ForeColor = value;
            }
        }

        private Color _LEDBorderColor;

        /// <summary>
        /// Gets or sets the border color of the LED
        /// </summary>
        public Color LEDBorderColor
        {
            get { return _LEDBorderColor; }
            set { _LEDBorderColor = value; }
        }

        private int _LEDBorderWidth;

        /// <summary>
        /// Gets or sets the border width of the LED
        /// </summary>
        public int LEDBorderWidth
        {
            get { return _LEDBorderWidth; }
            set { _LEDBorderWidth = value; }
        }
    }
}