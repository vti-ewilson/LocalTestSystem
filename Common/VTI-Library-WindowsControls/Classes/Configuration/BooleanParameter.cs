using System;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components.Configuration;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Boolean parameter to be used for application settings, which will automatically
    /// appear in the Edit Cycle form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// BooleanParameters will appear in the Edit Cycle form as a graphical check-box,
    /// displaying "Enabled" for "true", and "Disabled" for "false".
    /// </para>
    /// <para>
    /// Order of parameters and default values are determined by the Config class
    /// in the client application. Common parameters and Default Model parameters
    /// are stored in the "C:\Documents and Settings\[user]\Local Settings\Application Data\[Manufacturer]\[Application]\[Version]\user.config" file.
    /// </para>
    /// </remarks>
    /// <seealso cref="EnumParameter{T}"/>
    /// <seealso cref="NumericParameter"/>
    /// <seealso cref="SerialPortParameter"/>
    /// <seealso cref="StringParameter"/>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class BooleanParameter : EditCycleParameter<bool>
    {
        #region Fields (1)

        #region Private Fields (1)

        private BooleanParameterControl editorControl;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanParameter"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="processValue">The process value.</param>
        /// <param name="toolTip">The tool tip (description).</param>
        public BooleanParameter(string displayName, bool processValue, string toolTip)
        {
            DisplayName = displayName;
            ProcessValue = processValue;
            ToolTip = toolTip;
            SequenceStep = "";
            OperatingSequence = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanParameter"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="processValue">The process value.</param>
        /// <param name="toolTip">The tool tip (description).</param>
        /// <param name="sequenceStep">Sequence Step to which this parameter belongs.</param>
        /// <param name="operatingSequence">Operating Sequence to which this parameter belongs.</param>
        public BooleanParameter(string displayName, bool processValue, string toolTip, string sequenceStep, string operatingSequence)
        {
            DisplayName = displayName;
            ProcessValue = processValue;
            ToolTip = toolTip;
            SequenceStep = sequenceStep;
            OperatingSequence = operatingSequence;
        }

        /// <remarks>
        /// This parameterless constructor is required for XML serialization
        /// </remarks>
        public BooleanParameter()
        {
        }

        #endregion Constructors

        #region Methods (4)

        #region Public Methods (2)

        /// <summary>
        /// Gets the editor control to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// The editor control to be displayed in the Edit Cycle form.
        /// </returns>
        public override Control GetEditorControl(PropertyInfo propertyInfo)
        {
            if (editorControl == null)
            {
                editorControl = new BooleanParameterControl(ToolTip, NewValue);
                editorControl.ValueChanged += new EventHandler<EventArgs<bool>>(editorControl_ValueChanged);
            }
            else
            {
                editorControl.Value = NewValue;
            }
            return editorControl;
        }

        /// <summary>
        /// Not implemented.  BooleanParameters to don't contain child nodes.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>
        /// nothing
        /// </returns>
        /// <exception cref="NotImplementedException">BooleanParameter does not contain child nodes.</exception>
        public override Control GetEditorControl(PropertyInfo propertyInfo, string childNodeName)
        {
            throw new NotImplementedException("BooleanParameter does not contain child nodes.");
        }

        #endregion Public Methods
        #region Protected Methods (1)

        /// <summary>
        /// Called when new value is changed.
        /// </summary>
        protected override void OnNewValueChanged()
        {
            base.OnNewValueChanged();
            if (editorControl == null)
                return; // happens when user creates a new model
            editorControl.Value = NewValue;
            if (Updated)
                editorTreeNode.ForeColor = Color.Blue;
            else
                editorTreeNode.ForeColor = Color.Black;
        }

        #endregion Protected Methods
        #region Private Methods (1)

        private void editorControl_ValueChanged(object sender, EventArgs<bool> e)
        {
            NewValue = e.Value;
            if (NewValue == ProcessValue)
            {
                editorTreeNode.ForeColor = Color.Black;
            }
            else
            {
                editorTreeNode.ForeColor = Color.Blue;
                Updated = true;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}