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
    /// Numeric parameter to be used for application settings, which will automatically
    /// appear in the Edit Cycle form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// NumericParameters will appear in the Edit Cycle form with a text box for the
    /// user to enter a value.  It will also include a slider bar for adjusting the value, and
    /// the value can be entered via the numeric keypad which is intended to be used with a touchscreen.
    /// </para>
    /// <para>
    /// Order of parameters and default values are determined by the Config class
    /// in the client application. Common parameters and Default Model parameters
    /// are stored in the "C:\Documents and Settings\[user]\Local Settings\Application Data\[Manufacturer]\[Application]\[Version]\user.config" file.
    /// </para>
    /// </remarks>
    /// <seealso cref="BooleanParameter"/>
    /// <seealso cref="EnumParameter{T}"/>
    /// <seealso cref="SerialPortParameter"/>
    /// <seealso cref="StringParameter"/>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class NumericParameter : NumericParameter<string>
    {
        private NumericParameterControl editorControl;

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
                editorControl = new NumericParameterControl(Units, ToolTip, NewValue, MinValue, MaxValue, LargeStep, SmallStep, StringFormat);
                editorControl.ValueChanged += new EventHandler<EventArgs<double>>(editorControl_ValueChanged);
            }
            else
            {
                editorControl.Value = NewValue;
            }
            return editorControl;
        }

        /// <summary>
        /// Not implemented.  NumericParameters to don't contain child nodes.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>
        /// nothing
        /// </returns>
        /// <exception cref="NotImplementedException">NumericParameter does not contain child nodes.</exception>
        public override Control GetEditorControl(PropertyInfo propertyInfo, string childNodeName)
        {
            throw new NotImplementedException("NumericParameter does not contain child nodes.");
        }

        /// <summary>
        /// Called when the new value is changed.
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

        private void editorControl_ValueChanged(object sender, EventArgs<double> e)
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
    }
}