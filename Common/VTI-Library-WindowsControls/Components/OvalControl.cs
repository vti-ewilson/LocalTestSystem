using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// A control which represents an oval
    /// </summary>
    public partial class OvalControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="OvalControl">OvalControl</see>.
        /// </summary>
        public OvalControl()
        {
            InitializeComponent();
        }

        private void OvalControl_Paint(object sender, PaintEventArgs e)
        {
            if (_DrawFilled)
                e.Graphics.FillEllipse(fillBrush, 0, 0, this.Width - 1, this.Height - 1);
            e.Graphics.DrawEllipse(borderPen, 0, 0, this.Width - 1, this.Height - 1);
        }

        private Pen borderPen = new Pen(Color.Black);
        private Color _BorderColor = Color.Black;

        /// <summary>
        /// Gets or sets the border color for the oval
        /// </summary>
        [SettingsBindable(true)]
        public Color BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
                borderPen.Color = _BorderColor;
                this.Invalidate();
            }
        }

        private SolidBrush fillBrush = new SolidBrush(Color.Red);
        private Color _FillColor = Color.Red;

        /// <summary>
        ///  Gets or sets the fill color of the oval
        /// </summary>
        [SettingsBindable(true)]
        public Color FillColor
        {
            get
            {
                return _FillColor;
            }
            set
            {
                _FillColor = value;
                fillBrush.Color = _FillColor;
                this.Invalidate();
            }
        }

        private int _BorderWidth = 1;

        /// <summary>
        /// Gets or sets the border width of the oval
        /// </summary>
        [SettingsBindable(true)]
        public int BorderWidth
        {
            get
            {
                return _BorderWidth;
            }
            set
            {
                _BorderWidth = value;
                borderPen.Width = _BorderWidth;
                this.Invalidate();
            }
        }

        private bool _DrawFilled = true;

        /// <summary>
        ///  Gets or sets a value to indicate if the oval should be filled.
        /// </summary>
        [SettingsBindable(true)]
        public bool DrawFilled
        {
            get
            {
                return _DrawFilled;
            }
            set
            {
                _DrawFilled = value;
                this.Invalidate();
            }
        }
    }
}