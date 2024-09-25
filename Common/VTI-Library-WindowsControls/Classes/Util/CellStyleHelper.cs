using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// Contains a helper method for setting the <see cref="DataGridViewCellStyle">cell syle</see>
    /// for cells in the <see cref="VTIWindowsControlLibrary.Forms.EventLogViewerForm">EventLogViewerForm</see>
    /// </summary>
	public class CellStyleHelper
    {
        /// <summary>
        /// Returns a <see cref="DataGridViewCellStyle">cell syle</see> with a specific
        /// <see cref="DataGridViewCellStyle.BackColor">BackColor</see>.
        /// </summary>
        /// <param name="color">Color to be used</param>
        /// <returns>Cell style with a specific BackColor</returns>
		public static DataGridViewCellStyle GetColorStyle(System.Drawing.Color color)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = color;
            return style;
        }
    }
}