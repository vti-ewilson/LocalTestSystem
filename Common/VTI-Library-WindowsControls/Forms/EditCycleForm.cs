using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.Configuration.Interfaces;
using VTIWindowsControlLibrary.Classes.Data;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Enums;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents the Edit Cycle form
    /// </summary>
    public partial class EditCycleForm : Form
    {
        #region Fields (3)

        #region Private Fields (3)

        private IEditCycleParameter currentParameter;
        public VtiDataContext db;
        public VTIWindowsControlLibrary.Data.VtiDataContext2.VtiDataContext2 db2;
        private List<ModelSettingsBase> modelsToDelete = new List<ModelSettingsBase>();
        public SequenceStepsControl.SequenceStepList[] sequenceStepList;
        public List<string> operatingSeqList = new List<string>();
        private TreeNode nodeMatch;
        public List<string> modelNameList = new List<string>();
        public bool bIsOpenEditCycleSearchForm = false;
        public bool refreshCOMports = false;
        public List<(string deviceAlreadyUsingCOMPort, string COMPortNumberInUse, string deviceThatWasAssignedCOMPort)> COMPortAlreadyInUseList = new List<(string deviceAlreadyUsingCOMPort, string COMPortNumberInUse, string deviceThatWasAssignedCOMPort)>();

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCycleForm">EditCycleForm</see> class
        /// </summary>
        public EditCycleForm()
        {
            InitializeComponent();
            this.KeyPreview = true;

            string strExportFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            strExportFolder = strExportFolder.Replace(@"file:\", "");
            strExportFolder = "Export to folder " + strExportFolder;
            toolTipExport.SetToolTip(buttonExport, strExportFolder);
            VtiEvent.Log.WriteInfo("Initializing Edit Cycle");
        }

        #endregion Constructors

        #region Methods (11)

        #region Private Methods (11)

        private void BuildNode(TreeNode node)
        {
            if (VtiLib.UseRemoteModelDB)
            {
                Data.VtiDataContext2.VtiDataContext2 db = new Data.VtiDataContext2.VtiDataContext2(VtiLib.ConnectionString3);
            }
            else
            {
                VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString);
            }

            //ModelSettingsBase objectSettings;
            object objectSettings;
            //TreeNode treeNode2, treeNode3;
            Model model;
            Data.VtiDataContext2.Model model2;
            ModelParameter modelParameter;
            Data.VtiDataContext2.ModelParameter modelParameter2;

            TreeNode treeNode = null;
            ModelSettingsBase modelSettingsToCopy;

            foreach (TreeNode node0 in treeView1.Nodes)
            {
                if (node0.Text == "Model DEFAULT Parameters")
                {
                    treeNode = node0;
                    break;
                }
            }
            modelSettingsToCopy = treeNode.Tag as ModelSettingsBase;

            this.Cursor = Cursors.WaitCursor;

            node.Nodes.Clear();

            if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
            {
                List<TreeNode> CommonParametersNodeList = new List<TreeNode>();
                //objectSettings = (ModelSettingsBase)node.Tag;
                objectSettings = node.Tag;
                try
                {
                    // Get the properties of the current settings object
                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                            if (editCycleParameter.Visible)
                            {
                                TreeNode newNode = null;
                                if (node.Text.StartsWith("Common "))
                                {
                                    newNode = editCycleParameter.CreateTreeNode(property.Name);
                                    newNode.Tag = property;
                                    if (Properties.Settings.Default.SortCommonEditCycleParams)
                                    {
                                        CommonParametersNodeList.Add(newNode);
                                    }
                                    else
                                    {
                                        node.Nodes.Add(newNode);
                                    }
                                }
                                else
                                {
                                    newNode = editCycleParameter.CreateTreeNode(property, editCycleParameter, this);
                                    newNode.Tag = property;
                                    node.Nodes.Add(newNode);
                                }
                            }
                            //if (node.Text.Equals("Model DEFAULT Parameters"))
                            //{
                            //    // Install event handler to propagate NewUnits changes from the default model to other models
                            //    if (editCycleParameter is TimeDelayParameter)
                            //    {
                            //        TimeDelayParameter timeDelayParameter = editCycleParameter as TimeDelayParameter;
                            //        timeDelayParameter.NewUnitsChanged += new EventHandler<EventArgs<CycleStepTimeDelayUnits>>(timeDelayParameter_Default_NewUnitsChanged);
                            //    }
                            //}
                        }
                    }
                    if (Properties.Settings.Default.SortCommonEditCycleParams)
                    {
                        CommonParametersNodeList = CommonParametersNodeList.OrderBy(x => x.Text).ToList();
                        foreach (TreeNode commonNode in CommonParametersNodeList)
                        {
                            node.Nodes.Add(commonNode);
                        }
                    }
                }
                //NJ 5-14-2020
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                    MessageBox.Show("Error Loading Edit Cycle parameters for selected section. " +
                        "Make sure the name of the parameter is consistent every time it is used in its declaration (in the get AND set). Exception: " + ee.ToString());
                }
            }
            else
            {

                // Create new blank model
                //objectSettings = Activator.CreateInstance(VtiLib.ConfigType.GetField("DefaultModel").FieldType);
                objectSettings = Activator.CreateInstance(VtiLib.ModelSettingsType) as ModelSettingsBase;

                // Get the name we saved earlier
                //objectSettings.Name = node.Tag.ToString();
                string modelName = node.Tag.ToString();
                if (modelName.Contains("Classes.Configuration.ModelSettings"))
                {
                    var match = Regex.Match(node.Text, @"Model ([\w\s]+) Parameters");
                    modelName = match.Groups[1].Value;
                }

                ((ModelSettingsBase)objectSettings).Name = modelName;

                // Save the model object to the treenode tag
                node.Tag = objectSettings;

                if (VtiLib.UseRemoteModelDB)
                {
                    // Get the model from the database
                    //model = db.Models.Single(m => m.ModelNo == objectSettings.Name);
                    model2 = db2.Models.Single(m => m.ModelNo == ((ModelSettingsBase)objectSettings).Name);

                    // Get the properties of the current settings object
                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                            IEditCycleParameter editCycleParameterToCopy = property.GetValue(modelSettingsToCopy, null) as IEditCycleParameter;
                            if (editCycleParameterToCopy.Visible)
                            {
                                //TreeNode newNode = editCycleParameter.CreateTreeNode(property.Name);
                                TreeNode newNode = editCycleParameter.CreateTreeNode(property, editCycleParameter, this);
                                newNode.Tag = property;
                                node.Nodes.Add(newNode);

                                //// Install event handler to propagate NewUnits changes from the models back to the default model
                                //if (editCycleParameter is TimeDelayParameter)
                                //{
                                //    TimeDelayParameter timeDelayParameter = editCycleParameter as TimeDelayParameter;
                                //    timeDelayParameter.NewUnitsChanged += new EventHandler<EventArgs<CycleStepTimeDelayUnits>>(timeDelayParameter_Model_NewUnitsChanged);
                                //}

                                modelParameter2 = model2.ModelParameters.FirstOrDefault(p => p.ParameterID == property.Name && p.SystemType == VtiLib.ModelDBSystemType);
                                // Parameters exist in database
                                if (modelParameter2 != null)
                                {
                                    editCycleParameter.NewValueString =
                                        editCycleParameter.ProcessValueString =
                                        modelParameter2.ProcessValue;
                                    editCycleParameter.Updated = false;
                                }

                                // Parameter was added which isn't in the database.  Get the default value!
                                else
                                {
                                    object defaultSettings = Activator.CreateInstance(VtiLib.ModelSettingsType);
                                    IEditCycleParameter defaultParameter = property.GetValue(defaultSettings, null) as IEditCycleParameter;
                                    editCycleParameter.NewValueString = defaultParameter.ProcessValueString;
                                    editCycleParameter.Updated = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Get the model from the database
                    //model = db.Models.Single(m => m.ModelNo == objectSettings.Name);
                    model = db.Models.Single(m => m.ModelNo == ((ModelSettingsBase)objectSettings).Name);

                    // Get the properties of the current settings object
                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                            IEditCycleParameter editCycleParameterToCopy = property.GetValue(modelSettingsToCopy, null) as IEditCycleParameter;
                            if (editCycleParameterToCopy.Visible)
                            {
                                //TreeNode newNode = editCycleParameter.CreateTreeNode(property.Name);
                                TreeNode newNode = editCycleParameter.CreateTreeNode(property, editCycleParameter, this);
                                newNode.Tag = property;
                                node.Nodes.Add(newNode);

                                //// Install event handler to propagate NewUnits changes from the models back to the default model
                                //if (editCycleParameter is TimeDelayParameter)
                                //{
                                //    TimeDelayParameter timeDelayParameter = editCycleParameter as TimeDelayParameter;
                                //    timeDelayParameter.NewUnitsChanged += new EventHandler<EventArgs<CycleStepTimeDelayUnits>>(timeDelayParameter_Model_NewUnitsChanged);
                                //}

                                modelParameter = model.ModelParameters.FirstOrDefault(p => p.ParameterID == property.Name);
                                // Parameters exist in database
                                if (modelParameter != null)
                                {
                                    editCycleParameter.NewValueString =
                                        editCycleParameter.ProcessValueString =
                                        modelParameter.ProcessValue;
                                    editCycleParameter.Updated = false;
                                }

                                // Parameter was added which isn't in the database.  Get the default value!
                                else
                                {
                                    object defaultSettings = Activator.CreateInstance(VtiLib.ModelSettingsType);
                                    IEditCycleParameter defaultParameter = property.GetValue(defaultSettings, null) as IEditCycleParameter;
                                    editCycleParameter.NewValueString = defaultParameter.ProcessValueString;
                                    editCycleParameter.Updated = true;
                                }
                            }
                        }
                    }
                }
            }

            this.Cursor = Cursors.Default;
        }

        public enum EditCycleType
        {
            Control = 0,
            Flow,
            Mode,
            Pressure,
            Time
        }

        public Color EditCycleColor(EditCycleType type)
        {
            int ndx = 190;
            Color clr = Color.White;
            switch (type)
            {
                case EditCycleType.Control:
                    clr = System.Drawing.Color.FromArgb(255, ndx, 255);
                    break;

                case EditCycleType.Flow:
                    clr = System.Drawing.Color.FromArgb(255, 255, ndx);
                    break;

                case EditCycleType.Mode:
                    clr = System.Drawing.Color.FromArgb(ndx, ndx, 255);
                    break;

                case EditCycleType.Pressure:
                    clr = System.Drawing.Color.FromArgb(255, ndx, ndx);
                    break;

                case EditCycleType.Time:
                    clr = System.Drawing.Color.FromArgb(ndx, 255, 255);
                    break;

                default:
                    clr = System.Drawing.Color.FromArgb(255, 255, 255);
                    break;
            }
            return clr;
        }

        //void timeDelayParameter_Default_NewUnitsChanged(object sender, EventArgs<CycleStepTimeDelayUnits> e)
        //{
        //    // The time delay parameter for the default model is the sender
        //    TimeDelayParameter timeDelayParameter = sender as TimeDelayParameter;

        //    // Iterate through all loaded models, setting the NewUnits for the corresponding time delay parameters
        //    foreach (TreeNode node in treeView1.Nodes)
        //    {
        //        if (!(node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters")) &&
        //            node.Nodes.Count > 0 && node.Nodes[0].Text != "Place Holder")
        //        {
        //            // Get the corresponding parameter node from the current model node
        //            TreeNode modelParameterNode = node.Nodes[treeView1.SelectedNode.Name];
        //            // Get the model settings object from the current model node tag
        //            object modelSettings = node.Tag;
        //            // Get the model property from the model parameter node tag
        //            PropertyInfo modelProperty = modelParameterNode.Tag as PropertyInfo;
        //            // Get the model parameter
        //            TimeDelayParameter modelParameter = modelProperty.GetValue(modelSettings, null) as TimeDelayParameter;
        //            // Set the NewUnits for the model parameter
        //            modelParameter.NewUnits = e.Value;
        //        }
        //    }
        //}

        //void timeDelayParameter_Model_NewUnitsChanged(object sender, EventArgs<CycleStepTimeDelayUnits> e)
        //{
        //    // The current model-specific time delay parameter is the sender
        //    TimeDelayParameter timeDelayParameter = sender as TimeDelayParameter;

        //    // Check to see if the Default Model node has been built
        //    TreeNode defaultModelNode = treeView1.Nodes["DefaultModel"];
        //    if ((defaultModelNode.Nodes.Count > 0) && (defaultModelNode.Nodes[0].Text == "Place Holder"))
        //    {
        //        BuildNode(defaultModelNode);
        //    }
        //    // Get the corresponding parameter node from the default model node
        //    TreeNode defaultParameterNode = defaultModelNode.Nodes[treeView1.SelectedNode.Name];
        //    // Get the default model object from the default model node tag
        //    object defaultSettings = defaultModelNode.Tag;
        //    // Get the default property from the default parameter node tag
        //    PropertyInfo defaultProperty = defaultParameterNode.Tag as PropertyInfo;
        //    // Get the default parameter
        //    TimeDelayParameter defaultParameter = defaultProperty.GetValue(defaultSettings, null) as TimeDelayParameter;
        //    // Set the NewUnits for the default parameter
        //    defaultParameter.NewUnits = e.Value;
        //}

        private void EditCycleForm_VisibleChanged(object sender, EventArgs e)
        {

            if (this.Visible)
            {
                TreeNode treeNode1;

                if (VtiLib.UseRemoteModelDB)
                {
                    db2 = new VTIWindowsControlLibrary.Data.VtiDataContext2.VtiDataContext2(VtiLib.ConnectionString3);
                }
                else
                {
                    db = new VtiDataContext(VtiLib.ConnectionString);
                }


                this.treeView1.Nodes.Clear();
                this.numberPadControl.Visible = false;
                this.Refresh();
                this.Cursor = Cursors.WaitCursor;
                this.treeView1.SuspendLayout();

                modelsToDelete.Clear();

                // Get the members of the Config type
                PropertyInfo[] properties = VtiLib.Config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
                foreach (PropertyInfo property in properties)
                {
                    if ((property.PropertyType.IsSubclassOf(typeof(ApplicationSettingsBase))) && (property.Name != "CurrentModel") && (property.Name != "IO"))
                    {
                        object objectSettings = property.GetValue(null, null);

                        if (objectSettings != null)
                        {
                            // Create the tree node for the current settings object
                            treeNode1 = new TreeNode();
                            treeNode1.Name = property.Name;

                            if (property.Name == "DefaultModel")
                                treeNode1.Text = "Model DEFAULT Parameters";
                            else
                                treeNode1.Text = "Common " + property.Name + " Parameters";

                            treeNode1.Tag = objectSettings;  // save objectSettings in the tag of the TreeNode
                            treeView1.Nodes.Add(treeNode1);

                            treeNode1.Nodes.Add("Place Holder");
                            try
                            {
                                // assign a background color to each node
                                if (treeNode1.Text.StartsWith("Common Control"))
                                    treeNode1.BackColor = EditCycleColor(EditCycleType.Control);
                                else if (treeNode1.Text.StartsWith("Common Time"))
                                    treeNode1.BackColor = EditCycleColor(EditCycleType.Time); //LightCyan
                                else if (treeNode1.Text.StartsWith("Common Flow"))
                                    treeNode1.BackColor = EditCycleColor(EditCycleType.Flow);
                                else if (treeNode1.Text.StartsWith("Common Pressure"))
                                    treeNode1.BackColor = EditCycleColor(EditCycleType.Pressure);
                                else if (treeNode1.Text.StartsWith("Common Mode"))
                                    treeNode1.BackColor = EditCycleColor(EditCycleType.Mode);
                            }
                            catch (Exception ex)
                            {
                                VtiEvent.Log.WriteError(
                                    string.Format("Error occurred inside EditCycleForm_VisibleChanged while assigned node color"), VtiEventCatType.Parameter_Update,
                                    ex.ToString());
                            }
                        }
                    }
                }

                if (Properties.Settings.Default.UsesVtiDataDatabase)
                {
                    if (VtiLib.UseRemoteModelDB)
                    {
                        foreach (Data.VtiDataContext2.Model model in db2.Models)
                        {
                            // Create the tree node for the current settings object
                            treeNode1 = new TreeNode();
                            treeNode1.Name = "Model " + model.ModelNo;
                            treeNode1.Text = "Model " + model.ModelNo + " Parameters";
                            treeNode1.Tag = model.ModelNo; // hang onto the name for now
                            treeView1.Nodes.Add(treeNode1);

                            treeNode1.Nodes.Add("Place Holder");
                        }
                    }
                    else
                    {
                        // Load Model Parameters (just the top level treenodes for now)
                        foreach (Model model in db.Models)
                        {
                            // Create the tree node for the current settings object
                            treeNode1 = new TreeNode();
                            treeNode1.Name = "Model " + model.ModelNo;
                            treeNode1.Text = "Model " + model.ModelNo + " Parameters";
                            treeNode1.Tag = model.ModelNo; // hang onto the name for now
                            treeView1.Nodes.Add(treeNode1);

                            treeNode1.Nodes.Add("Place Holder");
                        }
                    }

                }
                this.treeView1.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
            else
            {
                Control control = tableLayoutPanel2.GetControlFromPosition(0, 1);
                if (control != null) tableLayoutPanel2.Controls.Remove(control);
                treeView1.SelectedNode = null;
                VtiLib.overrideUser = null;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            int maxParameterChangesToDisplay = 10;
            List<(string parameterName, string oldValue, string newValue)> unsavedParameterChanges = new List<(string parameterName, string oldValue, string newValue)>();
            // Enumerate through the tree nodes
            foreach (TreeNode node in treeView1.Nodes)
            {
                // Node is Top-Level and has child nodes
                if ((node.Level == 0) && (node.Nodes.Count > 0) && (node.Nodes[0].Text != "Place Holder"))
                {
                    object objectSettings = node.Tag;
                    if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                    {
                        foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                        {
                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                            {
                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;

                                if (editCycleParameter.Updated)
                                {
                                    string unsavedParamName = editCycleParameter.DisplayName;
                                    string unsavedOldValue = editCycleParameter.ProcessValueString;
                                    string unsavedNewValue = editCycleParameter.NewValueString;

                                    if (property.PropertyType.Name == "SerialPortParameter")
                                    {
                                        SerialPortParameter tempPortToGetParams = editCycleParameter as SerialPortParameter;
                                        unsavedOldValue = tempPortToGetParams.ProcessValue.PortName + "," + tempPortToGetParams.ProcessValue.BaudRate + "," + tempPortToGetParams.ProcessValue.Parity + "," + tempPortToGetParams.ProcessValue.DataBits + "," + tempPortToGetParams.ProcessValue.StopBits + "," + tempPortToGetParams.ProcessValue.Handshake;
                                        unsavedNewValue = tempPortToGetParams.NewValue.PortName + "," + tempPortToGetParams.NewValue.BaudRate + "," + tempPortToGetParams.NewValue.Parity + "," + tempPortToGetParams.NewValue.DataBits + "," + tempPortToGetParams.NewValue.StopBits + "," + tempPortToGetParams.NewValue.Handshake;
                                    }
                                    else if (property.PropertyType.Name == "EthernetPortParameter")
                                    {
                                        EthernetPortParameter tempPortToGetParams = editCycleParameter as EthernetPortParameter;
                                        unsavedOldValue = tempPortToGetParams.ProcessValue.PortName + "-" + tempPortToGetParams.ProcessValue.IPAddress + ":" + tempPortToGetParams.ProcessValue.Port;
                                        unsavedNewValue = tempPortToGetParams.NewValue.PortName + "-" + tempPortToGetParams.NewValue.IPAddress + ":" + tempPortToGetParams.NewValue.Port;
                                    }

                                    unsavedParameterChanges.Add((node.Text + " - " + unsavedParamName, unsavedOldValue, unsavedNewValue));
                                    if (unsavedParameterChanges.Count > maxParameterChangesToDisplay)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    // Model settings in the database
                    else
                    {
                        string newModelName = "";
                        //prevent adding a new model which is also set to be deleted on the OK click
                        if (!modelsToDelete.Any(x => x.Name.Insert(0, "!") == ((ModelSettingsBase)objectSettings).Name))
                        {
                            // Add/Update Model Parameters
                            if (((ModelSettingsBase)objectSettings).Name.Substring(0, 1) == "!")
                            {
                                newModelName = ((ModelSettingsBase)objectSettings).Name.Substring(1);
                                unsavedParameterChanges.Add(("New Model Created", "", newModelName));
                                if (unsavedParameterChanges.Count > maxParameterChangesToDisplay)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                                {
                                    if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                    {
                                        IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                        if (editCycleParameter.Updated)
                                        {
                                            if (editCycleParameter.ProcessValueString == editCycleParameter.NewValueString)
                                            {
                                                // happens if a custom model exists in VtiData dbo.Models and then you create a new parameter in the project's ModelSettings.cs and then re-run the app and expand the custom model's parameter section
                                                unsavedParameterChanges.Add(($"Model {(((ModelSettingsBase)objectSettings).Name)} - {editCycleParameter.DisplayName}", "(PARAMETER DOES NOT EXIST)", editCycleParameter.NewValueString));
                                            }
                                            else
                                            {
                                                unsavedParameterChanges.Add(($"Model {(((ModelSettingsBase)objectSettings).Name)} - {editCycleParameter.DisplayName}", editCycleParameter.ProcessValueString, editCycleParameter.NewValueString));
                                            }
                                            if (unsavedParameterChanges.Count > maxParameterChangesToDisplay)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string unsavedChangesWarning = "You have unsaved changes. Are you sure you want to cancel?";
            foreach (var unsavedParamChange in unsavedParameterChanges)
            {
                unsavedChangesWarning += $"{Environment.NewLine}{Environment.NewLine}{unsavedParamChange.parameterName}{Environment.NewLine}Old value: {unsavedParamChange.oldValue}{Environment.NewLine}New value: {unsavedParamChange.newValue}";
            }

            if (unsavedParameterChanges.Count == 0 || MessageBox.Show(unsavedChangesWarning, "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                VtiEvent.Log.WriteWarning("Edit Cycle changes canceled by the user.",
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Parameter_Update);
                this.Hide();
            }
        }

        /// <summary>
        /// Cycle through all of the members of Config
        /// If the value has changed, copy the new value to the process value
        /// Save the Config to the user.config file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            SaveConfig(true);
            buttonOK.Enabled = true;
        }

        /// <summary>
        /// Log the parameter change to the local or remote dbo.ParamChangeLog table
        /// </summary>
        public void LogParameterChanges(List<Classes.Data.ChangedParameter> changedParameters)
        {
            string overrideOpID = "";
            string sqlCmd = "";
            string connString = "";
            if (VtiLib.overrideUser != null)
            {
                overrideOpID = VtiLib.overrideUser.OpID;
            }
            foreach (Classes.Data.ChangedParameter cp in changedParameters)
            {
                try
                {
                    //Insert changed parameters into local dbo.ParamChangeLog
                    connString = VtiLib.ConnectionString;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "Insert into dbo.ParamChangeLog " +
                        "(DateTime, OpID, OverrideOpID, SystemID, ParameterSectionName, ParameterName, OldValue, NewValue) " +
                        "values (@DateTime, @OpID, @OverrideOpID, @SystemID, @ParameterSectionName, @ParameterName, @OldValue, @NewValue);"
                    })
                    {
                        // using parameterized query removes the need to manually replace ' or any other part that would break your query
                        cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = DateTime.Now;
                        //  string.format calls the "toString()" method on all the things passed to it, which will format according to culture settings, which will break SQL.
                        // Using parameterized query for DateTime type will always format the date correctly for the database
                        cmd.Parameters.Add("@OpID", SqlDbType.NVarChar).Value = cp.OpID;
                        cmd.Parameters.Add("@OverrideOpID", SqlDbType.NVarChar).Value = overrideOpID;
                        cmd.Parameters.Add("@SystemID", SqlDbType.NVarChar).Value = cp.SystemID;
                        cmd.Parameters.Add("@ParameterSectionName", SqlDbType.NVarChar).Value = cp.paramSectionName;
                        cmd.Parameters.Add("@ParameterName", SqlDbType.NVarChar).Value = cp.paramName;
                        cmd.Parameters.Add("@OldValue", SqlDbType.NVarChar).Value = cp.oldValue;
                        cmd.Parameters.Add("@NewValue", SqlDbType.NVarChar).Value = cp.newValue;

                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error inserting parameter change record into local dbo.ParamChangeLog. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                    MessageBox.Show("Error inserting parameter change record into local dbo.ParamChangeLog. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                }
            }
            if (VtiLib.ConnectionString2 != "")
            {
                foreach (Classes.Data.ChangedParameter cp in changedParameters)
                {
                    sqlCmd = "";
                    connString = "";
                    try
                    {
                        //Insert changed parameters into local dbo.ParamChangeLog
                        connString = VtiLib.ConnectionString2;
                        using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                        using (SqlCommand cmd = new SqlCommand
                        {
                            Connection = sqlConnection1,
                            CommandType = CommandType.Text,
                            CommandText = "Insert into dbo.ParamChangeLog " +
                            "(DateTime, OpID, OverrideOpID, SystemID, ParameterSectionName, ParameterName, OldValue, NewValue) " +
                            "values (@DateTime, @OpID, @OverrideOpID, @SystemID, @ParameterSectionName, @ParameterName, @OldValue, @NewValue);"
                        })
                        {
                            // using parameterized query removes the need to manually replace ' or any other part that would break your query
                            cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = DateTime.Now;
                            //  string.format calls the "toString()" method on all the things passed to it, which will format according to culture settings, which will break SQL.
                            // Using parameterized query for DateTime type will always format the date correctly for the database
                            cmd.Parameters.Add("@OpID", SqlDbType.NVarChar).Value = cp.OpID;
                            cmd.Parameters.Add("@OverrideOpID", SqlDbType.NVarChar).Value = overrideOpID;
                            cmd.Parameters.Add("@SystemID", SqlDbType.NVarChar).Value = cp.SystemID;
                            cmd.Parameters.Add("@ParameterSectionName", SqlDbType.NVarChar).Value = cp.paramSectionName;
                            cmd.Parameters.Add("@ParameterName", SqlDbType.NVarChar).Value = cp.paramName;
                            cmd.Parameters.Add("@OldValue", SqlDbType.NVarChar).Value = cp.oldValue;
                            cmd.Parameters.Add("@NewValue", SqlDbType.NVarChar).Value = cp.newValue;

                            sqlCmd = cmd.CommandText;
                            sqlConnection1.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ee)
                    {
                        VtiEvent.Log.WriteError("Error inserting parameter change record into remote dbo.ParamChangeLog. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                        MessageBox.Show("Error inserting parameter change record into remote dbo.ParamChangeLog. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                    }
                }
            }
        }

        public void AddModelToRemoteDatabase(ModelParameterToUpdate model)
        {
            if (VtiLib.ConnectionString2 != "")
            {
                string sqlCmd = "";
                string connString = "";
                try
                {
                    //Insert new model into remote dbo.Models table
                    connString = VtiLib.ConnectionString2;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "Insert into dbo.Models " +
                        "(ModelNo, LastModifiedBy, LastModified, SystemType) " +
                        "values(@ModelNo, @LastModifiedBy, @LastModified, @SystemType);",
                    })
                    {
                        sqlCmd = cmd.CommandText;
                        // using parameterized query removes the need to manually replace ' or any other part that would break your query
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;
                        cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = model.LastModifiedBy;
                        cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = model.LastModified ?? DateTime.Now;
                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = model.SystemType;

                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error inserting new model into remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error inserting new model into remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                }
            }
            else
            {
                VtiEvent.Log.WriteError("Unable to insert new model into remote database table dbo.Models. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                MessageBox.Show("Unable to insert new model into remote database table dbo.Models. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
            }
        }

        public void UpdateModelToRemoteDatabase(ModelParameterToUpdate model)
        {
            if (VtiLib.ConnectionString2 != "")
            {
                string sqlCmd = "";
                string connString = "";
                try
                {
                    //Insert new model into remote dbo.Models table
                    connString = VtiLib.ConnectionString2;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "UPDATE dbo.Models SET " +
                        "LastModifiedBy = @op, " +
                        "LastModified = @time " +
                        "WHERE ModelNo = @model AND SystemType = @type " +
                        " IF @@ROWCOUNT=0 INSERT INTO dbo.Models " +
                        " (ModelNo, LastModifiedBy, LastModified, SystemType) " +
                        " values(@ModelNo, @LastModifiedBy, @LastModified, @SystemType);",
                    })
                    {
                        sqlCmd = cmd.CommandText;
                        // using parameterized query removes the need to manually replace ' or any other part that would break your query
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;
                        cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = model.LastModifiedBy;
                        cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = model.LastModified ?? DateTime.Now;
                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = model.SystemType;

                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error updating model on remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error updating model on remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                }
            }
            else
            {
                VtiEvent.Log.WriteError("Unable to update model on remote database table dbo.Models. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                MessageBox.Show("Unable to update model on remote database table dbo.Models. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
            }
        }

        public void AddOrUpdateModelParametersToRemoteDatabase(List<Classes.Data.ModelParameterToUpdate> listOfModelParametersToUpdate)
        {
            // Insert new/updated model parameters into remote dbo.ModelParameters database tables
            if (VtiLib.ConnectionString2 != "")
            {
                string sqlCmd = "";
                string connString = "";
                foreach (Classes.Data.ModelParameterToUpdate mp in listOfModelParametersToUpdate)
                {
                    try
                    {
                        connString = VtiLib.ConnectionString2;
                        using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                        using (SqlCommand cmd = new SqlCommand
                        {
                            Connection = sqlConnection1,
                            CommandType = CommandType.Text,
                            CommandText = "UPDATE dbo.ModelParameters SET " +
                                "ProcessValue = @ProcessValue, " +
                                "LastModifiedBy = @LastModifiedBy, " +
                                "LastModified = @LastModified " +
                                "WHERE ModelNo = @ModelNo AND ParameterID = @ParameterID AND SystemType = @SystemType " +
                                " IF @@ROWCOUNT=0 INSERT INTO dbo.ModelParameters (ModelNo, ParameterID, ProcessValue, LastModifiedBy, LastModified, SystemType) " +
                                " VALUES (@ModelNo, @ParameterID, @ProcessValue, @LastModifiedBy, @LastModified, @SystemType);",
                        })
                        {
                            sqlCmd = cmd.CommandText;
                            // using parameterized query removes the need to manually replace ' or any other part that would break your query
                            cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = mp.ModelNo;
                            cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = mp.LastModifiedBy;
                            cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = mp.LastModified ?? DateTime.Now;
                            cmd.Parameters.Add("@ParameterID", SqlDbType.NVarChar).Value = mp.ParameterID;
                            cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = mp.SystemType;

                            sqlConnection1.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ee)
                    {
                        VtiEvent.Log.WriteError("Error inserting or updating model parameter into remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                        MessageBox.Show("Error inserting or updating model parameter into remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    }
                }
            }
            else
            {
                VtiEvent.Log.WriteError("Unable to insert or update model parameter into remote database table dbo.ModelParameters. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                MessageBox.Show("Unable to insert or update model parameter into remote database table dbo.ModelParameters. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
            }
        }

        public void DeleteModelFromRemoteDatabase(ModelParameterToUpdate model)
        {
            if (VtiLib.ConnectionString2 != "")
            {
                string sqlCmd = "";
                string connString = "";
                //Delete model from remote dbo.Models table
                try
                {
                    connString = VtiLib.ConnectionString2;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = $"DELETE FROM dbo.Models WHERE ModelNo = @ModelNo AND SystemType = @SystemType;"
                    })
                    {
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;
                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = model.SystemType;
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error deleting model from remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                    MessageBox.Show("Error deleting model from remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                }

                //Delete model parameters from remote dbo.ModelParameters table
                try
                {
                    connString = VtiLib.ConnectionString2;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = $"DELETE FROM dbo.ModelParameters WHERE ModelNo = @ModelNo AND SystemType = @SystemType;"
                    })
                    {
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;
                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = model.SystemType;
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error deleting model parameter from remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error deleting model parameter from remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                }
            }
            else
            {
                VtiEvent.Log.WriteError("Unable to delete model from remote database table dbo.Models. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                MessageBox.Show("Unable to delete model from remote database table dbo.Models. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
            }
        }

        /// <summary>
        /// Save the Configuration settings to disk
        /// </summary>
        public void SaveConfig(bool bHideEditCycleForm)
        {
            VtiLib.MuteParamChangeLog = true;
            refreshCOMports = false;
            COMPortAlreadyInUseList = new List<(string deviceAlreadyUsingCOMPort, string COMPortNumberInUse, string deviceThatWasAssignedCOMPort)>();
            //String modelName;
            ArrayList parameterUpdates = new ArrayList();   // list of parameter update strings for event log entry
            Model model;
            Data.VtiDataContext2.Model model2;

            if (VtiLib.UseRemoteModelDB)
            {
                model2 = new Data.VtiDataContext2.Model();
            }
            else
            {
                model = new Model();
            }

            string opId = VtiLib.Config._OpID;
            string systemID = "";
            string systemTypeForModelSync = "";
            bool syncModelsWithRemoteDatabase = false;
            List<Classes.Data.ChangedParameter> listOfChangedParameters = new List<Classes.Data.ChangedParameter>();
            List<Classes.Data.ModelParameterToUpdate> listOfModelParametersToUpdate = new List<Classes.Data.ModelParameterToUpdate>();
            this.Cursor = Cursors.WaitCursor;

            // wrap the database updates in a transaction
            using (TransactionScope ts = new TransactionScope())
            {
                // Enumerate through the tree nodes
                foreach (TreeNode node in treeView1.Nodes)
                {
                    //keep these if-statements for "System ID", "System Type For Models Sync", and "Sync Models With Remote Database" located here. Otherwise these will not get executed if only a model parameter gets changed
                    if (node.Text.StartsWith("Common Control"))
                    {
                        object objectSettings = node.Tag;
                        foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                        {
                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                            {
                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                if (editCycleParameter.DisplayName.Equals("System ID", StringComparison.CurrentCultureIgnoreCase)
                                        || editCycleParameter.DisplayName.Equals("System_ID", StringComparison.CurrentCultureIgnoreCase)
                                        || editCycleParameter.DisplayName.Equals("SystemID", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    systemID = editCycleParameter.ProcessValueString;
                                }
                                else if (editCycleParameter.DisplayName.Equals("System Type For Models Sync", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    systemTypeForModelSync = editCycleParameter.ProcessValueString;
                                }
                            }
                        }
                    }
                    else if (node.Text.StartsWith("Common Mode"))
                    {
                        object objectSettings = node.Tag;
                        foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                        {
                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                            {
                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                if (editCycleParameter.DisplayName.Equals("Auto Update Models From Remote Database", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    bool.TryParse(editCycleParameter.ProcessValueString, out syncModelsWithRemoteDatabase);
                                }
                            }
                        }
                    }
                    // Node is Top-Level and has child nodes
                    if ((node.Level == 0) && (node.Nodes.Count > 0) && (node.Nodes[0].Text != "Place Holder"))
                    {
                        //ModelSettingsBase objectSettings = (ModelSettingsBase)node.Tag;
                        object objectSettings = node.Tag;

                        parameterUpdates.Clear();

                        // Settings in the Edit Cycle .config file
                        if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                        {
                            foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                            {
                                if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                {
                                    IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                    #region old
                                    //// Check for Time Delay Parameters in the default model whose units have changed
                                    //if (node.Text.Equals("Model DEFAULT Parameters") &&
                                    //    editCycleParameter is TimeDelayParameter &&
                                    //    (editCycleParameter as TimeDelayParameter).NewUnits !=
                                    //    (editCycleParameter as TimeDelayParameter).Units)
                                    //{
                                    //    TimeDelayParameter timeDelayParameter = editCycleParameter as TimeDelayParameter;
                                    //    // Iterate through all unloaded models, updating the process values in the database for the new units
                                    //    foreach (TreeNode subnode in treeView1.Nodes)
                                    //    {
                                    //        if (!(subnode.Text.StartsWith("Common ") || subnode.Text.Equals("Model DEFAULT Parameters")) &&
                                    //            subnode.Nodes.Count > 0 && subnode.Nodes[0].Text == "Place Holder")
                                    //        {
                                    //            string modelName = subnode.Tag.ToString();
                                    //            model = db.Models.Single(m => m.ModelNo == modelName);
                                    //            model.LastModifiedBy = opId;
                                    //            model.LastModified = DateTime.Now;
                                    //            if (model.ModelParameters.Any(p => p.ParameterID == property.Name))
                                    //            {
                                    //                ModelParameter param =
                                    //                    model.ModelParameters.Single(p => p.ParameterID == property.Name);
                                    //                param.ProcessValue = (double.Parse(param.ProcessValue) * (double)timeDelayParameter.Units / (double)timeDelayParameter.NewUnits).ToString();
                                    //                param.LastModifiedBy = opId;
                                    //                param.LastModified = DateTime.Now;

                                    //                parameterUpdates.Add(String.Format("{0} changed from {1} to {2}",
                                    //                    property.Name, editCycleParameter.ProcessValueString, editCycleParameter.NewValueString));
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    #endregion old

                                    if (editCycleParameter.Updated)
                                    {
                                        Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                                        changedParameter.OpID = opId;
                                        changedParameter.SystemID = systemID;
                                        changedParameter.paramSectionName = node.Text;
                                        changedParameter.paramName = editCycleParameter.DisplayName;
                                        if (property.PropertyType.Name == "SerialPortParameter")
                                        {
                                            //COM port changed by user. Close all COM ports and re-open with new ProcessValues.
                                            refreshCOMports = true;

                                            SerialPortParameter tempPortToGetParams = editCycleParameter as SerialPortParameter;
                                            changedParameter.oldValue = tempPortToGetParams.ProcessValue.PortName + "," + tempPortToGetParams.ProcessValue.BaudRate + "," + tempPortToGetParams.ProcessValue.Parity + "," + tempPortToGetParams.ProcessValue.DataBits + "," + tempPortToGetParams.ProcessValue.StopBits + "," + tempPortToGetParams.ProcessValue.Handshake;
                                            changedParameter.newValue = tempPortToGetParams.NewValue.PortName + "," + tempPortToGetParams.NewValue.BaudRate + "," + tempPortToGetParams.NewValue.Parity + "," + tempPortToGetParams.NewValue.DataBits + "," + tempPortToGetParams.NewValue.StopBits + "," + tempPortToGetParams.NewValue.Handshake;

                                            // check if new COM port selected is already being used by an existing device (User changes leak detector com port to COM4, but barcode scanner COM port is already set to COM4)
                                            foreach (PropertyInfo serialProperty in objectSettings.GetType().GetProperties())
                                            {
                                                if (serialProperty.PropertyType.GetInterface("IEditCycleParameter") != null)
                                                {
                                                    IEditCycleParameter serialEditCycleParameter = serialProperty.GetValue(objectSettings, null) as IEditCycleParameter;
                                                    if (serialProperty.PropertyType.Name == "SerialPortParameter")
                                                    {
                                                        SerialPortParameter tempSerialPortToGetParams = serialEditCycleParameter as SerialPortParameter;
                                                        if (tempSerialPortToGetParams.ProcessValue.PortName == tempPortToGetParams.NewValue.PortName && !tempSerialPortToGetParams.Updated && tempSerialPortToGetParams.DisplayName != tempPortToGetParams.DisplayName)
                                                        {
                                                            COMPortAlreadyInUseList.Add((tempSerialPortToGetParams.DisplayName, tempSerialPortToGetParams.ProcessValue.PortName, tempPortToGetParams.DisplayName));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (property.PropertyType.Name == "EthernetPortParameter")
                                        {
                                            EthernetPortParameter tempPortToGetParams = editCycleParameter as EthernetPortParameter;
                                            changedParameter.oldValue = tempPortToGetParams.ProcessValue.PortName + "-" + tempPortToGetParams.ProcessValue.IPAddress + ":" + tempPortToGetParams.ProcessValue.Port;
                                            changedParameter.newValue = tempPortToGetParams.NewValue.PortName + "-" + tempPortToGetParams.NewValue.IPAddress + ":" + tempPortToGetParams.NewValue.Port;
                                        }
                                        else
                                        {
                                            changedParameter.oldValue = editCycleParameter.ProcessValueString;
                                            changedParameter.newValue = editCycleParameter.NewValueString;
                                        }
                                        if (!COMPortAlreadyInUseList.Select(x => x.deviceThatWasAssignedCOMPort).Contains(changedParameter.paramName))
                                        {
                                            listOfChangedParameters.Add(changedParameter);
                                            parameterUpdates.Add(
                                                editCycleParameter.UpdateProcessValueFromNewValue());
                                        }
                                    }
                                }
                            }

                            VtiEvent.Log.WriteWarning(
                                String.Format("'{0}' updated by user '{1}'",
                                    node.Text, opId),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Parameter_Update,
                                parameterUpdates.ToArray());
                        }
                        // Model settings in the database
                        else
                        {
                            string modelName = "";
                            bool newModel = false;
                            //sb = new StringBuilder();
                            parameterUpdates.Clear();
                            //prevent adding a new model which is also set to be deleted on the OK click
                            if (!modelsToDelete.Any(x => x.Name.Insert(0, "!") == ((ModelSettingsBase)objectSettings).Name))
                            {
                                #region needs cleanup
                                if (VtiLib.UseRemoteModelDB)
                                {
                                    // Add new model
                                    if (((ModelSettingsBase)objectSettings).Name.Substring(0, 1) == "!")
                                    {
                                        modelName = ((ModelSettingsBase)objectSettings).Name.Substring(1);
                                        newModel = true;
                                        model2 = new Data.VtiDataContext2.Model
                                        {
                                            ModelNo = modelName,
                                            LastModifiedBy = opId,
                                            LastModified = DateTime.Now
                                        };

                                        Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                                        changedParameter.OpID = opId;
                                        changedParameter.SystemID = systemID;
                                        changedParameter.paramSectionName = VtiLibLocalization.NewModelAdded;
                                        changedParameter.paramName = VtiLibLocalization.NewModelAdded;
                                        changedParameter.oldValue = "";
                                        changedParameter.newValue = modelName;
                                        listOfChangedParameters.Add(changedParameter);

                                        VtiEvent.Log.WriteWarning(
                                            String.Format("Model '{0}' added by user '{1}'",
                                                modelName, opId),
                                            VtiEventCatType.Parameter_Update,
                                            parameterUpdates.ToArray());

                                        //VtiEvent.Log.WriteWarning(
                                        //    String.Format("Model '{0}' added by user '{1}'.",
                                        //        modelName, VtiLib.Config.GetField("OpID").GetValue(null)),
                                        //    VTIWindowsControlLibrary.Enum.VtiEventCatType.Parameter_Update);
                                    }
                                    else
                                    {
                                        //updating exiting model
                                        modelName = ((ModelSettingsBase)objectSettings).Name;
                                        newModel = false;
                                        model2 = db2.Models.Single(m => m.ModelNo == modelName);
                                    }
                                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                                    {
                                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                        {
                                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                            // model parameter updated
                                            if (editCycleParameter.Updated)
                                            {
                                                model2.LastModifiedBy = opId;
                                                model2.LastModified = DateTime.Now;
                                                if (model2.ModelParameters.Any(p => p.ParameterID == property.Name && p.SystemType == VtiLib.ModelDBSystemType))
                                                {
                                                    //updating exiting model parameter
                                                    //ModelParameter param = model2.ModelParameters.Single(p => p.ParameterID == property.Name);
                                                    Data.VtiDataContext2.ModelParameter param = model2.ModelParameters.First(p => p.ParameterID == property.Name && p.SystemType == VtiLib.ModelDBSystemType);
                                                    param.ProcessValue = editCycleParameter.NewValueString;
                                                    param.LastModifiedBy = opId;
                                                    param.LastModified = DateTime.Now;

                                                    Classes.Data.ModelParameterToUpdate modelParameterToUpdate = new Classes.Data.ModelParameterToUpdate();
                                                    modelParameterToUpdate.ModelNo = model2.ModelNo;
                                                    modelParameterToUpdate.ParameterID = param.ParameterID;
                                                    modelParameterToUpdate.ProcessValue = editCycleParameter.NewValueString;
                                                    modelParameterToUpdate.LastModifiedBy = opId;
                                                    modelParameterToUpdate.LastModified = param.LastModified;
                                                    modelParameterToUpdate.SystemType = systemTypeForModelSync;
                                                    listOfModelParametersToUpdate.Add(modelParameterToUpdate);

                                                    Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                                                    changedParameter.OpID = opId;
                                                    changedParameter.SystemID = systemID;
                                                    changedParameter.paramSectionName = node.Text;
                                                    changedParameter.paramName = editCycleParameter.DisplayName;
                                                    changedParameter.oldValue = editCycleParameter.ProcessValueString;
                                                    changedParameter.newValue = editCycleParameter.NewValueString;
                                                    listOfChangedParameters.Add(changedParameter);

                                                    parameterUpdates.Add(String.Format("{0} changed from {1} to {2}",
                                                        property.Name, editCycleParameter.ProcessValueString, editCycleParameter.NewValueString));
                                                }
                                                else
                                                {
                                                    //adding new model parameters (new model created)
                                                    model2.ModelParameters.Add(new Data.VtiDataContext2.ModelParameter
                                                    {
                                                        ParameterID = property.Name,
                                                        ProcessValue = editCycleParameter.NewValueString,
                                                        LastModifiedBy = opId,
                                                        LastModified = DateTime.Now,
                                                        SystemType = VtiLib.ModelDBSystemType
                                                    });

                                                    Classes.Data.ModelParameterToUpdate modelParameterToUpdate = new Classes.Data.ModelParameterToUpdate();
                                                    modelParameterToUpdate.ModelNo = model2.ModelNo;
                                                    modelParameterToUpdate.ParameterID = property.Name;
                                                    modelParameterToUpdate.ProcessValue = editCycleParameter.NewValueString;
                                                    modelParameterToUpdate.LastModifiedBy = opId;
                                                    modelParameterToUpdate.LastModified = model2.ModelParameters.Last().LastModified;
                                                    modelParameterToUpdate.SystemType = systemTypeForModelSync;
                                                    listOfModelParametersToUpdate.Add(modelParameterToUpdate);

                                                    Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                                                    changedParameter.OpID = opId;
                                                    changedParameter.SystemID = systemID;
                                                    changedParameter.paramSectionName = node.Text;
                                                    changedParameter.paramName = editCycleParameter.DisplayName;
                                                    changedParameter.oldValue = "";
                                                    changedParameter.newValue = editCycleParameter.NewValueString;
                                                    listOfChangedParameters.Add(changedParameter);

                                                    parameterUpdates.Add(String.Format("{0}: {1}",
                                                        property.Name, editCycleParameter.NewValueString));
                                                }
                                            }
                                        }
                                    }
                                    if (newModel)
                                    {
                                        db2.Models.InsertOnSubmit(model2);
                                        if (syncModelsWithRemoteDatabase)
                                        {
                                            ModelParameterToUpdate m = new ModelParameterToUpdate
                                            {
                                                ModelNo = model2.ModelNo,
                                                LastModified = model2.LastModified,
                                                LastModifiedBy = model2.LastModifiedBy,
                                                SystemType = systemTypeForModelSync
                                            };
                                            AddModelToRemoteDatabase(m);
                                        }
                                    }
                                    else
                                    {
                                        if (syncModelsWithRemoteDatabase)
                                        {
                                            ModelParameterToUpdate m = new ModelParameterToUpdate
                                            {
                                                ModelNo = model2.ModelNo,
                                                LastModified = model2.LastModified,
                                                LastModifiedBy = model2.LastModifiedBy,
                                                SystemType = systemTypeForModelSync
                                            };
                                            UpdateModelToRemoteDatabase(m);
                                        }
                                        if (parameterUpdates.Count > 0)
                                        {
                                            VtiEvent.Log.WriteWarning(
                                                String.Format("Model '{0}' updated by user '{1}'",
                                                    modelName, opId),
                                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Parameter_Update,
                                                parameterUpdates.ToArray());
                                        }
                                    }
                                }
                                else
                                {
                                    // Add new model
                                    if (((ModelSettingsBase)objectSettings).Name.Substring(0, 1) == "!")
                                    {
                                        modelName = ((ModelSettingsBase)objectSettings).Name.Substring(1);
                                        newModel = true;
                                        model = new Model
                                        {
                                            ModelNo = modelName,
                                            LastModifiedBy = opId,
                                            LastModified = DateTime.Now
                                        };

                                        Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                                        changedParameter.OpID = opId;
                                        changedParameter.SystemID = systemID;
                                        changedParameter.paramSectionName = VtiLibLocalization.NewModelAdded;
                                        changedParameter.paramName = VtiLibLocalization.NewModelAdded;
                                        changedParameter.oldValue = "";
                                        changedParameter.newValue = modelName;
                                        listOfChangedParameters.Add(changedParameter);

                                        VtiEvent.Log.WriteWarning(
                                            String.Format("Model '{0}' added by user '{1}'",
                                                modelName, opId),
                                            VtiEventCatType.Parameter_Update,
                                            parameterUpdates.ToArray());

                                        //VtiEvent.Log.WriteWarning(
                                        //    String.Format("Model '{0}' added by user '{1}'.",
                                        //        modelName, VtiLib.Config.GetField("OpID").GetValue(null)),
                                        //    VTIWindowsControlLibrary.Enum.VtiEventCatType.Parameter_Update);
                                    }
                                    else
                                    {
                                        //updating exiting model
                                        modelName = ((ModelSettingsBase)objectSettings).Name;
                                        newModel = false;
                                        model = db.Models.Single(m => m.ModelNo == modelName);
                                    }
                                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                                    {
                                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                        {
                                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                            // model parameter updated
                                            if (editCycleParameter.Updated)
                                            {
                                                model.LastModifiedBy = opId;
                                                model.LastModified = DateTime.Now;
                                                if (model.ModelParameters.Any(p => p.ParameterID == property.Name))
                                                {
                                                    //updating exiting model parameter
                                                    //ModelParameter param = model.ModelParameters.Single(p => p.ParameterID == property.Name);
                                                    ModelParameter param = model.ModelParameters.First(p => p.ParameterID == property.Name);
                                                    param.ProcessValue = editCycleParameter.NewValueString;
                                                    param.LastModifiedBy = opId;
                                                    param.LastModified = DateTime.Now;

                                                    Classes.Data.ModelParameterToUpdate modelParameterToUpdate = new Classes.Data.ModelParameterToUpdate();
                                                    modelParameterToUpdate.ModelNo = model.ModelNo;
                                                    modelParameterToUpdate.ParameterID = param.ParameterID;
                                                    modelParameterToUpdate.ProcessValue = editCycleParameter.NewValueString;
                                                    modelParameterToUpdate.LastModifiedBy = opId;
                                                    modelParameterToUpdate.LastModified = param.LastModified;
                                                    modelParameterToUpdate.SystemType = systemTypeForModelSync;
                                                    listOfModelParametersToUpdate.Add(modelParameterToUpdate);

                                                    Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                                                    changedParameter.OpID = opId;
                                                    changedParameter.SystemID = systemID;
                                                    changedParameter.paramSectionName = node.Text;
                                                    changedParameter.paramName = editCycleParameter.DisplayName;
                                                    changedParameter.oldValue = editCycleParameter.ProcessValueString;
                                                    changedParameter.newValue = editCycleParameter.NewValueString;
                                                    listOfChangedParameters.Add(changedParameter);

                                                    parameterUpdates.Add(String.Format("{0} changed from {1} to {2}",
                                                        property.Name, editCycleParameter.ProcessValueString, editCycleParameter.NewValueString));
                                                }
                                                else
                                                {
                                                    //adding new model parameters (new model created)
                                                    model.ModelParameters.Add(new ModelParameter
                                                    {
                                                        ParameterID = property.Name,
                                                        ProcessValue = editCycleParameter.NewValueString,
                                                        LastModifiedBy = opId,
                                                        LastModified = DateTime.Now
                                                    });

                                                    Classes.Data.ModelParameterToUpdate modelParameterToUpdate = new Classes.Data.ModelParameterToUpdate();
                                                    modelParameterToUpdate.ModelNo = model.ModelNo;
                                                    modelParameterToUpdate.ParameterID = property.Name;
                                                    modelParameterToUpdate.ProcessValue = editCycleParameter.NewValueString;
                                                    modelParameterToUpdate.LastModifiedBy = opId;
                                                    modelParameterToUpdate.LastModified = model.ModelParameters.Last().LastModified;
                                                    modelParameterToUpdate.SystemType = systemTypeForModelSync;
                                                    listOfModelParametersToUpdate.Add(modelParameterToUpdate);

                                                    Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                                                    changedParameter.OpID = opId;
                                                    changedParameter.SystemID = systemID;
                                                    changedParameter.paramSectionName = node.Text;
                                                    changedParameter.paramName = editCycleParameter.DisplayName;
                                                    changedParameter.oldValue = "";
                                                    changedParameter.newValue = editCycleParameter.NewValueString;
                                                    listOfChangedParameters.Add(changedParameter);

                                                    parameterUpdates.Add(String.Format("{0}: {1}",
                                                        property.Name, editCycleParameter.NewValueString));
                                                }
                                            }
                                        }
                                    }
                                    if (newModel)
                                    {
                                        db.Models.InsertOnSubmit(model);
                                        if (syncModelsWithRemoteDatabase)
                                        {
                                            ModelParameterToUpdate m = new ModelParameterToUpdate
                                            {
                                                ModelNo = model.ModelNo,
                                                LastModified = model.LastModified,
                                                LastModifiedBy = model.LastModifiedBy,
                                                SystemType = systemTypeForModelSync
                                            };
                                            AddModelToRemoteDatabase(m);
                                        }
                                    }
                                    else
                                    {
                                        if (syncModelsWithRemoteDatabase)
                                        {
                                            ModelParameterToUpdate m = new ModelParameterToUpdate
                                            {
                                                ModelNo = model.ModelNo,
                                                LastModified = model.LastModified,
                                                LastModifiedBy = model.LastModifiedBy,
                                                SystemType = systemTypeForModelSync
                                            };
                                            UpdateModelToRemoteDatabase(m);
                                        }
                                        if (parameterUpdates.Count > 0)
                                        {
                                            VtiEvent.Log.WriteWarning(
                                                String.Format("Model '{0}' updated by user '{1}'",
                                                    modelName, opId),
                                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Parameter_Update,
                                                parameterUpdates.ToArray());
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                // Delete Models
                foreach (ModelSettingsBase objectSettings in modelsToDelete)
                {
                    #region needs cleanup 2
                    if (VtiLib.UseRemoteModelDB)
                    {
                        string modelName = objectSettings.Name;
                        if (db2.Models.Count(m => m.ModelNo == modelName) == 1)
                        {
                            model2 = db2.Models.Single(m => m.ModelNo == modelName);
                            db2.Models.DeleteOnSubmit(model2);

                            if (syncModelsWithRemoteDatabase)
                            {
                                ModelParameterToUpdate m = new ModelParameterToUpdate
                                {
                                    ModelNo = model2.ModelNo,
                                    LastModified = model2.LastModified,
                                    LastModifiedBy = model2.LastModifiedBy,
                                    SystemType = systemTypeForModelSync
                                };
                                DeleteModelFromRemoteDatabase(m);
                            }

                            Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                            changedParameter.OpID = opId;
                            changedParameter.SystemID = systemID;
                            changedParameter.paramSectionName = VtiLibLocalization.ModelDeleted;
                            changedParameter.paramName = VtiLibLocalization.ModelDeleted;
                            changedParameter.oldValue = modelName;
                            changedParameter.newValue = "";
                            listOfChangedParameters.Add(changedParameter);

                            VtiEvent.Log.WriteWarning(
                                String.Format("Model '{0}' deleted by user '{1}'",
                                modelName, opId),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Parameter_Update);
                        }
                    }
                    else
                    {
                        string modelName = objectSettings.Name;
                        if (db.Models.Count(m => m.ModelNo == modelName) == 1)
                        {
                            model = db.Models.Single(m => m.ModelNo == modelName);
                            db.Models.DeleteOnSubmit(model);

                            if (syncModelsWithRemoteDatabase)
                            {
                                ModelParameterToUpdate m = new ModelParameterToUpdate
                                {
                                    ModelNo = model.ModelNo,
                                    LastModified = model.LastModified,
                                    LastModifiedBy = model.LastModifiedBy,
                                    SystemType = systemTypeForModelSync
                                };
                                DeleteModelFromRemoteDatabase(m);
                            }

                            Classes.Data.ChangedParameter changedParameter = new Classes.Data.ChangedParameter();
                            changedParameter.OpID = opId;
                            changedParameter.SystemID = systemID;
                            changedParameter.paramSectionName = VtiLibLocalization.ModelDeleted;
                            changedParameter.paramName = VtiLibLocalization.ModelDeleted;
                            changedParameter.oldValue = modelName;
                            changedParameter.newValue = "";
                            listOfChangedParameters.Add(changedParameter);

                            VtiEvent.Log.WriteWarning(
                                String.Format("Model '{0}' deleted by user '{1}'",
                                modelName, opId),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Parameter_Update);
                        }
                    }
                    #endregion
                }

                try
                {
                    if (VtiLib.UseRemoteModelDB)
                    {
                        // Submit the changes to the database
                        db2.SubmitChanges();
                    }
                    else
                    {
                        // Submit the changes to the database
                        db.SubmitChanges();
                    }

                    // Complete the transaction
                    ts.Complete();
                }
                catch (Exception e)
                {
                    string msg = "Error submitting Edit Cycle changes. Exception: " + e.Message;
                    MessageBox.Show(msg);
                    VtiEvent.Log.WriteError(msg);
                }
            }

            if (syncModelsWithRemoteDatabase && listOfModelParametersToUpdate.Count > 0)
            {
                AddOrUpdateModelParametersToRemoteDatabase(listOfModelParametersToUpdate);
            }
            //cannot log parameter changes within the TransactionScope above because it performs a SQL insert statement, causing a SQL error.
            LogParameterChanges(listOfChangedParameters);
            VtiLib.overrideUser = null;
            // Save the Config to the user.config file
            VtiLib.Config._Save();

            if (COMPortAlreadyInUseList.Count > 0)
            {
                string msg = "COM port(s) selected already in use by an existing device. Changes were not applied." + Environment.NewLine + Environment.NewLine;
                foreach (var comPort in COMPortAlreadyInUseList)
                {
                    msg += $"Device that was assigned COM port: {comPort.deviceThatWasAssignedCOMPort}" + Environment.NewLine;
                    msg += $"Device already using COM port: {comPort.deviceAlreadyUsingCOMPort}" + Environment.NewLine;
                    msg += $"COM port: {comPort.COMPortNumberInUse}" + Environment.NewLine + Environment.NewLine;
                }
                MessageBox.Show(msg);
            }

            if (refreshCOMports)
            {
                VtiLib.Machine.ConfigChanged(true);
            }
            else
            {
                VtiLib.Machine.ConfigChanged(false);
            }
            if (bHideEditCycleForm)
                this.Hide();
            if (VtiLib.UseRemoteModelDB)
            {
                db2.Dispose();
            }
            else
            {
                db.Dispose();
            }
            this.Cursor = Cursors.Default;
            VtiLib.MuteParamChangeLog = false;
        }

        private void copyModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UsesVtiDataDatabase)
            {
                bool bIsDuplicateModelName = true;
                String newModel = "";
                while (bIsDuplicateModelName)
                {
                    bIsDuplicateModelName = false;
                    newModel = InputBox.Show("Enter a name for the new model", "Copy '" + this.treeView1.SelectedNode.Name + "'", "", this.contextMenuStripModel.Left - 100, this.contextMenuStripModel.Top - 50);
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if (node.Text.Equals("Model " + newModel + " Parameters"))
                        {
                            bIsDuplicateModelName = true;
                            break;
                        }
                    }
                }
                if (newModel != "")
                {
                    ModelSettingsBase modelSettings, modelSettingsToCopy;
                    TreeNode treeNode, treeNode1;
                    Boolean found;
                    Type type2;
                    PropertyInfo[] properties2;

                    modelSettings = Activator.CreateInstance(VtiLib.ModelSettingsType) as ModelSettingsBase;

                    // If this is a parameter node, get the node of the parent
                    if (treeView1.SelectedNode.Level == 0)
                        treeNode = treeView1.SelectedNode;
                    else if (treeView1.SelectedNode.Level == 1)
                        treeNode = treeView1.SelectedNode.Parent;
                    else
                        treeNode = treeView1.SelectedNode.Parent.Parent;

                    modelSettingsToCopy = treeNode.Tag as ModelSettingsBase;

                    modelSettings.Name = "!" + newModel;

                    // Create the tree node for the current settings object
                    treeNode1 = new TreeNode();
                    treeNode1.Name = "Model " + newModel;
                    treeNode1.Text = "Model " + newModel + " Parameters";
                    treeNode1.Tag = modelSettings;  // save the ModelSettings in the tag of the TreeNode
                    found = false;
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if ((!(node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))) &&
                            (treeNode1.Text.CompareTo(node.Text) < 0))
                        {
                            treeView1.Nodes.Insert(node.Index, treeNode1);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        treeView1.Nodes.Add(treeNode1);
                    treeView1.SelectedNode = treeNode1; // make this the currently selected node
                                                        // Get the properties of the current settings object
                    type2 = modelSettings.GetType();
                    properties2 = type2.GetProperties();
                    foreach (PropertyInfo property in properties2)
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameterToCopy = property.GetValue(modelSettingsToCopy, null) as IEditCycleParameter;
                            IEditCycleParameter editCycleParameter = property.GetValue(modelSettings, null) as IEditCycleParameter;
                            editCycleParameter.NewValueString = editCycleParameterToCopy.ProcessValueString;
                            editCycleParameter.ProcessValueString = editCycleParameter.NewValueString;
                            // do not display the parameter if it is invisible in DEFAULT model
                            foreach (TreeNode node in treeView1.Nodes)
                            {
                                if (node.Text.Equals("Model DEFAULT Parameters"))
                                {
                                    object objectSettings_DEFAULT = node.Tag;
                                    foreach (PropertyInfo property_DEFAULT in objectSettings_DEFAULT.GetType().GetProperties())
                                    {
                                        if (property_DEFAULT.PropertyType.GetInterface("IEditCycleParameter") != null)
                                        {
                                            IEditCycleParameter editCycleParameter_DEFAULT = property.GetValue(objectSettings_DEFAULT, null) as IEditCycleParameter;
                                            if (editCycleParameter_DEFAULT.DisplayName == editCycleParameter.DisplayName)
                                            {
                                                if (editCycleParameter_DEFAULT.Visible)
                                                {
                                                    //TreeNode newNode = editCycleParameter.CreateTreeNode(property.Name);
                                                    TreeNode newNode = editCycleParameter.CreateTreeNode(property, editCycleParameter, this);
                                                    object objectSettings = property;
                                                    if (objectSettings != null)
                                                        newNode.Tag = objectSettings; // save objectSettings in the tag of the new node
                                                    treeNode1.Nodes.Add(newNode);
                                                    editCycleParameter.Updated = true;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //VtiEvent.Log.WriteWarning("Model '" + newModel + " added by user '" +
                    //    VtiLib.Config.GetField("OpID").GetValue(null) + "'. (Copied from model '" +
                    //    modelSettingsToCopy.GetType().GetField("Name").GetValue(modelSettingsToCopy) + "'.)",
                    //    VTIWindowsControlLibrary.Enum.VtiEventCatType.Parameter_Update);
                }
            }
            else
            {
                MessageBox.Show("Custom models are not supported with this application.");
            }
        }

        private void deleteModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelSettingsBase modelSettings;
            TreeNode treeNode;
            // If this is a parameter node, get the node of the parent
            if (treeView1.SelectedNode.Level == 0)
                treeNode = treeView1.SelectedNode;
            else if (treeView1.SelectedNode.Level == 1)
                treeNode = treeView1.SelectedNode.Parent;
            else
                treeNode = treeView1.SelectedNode.Parent.Parent;

            modelSettings = (ModelSettingsBase)treeNode.Tag;
            string strModel = modelSettings.Name;
            if (strModel.StartsWith("!"))
                strModel = modelSettings.Name.Substring(1);
            if (MessageBox.Show("Are you sure you want to delete model '" + strModel + "'?", "Delete Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                modelsToDelete.Add(modelSettings);
                treeNode.Remove();  // remove the node from the tree
            }
        }

        private void EditCycleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing ||
                e.CloseReason == CloseReason.FormOwnerClosing ||
                e.CloseReason == CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }

            //db.Dispose();
        }

        private void newModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UsesVtiDataDatabase)
            {
                bool bIsDuplicateModelName = true;
                String newModel = "";
                while (bIsDuplicateModelName)
                {
                    bIsDuplicateModelName = false;
                    newModel = InputBox.Show("Enter a name for the new model", "New Model from 'Model DEFAULT'", "", this.contextMenuStripModel.Left - 100, this.contextMenuStripModel.Top - 50);
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if (node.Text.Equals("Model " + newModel + " Parameters"))
                        {
                            bIsDuplicateModelName = true;
                            break;
                        }
                    }
                }
                if (newModel != "")
                {
                    ModelSettingsBase modelSettings, modelSettingsToCopy;
                    TreeNode treeNode, treeNode1;
                    Boolean found;
                    modelSettings = Activator.CreateInstance(VtiLib.ModelSettingsType) as ModelSettingsBase;
                    modelSettings.Name = "!" + newModel;

                    // Create the tree node for the current settings object
                    treeNode1 = new TreeNode();
                    treeNode1.Name = "Model " + newModel;
                    treeNode1.Text = "Model " + newModel + " Parameters";
                    treeNode1.Tag = modelSettings;  // save the ModelSettings in the tag of the TreeNode
                                                    // insert the new node in alphabetical order
                    found = false;
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if ((!(node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))) &&
                            (treeNode1.Text.CompareTo(node.Text) < 0))
                        {
                            treeView1.Nodes.Insert(node.Index, treeNode1);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        treeView1.Nodes.Add(treeNode1);
                    treeView1.SelectedNode = treeNode1; // make this the currently selected node
                    treeNode = null;
                    //foreach(TreeNode node in treeView1.Nodes.Find("Model DEFAULT Parameters", true)) {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if (node.Text == "Model DEFAULT Parameters")
                        {
                            treeNode = node;
                            break;
                        }
                    }
                    modelSettingsToCopy = treeNode.Tag as ModelSettingsBase;
                    // Get the properties of the current settings object
                    foreach (PropertyInfo property in modelSettings.GetType().GetProperties())
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameterToCopy = property.GetValue(modelSettingsToCopy, null) as IEditCycleParameter;
                            IEditCycleParameter editCycleParameter = property.GetValue(modelSettings, null) as IEditCycleParameter;
                            editCycleParameter.NewValueString = editCycleParameterToCopy.ProcessValueString;
                            editCycleParameter.ProcessValueString = editCycleParameter.NewValueString;
                            // do not display the parameter if it is invisible in DEFAULT model
                            foreach (TreeNode node in treeView1.Nodes)
                            {
                                if (node.Text.Equals("Model DEFAULT Parameters"))
                                {
                                    object objectSettings_DEFAULT = node.Tag;
                                    foreach (PropertyInfo property_DEFAULT in objectSettings_DEFAULT.GetType().GetProperties())
                                    {
                                        if (property_DEFAULT.PropertyType.GetInterface("IEditCycleParameter") != null)
                                        {
                                            IEditCycleParameter editCycleParameter_DEFAULT = property.GetValue(objectSettings_DEFAULT, null) as IEditCycleParameter;
                                            if (editCycleParameter_DEFAULT.DisplayName == editCycleParameter.DisplayName)
                                            {
                                                if (editCycleParameter_DEFAULT.Visible)
                                                {
                                                    //TreeNode newNode = editCycleParameter.CreateTreeNode(property.Name);
                                                    TreeNode newNode = editCycleParameter.CreateTreeNode(property, editCycleParameter, this);
                                                    object objectSettings = property;
                                                    if (objectSettings != null)
                                                        newNode.Tag = objectSettings; // save objectSettings in the tag of the new node
                                                    treeNode1.Nodes.Add(newNode);
                                                    editCycleParameter.Updated = true;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //treeView1.SelectedNode = treeNode1;
                    //treeNode1.Expand();

                    //VtiEvent.Log.WriteWarning("Model '" + newModel + " added by user '" +
                    //    VtiLib.Config.GetField("OpID").GetValue(null) + "'.",
                    //    VTIWindowsControlLibrary.Enum.VtiEventCatType.Parameter_Update);
                }
            }
            else
            {
                MessageBox.Show("Custom models are not supported with this application.");
            }
        }

        private void numberPadControl_CurrentSettingChanged(object sender, CurrentSettingChangedEventArgs e)
        {
            if (currentParameter is NumericParameter)
            {
                NumericParameter numericParameter = currentParameter as NumericParameter;
                if ((e.CurrentSetting <= numericParameter.MaxValue) && (e.CurrentSetting >= numericParameter.MinValue))
                {
                    numericParameter.NewValue = e.CurrentSetting;
                }
                else
                    MessageBox.Show("Value entered is out of range!", "Edit Cycle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (currentParameter is TimeDelayParameter)
            {
                TimeDelayParameter timeDelayParameter = currentParameter as TimeDelayParameter;
                if ((e.CurrentSetting <= timeDelayParameter.MaxValue) && (e.CurrentSetting >= timeDelayParameter.MinValue))
                {
                    timeDelayParameter.NewValue = e.CurrentSetting;
                }
                else
                    MessageBox.Show("Value entered is out of range!", "Edit Cycle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// When the user selects an item in the treeview, this displays the current process value
        /// and arranges various display elements on the Edit Cycle form to match the current data type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.SuspendLayout();

            Control control = tableLayoutPanel2.GetControlFromPosition(0, 1);
            if (control != null) tableLayoutPanel2.Controls.Remove(control);

            // 1st level treenode
            if (treeView1.SelectedNode.Level == 0)
            {
                //this.groupBoxKeyPad.Visible = false;
                this.numberPadControl.Visible = false;
                //this.groupBoxParameter.Visible = false;
            }
            else
            {
                // 2nd level treenode
                if (treeView1.SelectedNode.Level == 1)
                {
                    object objectSettings = treeView1.SelectedNode.Parent.Tag;

                    if (objectSettings != null)
                    {
                        PropertyInfo property = (PropertyInfo)(treeView1.SelectedNode.Tag);
                        numberPadControl.Visible = false;
                        if (property != null)
                        {
                            currentParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                            Control editorControl = currentParameter.GetEditorControl(property);
                            tableLayoutPanel2.Controls.Add(editorControl, 0, 1);
                            editorControl.Dock = DockStyle.Fill;
                            if (currentParameter is NumericParameter)
                            {
                                numberPadControl.Visible = true;
                                numberPadControl.MinValue = (currentParameter as NumericParameter).MinValue;
                                numberPadControl.MaxValue = (currentParameter as NumericParameter).MaxValue;
                            }
                            else if (currentParameter is TimeDelayParameter)
                            {
                                numberPadControl.Visible = true;
                                numberPadControl.MinValue = (currentParameter as TimeDelayParameter).MinValue;
                                numberPadControl.MaxValue = (currentParameter as TimeDelayParameter).MaxValue;
                            }
                        }
                    }
                }
                // 3rd level treenode
                else
                {
                    object objectSettings = treeView1.SelectedNode.Parent.Parent.Tag;
                    if (objectSettings != null)
                    {
                        PropertyInfo property = (PropertyInfo)(treeView1.SelectedNode.Parent.Tag);
                        IEditCycleParameter editCycleParamter =
                            property.GetValue(objectSettings, null) as IEditCycleParameter;
                        Control editorControl = editCycleParamter.GetEditorControl(property, treeView1.SelectedNode.Name);
                        tableLayoutPanel2.Controls.Add(editorControl, 0, 1);
                    }
                }
            }

            this.ResumeLayout();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if ((e.Node.Level == 0) && (e.Node.Nodes.Count == 1) && (e.Node.Nodes[0].Text == "Place Holder"))
            {
                BuildNode(e.Node);
            }
        }

        /// <summary>
        /// Populates Operating Sequence list based on client XML configuration in Edit Cycle
        /// </summary>
        /// <param name="List<string>"></param>
        public void AssignOperatingSequences()
        {
            operatingSeqList.Clear();
            //TreeNode treeNode = null;
            //int ndx = 0;
            // Enumerate through the tree nodes
            foreach (TreeNode node in treeView1.Nodes)
            {
                // Node is Top-Level and has child nodes
                //if ((node.Level == 0) && (node.Nodes.Count > 0) && (node.Nodes[0].Text != "Place Holder")) {
                if ((node.Level == 0) && (node.Nodes.Count > 0))
                {
                    object objectSettings = node.Tag;
                    // Settings in the user.config file
                    if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                    {
                        foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                        {
                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                            {
                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                if (editCycleParameter.OperatingSequence != null)
                                    operatingSeqList.Add(editCycleParameter.OperatingSequence.ToUpper().Replace(" ", ""));
                            }
                        }
                    }
                }
            }
            List<string> s = new List<string>();
            foreach (string s2 in operatingSeqList)
            {
                string[] strSplit = s2.Split(',');
                foreach (string s3 in strSplit)
                {
                    if (!s.Contains(s3))
                        s.Add(s3);
                }
            }
            s.Sort();
            operatingSeqList.Clear();
            foreach (string s2 in s)
                operatingSeqList.Add(s2);
        }

        /// <summary>
        /// When the user selects an item in the Edit Cycle Search Form, this displays the current item in Edit Cycle
        /// </summary>
        /// <param name="EditCycleSearchForm.EditCycleSearchParameter"></param>
        public void SelectFromSearchForm(EditCycleSearchForm.EditCycleSearchParameter ecsp)
        {
            Model model;
            Data.VtiDataContext2.Model model2;
            TreeNode treeNode = null;
            int ndx = 0;
            //bool bIsEmptyModelList = true;
            //if (modelNameList.Count > 0)
            //  bIsEmptyModelList = false;
            foreach (TreeNode node in treeView1.Nodes)
            {
                // Node is Top-Level and has child nodes
                if ((node.Level == 0) && (node.Nodes.Count > 0))
                {
                    object objectSettings = node.Tag;
                    // Settings in the user.config file
                    if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                    {
                        foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                        {
                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                            {
                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                if (node.Text == ecsp.Group && editCycleParameter.DisplayName == ecsp.Name &&
                                  (editCycleParameter.ProcessValueString == ecsp.Value || editCycleParameter.ProcessValueString == "VTIWindowsControlLibrary.Classes.IO.SerialIO.SerialPortSettings") &&
                                  editCycleParameter.ToolTip == ecsp.ToolTip)
                                {
                                    treeNode = node;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    { // Model settings in the database
                        string modelName;
                        modelName = objectSettings.ToString();
                        if (modelName.Contains("Classes.Configuration.ModelSettings"))
                            modelName = modelNameList[ndx];
                        ndx++;
                        try
                        {
                            if (VtiLib.UseRemoteModelDB)
                            {
                                model2 = db2.Models.Single(m => m.ModelNo == modelName);
                                foreach (Data.VtiDataContext2.ModelParameter param in db2.Models.SingleOrDefault(m => m.ModelNo == modelName).ModelParameters.Where(p => p.SystemType == VtiLib.ModelDBSystemType))
                                {
                                    if (node.Text == ecsp.Group && param.ParameterID == ecsp.Name && param.ProcessValue == ecsp.Value)
                                    {
                                        treeNode = node;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                model = db.Models.Single(m => m.ModelNo == modelName);
                                foreach (ModelParameter param in db.Models.SingleOrDefault(m => m.ModelNo == modelName).ModelParameters)
                                {
                                    if (node.Text == ecsp.Group && param.ParameterID == ecsp.Name && param.ProcessValue == ecsp.Value)
                                    {
                                        treeNode = node;
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
            if (treeNode == null)
                return;
            if (!treeNode.IsExpanded)
                treeNode.Expand();
            nodeMatch = null;
            foreach (TreeNode node in treeView1.Nodes)
            {
                RecurseNode(node, ecsp);
            }
            if (nodeMatch != null)
                treeView1.SelectedNode = nodeMatch;
        }

        private void RecurseNode(TreeNode treeNode, EditCycleSearchForm.EditCycleSearchParameter ecsp)
        {
            if (nodeMatch != null)
                return;
            string strEcsp = ecsp.Group + @"\" + ecsp.Name;
            if (treeNode.FullPath == strEcsp)
            {
                nodeMatch = treeNode;
                return;
            }
            if (treeNode.FullPath.ToLower().Replace(" ", "") == strEcsp.ToLower().Replace(" ", ""))
            {
                nodeMatch = treeNode;
                return;
            }
            foreach (TreeNode tn in treeNode.Nodes)
            {
                RecurseNode(tn, ecsp);
            }
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            Object objectSettings;
            TreeNode treeNode;

            if (e.Button == MouseButtons.Left)
            {
                treeNode = this.treeView1.GetNodeAt(e.X, e.Y);
                // Check to see if the mouse was clicked on the treenode text
                if ((treeNode != null) && (treeNode.Bounds.Contains(e.Location)))
                {
                    this.treeView1.SelectedNode = treeNode;
                    // Expand the node where the mouse was clicked.
                    if (!treeView1.SelectedNode.IsExpanded)
                        treeView1.SelectedNode.Expand();
                }
            }

            // Show the context menu when the user right-clicks in the treeview
            if (e.Button == MouseButtons.Right)
            {
                // Highlight the node where the mouse was clicked.  Right-click doesn't normally do this.
                this.treeView1.SelectedNode = this.treeView1.GetNodeAt(e.X, e.Y);
                // Make sure a node is selected
                if (this.treeView1.SelectedNode != null)
                {
                    // If this is a parameter node, get the node of the parent
                    if (treeView1.SelectedNode.Level == 0)
                        treeNode = treeView1.SelectedNode;
                    else if (treeView1.SelectedNode.Level == 1)
                        treeNode = treeView1.SelectedNode.Parent;
                    else
                        treeNode = treeView1.SelectedNode.Parent.Parent;

                    // Build the treenode
                    if ((treeNode.Nodes.Count == 1) && (treeNode.Nodes[0].Text == "Place Holder"))
                    {
                        BuildNode(treeNode);
                    }

                    // Get the settings object from the treeNode.Tag
                    objectSettings = treeNode.Tag;

                    // Make sure the object isn't null
                    if (objectSettings != null)
                    {
                        // If it's a model (and not the default model), make the Copy & Delete menus visible
                        if ((objectSettings.GetType().Name == "ModelSettings") &&// == typeof(ModelSettings)) &&
                            (treeNode.Name != "DefaultModel"))
                        {
                            this.copyModelToolStripMenuItem.Visible = true;
                            this.deleteModelToolStripMenuItem.Visible = true;
                        }
                        // If it's the default model, make the Copy menu visible, but hide the delete menu
                        else if ((objectSettings.GetType().Name == "ModelSettings") &&// == typeof(ModelSettings)) &&
                            (treeNode.Name == "DefaultModel"))
                        {
                            this.copyModelToolStripMenuItem.Visible = true;
                            this.deleteModelToolStripMenuItem.Visible = false;
                        }
                        // If it's not a model at all, hide both the Copy & Delete menus
                        else
                        {
                            this.copyModelToolStripMenuItem.Visible = false;
                            this.deleteModelToolStripMenuItem.Visible = false;
                        }
                        // Show the context menu
                        this.contextMenuStripModel.Show(treeView1, e.X, e.Y);
                    }
                }
            }
        }

        #endregion Private Methods

        private void buttonExport_Click(object sender, EventArgs e)
        {
            //ArrayList parameterUpdates = new ArrayList();   // list of parameter update strings for event log entry
            Model model;
            Data.VtiDataContext2.Model model2;

            string opId = VtiLib.Config._OpID;

            this.Cursor = Cursors.WaitCursor;
            string folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filename = folderName + $@"\EditCycleParameters_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff")}.txt";
            //filename = filename.Replace('/', '-');
            //filename = filename.Replace(':', '-');
            var csv = new System.Text.StringBuilder();
            //separator character is tab so that Excel does not create a new column when using commas in the description of your parameter
            var sep = "\t";// comma - System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            int ndx = 0;
            bool bIsEmptyModelList = true;
            if (modelNameList.Count > 0)
            {
                bIsEmptyModelList = false;
            }
            // Enumerate through the tree nodes
            foreach (TreeNode node in treeView1.Nodes)
            {
                // Node is Top-Level and has child nodes
                //if ((node.Level == 0) && (node.Nodes.Count > 0) && (node.Nodes[0].Text != "Place Holder")) {
                if ((node.Level == 0) && (node.Nodes.Count > 0))
                {
                    object objectSettings = node.Tag;

                    //parameterUpdates.Clear();

                    // Settings in the config file
                    if ((node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters")) && objectSettings.GetType().GetProperties().Count() > 0)
                    {
                        // write the header for the data
                        csv.AppendLine($"{node.Text + sep}Process Value{sep}Description");
                        foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                        {
                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                            {
                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                if (editCycleParameter.Visible)
                                {
                                    if (property.PropertyType.Name == "SerialPortParameter")
                                    {
                                        var pv = ((SerialPortParameter)editCycleParameter).ProcessValue;
                                        csv.AppendLine(editCycleParameter.DisplayName + sep + pv.PortName.ToString() + "-"
                                            + pv.BaudRate.ToString() + "-" + pv.Parity.ToString() + "-" + pv.DataBits.ToString());
                                        //    + "-" + pv.StopBits.ToString() + "-" + pv.Handshake.ToString() + sep + editCycleParameter.ToolTip);
                                    }
                                    else if (property.PropertyType.Name == "EthernetPortParameter")
                                    {
                                        var pv = ((EthernetPortParameter)editCycleParameter).ProcessValue;
                                        csv.AppendLine(editCycleParameter.DisplayName + sep + pv.PortName.ToString() + "-" + pv.IPAddress.ToString() + ":"
                                            + pv.Port.ToString());
                                    }
                                    else
                                    {
                                        csv.AppendLine(editCycleParameter.DisplayName + sep + editCycleParameter.ProcessValueString + sep + editCycleParameter.ToolTip);
                                    }
                                }
                            }
                        }
                        csv.AppendLine();
                    }
                    else
                    { // Model settings in the database
                        string modelName;
                        modelName = objectSettings.ToString();
                        if (modelName.Contains("Classes.Configuration.ModelSettings"))
                            modelName = modelNameList[ndx];
                        ndx++;
                        if (bIsEmptyModelList)
                            modelNameList.Add(modelName);
                        if (VtiLib.UseRemoteModelDB)
                        {
                            model2 = db2.Models.Single(m => m.ModelNo == modelName);

                            csv.AppendLine(node.Text + sep + $"Process Value{sep}Description");
                            foreach (Data.VtiDataContext2.ModelParameter param in db2.Models.Single(m => m.ModelNo == modelName).ModelParameters.Where(p => p.SystemType == VtiLib.ModelDBSystemType))
                            {
                                //if the parameter is not visible to the user, don't export it
                                //the visible property only exists in the Default Model, so find the matching parameter then see if it is visible
                                foreach (TreeNode n in treeView1.Nodes)
                                {
                                    if (n.Text.Equals("Model DEFAULT Parameters"))
                                    {
                                        object objectSettingsDefaultModel = n.Tag;
                                        foreach (PropertyInfo property in objectSettingsDefaultModel.GetType().GetProperties())
                                        {
                                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                            {
                                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettingsDefaultModel, null) as IEditCycleParameter;
                                                if (property.Name == param.ParameterID)
                                                {
                                                    if (editCycleParameter.Visible)
                                                    {
                                                        csv.AppendLine(editCycleParameter.DisplayName + sep + param.ProcessValue + sep + editCycleParameter.ToolTip);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            csv.AppendLine();
                        }
                        else
                        {
                            model = db.Models.Single(m => m.ModelNo == modelName);

                            csv.AppendLine(node.Text + sep + $"Process Value{sep}Description");
                            foreach (ModelParameter param in db.Models.Single(m => m.ModelNo == modelName).ModelParameters)
                            {
                                //if the parameter is not visible to the user, don't export it
                                //the visible property only exists in the Default Model, so find the matching parameter then see if it is visible
                                foreach (TreeNode n in treeView1.Nodes)
                                {
                                    if (n.Text.Equals("Model DEFAULT Parameters"))
                                    {
                                        object objectSettingsDefaultModel = n.Tag;
                                        foreach (PropertyInfo property in objectSettingsDefaultModel.GetType().GetProperties())
                                        {
                                            if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                            {
                                                IEditCycleParameter editCycleParameter = property.GetValue(objectSettingsDefaultModel, null) as IEditCycleParameter;
                                                if (property.Name == param.ParameterID)
                                                {
                                                    if (editCycleParameter.Visible)
                                                    {
                                                        csv.AppendLine(editCycleParameter.DisplayName + sep + param.ProcessValue + sep + editCycleParameter.ToolTip);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            csv.AppendLine();
                        }
                    }
                }
            }
            File.WriteAllText(filename, csv.ToString());
            MessageBox.Show("Parameters saved to " + Environment.NewLine + filename, "Exported Parameters",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                System.Diagnostics.Process.Start(folderName);
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
            }
            this.Cursor = Cursors.Default;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            // display the Edit Cycle Search window
            if (!bIsOpenEditCycleSearchForm)
            {
                EditCycleSearchForm frmEditCycleSearch = new EditCycleSearchForm(this);
                frmEditCycleSearch.Show(this.MdiParent);
                bIsOpenEditCycleSearchForm = true;
            }
        }

        #endregion Methods

        private void EditCycleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonOK.PerformClick();
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {


            // List indices correspond to config sections 0:Control 1:Mode 2:Press 3:Time 4:Flow 5:Model
            List<SortedDictionary<string, IEditCycleParameter>> existingParams = new List<SortedDictionary<string, IEditCycleParameter>>();
            List<SortedDictionary<string, KeyValuePair<string, string>>> newParams = new List<SortedDictionary<string, KeyValuePair<string, string>>>();

            // Data structures that store existing/new model parameters in format {modelName, {parameterName, parameterValue}}
            SortedDictionary<string, SortedDictionary<string, IEditCycleParameter>> existingModels = new SortedDictionary<string, SortedDictionary<string, IEditCycleParameter>>();
            SortedDictionary<string, SortedDictionary<string, string>> newModels = new SortedDictionary<string, SortedDictionary<string, string>>();
            List<TreeNode> nodesToRemove = new List<TreeNode>();

            for (int i = 0; i < 6; i++)
            {
                existingParams.Add(new SortedDictionary<string, IEditCycleParameter>());
                newParams.Add(new SortedDictionary<string, KeyValuePair<string, string>>());
            }

            #region read in new param values and models

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;
                string[] readText = File.ReadAllLines(filePath);

                int index = 0;
                bool isModel = false;
                string currentModelName = "";

                for (int i = 0; i < readText.Length; i++)
                {
                    string[] items = readText[i].Split('\t');
                    if (items.Length < 2) continue;

                    if (readText[i].StartsWith("Common Control")) index = 0;
                    else if (readText[i].StartsWith("Common Mode")) index = 1;
                    else if (readText[i].StartsWith("Common Pressure")) index = 2;
                    else if (readText[i].StartsWith("Common Time")) index = 3;
                    else if (readText[i].StartsWith("Common Flow")) index = 4;
                    else if (readText[i].StartsWith("Model DEFAULT")) index = 5;
                    else if (items[0].StartsWith("Model") && items[1] == "Process Value" && items[2] == "Description")
                    {
                        isModel = true;
                        var match = Regex.Match(items[0], @"Model ([\w\s]+) Parameters");
                        currentModelName = match.Groups[1].Value;
                        newModels.Add(currentModelName, new SortedDictionary<string, string>());
                    }
                    else if (isModel)
                    {
                        newModels[currentModelName].Add(items[0], items[1]);
                    }
                    else
                    {
                        if (items.Length == 2)
                        {
                            newParams[index].Add(items[0], new KeyValuePair<string, string>(items[1], ""));
                        }
                        else
                        {
                            newParams[index].Add(items[0], new KeyValuePair<string, string>(items[1], items[2]));
                        }
                    }

                }
            }
            else
            {
                return;
            }
            if (MessageBox.Show("Warning: This will overwrite all parameter values and models.\n\nContinue?", "Import Parameters", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            #endregion

            #region Store existing params in data structure

            TreeNode defaultNode = new TreeNode();
            foreach (TreeNode node0 in treeView1.Nodes)
            {
                if (node0.Text == "Model DEFAULT Parameters")
                {
                    defaultNode = node0;
                    break;
                }
            }

            // Add param names to list and parameters to parallel list
            foreach (TreeNode node in treeView1.Nodes)
            {

                Console.WriteLine(node.Text);
                if (node.Text.StartsWith("Common Control"))
                {
                    addParamToList(node, existingParams[0]);

                }
                else if (node.Text.StartsWith("Common Mode"))
                {
                    addParamToList(node, existingParams[1]);

                }
                else if (node.Text.StartsWith("Common Pressure"))
                {
                    addParamToList(node, existingParams[2]);

                }
                else if (node.Text.StartsWith("Common Time"))
                {
                    addParamToList(node, existingParams[3]);

                }
                else if (node.Text.StartsWith("Common Flow"))
                {
                    addParamToList(node, existingParams[4]);

                }
                else if (node.Text.StartsWith("Model DEFAULT"))
                {
                    addParamToList(node, existingParams[5]);
                }
                else
                { // Custom model

                    object objectSettings = node.Tag;

                    var match = Regex.Match(node.Text, @"Model ([\w\s]+) Parameters");
                    string modelName = match.Groups[1].Value;

                    if (modelsToDelete.Any(x => x.Name.Insert(0, "!") == modelName)) continue; // Check if model is already set to be deleted

                    BuildNode(node);
                    ModelSettingsBase modelSettings = (ModelSettingsBase)node.Tag;

                    if (!newModels.ContainsKey(modelName))
                    {
                        modelsToDelete.Add(modelSettings);
                        nodesToRemove.Add(node);
                        continue;
                    }

                    existingModels.Add(modelName, new SortedDictionary<string, IEditCycleParameter>());

                    objectSettings = node.Tag;
                    object modelSettingsToCopy = defaultNode.Tag as ModelSettingsBase;

                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                            IEditCycleParameter editCycleParameterToCopy = property.GetValue(modelSettingsToCopy, null) as IEditCycleParameter;
                            if (editCycleParameterToCopy.Visible)
                            {
                                existingModels[modelName].Add(editCycleParameter.DisplayName, editCycleParameter);
                            }

                        }
                    }
                }
            }

            #endregion

            #region check if file parameter names match existing
            for (int i = 0; i < 6; i++)
            {
                if (existingParams[i].Count != newParams[i].Count || !existingParams[i].Keys.SequenceEqual(newParams[i].Keys))
                {
                    MessageBox.Show("Import failed. Parameters from file do not match existing parameters.");
                    return;
                }
            }
            foreach (KeyValuePair<string, SortedDictionary<string, string>> model in newModels)
            {
                if (model.Value.Count != existingParams[5].Count || !model.Value.Keys.SequenceEqual(existingParams[5].Keys))
                {
                    MessageBox.Show("Import failed. Parameters from file do not match existing parameters.");
                    return;
                }
            }
            #endregion

            #region overwrite config values
            for (int i = 0; i < 6; i++)
            {
                foreach (KeyValuePair<string, KeyValuePair<string, string>> param in newParams[i])
                {
                    try
                    { // This will need to be updated if new parameter type is added
                        if (existingParams[i][param.Key].GetType().Name == "SerialPortParameter")
                        {
                            Match match = Regex.Match(param.Value.Key, @"([\w\d]+)-(\d+)-(\w+)-(\d+)");
                            ((SerialPortParameter)existingParams[i][param.Key]).ProcessValue.PortName = match.Groups[1].ToString();
                            ((SerialPortParameter)existingParams[i][param.Key]).ProcessValue.BaudRate = Int32.Parse(match.Groups[2].ToString());
                            ((SerialPortParameter)existingParams[i][param.Key]).ProcessValue.Parity = (Parity)Enum.Parse(typeof(Parity), match.Groups[3].ToString());
                            ((SerialPortParameter)existingParams[i][param.Key]).ProcessValue.DataBits = Int32.Parse(match.Groups[4].ToString());

                        }
                        else if (existingParams[i][param.Key].GetType().Name == "EthernetPortParameter")
                        {
                            Match match = Regex.Match(param.Value.Key, @"(.+)-([\d\.]+):(\d+)");
                            ((EthernetPortParameter)existingParams[i][param.Key]).ProcessValue.PortName = match.Groups[1].ToString();
                            ((EthernetPortParameter)existingParams[i][param.Key]).ProcessValue.IPAddress = match.Groups[2].ToString();
                            ((EthernetPortParameter)existingParams[i][param.Key]).ProcessValue.Port = match.Groups[3].ToString();
                        }
                        else
                        {
                            existingParams[i][param.Key].NewValueString = param.Value.Key;
                            existingParams[i][param.Key].ToolTip = param.Value.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }


                }
            }
            foreach (KeyValuePair<string, SortedDictionary<string, string>> model in newModels)
            {
                //If model already exists, update process values of its parameters
                if (existingModels.ContainsKey(model.Key))
                {
                    foreach (KeyValuePair<string, string> param in model.Value)
                    {
                        if (existingModels[model.Key].ContainsKey(param.Key) && existingModels[model.Key][param.Key].ProcessValueString != param.Value)
                        {
                            //existingModels[model.Key][param.Key].ProcessValueString = param.Value;
                            existingModels[model.Key][param.Key].NewValueString = param.Value;
                            existingModels[model.Key][param.Key].Updated = true;

                        }
                    }
                }
                else
                {

                    ModelSettingsBase modelSettings, modelSettingsToCopy;
                    TreeNode treeNode, treeNode1;
                    modelSettings = Activator.CreateInstance(VtiLib.ModelSettingsType) as ModelSettingsBase;
                    modelSettings.Name = "!" + model.Key;

                    // Create the tree node for the current settings object
                    treeNode1 = new TreeNode();
                    treeNode1.Name = "Model " + model.Key;
                    treeNode1.Text = "Model " + model.Key + " Parameters";
                    treeNode1.Tag = modelSettings;  // save the ModelSettings in the tag of the TreeNode
                                                    // insert the new node in alphabetical order

                    treeNode = null;
                    //foreach(TreeNode node in treeView1.Nodes.Find("Model DEFAULT Parameters", true)) {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if (node.Text == "Model DEFAULT Parameters")
                        {
                            treeNode = node;
                            break;
                        }
                    }
                    modelSettingsToCopy = treeNode.Tag as ModelSettingsBase;

                    treeView1.Nodes.Add(treeNode1);
                    // Get the properties of the current settings object
                    foreach (PropertyInfo property in modelSettings.GetType().GetProperties())
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameter = property.GetValue(modelSettings, null) as IEditCycleParameter;
                            IEditCycleParameter editCycleParameterToCopy = property.GetValue(modelSettingsToCopy, null) as IEditCycleParameter;
                            if (editCycleParameterToCopy.Visible)
                            {
                                editCycleParameter.NewValueString = model.Value[editCycleParameter.DisplayName];
                                editCycleParameter.ProcessValueString = model.Value[editCycleParameter.DisplayName];
                                TreeNode newNode = editCycleParameter.CreateTreeNode(property, editCycleParameter, this);
                                object objectSettings = property;
                                if (objectSettings != null)
                                    newNode.Tag = objectSettings; // save objectSettings in the tag of the new node
                                treeNode1.Nodes.Add(newNode);
                                editCycleParameter.Updated = true;
                            }
                            else
                            {
                                editCycleParameter.NewValueString = editCycleParameterToCopy.NewValueString;
                                editCycleParameter.ProcessValueString = editCycleParameterToCopy.ProcessValueString;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < nodesToRemove.Count; i++)
            {
                nodesToRemove[i].Remove();
            }
            SaveConfig(true);
            #endregion

            MessageBox.Show("Parameters imported successfully", "Import Parameters");
        }

        private void addParamToList(TreeNode node, SortedDictionary<string, IEditCycleParameter> existingParams)
        {
            object objectSettings = node.Tag;

            foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
            {
                if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                {
                    IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                    //editCycleParameter.GetType().Name;
                    if (editCycleParameter.Visible)
                    {
                        existingParams.Add(editCycleParameter.DisplayName, editCycleParameter);
                    }

                }
            }
        }

    }
}