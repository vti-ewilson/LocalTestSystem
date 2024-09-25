using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components.Configuration;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// String parameter to be used for application settings, which will automatically
    /// appear in the Edit Cycle form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// StringParameters will appear in the Edit Cycle form with a text box for the
    /// user to enter a value.
    /// </para>
    /// <para>
    /// Order of parameters and default values are determined by the Config class
    /// in the client application. Common parameters and Default Model parameters
    /// are stored in the "C:\Documents and Settings\[user]\Local Settings\Application Data\[Manufacturer]\[Application]\[Version]\user.config" file.
    /// </para>
    /// </remarks>
    /// <seealso cref="BooleanParameter"/>
    /// <seealso cref="EnumParameter{T}"/>
    /// <seealso cref="NumericParameter"/>
    /// <seealso cref="SerialPortParameter"/>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class StringParameter : EditCycleParameter<string>
    {
        #region Fields (2)

        #region Private Fields (2)

        private DropDownParameterControl editorDropDownControl;
        private StringParameterControl editorStringControl;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (2)

        /// <param name="displayName">Friendly name to be displayed in Edit Cycle.</param>
        /// <param name="processValue">Default ProcessValue to be used by all instances of the class.</param>
        /// <param name="toolTip">Description to be displayed in the Edit Cycle form.</param>
        public StringParameter(string displayName, string processValue, string toolTip)
        {
            DisplayName = displayName;
            ProcessValue = processValue;
            ToolTip = toolTip;
            SequenceStep = "";
            OperatingSequence = "";
        }

        /// <param name="displayName">Friendly name to be displayed in Edit Cycle.</param>
        /// <param name="processValue">Default ProcessValue to be used by all instances of the class.</param>
        /// <param name="toolTip">Description to be displayed in the Edit Cycle form.</param>
        /// <param name="sequenceStep">Sequence Step to which this parameter belongs.</param>
        /// <param name="operatingSequence">Operating Sequence to which this parameter belongs.</param>
        public StringParameter(string displayName, string processValue, string toolTip, string sequenceStep, string operatingSequence)
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
        public StringParameter()
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
            if (editorStringControl != null)
            {
                editorStringControl.Value = NewValue;
                return editorStringControl;
            }
            else if (editorDropDownControl != null)
            {
                editorDropDownControl.Value = NewValue;
                return editorDropDownControl;
            }
            else
            {
                // If the StringParameter has a StringSourceAttribute, turn it into a combobox and load the items
                if (propertyInfo.GetCustomAttributes(typeof(StringSourceAttribute), false).Length > 0)
                {
                    editorDropDownControl = new DropDownParameterControl();
                    editorDropDownControl.Description = ToolTip;
                    editorDropDownControl.Items.Clear();
                    StringSourceAttribute attribute = (StringSourceAttribute)propertyInfo.GetCustomAttributes(typeof(StringSourceAttribute), false)[0];

                    editorDropDownControl.Items.AddRange(
                        (attribute
                            .StringSourceType
                            .InvokeMember(attribute.StringSourceMethod, BindingFlags.Default | BindingFlags.InvokeMethod, null, null, null) as string[])
                            .OrderBy(s => s)
                            .Distinct()
                            .ToArray());

                    editorDropDownControl.Value = NewValue;
                    editorDropDownControl.ValueChanged += new EventHandler<EventArgs<string>>(editorControl_ValueChanged);
                    return editorDropDownControl;
                }
                else
                {
                    editorStringControl = new StringParameterControl(ToolTip, NewValue);
                    editorStringControl.ValueChanged += new EventHandler<EventArgs<string>>(editorControl_ValueChanged);
                    return editorStringControl;
                }
            }
        }

        /// <summary>
        /// Not implemented.  StringParameters to don't contain child nodes.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>
        /// nothing
        /// </returns>
        /// <exception cref="NotImplementedException">StringParameter does not contain child nodes.</exception>
        public override Control GetEditorControl(PropertyInfo propertyInfo, string childNodeName)
        {
            throw new NotImplementedException("StringParameter does not contain child nodes.");
        }

        #endregion Public Methods
        #region Protected Methods (1)

        /// <summary>
        /// Called when the new value is changed.
        /// </summary>
        protected override void OnNewValueChanged()
        {
            base.OnNewValueChanged();
            if (editorStringControl != null)
            {
                editorStringControl.Value = NewValue;
            }
            else if (editorDropDownControl != null)
            {
                editorDropDownControl.Value = NewValue;
            }
            if (Updated)
                editorTreeNode.ForeColor = Color.Blue;
            else
                editorTreeNode.ForeColor = Color.Black;
        }

        #endregion Protected Methods
        #region Private Methods (1)

        private void editorControl_ValueChanged(object sender, EventArgs<string> e)
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