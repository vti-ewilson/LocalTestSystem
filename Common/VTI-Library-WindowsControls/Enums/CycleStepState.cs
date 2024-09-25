namespace VTIWindowsControlLibrary.Enums
{
    /// <summary>
    /// Indicates the current state of a
    /// <see cref="VTIWindowsControlLibrary.Classes.CycleSteps.CycleStep">CycleStep</see>
    /// </summary>
    public enum CycleStepState
    {
        /// <summary>
        /// Indicates that the cycle step is idle
        /// </summary>
        Idle,

        /// <summary>
        /// Indicates that the cycle step is in process
        /// </summary>
        InProcess,

        /// <summary>
        /// Indicates that the cycle step has failed
        /// </summary>
        Passed,

        /// <summary>
        /// Indicates that the cycle step has passed
        /// </summary>
        Failed,

        /// <summary>
        /// Indicates that the cycle step has elapsed without a pass/fail determination
        /// </summary>
        Elapsed
    }
}