using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Barcodes;
using iText.Kernel.Pdf.Xobject;
using VTIWindowsControlLibrary.Forms;
using VTIWindowsControlLibrary.Interfaces;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Kernel.Pdf.Canvas.Draw;
using System.Configuration;
using VTIWindowsControlLibrary.Classes.Configuration.Interfaces;
using iText.Signatures;
using VTIWindowsControlLibrary.Classes.Configuration;
using System.Text.RegularExpressions;

namespace VTIWindowsControlLibrary.Classes.ManualCommands
{
    /// <summary>
    /// Base class for the ManualCommands class in the host application.
    /// This class is a subclass of the ContextBoundObject, which allows
    /// messages (such as firing a method) to be intercepted
    /// </summary>
    [ManualCommandIntercept]
    public class ManualCommandsBase : ContextBoundObject, IManualCommands
    {
        #region Fields (2)

        #region Private Fields (2)

        private List<string> hiddenCommands;
        //private List<string> visibleCommands;
        private TouchScreenButtonForm<MethodInfo> touchScreenButtonForm;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualCommandsBase">ManualCommandsBase</see> class
        /// </summary>
        public ManualCommandsBase()
        {
            VtiEvent.Log.WriteVerbose("Initializing Manual Commands...");
            touchScreenButtonForm = new TouchScreenButtonForm<MethodInfo>();
            touchScreenButtonForm.UpdateList += new EventHandler(touchScreenButtonForm_UpdateList);
            touchScreenButtonForm.ButtonClicked += new TouchScreenButtonForm<MethodInfo>.TouchScreenButtonClickedEventHandler(touchScreenButtonForm_ButtonClicked);

            touchScreenButtonForm.MouseWheel += TouchScreenButtonForm_MouseWheel;

            touchScreenButtonForm.Text = VtiLibLocalization.ManualCommands;
            //touchScreenButtonForm.Icon = Properties.Resources.
            hiddenCommands = new List<string>();
            //UpdateCommands();
        }

        private void TouchScreenButtonForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                touchScreenButtonForm.buttonUpPush();
            }
            else
            {
                touchScreenButtonForm.buttonDownPush();
            }
        }

        #endregion Constructors

        #region Methods (13)

        #region Public Methods (10)

        //public List<Action> HiddenCommands
        //{
        //    get
        //    {
        //        List<string> hiddenCommandsStringList = hiddenCommands;
        //        List<Action> hiddenCmds = new List<Action>();
        //        //foreach (string hiddenCmdString in hiddenCommandsStringList)
        //        //{
        //        //    //Action cmd = touchScreenButtonForm.CommandList.FirstOrDefault(x => x.Name == hiddenCmdString);
        //        //    //hiddenCmds.Add(cmd);
        //        //}
        //        hiddenCmds = (List<Action>)GetType().GetMethods(BindingFlags.Public |
        //            BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        //            //.SelectMany(m => m.GetCustomAttributes(false).OfType<ManualCommandAttribute>(),
        //            //    (m, a) => new { m, a })
        //            //.Where(ma => ma.a.Visible && !hiddenCommands.Contains(ma.m.Name)
        //            );
        //        return hiddenCmds;
        //    }
        //}

        //public List<string> VisibleCommands
        //{
        //    get { return visibleCommands; }
        //}

        /// <summary>
        /// Changes the language of the Manual Commands form to the current language.
        /// </summary>
        public void ChangeLanguage()
        {
            touchScreenButtonForm.ChangeLanguage();
            UpdateCommands();
            touchScreenButtonForm.RefreshButtons();
            touchScreenButtonForm.Text = VtiLibLocalization.ManualCommands;
        }

        /// <summary>
        /// Checks to see if a text string matches a manual command
        /// </summary>
        /// <param name="scannerText">Text string to check</param>
        /// <returns>True if the text string matches a manual command</returns>
        public bool CheckForCommand(string scannerText)
        {
            return touchScreenButtonForm.CommandList.Count(c =>
                    c.Text.Equals(scannerText, StringComparison.CurrentCultureIgnoreCase) ||
                    c.Name.Equals(scannerText, StringComparison.CurrentCultureIgnoreCase)) != 0;
        }

        /// <summary>
        /// Executes the manual command indicated by a text string
        /// </summary>
        /// <param name="scannerText">Text string containing the text of the manual command to execute</param>
        public void ExecuteCommand(string scannerText)
        {
            if (CheckForCommand(scannerText))
                touchScreenButtonForm.CommandList.First(c =>
                    c.Text.Equals(scannerText, StringComparison.CurrentCultureIgnoreCase) ||
                    c.Name.Equals(scannerText, StringComparison.CurrentCultureIgnoreCase)).UserObject.InvokeWithPermission();
        }

        /// <summary>
        /// Returns true if the Manual Commands form is currently visible
        /// </summary>
        public bool IsVisible()
        {
            return touchScreenButtonForm.Visible;
        }

        /// <summary>
        /// Hides the Manual Commands Form
        /// </summary>
        public void Hide()
        {
            touchScreenButtonForm.Hide();
        }

        /// <summary>
        /// Hides the indicated manual command in the manual commmands window.
        /// </summary>
        /// <param name="command">Delegate of the manual command to be hidden.</param>
        public void HideCommand(Action command)
        {
            if (!hiddenCommands.Contains(command.Method.Name))
            {
                hiddenCommands.Add(command.Method.Name);
            }
        }

        /// <summary>
        /// Hides the indicated manual command in the manual commmands window.
        /// </summary>
        /// <param name="command">Name of the manual command to be hidden.</param>
        public void HideCommand(string command)
        {
            if (!hiddenCommands.Contains(command))
            {
                hiddenCommands.Add(command);
            }
        }

        /// <summary>
        /// Shows the Manual Commands Form
        /// </summary>
        public void Show()
        {
            this.Show(null);
        }

        /// <summary>
        /// Shows the Manual Command Form
        /// </summary>
        /// <param name="mdiParent">Parent of the Manual Commands Form</param>
        public void Show(Form mdiParent)
        {
            if (mdiParent != null)
            {
                touchScreenButtonForm.MdiParent = mdiParent;
            }
            UpdateCommands();
            touchScreenButtonForm.Show();
            touchScreenButtonForm.BringToFront();
        }

        /// <summary>
        /// Shows the indicated manual command in the manual commands window, if it was previously hidden.
        /// </summary>
        /// <param name="command">Delegate of the manual command to be shown.</param>
        public void ShowCommand(Action command)
        {
            if (hiddenCommands.Contains(command.Method.Name))
                hiddenCommands.Remove(command.Method.Name);
        }

        /// <summary>
        /// Shows the indicated manual command in the manual commands window, if it was previously hidden.
        /// </summary>
        /// <param name="command">Name of the manual command to be shown.</param>
        public void ShowCommand(string command)
        {
            if (hiddenCommands.Contains(command))
                hiddenCommands.Remove(command);
        }

        /// <summary>
        /// Set visibility of Up and Down buttons on form
        /// </summary>
        public void UpDownButtonsVisibility(bool bIsVisible)
        {
            if (bIsVisible)
            {
                touchScreenButtonForm.buttonUp.Show();
                touchScreenButtonForm.buttonDown.Show();
            }
            else
            {
                touchScreenButtonForm.buttonUp.Hide();
                touchScreenButtonForm.buttonDown.Hide();
            }
        }

        /// <summary>
        /// Set width and height of form
        /// </summary>
        public void SetSize(int w, int h)
        {
            int wBias = 97, hBias = 37;
            int xForm = touchScreenButtonForm.Size.Width;
            int yForm = touchScreenButtonForm.Size.Height;
            touchScreenButtonForm.Size = new Size(w + wBias, h + hBias);
            touchScreenButtonForm.flowLayoutPanel1.Size = new Size(w, h);
            touchScreenButtonForm.flowLayoutPanel1.MaximumSize = new Size(w, h);
            int xPanel1 = touchScreenButtonForm.panel1.Location.X;
            int yPanel1 = touchScreenButtonForm.panel1.Location.Y;
            touchScreenButtonForm.panel1.Location = new Point(xPanel1 + w + wBias - xForm, yPanel1);
        }

        // Creates a manual containing all manual commands with a localization with key ManualCommandTooltipCOMMAND
        // And all visible parameters, their process values, and descriptions
        // Meant to be called from button on main form
        public virtual void GenerateManual(string outFile)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFile));
            Document doc = new Document(pdfDoc);
            doc.SetLeftMargin(50);
            doc.SetRightMargin(50);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            Text title = createTextObj(font, 20, true, "Commands and Parameters");
            doc.Add(new Paragraph().Add(title));

            SolidLine line = new SolidLine(1f);
            LineSeparator ls = new LineSeparator(line);
            doc.Add(ls);

            Text commandHeader = createTextObj(font, 14, true, "System Commands:");
            commandHeader.SetUnderline();
            commandHeader.SetRelativePosition(0, 15, 0, 0);
            doc.Add(new Paragraph().Add(commandHeader));

            Text commandDescription = createTextObj(font, 13, false, "Most commands require that the user be logged in. For those commands, permissions can be set on a per command basis for up to nine different groups. Operators typically would not be able to open Edit Cycle Parameters but they would be able to start a test. To learn more about setting Permissions, see other manual sections. Most commands are on the Manual Commands window but not all of them.");
            commandDescription.SetRelativePosition(0, 10, 0, 0);
            doc.Add(new Paragraph().Add(commandDescription).SetMultipliedLeading((float)1.2));

            float[] colWidths = { 120, 375 };
            Table table = new Table(colWidths, false);
            table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
            table.AddFooterCell(new Cell(1, 3).SetHeight(20).SetBorder(Border.NO_BORDER));
            table.SetRelativePosition(0, 15, 0, 0);

            addTextToTable(table, font, 13, true, "Command Text");
            addTextToTable(table, font, 13, true, "Command Description");

            for (int i = 0; i < touchScreenButtonForm.CommandList.Count; i++)
            {
                var cmd = touchScreenButtonForm.CommandList[i];
                // ManualCommandTooltip + commandName without whitespace
                string manualCmdToolTipName = "ManualCommandTooltip" + Regex.Replace(cmd.Name, @"\s", "");
                string description = VtiLib.Localization.GetString(manualCmdToolTipName);
                if (description != null)
                {
                    addTextToTable(table, font, 12, false, cmd.Name);
                    addTextToTable(table, font, 12, false, description);
                }
                else
                {
                    // for debugging / breakpoint
                }
            }
            doc.Add(table);

            Text paramHeader = createTextObj(font, 14, true, "Edit Cycle:\n");
            paramHeader.SetUnderline();
            paramHeader.SetRelativePosition(0, 20, 0, 0);

            Text paramDescription = createTextObj(font, 13, false, "The following list is composed of the Edit Cycle parameters. It’s organized in a manner similar to how it’s displayed in the software. Each Parameter is categorized into one of six categories: Common Control, Common Mode, Common Pressure, Common Time, Common Flow, or Model Parameters. The first five categories contain parameters that would not need to be changed if other parts are run. Independent sets of the model parameters however can be created so the different test “recipes” can be created. Whichever model is selected, that is the model that will be used during the test.");
            paramDescription.SetRelativePosition(0, 25, 0, 0);

            var p = new Paragraph();
            p.SetKeepTogether(true);
            p.Add(paramHeader);
            p.Add(paramDescription);
            p.SetMultipliedLeading((float)1.2);
            doc.Add(p);

            float[] colWidths2 = { 120, 120, 250 };
            Table table2 = new Table(colWidths2, true);
            table2.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
            table2.SetRelativePosition(0, 30, 0, 0);
            table2.AddFooterCell(new Cell(1, 3).SetHeight(20).SetBorder(Border.NO_BORDER));

            #region build parameter tree
            TreeNode treeNode1;
            System.Windows.Forms.TreeView treeView1 = new System.Windows.Forms.TreeView();
            // Get the members of the Config type
            PropertyInfo[] properties = VtiLib.Config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsSubclassOf(typeof(ApplicationSettingsBase)) && (property.Name != "CurrentModel") && (property.Name != "IO"))
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
                    }
                }
            }
            #endregion

            addSectionToTable("Common Control ", treeView1, table2, font);
            addSectionToTable("Common Mode ", treeView1, table2, font);
            addSectionToTable("Common Pressure ", treeView1, table2, font);
            addSectionToTable("Common Time ", treeView1, table2, font);
            addSectionToTable("Common Flow ", treeView1, table2, font);
            addSectionToTable("Model DEFAULT ", treeView1, table2, font);


            doc.Add(table2);
            table2.Complete();

            doc.Close();

            try
            {
                System.Diagnostics.Process.Start(Path.GetDirectoryName(outFile));
            }
            catch (Exception ex) { }
        }

        // Adds parameter section (Control, mode, etc) to table with process values and descriptions
        private void addSectionToTable(string sectionName, System.Windows.Forms.TreeView treeView1, Table table, PdfFont font)
        {

            addTextToTable(table, font, 13, true, sectionName + "Parameters");

            addTextToTable(table, font, 13, true, "Process Value");

            addTextToTable(table, font, 13, true, "Description");

            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Text.StartsWith(sectionName))
                {
                    object objectSettings = node.Tag;

                    foreach (PropertyInfo property in objectSettings.GetType().GetProperties())
                    {
                        if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                        {
                            IEditCycleParameter editCycleParameter = property.GetValue(objectSettings, null) as IEditCycleParameter;
                            string processValue = "";

                            if (!editCycleParameter.Visible) continue;

                            if (property.PropertyType.Name == "SerialPortParameter")
                            {
                                //COM port changed by user. Close all COM ports and re-open with new ProcessValues.
                                SerialPortParameter tempPortToGetParams = editCycleParameter as SerialPortParameter;
                                processValue = tempPortToGetParams.ProcessValue.PortName + ", " + tempPortToGetParams.ProcessValue.BaudRate + ", " + tempPortToGetParams.ProcessValue.Parity + ", " + tempPortToGetParams.ProcessValue.DataBits + ", " + tempPortToGetParams.ProcessValue.StopBits + ",  " + tempPortToGetParams.ProcessValue.Handshake;
                            }
                            else if (property.PropertyType.Name == "EthernetPortParameter")
                            {
                                EthernetPortParameter tempPortToGetParams = editCycleParameter as EthernetPortParameter;
                                processValue = tempPortToGetParams.ProcessValue.PortName + "-" + tempPortToGetParams.ProcessValue.IPAddress + ":" + tempPortToGetParams.ProcessValue.Port;
                            }
                            else
                            {
                                processValue = editCycleParameter.ProcessValueString.Replace(';', '\n');
                            }

                            addTextToTable(table, font, 12, false, editCycleParameter.DisplayName);

                            addTextToTable(table, font, 12, false, processValue);

                            addTextToTable(table, font, 12, false, editCycleParameter.ToolTip);
                        }
                    }
                }
            }
            Cell cell = new Cell();
            table.AddCell(cell);
            cell = new Cell();
            table.AddCell(cell);
            cell = new Cell();
            table.AddCell(cell);
        }

        // adds text to a table contained in a new cell
        private void addTextToTable(Table table, PdfFont font, float size, bool bold, string text)
        {
            Cell cell = new Cell();
            cell.SetKeepTogether(true);
            cell.SetTextAlignment(TextAlignment.CENTER);
            Text textObj = createTextObj(font, size, bold, text);
            cell.Add(new Paragraph(textObj));
            table.AddCell(cell);
        }

        private Text createTextObj(PdfFont font, float size, bool bold, string text)
        {
            Text textObj = new Text(text);
            textObj.SetFont(font);
            if (bold) textObj.SetBold();
            textObj.SetFontSize(size);
            return textObj;
        }

        #endregion Public Methods
        #region Private Methods (3)

        private void touchScreenButtonForm_ButtonClicked(object sender, TouchScreenButtonForm<MethodInfo>.TouchScreenButtonClickedEventArgs e)
        {
            e.TouchScreenCommand.UserObject.InvokeWithPermission();
        }

        private void touchScreenButtonForm_UpdateList(object sender, EventArgs e)
        {
            UpdateCommands();
        }

        public void UpdateCommands()
        {
            //var t = hiddenCommands;

            //var cmds = this.GetType().GetMethods(BindingFlags.Public |
            //        BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            //        .SelectMany(m => m.GetCustomAttributes(false).OfType<ManualCommandAttribute>(),
            //            (m, a) => new { m, a });

            //foreach (var cmd in cmds)
            //{
            //    cmd.a.ShowInPermissionsForm = false;
            //    cmd.a.Visible = false;
            //}

            ////for each ManualCommand in "hiddenCommands" list, set the "Visible" and "ShowInPermissionsForm" properties to false. 
            //this.GetType().GetMethods(BindingFlags.Public |
            //        BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            //        .SelectMany(m => m.GetCustomAttributes(false).OfType<ManualCommandAttribute>(),
            //            (m, a) => new { m, a })
            //        .Where(ma => !hiddenCommands.Contains(ma.m.Name)).ToList()
            //        .ForEach(ma => { ma.a.Visible = false; ma.a.ShowInPermissionsForm = false; });


            touchScreenButtonForm.CommandList.Clear();

            // Before looking up an exact Localization match below to change the text of the Manual Command Button, set the Localization to ignore case so that the Localization variable can be ManualCommandAUTOTEST or ManualCommandAutotest or MaNuAlCoMmANDautoTeST.
            VtiLib.Localization.IgnoreCase = true;

            // add visible commands to Manual Commands Form. If there is a localization match for the displayed text OR Manual Command method name (for muti-language support), use it.
            // If manual command method name is "ResetBlue()" and the displayed text is "RESET - BLUE PORT", the Localization variable must be ManualCommandResetBlue or ManualCommandRESETBLUEPORT (case-insensitive)

            touchScreenButtonForm.CommandList.AddRange(
                this.GetType().GetMethods(BindingFlags.Public |
                    BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .SelectMany(m => m.GetCustomAttributes(false).OfType<ManualCommandAttribute>(),
                        (m, a) => new { m, a })
                    .Where(ma => ma.a.Visible && !hiddenCommands.Contains(ma.m.Name))
                    .Select(ma => new TouchScreenButtonForm<MethodInfo>.TouchScreenCommand
                    {
                        Name = ma.a.CommandText,
                        Text = VtiLib.Localization.GetString("ManualCommand" + ma.a.CommandText.Replace(" ", "").Replace("-", "")) ?? VtiLib.Localization.GetString("ManualCommand" + ma.m.Name) ?? ma.a.CommandText,
                        UserObject = ma.m,
                        ToolTipText = VtiLib.Localization.GetString("ManualCommandToolTip" + ma.a.CommandText.Replace(" ", "").Replace("-", "")) ?? VtiLib.Localization.GetString("ManualCommandToolTip" + ma.m.Name) ?? ""
                    }));
        }

        // Creates PDF with barcodes for all visible manual commands
        public virtual void GenerateBarcodePDF(string outputPDFName)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outputPDFName));
            Document doc = new Document(pdfDoc);
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();

            var commandListSorted = touchScreenButtonForm.CommandList.OrderBy(x => x.Name).ToList();

            for (int i = 0; i < commandListSorted.Count; i++)
            {
                // Do not use commandListSorted[i].Text. If commands are translated to Spanish,
                // barcoded data will be in Spanish and this function will break trying to encode special Spanish characters 
                table.AddCell(CreateBarcode(commandListSorted[i].Name, pdfDoc));
                if (i % 13 != 0 || i == 0)
                { // Dont add empty cell at start of new page
                    table.AddCell(CreateEmptyCell());
                }
            }

            doc.Add(table);
            doc.Close();
        }
        private static Cell CreateBarcode(string code, PdfDocument pdfDoc)
        {
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            Barcode128 barcode = new Barcode128(pdfDoc, font);
            barcode.SetCode(code);
            barcode.SetBarHeight(25);
            barcode.SetSize(6); // font size
            barcode.FitWidth(100); // prevents text from compressing
            barcode.SetBaseline(6); // space between barcode and text

            var barcodeImg = new iText.Layout.Element.Image(barcode.CreateFormXObject(null, null, pdfDoc));
            barcodeImg.SetWidth(UnitValue.CreatePercentValue(100));
            barcodeImg.SetHeight(75);

            Cell cell = new Cell().Add(barcodeImg);
            cell.SetPaddingTop(5);
            cell.SetPaddingRight(5);
            cell.SetPaddingBottom(5);
            cell.SetPaddingLeft(5);
            cell.SetBorder(Border.NO_BORDER);

            return cell;
        }

        private static Cell CreateEmptyCell()
        {
            Cell cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);

            return cell;
        }

        #endregion Private Methods

        #endregion Methods
    }
}