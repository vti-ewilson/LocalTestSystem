using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a TextBox that automatically resizes to its contents.
    /// </summary>
    public partial class AutoSizeTextBox : TextBox
    {
        #region Private

        private Boolean _autoSize = true;

        #endregion Private

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSizeTextBox">AutoSizeTextBox</see>
        /// </summary>
        public AutoSizeTextBox()
        {
            this.TextChanged += new EventHandler(AutoSizeTextBox_TextChanged);
            this.Multiline = true;
        }

        #endregion Construction

        #region Events

        /// <summary>
        /// Raises the Resize event
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnResize(EventArgs e)
        {
            base.Invalidate();
            base.OnResize(e);
            this.ResizeMe();
        }

        private void AutoSizeTextBox_TextChanged(object sender, EventArgs e)
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
                    Size sz = new Size(this.Width + 4, 500);
                    int LinesFilled, CharsFitted;
                    g.MeasureString(this.Text, this.Font, sz,
                            StringFormat.GenericDefault, out CharsFitted,
                            out LinesFilled);
                    if (LinesFilled == 0)
                        LinesFilled = 1;
                    if (Text.Length >= 2)
                    {
                        if (Text.Substring(Text.Length - 2, 2) == "\r\n")
                            LinesFilled++;
                    }
                    this.Height = LinesFilled * this.Font.Height + 6;
                }
            }
        }

        #endregion Private Methods

        #region Properties

        /// <summary>
        /// Gets or sets a value to indicate if the text box should automatically resize.
        /// </summary>
        [Browsable(true)]
        public override Boolean AutoSize
        {
            get { return _autoSize; }
            set { _autoSize = value; }
        }

        #endregion Properties
    }
}