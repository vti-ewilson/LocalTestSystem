using System;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Enums;

namespace LocalTestSystem.Classes.IOClasses
{
    /// <summary>
    /// AnalogSignals class
    /// 
    /// Subclass of the IOSubSystemBase class
    /// Contains fields for the analog signals.  The Analog Signals
    /// are the "real" signals (i.e. psi, torr, cc/sec, etc).
    /// This class also contains an example of a subclass "AnalogSignalPort" that
    /// can be used for multi-port systems, such as a PD
    /// </summary>
    public class AnalogSignals : AnalogSignalCollection
    {
        

        public AnalogSignal VacuumManifold100PSIGTransducerVolts; // Analog
		public AnalogSignal VacuumManifold100PSIGTransducer;

		public AnalogSignal CDG10TorrPressureSensorVolts;  // Analog
		public AnalogSignal CDG10TorrPressureSensor;
		public AnalogSignal CDGVariableUnits;

		public AnalogSignal ConvecCDGRatio;

        public AnalogSignal BlueElapsedTime;

		public class AnalogSignalPort
        {
            public AnalogSignal ElapsedTime;

			public AnalogSignal VacuumManifold100PSIGTransducerVolts;
			public AnalogSignal VacuumManifold100PSIGTransducer;

			public AnalogSignal CDG10TorrPressureSensorVolts;
			public AnalogSignal CDG10TorrPressureSensor;

			public AnalogSignal ConvecCDGRatio;

		}

		// Array for the analog signal "ports"
		protected AnalogSignalPort[] _port;
        public AnalogSignalPort[] Port { get { return _port; } }

        // Constructor for AnalogSignals
        // Calls the base class constructor
        // Creates the analog signal ports, and assigns the FillPressure
        // field for each
        public AnalogSignals()
        {
            InitializePorts();
            InitializeSignals();
        }

        protected virtual void InitializePorts()
        {
            _port = new AnalogSignalPort[2];
            _port[0] = new AnalogSignalPort();
            _port[1] = new AnalogSignalPort();
        }

        protected virtual void InitializeSignals()
        {


            // 18890, Displayed

			VacuumManifold100PSIGTransducerVolts = new AnalogSignal("Vacuum Manifold Transducer Pressure Volts", "PSIG", "0.000", 1f, false, true, IO.AIn.VacuumManifold100PSIGTransducerVolts, IO.SignalConverters.ExampleConverter);
			VacuumManifold100PSIGTransducerVolts.ValueChanged += VacuumManifold100PSIGTransducerVolts_ValueChanged;
            VacuumManifold100PSIGTransducer = new AnalogSignal("100 PSIG Pressure Transducer", "PSIG", "0.00", 50F, true, true);

			CDG10TorrPressureSensorVolts = new AnalogSignal("CDG 10 Torr Pressure Volts", "Torr", "0.000", 1f, false, true, IO.AIn.CDG10TorrPressureSensorVolts, IO.SignalConverters.ExampleConverter);
			CDG10TorrPressureSensorVolts.ValueChanged += CDG10TorrPressureSensorVolts_ValueChanged;
			CDG10TorrPressureSensor = new AnalogSignal("Evac Manifold CDG", "Torr", "0.000", 1000F, true, true);
			CDGVariableUnits = new AnalogSignal("Evac Manifold CDG", "microns", "0", 1000F, true, true);

			

			ConvecCDGRatio = new AnalogSignal("Convec/CDG Ratio", "", "0.0", 100F, false, true);

			BlueElapsedTime = new AnalogSignal("Elapsed Time", "sec", "0.0", 0, false, true);

            _port[0].ElapsedTime = BlueElapsedTime;
        }

		private void CDG10TorrPressureSensorVolts_ValueChanged(object sender, EventArgs e)
		{
            // If < 1 Torr display as microns, else Torr
            double val = ((CDG10TorrPressureSensorVolts.RawValue / 10.0) * 10) - 0;
            CDG10TorrPressureSensor.Value = val;
			if(val < 1) {
				CDGVariableUnits.Value = val * 1000;
				CDGVariableUnits.Units = "microns";
				CDGVariableUnits.Format = "0";

			} else {
				CDGVariableUnits.Value = val;
				CDGVariableUnits.Units = "Torr";
				CDGVariableUnits.Format = "0.0";
			}
		}

		private void VacuumManifold100PSIGTransducerVolts_ValueChanged(object sender, EventArgs e)
		{
			VacuumManifold100PSIGTransducer.RawValue = VacuumManifold100PSIGTransducerVolts.RawValue;
			VacuumManifold100PSIGTransducer.Value = VacuumManifold100PSIGTransducerVolts.RawValue;
		}

        

    }
}
