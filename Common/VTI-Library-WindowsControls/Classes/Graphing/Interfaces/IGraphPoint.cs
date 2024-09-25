namespace VTIWindowsControlLibrary.Classes.Graphing.Interfaces
{
    /// <summary>
    /// Interface for a graphical point on a
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">Graph Control</see>.
    /// </summary>
    public interface IGraphPoint
    {
        /// <summary>
        /// Gets or sets the X-Axis value of the data point
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// Gets or sets the Y-Axis value of the data point
        /// </summary>
        float Y { get; set; }
    }
}