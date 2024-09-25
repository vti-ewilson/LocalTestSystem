namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Value to hold the state of a Digital I/O at a given point of time for the data plot.
    /// </summary>
    public struct DataPlotIOState
    {
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public float Time { get; set; }

        /// <summary>
        /// Gets or sets the name of the Digital I/O.
        /// </summary>
        /// <value>The name of the Digital I/O.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the Digital I/O.
        /// </summary>
        /// <value>The description of the Digital I/O.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Digital I/O is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }
    }
}