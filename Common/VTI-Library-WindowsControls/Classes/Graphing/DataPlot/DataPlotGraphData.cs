using System.Collections.Generic;

namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// A
    /// <see cref="GraphData{DataPlotTraceCollection, DataPlotTraceType, DataPointType, DataPlotSettings}">GraphData</see>
    /// class for the
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.DataPlotGraphControl">DataPlotGraphControl</see>.
    /// </summary>
    public class DataPlotGraphData : GraphData<DataPlotTraceCollection, DataPlotTraceType, DataPointType, DataPlotSettings>
    {
        /// <summary>
        /// Gets or sets a list of <see cref="DataPlotCycleComment">Cycle Comments</see>.  These are automatically
        /// added by the <see cref="VTIWindowsControlLibrary.Classes.CycleSteps.CycleStepsBase">CycleStepsBase</see> object as the cycle steps
        /// are processed.
        /// </summary>
        public List<DataPlotCycleComment> CycleComments { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="DataPlotIOState">I/O States</see>. These are automatically
        /// added when the I/O states change and the data plot is running
        /// </summary>
        /// <value>The IO states.</value>
        public List<DataPlotIOState> IOStates { get; set; }
    }
}