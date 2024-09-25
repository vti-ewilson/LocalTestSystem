using System;
using System.Threading;
using System.Windows.Forms;
using LocalTestSystem.Classes;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Classes.IOClasses;
using LocalTestSystem.Enums;
using VTIPLCInterface;
using VTIWindowsControlLibrary;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.CycleSteps;
using VTIWindowsControlLibrary.Enums;

namespace LocalTestSystem.Forms
{
    public partial class MainForm : Form
    {
        # region Construction

        public MainForm()
        {
            if (Properties.Settings.Default.DebugMode) MessageBox.Show("Waiting for debugger to attach.  Click OK to continue.", "Debug Mode", MessageBoxButtons.OK);
            VtiEvent.Log.WriteInfo("Initializing Main Form...");
            InitializeComponent();
            timerStatusBar.Interval = (61 - System.DateTime.Now.Second) * 1000;
            timerStatusBar.Enabled = true;
            CurrentTime.Text = System.DateTime.Now.ToShortTimeString();
            CurrentDate.Text = System.DateTime.Now.ToShortDateString();
        }

        #endregion

        #region Properties

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        //public StatusStrip statusStrip
        //{
        //    get { return this.statusStrip1; }
        //}

        //public MenuStrip MenuStrip
        //{
        //    get { return menuStrip1; }
        //}

        #endregion

        #region Events

        private void toolStripButtonMeters_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewMeters();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void toolStripButtonManualCommands_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewManualCommands();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void toolStripButtonEditCycle_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewEditCycle();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void toolStripButtonShutdown_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ShutDown();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void toolStripButtonSystemLog_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewEventViewer();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void toolStripButtonDataPlot_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewDataPlot();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void toolStripButtonSchematic_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewSchematic();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void permissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewPermissions();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void systemPressuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewMeters();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void manualCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewManualCommands();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void systemLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Machine.ManualCommands.ViewEventViewer();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
            };
        }

        private void editCycleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Machine.ManualCommands.ViewEditCycle();
        }

        private void dataPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Machine.ManualCommands.ViewDataPlot();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Machine.ManualCommands.ShutDown();
        }

        #endregion

        private void toolStripButtonDigitalIO_Click(object sender, EventArgs e)
        {
            Machine.ManualCommands.ViewDigitalIO();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            VtiEvent.Log.WriteVerbose("Loading Main Form...");

            statusStrip1.Items["SystemID"].Text = Config.Control.System_ID.ProcessValue.ToString();

            if (Properties.Settings.Default.DualPortSystem)
                Machine.OpFormDual.Show();
            else
                Machine.OpFormSingle.Show();

            //if (!Config.Mode.AllowBadgeScanToLogin.ProcessValue)
            {
                //if (Config.Mode.AutoLoginAsOperator.ProcessValue)
                //{
                //    Machine.ManualCommands.OperatorLogin();
                //}
                //else
                {
                    Machine.ManualCommands.Login();
                }
            }
            //else
            //{
            //    Machine.Prompt[0].AppendText(Environment.NewLine + Localization.ScanBadgeToLogin + Environment.NewLine);
            //}
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.FormOwnerClosing) ||
                (e.CloseReason == CloseReason.MdiFormClosing) ||
                (e.CloseReason == CloseReason.UserClosing) ||
                (e.CloseReason == CloseReason.None))
            {
                e.Cancel = true;
                Machine.ManualCommands.ShutDown();
            }
        }

        private void timerStatusBar_Tick(object sender, EventArgs e)
        {
            timerStatusBar.Interval = 60000;
            CurrentTime.Text = System.DateTime.Now.ToShortTimeString();
            CurrentDate.Text = System.DateTime.Now.ToShortDateString();
        }

        private void testHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DualPortSystem)
            {
                Machine.OpFormDual.TestHistoryDockControl[0].ShowDocked();
                Machine.OpFormDual.TestHistoryDockControl[1].ShowDocked();
            }
            else
            {
                Machine.OpFormSingle.TestHistoryDockControl.ShowDocked();
            }
        }

        private void toolStripButtonValvesPanel_Click(object sender, EventArgs e)
        {
            // display only one ValvePanel regardless of how many ports are enabled
            if (Properties.Settings.Default.DualPortSystem)
            {
                // if (Machine.OpFormDual.ValvesPanelDockControl[0].Visible)
                // {
                //     Machine.OpFormDual.ValvesPanelDockControl[0].Hide();
                // }
                // else
                // {
                //     Machine.OpFormDual.ValvesPanelDockControl[0].Show();
                // }
            }
            else
            {
                if (Machine.OpFormSingle.ValvesPanelDockControl.Visible)
                {
                    Machine.OpFormSingle.ValvesPanelDockControl.Hide();
                }
                else
                {
                    Machine.OpFormSingle.ValvesPanelDockControl.Show();
                }
            }
        }

        private void valvesPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonValvesPanel_Click(sender, e);
        }
        private void timerSlidePanels_Tick(object sender, EventArgs e)
        {
            if (DigitalIO.ManualMode)
            {
                Config.TestMode = TestModes.Manual;
                DigitalIO.ManualMode = false;
            }

            #region Active Cycle Steps form
            if (Config.Mode.ShowCycleSteps.ProcessValue)
            {
                if (Machine.CycleStepsForm.Visible == false)
                    Machine.CycleStepsForm.Show();

                String strStepNames = "";
                strStepNames += String.Format("\n" + "Active Steps:");
                foreach (CycleStep step in Machine.Cycle[Port.Blue].CycleSteps)
                {
                    if (step.Name == "WaitingForAcknowledge")  // was showing in process but was not
                        Console.Write("");
                    //if (step.Name == "ScanSerialNumber" || step.Name == "ScanSerialNumber2")
                    //    continue;
                    if (step.State == CycleStepState.InProcess)
                        if (strStepNames.Length == 0)
                            strStepNames += step.Name;
                        else
                            strStepNames += String.Format("\n" + step.Name);
                }

                if (Machine.CycleStepsForm.rtbCycleStepsActive.Text != strStepNames)
                    Machine.CycleStepsForm.rtbCycleStepsActive.Text = strStepNames;

                // = strStepNames; // add branch color
            }
            else { if (Machine.CycleStepsForm.Visible) Machine.CycleStepsForm.Hide(); }
            #endregion

            if (Machine.Cycle[Port.Blue].bUpdateLanguage)
            {
                Machine.Cycle[Port.Blue].bUpdateLanguage = false;

                switch (Config.Control.Language.ProcessValue)
                {
                    case Languages.English:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = Machine.EnglishCulture;
                        //Program.MainForm.EnglishButton.Checked = true;
                        //Program.MainForm.SpanishButton.Checked = false;
                        break;

                    case Languages.Spanish:
                        System.Threading.Thread.CurrentThread.CurrentUICulture = Machine.SpanishCulture;
                        //Program.MainForm.EnglishButton.Checked = false;
                        //Program.MainForm.SpanishButton.Checked = true;
                        break;
                }

                for (int i = 0; i < 2; i++)
                {
                    if ((i == 0) || ((i == 1) && Properties.Settings.Default.DualPortSystem))
                    {
                        // Reset sequence step text
                        Machine.Cycle[i].ReadyForSequence.Sequence.Text = Localization.SeqReadyForSequence;

                        // Reset step prompts
                        foreach (CycleStep step in Machine.Cycle[i].CycleSteps)
                        {
                            if (!string.IsNullOrEmpty(Machine.LocalizationResource.GetString(step.Name + "_Prompt")))
                            {
                                step.Prompt = Machine.LocalizationResource.GetString(step.Name + "_Prompt");
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Machine.LocalizationResource.GetString(step.Name + "Prompt")))
                                {
                                    step.Prompt = Machine.LocalizationResource.GetString(step.Name + "Prompt");
                                }
                            }
                        }
                        Machine.Cycle[i].NotEnergized.Color = System.Drawing.Color.Red;
                    }
                }
                Machine.ManualCommands.UpdateCommands();

                Machine.Cycle[0].Reset.Start();
                if (Properties.Settings.Default.DualPortSystem)
                {
                    Machine.Cycle[1].Reset.Start();
                }


                if (Machine.Cycle[Port.Blue].bAutoRunDataPlot)
                {
                    Machine.Cycle[Port.Blue].bAutoRunDataPlot = false;

                    if (Properties.Settings.Default.DualPortSystem)
                    {
                        Machine.OpFormDual.DataPlot[Port.Blue].Start();
                    }
                    else
                    {
                        Machine.OpFormSingle.DataPlot.Start();
                    }
                }
            }
		}

        private void ScannerText_TextChanged(object sender, EventArgs e)
        {
        }

        private void parameterChangeLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Machine.ManualCommands.ViewParamChangeLog();
        }

        private void manualCmdExecLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Machine.ManualCommands.ViewManualCmdExecLog();
        }

        private void saveConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Machine.ManualCommands.SaveConfigGoodFile();
        }

        private void cycleStepsFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Mode.ShowCycleSteps.ProcessValue = true;
            Config.Save();
        }
    }

}