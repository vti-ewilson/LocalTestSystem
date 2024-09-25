﻿namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Represents a "Cycle Comment" which is automatically generated by the
    /// <see cref="VTIWindowsControlLibrary.Classes.CycleSteps.CycleStepsBase">CycleStepsBase</see>
    /// object as the cycle steps are processed.
    /// </summary>
    public class DataPlotCycleComment
    {
        /// <summary>
        /// Gets or sets the elapsed cycle time when the comment was generated
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Gets or sets the text of the cycle comment
        /// </summary>
        public string Comment { get; set; }
    }
}