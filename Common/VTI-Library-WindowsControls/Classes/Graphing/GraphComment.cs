using System.Drawing;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Represents a comment on a
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">Graph Control</see>.
    /// </summary>
    public class GraphComment
    {
        /// <summary>
        /// Gets or sets the text of the comment.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the location of the comment within the data.  Units are in the units of the data.
        /// </summary>
        public PointF Location { get; set; }

        /// <summary>
        /// Gets or sets the offset from the <see cref="Location">Location</see> to the text box.
        /// </summary>
        public Point Offset { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate if the comment is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate if the trace is an overlay trace.
        /// </summary>
        public bool IsOverlay { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CommentControl">CommentControl</see>
        /// which will represent the comment on the screen.
        /// </summary>
        [XmlIgnore()]
        public CommentControl CommentControl { get; set; }
    }
}