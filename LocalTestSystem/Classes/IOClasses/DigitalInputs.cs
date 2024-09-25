using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace LocalTestSystem.Classes.IOClasses
{
    /// <summary>
    /// DigitalInputs class
    /// 
    /// Subclass of the IOSubSystemBase
    /// Contains fields for the digital inputs.
    /// Each field is of type IDigitalInput, which is an interface
    /// in the VTIWindowsControlLibrary.  At runtime, the IOSubSystemBase
    /// class constructor locates the digital input in the I/O Interface
    /// that matches the field name.
    /// </summary>
    public class DigitalInputs : IOSubSystemBase<IDigitalInput>
    {
        public DigitalInputs(IOInterface IOInterface)
            : base(IOInterface)
        {
            InitializePorts();
            InitializeInputs();
        }
        public IDigitalInput Acknowledge;
        public IDigitalInput MCRPower;

        public IDigitalInput VacuumPumpOilLevelSensor;

		public IDigitalInput RecoveryTankLowLevelSensor;
		public IDigitalInput RecoveryTankHighLevelSensor;
		public IDigitalInput RecoveryTankTopLevelSensor;
		public IDigitalInput SupplyTankLowLevelSensor;
		public IDigitalInput SupplyTankHighLevelSensor;
		public IDigitalInput SupplyTankTopLevelSensor;

        public IDigitalInput FillButton;
        public IDigitalInput DrainButton;

		public class DigitalInputPort
        {
            public IDigitalInput Acknowledge;
            public IDigitalInput MCRPower;
            public IDigitalInput VacuumPumpOilLevelSensor;

			public IDigitalInput RecoveryTankLowLevelSensor;
			public IDigitalInput RecoveryTankHighLevelSensor;
			public IDigitalInput RecoveryTankTopLevelSensor;
			public IDigitalInput SupplyTankLowLevelSensor;
			public IDigitalInput SupplyTankHighLevelSensor;
			public IDigitalInput SupplyTankTopLevelSensor;

			public IDigitalInput FillButton;
			public IDigitalInput DrainButton;
		}

        protected DigitalInputPort[] _port;
        public DigitalInputPort[] Port { get { return _port; } }

        protected virtual void InitializePorts()
        {
            if (Properties.Settings.Default.DualPortSystem)
            {
                //_port = new DigitalInputPort[2];
                _port[0] = new DigitalInputPort();
                _port[1] = new DigitalInputPort();
            }
            else
            {
                _port = new DigitalInputPort[1];
                _port[0] = new DigitalInputPort();
            }
        }

        protected virtual void InitializeInputs()
        {
            _port[0].Acknowledge = Acknowledge;
            _port[0].MCRPower = MCRPower;
            _port[0].VacuumPumpOilLevelSensor = VacuumPumpOilLevelSensor;
			MCRPower.ValueChanged += Powersense24VDC_ValueChanged;

			_port[0].RecoveryTankLowLevelSensor = RecoveryTankLowLevelSensor;
			_port[0].RecoveryTankHighLevelSensor = RecoveryTankHighLevelSensor;
			RecoveryTankHighLevelSensor.ValueChanged += RecoveryTankHighLevelSensor_ValueChanged;
			_port[0].RecoveryTankTopLevelSensor = RecoveryTankTopLevelSensor;

			_port[0].SupplyTankLowLevelSensor = SupplyTankLowLevelSensor;
			_port[0].SupplyTankHighLevelSensor = SupplyTankHighLevelSensor;
			SupplyTankHighLevelSensor.ValueChanged += SupplyTankHighLevelSensor_ValueChanged;
			_port[0].SupplyTankTopLevelSensor = SupplyTankTopLevelSensor;

            _port[0].FillButton = FillButton;
            _port[0].DrainButton = DrainButton;

        }


		private void SupplyTankHighLevelSensor_ValueChanged(object sender, EventArgs e) {
            if(SupplyTankHighLevelSensor.Value) {
                IO.DOut.SupplyRecircValve.Disable();
            }
		}

		private void RecoveryTankHighLevelSensor_ValueChanged(object sender, EventArgs e) {
            if(RecoveryTankHighLevelSensor.Value) {
                IO.DOut.PumpDrainIntakeValve.Disable();
            }
		}


        private void Powersense24VDC_ValueChanged(object sender, EventArgs e)
        {

            if (MyStaticVariables.AnalogInitialized & !IO.DIn.MCRPower.Value)
            {
                Machine.Cycle[0].CloseAllValves();
                IO.DOut.VacuumPumpEnable.Disable();
                IO.DOut.BlowerEnable.Disable();
                Machine.ManualCommands.Reset();
            }

        }
    }
}
