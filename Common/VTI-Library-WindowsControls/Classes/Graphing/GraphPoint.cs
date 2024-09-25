using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;

namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Basic implementation of the <see cref="IGraphPoint">IGraphPoint</see> interface
    /// for graphical data on a
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">GraphControl</see>.
    /// </summary>
    public class GraphPoint : IGraphPoint
    {
        #region IGraphPoint Members

        /// <summary>
        /// Gets or sets the X-Axis value of the data point
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y-Axis value of the data point
        /// </summary>
        public float Y { get; set; }

        #endregion IGraphPoint Members

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphPoint">GraphPoint</see> class.
        /// </summary>
        public GraphPoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphPoint">GraphPoint</see> class.
        /// </summary>
        /// <param name="x">X-Axis value of the data point</param>
        /// <param name="y">Y-Axis value of the data point</param>
        public GraphPoint(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}