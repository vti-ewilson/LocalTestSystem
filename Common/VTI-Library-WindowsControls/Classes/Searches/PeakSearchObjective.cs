namespace VTIWindowsControlLibrary.Classes.Searches
{
    /// <summary>
    /// Represents an objective for a
    /// <see cref="PeakSearch{TData, TCollection, TTrace, TPoint, TSettings}">Peak Search</see>.
    /// </summary>
    public class PeakSearchObjective
    {
        /// <summary>
        /// Gets or sets the value of the location (in the X-Axis) for the target peak.
        /// </summary>
        public int TargetLocation { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate if the matching peak must be a "Major Peak".
        /// Major peaks are defined as being at least 1/5th the height of the tallest peak
        /// in the data, and taller than any other peak within +/- 2 units on the X-Axis.
        /// </summary>
        public bool IsMajor { get; set; }

        /// <summary>
        /// Initializes a new instance of a <see cref="PeakSearchObjective">PeakSearchObjective</see> object.
        /// </summary>
        public PeakSearchObjective()
        {
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="PeakSearchObjective">PeakSearchObjective</see> object.
        /// </summary>
        /// <param name="targetLocation">Location (in the X-Axis) for the target peak</param>
        /// <param name="isMajor">Value to indicate if the matching peak must be a "Major Peak"</param>
        public PeakSearchObjective(int targetLocation, bool isMajor)
        {
            TargetLocation = targetLocation;
            IsMajor = isMajor;
        }
    }
}