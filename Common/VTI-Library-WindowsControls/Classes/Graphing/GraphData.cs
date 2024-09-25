using System.Collections.Generic;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;

namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Represents all of the data for a
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">Graph Control</see>.
    /// </summary>
    /// <typeparam name="TCollection">Type of the
    /// <see cref="KeyedTraceCollection{TTrace, TPoint}">Trace Collection</see></typeparam>
    /// <typeparam name="TTrace">Type of the
    /// <see cref="IGraphTrace{TTrace, TPoint}">Trace</see></typeparam>
    /// <typeparam name="TPoint">Type of the <see cref="IGraphPoint">Graph Point</see></typeparam>
    /// <typeparam name="TSettings">Type of the <see cref="GraphSettings">GraphSettings</see></typeparam>
    public class GraphData<TCollection, TTrace, TPoint, TSettings>
        where TCollection : KeyedTraceCollection<TTrace, TPoint>
        where TTrace : class, IGraphTrace<TTrace, TPoint>, new()
        where TPoint : class, IGraphPoint, new()
        where TSettings : GraphSettings
    {
        /// <summary>
        /// Gets or sets the
        /// <see cref="KeyedTraceCollection{TTrace, TPoint}">collection</see> of
        /// <see cref="IGraphTrace{TTrace, TPoint}">traces</see>.
        /// </summary>
        public TCollection Traces { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="GraphComment">comments</see> for this graph.
        /// </summary>
        public List<GraphComment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="GraphSettings">GraphSettings</see> for this graph.
        /// </summary>
        public TSettings Settings { get; set; }
    }
}