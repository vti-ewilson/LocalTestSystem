using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using LocalTestSystem.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace LocalTestSystem.Classes.IOClasses
{
    /// <summary>
    /// DigitalOutputs class
    /// 
    /// Subclass of the IOSubSystemBase
    /// Contains fields for the digital inputs.
    /// Each field is of type IDigitalOutput, which is an interface
    /// in the VTIWindowsControlLibrary.  At runtime, the IOSubSystemBase
    /// class constructor locates the digital output in the I/O Interface
    /// that matches the field name.
    /// </summary>
    public class DigitalOutputs : IOSubSystemBase<IDigitalOutput>
    {
        public IDigitalOutput VacuumPumpEnable;
        public IDigitalOutput BlowerEnable;
		public IDigitalOutput OilRecircPumpEnable;

        public IDigitalOutput BlowerRORValve;
		public IDigitalOutput PumpDrainIntakeValve;
        public IDigitalOutput RecoveryOutletValve;
        public IDigitalOutput MakeupIntakeValve;
        public IDigitalOutput SupplyOutletValve;
        public IDigitalOutput OilPumpDriveAir;
        public IDigitalOutput SupplyRecircValve;
        public IDigitalOutput OilFillValve;
        public IDigitalOutput SupplyVacuumValve;
        public IDigitalOutput RecoveryVacuumValve;

        public IDigitalOutput LightGreen;
        public IDigitalOutput LightAmber;
        public IDigitalOutput LightRed;




        public class DigitalOutputPort
        {
			public IDigitalOutput VacuumPumpEnable;
			public IDigitalOutput BlowerEnable;
			public IDigitalOutput OilRecircPumpEnable;

            public IDigitalOutput BlowerRORValve;
			public IDigitalOutput PumpDrainIntakeValve;
			public IDigitalOutput RecoveryOutletValve;
			public IDigitalOutput MakeupIntakeValve;
			public IDigitalOutput SupplyOutletValve;
			public IDigitalOutput OilPumpDriveAir;
			public IDigitalOutput SupplyRecircValve;
			public IDigitalOutput OilFillValve;
			public IDigitalOutput SupplyVacuumValve;
			public IDigitalOutput RecoveryVacuumValve;

			public IDigitalOutput LightGreen;
			public IDigitalOutput LightAmber;
			public IDigitalOutput LightRed;

        }

		protected DigitalOutputPort[] _port;
        public DigitalOutputPort[] Port { get { return _port; } }

        public DigitalOutputs(IOInterface IOInterface)
            : base(IOInterface)
        {
            InitializePorts();
            InitializeOutputs();
        }

        protected virtual void InitializePorts()
        {
            if (Properties.Settings.Default.DualPortSystem)
            {
                _port = new DigitalOutputPort[2];
                _port[0] = new DigitalOutputPort();
                _port[1] = new DigitalOutputPort();
            }
            else
            {
                _port = new DigitalOutputPort[1];
                _port[0] = new DigitalOutputPort();
            }
        }

        protected virtual void InitializeOutputs()
        {
            _port[0].BlowerRORValve = BlowerRORValve;
			_port[0].PumpDrainIntakeValve = PumpDrainIntakeValve;
			_port[0].RecoveryOutletValve = RecoveryOutletValve;
			_port[0].MakeupIntakeValve = MakeupIntakeValve;
			_port[0].SupplyOutletValve = SupplyOutletValve;
			_port[0].OilPumpDriveAir = OilPumpDriveAir;
			_port[0].SupplyRecircValve = SupplyRecircValve;
			_port[0].OilFillValve = OilFillValve;
			_port[0].SupplyVacuumValve = SupplyVacuumValve;
			_port[0].RecoveryVacuumValve = RecoveryVacuumValve;

			_port[0].VacuumPumpEnable = VacuumPumpEnable;
            _port[0].OilRecircPumpEnable = OilRecircPumpEnable;
            _port[0].BlowerEnable = BlowerEnable;

            _port[0].LightGreen = LightGreen;
            _port[0].LightAmber = LightAmber;
            _port[0].LightRed = LightRed;

            // Examples

            //_port[0].ChamberRoughIso = ChamberRoughIso;
            //ChamberRoughIso.ValueChanging += new DigitalOutputChangingEventHandler(ChamberRoughIso_ValueChanging);
            ////_port[0].ChamberBypass = ChamberBypass;
            ////ChamberBypass.ValueChanged += new EventHandler(ChamberBypass_ValueChanged);

            // No events
            //_port[0].OvenPower = OvenPower;
            //_port[0].TurboPower = TurboPower;

            // Assign interlocks to prevent inadvertent opening of a valve
            //GPTOutletValve.ValueChanging += new DigitalOutputChangingEventHandler(GPTOutletValve_ValueChanging);
            //GPTInletValve.ValueChanging += new DigitalOutputChangingEventHandler(GPTInletValve_ValueChanging);
        }

    }
}
