namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Types of ways to display a trace on a graph control.
    /// </summary>
    public enum TraceDisplayType
    {
        /// <summary>
        /// Display the trace as a solid line
        /// </summary>
        Line,

        /// <summary>
        /// Display the trace as a solid line, filled in solid to the bottom of the Y-Axis
        /// </summary>
        Fill,

        /// <summary>
        /// Display the trace as a vertical line at each data point
        /// </summary>
        Histogram,

        /// <summary>
        /// Display the trace as a dot at each data point
        /// </summary>
        Point,

        /// <summary>
        /// Display the trace as a cross (X) at each data point
        /// </summary>
        Cross,

        /// <summary>
        /// Display the trace as a plus (+) at each data point
        /// </summary>
        Plus
    }
}