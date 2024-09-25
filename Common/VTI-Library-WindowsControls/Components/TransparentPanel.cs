using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a panel with a transparent background.
    /// </summary>
    public class TransparentPanel : Panel
    {
        /// <summary>
        /// Encapsulates information needed when creating a control.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        /// <summary>
        /// Overrides the OnPaintBackground of the base Panel, and doesn't paing the background.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }
    }
}