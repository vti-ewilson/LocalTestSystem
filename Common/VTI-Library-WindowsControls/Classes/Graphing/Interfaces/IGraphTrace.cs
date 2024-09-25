using System;
using System.Collections.Generic;
using System.Drawing;

namespace VTIWindowsControlLibrary.Classes.Graphing.Interfaces
{
    /// <summary>
    /// Interface for a list of <see cref="IGraphPoint">points</see> on a
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">Graph Control</see>.
    /// </summary>
    /// <typeparam name="TTrace">Type of trace</typeparam>
    /// <typeparam name="TPoint">Type of point</typeparam>
    public interface IGraphTrace<TTrace, TPoint>
        where TTrace : IGraphTrace<TTrace, TPoint>
        where TPoint : IGraphPoint
    {
        /// <summary>
        /// Occurs when the trace is changed.
        /// </summary>
        event EventHandler<TraceCollectionChangedEventArgs<TTrace, TPoint>> Changed;

        ///// <summary>
        ///// Occurs when the value of the trace is changed.
        ///// </summary>
        //event EventHandler ValueChanged;

        /// <summary>
        /// Gets the Key for the trace.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets or sets the text to be displayed when referring to this trace
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate if the trace is visible
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the color of the trace
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets the list of <see cref="IGraphPoint">points</see> that make up the trace.
        /// </summary>
        List<TPoint> Points { get; }

        /// <summary>
        /// Gets or sets a value to indicate if the trace is an overlay trace.
        /// </summary>
        bool IsOverlay { get; set; }

        /// <summary>
        /// Gets or sets the units of the trace.
        /// </summary>
        string Units { get; set; }

        /// <summary>
        /// Gets or sets the format string to be used when displaying numerical values.
        /// </summary>
        string Format { get; set; }

        /// <summary>
        /// Gets or sets the line width to be used when displaying the trace as a
        /// <see cref="TraceDisplayType.Line">Line</see> or
        /// <see cref="TraceDisplayType.Histogram">Histogram</see>.
        /// </summary>
        int LineWidth { get; set; }

        /// <summary>
        /// Gets or sets the point size to be used when displaying the trace as
        /// <see cref="TraceDisplayType.Point">Points</see>,
        /// <see cref="TraceDisplayType.Cross">Crosses</see> or
        /// <see cref="TraceDisplayType.Plus">Pluses</see>.
        /// </summary>
        int PointSize { get; set; }

        /// <summary>
        /// Order of the trace in the graph.
        /// </summary>
        int ZOrder { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate how to display the trace.
        /// </summary>
        TraceDisplayType DisplayType { get; set; }
    }
}