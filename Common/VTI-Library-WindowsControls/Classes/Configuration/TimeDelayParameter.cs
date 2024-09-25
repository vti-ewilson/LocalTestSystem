using System;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components.Configuration;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Time delay parameter to be used for application settings, which will automatically
    /// appear in the Edit Cycle form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// TimeDelayParameters will appear in the Edit Cycle form with a text box for the
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
    public class TimeDelayParameter : NumericParameter<CycleStepTimeDelayUnits>
    {
        private CycleStepTimeDelayUnits _newUnits;

        //private TimeDelayParameterControl editorControl;
        private NumericParameterControl editorControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDelayParameter"/> class.
        /// </summary>
        public TimeDelayParameter()
        {
        }

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
                editorControl = new NumericParameterControl(Units.ToString(), ToolTip, NewValue, MinValue, MaxValue, LargeStep, SmallStep, StringFormat);
                editorControl.ValueChanged += new EventHandler<EventArgs<double>>(editorControl_ValueChanged);
                //editorControl.UnitsChanged += new EventHandler<EventArgs<CycleStepTimeDelayUnits>>(editorControl_UnitsChanged);
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

        /// <summary>
        /// Gets the time delay in seconds, regardless of the value of the <see cref="Units">Units</see> property.
        /// </summary>
        /// <value>The time delay in seconds.</value>
        public double Seconds
        {
            get
            {
                return ProcessValue * (double)Units;
            }
        }

        /// <summary>
        /// Creates the tree node to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>
        /// The tree node to be displayed in the Edit Cycle form
        /// </returns>
        public override TreeNode CreateTreeNode(string nodeName)
        {
            _newUnits = Units;
            return base.CreateTreeNode(nodeName);
        }

        private CycleStepTimeDelayUnits _units = CycleStepTimeDelayUnits.Seconds;

        /// <summary>
        /// Units to be displayed for this parameter (i.e. Seconds, Minutes, Hours)
        /// </summary>
        /// <value></value>
        public override CycleStepTimeDelayUnits Units
        {
            get
            {
                return _units;
            }
            set
            {
                if (_units != value)
                {
                    //MinValue *= (double)_units / (double)value;
                    //MaxValue *= (double)_units / (double)value;
                    //LargeStep *= (double)_units / (double)value;
                    //SmallStep *= (double)_units / (double)value;
                    _units = value;
                    OnUnitsChanged(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the new units.
        /// </summary>
        /// <value>The new units.</value>
        [XmlIgnore()]
        public CycleStepTimeDelayUnits NewUnits
        {
            get { return _newUnits; }
            set
            {
                if (_newUnits != value)
                {
                    _newUnits = value;
                    OnNewUnitsChanged(value);
                }
            }
        }

        /// <summary>
        /// Occurs when time delay units have changed.
        /// </summary>
        public event EventHandler<EventArgs<CycleStepTimeDelayUnits>> UnitsChanged;

        /// <summary>
        /// Called when time delay units have changed.
        /// </summary>
        /// <param name="units">The units.</param>
        protected virtual void OnUnitsChanged(CycleStepTimeDelayUnits units)
        {
            if (UnitsChanged != null)
                UnitsChanged(this, new EventArgs<CycleStepTimeDelayUnits>(units));
        }

        /// <summary>
        /// Occurs when new time delay units have changed.
        /// </summary>
        public event EventHandler<EventArgs<CycleStepTimeDelayUnits>> NewUnitsChanged;

        /// <summary>
        /// Called when new time delay units have changed.
        /// </summary>
        /// <param name="units">The units.</param>
        protected virtual void OnNewUnitsChanged(CycleStepTimeDelayUnits units)
        {
            if (NewUnitsChanged != null)
                NewUnitsChanged(this, new EventArgs<CycleStepTimeDelayUnits>(units));
        }

        /// <summary>
        /// Updates the process value from new value.
        /// </summary>
        /// <returns>
        /// A message indicating that the process value was changed.
        /// </returns>
        public override string UpdateProcessValueFromNewValue()
        {
            string retVal = string.Format("{0} changed from {1} {2} to {3} {4}.",
                DisplayName,
                ProcessValue.ToString(), Units.ToString(),
                NewValue.ToString(), _newUnits.ToString());
            ProcessValue = NewValue;
            if (_newUnits != CycleStepTimeDelayUnits.Unitialized)
                Units = _newUnits;
            // The following block handles the case where occasionally the units get
            //  switched from Seconds to Uninitialized
            if (Units == CycleStepTimeDelayUnits.Unitialized)
                Units = CycleStepTimeDelayUnits.Seconds;
            return retVal;
        }
    }
}