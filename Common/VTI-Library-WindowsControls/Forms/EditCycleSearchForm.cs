using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.Configuration.Interfaces;
using VTIWindowsControlLibrary.Data;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Displays a window similar to a <see cref="MessageBox">MessageBox</see>
    /// with a <see cref="TextBox">TextBox</see> for the user to enter a value
    /// </summary>
    public partial class EditCycleSearchForm : Form
    {
        private string strReturnValue;
        private Point pntStartLocation;
        private int nSelectedSearchItem;
        private EditCycleForm editCycleForm;
        private static List<EditCycleSearchParameter> listEditCycleParameters = new List<EditCycleSearchParameter>();
        private VtiDataContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCycleSearchForm">EditCycleSearchForm</see>
        /// </summary>
        public EditCycleSearchForm(EditCycleForm ecf)
        {
            InitializeComponent();
            strReturnValue = "";
            editCycleForm = ecf;
            nSelectedSearchItem = -1; // default selected search item
                                      // initialize Filter By Sequence dropdown list
            comboBoxFilterBySeq.Items.Clear();
            Dictionary<int, string> dictionaryList = new Dictionary<int, string>();
            int ndx = 0;
            dictionaryList.Add(ndx++, "All");
            if (EditCycle.editCycle.sequenceStepList != null)
            {
                if (EditCycle.editCycle.sequenceStepList[0] != null)
                {
                    foreach (SequenceStep seqStep in EditCycle.editCycle.sequenceStepList[0])
                    {
                        dictionaryList.Add(ndx++, seqStep.Text);
                    }
                }
            }
            comboBoxFilterBySeq.DataSource = new BindingSource(dictionaryList, null);
            comboBoxFilterBySeq.DisplayMember = "Value";
            comboBoxFilterBySeq.ValueMember = "Key";
            db = EditCycle.editCycle.db;
            // expand all model nodes; required so can iterate through child nodes
            foreach (TreeNode node in EditCycle.editCycle.treeView1.Nodes)
            { // Node is Top-Level and has child nodes
                if (node.Level == 0 && node.Nodes.Count > 0)
                {
                    if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                    {
                    }
                    else
                    { // Model settings in the database
                        if (!node.IsExpanded)
                            node.Expand();
                        editCycleForm.modelNameList.Add((string)node.Text.Replace("Model ", "").Replace(" Parameters", ""));
                    }
                }
            }
            // initialize Operating Sequences dropdown list
            comboBoxFilterByOpSeq.Items.Clear();
            Dictionary<int, string> dictionaryList2 = new Dictionary<int, string>();
            ndx = 0;
            dictionaryList2.Add(ndx++, "All");
            if (EditCycle.editCycle.operatingSeqList != null)
            {
                foreach (string s in EditCycle.editCycle.operatingSeqList)
                {
                    if (s.Length > 0)
                        dictionaryList2.Add(ndx++, s);
                }
            }
            comboBoxFilterByOpSeq.DataSource = new BindingSource(dictionaryList2, null);
            comboBoxFilterByOpSeq.DisplayMember = "Value";
            comboBoxFilterByOpSeq.ValueMember = "Key";
        }

        private void frmInputBox_Load(object sender, EventArgs e)
        {
            if (!this.pntStartLocation.IsEmpty)
            {
                this.Top = this.pntStartLocation.Y;
                this.Left = this.pntStartLocation.X;
            }
            this.txtSearchFor.Select();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DisplaySearchResults();
        }

        /// <summary>
        /// Displays the Search Results
        /// </summary>
        public void DisplaySearchResults()
        {
            bool bIsMatch, bIsAtLeastOne, bIsFirstTime = true;
            string strLSearchFor = txtSearchFor.Text.ToLower(), strHeader = "";
            listEditCycleParameters.Clear();
            // Iterate through the tree nodes
            listView1.Items.Clear();
            ListViewItem row;
            List<string> listDesc = new List<string>();
            Dictionary<string, string> defaultModelListSeq = new Dictionary<string, string>();
            Dictionary<string, string> defaultModelListOpSeq = new Dictionary<string, string>();
            int ndx = 0;
            bool bIsEmptyModelList = true;
            if (editCycleForm.modelNameList.Count > 0)
                bIsEmptyModelList = false;
            foreach (TreeNode node in EditCycle.editCycle.treeView1.Nodes)
            { // Node is Top-Level and has child nodes
                if (node.Level == 0 && node.Nodes.Count > 0)
                {
                    object objectSettings = node.Tag;
                    bIsAtLeastOne = false;
                    // Settings in the user.config file
                    if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                    {
                        // write the header for the data
                        row = new ListViewItem(node.Text);
                        strHeader = node.Text;
                        try
                        {
                            foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                            {
                                bIsMatch = false;
                                if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                {
                                    IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                    if (!editCycleParameter.Visible) // parameter is invisible so exclude from search results
                                        continue;
                                    if (checkBoxName.Checked && editCycleParameter.DisplayName.ToString().ToLower().Contains(strLSearchFor))
                                        bIsMatch = true;
                                    if (checkBoxValue.Checked && editCycleParameter.ProcessValueString.ToString().ToLower().Contains(strLSearchFor))
                                        bIsMatch = true;
                                    if (checkBoxToolTip.Checked && editCycleParameter.ToolTip.ToString().ToLower().Contains(strLSearchFor))
                                        bIsMatch = true;
                                    if (checkBoxSequenceStep.Checked)
                                    {
                                        if (editCycleParameter.SequenceStep != null)
                                        {
                                            if (editCycleParameter.SequenceStep.ToString().ToLower().Contains(strLSearchFor))
                                            {
                                                bIsMatch = true;
                                                if (bIsFirstTime && node.Text.Equals("Model DEFAULT Parameters"))
                                                {
                                                    defaultModelListSeq.Add(editCycleParameter.DisplayName.ToString().ToLower().Replace(" ", ""),
                                                      editCycleParameter.SequenceStep.ToString().ToLower());
                                                }
                                            }
                                        }
                                        else if (comboBoxFilterBySeq.SelectedItem.ToString().ToLower().Contains("all") && strLSearchFor.Length == 0)
                                        {
                                            bIsMatch = true;
                                        }
                                    }
                                    if (editCycleParameter.OperatingSequence != null)
                                    {
                                        if (bIsFirstTime && node.Text.Equals("Model DEFAULT Parameters"))
                                        {
                                            defaultModelListOpSeq.Add(editCycleParameter.DisplayName.ToString().ToLower().Replace(" ", ""),
                                              editCycleParameter.OperatingSequence.ToString().ToLower());
                                        }
                                    }
                                    if (checkBoxName.Checked && editCycleParameter.ProcessValueString.Contains("SerialPortSettings"))
                                    {
                                        if (((SerialPortParameter)editCycleParameter).ProcessValue.PortName.ToString().ToLower().Contains(strLSearchFor))
                                            bIsMatch = true;
                                        if (((SerialPortParameter)editCycleParameter).ProcessValue.BaudRate.ToString().ToLower().Contains(strLSearchFor))
                                            bIsMatch = true;
                                        if (((SerialPortParameter)editCycleParameter).ProcessValue.Parity.ToString().ToLower().Contains(strLSearchFor))
                                            bIsMatch = true;
                                        if (((SerialPortParameter)editCycleParameter).ProcessValue.DataBits.ToString().ToLower().Contains(strLSearchFor))
                                            bIsMatch = true;
                                        if (((SerialPortParameter)editCycleParameter).ProcessValue.StopBits.ToString().ToLower().Contains(strLSearchFor))
                                            bIsMatch = true;
                                        if (((SerialPortParameter)editCycleParameter).ProcessValue.Handshake.ToString().ToLower().Contains(strLSearchFor))
                                            bIsMatch = true;
                                    }
                                    // Filter By Sequence
                                    bool bIsAtLeastOneMatch = false;
                                    if (!comboBoxFilterBySeq.SelectedItem.ToString().ToLower().Contains("all"))
                                    {
                                        if (editCycleParameter.SequenceStep != null && bIsMatch)
                                        {
                                            string[] strParse = editCycleParameter.SequenceStep.ToLower().Split(',').Select(sValue => sValue.Trim()).ToArray();
                                            foreach (string s in strParse)
                                            {
                                                if (s.Contains(comboBoxFilterBySeq.Text.ToLower()) || comboBoxFilterBySeq.Text.ToLower().Contains(s))
                                                {
                                                    bIsAtLeastOneMatch = true;
                                                    break;
                                                }
                                            }
                                        }
                                        bIsMatch = bIsAtLeastOneMatch;
                                    }
                                    // Filter by Operating Sequence
                                    if (!comboBoxFilterByOpSeq.SelectedItem.ToString().ToLower().Contains("all"))
                                    {
                                        bIsAtLeastOneMatch = false;
                                        if (editCycleParameter.OperatingSequence != null && bIsMatch)
                                        {
                                            string[] strParse = editCycleParameter.OperatingSequence.ToLower().Split(',').Select(sValue => sValue.Trim()).ToArray();
                                            foreach (string s in strParse)
                                            {
                                                if (s.Equals(comboBoxFilterByOpSeq.Text.ToLower()))
                                                {
                                                    bIsAtLeastOneMatch = true;
                                                    break;
                                                }
                                            }
                                        }
                                        bIsMatch = bIsAtLeastOneMatch;
                                    }
                                    if (bIsMatch == true)
                                    { // check for Blue and White sides checkboxes
                                        if (checkBoxBlue.Checked && !checkBoxWhite.Checked)
                                        {
                                            if (editCycleParameter.DisplayName.ToString().ToLower().Contains("white"))
                                                bIsMatch = false;
                                        }
                                        else if (!checkBoxBlue.Checked && checkBoxWhite.Checked)
                                        {
                                            if (editCycleParameter.DisplayName.ToString().ToLower().Contains("blue"))
                                                bIsMatch = false;
                                        }
                                        else if (!checkBoxBlue.Checked && !checkBoxWhite.Checked)
                                        {
                                            if (editCycleParameter.DisplayName.ToString().ToLower().Contains("blue") || editCycleParameter.DisplayName.ToString().ToLower().Contains("white"))
                                                bIsMatch = false;
                                        }
                                    }
                                    if (bIsMatch)
                                    {
                                        if (!bIsAtLeastOne)
                                        {
                                            listView1.Items.Add(row);
                                            bIsAtLeastOne = true;
                                            EditCycleSearchParameter ecp2 = new EditCycleSearchParameter();
                                            ecp2.Group = strHeader;
                                            ecp2.Name = "";
                                            ecp2.ToolTip = "";
                                            ecp2.Value = "";
                                            if (strHeader.StartsWith("Common Control"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Control);
                                            else if (strHeader.StartsWith("Common Time"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Time);
                                            else if (strHeader.StartsWith("Common Flow"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Flow);
                                            else if (strHeader.StartsWith("Common Pressure"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Pressure);
                                            else if (strHeader.StartsWith("Common Mode"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Mode);
                                            listEditCycleParameters.Add(ecp2);
                                        }
                                        EditCycleSearchParameter ecp = new EditCycleSearchParameter();
                                        row = new ListViewItem(editCycleParameter.DisplayName);
                                        ecp.Group = strHeader;
                                        ecp.Name = editCycleParameter.DisplayName;
                                        if (editCycleParameter.ProcessValueString.ToLower().Contains("vtiwindowscontrollibrary"))
                                        {
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ""));
                                            ecp.Value = "";
                                        }
                                        else
                                        {
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, editCycleParameter.ProcessValueString));
                                            ecp.Value = editCycleParameter.ProcessValueString;
                                        }
                                        row.SubItems.Add(new ListViewItem.ListViewSubItem(row, editCycleParameter.ToolTip));
                                        ecp.ToolTip = editCycleParameter.ToolTip;
                                        if (editCycleParameter.SequenceStep != null)
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, editCycleParameter.SequenceStep));
                                        else
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ""));
                                        if (editCycleParameter.OperatingSequence != null)
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, editCycleParameter.OperatingSequence));
                                        else
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ""));
                                        listView1.Items.Add(row);

                                        if (property.PropertyType.Name == "StringParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Control);
                                        else if (property.PropertyType.Name == "TimeDelayParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Time);
                                        else if (property.PropertyType.Name == "NumericParameter")
                                        {
                                            NumericParameter numericParameter = editCycleParameter as NumericParameter;
                                            string strUnits = numericParameter.Units.ToLower();
                                            if (strUnits.Contains("cc/s") || strUnits.Contains("cc/min") || strUnits.Contains("cc/h") || strUnits.Contains("oz/yr") || strUnits.Contains("torr-l/s") || strUnits.Contains("g/y") || strUnits.Contains("mbar-l/") || strUnits.Contains("pa-m3/"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Flow);
                                            else if (strUnits.Equals("torr") || strUnits.Equals("mtorr") || strUnits.Equals("atm") || strUnits.Equals("bar") || strUnits.Equals("mbar") || strUnits.Contains("kgf/cm2") || strUnits.Equals("kpa") || strUnits.Equals("mpa") || strUnits.Equals("psi") || strUnits.Equals("psig") || strUnits.Equals("psia"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Pressure);
                                        }
                                        else if (property.PropertyType.Name == "SerialPortParameter" || property.PropertyType.Name == "EthernetPortParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Control);
                                        else if (property.PropertyType.Name == "BooleanParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Mode);
                                        else if (property.PropertyType.Name == "EnumParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Control);

                                        listEditCycleParameters.Add(ecp);
                                    }
                                    if (editCycleParameter.ProcessValueString.Contains("SerialPortSettings"))
                                    {
                                        if (bIsMatch)
                                        {
                                            // PortName
                                            row = new ListViewItem("PortName");
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ((SerialPortParameter)editCycleParameter).ProcessValue.PortName.ToString()));
                                            listView1.Items.Add(row);
                                            EditCycleSearchParameter ecp2 = new EditCycleSearchParameter();
                                            ecp2.Group = strHeader;
                                            ecp2.Name = "PortName";
                                            ecp2.Value = ((SerialPortParameter)editCycleParameter).ProcessValue.PortName.ToString();
                                            ecp2.ToolTip = "";
                                            listEditCycleParameters.Add(ecp2);
                                            // BaudRate
                                            row = new ListViewItem("BaudRate");
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ((SerialPortParameter)editCycleParameter).ProcessValue.BaudRate.ToString()));
                                            listView1.Items.Add(row);
                                            ecp2 = new EditCycleSearchParameter();
                                            ecp2.Group = strHeader;
                                            ecp2.Name = "BaudRate";
                                            ecp2.Value = ((SerialPortParameter)editCycleParameter).ProcessValue.BaudRate.ToString();
                                            ecp2.ToolTip = "";
                                            listEditCycleParameters.Add(ecp2);
                                            // Parity
                                            row = new ListViewItem("Parity");
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ((SerialPortParameter)editCycleParameter).ProcessValue.Parity.ToString()));
                                            listView1.Items.Add(row);
                                            ecp2 = new EditCycleSearchParameter();
                                            ecp2.Group = strHeader;
                                            ecp2.Name = "Parity";
                                            ecp2.Value = ((SerialPortParameter)editCycleParameter).ProcessValue.Parity.ToString();
                                            ecp2.ToolTip = "";
                                            listEditCycleParameters.Add(ecp2);
                                            // DataBits
                                            row = new ListViewItem("DataBits");
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ((SerialPortParameter)editCycleParameter).ProcessValue.DataBits.ToString()));
                                            listView1.Items.Add(row);
                                            ecp2 = new EditCycleSearchParameter();
                                            ecp2.Group = strHeader;
                                            ecp2.Name = "DataBits";
                                            ecp2.Value = ((SerialPortParameter)editCycleParameter).ProcessValue.DataBits.ToString();
                                            ecp2.ToolTip = "";
                                            listEditCycleParameters.Add(ecp2);
                                            // StopBits
                                            row = new ListViewItem("StopBits");
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ((SerialPortParameter)editCycleParameter).ProcessValue.StopBits.ToString()));
                                            listView1.Items.Add(row);
                                            ecp2 = new EditCycleSearchParameter();
                                            ecp2.Group = strHeader;
                                            ecp2.Name = "StopBits";
                                            ecp2.Value = ((SerialPortParameter)editCycleParameter).ProcessValue.StopBits.ToString();
                                            ecp2.ToolTip = "";
                                            listEditCycleParameters.Add(ecp2);
                                            // Handshake
                                            row = new ListViewItem("Handshake");
                                            row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ((SerialPortParameter)editCycleParameter).ProcessValue.Handshake.ToString()));
                                            listView1.Items.Add(row);
                                            ecp2 = new EditCycleSearchParameter();
                                            ecp2.Group = strHeader;
                                            ecp2.Name = "Handshake";
                                            ecp2.Value = ((SerialPortParameter)editCycleParameter).ProcessValue.Handshake.ToString();
                                            ecp2.ToolTip = "";
                                            listEditCycleParameters.Add(ecp2);
                                        }
                                    }
                                    if (node.Text.Equals("Model DEFAULT Parameters"))
                                    {
                                        listDesc.Add(editCycleParameter.ToolTip.ToString());
                                    }
                                }
                            }
                            if (node.Text.Equals("Model DEFAULT Parameters"))
                                bIsFirstTime = false;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    else
                    { // Model settings in the database
                        row = new ListViewItem(node.Text);
                        strHeader = node.Text;
                        string modelName;
                        modelName = objectSettings.ToString();
                        if (modelName.Contains("Classes.Configuration.ModelSettings"))
                            modelName = editCycleForm.modelNameList[ndx];
                        ndx++;
                        if (bIsEmptyModelList)
                            editCycleForm.modelNameList.Add(modelName);
                        try
                        {
                            int ndx2 = 0;
                            foreach (ModelParameter param in db.Models.SingleOrDefault(m => m.ModelNo == modelName).ModelParameters)
                            {
                                bIsMatch = false;
                                if (checkBoxName.Checked && param.ParameterID.ToLower().Contains(strLSearchFor))
                                    bIsMatch = true;
                                if (checkBoxValue.Checked && param.ProcessValue.ToLower().Contains(strLSearchFor))
                                    bIsMatch = true;
                                if (checkBoxToolTip.Checked && listDesc[ndx2].ToLower().Contains(strLSearchFor))
                                    bIsMatch = true;

                                // Filter By SequenceStep
                                bool bIsAtLeastOneMatch = false;
                                if (!comboBoxFilterBySeq.SelectedItem.ToString().ToLower().Contains("all"))
                                {
                                    try
                                    {
                                        string s = defaultModelListSeq[param.ParameterID.ToLower()];
                                        if (s.Contains(comboBoxFilterBySeq.Text.ToLower()) || comboBoxFilterBySeq.Text.ToLower().Contains(s))
                                        {
                                            bIsAtLeastOneMatch = true;
                                        }
                                    }
                                    catch (Exception e2)
                                    {
                                    }
                                    bIsMatch = bIsAtLeastOneMatch;
                                }
                                // Filter by Operating Sequence
                                if (!comboBoxFilterByOpSeq.SelectedItem.ToString().ToLower().Contains("all"))
                                {
                                    try
                                    {
                                        bIsAtLeastOneMatch = false;
                                        string s = defaultModelListOpSeq[param.ParameterID.ToLower()];
                                        string[] strParse = s.Split(',').ToArray();
                                        foreach (string s2 in strParse)
                                        {
                                            if (s2.Equals(comboBoxFilterByOpSeq.Text.ToLower()))
                                            {
                                                bIsAtLeastOneMatch = true;
                                                break;
                                            }
                                        }
                                        bIsMatch = bIsAtLeastOneMatch;
                                    }
                                    catch (Exception e2)
                                    {
                                    }
                                    bIsMatch = bIsAtLeastOneMatch;
                                }
                                if (bIsMatch == true)
                                { // check for Blue and White sides checkboxes
                                    if (checkBoxBlue.Checked && !checkBoxWhite.Checked)
                                    {
                                        if (listDesc[ndx2].ToLower().Contains("white"))
                                            bIsMatch = false;
                                    }
                                    else if (!checkBoxBlue.Checked && checkBoxWhite.Checked)
                                    {
                                        if (listDesc[ndx2].ToLower().Contains("blue"))
                                            bIsMatch = false;
                                    }
                                    else if (!checkBoxBlue.Checked && !checkBoxWhite.Checked)
                                    {
                                        if (listDesc[ndx2].ToLower().Contains("blue") || listDesc[ndx2].ToLower().Contains("white"))
                                            bIsMatch = false;
                                    }
                                }

                                if (bIsMatch)
                                {
                                    if (!bIsAtLeastOne)
                                    {
                                        listView1.Items.Add(row);
                                        bIsAtLeastOne = true;
                                        EditCycleSearchParameter ecp2 = new EditCycleSearchParameter();
                                        ecp2.Group = strHeader;
                                        ecp2.Name = "";
                                        ecp2.ToolTip = "";
                                        ecp2.Value = "";
                                        listEditCycleParameters.Add(ecp2);
                                    }
                                    row = new ListViewItem(param.ParameterID);
                                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, param.ProcessValue));
                                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, listDesc[ndx2]));
                                    // assign Sequence Step
                                    try
                                    {
                                        row.SubItems.Add(new ListViewItem.ListViewSubItem(row, defaultModelListSeq[param.ParameterID.ToLower()]));
                                    }
                                    catch (Exception e4)
                                    {
                                        row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ""));
                                    }
                                    // assign Operating Sequence
                                    try
                                    {
                                        row.SubItems.Add(new ListViewItem.ListViewSubItem(row, defaultModelListOpSeq[param.ParameterID.ToLower()].ToUpper()));
                                    }
                                    catch (Exception e4)
                                    {
                                        row.SubItems.Add(new ListViewItem.ListViewSubItem(row, ""));
                                    }
                                    IEditCycleParameter iecp = null;
                                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                                    {
                                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                        {
                                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                            if (!editCycleParameter.Visible) // parameter is invisible so exclude from search results
                                                continue;
                                            if (editCycleParameter.DisplayName.ToLower().Replace(" ", "").Replace("-", "") == param.ParameterID.ToLower().Replace(" ", "").Replace("-", ""))
                                            {
                                                iecp = editCycleParameter;
                                                break;
                                            }
                                        }
                                    }
                                    // assign a background color to each node
                                    if (iecp != null)
                                    {
                                        if (iecp.GetType().Name == "StringParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Control);
                                        else if (iecp.GetType().Name == "TimeDelayParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Time);
                                        else if (iecp.GetType().Name == "NumericParameter")
                                        {
                                            NumericParameter numericParameter = (IEditCycleParameter)param as NumericParameter;
                                            string strUnits = numericParameter.Units.ToLower();
                                            if (strUnits.Contains("cc/s") || strUnits.Contains("cc/min") || strUnits.Contains("cc/h") || strUnits.Contains("oz/yr") || strUnits.Contains("torr-l/s") || strUnits.Contains("g/y") || strUnits.Contains("mbar-l/") || strUnits.Contains("pa-m3/"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Flow);
                                            else if (strUnits.Equals("torr") || strUnits.Equals("mtorr") || strUnits.Equals("atm") || strUnits.Equals("bar") || strUnits.Equals("mbar") || strUnits.Contains("kgf/cm2") || strUnits.Equals("kpa") || strUnits.Equals("mpa") || strUnits.Equals("psi") || strUnits.Equals("psig") || strUnits.Equals("psia"))
                                                row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Pressure);
                                        }
                                        else if (iecp.GetType().Name == "SerialPortParameter" || iecp.GetType().Name == "EthernetPortParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Control);
                                        else if (iecp.GetType().Name == "BooleanParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Mode);
                                        else if (iecp.GetType().Name == "EnumParameter")
                                            row.BackColor = editCycleForm.EditCycleColor(EditCycleForm.EditCycleType.Control);
                                    }
                                    listView1.Items.Add(row);
                                    EditCycleSearchParameter ecp3 = new EditCycleSearchParameter();
                                    ecp3.Group = strHeader;
                                    ecp3.Name = param.ParameterID;
                                    ecp3.Value = param.ProcessValue;
                                    ecp3.ToolTip = listDesc[ndx2];
                                    listEditCycleParameters.Add(ecp3);
                                }
                                ndx2++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    if (bIsAtLeastOne)
                    {
                        row = new ListViewItem(""); // insert a blank line into the list
                        listView1.Items.Add(row);
                        EditCycleSearchParameter ecp = new EditCycleSearchParameter();
                        ecp.Group = "";
                        ecp.Name = "";
                        ecp.Value = "";
                        ecp.ToolTip = "";
                        listEditCycleParameters.Add(ecp);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            editCycleForm.bIsOpenEditCycleSearchForm = false;
            this.Close();
        }

        /// <summary>
        /// Title to display on then input box
        /// </summary>
        public string Title
        {
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// Prompt to display in the input box
        /// </summary>
        public string Prompt
        {
            set
            {
                this.lblText.Text = value;
            }
        }

        /// <summary>
        /// Value entered by the user
        /// </summary>
        /// <remarks>
        /// If the user clicks the Cancel button, the value will be an empty string.
        /// </remarks>
        public string ReturnValue
        {
            get
            {
                return strReturnValue;
            }
        }

        /// <summary>
        /// Optional default response for the input box
        /// </summary>
        public string DefaultResponse
        {
            set
            {
                this.txtSearchFor.Text = value;
                this.txtSearchFor.SelectAll();
            }
        }

        /// <summary>
        /// Optional starting location for the input box window
        /// </summary>
        public Point StartLocation
        {
            set
            {
                this.pntStartLocation = value;
            }
        }

        public class EditCycleSearchParameter
        {
            public string Group, Name, Value, ToolTip;
        }

        // Implement the manual sorting of items by column
        private class ListViewItemComparer : IComparer
        {
            private int col;

            public ListViewItemComparer()
            {
                col = 0;
            }

            public ListViewItemComparer(int column)
            {
                col = column;
            }

            public int Compare(object x, object y)
            {
                int returnVal = -1;
                try
                {
                    if (((ListViewItem)x).SubItems[col] != null && ((ListViewItem)y).SubItems[col] != null)
                        returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                    if (returnVal > 0)
                    {
                        EditCycleSearchParameter ecpTemp = listEditCycleParameters[((ListViewItem)x).Index];
                        listEditCycleParameters[((ListViewItem)x).Index] = listEditCycleParameters[((ListViewItem)y).Index];
                        listEditCycleParameters[((ListViewItem)y).Index] = ecpTemp;
                    }
                }
                catch (Exception e)
                {
                }
                return returnVal;
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
            //listView1.Sort();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSel = listView1.FocusedItem.Index;
            if (nSel != nSelectedSearchItem)
            {
                nSelectedSearchItem = nSel;
                // determine which Edit Cycle parameter this is
                EditCycleSearchParameter ecp = listEditCycleParameters[nSel];
                editCycleForm.SelectFromSearchForm(ecp);
            }
        }

        private void buttonCopyBlueToWhite_Click(object sender, EventArgs e)
        {
            CopyAllToOppositePort(CopyEditCycleMode.FromBlueToWhiteConfig);
        }

        private void CopyAllToOppositePort(CopyEditCycleMode cecm)
        {
            DialogResult dr;
            if (cecm == CopyEditCycleMode.FromBlueToWhiteConfig)
                dr = MessageBox.Show(this.MdiParent, "Are you sure you want to copy all Blue port settings to the White port.  All White port parameters will be overwritten.", "Confirm Copy Blue to White", MessageBoxButtons.YesNo);
            else
                dr = MessageBox.Show(this.MdiParent, "Are you sure you want to copy all White port settings to the Blue port.  All Blue port parameters will be overwritten.", "Confirm Copy White to Blue", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes) return;
            int ndx = 0;
            bool bIsEmptyModelList = true;
            if (editCycleForm.modelNameList.Count > 0)
                bIsEmptyModelList = false;
            foreach (TreeNode node in EditCycle.editCycle.treeView1.Nodes)
            { // Node is Top-Level and has child nodes
                if (node.Level == 0 && node.Nodes.Count > 0)
                {
                    object objectSettings = node.Tag;
                    // Settings in the user.config file
                    if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                    {
                        try
                        {
                            foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                            {
                                if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                {
                                    IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                    if (cecm == CopyEditCycleMode.FromBlueToWhiteConfig)
                                    { // copy from blue to white
                                        if (editCycleParameter.DisplayName.ToString().ToLower().Contains("blue"))
                                            CopyToOppositePort(CopyEditCycleMode.FromBlueToWhiteConfig, editCycleParameter, null);
                                    }
                                    else
                                    { // copy from white to blue
                                        if (editCycleParameter.DisplayName.ToString().ToLower().Contains("white"))
                                            CopyToOppositePort(CopyEditCycleMode.FromWhiteToBlueConfig, editCycleParameter, null);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    else
                    { // Model settings in the database
                        string modelName;
                        modelName = objectSettings.ToString();
                        if (modelName.Contains("Classes.Configuration.ModelSettings"))
                            modelName = editCycleForm.modelNameList[ndx];
                        ndx++;
                        if (bIsEmptyModelList)
                            editCycleForm.modelNameList.Add(modelName);
                        try
                        {
                            foreach (ModelParameter param in db.Models.SingleOrDefault(m => m.ModelNo == modelName).ModelParameters)
                            {
                                if (cecm == CopyEditCycleMode.FromBlueToWhiteConfig)
                                { // copy from blue to white
                                    if (param.ParameterID.ToLower().Contains("blue"))
                                        CopyToOppositePort(CopyEditCycleMode.FromBlueToWhiteModel, null, param);
                                }
                                else
                                { // copy from white to blue
                                    if (param.ParameterID.ToLower().Contains("white"))
                                        CopyToOppositePort(CopyEditCycleMode.FromWhiteToBlueModel, null, param);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
            // save configuration
            editCycleForm.SaveConfig(false);
            // redisplay the search results
            DisplaySearchResults();
        }

        private void CopyToOppositePort(CopyEditCycleMode cecm, IEditCycleParameter iecp, ModelParameter mp) // string strParam)
        {
            string strParamOpposite = "";
            if (iecp != null)
            {
                if (iecp.ProcessValueString.Contains("SerialPortSettings"))
                    return; // do not copy serial port config to opposite side
                if (cecm == CopyEditCycleMode.FromBlueToWhiteConfig)
                    strParamOpposite = iecp.DisplayName.ToLower().Replace("blue", "white");
                else
                    strParamOpposite = iecp.DisplayName.ToLower().Replace("white", "blue");
            }
            else
            { // copy model config
                if (cecm == CopyEditCycleMode.FromBlueToWhiteModel)
                    strParamOpposite = mp.ParameterID.ToLower().Replace("blue", "white");
                else
                    strParamOpposite = mp.ParameterID.ToLower().Replace("white", "blue");
            }
            int ndx = 0;
            bool bIsEmptyModelList = true;
            if (editCycleForm.modelNameList.Count > 0)
                bIsEmptyModelList = false;
            foreach (TreeNode node in EditCycle.editCycle.treeView1.Nodes)
            { // Node is Top-Level and has child nodes
                if (node.Level == 0 && node.Nodes.Count > 0)
                {
                    object objectSettings = node.Tag;
                    // Settings in the user.config file
                    if (node.Text.StartsWith("Common ") || node.Text.Equals("Model DEFAULT Parameters"))
                    {
                        if (cecm == CopyEditCycleMode.FromBlueToWhiteConfig || cecm == CopyEditCycleMode.FromWhiteToBlueConfig)
                        {
                            try
                            {
                                foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                                {
                                    if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                                    {
                                        IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                                        if (editCycleParameter.DisplayName.ToString().ToLower().Equals(strParamOpposite))
                                        {
                                            editCycleParameter.ProcessValueString = iecp.ProcessValueString;
                                            break;
                                        }
                                        //if (editCycleParameter.ProcessValueString.Contains("SerialPortSettings")) {
                                        //}
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    { // Model settings in the database
                        string modelName;
                        modelName = objectSettings.ToString();
                        if (modelName.Contains("Classes.Configuration.ModelSettings"))
                            modelName = editCycleForm.modelNameList[ndx];
                        ndx++;
                        if (bIsEmptyModelList)
                            editCycleForm.modelNameList.Add(modelName);
                        if (cecm == CopyEditCycleMode.FromBlueToWhiteModel || cecm == CopyEditCycleMode.FromWhiteToBlueModel)
                        {
                            try
                            {
                                foreach (ModelParameter param in db.Models.SingleOrDefault(m => m.ModelNo == modelName).ModelParameters)
                                {
                                    if (param.ParameterID.ToLower().Equals(strParamOpposite))
                                    {
                                        param.ProcessValue = mp.ProcessValue;
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void buttonCopyWhiteToBlue_Click(object sender, EventArgs e)
        {
            CopyAllToOppositePort(CopyEditCycleMode.FromWhiteToBlueConfig);
        }
    }

    internal enum CopyEditCycleMode
    {
        None,
        FromBlueToWhiteConfig,
        FromWhiteToBlueConfig,
        FromBlueToWhiteModel,
        FromWhiteToBlueModel
    }
}