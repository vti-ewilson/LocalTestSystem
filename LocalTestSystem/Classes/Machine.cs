using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq.Expressions;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using LocalTestSystem.Classes;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Classes.IOClasses;
using LocalTestSystem.Enums;
using LocalTestSystem.Forms;
using VTIWindowsControlLibrary;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.ManualCommands;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Components.Graphing;
using VTIWindowsControlLibrary.Enums;
using VTIWindowsControlLibrary.Forms;
using VTIWindowsControlLibrary.Interfaces;

namespace LocalTestSystem
{
    public class Machine : GenericSingleton<Machine>, IMachine
    {
        protected Machine() { }

        public static void Initialize()
        {
            Instance = new Machine();

        }

        protected OperatorFormDualNested2 _OpFormDual;
        protected OperatorFormSingleNested _OpFormSingle;
        protected RichTextPrompt[] _Prompt;
        protected SequenceStepsControl.SequenceStepList[] _Sequences;
        protected DataPlotControl[] _DataPlot;
        protected DataPlotDockControl[] _DataPlotDockControl;
        protected TestHistoryControl[] _TestHistory;
        protected ValvesPanelControl[] _ValvesPanel;
        protected SystemSignalsControl[] _SystemSignals;
        protected ResourceManager _LocalizationResource;
        protected ResourceManager _Resources;
        protected CultureInfo _EnglishCulture;
        protected CultureInfo _FrenchCulture;
        protected CultureInfo _SpanishCulture;
        protected ManualCommands _ManualCommands;
        protected CycleSteps[] _Cycle;
        protected TestInfo[] _Test;
        protected SchematicFormBase _Schematic;
        private string scannerText;
        protected MainForm _MainForm;
        protected CycleStepsActiveForm _CycleStepsActiveForm;
        protected RichTextBox _RichTextBox;

        public static OperatorFormDualNested2 OpFormDual { get { return Instance._OpFormDual; } }
        public static OperatorFormSingleNested OpFormSingle { get { return Instance._OpFormSingle; } }

        public static RichTextPrompt[] Prompt { get { return Instance._Prompt; } }
        public static SequenceStepsControl.SequenceStepList[] Sequences { get { return Instance._Sequences; } }
        public static DataPlotControl[] DataPlot { get { return Instance._DataPlot; } }
        public static DataPlotDockControl[] DataPlotDockControl { get { return Instance._DataPlotDockControl; } }
        public static TestHistoryControl[] TestHistory { get { return Instance._TestHistory; } }
        public static ValvesPanelControl[] ValvesPanel { get { return Instance._ValvesPanel; } }
        public static SystemSignalsControl[] SystemSignals { get { return Instance._SystemSignals; } }

        public static ResourceManager LocalizationResource { get { return Instance._LocalizationResource; } }
        public static ResourceManager Resources { get { return Instance._Resources; } }
        public static CultureInfo EnglishCulture { get { return Instance._EnglishCulture; } }
        public static CultureInfo FrenchCulture { get { return Instance._FrenchCulture; } }
        public static CultureInfo SpanishCulture { get { return Instance._SpanishCulture; } }
        public static ManualCommands ManualCommands { get { return Instance._ManualCommands; } }
        public static CycleSteps[] Cycle { get { return Instance._Cycle; } }
        public static TestInfo[] Test { get { return Instance._Test; } }
        public static SchematicFormBase Schematic { get { return Instance._Schematic; } }
        public static MainForm MainForm { get { return Instance._MainForm; } }
        public ResourceManager LocalizationInstance { get { return _LocalizationResource; } }
        public IManualCommands ManualCommandsInstance { get { return _ManualCommands; } }
        public static CycleStepsActiveForm CycleStepsForm { get { return Instance._CycleStepsActiveForm; } }

        private bool useReadExistingForScanner;

        protected override void InitializeInstance()
        {
            SplashScreen.Show(Application.ProductName, Application.ProductVersion, VtiLib.AssemblyCopyright + "  " + Application.CompanyName);
            VtiEvent.Log.WriteInfo("Initializing System...");

            _MainForm = Program.MainForm;

            InitializeResources();
            InitializeConfiguration();
            InitializeParameters();
            InitializeEventLog();
            InitializeManualCommands();
            InitializeLibrary();
            InitializeOperatorForm(false);
            InitializeTestInfo();
            InitializeSchematic();
            InitializeDatabase();
            InitializeIoInterface();
            InitializeDataPlot();
            InitializeSystemSignals();
            InitializeValvesPanel();
            InitializeBarcodeScanner();
            InitializeCycleSteps();
            HideCommands();

            // Initial mode is "logged off"
            Config.TestMode = TestModes.Logoff;

            VtiEvent.Log.WriteVerbose("Done Initializing Machine...");
            SplashScreen.Hide();
        }

        protected virtual void InitializeResources()
        {
            SplashScreen.Message = "Initializing Machine...";
            VtiEvent.Log.WriteVerbose("Initializing Machine...");
            // Need to initialize the Resource Manager early on, since other things use it
            _LocalizationResource =
                new ChainableResourceManager(VtiLib.StandardMessages, "LocalTestSystem.Localization", System.Reflection.Assembly.GetExecutingAssembly());
            _Resources = new ResourceManager("LocalTestSystem.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            _EnglishCulture = new CultureInfo("en-US");
            _FrenchCulture = new CultureInfo("fr-FR");
            _SpanishCulture = new CultureInfo("es-ES");
        }

        protected virtual void InitializeConfiguration()
        {
            // Initialize the Configuration
            SplashScreen.Message = "Initializing Configuration...";
            Config.Initialize();
        }

        protected virtual void InitializeParameters()
        {
            Config.Control.Language.Visible = false;
            Config.Mode.ShowCycleSteps.Visible = true;
        }

        protected virtual void InitializeEventLog()
        {
            // Set the log level
            SplashScreen.Message = "Initializing Event Log...";
            VtiEvent.Log.WriteToSimpleTextFile = true;
            VtiEvent.Log.DaysToKeepOldVtiEventLogTextFiles = 0;
            VtiEvent.Log.Level = Config.Mode.Trace_Level; // first place we can safely set this
            VtiEvent.Log.WriteVerbose("Event Viewer Trace Level set to '" + VtiEvent.Log.Level.ToString() + "'.");
        }

        protected virtual void InitializeLibrary()
        {
            SplashScreen.Message = "Initializing Library...";

			switch(Config.Mode.DatabaseMode.ProcessValue)
			{
				case DatabaseOptions.Local:
					VtiLib.Initialize<Machine, Config, ManualCommands, ModelSettings, IOSettings>
                        (Machine.Instance, Config.Instance, Properties.Settings.Default.VtiDataConnectionString);
                    break;
				case DatabaseOptions.Remote2:
					VtiLib.Initialize<Machine, Config, ManualCommands, ModelSettings, IOSettings>
                        (Machine.Instance, Config.Instance, Properties.Settings.Default.VtiDataConnectionString, Config.Control.VtiConnectionString2.ProcessValue);
                    break;
				//case DatabaseOptions.Remote3:
				//	string s = Config.Control.VtiConnectionString3.ProcessValue;
				//	VtiLib.Initialize<Machine, Config, ManualCommands, ModelSettings, IOSettings>
				//			(Machine.Instance, Config.Instance, Properties.Settings.Default.VtiDataConnectionString, s, s, s, s, "test");
				//	break;
				default:
					VtiLib.Initialize<Machine, Config, ManualCommands, ModelSettings, IOSettings>
						(Machine.Instance, Config.Instance, Properties.Settings.Default.VtiDataConnectionString);
					break;
			}
        }

        protected virtual void InitializeTestInfo()
        {
            _Test = new TestInfo[2];
            _Test[0] = new TestInfo();
            _Test[1] = new TestInfo();
        }

        protected virtual void InitializeManualCommands()
        {
            SplashScreen.Message = "Initializing Manual Commands...";
            _ManualCommands = new ManualCommands();
        }

        protected virtual void HideCommands()
        {
            _ManualCommands.HideCommand(_ManualCommands.English);
            _ManualCommands.HideCommand(_ManualCommands.Spanish);

            Machine.ManualCommands.UpdateCommands();
        }

        protected virtual void InitializeSchematic()
        {
            SplashScreen.Message = "Initializing Schematic...";
            if (Properties.Settings.Default.DualPortSystem)
            {
                _Schematic = new SchematicFormDual();
            }
            else
            {
                _Schematic = new SchematicForm();
            }
            _CycleStepsActiveForm = new CycleStepsActiveForm();
        }

        protected virtual void InitializeDatabase()
        {
            SplashScreen.Message = "Initializing Database...";
            if (!VtiLib.Data.CheckConnStatus())
            {
                VtiEvent.Log.WriteError("Error initializing database", VtiEventCatType.Application_Error, Properties.Settings.Default.VtiDataConnectionString);
                MessageBox.Show("Can not connect to VTIData.mdf " + Properties.Settings.Default.VtiDataConnectionString, "VTIData.mdf", MessageBoxButtons.OK);
            }
            if (Config.Mode.RemoteVTIDataconnection2Enable.ProcessValue)
            {
                SplashScreen.Message = "Initializing Database 2...";
                if (!VtiLib.Data2.CheckConnStatus2())
                {
                    //Use control parameter for connection string
                    VtiEvent.Log.WriteError("Error initializing database", VtiEventCatType.Application_Error, Config.Control.VtiConnectionString2.ProcessValue);
                    MessageBox.Show("Can not Connect to remote VTIData.mdf " + Config.Control.VtiConnectionString2.ProcessValue, "Remote VTIData.mdf", MessageBoxButtons.OK);
                    //disable the mode and re-initialize the library without the remote VtiData connection to avoid more remote database connection errors in app
                    Config.Mode.RemoteVTIDataconnection2Enable.ProcessValue = false;
                    Config.Save();
                    InitializeLibrary();
                }
            }
        }

        protected virtual void InitializeIoInterface()
        {
            SplashScreen.Message = "Initializing I/O Interface...";
            Config.IO.Interface.Start();
            IO.Initialize();
        }
        
        protected virtual void InitializeDataPlot()
        {
            // Add analog inputs to the data plot
            SplashScreen.Message = "Initializing Data Plot...";

            _DataPlot[Port.Blue].Traces.AddAnalogSignal(IO.Signals.CDG10TorrPressureSensor);
            _DataPlot[Port.Blue].Traces.AddAnalogSignal(IO.Signals.VacuumManifold100PSIGTransducer);
			_DataPlot[Port.Blue].Traces.AddAnalogSignal(IO.Signals.ConvecCDGRatio);

			_DataPlot[Port.Blue].AutoRun1Visible = false;
            _DataPlot[Port.Blue].AutoRun2Visible = false;
            _DataPlot[Port.Blue].Settings.DrawPlotCursorCallouts = false;
            _DataPlot[Port.Blue].LocalDropDownMenus = true;
            SystemSignals[Port.Blue].DataPlotControl = DataPlot[Port.Blue];
        }

        protected virtual void InitializeValvesPanel()
        {
            //_ValvesPanel[Port.Blue].AddDigitalSignal(IO.DOut.LN2OutletValve);
            ValvesPanel[Port.Blue].UpdateValvesPanel();
        }

        protected virtual void InitializeSystemSignals()
        {
			// Left Side is Blue, Primary

			_SystemSignals[Port.Blue].AddAnalogSignal(IO.Signals.CDGVariableUnits);
			_SystemSignals[Port.Blue].AddAnalogSignal(IO.Signals.VacuumManifold100PSIGTransducer);

			MyStaticVariables.AnalogInitialized = true;
        }

        protected virtual void InitializeBarcodeScanner()
        {
            SplashScreen.Message = "Initializing Barcode Scanner...";
            if (Config.Mode.BarcodeScannerEnabled.ProcessValue)
            {
                //IO.SerialIn.Scanner.SerialPort.DataReceived += BarcodeScanner_DataReceived;
            }
        }

        protected virtual void InitializeCycleSteps()
        {
            _Cycle = new CycleSteps[3];
            _Cycle[Port.Blue] = new CycleSteps(Port.Blue);

			_Cycle[Port.Blue].Start();
            //_Sequences[0][0].BackColor = Properties.Settings.Default.SequenceGoodColor;
            MainForm.timerSlidePanels.Interval = 100;
            MainForm.timerSlidePanels.Enabled = true;

            if (Properties.Settings.Default.DualPortSystem)
            {

                _Cycle[Port.Blue].NewlineAfterPrompt = true;
            }
        }

        protected virtual void BarcodeScanner_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (useReadExistingForScanner)
            {
                scannerText = IO.SerialIn.Scanner.SerialPort.ReadExisting();
            }
            else
            {
                try
                {
                    scannerText = IO.SerialIn.Scanner.SerialPort.ReadLine();
                    useReadExistingForScanner = false;
                }
                catch
                {
                    scannerText = IO.SerialIn.Scanner.SerialPort.ReadExisting();
                    useReadExistingForScanner = true;
                }
            }

            if (!string.IsNullOrEmpty(scannerText))
            {
                // throw away any leading CR or LF
                while (scannerText.Substring(0, 1) == "\r" || scannerText.Substring(0, 1) == "\n")
                    scannerText = scannerText.Substring(1);
                // if scannerText contains a CR, process it
                if (scannerText.Contains("\r"))
                {
                    ParseScannerText(scannerText.Substring(0, scannerText.IndexOf("\r")));
                    scannerText = string.Empty;
                }
            }
        }

        protected virtual void InitializeOperatorForm(bool ShowForm)
        {
            SplashScreen.Message = "Initializing Operator Form...";
            _RichTextBox = MainForm.ScannerText;
            _RichTextBox.TextChanged += _RichTextBox_TextChanged;

            if (Properties.Settings.Default.DualPortSystem)
            {
                _OpFormDual = new OperatorFormDualNested2(Properties.Settings.Default.PortNames, Properties.Settings.Default.PortColors, _MainForm);
                _OpFormDual.CommandWindowsVisible = false;

                _Prompt = _OpFormDual.Prompt;
                _Sequences = _OpFormDual.Sequences;
                _DataPlot = _OpFormDual.DataPlot;
                _DataPlotDockControl = _OpFormDual.DataPlotDockControl;
                _TestHistory = _OpFormDual.TestHistory;
                _ValvesPanel = _OpFormDual.ValvesPanel;
                _SystemSignals = _OpFormDual.SystemSignals;

                _OpFormDual.SystemSignalPanelWidth = 225;//Properties.Settings.Default.SystemSignalPanelWidth;

                _Prompt[Port.Blue].DefaultFont = new Font("Arial", 11, FontStyle.Regular);
                _SystemSignals[Port.Blue].SignalCaptionFont = new Font("Arial", 14);//Properties.Settings.Default.SystemSignalCaptionFont;
                _SystemSignals[Port.Blue].SignalValueFont = new Font("Arial", 14);//Properties.Settings.Default.SystemSignalValueFont;
                _SystemSignals[Port.Blue].SignalCaptionWidth = _OpFormDual.SystemSignalPanelWidth - 10;//Properties.Settings.Default.SystemSignalPanelWidth;
                _TestHistory[Port.Blue].Width = _OpFormDual.SystemSignalPanelWidth - 10;
                _TestHistory[Port.Blue].LabelSize = new Size(_OpFormDual.SystemSignalPanelWidth, 15);

                if (_OpFormSingle != null)
                {
                    _OpFormSingle.Hide();
                    _OpFormSingle.Dispose();
                }

                if (ShowForm) _OpFormDual.Show();
            }
            else
            {
                _OpFormSingle = new OperatorFormSingleNested(Properties.Settings.Default.PortNames[0], Properties.Settings.Default.PortColors[0], _MainForm);
                _OpFormSingle.CommandWindowVisible = false;
                _OpFormSingle.FlowRateVisible = false;
                _OpFormSingle.SignalIndicator.Visible = false;
                _OpFormSingle.PortNameVisible = false;

                _OpFormSingle.SystemSignals.refreshTimerInterval = 50;

                _Prompt = new RichTextPrompt[1];
                _Prompt[0] = _OpFormSingle.Prompt;

                _Sequences = new SequenceStepsControl.SequenceStepList[1];
                _Sequences[0] = _OpFormSingle.Sequences;

                _DataPlot = new DataPlotControl[1];
                _DataPlot[0] = _OpFormSingle.DataPlot;

                _DataPlotDockControl = new DataPlotDockControl[1];
                _DataPlotDockControl[0] = _OpFormSingle.DataPlotDockControl;

                _TestHistory = new TestHistoryControl[1];
                _TestHistory[0] = _OpFormSingle.TestHistory;

                _ValvesPanel = new ValvesPanelControl[1];
                _ValvesPanel[0] = _OpFormSingle.ValvesPanel;

                _SystemSignals = new SystemSignalsControl[1];
                _SystemSignals[0] = _OpFormSingle.SystemSignals;

                _SystemSignals[0].SignalCaptionFont = new Font("Arial", 14);//Properties.Settings.Default.SystemSignalCaptionFont;
                _SystemSignals[0].SignalValueFont = new Font("Arial", 14);//Properties.Settings.Default.SystemSignalValueFont;
                _SystemSignals[0].SignalCaptionWidth = 300;//Properties.Settings.Default.SystemSignalPanelWidth;
                _OpFormSingle.SystemSignalPanelWidth = 300;//Properties.Settings.Default.SystemSignalPanelWidth;
                _TestHistory[Port.Blue].Width = _OpFormSingle.SystemSignalPanelWidth - 10;
                _TestHistory[Port.Blue].LabelSize = new Size(_OpFormSingle.SystemSignalPanelWidth, 15);

                if (_OpFormDual != null)
                {
                    _OpFormDual.Hide();
                    _OpFormDual.Dispose();
                }

                _Prompt[Port.Blue].DefaultFont = new Font("Arial", 14, FontStyle.Regular);

                if (ShowForm) _OpFormSingle.Show();
            }
        }

        private void _RichTextBox_TextChanged(object sender, EventArgs e)
        {
            string TempText = _RichTextBox.Text;//.ToUpper();
            if (TempText.Contains("\n"))
            {
                _RichTextBox.Text = "";
                ParseScannerText(TempText);
            }
        }

        /// <summary>
        /// Use the ModelXRef to assign different model numbers scanned to one set of model parameters in edit cycle 
        /// Reduce the number of recipes need when ever possible, easier on administrtion of parameters
        /// /// </summary>
        /// <param name="ParsedScannedCharacters"></param>
        /// <returns></returns>
        private queryResultModelXRef SelectedModelFromModelXRef(string ParsedScannedCharacters)
        {
            string sqlCmd = "";
            queryResultModelXRef qrReturn = null;
            try
            {
                DataContext db = new DataContext(Properties.Settings.Default.VtiDataConnectionString);
                sqlCmd = "select * from ModelXRef";
                IEnumerable<queryResultModelXRef> flagtest = db.ExecuteQuery<queryResultModelXRef>(sqlCmd);
                foreach (queryResultModelXRef qr in flagtest)
                {
                    if (ParsedScannedCharacters.Contains(qr.ScannedChars))
                    {
                        qrReturn = qr;
                    }
                }
                db.Connection.Close();
                db.Dispose();
                return qrReturn;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError(
                  string.Format("An error reading from database."),
                  VtiEventCatType.Database, e.ToString());
                return qrReturn;
            }
        }
     
        public virtual void ParseScannerText(string ScannerText)
        {
            String TempScannerText;
            String EditCycleModel = "";
            bool bWaitingONSerial = false
                , bWaitingOnModel = false
                , bInvalidModel = false;

            int Port = 0; // Port.Blue;
            if (Machine.Cycle[0].ScanSerialNumber.State == CycleStepState.InProcess)
            {
                Port = 0;
                bWaitingONSerial = true;
            }

            TempScannerText = ScannerText.Trim();
            VtiEvent.Log.WriteVerbose("ParseScannerText: " + TempScannerText);

            if (_ManualCommands.CheckForCommand(TempScannerText))
            {
                _ManualCommands.ExecuteCommand(TempScannerText);
            }
            else
            {
                // Check to see if the ScannerText matches the SerialNumberPattern regex
                //System.Text.RegularExpressions.Regex rSn = new System.Text.RegularExpressions.Regex(Config.Control.SerialNumberPattern);
                //System.Text.RegularExpressions.Match mSn = rSn.Match(TempScannerText);
                //System.Text.RegularExpressions.Regex rMn = new System.Text.RegularExpressions.Regex(Config.Control.ModelNumberPattern);
                //System.Text.RegularExpressions.Match mMn = rMn.Match(TempScannerText);

                //if (mMn.Success && bWaitingOnModel && !bInvalidModel)
                //{
                //    Machine.Test[Port].ModelNumberScanned = EditCycleModel;
                //}

                //if (mSn.Success && bWaitingONSerial)
                //{
                //    Machine.Test[Port].SerialNumber = TempScannerText;
                //}
            }
        }

        /// <summary>
        /// ConfigChanged
        /// Called when the user closes the Edit Cycle window to process any
        /// changes to the config that need to have an immediate effect.
        /// </summary>
        public virtual void ConfigChanged(bool serialParamsChanged)
        {
            (_MainForm.Controls["statusStrip1"] as StatusStrip).Items["SystemID"].Text = Config.Control.System_ID.ProcessValue.ToString();

            // Reload models on setting change
            for (int i = 0; i < (Properties.Settings.Default.DualPortSystem ? 2 : 1); i++)
            {
                if (Config.CurrentModel[i].Name == "Default")
                    Config.CurrentModel[i].LoadFrom(Config.DefaultModel);
                else
                    Config.CurrentModel[i].Load(Config.CurrentModel[i].Name);
            }
            if (serialParamsChanged)
            {
                IO.Instance.RestartSerialInDevices();
            }
        }
    }
}
