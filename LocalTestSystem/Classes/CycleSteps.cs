using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Classes.IOClasses;
using LocalTestSystem.Enums;
using LocalTestSystem.Properties;
using VTIWindowsControlLibrary;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.CycleSteps;
using VTIWindowsControlLibrary.Classes.Graphing.Util;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Enums;
using VTIWindowsControlLibrary.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace LocalTestSystem.Classes
{
    public class CycleSteps : CycleStepsBase
    {
        // Cycle Steps
        public CycleStep ReadyForSequence,
          Reset, ResetComplete,
          TestAborted, Idle,
          WaitForAcknowledge, WaitForModelSelection, ScanModelNumberOnPart, ScanSerialNumber,

          NotEnergized,
          InvalidModelNumber, Disabled,

        TestStep,
        TestStep2;

		public Boolean TestInProgress { get; set; }
        public bool bDisplayInquireForm, bAutoRunDataPlot, bdisplayMessageForm, bUpdateLanguage;
        public string bMessageFormText = "";
        public DateTime[] FlashLightTime = new DateTime[2];
        public DateTime[] ContinueToFill = new DateTime[2];

        // These local fields can be used in the case of a multi-port system, to
        // access the I/O in a non-port-specific manner.
        protected DigitalInputs.DigitalInputPort din;
        protected DigitalOutputs.DigitalOutputPort dout;
        protected AnalogSignals.AnalogSignalPort signal;
        protected int port;
        protected TestInfo test;
        protected ModelSettings model;
        protected PressureSettings pressure;
        protected ControlSettings control;
        protected ModeSettings mode;
        protected FlowSettings flow;
        protected TimeSettings time;
        protected VTIWindowsControlLibrary.Components.Graphing.DataPlotControl dataPlot;
        protected VTIWindowsControlLibrary.Components.RichTextPrompt prompt;

        public CycleSteps(int Port)
            : base(Machine.Prompt[Port], Machine.DataPlot[Port], Machine.TestHistory[Port])
        {
            din = IO.DIn.Port[Port];
            dout = IO.DOut.Port[Port];
            signal = IO.Signals.Port[Port];
            port = Port;
            PortName = Properties.Settings.Default.PortNames[port];
            model = Config.CurrentModel[port];
            pressure = Config.Pressure;
            control = Config.Control;
            mode = Config.Mode;
            flow = Config.Flow;
            time = Config.Time;
            test = Machine.Test[port];
            dataPlot = Machine.DataPlot[port];
            prompt = Machine.Prompt[port];

			TestStep = new CycleStep
			{
				DisplayElapsedTime = true,
				TimeDelay = Config.CurrentModel[0].Final_Evac_Delay
			};
			TestStep.Started += TestStep_Started;
			TestStep.Elapsed += TestStep_Elapsed;
			TestStep.Passed += TestStep_Passed;

			TestStep2 = new CycleStep
			{
				DisplayElapsedTime = true,
                Prompt = "test2"
			};
			TestStep2.Started += TestStep2_Started;
			TestStep2.Tick += TestStep2_Tick;
			TestStep2.Passed += TestStep2_Passed;

			
            
            #region common
            TestAborted = new CycleStep();

            #region ScanModelNumberOnPart
            ScanModelNumberOnPart = new CycleStep
            {
            };
            ScanModelNumberOnPart.Started += ScanModelNumberOnPart_Started;
            ScanModelNumberOnPart.Tick += ScanModelNumberOnPart_Tick;
            ScanModelNumberOnPart.Passed += ScanModelNumberOnPart_Passed;
            #endregion

            #region ScanSerialNumber
            ScanSerialNumber = new CycleStep
            {
                DisplayElapsedTime = true,
            };
            ScanSerialNumber.Started += new CycleStep.CycleStepEventHandler(ScanSerialNumber_Started);
            ScanSerialNumber.Tick += new CycleStep.CycleStepEventHandler(ScanSerialNumber_Tick);
            ScanSerialNumber.Passed += ScanSerialNumber_Passed;
            #endregion

            #region WaitForModelSelection
            WaitForModelSelection = new CycleStep
            {
                Prompt = "Waiting for model selection to start test." // Localization.WaitForModelSelectionPrompt,
            };
            WaitForModelSelection.Started += WaitForModelSelection_Started;
            WaitForModelSelection.Tick += WaitForModelSelection_Tick;
            WaitForModelSelection.Passed += WaitForModelSelection_Passed;
            #endregion

            ReadyForSequence = new CycleStep
            {
                TimeDelay = new TimeDelayParameter { ProcessValue = 0.1 }, // must be non-zero so will call OnElapsed()
                Sequence = new SequenceStep(Localization.SeqReadyForSequence)
            };
            ReadyForSequence.Elapsed += ReadyForSequence_Elapsed;

            WaitForAcknowledge = new CycleStep
            {
                Prompt = Localization.SeqWaitForReset
            };
            WaitForAcknowledge.Started += WaitForAcknowledge_Started;
            WaitForAcknowledge.Tick += WaitForAcknowledge_Tick;
            WaitForAcknowledge.Passed += WaitForAcknowledge_Passed;

            #region Reset Steps

            TestAborted = new CycleStep();

            Reset = new CycleStep();
            Reset.Started += Reset_Started;

            Idle = new CycleStep
            {
                
			};
            Idle.Started += Idle_Started;
            Idle.Tick += Idle_Tick;
            #endregion

            ResetComplete = new CycleStep();
            ResetComplete.Started += ResetComplete_Started;

            #region UpdateTime

            //UpdateTime = new CycleStep { };
            //UpdateTime.Tick += UpdateTime_Tick;

            #endregion UpdateTime


            InvalidModelNumber = new CycleStep
            {
                Color = Color.Red
            };

            #region NotEnergized

            NotEnergized = new CycleStep
            {
                Color = Color.Red,
                Prompt = Localization.MCRPowerError,
            };
            NotEnergized.Started += NotEnergized_Started;
            NotEnergized.Tick += NotEnergized_Tick;

            #endregion NotEnergized

            Disabled = new CycleStep();

            FlashLightTime = new DateTime[2];
            #endregion common

            this.Init();
        }

		private void TestStep2_Passed(CycleStep step, CycleStep.CycleStepEventArgs e)
		{
            Reset.Start();
		}

		private void TestStep2_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
		{
			if((DateTime.Now - test.startTime).TotalSeconds > model.Final_Evac_Delay)
            {
                step.Pass();
            }
		}

		private void TestStep2_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
		{
            test.startTime = DateTime.Now;
		}

		private void TestStep_Passed(CycleStep step, CycleStep.CycleStepEventArgs e)
		{
            if(test.testCounter > 4) Reset.Start();
            test.testCounter++;
            TestStep.Start();
		}

		private void TestStep_Elapsed(CycleStep step, CycleStep.CycleStepEventArgs e)
		{
            step.Pass();
		}

		private void TestStep_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
		{
            step.Prompt = "Test " + test.testCounter;
		}




        


        public void AddSequences()
        {
            //#region save original sequence text
            //test.OriginalSeqTextList = new System.Collections.Generic.List<string>();
            //foreach (SequenceStep seq in Machine.Sequences[port])
            //{
            //    test.OriginalSeqTextList.Add(seq.Text);
            //}
            //#endregion save original sequence text
        }

       

        #region UpdateTime

        private void UpdateTime_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            signal.ElapsedTime.Value = step.ElapsedTime.TotalSeconds;
        }

        #endregion UpdateTime

        #region WaitForModelSelection
        /// <summary>
        /// For use when prompting to scan a model name or prompt with the model selection form
        /// </summary>
        /// <param name="step"></param>
        /// <param name="e"></param>
        void WaitForModelSelection_Passed(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            //CycleStart();
        }

        void WaitForModelSelection_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            if (Config.CurrentModel[port].Name.ToUpper() != "DEFAULT")
            {
                step.Pass();
            }
        }

        void WaitForModelSelection_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            Machine.Prompt[port].Clear();
        }
        #endregion

        #region ScanModelNumberOnPart 
        /// <summary>
        /// For use when scanning the model number barcode on the part
        /// </summary>
        /// <param name="step"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ScanModelNumberOnPart_Passed(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            if (Config.CurrentModel[port].Load(Machine.Test[port].ModelNumberScanned))
            {
                if (Machine.Cycle[port].InvalidModelNumber.Enabled)
                    Machine.Cycle[port].InvalidModelNumber.Stop();
                
                if (Machine.Cycle[port].ScanSerialNumber.State != CycleStepState.InProcess)
                    ScanSerialNumber.Start();
            }
            else
            {
                VtiEvent.Log.WriteWarning(
                string.Format("Model Number '{0}' not found in database.", Machine.Test[port].ModelNumberScanned), VtiEventCatType.Test_Cycle);
                prompt.AppendText(string.Format("'{0}' not found in ModelXRef table.", Machine.Test[port].ModelNumberScanned) + "\n");

                if (Machine.Cycle[port].InvalidModelNumber.State != CycleStepState.InProcess)
                {
                    Machine.Cycle[port].InvalidModelNumber.Start();
                }
            }
        }

        private void ScanModelNumberOnPart_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            // If a force charge then show the select model number form
            if (Machine.Test[port].ModelNumberScanned != "" && !string.IsNullOrEmpty(Machine.Test[port].ModelNumberScanned))
            {
                Machine.Test[port].ReadyToTest = false;
                step.Pass();
            }
            if (!Machine.Test[port].ReadyToTest && Machine.Test[port].ModelNumberScanned == "") 
                step.Reset();
        }

        private void ScanModelNumberOnPart_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            step.Prompt = "Scan the compressor model number first to start a test.";
        }

        #endregion

        #region ScanSerialNumber
        protected virtual void ScanSerialNumber_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            test = Machine.Test[port] = new TestInfo();  // resets all test variables
            step.Prompt = "Scan the SERIAL NUMBER.";
        }

        void ScanSerialNumber_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
        { // Port.Blue not defined in this context and [0] does not refer to same variable
            if (Machine.Test[port].SerialNumber != "" && !string.IsNullOrEmpty(Machine.Test[port].SerialNumber))
            {
                step.Pass();
            }

            if (Config.TestMode != TestModes.Autotest)
                step.Fail();
        }
        private void ScanSerialNumber_Passed(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            CycleStart();
            //CounterLoadFillData.Start();
        }
        #endregion


        #region NotEnergized

        private void NotEnergized_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            if (IO.DIn.MCRPower.Value)
            {
                NotEnergized.Stop();
                if (Properties.Settings.Default.DualPortSystem)
                {
                    //Machine.ManualCommands.ResetBoth();
                }
                else
                {
                    Machine.ManualCommands.Reset();
                }
            }
        }

        private void NotEnergized_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            CloseAllValves();
            dout.VacuumPumpEnable.Disable();
        }

        #endregion NotEnergized

        protected virtual void ReadyForSequence_Elapsed(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            prompt.Clear();
        }

        void ATestStep_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {

        }

        private void WaitForAcknowledge_Passed(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            //dout.Alarm.Disable();
            Reset.Start();
        }
        protected virtual void WaitForAcknowledge_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            if (din.Acknowledge.Value || MyStaticVariables.Acknowledge)
            {
                MyStaticVariables.Acknowledge = false;
            
                if (MyStaticVariables.RequireAckAfterEStopReleased)
                {
                    MyStaticVariables.RequireAckAfterEStopReleased = false;
                }
                step.Pass();
            }
        }
        private void WaitForAcknowledge_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            if (MyStaticVariables.RequireAckAfterEStopReleased)
            {
                step.Prompt = "Press ACKNOWLEDGE button after disengaging E-Stop.";
            }
            else
            {
                step.Prompt = Localization.SeqWaitForReset;
                //dout.Alarm.Enable();
            }
        }

        public bool IsIdle
        {
            get
            {
                bool idle = true;

                //if (Properties.Settings.Default.DualPortSystem)
                //{
                //  if (port == 0 ? Config.Mode.BluePortEnabled : Config.Mode.WhitePortEnabled)
                //  {
                //    if (!Idle.Enabled) idle = false;
                //  }
                //}
                //else
                {
                    //if (!ScanSerialNumber2.Enabled) idle = false;
                    if (!Idle.Enabled) idle = false;
                }

                return idle;
            }
        }

        public virtual bool RequiresAcknowledge
        {
            get
            {
                return
                    this.CycleSteps.Any(s => s.State == CycleStepState.Failed) ||
                    TestAborted.State == CycleStepState.InProcess ||
                    InvalidModelNumber.State == CycleStepState.InProcess;
            }
        }

        public virtual void Acknowledge()
        {
            // If any step is failed, reset the cycle
            if (this.CycleSteps.Any(s => s.State == CycleStepState.Failed))
            {
                Reset.Start();
            }

            // check to see if this test is waiting for an acknowledge
            else if (TestAborted.State == CycleStepState.InProcess ||
                InvalidModelNumber.State == CycleStepState.InProcess)
            {
                Reset.Start();
            }
            // otherwise, abort this test
            else if (!IsIdle)
            {
                CycleAbort(Localization.TestAborted, Localization.TestAbortedTH);
                TestAborted.Start();
            }
        }
		/*
            public virtual void ReadyForDummySequence()
            {
              // start dummy Ready for Sequence cycle step
              ReadyForSequence.Start();
            }
        */

		public virtual void IdleState() {

			dout.PumpDrainIntakeValve.Disable();
			dout.RecoveryOutletValve.Disable();
			dout.MakeupIntakeValve.Disable();
			dout.SupplyOutletValve.Disable();
			dout.SupplyRecircValve.Disable();
			dout.OilFillValve.Disable();
			dout.SupplyVacuumValve.Enable();
			dout.RecoveryVacuumValve.Enable();

			dout.OilRecircPumpEnable.Disable();
            dout.VacuumPumpEnable.Enable();
            dout.BlowerEnable.Enable();
		}

		public virtual void CloseAllValves()
        {

		    dout.PumpDrainIntakeValve.Disable();
		    dout.RecoveryOutletValve.Disable();
		    dout.MakeupIntakeValve.Disable();
		    dout.SupplyOutletValve.Disable();
		    dout.SupplyRecircValve.Disable();
		    dout.OilFillValve.Disable();
		    dout.SupplyVacuumValve.Disable();
		    dout.RecoveryVacuumValve.Disable();

            dout.OilRecircPumpEnable.Disable();
	    }

		public override void CycleStart()
        {
            test.ReadyToTest = false;
            test.StartCycleTime = DateTime.Now;
            test.SupplyInProcess = true;

            Machine.Test[port].Result = Localization.NoTestTestAborted;

            VtiEvent.Log.WriteInfo("(" + PortName + ") " + String.Format(Localization.CycleStarted, test.SerialNumber),
                VtiEventCatType.Test_Cycle);

            // Reset prompt
            prompt.Clear();
            prompt.AppendText(Localization.CurrentTestMode + ": " + Config.TestMode.ToString() + Environment.NewLine);
            prompt.AppendText(
                string.Format(Localization.ModelNumber, model.Name) + Environment.NewLine);
            if (!string.IsNullOrEmpty(test.SerialNumber))
                prompt.AppendText(
                    string.Format(Localization.SerialNumberForLog, test.SerialNumber) + Environment.NewLine);
            prompt.AppendText(Environment.NewLine);

            ResetSequenceText();
            SetControlPropertyValue(prompt, "BackColor", Color.Black); //This is a thread-safe method
            // Set up UutRecord
            this.UutRecord = new UutRecord
            {
                SerialNo = test.SerialNumber,
                SystemID = control.System_ID.ProcessValue,
                ModelNo = model.Name,
                OpID = Config.OpID,
                DateTime = DateTime.Now,
                TestType = control.UutRecordTestType.ProcessValue,
                TestPort = PortName
            };
            // AutoRun the DataPlot
            if (Machine.DataPlot[port].Settings.AutoRun1)
            {
                Machine.Cycle[port].bAutoRunDataPlot = true;
            }
            //UpdateTime.Start();
            dout.LightAmber.Disable();
            dout.LightRed.Disable();
            dout.LightGreen.Disable();

        }

        public override void CycleStop()
        {
            // Stop all cycle steps
            foreach (CycleStep step in this.EnabledSteps)
            {
                if (!step.Equals(this.Idle) && step.Enabled) step.Stop();
            }
        }

        public virtual void CycleAbort(String TestResult) { CycleAbort(TestResult, TestResult); }
        public virtual void CycleAbort(String TestResult, String TestHistoryEntry)
        {
            VtiEvent.Log.WriteWarning(TestResult,
                VtiEventCatType.Test_Cycle,
                String.Format(Localization.SerialNumberForLog, test.SerialNumber));

            this.CycleStop();

            this.CycleNoTest(TestResult, TestHistoryEntry);
        }

        public virtual void CycleNoTest(String TestResult) { CycleNoTest(TestResult, TestResult); }
        public virtual void CycleNoTest(String TestResult, String TestHistoryEntry)
        {
            this.RecordCycleResults(TestResult, TestHistoryEntry, Color.Black, Color.Yellow);
            //SetControlPropertyValue(Machine.Prompt[port], "BackColor", Color.DarkGoldenrod); //This is a thread-safe method

            //MyStaticVariables.FlashBlueYellowLight = false;
            //dout.LightAmber.Enable();
            //dout.LightRed.Disable();
            //dout.LightGreen.Disable();

            //if (!test.SkipAcknowledge)
            //{
            //    WaitForAcknowledge.Start();
            //}
            //else
            //{
            //    Reset.Start();
            //}
        }

        public override void CycleNoTest(CycleStep step)
        {
            base.CycleNoTest(step);
        }

        public virtual void CycleFail(String TestResult) { CycleFail(TestResult, TestResult); }
        public virtual void CycleFail(String TestResult, String TestHistoryEntry)
        {
            this.RecordCycleResults(TestResult, TestHistoryEntry, Color.Yellow, Color.Red);
            SetControlPropertyValue(Machine.Prompt[port], "BackColor", Properties.Settings.Default.SequenceBadColor); //This is a thread safe method

            MyStaticVariables.FlashBlueYellowLight = false;
            dout.LightAmber.Disable();
            dout.LightRed.Enable();
            dout.LightGreen.Disable();

            if (!test.SkipAcknowledge)
            {
                WaitForAcknowledge.Start();
            }
            else
            {
                Reset.Start();
            }

        }

        public override void CycleFail(CycleStep step)
        {
            this.CycleStop();

            base.CycleFail(step);
        }

        public virtual void CyclePass(String TestResult) { CyclePass(TestResult, TestResult); }
        public virtual void CyclePass(String TestResult, String TestHistoryEntry)
        {
            //FlashingGreenLight.Reset();
            MyStaticVariables.FlashBlueYellowLight = false;
            //dout.LightAmber.Disable();
            //dout.LightRed.Disable();
            //dout.LightGreen.Enable();

            this.RecordCycleResults(TestResult, TestHistoryEntry, Color.Black, Color.LawnGreen);
        }

        public override void CyclePass(CycleStep step)
        {
            base.CyclePass(step);
        }

        public override void RecordCycleResults(String TestResult, String TestHistoryEntry, Color TestHistoryForeground, Color TestHistoryBackground)
        {
            // Add entry to Test History window
            if (test.SerialNumber != "")
                Machine.TestHistory[port].AddEntry(test.SerialNumber + ": " + TestHistoryEntry, TestHistoryForeground, TestHistoryBackground);
            else
                Machine.TestHistory[port].AddEntry(TestHistoryEntry, TestHistoryForeground, TestHistoryBackground);

            prompt.AppendText(TestHistoryEntry + Environment.NewLine);

            // Set the test result and write the records
            if (this.UutRecord != null)
            {
                this.UutRecord.TestResult = TestResult;
                this.RecordResults();
            }

            // Save test result for prompt
            test.Result = TestHistoryEntry;
        }

        protected virtual void Reset_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            VtiEvent.Log.WriteInfo(String.Format("({0}) {1}", PortName, Localization.ResetInitiated));

            // Stop all cycle steps
            foreach (CycleStep step2 in this.CycleSteps)
            {
                if (!step2.Equals(this.Idle))
                    step2.Reset();
            }

            prompt.Clear();

            //if (MyStaticVariables.RequireAckAfterEStopReleased)
            //{
            //    CloseAllValves();
            //    dout.VacuumPumpEnable.Disable();
            //    WaitForAcknowledge.Start();
            //    return;
            //}

            ////If Reset was called in the middle of a test, record results up to that point
            //if (test.Result == "")
            //{
            //    if (string.IsNullOrEmpty(test.SerialNumber))
            //    {
            //        //Machine.TestHistory[port].AddEntry((!string.IsNullOrEmpty(test.SerialNumber) ? (test.SerialNumber + ": ") : "")
            //            //+ "RESET", Color.Black, Color.Yellow);
            //    }
            //    else
            //    {
            //        test.SkipAcknowledge = true;
            //        CycleNoTest("RESET", "RESET");
            //        return;
            //    }
            //    VtiEvent.Log.WriteInfo("RESET",
            //        VtiEventCatType.Test_Cycle,
            //        String.Format(Localization.SerialNumberForLog, test.SerialNumber));
            //}
            ////SetControlPropertyValue(Machine.Prompt[port], "BackColor", Properties.Settings.Default.OpFormBackgroundColor);
            //#region Clear Flags
            //test.SerialNumber = "";
            //test.ModelNumberScanned = "";
            //test.ReadyToTest = false;
            //test.SupplyInProcess = false;
            //test.FeedInProcess = false; 
            //test.Result = "";
            //test.ResultTH = "";
            //test.WriteCycleNoTest = false;
            //test.WriteCyclePass = false;
            //test.WriteCycleFail = false;
            //test.SkipAcknowledge = false;
            //#endregion

            CloseAllValves();

            dout.LightGreen.Disable();


            //if (Machine.DataPlot[port].Settings.AutoRun1)
            //{
            //    if (Properties.Settings.Default.DualPortSystem)
            //    {
            //        Machine.OpFormDual.DataPlot[port].Stop();
            //    }
            //    else
            //    {
            //        Machine.OpFormSingle.DataPlot.Stop();
            //    }
            //}

            //if (IO.DIn.MCRPower.Value)
            //{
                Idle.Start();
            //}
            //else
            //{
            //    //NotEnergized.Start();
            //}
        }

        protected virtual void ResetComplete_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {

            // Reset Sequences
            foreach (var sequence in Machine.Sequences[port])
                sequence.BackColor = System.Drawing.SystemColors.Control;
            //Machine.Sequences[port][0].BackColor = Properties.Settings.Default.SequenceGoodColor;

            VtiEvent.Log.WriteInfo(Localization.ResetComplete);
          
            Machine.Cycle[port].Idle.Start();

            Machine.Cycle[port].ResetComplete.Stop();

        }

        protected virtual void Idle_Started(CycleStep step, CycleStep.CycleStepEventArgs e)
        {
            step.Prompt = string.Format(Localization.Idle_Prompt, Config.TestMode.ToString());

			IdleState(); // Closes all valves except vacuum valves and turns on vac pump
        }

        protected virtual void Idle_Tick(CycleStep step, CycleStep.CycleStepEventArgs e)
        {

            // Stop the "Idle" step if any other step starts
            // like Oil Fill, Degas Oil and Fill from supply
            foreach (CycleStep step2 in this.EnabledSteps) {
                if (step != step2) {
                    Idle.Stop();
                }
            }


		}


		private void UpdateElapsedTime()
        {
            TimeSpan ElapsedTime = (DateTime.Now - Machine.Test[Port.Blue].StartCycleTime);
            signal.ElapsedTime.Value = ElapsedTime.TotalSeconds;

            //blinking green light during a cycle
            //if (Machine.Test[Port.Blue].NoTest)
            //{
            //    dout.GreenLight.Enable();
            //    dout.RedLight.Enable();
            //}
            //else
            //{
            //    if (Machine.Test[Port.Blue].Pass)
            //    {
            //        dout.GreenLight.Enable();
            //        dout.RedLight.Disable();
            //    }
            //    else
            //    {
            //        TimeSpan LightTime = DateTime.Now-Machine.Test[Port.Blue].LightTimer;
            //        if(LightTime.TotalSeconds>0.5)
            //        {
            //            Machine.Test[Port.Blue].LightTimer = DateTime.Now;
            //            if (Machine.Test[Port.Blue].Fail)
            //            {
            //                dout.RedLight.Enable();
            //                dout.GreenLight.Disable();
            //            }
            //            else
            //            {
            //                if (!Machine.Test[Port.Blue].NoTest)
            //                {
            //                    dout.RedLight.Disable();
            //                    if(dout.GreenLight.Value)
            //                    {
            //                        dout.GreenLight.Disable();
            //                    }
            //                    else
            //                    {
            //                        dout.GreenLight.Enable();
            //                    }
            //                }
            //                else
            //                {
            //                    dout.RedLight.Disable();
            //                    dout.GreenLight.Disable();
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
        #region AddElapsedTimeToSeq

        public virtual void AddElapsedTimeToSeq(CycleStep step)
        {
            if (step.Sequence != null)
            {
                try
                {
                    //Programatically get the index of the current sequence
                    int num = -1;
                    string textToCheck = "";
                    for (int i = 0; i < Machine.Sequences[port].Count(); i++)
                    {
                        textToCheck = "";
                        if (step.Sequence.Text.Contains(" ("))
                        {
                            textToCheck = step.Sequence.Text.Substring(0, step.Sequence.Text.IndexOf(" ("));
                        }
                        else
                        {
                            textToCheck = step.Sequence.Text;
                        }
                        if (textToCheck == Machine.Sequences[port][i].Text)
                        {
                            num = i;
                            break;
                        }
                    }
                    if (num != -1)
                    {
                        Machine.Sequences[port][num].Text = textToCheck + " (" + step.ElapsedTime.TotalSeconds.ToString("0.0") + " SEC)";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        #endregion AddElapsedTimeToSeq

        #region ResetSequenceText

        public virtual void ResetSequenceText()
        {
            // remove step elapsed times from sequence text
            foreach (SequenceStep seq in Machine.Sequences[port])
            {
                string textToCheck = "";
                if (seq.Text.Contains(" ("))
                {
                    textToCheck = seq.Text.Substring(0, seq.Text.IndexOf('(') - 1);
                }
                else
                {
                    textToCheck = seq.Text;
                }
                //seq.Text = test.OriginalSeqTextList.Where(x => x == textToCheck).FirstOrDefault();
            }

            //Remove all sequences
            Machine.Sequences[port].Clear();
            //Re-add sequences based on which test steps are enabled
            AddSequences();
        }

        #endregion ResetSequenceText
        #region AddUutDetail

        public virtual void AddUutDetail(CycleStep step, AnalogSignal processSignal, int digitsToRoundSignal, NumericParameter limit, bool isMinLimit, bool isMaxLimit)
        {
            try
            {
                if (this.UutRecord != null)
                {
                    string strTest = step.Name;
                    string strResult = step.State.ToString();
                    string strValueName = processSignal != null ? processSignal.Label : string.Empty;
                    string strMinSetpointName = isMinLimit ? limit.DisplayName : string.Empty;
                    string strMaxSetpointName = isMaxLimit ? limit.DisplayName : string.Empty;
                    string strUnits = processSignal != null ? processSignal.Units : string.Empty;
                    LeftStr(ref strTest, 50); // UutRecordDetails limits this to 50 test characters
                    LeftStr(ref strResult, 50);
                    LeftStr(ref strValueName, 50);
                    LeftStr(ref strMinSetpointName, 50);
                    LeftStr(ref strMaxSetpointName, 50);
                    LeftStr(ref strUnits, 50);
                    this.UutRecord.UutRecordDetails.Add(
                        new UutRecordDetail
                        {
                            DateTime = DateTime.Now,
                            Test = strTest,
                            Result = strResult,
                            ValueName = strValueName,
                            Value = processSignal != null ? Math.Round(processSignal.Value, digitsToRoundSignal) : 0,
                            MinSetpointName = strMinSetpointName,
                            MinSetpoint = isMinLimit ? limit.ProcessValue : 0,
                            MaxSetpointName = strMaxSetpointName,
                            MaxSetpoint = isMaxLimit ? limit.ProcessValue : 0,
                            Units = strUnits,
                            ElapsedTime = Math.Round(step.ElapsedTime.TotalSeconds, 2)
                        });
                }
            }
            catch (Exception ee)
            {
                VtiEvent.Log.WriteError("Error adding UutRecordDetail after CycleStep '" + step.Name + "': " + ee.ToString());
            }
            StringBuilder sb = new StringBuilder();
            if (step.State == CycleStepState.Passed)
            {
                sb.Append("CycleStep '" + step.Name + "' passed.");
            }
            else if (step.State == CycleStepState.Failed)
            {
                sb.Append("CycleStep '" + step.Name + "' failed.");
            }
            else
            {
                sb.Append("CycleStep '" + step.Name + "' completed.");
            }
            if (processSignal != null)
            {
                sb.AppendLine(processSignal.Label + ": " + processSignal.Value);
            }
            if (isMaxLimit)
            {
                sb.AppendLine("Max limit: " + limit.DisplayName + " (" + limit.ProcessValue + ")");
            }
            else if (isMinLimit)
            {
                sb.AppendLine("Min limit: " + limit.DisplayName + " (" + limit.ProcessValue + ")");
            }
            if (step.TimeDelay != null)
            {
                sb.AppendLine("Time Delay: " + step.TimeDelay.ProcessValue);
            }
            VtiEvent.Log.WriteInfo(sb.ToString(), VtiEventCatType.Test_Cycle);
        }
        public virtual void AddUutDetail(CycleStep step, AnalogSignal processSignal, int digitsToRoundSignal, NumericParameter minLimit, NumericParameter maxLimit)
        {
            try
            {
                if (this.UutRecord != null)
                {
                    string strTest = step.Name;
                    string strResult = step.State.ToString();
                    string strValueName = processSignal != null ? processSignal.Label : string.Empty;
                    string strMinSetpointName = minLimit == null ? "" : minLimit.DisplayName;
                    string strMaxSetpointName = maxLimit == null ? "" : maxLimit.DisplayName;
                    string strUnits = processSignal != null ? processSignal.Units : string.Empty;
                    LeftStr(ref strTest, 50); // UutRecordDetails limits this to 50 test characters
                    LeftStr(ref strResult, 50);
                    LeftStr(ref strValueName, 50);
                    LeftStr(ref strMinSetpointName, 50);
                    LeftStr(ref strMaxSetpointName, 50);
                    LeftStr(ref strUnits, 50);
                    this.UutRecord.UutRecordDetails.Add(
                        new UutRecordDetail
                        {
                            DateTime = DateTime.Now,
                            Test = strTest,
                            Result = strResult,
                            ValueName = strValueName,
                            Value = processSignal != null ? Math.Round(processSignal.Value, digitsToRoundSignal) : 0,
                            MinSetpointName = strMinSetpointName,
                            MinSetpoint = minLimit == null ? 0 : minLimit.ProcessValue,
                            MaxSetpointName = strMaxSetpointName,
                            MaxSetpoint = maxLimit == null ? 0 : maxLimit.ProcessValue,
                            Units = strUnits,
                            ElapsedTime = Math.Round(step.ElapsedTime.TotalSeconds, 2)
                        });
                }
            }
            catch (Exception ee)
            {
                VtiEvent.Log.WriteError("Error adding UutRecordDetail after CycleStep '" + step.Name + "': " + ee.ToString());
            }
            StringBuilder sb = new StringBuilder();
            if (step.State == CycleStepState.Passed)
            {
                sb.Append("CycleStep '" + step.Name + "' passed.");
            }
            else if (step.State == CycleStepState.Failed)
            {
                sb.Append("CycleStep '" + step.Name + "' failed.");
            }
            else
            {
                sb.Append("CycleStep '" + step.Name + "' completed.");
            }
            if (processSignal != null)
            {
                sb.AppendLine(processSignal.Label + ": " + processSignal.Value);
            }
            if (maxLimit != null)
            {
                sb.AppendLine("Max limit: " + maxLimit.DisplayName + " (" + maxLimit.ProcessValue + ")");
            }
            if (minLimit != null)
            {
                sb.AppendLine("Min limit: " + minLimit.DisplayName + " (" + minLimit.ProcessValue + ")");
            }
            if (step.TimeDelay != null)
            {
                sb.AppendLine("Time Delay: " + step.TimeDelay.ProcessValue);
            }
            VtiEvent.Log.WriteInfo(sb.ToString(), VtiEventCatType.Test_Cycle);
        }
        public virtual void AddUutDetail(string UutDetailTestName, double doubleValue, string valueName, string units, double limit, string limitName, bool isMinLimit, bool isMaxLimit, double elapsedTime)
        {
            try
            {
                if (this.UutRecord != null)
                {
                    string strTest = UutDetailTestName;
                    string strResult = "Passed";
                    string strValueName = valueName;
                    string strMinSetpointName = isMinLimit ? limitName : string.Empty;
                    string strMaxSetpointName = isMaxLimit ? limitName : string.Empty;
                    string strUnits = units;
                    LeftStr(ref strTest, 50); // UutRecordDetails limits this to 50 test characters
                    LeftStr(ref strResult, 50);
                    LeftStr(ref strValueName, 50);
                    LeftStr(ref strMinSetpointName, 50);
                    LeftStr(ref strMaxSetpointName, 50);
                    LeftStr(ref strUnits, 50);
                    this.UutRecord.UutRecordDetails.Add(
                        new UutRecordDetail
                        {
                            DateTime = DateTime.Now,
                            Test = strTest,
                            Result = strResult,
                            ValueName = strValueName,
                            Value = doubleValue,
                            MinSetpointName = strMinSetpointName,
                            MinSetpoint = isMinLimit ? limit : 0,
                            MaxSetpointName = strMaxSetpointName,
                            MaxSetpoint = isMaxLimit ? limit : 0,
                            Units = strUnits,
                            ElapsedTime = Math.Round(elapsedTime, 2)
                        });
                }
            }
            catch (Exception ee)
            {
                VtiEvent.Log.WriteError("Error adding UutRecordDetail: " + ee.ToString());
            }
        }
        public virtual void AddUutDetail(string UutDetailTestName, string stringValue, string valueName, string units, double limit, string limitName, bool isMinLimit, bool isMaxLimit)
        {
            try
            {
                if (this.UutRecord != null)
                {
                    string strTest = UutDetailTestName;
                    string strResult = stringValue;
                    string strValueName = valueName;
                    string strMinSetpointName = isMinLimit ? limitName : string.Empty;
                    string strMaxSetpointName = isMaxLimit ? limitName : string.Empty;
                    string strUnits = units;
                    LeftStr(ref strTest, 50); // UutRecordDetails limits this to 50 test characters
                    LeftStr(ref strResult, 50);
                    LeftStr(ref strValueName, 50);
                    LeftStr(ref strMinSetpointName, 50);
                    LeftStr(ref strMaxSetpointName, 50);
                    LeftStr(ref strUnits, 50);
                    this.UutRecord.UutRecordDetails.Add(
                        new UutRecordDetail
                        {
                            DateTime = DateTime.Now,
                            Test = strTest,
                            Result = strResult,
                            ValueName = strValueName,
                            Value = 0,
                            MinSetpointName = strMinSetpointName,
                            MinSetpoint = isMinLimit ? limit : 0,
                            MaxSetpointName = strMaxSetpointName,
                            MaxSetpoint = isMaxLimit ? limit : 0,
                            Units = strUnits,
                            ElapsedTime = 0
                        });
                }
            }
            catch (Exception ee)
            {
                VtiEvent.Log.WriteError("Error adding UutRecordDetail: " + ee.ToString());
            }
        }

        #endregion AddUutDetail
    }
}

