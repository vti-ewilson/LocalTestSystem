namespace VTIWindowsControlLibrary.Enums
{
    /// <summary>
    /// Indicates whether the time delay of a cycle step should be in seconds, minutes, or hours.
    /// </summary>
    public enum CycleStepTimeDelayUnits
    {
        /// <summary>
        /// Indicates that the time delay has not been initialized
        /// </summary>
        Unitialized = 0,

        /// <summary>
        /// Indicates that the time delay of a cycle step should be in seconds
        /// </summary>
        Seconds = 1,

        /// <summary>
        /// Indicates that the time delay of a cycle step should be in minutes
        /// </summary>
        Minutes = 60,

        /// <summary>
        /// Indicates that the time delay of a cycle step should be in hours
        /// </summary>
        Hours = 3600
    }
}