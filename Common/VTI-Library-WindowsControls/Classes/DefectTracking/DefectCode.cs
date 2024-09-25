namespace VTIWindowsControlLibrary.Classes.DefectTracking
{
    /// <summary>
    /// Defines a Defect Code
    /// </summary>
    public class DefectCode
    {
        /// <summary>
        /// Value of the defect code
        /// </summary>
        /// <remarks>
        /// Typically, this is a coded value that isn't a friendly, human-readable value.
        /// </remarks>
        public string Value { get; set; }

        /// <summary>
        /// Category for the defect code
        /// </summary>
        /// <remarks>
        /// Typical categories might be AutoBraze, LineBraze, Supplier, etc.
        /// </remarks>
        public string Category { get; set; }

        /// <summary>
        /// Description for the defect code
        /// </summary>
        /// <remarks>
        /// This is the friendlier, human-readable value to be displayed on the
        /// <see cref="VTIWindowsControlLibrary.Forms.DefectEntryForm">DefectEntryForm</see>
        /// </remarks>
        public string Description { get; set; }
    }
}