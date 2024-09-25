namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Represents the types of changes that can be reported by the
    /// <see cref="TraceTypeBase{TTrace, TPoint}.Changed">TraceType.Changed</see> event.
    /// </summary>
    public enum TraceChangeType
    {
        /// <summary>
        /// Indicates that the <see cref="Interfaces.IGraphTrace{TTrace, TPoint}">Trace</see> was added.
        /// </summary>
        Added,

        /// <summary>
        /// Indicates that the <see cref="Interfaces.IGraphTrace{TTrace, TPoint}">Trace</see> was removed.
        /// </summary>
        Removed,

        /// <summary>
        /// Indicates that the <see cref="Interfaces.IGraphTrace{TTrace, TPoint}">Trace</see> was replaced.
        /// </summary>
        Replaced,

        /// <summary>
        /// Indicates that the value of the <see cref="Interfaces.IGraphTrace{TTrace, TPoint}">Trace</see> was changed.
        /// </summary>
        Changed,

        /// <summary>
        /// Indicates that the <see cref="Interfaces.IGraphTrace{TTrace, TPoint}">Trace</see> was cleared.
        /// </summary>
        Cleared
    }
}