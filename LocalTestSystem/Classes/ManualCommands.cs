using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Windows.Forms;
using LocalTestSystem.Classes;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Classes.IOClasses;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.ManualCommands;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Enums;
using VTIWindowsControlLibrary.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Forms.Application;

namespace LocalTestSystem
{
    /// <summary>
    /// ManualCommandsClass
    /// Contains all of the methods that become "manual commands"
    /// Each method must be proceeded by the ManualCommand attribute to become a manual command
    /// The ManualCommand attribute takes care of getting the command onto the manual commands form
    /// and it handles the permissions to the commands.
    /// </summary>
    public class ManualCommands : ManualCommandsBase
    {
		[ManualCommand("asdfasdf", true, CommandPermissionType.CheckPermissionWithWarning)]
		public virtual void TestStep()
		{
            WebBrowser wb = new WebBrowser();
            //string html = File.ReadAllText("file:///C:\\Users\\ewilson\\Downloads\\a.html");
            wb.Navigate(new Uri("file:///C:\\Users\\ewilson\\Downloads\\a.html"));
            HtmlDocument document = wb.Document;
            //var x = document.GetElementById("textIn").InnerText;
            string x = "asdf";
            int i, j, intWeight, intLength;
            int intWtProd = 0;
            char[] arrayData = new char[256]; 
            int fs;
			string[] arraySubst = { "Ã", "Ä", "Å", "Æ", "Ç", "È", "É", "Ê" };
            string chrString;

			/*
			 * Checksum Calculation for Code 128 B
			 */
			intLength = x.Length;
			arrayData[0] = (char)104; // Assume Code 128B, Will revise to support A, C and switching.
			intWtProd = 104;
			for(j = 0; j < intLength; j += 1)
			{
				arrayData[j + 1] = (char)(x[j] - 32); // Have to convert to Code 128 encoding, might be broken
				intWeight = j + 1; // to generate the checksum
				intWtProd += intWeight * arrayData[j + 1]; // Just a weighted sum
			}
			arrayData[j + 1] = (char)(intWtProd % 103); // Modulo 103 on weighted sum
			arrayData[j + 2] = (char)106; // Code 128 Stop character
			int chr = (int)arrayData[j + 1]; // Gotta convert from character to a number
			if(chr > 94)
			{
				chrString = arraySubst[chr - 95];
			}
			else
			{
				chrString = (chr + 32).ToString();
			}

            var a = document.GetElementById("check");
            var b = document.GetElementsByTagName("div");
			document.GetElementById("check").InnerHtml =
			  "Checksum = " + chr + " or character " + // Make It Visual
			  chrString + ", for text = \"" + x + "\"";

			document.GetElementById("test").InnerHtml =
			  'Ì' + // Start Code B
			  x + // The originally typed string
			  chrString + // The generated checksum
			  'Î'; // Stop Code
		}

		

		#region common commands

		[ManualCommand("ACKNOWLEDGE", false, CommandPermissionType.AnyLoggedInUser)]
        [ManualCommand("ack", false, CommandPermissionType.AnyLoggedInUser)]
        public virtual void AcknowledgeCommand()
        {
            MyStaticVariables.Acknowledge = true;
        }

        [ManualCommand("SAVE CONFIG FILE", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void SaveConfigGoodFile()
        {
            VTIWindowsControlLibrary.Classes.Util.SaveConfigGoodFile.Save();
        }

        [ManualCommand("VIEW MANUAL CMD EXEC LOG", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewManualCmdExecLog()
        {
            VTIWindowsControlLibrary.Classes.ClientForms.ManualCmdExecLog.Show();
        }
        [ManualCommand("VIEW PARAMETER CHANGE LOG", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewParamChangeLog()
        {
            VTIWindowsControlLibrary.Classes.ClientForms.ParamChangeLog.Show();
        }
        [ManualCommand("VIEW EVENT VIEWER", false, CommandPermissionType.CheckPermissionWithWarning)]
        [ManualCommand("VIEW SYSTEM LOG", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewEventViewer()
        {
            System.Diagnostics.Process.Start("notepad.exe", VtiEvent.Log.LogFileName);
        }

        [ManualCommand("VIEW EDIT CYCLE", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewEditCycle()
        {
            EditCycle.Show(Machine.MainForm);
        }

        [ManualCommand("VIEW MANUAL COMMANDS", false, CommandPermissionType.None)]
        public virtual void ViewManualCommands()
        {
            this.Show(Machine.MainForm);
        }

        [ManualCommand("VIEW METERS", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewMeters()
        {
            if (Properties.Settings.Default.DualPortSystem)
            {

            }
            else
                Machine.SystemSignals[Port.Blue].Visible =
                    !Machine.SystemSignals[Port.Blue].Visible;
        }

        [ManualCommand("VIEW PERMISSIONS", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewPermissions()
        {
            Permissions.ShowDialog();
        }

        [ManualCommand("VIEW DATA PLOT", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewDataPlot()
        {
            Machine.DataPlotDockControl[Port.Blue].Show();
        }

        [ManualCommand("VIEW SCHEMATIC", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewSchematic()
        {

            switch (MessageBox.Show("Would you like the Schematic to be Active?", "VIEW SCHEMATIC", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    Machine.Schematic.Show(true);
                    break;
                case DialogResult.No:
                    Machine.Schematic.Show(false);
                    break;
            }
        }

        [ManualCommand("VIEW DIGITAL IO", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ViewDigitalIO()
        {
            switch (MessageBox.Show("Would you like the Digital I/O to be Active?", "VIEW DIGITAL I/O", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    DigitalIO.Show(true);
                    break;
                case DialogResult.No:
                    DigitalIO.Show(false);
                    break;
            }
        }
        [ManualCommand("LOGIN", false, CommandPermissionType.None)]
        [ManualCommand("LOGON", true, CommandPermissionType.None)]
        public virtual void Login()
        {
            if (string.IsNullOrEmpty(Config.OpID))
            {
                String tempOpId;
                tempOpId = VtiLib.Data.OpIDfromPassword(KeyPad.Show(Localization.AskPassword, true));
                if (tempOpId != string.Empty)
                {
                    Config.OpID = tempOpId;
                    (Machine.MainForm.Controls["statusStrip1"] as StatusStrip).Items["OpID"].Text = Config.OpID;
                    VtiEvent.Log.WriteInfo("User '" + Config.OpID + "' logged in.", VtiEventCatType.Login);
                    Config.TestMode = TestModes.Autotest;
                }
                else
                {
                    VtiEvent.Log.WriteWarning("Invalid password entered.", VtiEventCatType.Login);
                    MessageBox.Show(Localization.InvalidPassword, Application.ProductName);
                    this.Login();   // make the user try again
                }
            }
            else
            {
                VtiEvent.Log.WriteWarning("User '" + Config.OpID + "' already logged in.", VtiEventCatType.Login);
                MessageBox.Show(Localization.CurrentUser + "'" + Config.OpID + "' " + Localization.MustLogOff, Application.ProductName);
            }
        }

        [ManualCommand("LOGOFF", true, CommandPermissionType.AnyLoggedInUser)]
        [ManualCommand("LOGOUT", false, CommandPermissionType.AnyLoggedInUser)]
        public virtual void Logoff()
        {
            if (MessageBox.Show(Localization.AskLogOff, Application.ProductName, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                VtiEvent.Log.WriteInfo("User '" + Config.OpID + "' logged off.", VtiEventCatType.Login);
                Config.OpID = string.Empty;
                (Machine.MainForm.Controls["statusStrip1"] as StatusStrip).Items["OpID"].Text = Localization.LoggedOff;
                Config.TestMode = TestModes.Logoff;
            }
        }

        [ManualCommand("RESET", true, CommandPermissionType.AnyLoggedInUser)]
        public virtual void Reset()
        {
            Machine.Cycle[0].Reset.Start();
            Config.TestMode = TestModes.Autotest;
        }


        [ManualCommand("SHUT DOWN", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void ShutDown()
        {
            IO.DOut.VacuumPumpEnable.Disable();
            bool thrdError = false;
            try
            {
                Machine.MainForm.Cursor = Cursors.WaitCursor;

                VtiEvent.Log.WriteWarning("System shutting down...");

                VtiEvent.Log.WriteVerbose("Saving Configuration...");
                Config.Save();

                VtiEvent.Log.WriteVerbose("Stopping Machine Cycle...");
                Machine.Cycle[0].Stop();
                if (Machine.Cycle[0].ProcessThread != null && !Machine.Cycle[0].ProcessThread.Join(3000))
                {
                    thrdError = true;
                    VtiEvent.Log.WriteError("Error stopping the Machine Cycle Thread!");
                }

                VtiEvent.Log.WriteVerbose("Stopping I/O Interface...");
                Config.IO.Interface.TurnAllOff();
                Config.IO.Interface.Stop();
                if (Config.IO.Interface.ProcessThread != null && !Config.IO.Interface.ProcessThread.Join(3000))
                {
                    thrdError = true;
                    VtiEvent.Log.WriteError("Error stopping the I/O Interface Thread!");
                }


                if (thrdError)
                {
                    VtiEvent.Log.WriteVerbose("Terminating the application...");
                    Environment.Exit(-1);
                }
                else
                {
                    VtiEvent.Log.WriteVerbose("Exiting the application...");
                    Application.Exit();
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("An error occurred shutting down the application!",
                  VtiEventCatType.Application_Error, e.ToString());
                VtiEvent.Log.WriteVerbose("Terminating the application...");
                Environment.Exit(-1);
            }
            //      }
        }

        [ManualCommand("AUTOTEST", true, CommandPermissionType.AnyLoggedInUser)]
        public virtual void AutoTest()
        {
            Machine.Test[0].ReadyToTest = false;
            Machine.Test[1].ReadyToTest = false;
            Config.TestMode = TestModes.Autotest;
            //Machine.Cycle[0].Reset.Start();
            //Machine.Cycle[1].Reset.Start();

        }

        [ManualCommand("SELECT MODEL", true, CommandPermissionType.AnyLoggedInUser)]
        public virtual void SelectModel()
        {
            SelectModelForm.Show(Machine.MainForm, Config.CurrentModel[Port.Blue], Config.DefaultModel);
        }

        [ManualCommand("INQUIRE", false, CommandPermissionType.CheckPermissionWithWarning)]
        public virtual void Inquire()
        {
            VTIWindowsControlLibrary.Classes.ClientForms.Inquire.Show();
        }
        public virtual void DisplayInquire()
        {
            VTIWindowsControlLibrary.Classes.ClientForms.Inquire.Show();
        }

        public void VTIMessagebox(string msg)
        {
            Machine.Cycle[0].bMessageFormText = msg;
            Machine.Cycle[0].bdisplayMessageForm = true;
        }

        [ManualCommand("SPANISH", false, CommandPermissionType.AnyLoggedInUser)]
        public void Spanish()
        {
            Config.Control.Language.ProcessValue = Languages.Spanish;
            Config.Save();
            Machine.Cycle[0].bUpdateLanguage = true;
        }

        [ManualCommand("ENGLISH", false, CommandPermissionType.AnyLoggedInUser)]
        public void English()
        {
            Config.Control.Language.ProcessValue = Languages.English;
            Config.Save();
            Machine.Cycle[0].bUpdateLanguage = true;
        }

        #endregion
    }
}
