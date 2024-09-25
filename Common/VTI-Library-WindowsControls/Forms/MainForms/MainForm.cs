using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Windows.Forms;
using VTIWindowsControlLibrary;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.CycleSteps;
using VTIWindowsControlLibrary.Classes.ManualCommands;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Components.Graphing;
using VTIWindowsControlLibrary.Enums;
using VTIWindowsControlLibrary.Interfaces;
using VTIWindowsControlLibrary.Classes.Configuration.Interfaces;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace VTIWindowsControlLibrary.Forms.MainForms
{
    public partial class MainForm : Form
    {
        # region Construction

        public MainForm()
        {
            //if (Properties.Settings.Default.DebugMode) MessageBox.Show("Waiting for debugger to attach.  Click OK to continue.", "Debug Mode", MessageBoxButtons.OK);
            VtiEvent.Log.WriteInfo("Initializing Main Form...");
            InitializeComponent();
            timerStatusBar.Interval = (61 - System.DateTime.Now.Second) * 1000;
            timerStatusBar.Enabled = true;
            CurrentTime.Text = System.DateTime.Now.ToShortTimeString();
            CurrentDate.Text = System.DateTime.Now.ToShortDateString();
            _ManualCommands = new SortedDictionary<string, MethodInfo>();
		}

		#endregion

		#region Fields
		protected Form _OpFormDual;
		protected Form _OpFormSingle;
        private SortedDictionary<string, MethodInfo> _ManualCommands;
        private MethodInfo OnStartup, OnShutdown, ConstantTick;
        private bool machineMethodsGenterated = false;
		#endregion

		#region Properties

		#region Machine properties
		//These properties are linked to the identical ones in Machine.cs through the OpForm (see initFormComponents)
		public virtual Form OpFormDual { get { return _OpFormDual;  } }

		public virtual Form OpFormSingle { get { return _OpFormSingle; } }
		
        #endregion

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

		#endregion

		#region Events

		private void frmMain_Load(object sender, EventArgs e) {
			VtiEvent.Log.WriteVerbose("Loading Main Form...");

            // Gets system id displays at bottom of form
			IEditCycleParameter systemID = null;
			if(getParameter("Control", "System_ID", ref systemID)) {
                statusStrip1.Items["SystemID"].Text = ((StringParameter)systemID).ProcessValue;
			} else {
				statusStrip1.Items["SystemID"].Text = "";
			}

			initFormComponents(); // Initialize OpForm

            // Add all Manual commands to dictionary
            foreach (MethodInfo method in VtiLib.ManualCommands.GetType().GetMethods())
            {
                if (!_ManualCommands.ContainsKey(method.Name)) _ManualCommands.Add(method.Name, method);
            }

            if (VtiLib.IsDualPortSystem)
                OpFormDual.Show();
            else
                OpFormSingle.Show();

            getMachineMethods();

			if(OnStartup != null) {
				OnStartup.Invoke(VtiLib.Machine, null);
			} else {
				VtiEvent.Log.WriteWarning("Machine.OnStartup method not found");
			}
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                if((e.CloseReason == CloseReason.FormOwnerClosing) ||
				    (e.CloseReason == CloseReason.MdiFormClosing) ||
				    (e.CloseReason == CloseReason.UserClosing) ||
				    (e.CloseReason == CloseReason.None)) {
				    e.Cancel = true;
                    //VtiLib.Machine.OnShutdown();
			    }
            }catch(Exception ex) {
				MessageBox.Show("Error: Operator ID (Config.OpID) does not exist in users table or corrupted config file. Make sure GROUP01-09 users are added if needed.");
			}

		}

        private void timerStatusBar_Tick(object sender, EventArgs e) {
			timerStatusBar.Interval = 60000;
			CurrentTime.Text = System.DateTime.Now.ToShortTimeString();
			CurrentDate.Text = System.DateTime.Now.ToShortDateString();
		}

		private void timerSlidePanels_Tick(object sender, EventArgs e) 
        {
            if (machineMethodsGenterated)
            {
                if (ConstantTick != null)
                {
                    ConstantTick.Invoke(VtiLib.Machine, null);
                }
                else
                {
                    VtiEvent.Log.WriteWarning("Machine.ConstantTick method not found");
                }
            }
		}

		private void ScannerText_TextChanged(object sender, EventArgs e) {
		}

		// Checks if parameter and section exist then sets param, returns true on success
        // sectionName = "Control", "Mode" etc.
        // paramName = variable name of parameter (not display name)
        // param = object to store parameter in
		private bool getParameter(string sectionName, string paramName, ref IEditCycleParameter param) {

			PropertyInfo[] sectionSettingsPropertyArray = VtiLib.Config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == sectionName).ToArray();

			if(sectionSettingsPropertyArray.Length > 0) { // Checks if section exists

				object modeSettings = sectionSettingsPropertyArray[0].GetValue(null, null);
				PropertyInfo[] parameterPropertyArray = modeSettings.GetType().GetProperties().Where(m => m.PropertyType.GetInterface("IEditCycleParameter") != null && m.Name == paramName).ToArray();

                if(parameterPropertyArray.Length > 0) { // Checks if parameter exists

                    param = parameterPropertyArray[0].GetValue(modeSettings, null) as IEditCycleParameter;
                    return true;
                } 
			}
            return false;
		}

		#endregion

		private void initFormComponents() {
            if(VtiLib.IsDualPortSystem) {
                //Checks if OpFormDual property exists in machine
                PropertyInfo[] opFormDualArray = VtiLib.Machine.GetType().GetProperties().Where(m => m.Name == "OpFormDual").ToArray();
                if(opFormDualArray.Length > 0) {
                    _OpFormDual = (Form)opFormDualArray[0].GetValue(null, null);
                } else {
                    VtiEvent.Log.WriteInfo("ERROR: Machine.OpFormDual Property not found");
                }
            } else {
                // Checks if OpFormSingle property exists in machine
				PropertyInfo[] opFormSingleArray = VtiLib.Machine.GetType().GetProperties().Where(m => m.Name == "OpFormSingle").ToArray();
                if(opFormSingleArray.Length > 0) {
                    _OpFormSingle = (Form) opFormSingleArray[0].GetValue(null, null);
                } else {
        	        VtiEvent.Log.WriteInfo("ERROR: Machine.OpFormSingle Property not found");
                }

            }

        }

        private void getMachineMethods() {
            // Gets Machine.OnStartup Method
            MethodInfo[] MethodArray = VtiLib.Machine.GetType().GetMethods().Where(m => m.Name == "OnStartup").ToArray();
            if(MethodArray.Length > 0) {
                OnStartup = MethodArray[0];
            } else {
                OnStartup = null;
            }

			// Gets Machine.OnShutdown Method
			MethodArray = VtiLib.Machine.GetType().GetMethods().Where(m => m.Name == "OnShutdown").ToArray();
			if(MethodArray.Length > 0) {
				OnShutdown = MethodArray[0];
			} else {
				OnShutdown = null;
			}

			// Gets Machine.ConstantTick Method
			MethodArray = VtiLib.Machine.GetType().GetMethods().Where(m => m.Name == "ConstantTick").ToArray();
			if(MethodArray.Length > 0) {
				ConstantTick = MethodArray[0];
			} else {
				ConstantTick = null;
			}

            machineMethodsGenterated = true;
        }

        #region toolStripButtons

        private void toolStripButtonMeters_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewMeters"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewMeters Manual Command does not exist");
            };
        }

        private void toolStripButtonManualCommands_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewManualCommands"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewManualCommands Manual Command does not exist");
            };
        }

        private void toolStripButtonEditCycle_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewEditCycle"].InvokeWithPermission();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewEditCycle Manual Command does not exist");
            };
        }

        private void toolStripButtonShutdown_Click(object sender, EventArgs e)
        {
            try
            {
                if (OnShutdown != null)
                {
                    OnShutdown.Invoke(VtiLib.Machine, null);
                }
                else
                {
                    VtiEvent.Log.WriteWarning("Machine.OnShutdown method not found");
                }
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
                _ManualCommands["ViewEventViewer"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewEventViewer Manual Command does not exist");
            };
        }

        private void toolStripButtonDataPlot_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewDataPlot"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewDataPlot Manual Command does not exist");
            };
        }

        private void toolStripButtonSchematic_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewSchematic"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewSchematic Manual Command does not exist");
            };
        }

        private void permissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewPermissions"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewPermissions Manual Command does not exist");
            };
        }

        private void systemPressuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewMeters"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewMeters Manual Command does not exist");
            };
        }

        private void manualCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewManualCommands"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewMeters Manual Command does not exist");
            };
        }

        private void systemLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewEventViewer"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewEventViewer Manual Command does not exist");
            };
        }

        private void editCycleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewEditCycle"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewEditCycle Manual Command does not exist");
            };
        }

        private void dataPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _ManualCommands["ViewDataPlot"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewDataPlot Manual Command does not exist");
            };
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnShutdown != null)
            {
                OnShutdown.Invoke(VtiLib.Machine, null);
            }
            else
            {
                VtiEvent.Log.WriteWarning("Machine.OnShutdown method not found");
            }
        }

        private void toolStripButtonDigitalIO_Click(object sender, EventArgs e)
        {

            try
            {
                _ManualCommands["ViewDigitalIO"].InvokeWithPermission();
            }
            catch
            {
                MessageBox.Show("Error: Operator ID (Config.OpID) does not exist or ViewDigitalIO Manual Command does not exist");
            };
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox.Show();
        }

        // Returns Test History to its docked position
        private void testHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Equivalent to OpFormSingle.TestHistoryDockControl.ShowDocked(); but works with multiple different OpForm classes
            try
            {
                if (VtiLib.IsDualPortSystem)
                {
                    dynamic formObj = OpFormDual;
                    formObj.TestHistoryDockControl[0].ShowDocked();
                    formObj.TestHistoryDockControl[1].ShowDocked();
                }
                else
                {
                    dynamic formObj = OpFormSingle;
                    formObj.TestHistoryDockControl.ShowDocked();
                }
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteWarning("OpForm TestHistoryDockControl property not recognized");
            }
        }

        private void parameterChangeLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamChangeLog.Show();
        }

        private void manualCmdExecLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualCmdExecLog.Show();
        }

        private void saveConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveConfigGoodFile.Save();
        }

        private void cycleStepsFormToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Checks if ShowCycleSteps Mode setting exists then sets it to true
            IEditCycleParameter showCycle = null;
            if (getParameter("Mode", "ShowCycleSteps", ref showCycle))
            {
                ((BooleanParameter)showCycle).ProcessValue = true;
            }
            else
            {
                MessageBox.Show("Failed to open Cycle Steps form: Show Cycle Steps parameter not found");
            }
        }

        private void saveBarcodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string barcodesFilePath = @"C:\VTI PC\Procedures and Manuals\barcodes.pdf";
                string commandsAndParametersFilePath = @"C:\VTI PC\Procedures and Manuals\CommandsAndParameters.pdf";
                VtiLib.ManualCommands.GenerateBarcodePDF(barcodesFilePath);
                VtiLib.ManualCommands.GenerateManual(commandsAndParametersFilePath);
                MessageBox.Show($"Manual Command Barcodes saved to {barcodesFilePath}\n\nCommands and Parameter Manual Section saved to {commandsAndParametersFilePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error message: " + ex.Message);
            }
        }

        private void createShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VtiLib.CreateShortcuts();
            MessageBox.Show("Shortcuts Created.");
        }

        #endregion toolStripButtons

    }
}