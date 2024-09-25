namespace VTIWindowsControlLibrary.Enums
{
    /// <summary>
    /// Used to filter records in the
    /// <see cref="VTIWindowsControlLibrary.Forms.EventLogViewerForm">EventLogViewerForm</see>
    /// </summary>
    public enum DateFilterType
    {
        /// <summary>
        /// Indicates that the <see cref="VTIWindowsControlLibrary.Forms.EventLogViewerForm">EventLogViewerForm</see>
        /// should show all records in the
        /// <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        /// </summary>
        All_Records,

        /// Indicates that the <see cref="VTIWindowsControlLibrary.Forms.EventLogViewerForm">EventLogViewerForm</see>
        /// should show only records from the last 2 hours in the
        /// <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        Last_2_Hours,

        /// Indicates that the <see cref="VTIWindowsControlLibrary.Forms.EventLogViewerForm">EventLogViewerForm</see>
        /// should show only records from the last 12 hours in the
        /// <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        Last_12_Hours,

        /// Indicates that the <see cref="VTIWindowsControlLibrary.Forms.EventLogViewerForm">EventLogViewerForm</see>
        /// should show only records from the last 7 days in the
        /// <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        Last_7_Days
    }
}