using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Implements a label which can display a horizontal indicator behind the text.
    /// </summary>
    public partial class HorizSignalIndicator : UserControl
    {
        #region Globals

        private bool _semiLog = true;
        private int _logMinExp = -7;
        private int _logMaxExp = -2;
        private Single _linMin = 0;
        private Single _linMax = 100;
        private Single _value = 0;
        private Color indicatorColor = Color.Red;
        private Brush indicatorBrush = new SolidBrush(Color.Red);
        private Pen myPen;
        private Brush myBrush = new SolidBrush(Color.Black);
        private Font _font;
        private Bitmap offScreenBmp;
        private Graphics offscreenDC, onscreenDC;
        private ContentAlignment _TextAlign = ContentAlignment.TopLeft;
        private string _text;

        private Action drawIndicatorCallback;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalIndicatorControl">SignalIndicatorControl</see>
        /// </summary>
        public HorizSignalIndicator()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor,
                    true);

            InitGraphics();
            myPen = new Pen(this.ForeColor);
            _font = new Font("Arial", 8);
            //ticBrush = new SolidBrush(this.ForeColor);
            //panelIndicator.Paint += new PaintEventHandler(panelIndicator_Paint);
            drawIndicatorCallback = new Action(DrawIndicator);
        }

        #endregion Construction

        #region Private Methods

        private void InitGraphics()
        {
            // Initialize Win32 BitBlt

            // Offscreen Graph
            if (offScreenBmp != null) offScreenBmp.Dispose();
            offScreenBmp = new Bitmap((int)(this.Width), this.Height);
            Graphics clientDC = this.CreateGraphics();
            IntPtr hdc = clientDC.GetHdc();
            IntPtr memdc = Win32Support.CreateCompatibleDC(hdc);
            Win32Support.SelectObject(memdc, offScreenBmp.GetHbitmap());
            offscreenDC = Graphics.FromHdc(memdc);
            clientDC.ReleaseHdc(hdc);

            // Onscreen Graph
            if (onscreenDC != null) onscreenDC.Dispose();
            onscreenDC = this.CreateGraphics();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //onscreenDC = e.Graphics;
            DrawIndicator();
        }

        private void DrawIndicator()
        {
            if (this.InvokeRequired)
                this.Invoke(drawIndicatorCallback);
            else
            {
                //Rectangle rect;
                //Rectangle indicatorRect;
                double range, scalefactor;
                double factorA, factorB;//, FactorC, FactorD, FactorE;
                int xValue;
                //single y, ticValue;
                //String ticText;

                try
                {
                    //rect = new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height);

                    offscreenDC.Clear(this.BackColor);

                    if (_semiLog)
                    {
                        range = _logMaxExp - _logMinExp;
                        if (range == 0) range = 1;
                        scalefactor = this.Width / range;
                    }
                    else
                    {
                        range = _linMax - _linMin;
                        if (range == 0) range = 1;
                        scalefactor = this.Width / range;
                    }

                    if (Single.IsNaN(_value)) xValue = 0;
                    else
                    {
                        if (_semiLog)
                        {
                            if (_value < 0) xValue = 0;
                            else
                            {
                                factorA = (Double)Math.Pow(10, _logMinExp);
                                factorB = scalefactor / 2.30259F;

                                xValue = (int)(Math.Log((_value + 0.000000000000001) / factorA) * factorB);
                            }
                        }
                        else
                        {
                            xValue = (int)((_value - _linMin) * scalefactor);
                        }
                    }

                    // Draw Indicator Rectangle
                    offscreenDC.FillRectangle(indicatorBrush, 0, 0, xValue, this.Height);

                    float x = 0, y = 0;
                    SizeF textSize = offscreenDC.MeasureString(_text, _font);
                    switch (_TextAlign)
                    {
                        case System.Drawing.ContentAlignment.BottomCenter:
                            x = (this.Width - textSize.Width) / 2;
                            y = this.Height - textSize.Height;
                            break;

                        case System.Drawing.ContentAlignment.BottomLeft:
                            x = 0;
                            y = this.Height - textSize.Height;
                            break;

                        case System.Drawing.ContentAlignment.BottomRight:
                            x = this.Width - textSize.Width;
                            y = this.Height - textSize.Height;
                            break;

                        case System.Drawing.ContentAlignment.MiddleCenter:
                            x = (this.Width - textSize.Width) / 2;
                            y = (this.Height - textSize.Height) / 2;
                            break;

                        case System.Drawing.ContentAlignment.MiddleLeft:
                            x = 0;
                            y = (this.Height - textSize.Height) / 2;
                            break;

                        case System.Drawing.ContentAlignment.MiddleRight:
                            x = this.Width - textSize.Width;
                            y = (this.Height - textSize.Height) / 2;
                            break;

                        case System.Drawing.ContentAlignment.TopCenter:
                            x = (this.Width - textSize.Width) / 2;
                            y = 0;
                            break;

                        case System.Drawing.ContentAlignment.TopLeft:
                            x = 0;
                            y = 0;
                            break;

                        case System.Drawing.ContentAlignment.TopRight:
                            x = this.Width - textSize.Width;
                            y = 0;
                            break;
                    }
                    offscreenDC.DrawString(_text, _font, myBrush, x, y);
                    CopyToScreen();
                }
                catch { }
            }
        }

        private void CopyToScreen()
        {
            IntPtr hdc;
            IntPtr hMemdc;

            // BitBlt from bitmap to screen
            hdc = onscreenDC.GetHdc();
            hMemdc = offscreenDC.GetHdc();
            Win32Support.BitBlt(hdc, 0, 0, (int)((Double)this.Width), this.Height,
                hMemdc, 0, 0, Win32Support.TernaryRasterOperations.SRCCOPY);
            onscreenDC.ReleaseHdc(hdc);
            offscreenDC.ReleaseHdc(hMemdc);
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Resizes the control
        /// </summary>
        /// <param name="e">An System.EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            InitGraphics();
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to indicate if the value should be displayed in Semi-Log format.
        /// </summary>
        public bool SemiLog
        {
            get { return _semiLog; }
            set
            {
                _semiLog = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the minimum exponent to be used in the Semi-Log mode.
        /// </summary>
        public int LogMinExp
        {
            get { return _logMinExp; }
            set
            {
                _logMinExp = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the maximum exponent to be used in the Semi-Log mode.
        /// </summary>
        public int LogMaxExp
        {
            get { return _logMaxExp; }
            set
            {
                _logMaxExp = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the minimum value to be used in the Linear mode.
        /// </summary>
        public Single LinMin
        {
            get { return _linMin; }
            set
            {
                _linMin = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the maximum value to be used in the Linear mode.
        /// </summary>
        public Single LinMax
        {
            get { return _linMax; }
            set
            {
                _linMax = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the value of the indicator.
        /// </summary>
        public Single Value
        {
            get { return _value; }
            set
            {
                _value = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the text associted with the control.
        /// </summary>
        [Bindable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                DrawIndicator();
            }
        }

        private static Font _defaultFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        public new Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                DrawIndicator();
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
        public ContentAlignment TextAlign
        {
            get { return _TextAlign; }
            set
            {
                _TextAlign = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the color of the indicator.
        /// </summary>
        public Color IndicatorColor
        {
            get { return indicatorColor; }
            set
            {
                indicatorColor = value;
                indicatorBrush = new SolidBrush(indicatorColor);
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the Foreground color of the control.
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
                myPen = new Pen(value);
                myBrush = new SolidBrush(value);
                DrawIndicator();
            }
        }

        #endregion Public Properties
    }
}