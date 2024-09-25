namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Determines how much the Data Plot
    /// shifts in the X-Axis when the data reaches the end of the window
    /// </summary>
    public enum XWindowShiftType
    {
        /// <summary>
        /// Indicates that the X-Axis should shift continuously.
        /// This makes the
        /// <see cref="VTIWindowsControlLibrary.Components.Graphing.DataPlotControl">DataPlotControl</see>
        /// operate like a strip chart.
        /// </summary>
        Continuous = 0,

        /// <summary>
        /// Indicates that the X-Axis should shift 1% of the total width when the
        /// data reaches the end of the window.
        /// This produces an affect similar to the <see cref="Continuous">Continuous</see>
        /// option, but is slightly better in CPU utilization.
        /// </summary>
        One_Percent = 1,

        /// <summary>
        /// Indicates that the X-Axis should shift 5% of the total width when the
        /// data reaches the end of the window
        /// </summary>
        Five_Percent = 5,

        /// <summary>
        /// Indicates that the X-Axis should shift 10% of the total width when the
        /// data reaches the end of the window.
        /// This is the most typical setting.
        /// </summary>
        Ten_Percent = 10
    }
}