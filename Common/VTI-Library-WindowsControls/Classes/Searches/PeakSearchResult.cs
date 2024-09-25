namespace VTIWindowsControlLibrary.Classes.Searches
{
    /// <summary>
    /// Represents a peak which has been located by a
    /// <see cref="PeakSearch{TData, TCollection, TTrace, TPoint, TSettings}">Peak Search</see>.
    /// </summary>
    public class PeakSearchResult
    {
        /// <summary>
        /// Gets or sets the value of the target location (in the X-Axis) for peak.
        /// </summary>
        public int TargetLocation { get; set; }

        /// <summary>
        /// Gets or sets the value of the actual location (in the X-Axis) where the peak was located.
        /// </summary>
        public float ActualLocation { get; set; }

        /// <summary>
        /// Initializes a new instance of a <see cref="PeakSearchResult">PeakSearchResult</see> object.
        /// </summary>
        public PeakSearchResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="PeakSearchResult">PeakSearchResult</see> object.
        /// </summary>
        /// <param name="targetLocation">Target location (in the X-Axis) for peak.</param>
        /// <param name="actualLocation">Actual location (in the X-Axis) where the peak was located.</param>
        public PeakSearchResult(int targetLocation, float actualLocation)
        {
            TargetLocation = targetLocation;
            ActualLocation = actualLocation;
        }
    }
}