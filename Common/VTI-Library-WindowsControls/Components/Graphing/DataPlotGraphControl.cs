using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VTIWindowsControlLibrary.Classes.Graphing.DataPlot;

namespace VTIWindowsControlLibrary.Components.Graphing
{
    /// <summary>
    /// Intermediate class between the generic
    /// <see cref="GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">GraphControl</see>
    /// class and the non-generic
    /// <see cref="DataPlotGraphControl">DataPlotGraphControl</see> class.
    /// </summary>
    /// <summary>
    /// Data Plot Control which derives from the generic
    /// <see cref="GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">GraphControl</see>
    /// </summary>
    public class DataPlotGraphControl : DataPlotGraphControlNonGenericParent
    {
        /// <summary>
        /// Initializes the <see cref="GraphControl{TData, TCollection, TTrace, TPoint, TSettings}.GraphData">GraphData</see>
        /// object.  Used to add event handlers after the graph data has been retrieved from a file.
        /// </summary>
        protected override void InitGraphData()
        {
            base.InitGraphData();

            GraphData.Traces.Settings = GraphData.Settings;

            if (GraphData.CycleComments == null)
                GraphData.CycleComments = new List<DataPlotCycleComment>();
        }

        /// <summary>
        /// Draws the Cycle Comments when the plot cursor is displayed
        /// </summary>
        /// <param name="x">Location of the plot cursor in the graph data</param>
        protected override void OnDrawFirstPlotCursorCallout(float x)
        {
            StringBuilder sb;

            if (GraphData.CycleComments != null && GraphData.CycleComments.Count > 0)
            {
                var cyclesteps =
                    GraphData.CycleComments
                        .Where(c => c.Comment.EndsWith(" Started"))
                        .Select(c => c.Comment.Replace(" Started", string.Empty))
                        .OrderBy(s => s)
                        .Distinct();

                var activesteps =
                    cyclesteps.Where(s =>
                        {
                            var prev = GraphData.CycleComments.LastOrDefault(
                                c => c.Time <= x && c.Comment.StartsWith(s));

                            if (prev == null || !prev.Comment.EndsWith(" Started")) return false;
                            else return true;
                        });

                sb = new StringBuilder();
                sb.AppendLine("Cycle Steps:");
                foreach (var step in activesteps) sb.AppendLine(step);

                var customcomment =
                    GraphData.CycleComments.LastOrDefault(
                        c => c.Time <= x &&
                            !c.Comment.EndsWith(" Started") &&
                            !c.Comment.EndsWith(" Elapsed") &&
                            !c.Comment.EndsWith(" Passed") &&
                            !c.Comment.EndsWith(" Failed"));

                if (customcomment != null) sb.AppendLine(customcomment.Comment);

                DrawPlotCursorCallout(sb.ToString(), 10, Color.Gray, false, false);
            }

            if (GraphData.IOStates != null && GraphData.IOStates.Count > 0)
            {
                List<string> EnabledIOToShowOnPlot = new List<string>();
                //iterate through each distinct IO point
                foreach (string IOName in GraphData.IOStates.Select(y => y.Name).Distinct())
                {
                    bool isMostRecentStateEnabled = GraphData.IOStates.Where(y => y.Name == IOName && y.Time <= x).OrderByDescending(y => y.Time).Select(y => y.Enabled).FirstOrDefault();
                    if (isMostRecentStateEnabled)
                    {
                        EnabledIOToShowOnPlot.Add(GraphData.IOStates.Where(y => y.Name == IOName).Select(y => y.Description).FirstOrDefault());
                    }
                }

                var enabledIO = GraphData.IOStates.Where(i => i.Time <= x && i.Enabled);
                sb = new StringBuilder();
                sb.AppendLine("Enabled I/O:");
                foreach (string digitalIO in EnabledIOToShowOnPlot) sb.AppendLine(digitalIO);
                DrawPlotCursorCallout(sb.ToString(), 10, Color.Gray, false, true);
            }
        }
    }

    /// <remarks>
    /// This intermediate class is required in order that the Visual Studio designers don't
    /// get confused because by the generic graph control.
    /// </remarks>
    public class DataPlotGraphControlNonGenericParent : GraphControl<DataPlotGraphData, DataPlotTraceCollection, DataPlotTraceType, DataPointType, DataPlotSettings> { }
}