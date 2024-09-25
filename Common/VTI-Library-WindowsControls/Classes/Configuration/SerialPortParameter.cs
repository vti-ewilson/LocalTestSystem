using System;
using System.Configuration;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components.Configuration;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Serial Port parameter to be used for application settings, which will automatically
    /// appear in the Edit Cycle form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// SerialPortParameters will appear in the Edit Cycle form with a drop-down list to
    /// select the serial port.  The drop-down list will contain all of the serial ports that
    /// exist on that particular computer.
    /// </para>
    /// <para>
    /// SerialPortParameters will contain subnodes in the Edit Cycle tree for configuring the
    /// BaudRate, Parity, DataBits, StopBits, and Handshaking properties.
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
    public class SerialPortParameter : EditCycleParameter<SerialPortSettings>
    {
        #region Fields (6)

        #region Private Fields (6)

        private DropDownParameterControl baudRateEditorControl;
        private DropDownParameterControl dataBitsEditorControl;
        private DropDownParameterControl handshakeEditorControl;
        private DropDownParameterControl parityEditorControl;
        private DropDownParameterControl portNameEditorControl;
        private DropDownParameterControl stopBitsEditorControl;

        #endregion Private Fields

        #region Public Fields
        //public int BaudRate;
        //public int DataBits;
        //public Handshake Handshake; --RESPONSIBLE FOR BAD CONFIG
        //public Parity Parity;
        //public String PortName;
        //public String PortNameWithDescription;
        //public StopBits StopBits;

        #endregion Public Fields

        #endregion Fields

        #region Constructors (2)

        /// <param name="displayName">Friendly name to be displayed in the <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>.</param>
        /// <param name="portName">Serial Port Name</param>
        /// <param name="baudRate">Baud Rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="dataBits">Data Bits</param>
        /// <param name="stopBits">Stop Bits</param>
        /// <param name="handshake">Handshaking</param>
        /// <param name="toolTip">Description to be displayed in the <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>.</param>
        public SerialPortParameter(string displayName, string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake, string toolTip)
        {
            DisplayName = displayName;
            ProcessValue = new SerialPortSettings(portName, baudRate, parity, dataBits, stopBits, handshake);
            ToolTip = toolTip;
            SequenceStep = "";
            OperatingSequence = "";
        }

        /// <param name="displayName">Friendly name to be displayed in the <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>.</param>
        /// <param name="portName">Serial Port Name</param>
        /// <param name="baudRate">Baud Rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="dataBits">Data Bits</param>
        /// <param name="stopBits">Stop Bits</param>
        /// <param name="handshake">Handshaking</param>
        /// <param name="toolTip">Description to be displayed in the <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>.</param>
        /// <param name="sequenceStep">Description to be displayed in the <see cref="VTIWindowsControlLibrary.Forms.EditCycleForm">Edit Cycle form</see>.</param>
        /// <param name="operatingSequence">Operating Sequence to which this parameter belongs.</param>
        public SerialPortParameter(string displayName, string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake, string toolTip, string sequenceStep, string operatingSequence)
        {
            DisplayName = displayName;
            ProcessValue = new SerialPortSettings(portName, baudRate, parity, dataBits, stopBits, handshake);
            ToolTip = toolTip;
            SequenceStep = sequenceStep;
            OperatingSequence = operatingSequence;
        }

        /// <remarks>
        /// This parameterless constructor is required for XML serialization
        /// </remarks>
        public SerialPortParameter()
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
                NewValue = new SerialPortSettings(
                    ProcessValue.PortName,
                    //ProcessValue.PortNameWithDescription,
                    ProcessValue.BaudRate,
                    ProcessValue.Parity,
                    ProcessValue.DataBits,
                    ProcessValue.StopBits,
                    ProcessValue.Handshake);
                Updated = false;
            }
            else
            {
                NewValue = new SerialPortSettings();
                Updated = true;
            }

            editorTreeNode = new TreeNode();
            editorTreeNode.Name = nodeName;
            editorTreeNode.Text = DisplayName;

            TreeNode treeNode2 = new TreeNode();
            treeNode2.Name = "BaudRate";
            treeNode2.Text = "Baud Rate";
            editorTreeNode.Nodes.Add(treeNode2);

            treeNode2 = new TreeNode();
            treeNode2.Name = "Parity";
            treeNode2.Text = "Parity";
            editorTreeNode.Nodes.Add(treeNode2);

            treeNode2 = new TreeNode();
            treeNode2.Name = "DataBits";
            treeNode2.Text = "Data Bits";
            editorTreeNode.Nodes.Add(treeNode2);

            treeNode2 = new TreeNode();
            treeNode2.Name = "StopBits";
            treeNode2.Text = "Stop Bits";
            editorTreeNode.Nodes.Add(treeNode2);

            treeNode2 = new TreeNode();
            treeNode2.Name = "Handshake";
            treeNode2.Text = "Handshaking";
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
                portNameEditorControl = new DropDownParameterControl();
                portNameEditorControl.Description = ToolTip;
                portNameEditorControl.ValueChanged += new EventHandler<EventArgs<string>>(portNameEditorControl_ValueChanged);
            }
            else
            {
                portNameEditorControl.Items.Clear();
            }

            //OLD - does not update COM Ports like Windows Device Manager does - NJ 04/23/2021
            //portNameEditorControl.Items.AddRange(
            //        System.IO.Ports.SerialPort.GetPortNames()
            //            .OrderBy(s => s)
            //            .Distinct()
            //            .ToArray());

            System.Collections.Generic.List<string> portNames = new System.Collections.Generic.List<string>();
            try
            {
                using (var searcher = new System.Management.ManagementObjectSearcher
                   ("root\\CIMV2", "SELECT * FROM WIN32_PnPEntity WHERE Caption LIKE '%(COM%'"))
                {
                    portNames = searcher.Get().Cast<System.Management.ManagementBaseObject>().Select(x => x["Caption"].ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Unable to query Windows for available COM ports. Exception: " + Environment.NewLine + e.Message);
                MessageBox.Show("Unable to query Windows for available COM ports. Exception: " + Environment.NewLine + e.Message);
            }
            //re-arranges string from "Communication Port (COM1)" to "(COM1) Communication Port" for ordering in drop-down list of comboBox
            portNames = portNames.Select(x => x.Substring(x.IndexOf("(COM")) + " " + x.Remove(x.IndexOf("(COM"))).ToList();
            portNameEditorControl.Items.AddRange(
                     portNames
                        //order by the actual number. Otherwise it is ordered like: COM1, COM10, COM11, COM2, COM3
                        .OrderBy(s => int.Parse(s.Substring(s.IndexOf("(COM"), s.IndexOf(")")).Substring(s.Substring(s.IndexOf("(COM"), s.IndexOf(")")).IndexOf("M") + 1)))
                        .Distinct()
                        .ToArray());
            //NewValue.PortName should be for example "COM1" (retrieved from config file)
            //portNames contains string values of existing COM ports such as "(COM1) Communication Port"
            //"portNames.Where(x => x.Substring(1, x.IndexOf(") ") - 1) == NewValue.PortName).FirstOrDefault()" MUST match an item contained in portNameEditorControl.Items for the value to be assigned to portNameEditorControl.Value.
            if (portNames.Where(x => x.Substring(1, x.IndexOf(") ") - 1) == NewValue.PortName).FirstOrDefault() != null)
            {
                portNameEditorControl.Value = portNames.Where(x => x.Substring(1, x.IndexOf(") ") - 1) == NewValue.PortName).FirstOrDefault();
            }
            else
            {
                portNameEditorControl.Value = "";
            }
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
                case "BaudRate":
                    if (baudRateEditorControl == null)
                    {
                        baudRateEditorControl = new DropDownParameterControl();
                        baudRateEditorControl.Description = "(bits/sec) Enter the baud rate for this serial device.";

                        baudRateEditorControl.Items.AddRange(
                            new string[] {
                                "110", "300", "1200", "2400", "4800",
                                "9600", "19200", "38400", "57600",
                                "115200", "230400", "460800", "921600" });
                        baudRateEditorControl.ValueChanged += new EventHandler<EventArgs<string>>(baudRateEditorControl_ValueChanged);
                    }
                    if (baudRateEditorControl.Items.Contains(NewValue.BaudRate.ToString()))
                        baudRateEditorControl.Value = NewValue.BaudRate.ToString();
                    else
                        baudRateEditorControl.Value = "9600";
                    return baudRateEditorControl;

                case "DataBits":
                    if (dataBitsEditorControl == null)
                    {
                        dataBitsEditorControl = new DropDownParameterControl();
                        dataBitsEditorControl.Description = "(#) Enter the number of data bits for this serial device.";
                        dataBitsEditorControl.Items.AddRange(new string[] { "5", "6", "7", "8" });
                        dataBitsEditorControl.ValueChanged += new EventHandler<EventArgs<string>>(dataBitsEditorControl_ValueChanged);
                    }
                    if (dataBitsEditorControl.Items.Contains(NewValue.DataBits.ToString()))
                        dataBitsEditorControl.Value = NewValue.DataBits.ToString();
                    else
                        dataBitsEditorControl.Value = "8";
                    return dataBitsEditorControl;

                case "Parity":
                    if (parityEditorControl == null)
                    {
                        parityEditorControl = new DropDownParameterControl();
                        parityEditorControl.Description = "(Parity) Enter the Parity for this serial device.";
                        parityEditorControl.Items.AddRange(
                            System.Enum.GetNames(typeof(System.IO.Ports.Parity)));
                        parityEditorControl.ValueChanged += new EventHandler<EventArgs<string>>(parityEditorControl_ValueChanged);
                    }
                    parityEditorControl.Value = NewValue.Parity.ToString();
                    return parityEditorControl;

                case "StopBits":
                    if (stopBitsEditorControl == null)
                    {
                        stopBitsEditorControl = new DropDownParameterControl();
                        stopBitsEditorControl.Description = "(Stop Bits) Enter the Stop Bits for this serial device.";
                        stopBitsEditorControl.Items.AddRange(
                            System.Enum.GetNames(typeof(System.IO.Ports.StopBits)));
                        stopBitsEditorControl.ValueChanged += new EventHandler<EventArgs<string>>(stopBitsEditorControl_ValueChanged);
                    }
                    stopBitsEditorControl.Value = NewValue.StopBits.ToString();
                    return stopBitsEditorControl;

                case "Handshake":
                    if (handshakeEditorControl == null)
                    {
                        handshakeEditorControl = new DropDownParameterControl();
                        handshakeEditorControl.Description = "(Handshaking) Enter the Handshaking for this serial device.";
                        handshakeEditorControl.Items.AddRange(
                            System.Enum.GetNames(typeof(System.IO.Ports.Handshake)));
                        handshakeEditorControl.ValueChanged += new EventHandler<EventArgs<string>>(handshakeEditorControl_ValueChanged);
                    }
                    handshakeEditorControl.Value = NewValue.Handshake.ToString();
                    return handshakeEditorControl;
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
                sb.AppendLine(string.Format("        Baud Rate: {0}", ProcessValue.BaudRate));
                sb.AppendLine(string.Format("        Parity: {0}", ProcessValue.Parity));
                sb.AppendLine(string.Format("        Data Bits: {0}", ProcessValue.DataBits));
                sb.AppendLine(string.Format("        Stop Bits: {0}", ProcessValue.StopBits));
                sb.AppendLine(string.Format("        Handshaking: {0}", ProcessValue.Handshake));
            }
            sb.AppendLine("    New Settings:");
            sb.AppendLine(string.Format("        Port Name: {0}", NewValue.PortName));
            sb.AppendLine(string.Format("        Baud Rate: {0}", NewValue.BaudRate));
            sb.AppendLine(string.Format("        Parity: {0}", NewValue.Parity));
            sb.AppendLine(string.Format("        Data Bits: {0}", NewValue.DataBits));
            sb.AppendLine(string.Format("        Stop Bits: {0}", NewValue.StopBits));
            sb.AppendLine(string.Format("        Handshaking: {0}", NewValue.Handshake));
            string retVal = sb.ToString();

            ProcessValue = NewValue;
            return retVal;
        }

        #endregion Public Methods
        #region Private Methods (7)
        private void portNameEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            if (e.Value != "")
            {
                //get COM number from full name displayed. Ex. "(COM1) Communication Port" to "COM1".
                NewValue.PortName = e.Value.Substring(e.Value.IndexOf("(COM") + 1).Remove(e.Value.IndexOf(")") - 1);
            }
            else
            {
                NewValue.PortName = "";
            }
            if (ProcessValue.PortName != null)
            {
                CheckIfUpdated();
            }

            // Do not update value until user clicks on Edit Cycle OK button (but keep the updated value in NewValue)
            //ProcessValue.PortName = NewValue.PortName;
        }
        private void baudRateEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.BaudRate = int.Parse(e.Value);
            CheckIfUpdated();
            if (NewValue.BaudRate != ProcessValue.BaudRate)
                editorTreeNode.Nodes["BaudRate"].ForeColor = Color.Blue;

            // Do not update value until user clicks on Edit Cycle OK button (but keep the updated value in NewValue)
            //ProcessValue.BaudRate = NewValue.BaudRate;
        }

        private void CheckIfUpdated()
        {
            if ((NewValue.PortName == ProcessValue.PortName || NewValue.PortName == "" || ProcessValue.PortName == "")
                && NewValue.BaudRate == ProcessValue.BaudRate
                && NewValue.Parity == ProcessValue.Parity
                && NewValue.Handshake == ProcessValue.Handshake
                && NewValue.DataBits == ProcessValue.DataBits
                && NewValue.StopBits == ProcessValue.StopBits)
            {
                editorTreeNode.ForeColor = Color.Black;
                foreach (TreeNode node in editorTreeNode.Nodes)
                    node.ForeColor = Color.Black;
                Updated = false;
            }
            else
            {
                editorTreeNode.ForeColor = Color.Blue;
                Updated = true;
            }
        }

        private void dataBitsEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.DataBits = int.Parse(e.Value);
            CheckIfUpdated();
            if (NewValue.DataBits != ProcessValue.DataBits)
                editorTreeNode.Nodes["DataBits"].ForeColor = Color.Blue;
            // Do not update value until user clicks on Edit Cycle OK button (but keep the updated value in NewValue)
            //ProcessValue.DataBits = NewValue.DataBits;
        }

        private void handshakeEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.Handshake = Enum<System.IO.Ports.Handshake>.Parse(e.Value);
            CheckIfUpdated();
            if (NewValue.Handshake != ProcessValue.Handshake)
            {
                editorTreeNode.Nodes["Handshake"].ForeColor = Color.Blue;
            }

            // Do not update value until user clicks on Edit Cycle OK button (but keep the updated value in NewValue)
            //ProcessValue.Handshake = NewValue.Handshake;
        }

        private void parityEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.Parity = Enum<System.IO.Ports.Parity>.Parse(e.Value);
            CheckIfUpdated();
            if (NewValue.Parity != ProcessValue.Parity)
                editorTreeNode.Nodes["Parity"].ForeColor = Color.Blue;

            // Do not update value until user clicks on Edit Cycle OK button (but keep the updated value in NewValue)
            //ProcessValue.Parity = NewValue.Parity;
        }

        private void stopBitsEditorControl_ValueChanged(object sender, EventArgs<string> e)
        {
            NewValue.StopBits = Enum<System.IO.Ports.StopBits>.Parse(e.Value);
            CheckIfUpdated();
            if (NewValue.StopBits != ProcessValue.StopBits)
                editorTreeNode.Nodes["StopBits"].ForeColor = Color.Blue;
            // Do not update value until user clicks on Edit Cycle OK button (but keep the updated value in NewValue)
            //ProcessValue.StopBits = NewValue.StopBits;
        }

        #endregion Private Methods

        #endregion Methods
    }
}