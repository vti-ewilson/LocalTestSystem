namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Used to indicate the type of units to be displayed on the X-Axis of the
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.DataPlotControl">DataPlotControl</see>
    /// </summary>
    public enum XAxisDisplayUnitsType
    {
        /// <summary>
        /// Indicates that only the elapsed time (seconds or minutes) are to
        /// be displayed on the X-Axis of the
        /// <see cref="VTIWindowsControlLibrary.Components.Graphing.DataPlotControl">DataPlotControl</see>
        /// </summary>
        SecondsMinutes,

        /// <summary>
        /// Indicates that the Time-of-day in addition to the elapsed time is to
        /// be displayed on the X-Axis of the
        /// <see cref="VTIWindowsControlLibrary.Components.Graphing.DataPlotControl">DataPlotControl</see>
        /// </summary>
        Time,

        /// <summary>
        /// Indicates that the Date and Time in addition to the elapsed time are to
        /// be displayed on the X-Axis of the
        /// <see cref="VTIWindowsControlLibrary.Components.Graphing.DataPlotControl">DataPlotControl</see>
        /// </summary>
        DateTime,

        /// <summary>
        /// Indicates that the Date in addition to the elapsed time is to
        /// be displayed on the X-Axis of the
        /// <see cref="VTIWindowsControlLibrary.Components.Graphing.DataPlotControl">DataPlotControl</see>
        /// </summary>
        Date
    }
}