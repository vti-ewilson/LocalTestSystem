using System;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

//using VTIWindowsControlLibrary.Classes.IO.SerialIO;
using VTIWindowsControlLibrary.Classes.IO.EthernetIO;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components.Configuration;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// EhternetPort parameter to be used for application settings, which will automatically
    /// appear in the Edit Cycle form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// EhternetPortParameters will appear in the Edit Cycle form with two entries
    /// </para>
    /// <para>
    /// EhternetPortParameters will contain subnodes in the Edit Cycle tree for configuring the
    /// IPAddress and Port.
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
    /// <seealso cref="StringParameter"/>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class EthernetPortParameter : EditCycleParameter<EthernetPortSettings>
    {
        #region Fields (6)

        #region Private Fields (6)

        private StringParameterControl portNameEditorControl;
        private StringParameterControl ipAddressEditControl;
        private StringParameterControl portEditControl;

        #endregion Private Fields

        #region Public Fields
        //public string IPAddress; - RESPONSIBLE FOR BAD CONFIG
        //public string Port;
        //public String PortName;

        #endregion Public Fields

        #endregion Fields

        #region Constructors (2)

        /// <param name="displayName">Friendly name to be displayed in the <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>.</param>
        /// <param name="portName">EhternetPort Name</param>
        /// <param name="ipAddress">Baud Rate</param>
        /// <param name="port">Parity</param>
        /// <param name="toolTip">Description to be displayed in the <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>.</param>
        public EthernetPortParameter(string displayName, string portName, string ipAddress, string port, string toolTip)
        {
            DisplayName = displayName;
            ProcessValue = new EthernetPortSettings(portName, ipAddress, port);
            ToolTip = toolTip;
            SequenceStep = "";
            OperatingSequence = "";
        }

        /// <remarks>
        /// This parameterless constructor is required for XML serialization
        /// </remarks>
        public EthernetPortParameter()
        {
        }

        #endregion Constructors

        #region Methods (11)

        #region Public Methods (4)

        /// <summary>
        /// Creates the tree node to be displayed in the Edit Cycle form.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>
        /// The tree node to be displayed in the Edit Cycle form
        /// </returns>
        public override TreeNode CreateTreeNode(string nodeName)
        {
            if (ProcessValue != null)
            {
                NewValue = new EthernetPortSettings(
                    ProcessValue.PortName,
                    ProcessValue.IPAddress,
                    ProcessValue.Port
                );
                Updated = false;
            }
            else
            {
                NewValue = new EthernetPortSettings();
                Updated = true;
            }

            editorTreeNode = new TreeNode();
            editorTreeNode.Name = nodeName;
            editorTreeNode.Text = DisplayName;

            TreeNode treeNode2 = new TreeNode();
            treeNode2.Name = "IPAddress";
            treeNode2.Text = "IPAddress";
            editorTreeNode.Nodes.Add(treeNode2);

            treeNode2 = new TreeNode();
            treeNode2.Name = "Port";
            treeNode2.Text = "Port";
            editorTreeNode.Nodes.Add(treeNode2);

            return editorTreeNode;
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
            if (portNameEditorControl == null)
            {
                portNameEditorControl = new StringParameterControl();
                portNameEditorControl.Description = ToolTip;

                //portNameEditorControl.Items.AddRange(
                //    System.IO.Ports.EhternetPort.GetPortNames()
                //        .OrderBy(s => s)
                //        .Distinct()
                //        .ToArray());

                portNameEditorControl.ValueChanged += new EventHandler<EventArgs<string>>(portNameEditorControl_ValueChanged);
            }

            portNameEditorControl.Value = NewValue.PortName;
            return portNameEditorControl;
        }

        /// <summary>
        /// Gets the editor control to be displayed in the Edit Cycle form for the specified child node.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>
        /// The editor control to be displayed in the Edit Cycle form for the specified child node.
        /// </returns>
        public override Control GetEditorControl(PropertyInfo propertyInfo, string childNodeName)
        {
            switch (childNodeName)
            {
                case "IPAddress":
                    if (ipAddressEditControl == null)
                    {
                        ipAddressEditControl = new StringParameterControl();
                        ipAddressEditControl.Description = "Enter the IPAddress for device.";
                        ipAddressEditControl.ValueChanged += new EventHandler<EventArgs<string>>(ipAddressEditorControl_ValueChanged);
                    }
                    ipAddressEditControl.Value = NewValue.IPAddress.ToString();
                    return ipAddressEditControl;

                case "Port":
                    if (portEditControl == null)
                    {
                        portEditControl = new StringParameterControl();
                        portEditControl.Description = "(#) Enter the Port for device.";
                        portEditControl.ValueChanged += new EventHandler<EventArgs<string>>(portEditorControl_ValueChanged);
                    }
                    portEditControl.Value = NewValue.Port.ToString();
                    return portEditControl;
            }

            return null;
        }

        /// <summary>
        /// Updates the process value from new value.
        /// </summary>
        /// <returns>
        /// A message indicating that the process value was changed.
        /// </returns>
        public override string UpdateProcessValueFromNewValue()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} changed.", DisplayName));
            if (ProcessValue != null)
            {
                sb.AppendLine("    Old Settings:");
                sb.AppendLine(string.Format("        Port Name: {0}", ProcessValue.PortName));
                sb.AppendLine(string.Format("        IPAddress: {0}", ProcessValue.IPAddress));
                sb.AppendLine(string.Format("        Port: {0}", ProcessValue.Port));
            }
            sb.AppendLine("    New Settings:");
            sb.AppendLine(string.Format("        Port Name: {0}", NewValue.PortName));
            sb.AppendLine(string.Format("        IPAddress: {0}", NewValue.IPAddress));
            sb.AppendLine(string.Format("        Parity: {0}", NewValue.Port));

            string retVal = sb.ToString();

            ProcessValue = NewValue;
            return retVal;
        }

        #endregion Public Methods
        #region Private Methods (7)

        private void ipAddressEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.IPAddress = e.Value;
            CheckIfUpdated();
            if (NewValue.IPAddress != ProcessValue.IPAddress)
                editorTreeNode.Nodes["IPAddress"].ForeColor = Color.Blue;
            //IPAddress = NewValue.IPAddress;
        }

        private void CheckIfUpdated()
        {
            if (NewValue == ProcessValue)
            {
                editorTreeNode.ForeColor = Color.Black;
                foreach (TreeNode node in editorTreeNode.Nodes)
                    node.ForeColor = Color.Black;
            }
            else
            {
                editorTreeNode.ForeColor = Color.Blue;
                Updated = true;
            }
        }

        private void portEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.Port = e.Value;
            CheckIfUpdated();
            if (NewValue.Port != ProcessValue.Port)
                editorTreeNode.Nodes["Port"].ForeColor = Color.Blue;
            //Port = NewValue.Port.ToString();
        }

        private void portNameEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.PortName = e.Value;
            CheckIfUpdated();
            if (NewValue.PortName != ProcessValue.PortName)
                editorTreeNode.Nodes["PortName"].ForeColor = Color.Blue;
            //if (PortName != null)
            //    ProcessValue.PortName = e.Value;
            //PortName = NewValue.PortName;
        }

        #endregion Private Methods

        #endregion Methods
    }
}