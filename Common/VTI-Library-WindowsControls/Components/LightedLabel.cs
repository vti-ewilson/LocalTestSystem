using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a type of label which has a colored background that
    /// can be made bright or dim.
    /// </summary>
    public partial class LightedLabel : UserControl
    {
        private Color colorLit;
        private Color colorDim;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightedLabel">LightedLabel</see> control.
        /// </summary>
        public LightedLabel()
        {
            InitializeComponent();

            colorLit = Color.Red;
            HSL hsl = HSL.FromRGB(colorLit);
            hsl.Luminance *= 0.25F;
            colorDim = hsl.RGB;
        }

        private bool _Lit;

        /// <summary>
        /// Gets or sets a value to indicate if the background color should be bright.
        /// </summary>
        public bool Lit
        {
            get { return _Lit; }
            set
            {
                _Lit = value;
                labelText.BackColor = _Lit ? colorLit : colorDim;
            }
        }

        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        [Bindable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return labelText.Text;
            }
            set
            {
                labelText.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return labelText.BackColor;
            }
            set
            {
                labelText.BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return labelText.ForeColor;
            }
            set
            {
                labelText.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text of the label.
        /// </summary>
        public System.Drawing.ContentAlignment TextAlign
        {
            get { return labelText.TextAlign; }
            set { labelText.TextAlign = value; }
        }
    }
}