using System;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a vertical bar (thermometer style) control which can be used to display
    /// an analog signal value in both linear and semi-log formats.
    /// </summary>
    public partial class SignalIndicatorControl : UserControl
    {
        #region Globals

        private bool semiLog = true;
        private int logMinExp = -7;
        private int logMaxExp = -2;
        private Single linMin = 0;
        private Single linMax = 100;
        private Single value = 0;
        private Color indicatorColor = Color.Red;
        private Brush indicatorBrush = new SolidBrush(Color.Red);
        private Pen myPen;
        private Brush ticBrush;
        private Font ticFont = new Font("Arial", 8);
        private Bitmap offScreenBmp;
        private Graphics offscreenDC, onscreenDC;

        private Action drawIndicatorCallback;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalIndicatorControl">SignalIndicatorControl</see>
        /// </summary>
        public SignalIndicatorControl()
        {
            InitializeComponent();
            InitGraphics();
            myPen = new Pen(this.ForeColor);
            ticBrush = new SolidBrush(this.ForeColor);
            panelIndicator.Paint += new PaintEventHandler(panelIndicator_Paint);
            drawIndicatorCallback = new Action(DrawIndicator);
        }

        #endregion Construction

        #region Private Methods

        private void InitGraphics()
        {
            try
            {
                // Initialize Win32 BitBlt

                // Offscreen Graph
                if (offScreenBmp != null) offScreenBmp.Dispose();
                offScreenBmp = new Bitmap((int)(this.panelIndicator.Width), this.panelIndicator.Height);
                Graphics clientDC = this.CreateGraphics();
                IntPtr hdc = clientDC.GetHdc();
                IntPtr memdc = Win32Support.CreateCompatibleDC(hdc);
                Win32Support.SelectObject(memdc, offScreenBmp.GetHbitmap());
                offscreenDC = Graphics.FromHdc(memdc);
                clientDC.ReleaseHdc(hdc);

                // Onscreen Graph
                if (onscreenDC != null) onscreenDC.Dispose();
                onscreenDC = this.panelIndicator.CreateGraphics();
            }
            catch (Exception ex)
            {
                // offscreen invalid param

            }
        }

        private void panelIndicator_Paint(object sender, PaintEventArgs e)
        {
            DrawIndicator();
        }

        private void DrawIndicator()
        {
            if (this.InvokeRequired)
                this.Invoke(drawIndicatorCallback);
            else
            {
                Rectangle rect;
                Rectangle indicatorRect;
                Double YRange, YScaleFactor;
                Double FactorA, FactorB;//, FactorC, FactorD, FactorE;
                Double YValue;
                Single y, ticValue;
                String ticText;

                try
                {
                    rect = new Rectangle(0, 0, this.panelIndicator.Bounds.Width - 1, this.panelIndicator.Bounds.Height - 1);
                    //base.OnPaint(e);
                    offscreenDC.Clear(this.BackColor);

                    if (this.semiLog)
                    {
                        YRange = logMaxExp - logMinExp;
                        if (YRange == 0) YRange = 1;
                        YScaleFactor = rect.Height / YRange;
                    }
                    else
                    {
                        YRange = linMax - linMin; //lin min gets set to 269.001.. somewhere
                        //if ((YRange <= Single (0.0)) = true) {
                        //  Console.WriteLine("Out of order");
                        //} // gets in this with a higher linmin calcs a negative YRange and gets stuck below at "hang place #1 8/29/12"
                        if (YRange == 0) YRange = 1;
                        if (YRange <= 0)
                        {
                            YRange = 1.0;
                        }
                        YScaleFactor = rect.Height / YRange;  // if YRange is neg then YScaleFactor is neg
                    }

                    if (Single.IsNaN(value)) YValue = 0;
                    else
                    {
                        if (semiLog)
                        {
                            if (value < 0) YValue = 0;
                            else
                            {
                                FactorA = (Double)Math.Pow(10, logMinExp);
                                FactorB = YScaleFactor / 2.30259F;

                                //FactorE = GraphBounds.Bottom + (int)(Settings.YMin * YScaleFactor);
                                YValue = rect.Bottom - (int)(Math.Log((value + 0.000000000000001) / FactorA) * FactorB);
                            }
                        }
                        else
                        {
                            YValue = rect.Bottom - (int)((value - linMin) * YScaleFactor);
                        }
                    }

                    // Draw Indicator Rectangle
                    indicatorRect = new Rectangle(0, (int)YValue, rect.Width, rect.Height - (int)YValue);
                    offscreenDC.FillRectangle(indicatorBrush, indicatorRect);
                    // Draw Outline Rectangle
                    offscreenDC.DrawRectangle(myPen, rect);
                    // Draw hash marks
                    if (semiLog)
                    {
                        for (y = (Single)rect.Height / (Single)Math.Abs(YRange), ticValue = logMaxExp - 1;
                             ticValue >= logMinExp || y <= rect.Height;
                             y += (Single)rect.Height / (Single)Math.Abs(YRange), ticValue--)
                        {
                            offscreenDC.DrawLine(myPen, 0, y, rect.Width, y);
                            ticText = String.Format("1E{0:0}", ticValue);
                            offscreenDC.DrawString(ticText, ticFont, ticBrush,
                                (rect.Width - offscreenDC.MeasureString(ticText, ticFont).Width) / 2,
                                y - offscreenDC.MeasureString(ticText, ticFont).Height);
                        }
                    }
                    else
                    {
                        for (y = (Single)rect.Height / 10F, ticValue = linMax - 0.1F * (Single)YRange;
                             ticValue >= linMin || y <= rect.Height;
                             y += (Single)rect.Height / 10F, ticValue -= 0.1F * (Single)YRange)
                        {  //"hang place #1 8/29/12" - check YRange if < 0 above
                            offscreenDC.DrawLine(myPen, 0, y, rect.Width, y);
                            ticText = String.Format("{0:0}", ticValue);
                            offscreenDC.DrawString(ticText, ticFont, ticBrush,
                                (rect.Width - offscreenDC.MeasureString(ticText, ticFont).Width) / 2,
                                y - offscreenDC.MeasureString(ticText, ticFont).Height);
                        }
                    }

                    IntPtr hdc;
                    IntPtr hMemdc;

                    // BitBlt from bitmap to screen
                    hdc = onscreenDC.GetHdc();
                    hMemdc = offscreenDC.GetHdc();
                    Win32Support.BitBlt(hdc, 0, 0, (int)((Double)this.panelIndicator.Width), this.panelIndicator.Height,
                        hMemdc, 0, 0, Win32Support.TernaryRasterOperations.SRCCOPY);
                    onscreenDC.ReleaseHdc(hdc);
                    offscreenDC.ReleaseHdc(hMemdc);
                }
                catch { }
            }
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
            get { return semiLog; }
            set
            {
                semiLog = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the minimum exponent to be used in the Semi-Log mode.
        /// </summary>
        public int LogMinExp
        {
            get { return logMinExp; }
            set
            {
                logMinExp = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the maximum exponent to be used in the Semi-Log mode.
        /// </summary>
        public int LogMaxExp
        {
            get { return logMaxExp; }
            set
            {
                logMaxExp = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the minimum value to be used in the Linear mode.
        /// </summary>
        public Single LinMin
        {
            get { return linMin; }
            set
            {
                linMin = value;
                if (value > 200)
                {
                    //System.Diagnostics.Debugger.Break();
                }
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the maximum value to be used in the Linear mode.
        /// </summary>
        public Single LinMax
        {
            get { return linMax; }
            set
            {
                linMax = value;
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets the value of the indicator.
        /// </summary>
        public Single Value
        {
            get { return value; }
            set
            {
                this.value = value;
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
        /// Gets or sets the caption of the control.
        /// </summary>
        public string Caption
        {
            get { return this.lblCaption.Text; }
            set { this.lblCaption.Text = value; }
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
                ticBrush = new SolidBrush(value);
                DrawIndicator();
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate that the "X" (close) button should be visible.
        /// </summary>
        public Boolean ShowXButton
        {
            get { return this.buttonHide.Visible; }
            set { this.buttonHide.Visible = value; }
        }

        #endregion Public Properties
    }
}