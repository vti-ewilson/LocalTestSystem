using System;
using System.Reflection;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Classes.Configuration.Interfaces
{
    /// <summary>
    /// Interface which must be implemented by all edit cycle parameters (NumericParameter, BooleanParameter,
    /// StringParameter, etc.)
    /// </summary>
    public interface IEditCycleParameter
    {
        /// <summary>
        /// Event handler to be used when the ProcessValue changes.
        /// </summary>
        event EventHandler ProcessValueChanged;

        /// <summary>
        /// Gets or sets the value to be displayed for the name of the paramter in the Edit Cycle window.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate if the paramter should be displayed in the Edit Cycle window.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Description to be displayed in the Edit Cycle form.
        /// </summary>
        String ToolTip { get; set; }

        /// <summary>
        /// Sequence Step to which this parameter belongs.
        /// </summary>
        String SequenceStep { get; set; }

        /// <summary>
        /// Operating Sequence that uses this parameter.
        /// </summary>
        String OperatingSequence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IEditCycleParameter"/> is updated.
        /// </summary>
        /// <value><c>true</c> if updated; otherwise, <c>false</c>.</value>
        bool Updated { get; set; }

        /// <summary>
        /// Uses the node name to create a tree node to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>The tree node to be displayed in the Edit Cycle form</returns>
        TreeNode CreateTreeNode(string nodeName);

        /// <summary>
        /// Used the property info to create a tree node to be displayed in the Edit Cycle form.
        /// </summary>
        /// <returns>The tree node to be displayed in the Edit Cycle form</returns>
        TreeNode CreateTreeNode(PropertyInfo propertyInfo, IEditCycleParameter ecParam, object ob);

        /// <summary>
        /// Gets the editor control to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>The editor control to be displayed in the Edit Cycle form.</returns>
        Control GetEditorControl(PropertyInfo propertyInfo);

        /// <summary>
        /// Gets the editor control to be displayed in the Edit Cycle form for the specified child node.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>The editor control to be displayed in the Edit Cycle form for the specified child node.</returns>
        Control GetEditorControl(PropertyInfo propertyInfo, string childNodeName);

        /// <summary>
        /// Updates the process value from new value.
        /// </summary>
        /// <returns></returns>
        string UpdateProcessValueFromNewValue();

        /// <summary>
        /// Gets or sets the new value string.
        /// </summary>
        /// <value>The new value string.</value>
        string NewValueString { get; set; }

        /// <summary>
        /// Gets or sets the process value string.
        /// </summary>
        /// <value>The process value string.</value>
        string ProcessValueString { get; set; }
    }

    /// <summary>
    /// Generic interface for edit cycle parameters
    /// </summary>
    /// <typeparam name="T">Type of te ProcessValue</typeparam>
    public interface IEditCycleParameter<T> : IEditCycleParameter
    {
        /// <summary>
        /// Gets or sets the new value.
        /// </summary>
        /// <value>The new value.</value>
        T NewValue { get; set; }

        /// <summary>
        /// Gets or sets the process value.
        /// </summary>
        /// <value>The process value.</value>
        T ProcessValue { get; set; }
    }
}