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
    /// Enumerated parameter to be used for application settings, which will automatically
    /// appear in the Edit Cycle form.
    /// </summary>
    /// <typeparam name="T">Enumerated Type to be used for the EnumParameter</typeparam>
    /// <remarks>
    /// <para>
    /// EnumParameters will appear in the Edit Cycle form as a drop-down list displaying all of the
    /// items of the enumerated type.
    /// </para>
    /// <para>
    /// Order of parameters and default values are determined by the Config class
    /// in the client application. Common parameters and Default Model parameters
    /// are stored in the "C:\Documents and Settings\[user]\Local Settings\Application Data\[Manufacturer]\[Application]\[Version]\user.config" file.
    /// </para>
    /// </remarks>
    /// <seealso cref="BooleanParameter"/>
    /// <seealso cref="NumericParameter"/>
    /// <seealso cref="SerialPortParameter"/>
    /// <seealso cref="StringParameter"/>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class EnumParameter<T> : EditCycleParameter<T>
    {
        #region Fields (1)

        #region Private Fields (1)

        private DropDownParameterControl editorControl;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (2)

        /// <param name="displayName">Friendly name to be displayed in Edit Cycle.</param>
        /// <param name="processValue">Default ProcessValue to be used by all instances of the class</param>
        /// <param name="toolTip">Description to be displayed in the Edit Cycle form</param>
        public EnumParameter(String displayName, T processValue, String toolTip)
        {
            DisplayName = displayName;
            ProcessValue = processValue;
            ToolTip = toolTip;
            SequenceStep = "";
            OperatingSequence = "";
        }

        /// <param name="displayName">Friendly name to be displayed in Edit Cycle.</param>
        /// <param name="processValue">Default ProcessValue to be used by all instances of the class</param>
        /// <param name="toolTip">Description to be displayed in the Edit Cycle form</param>
        /// <param name="sequenceStep">Sequence Step to which this parameter belongs.</param>
        /// <param name="operatingSequence">Operating Sequence to which this parameter belongs.</param>
        public EnumParameter(String displayName, T processValue, String toolTip, String sequenceStep, string operatingSequence)
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
        public EnumParameter()
        {
        }

        #endregion Constructors

        #region Methods (4)

        #region Public Methods (2)

        /// <summary>
        /// When implemented in a derived class, gets the editor control to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// The editor control to be displayed in the Edit Cycle form.
        /// </returns>
        public override Control GetEditorControl(PropertyInfo propertyInfo)
        {
            if (editorControl == null)
            {
                editorControl = new DropDownParameterControl();
                editorControl.Description = ToolTip;

                editorControl.Items.Clear();
                foreach (var field in typeof(T).GetFields())
                    if (field.FieldType == typeof(T))
                        editorControl.Items.Add(field.GetValue(null).ToString().Replace('_', ' '));

                editorControl.Value = NewValue.ToString().Replace('_', ' ');
                editorControl.ValueChanged += new EventHandler<EventArgs<string>>(editorControl_ValueChanged);
            }
            else
            {
                editorControl.Value = NewValue.ToString().Replace('_', ' ');
            }
            return editorControl;
        }

        /// <summary>
        /// Not implemented.  EnumParameters to don't contain child nodes.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>
        /// nothing
        /// </returns>
        /// <exception cref="NotImplementedException">EnumParameter does not contain child nodes.</exception>
        public override Control GetEditorControl(PropertyInfo propertyInfo, string childNodeName)
        {
            throw new NotImplementedException("EnumParameter does not contain child nodes.");
        }

        #endregion Public Methods
        #region Protected Methods (1)

        /// <summary>
        /// Called when the new value is changed.
        /// </summary>
        protected override void OnNewValueChanged()
        {
            base.OnNewValueChanged();
            if (editorControl == null)
                return; // happens when user creates a new model
            editorControl.Value = NewValue.ToString().Replace('_', ' ');
            if (Updated)
                editorTreeNode.ForeColor = Color.Blue;
            else
                editorTreeNode.ForeColor = Color.Black;
        }

        #endregion Protected Methods
        #region Private Methods (1)

        private void editorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue = Enum<T>.Parse(e.Value.Replace(' ', '_'));
            if (NewValue.Equals(ProcessValue))
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