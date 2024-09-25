using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Implements a text box that has a drop-shadow and that resizes to
    /// fit its contents by automatically adjusting its height.
    /// </summary>
    public partial class ShadowTextBox : Panel
    {
        #region Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Text">Text</see> changes
        /// </summary>
        [Browsable(true)]
        public new event EventHandler TextChanged;

        #endregion Event Handlers

        #region Globals

        private System.Windows.Forms.TextBox textBox1;
        private Color _panelColor;
        private Color _borderColor;
        private int shadowSize = 5;
        private int shadowMargin = 2;

        // static for good perfomance
        private static Image shadowDownRight = VTIWindowsControlLibrary.Properties.Resources.tshadowdownright;// new Bitmap(typeof(ShadowTextBox), "Images.tshadowdownright.png");

        private static Image shadowDownLeft = VTIWindowsControlLibrary.Properties.Resources.tshadowdownleft;// new Bitmap(typeof(ShadowTextBox), "Images.tshadowdownleft.png");
        private static Image shadowDown = VTIWindowsControlLibrary.Properties.Resources.tshadowdown;// new Bitmap(typeof(ShadowTextBox), "Images.tshadowdown.png");
        private static Image shadowRight = VTIWindowsControlLibrary.Properties.Resources.tshadowright;// new Bitmap(typeof(ShadowTextBox), "Images.tshadowright.png");
        private static Image shadowTopRight = VTIWindowsControlLibrary.Properties.Resources.tshadowtopright;// new Bitmap(typeof(ShadowTextBox), "Images.tshadowtopright.png");

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowTextBox">ShadowTextBox</see> class
        /// </summary>
        public ShadowTextBox()
        {
            this.Padding = new System.Windows.Forms.Padding(6, 6, 10, 10);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.Controls.Add(this.textBox1);
        }

        #endregion Construction

        #region Events

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.Paint">System.Windows.Forms.Control.Paint</see> event.
        /// Paints the <see cref="ShadowTextBox">ShadowTextBox</see>.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.PaintEventArgs">PaintEventArgs</see> that contains the event data</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Get the graphics object. We need something to draw with ;-)
            Graphics g = e.Graphics;

            // Create tiled brushes for the shadow on the right and at the bottom.
            TextureBrush shadowRightBrush = new TextureBrush(shadowRight, WrapMode.Tile);
            TextureBrush shadowDownBrush = new TextureBrush(shadowDown, WrapMode.Tile);

            // Translate (move) the brushes so the top or left of the image matches the top or left of the
            // area where it's drawed. If you don't understand why this is necessary, comment it out.
            // Hint: The tiling would start at 0,0 of the control, so the shadows will be offset a little.
            shadowDownBrush.TranslateTransform(0, Height - shadowSize);
            shadowRightBrush.TranslateTransform(Width - shadowSize, 0);

            // Define the rectangles that will be filled with the brush.
            // (where the shadow is drawn)
            Rectangle shadowDownRectangle = new Rectangle(
                shadowSize + shadowMargin,                      // X
                Height - shadowSize,                            // Y
                Width - (shadowSize * 2 + shadowMargin),        // width (stretches)
                shadowSize                                      // height
                );

            Rectangle shadowRightRectangle = new Rectangle(
                Width - shadowSize,                             // X
                shadowSize + shadowMargin,                      // Y
                shadowSize,                                     // width
                Height - (shadowSize * 2 + shadowMargin)        // height (stretches)
                );

            // And draw the shadow on the right and at the bottom.
            g.FillRectangle(shadowDownBrush, shadowDownRectangle);
            g.FillRectangle(shadowRightBrush, shadowRightRectangle);

            // Now for the corners, draw the 3 5x5 pixel images.
            g.DrawImage(shadowTopRight, new Rectangle(Width - shadowSize, shadowMargin, shadowSize, shadowSize));
            g.DrawImage(shadowDownRight, new Rectangle(Width - shadowSize, Height - shadowSize, shadowSize, shadowSize));
            g.DrawImage(shadowDownLeft, new Rectangle(shadowMargin, Height - shadowSize, shadowSize, shadowSize));

            // Fill the area inside with the color in the PanelColor property.
            // 1 pixel is added to everything to make the rectangle smaller.
            // This is because the 1 pixel border is actually drawn outside the rectangle.
            Rectangle fullRectangle = new Rectangle(
               1,                                              // X
               1,                                              // Y
               Width - (shadowSize + 2),                       // Width
               Height - (shadowSize + 2)                       // Height
               );

            if (PanelColor != null)
            {
                SolidBrush bgBrush = new SolidBrush(_panelColor);
                g.FillRectangle(bgBrush, fullRectangle);
            }

            // Draw a nice 1 pixel border it a BorderColor is specified
            if (_borderColor != null)
            {
                Pen borderPen = new Pen(BorderColor);
                g.DrawRectangle(borderPen, fullRectangle);
            }

            // Memory efficiency
            shadowDownBrush.Dispose();
            shadowRightBrush.Dispose();

            shadowDownBrush = null;
            shadowRightBrush = null;
        }

        // Correct resizing
        /// <summary>
        /// Fires the event indicating that the <see cref="ShadowTextBox">ShadowTextBox</see> has been resized.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs">System.EventArgs</see> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.Invalidate();
            base.OnResize(e);
            this.ResizeMe();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.ResizeMe();
            this.TextChanged(sender, e);
        }

        private void shadowPanel1_Resize(object sender, EventArgs e)
        {
            this.ResizeMe();
        }

        #endregion Events

        #region Private Methods

        private void ResizeMe()
        {
            if (this.AutoSize)
            {
                using (Graphics g = this.CreateGraphics())
                {
                    //    e.ToolTipSize = TextRenderer.MeasureText(
                    //        toolTip1.GetToolTip(e.AssociatedControl), f, new Size(100, int.MaxValue));
                    Size sz = new Size(this.Width - 4, 500);
                    int LinesFilled, CharsFitted;

                    g.MeasureString(this.Text, this.Font, sz,
                            StringFormat.GenericDefault, out CharsFitted,
                            out LinesFilled);
                    if (LinesFilled == 0) LinesFilled = 1;
                    this.Height = LinesFilled * this.Font.Height + 16;
                }
            }
        }

        #endregion Private Methods

        #region Properties

        /// <summary>
        /// Gets or sets the color to use for the background of the text box
        /// </summary>
        public Color PanelColor
        {
            get { return _panelColor; }
            set { _panelColor = value; }
        }

        /// <summary>
        /// Gets or sets the color to use for the border of the text box
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        /// <summary>
        /// Gets or sets the current text in the text box
        /// </summary>
        [Browsable(true)]
        public override string Text
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }

        #endregion Properties
    }
}