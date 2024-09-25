using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    public partial class SchematicLabel : Label
    {
        public SchematicLabel()
        {
            InitializeComponent();
        }

        private Size _OriginalSize;
        private Point _OriginalLocation;

        /// <summary>
        /// Gets or sets the original size of the SchematicCheckBox. Used in resizing the schematic form.
        /// </summary>
        [Browsable(false)]
        public Size OriginalSize
        {
            get { return _OriginalSize; }
            set { _OriginalSize = value; }
        }

        /// <summary>
        /// Gets or sets the original location of the SchematicCheckBox. Used in resizing the schematic form.
        /// </summary>
        [Browsable(false)]
        public Point OriginalLocation
        {
            get { return _OriginalLocation; }
            set { _OriginalLocation = value; }
        }
    }
}