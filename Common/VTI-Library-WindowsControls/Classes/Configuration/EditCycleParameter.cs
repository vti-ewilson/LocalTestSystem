using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.Configuration.Interfaces;
using VTIWindowsControlLibrary.Classes.CycleSteps;
using VTIWindowsControlLibrary.Classes.Data;
using VTIWindowsControlLibrary.Classes.ManualCommands;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Enums;
using VTIWindowsControlLibrary.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static VTIWindowsControlLibrary.Forms.EditCycleForm;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// A generic, abstract class from which all edit cycle parameter classes should derive.
    /// </summary>
    /// <typeparam name="T">Type of the process value.</typeparam>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public abstract class EditCycleParameter<T> : IEditCycleParameter<T>
    {
        #region Fields (5)

        #region Public Fields (4)

        /// <summary>
        /// Stores the new value if changed by the user in the
        /// <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>
        /// </summary>
        [XmlIgnore()]
        public T NewValue
        {
            get { return _NewValue; }
            set
            {
                _NewValue = value;
                _Updated = (!(_NewValue.Equals(_ProcessValue)));
                OnNewValueChanged();
            }
        }

        private bool _Updated = false;

        /// <summary>
        /// Indicates if the parameter has been changed by the user in the
        /// <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>
        /// </summary>
        [XmlIgnore]
        public bool Updated
        {
            get
            {
                return _Updated;
            }
            set
            {
                _Updated = value;
            }
        }

        #endregion Public Fields
        #region Private Fields (1)

        private bool _Visible = true;
        private T _NewValue;
        private T _ProcessValue;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (2)

        /// <param name="displayName">Friendly name to be displayed in Edit Cycle.</param>
        /// <param name="processValue">Default ProcessValue to be used by all instances of the class.</param>
        /// <param name="toolTip">Description to be displayed in the Edit Cycle form.</param>
        public EditCycleParameter(string displayName, T processValue, string toolTip)
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
        public EditCycleParameter(string displayName, T processValue, string toolTip, string sequenceStep, string operatingSequence)
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
        public EditCycleParameter()
        {
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Name to be displayed in the Edit Cycle form, and when the parameter is
        /// referred to in Event Logs and UutRecordDetail records.
        /// </summary>
        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Current value of the parameter.
        /// </summary>
        [XmlElement("ProcessValue")]
        public T ProcessValue
        {
            get { return _ProcessValue; }
            set
            {
                #region system_paramchangelog (broken)
                //            try {
                //	if(_ProcessValue != null && !_ProcessValue.Equals(value) && VtiLib.Machine != null
                //	&& VtiLib.Machine.ManualCommandsInstance != null && VtiLib.IO != null
                //	&& !VtiLib.MuteParamChangeLog) {
                //		if(DisplayName != null && DisplayName != "") {
                //			this.Updated = true;
                //			ToolTip = "test";
                //			List<Data.ChangedParameter> listOfChangedParameters = new List<Classes.Data.ChangedParameter>();
                //			Data.ChangedParameter cp = new Data.ChangedParameter();
                //			cp.paramName = DisplayName;
                //			cp.OpID = "SYSTEM";
                //			cp.newValue = value.ToString();
                //			cp.oldValue = _ProcessValue.ToString();


                //			TreeNode treeNode1;
                //			System.Windows.Forms.TreeView treeView1 = new System.Windows.Forms.TreeView();
                //			// Get the members of the Config type
                //			PropertyInfo[] properties = VtiLib.Config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
                //			foreach(PropertyInfo property in properties) {
                //				if(property.PropertyType.IsSubclassOf(typeof(ApplicationSettingsBase)) && (property.Name != "CurrentModel") && (property.Name != "IO")) {
                //					object objectSettings = property.GetValue(null, null);

                //					if(objectSettings != null) {
                //						// Create the tree node for the current settings object
                //						treeNode1 = new TreeNode();
                //						treeNode1.Name = property.Name;

                //						if(property.Name == "DefaultModel")
                //							treeNode1.Text = "Model DEFAULT Parameters";
                //						else
                //							treeNode1.Text = "Common " + property.Name + " Parameters";

                //						treeNode1.Tag = objectSettings;  // save objectSettings in the tag of the TreeNode
                //						treeView1.Nodes.Add(treeNode1);

                //						treeNode1.Nodes.Add("Place Holder");
                //					}
                //				}
                //			}

                //			if(Properties.Settings.Default.UsesVtiDataDatabase) {
                //				// Load Model Parameters (just the top level treenodes for now)
                //				foreach(Model model in VtiLib.Data.Models) {
                //					// Create the tree node for the current settings object
                //					treeNode1 = new TreeNode();
                //					treeNode1.Name = "Model " + model.ModelNo;
                //					treeNode1.Text = "Model " + model.ModelNo + " Parameters";
                //					treeNode1.Tag = model.ModelNo; // hang onto the name for now
                //					treeView1.Nodes.Add(treeNode1);

                //					//object objectSettings = Activator.CreateInstance(VtiLib.ModelSettingsType) as ModelSettingsBase;
                //					//((ModelSettingsBase)objectSettings).Name = treeNode1.Tag.ToString();
                //					//treeNode1.Tag = objectSettings;
                //					//Model mdl = VtiLib.Data.Models.Single(m => m.ModelNo == ((ModelSettingsBase)objectSettings).Name);

                //					treeNode1.Nodes.Add("Place Holder");
                //				}
                //			}

                //			//get parameter section name and System ID
                //			foreach(TreeNode node in treeView1.Nodes) {
                //				if((node.Level == 0) && (node.Nodes.Count > 0)) {
                //					#region build node
                //					TreeNode treeNode = null;
                //					ModelSettingsBase modelSettingsToCopy;

                //					foreach(TreeNode node0 in treeView1.Nodes) {
                //						if(node0.Text == "Model DEFAULT Parameters") {
                //							treeNode = node0;
                //							break;
                //						}
                //					}

                //					modelSettingsToCopy = treeNode.Tag as ModelSettingsBase;
                //					node.Nodes.Clear();

                //					#endregion


                //					object objectSettings = node.Tag;

                //					// Common Control/Mode/Pressure/Flow/Time parameters
                //					if(node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters")) {
                //						foreach(PropertyInfo property in objectSettings.GetType().GetProperties()) {
                //							if(property.PropertyType.GetInterface("IEditCycleParameter") != null) {
                //								// Find display name from xml string custom attribute
                //								Regex rx = new Regex(@"<DisplayName>([\s\S]*?)<\/DisplayName>");
                //								string propertyAttributes = property.CustomAttributes.ToList()[2].ToString();
                //								string propertyDisplayName = rx.Match(propertyAttributes).Groups[1].Value;

                //								if(cp.paramSectionName == null && propertyDisplayName == DisplayName) {
                //									cp.paramSectionName = node.Text;
                //								}
                //								if(cp.SystemID == null && property.Name.Replace("_", " ").Equals("System ID", StringComparison.CurrentCultureIgnoreCase) || property.Name.Replace("_", " ").Equals("SystemID", StringComparison.CurrentCultureIgnoreCase)) {
                //									IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                //									cp.SystemID = editCycleParameter.ProcessValueString;
                //								}
                //							}
                //						}
                //					}
                //					// Default and Custom Model parameters
                //					else //if (cp.paramSectionName == null)
                //					{
                //						objectSettings = Activator.CreateInstance(VtiLib.ModelSettingsType) as ModelSettingsBase;
                //						((ModelSettingsBase)objectSettings).Name = node.Tag.ToString();
                //						node.Tag = objectSettings;
                //						Model model = VtiLib.Data.Models.Single(m => m.ModelNo == ((ModelSettingsBase)objectSettings).Name);

                //						foreach(PropertyInfo property in objectSettings.GetType().GetProperties()) {
                //							if(property.PropertyType.GetInterface("IEditCycleParameter") != null) {
                //								IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                //								IEditCycleParameter editCycleParameterToCopy = property.GetValue(modelSettingsToCopy, null) as IEditCycleParameter;
                //								if(editCycleParameterToCopy.Visible) {
                //									TreeNode newNode = editCycleParameter.CreateTreeNode(property, editCycleParameter, this);
                //									newNode.Tag = property;
                //									node.Nodes.Add(newNode);

                //									ModelParameter modelParameter = model.ModelParameters.FirstOrDefault(p => p.ParameterID == property.Name);
                //									// Parameters exist in database
                //									if(modelParameter != null) {
                //										editCycleParameter.NewValueString =
                //											editCycleParameter.ProcessValueString =
                //											modelParameter.ProcessValue;
                //										editCycleParameter.Updated = false;
                //									}
                //									// Parameter was added which isn't in the database.  Get the default value!
                //									else {
                //										object defaultSettings = Activator.CreateInstance(VtiLib.ModelSettingsType);
                //										IEditCycleParameter defaultParameter = property.GetValue(defaultSettings, null) as IEditCycleParameter;
                //										editCycleParameter.NewValueString = defaultParameter.ProcessValueString;
                //										editCycleParameter.Updated = true;
                //									}
                //								}





                //								if(node.Text.Contains("test") && property.Name.Replace("_", " ") == DisplayName) //if (cp.paramSectionName == null && property.Name.Replace("_", " ") == DisplayName)
                //								{
                //									IEditCycleParameter param = property.GetValue(objectSettings, null) as IEditCycleParameter;
                //									var yp = param.NewValueString;
                //									var o = param.ProcessValueString;
                //									if(param.ProcessValueString == cp.oldValue) {
                //										cp.paramSectionName = node.Text;
                //									}
                //								}
                //								//IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                //								//if (editCycleParameter.DisplayName == DisplayName)
                //								//{
                //								//    cp.paramSectionName = node.Text;
                //								//}
                //							}
                //						}
                //					}
                //				}
                //			}
                //			if(cp.SystemID != null && cp.paramSectionName != null) {
                //				listOfChangedParameters.Add(cp);
                //				EditCycle.editCycle.LogParameterChanges(listOfChangedParameters);
                //			}


                //		}
                //	}
                //} catch(Exception ex) {}
                #endregion

                _ProcessValue = value;
                OnProcessValueChanged();
            }
        }

        /// <summary>
        /// Gets or sets the process value as a string.
        /// </summary>
        /// <value>The process value string.</value>
        [XmlIgnore()]
        public virtual string ProcessValueString
        {
            get { return _ProcessValue.ToString(); }
            set
            {
                T val;
                if (ExtensionMethods.TryParse<T>(value, out val))
                    _ProcessValue = val;
                else
                    VtiEvent.Log.WriteVerbose(
                       string.Format(VtiLibLocalization.ErrorSettingProcessValue, DisplayName, value),
                       VtiEventCatType.Parameter_Update);
            }
        }

        /// <summary>
        /// Gets or sets the new value as a string.
        /// </summary>
        /// <value>The new value string.</value>
        [XmlIgnore()]
        public virtual string NewValueString
        {
            get { return _NewValue == null ? "" : _NewValue.ToString(); }
            set
            {
                T val;
                if (ExtensionMethods.TryParse<T>(value, out val))
                    _NewValue = val;
            }
        }

        /// <summary>
        /// Description to be displayed in the Edit Cycle form.
        /// </summary>
        [XmlElement("ToolTip")]
        public string ToolTip 
        { get; 
            set; }

        /// <summary>
        /// Sequence Step to which this parameter belongs.
        /// </summary>
        [XmlElement("SequenceStep")]
        public string SequenceStep { get; set; }

        /// <summary>
        /// Operating Sequence to which this parameter belongs.
        /// </summary>
        [XmlElement("OperatingSequence")]
        public string OperatingSequence { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate if the parameter should be displayed
        /// in the Edit Cycle window.  Defaults to true.
        /// </summary>
        [XmlIgnore()]
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }

        #endregion Properties

        #region Delegates and Events (1)

        #region Events (1)

        /// <summary>
        /// Occurs when the <see cref="ProcessValue">ProcessValue</see> changes
        /// </summary>
        public event EventHandler ProcessValueChanged;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (6)

        #region Public Methods (5)

        /// <summary>
        /// When two EditCycleParameters are compared, this operator compares the ProcessValues.
        /// Thus, two EditCycleParameters will be considered inequal if the ProcessValues are inequal.
        /// </summary>
        /// <param name="left">Left-side EditCycleParameter</param>
        /// <param name="right">Right-side EditCycleParameter</param>
        /// <returns>Boolean value indicating whether or not the ProcessValues of the two paramters are inequal.</returns>
        public static bool operator !=(EditCycleParameter<T> left, EditCycleParameter<T> right)
        {
            return (!(left == right));
        }

        /// <summary>
        /// When two EditCycleParameters are compared, this operator compares the ProcessValues.
        /// Thus, two EditCycleParameters will be considered equal if the ProcessValues are the same.
        /// </summary>
        /// <param name="left">Left-side EditCycleParameter</param>
        /// <param name="right">Right-side EditCycleParameter</param>
        /// <returns>Boolean value indicating whether or not the ProcessValues of the two paramters are equal.</returns>
        public static bool operator ==(EditCycleParameter<T> left, EditCycleParameter<T> right)
        {
            object n = null;

            //  Both are null.
            if (object.ReferenceEquals(n, left) && object.ReferenceEquals(n, right))
                return true;

            //  Neither are null and process values are equal.
            return (!object.ReferenceEquals(n, left) && !object.ReferenceEquals(n, right) &&
                left.ProcessValue.Equals(right.ProcessValue));
        }

        /// <summary>
        /// Determines whether the specified
        /// <see cref="System.Object">System.Object</see> is equal to this
        /// <see cref="StringParameter">StringParameter</see>
        /// </summary>
        /// <param name="obj">The <see cref="System.Object">System.Object</see> to compare with this
        /// <see cref="StringParameter">StringParameter</see></param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is EditCycleParameter<T>)
                return (obj as EditCycleParameter<T>) == this;

            return false;
        }

        /// <summary>
        /// Gets the hash code for the parameter.
        /// </summary>
        public override int GetHashCode()
        {
            return this.DisplayName.GetHashCode();
        }

        /// <summary>
        /// Implicitly converts a StringParameter to String.
        /// </summary>
        /// <param name="x">StringParameter</param>
        /// <returns>String (value of the StringParameter.ProcessValue)</returns>
        public static implicit operator T(EditCycleParameter<T> x)
        {
            return x.ProcessValue;
        }

        /// <summary>
        /// The editor tree node.
        /// </summary>
        protected TreeNode editorTreeNode;

        /// <summary>
        /// Uses the node name to create a tree node to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>The tree node to be displayed in the Edit Cycle form</returns>
        public virtual TreeNode CreateTreeNode(string nodeName)
        {
            _NewValue = _ProcessValue;
            _Updated = false;

            editorTreeNode = new TreeNode();
            editorTreeNode.Name = nodeName;
            editorTreeNode.Text = DisplayName;
            return editorTreeNode;
        }

        /// <summary>
        /// Uses the property info to create a tree node to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>The tree node to be displayed in the Edit Cycle form</returns>
        public virtual TreeNode CreateTreeNode(PropertyInfo propertyInfo, IEditCycleParameter ecParam, object ob)
        {
            _NewValue = _ProcessValue;
            _Updated = false;

            editorTreeNode = new TreeNode();
            editorTreeNode.Name = propertyInfo.Name;
            editorTreeNode.Text = DisplayName;
            try
            {
                EditCycleForm ecf = ob as EditCycleForm;
                if (ecParam.GetType().Name == "StringParameter")
                    editorTreeNode.BackColor = ecf.EditCycleColor(EditCycleForm.EditCycleType.Control);
                else if (ecParam.GetType().Name == "TimeDelayParameter")
                    editorTreeNode.BackColor = ecf.EditCycleColor(EditCycleForm.EditCycleType.Time);
                else if (ecParam.GetType().Name == "NumericParameter")
                {
                    NumericParameter numericParameter = ecParam as NumericParameter;
                    string strUnits = numericParameter.Units.ToLower();
                    if (strUnits.Contains("cc/s") || strUnits.Contains("cc/min") || strUnits.Contains("cc/h") || strUnits.Contains("oz/yr") || strUnits.Contains("torr-l/s") || strUnits.Contains("g/y") || strUnits.Contains("mbar-l/") || strUnits.Contains("pa-m3/"))
                        editorTreeNode.BackColor = ecf.EditCycleColor(EditCycleForm.EditCycleType.Flow);
                    else if (strUnits.Equals("torr") || strUnits.Equals("mtorr") || strUnits.Equals("atm") || strUnits.Equals("bar") || strUnits.Equals("mbar") || strUnits.Contains("kgf/cm2") || strUnits.Equals("kpa") || strUnits.Equals("mpa") || strUnits.Equals("psi") || strUnits.Equals("psig") || strUnits.Equals("psia"))
                        editorTreeNode.BackColor = ecf.EditCycleColor(EditCycleForm.EditCycleType.Pressure);
                }
                else if (ecParam.GetType().Name == "SerialPortParameter" || ecParam.GetType().Name == "EthernetPortParameter")
                    editorTreeNode.BackColor = ecf.EditCycleColor(EditCycleForm.EditCycleType.Control);
                else if (ecParam.GetType().Name == "BooleanParameter")
                    editorTreeNode.BackColor = ecf.EditCycleColor(EditCycleForm.EditCycleType.Mode);
                else if (ecParam.GetType().Name == "EnumParameter")
                    editorTreeNode.BackColor = ecf.EditCycleColor(EditCycleForm.EditCycleType.Control);
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteError(
                    string.Format("Error occurred inside CreateTreeNode while assigning parameter color"), VtiEventCatType.Parameter_Update,
                    ex.ToString());
            }
            return editorTreeNode;
        }

        /// <summary>
        /// Sets the process value.
        /// </summary>
        /// <param name="s">The string representation of the process value.</param>
        public virtual void SetProcessValue(string s)
        {
            T result;
            if (ExtensionMethods.TryParse<T>(s, out result))
                ProcessValue = result;
        }

        /// <summary>
        /// Updates the process value from new value.
        /// </summary>
        /// <returns>A message indicating that the process value was changed.</returns>
        public virtual string UpdateProcessValueFromNewValue()
        {
            string retVal = string.Format("{0} changed from {1} to {2}.",
                DisplayName, ProcessValue.ToString(), NewValue.ToString());
            ProcessValue = NewValue;
            return retVal;
        }

        #endregion Public Methods
        #region Protected Methods (1)

        /// <summary>
        /// Raises the <see cref="ProcessValueChanged">ProcessValueChanged</see> event
        /// </summary>
        protected virtual void OnProcessValueChanged()
        {
            if (ProcessValueChanged != null)
                ProcessValueChanged(this, null);
        }

        /// <summary>
        /// Occurs when new value is changed.
        /// </summary>
        public event EventHandler<EventArgs<T>> NewValueChanged;

        /// <summary>
        /// Called when the new value is changed.
        /// </summary>
        protected virtual void OnNewValueChanged()
        {
            if (NewValueChanged != null)
                NewValueChanged(this, new EventArgs<T>(NewValue));
        }

        #endregion Protected Methods

        #endregion Methods

        #region IEditCycleParameter Members

        /// <summary>
        /// When implemented in a derived class, gets the editor control to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>The editor control to be displayed in the Edit Cycle form.</returns>
        public abstract Control GetEditorControl(PropertyInfo propertyInfo);

        /// <summary>
        /// When implemented in a derived class, gets the editor control to be displayed in the Edit Cycle form for the specified child node.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>The editor control to be displayed in the Edit Cycle form for the specified child node.</returns>
        public abstract Control GetEditorControl(PropertyInfo propertyInfo, string childNodeName);

        #endregion
    }
}