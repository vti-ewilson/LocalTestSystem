using System;
using System.ComponentModel;
using System.Globalization;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Implements the Binary communication protocol for an Inficon LDS2010 Leak Detector
    /// </summary>
    public partial class InficonLDS2010Profibus : PfeifferInterface
    {
        #region Enum

        /// <summary>
        /// External Calibration Status Values
        /// </summary>
        public enum ExtCalibStatusValues
        {
            /// <summary>
            /// External calibration is inactive
            /// </summary>
            Inactive = 0,

            /// <summary>
            /// External calibration is active; calibrated leak is open
            /// </summary>
            Active = 1,

            /// <summary>
            /// External calibration is active, waiting for calibrated leak to close
            /// </summary>
            WaitingForLeakClose = 2
        }

        /// <summary>
        /// Internal Calibration Status Values
        /// </summary>
        public enum IntCalibStatusValues
        {
            /// <summary>
            /// Internal calibration is inactive
            /// </summary>
            Inactive = 0,

            /// <summary>
            /// Internal calibration is active
            /// </summary>
            Active = 1
        }

        /// <summary>
        /// Gas Ballast Modes
        /// </summary>
        public enum GasBallastModes
        {
            /// <summary>
            /// Gas ballast is off
            /// </summary>
            Off = 0,

            /// <summary>
            /// Gas ballast is on
            /// </summary>
            On = 1,

            /// <summary>
            /// Gas ballast is fixed on; unaffected by power loss
            /// </summary>
            FixedOn = 2
        }

        /// <summary>
        /// Operational Modes
        /// </summary>
        public enum OpModes
        {
            /// <summary>
            /// Vacuum mode
            /// </summary>
            Vacuum = 0,

            /// <summary>
            /// Sniff mode
            /// </summary>
            Sniff = 1,

            /// <summary>
            /// Operation mode is determined by SPS-input Vac_Sniff
            /// </summary>
            ByVacSniffInput = 2
        }

        /// <summary>
        /// Operational States
        /// </summary>
        public enum States
        {
            /// <summary>
            /// Stand-By Mode
            /// </summary>
            Standby = 0,

            /// <summary>
            /// Error
            /// </summary>
            Error = 1,

            /// <summary>
            /// Calibrating
            /// </summary>
            Cal = 2,

            /// <summary>
            /// Running Up
            /// </summary>
            RunUp = 3,

            /// <summary>
            /// Ready
            /// </summary>
            Ready = 4
        }

        /// <summary>
        /// Calibration Request Modes
        /// </summary>
        public enum CalRequestModes
        {
            /// <summary>
            /// Calibration Request Off
            /// </summary>
            CalRequestOff = 0,

            /// <summary>
            /// Calibration Request Active; Request occurs when the temperature of the preamplified changes 5 deg. C
            /// or 30 min. after switching the machine on without an executed calibration.
            /// </summary>
            CalRequestActive = 1,

            /// <summary>
            /// Read-only; Calibration request active but unavailable
            /// </summary>
            CalRequestNotAvailable = 2
        }

        /// <summary>
        /// Calibration Modes
        /// </summary>
        public enum CalModes
        {
            /// <summary>
            /// Normal Calibration
            /// </summary>
            Normal = 0,

            /// <summary>
            /// Dynamic Calibration
            /// </summary>
            Dynamic = 1,

            /// <summary>
            /// Calibration mode is determined by SPS-input CAL-MODE
            /// </summary>
            ByCalModeInput = 2
        }

        /// <summary>
        /// Analog Output Modes
        /// </summary>
        public enum AnalogOutputModes
        {
            /// <summary>
            /// <para>Leak Rate linear in chosen units</para>
            /// <para>Pin 1: Leak Rate, linear mantissa, U = 1 to 10 V</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate Exponent, U = 1 to 10 V, 0.5 V / decade, 1 V = 10E-12</para>
            /// </summary>
            LeakRateLinear = 1,

            /// <summary>
            /// <para>Leak Rate and Foreline Pressure logarithmic in chosen units</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 1 to 10 V, 0.5 V / decade, 1V = 10E-12</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Foreline Pressure, logarithmic, U = 1 to 10 V, 0.5 V / decade, 1V = 10E-3</para>
            /// </summary>
            LeakRateAndPressureLogarithmic = 2,

            /// <summary>
            /// <para>Leak Rate linear in mbar l/s or atm cc/s</para>
            /// <para>Pin 1: Leak Rate, linear mantissa, U = 1 to 10 V</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate Exponent, U = 0 to 10 V, -1 V / decade, 10 V = 10E-10</para>
            /// </summary>
            LeakRateLinearMBar = 3,

            /// <summary>
            /// <para>Leak Rate logarithmic in mbar l/s or atm cc/s and Foreline Pressure logarithmic in mbar or atm</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 10 V, 1 V / decade, 0V = 10E-10</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Foreline Pressure, logarithmic, U = 2.5 to 8.5 V, 1 V / decade, 2.5V = 10E-3</para>
            /// </summary>
            LeakRateAndPressureLogarithmicMBar = 4,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-3 to 1E+1</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-3</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-3</para>
            /// </summary>
            LeakRateLogarithmicR0 = 10,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-4 to 1E+0</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-4</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-4</para>
            /// </summary>
            LeakRateLogarithmicR1 = 11,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-5 to 1E-1</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-5</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-5</para>
            /// </summary>
            LeakRateLogarithmicR2 = 12,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-6 to 1E-2</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-6</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-6</para>
            /// </summary>
            LeakRateLogarithmicR3 = 13,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-7 to 1E-3</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-7</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-7</para>
            /// </summary>
            LeakRateLogarithmicR4 = 14,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-8 to 1E-4</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-8</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-8</para>
            /// </summary>
            LeakRateLogarithmicR5 = 15,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-9 to 1E-5</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-9</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-9</para>
            /// </summary>
            LeakRateLogarithmicR6 = 16,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-10 to 1E-6</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-10</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-10</para>
            /// </summary>
            LeakRateLogarithmicR7 = 17,

            /// <summary>
            /// <para>Leak Rate Logarithmic, Range 1E-11 to 1E-7</para>
            /// <para>Pin 1: Leak Rate, logarithmic, U = 0 to 8 V, 2 V / decade, 0 V = 1E-11</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 10 V, 3 V / decade, 0 V = 1E-11</para>
            /// </summary>
            LeakRateLogarithmicR8 = 18,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ 0</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-3, Q = 10 ^ (U - 3)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR0 = 20,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -1</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-4, Q = 10 ^ (U - 4)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR1 = 21,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -2</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-5, Q = 10 ^ (U - 5)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR2 = 22,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -3</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-6, Q = 10 ^ (U - 6)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR3 = 23,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -4</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-7, Q = 10 ^ (U - 7)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR4 = 24,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -5</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-8, Q = 10 ^ (U - 8)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR5 = 25,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -6</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-9, Q = 10 ^ (U - 9)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR6 = 26,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -7</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-10, Q = 10 ^ (U - 10)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR7 = 27,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -8</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-11, Q = 10 ^ (U - 11)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR8 = 28,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -9</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-11, Q = 10 ^ (U - 11)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR9 = 29,

            /// <summary>
            /// <para>Leak Rate Linear and Logarithmic</para>
            /// <para>Pin 1: Leak Rate, linear, U = 0 to 10 V, Q = U * 10 ^ -10</para>
            /// <para>Pin 2: GND</para>
            /// <para>Pin 3: GND</para>
            /// <para>Pin 4: Leak Rate, logarithmic, U = 0 to 4 V, 1 V / decade, 0 V = 1E-11, Q = 10 ^ (U - 11)</para>
            /// </summary>
            LeakRateLinearAndLogarithmicR10 = 30
        }

        /// <summary>
        /// Zero mode (background suppression)
        /// </summary>
        public enum ZeroModes
        {
            /// <summary>
            /// 1 - 2 decades
            /// </summary>
            Decades1to2 = 0,

            /// <summary>
            /// 2 - 3 decades
            /// </summary>
            Decades2to3 = 1,

            /// <summary>
            /// 19/20 of the raw signal
            /// </summary>
            RawSignal = 2
        }

        /// <summary>
        /// Audio Modes
        /// </summary>
        public enum AudioModes
        {
            /// <summary>
            /// Vary tone of the audio signal according to the leak rate
            /// </summary>
            Varying = 0,

            /// <summary>
            /// Steady audio signal when exceeding Trigger 1
            /// </summary>
            Steady = 1
        }

        /// <summary>
        /// Leak Rate Units
        /// </summary>
        public enum LeakRateUnits
        {
            /// <summary>
            /// mbar l/s
            /// </summary>
            mbarls = 0,

            /// <summary>
            /// Pa m^3/s
            /// </summary>
            pam3s = 1,

            /// <summary>
            /// atm cc/s
            /// </summary>
            atmccs = 2,

            /// <summary>
            /// torr l/s
            /// </summary>
            torrls = 3
        }

        /// <summary>
        /// Leak Rate Units
        /// </summary>
        public enum LeakRateSnifferUnits
        {
            /// <summary>
            /// mbar l/s
            /// </summary>
            mbarls = 0,

            /// <summary>
            /// Pa m^3/s
            /// </summary>
            pam3s = 1,

            /// <summary>
            /// atm cc/s
            /// </summary>
            atmccs = 2,

            /// <summary>
            /// torr l/s
            /// </summary>
            torrls = 3,

            /// <summary>
            /// ppm
            /// </summary>
            ppm = 4,

            /// <summary>
            /// g/a
            /// </summary>
            ga = 5
        }

        /// <summary>
        /// Pressure Units
        /// </summary>
        public enum PressureUnits
        {
            /// <summary>
            /// mbar
            /// </summary>
            mbar = 0,

            /// <summary>
            /// Pa
            /// </summary>
            pa = 1,

            /// <summary>
            /// atm
            /// </summary>
            atm = 2,

            /// <summary>
            /// torr
            /// </summary>
            torr = 3
        }

        /// <summary>
        /// Physical Units for Leak Rate and Pressure measurements
        /// </summary>
        public class PhysicalUnitsType
        {
            /// <summary>
            /// Leak Rate Units in Vacuum Mode
            /// </summary>
            public LeakRateUnits LeakRate;

            /// <summary>
            /// Leak Rate Units in Sniffer Mode
            /// </summary>
            public LeakRateSnifferUnits LeakRateSniffer;

            /// <summary>
            /// Pressure Units
            /// </summary>
            public PressureUnits Pressure;
        }

        /// <summary>
        /// Filament Emission Modes
        /// </summary>
        public enum FilamentModes
        {
            /// <summary>
            /// Emission is off
            /// </summary>
            EmissionOff = 0,

            /// <summary>
            /// Filament 1 is active, automatic switching is disabled
            /// </summary>
            Filament1 = 1,

            /// <summary>
            /// Filament 2 is active, automatic switching is disabled
            /// </summary>
            Filament2 = 2,

            /// <summary>
            /// Filament 1 is active, switching occurs automatically after filament 1 is defective
            /// </summary>
            AutoFilament1 = 3,

            /// <summary>
            /// Filament 2 is active, switching occurs automatically after filament 2 is defective
            /// </summary>
            AutoFilament2 = 4
        }

        /// <summary>
        /// Control Modes
        /// </summary>
        public enum ControlModes
        {
            /// <summary>
            /// Local (SPS/control unit)
            /// </summary>
            Local = 0,

            /// <summary>
            /// RS-232
            /// </summary>
            RS232 = 1,

            /// <summary>
            /// RS-485
            /// </summary>
            RS485 = 2
        }

        /// <summary>
        /// States of the turbo pump
        /// </summary>
        public enum TurboStates
        {
            /// <summary>
            /// Turbo is Off
            /// </summary>
            Off = 0,

            /// <summary>
            /// Turbo operating at reduced rotational speed
            /// </summary>
            ReducedSpeed = 1,

            /// <summary>
            /// Turbo operating at normal speed
            /// </summary>
            NormalSpeed = 2,

            /// <summary>
            /// Turbo is running up
            /// </summary>
            RunningUp = 3,

            /// <summary>
            /// Turbo is running down
            /// </summary>
            RunningDown = 4,

            /// <summary>
            /// An error has occurred
            /// </summary>
            Error = 5
        }

        /// <summary>
        /// Preamplifier types
        /// </summary>
        public enum Amplifiers
        {
            /// <summary>
            /// Auto, 13M
            /// </summary>
            Auto_13M = 000,

            /// <summary>
            /// Auto, 470M
            /// </summary>
            Auto_470M = 001,

            /// <summary>
            /// Auto, 15G
            /// </summary>
            Auto_15G = 002,

            /// <summary>
            /// Auto, 500G
            /// </summary>
            Auto_500G = 003,

            /// <summary>
            /// Manual, 13M
            /// </summary>
            Manual_13M = 100,

            /// <summary>
            /// Manual, 470M
            /// </summary>
            Manual_470M = 101,

            /// <summary>
            /// Manual, 15G
            /// </summary>
            Manual_15G = 102,

            /// <summary>
            /// Manual, 500G
            /// </summary>
            Manual_500G = 103
        }

        #endregion Enum

        #region Globals

        private String format = "0";

        //private String units = "atm-cc/sec";
        private double min = 1E-15, max = 1E0;

        private Double leakRate, pressure;
        private String[] leakRateUnitNames = { "mbar-l/s", "Pa-m^3/s", "atm-cc/s", "Torr-l/s", "ppm", "g/a" };
        private String[] pressureUnitNames = { "mbar", "Pa", "atm", "Torr" };

        private AnalogSignal leakRateSignal, pressureSignal;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010Profibus">InficonLDS2010Profibus</see> class
        /// </summary>
        public InficonLDS2010Profibus()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010Profibus">InficonLDS2010Profibus</see> class
        /// </summary>
        /// <param name="container">Container for this object</param>
        public InficonLDS2010Profibus(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            leakRateSignal = new AnalogSignal("LD Signal", leakRateUnitNames[0], "0.0E-0", 1E-8F, false, true);
            pressureSignal = new AnalogSignal("LD Inlet Press", pressureUnitNames[0], "0.0E-0", 1000, false, true);
        }

        #endregion Construction

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> (Rotation Speed) of the Pfeiffer Turbo Controller
        /// </summary>
        public override void Process()
        {
            //while (true)
            //{
            //    if (this.IsAvailable && !this.CommError)
            //    {
            leakRate = LeakRate;
            if (!this.CommError) pressure = Pressure;

            if (!backgroundWorker1.IsBusy && !this.CommError)
                backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
            //    }
            //    Thread.Sleep(250);
            //}
        }

        /// <summary>
        /// When called, this method invokes the <see cref="PfeifferInterface.OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            leakRateSignal.Value = leakRate;
            pressureSignal.Value = pressure;
            OnValueChanged();
        }

        /// <summary>
        /// Stops the external calibration
        /// </summary>
        public void ExtCalibStop()
        {
            SetShortIntegerParameter(649, 0);
        }

        /// <summary>
        /// Starts the external calibration
        /// </summary>
        public void ExtCalibStart()
        {
            SetShortIntegerParameter(649, 1);
        }

        /// <summary>
        /// Indicates that the leak has been closed
        /// </summary>
        public void ExtCalibClose()
        {
            SetShortIntegerParameter(649, 2);
        }

        /// <summary>
        /// Stops the internal calibration
        /// </summary>
        public void IntCalibStop()
        {
            SetShortIntegerParameter(650, 0);
        }

        /// <summary>
        /// Starts the internal calibration
        /// </summary>
        public void IntCalibStart()
        {
            SetShortIntegerParameter(650, 1);
        }

        /// <summary>
        /// Puts the leak detector in stand-by mode
        /// </summary>
        public void Standby()
        {
            SetBooleanNewParameter(653, false);
        }

        /// <summary>
        /// Puts the leak detector in measure mode
        /// </summary>
        public void Measure()
        {
            SetBooleanNewParameter(653, true);
        }

        /// <summary>
        /// Acknowledges an error
        /// </summary>
        public void AcknowledgeError()
        {
            SetBooleanNewParameter(009, true);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Name for the Pfeiffer Turbo Controller
        /// </summary>
        public override string Name
        {
            get { return "Inficon LDS2010 Profibus Leak Detector on port " + this.SerialPort.PortName + " with ID " + this.Address; }
        }

        /// <summary>
        /// Rotation Speed (%)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Value
        {
            get { return leakRate; }
            internal set { leakRate = value; }
        }

        /// <summary>
        /// Formatted value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string FormattedValue
        {
            get
            {
                if (Double.IsNaN(this.Value))
                    return "ERROR";
                else
                    return this.Value.ToString(this.Format) + " " + this.Units;
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Format string for the displayed value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Format
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Units for the displayed value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Units
        {
            get { return leakRateUnitNames[(int)this.PhysicalUnits.LeakRate]; }
            set { throw new Exception("Not implemented.  Use PhysicalUnits instead."); }
        }

        /// <summary>
        /// Minimum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// Maximum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// Leak Rate in mbar l/s, pa m^3/s, atm cc/s, tor l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single LeakRate
        {
            get { return GetExpoNewParameter(669); }
        }

        /// <summary>
        /// Leak Rate as an AnalogSignal
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogSignal LeakRateSignal
        {
            get { return leakRateSignal; }
        }

        /// <summary>
        /// Foreline pressure in mbar, pa, atm, Torr
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Pressure
        {
            get { return GetExpoNewParameter(679); }
        }

        /// <summary>
        /// Foreline pressure as an AnalogSignal
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogSignal PressureSignal
        {
            get { return pressureSignal; }
        }

        /// <summary>
        /// Leak Rate in mbar l/s
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single LeakRate_mbarls
        {
            get { return GetExpoNewParameter(670); }
        }

        /// <summary>
        /// Foreline pressure in mbar
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Pressure_mbar
        {
            get { return GetExpoNewParameter(740); }
        }

        /// <summary>
        /// Zero (background suppression)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean Zero
        {
            get { return GetBooleanNewParameter(651); }
            set { SetBooleanNewParameter(651, value); }
        }

        /// <summary>
        /// Manual (display area between limit-low and limit-high)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean Manual
        {
            get { return GetBooleanNewParameter(652); }
            set { SetBooleanNewParameter(652, value); }
        }

        /// <summary>
        /// External Calibration Status
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ExtCalibStatusValues ExtCalibStatus
        {
            get { return (ExtCalibStatusValues)GetShortIntegerParameter(649); }
        }

        /// <summary>
        /// Internal Calibration Status
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntCalibStatusValues IntCalibStatus
        {
            get { return (IntCalibStatusValues)GetShortIntegerParameter(650); }
        }

        /// <summary>
        /// Gas Ballast
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GasBallastModes GasBallast
        {
            get { return (GasBallastModes)GetShortIntegerParameter(605); }
            set { SetShortIntegerParameter(605, (ushort)value); }
        }

        /// <summary>
        /// Trigger 1 in mbar l/s, pa m^3/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Trigger1
        {
            get { return GetExpoNewParameter(681); }
            set { SetExpoNewParameter(681, value); }
        }

        /// <summary>
        /// Trigger 2 in mbar l/s, pa m^3/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Trigger2
        {
            get { return GetExpoNewParameter(683); }
            set { SetExpoNewParameter(683, value); }
        }

        /// <summary>
        /// Trigger 3 in mbar l/s, pa m^3/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Trigger3
        {
            get { return GetExpoNewParameter(685); }
            set { SetExpoNewParameter(685, value); }
        }

        /// <summary>
        /// Trigger 4 in mbar l/s, pa m^3/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Trigger4
        {
            get { return GetExpoNewParameter(687); }
            set { SetExpoNewParameter(687, value); }
        }

        /// <summary>
        /// Trigger 1 in mbar l/s
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single Trigger1_mbarls
        {
            get { return GetExpoNewParameter(682); }
            set { SetExpoNewParameter(682, value); }
        }

        /// <summary>
        /// Operational Mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OpModes OpMode
        {
            get { return (OpModes)GetShortIntegerParameter(600); }
            set { SetShortIntegerParameter(600, (ushort)value); }
        }

        /// <summary>
        /// State
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public States State
        {
            get { return (States)GetShortIntegerParameter(607); }
        }

        /// <summary>
        /// <para>ErrorCode</para>
        /// <para>
        /// <list type="bullet">
        /// <item>000000 = no error</item>
        /// <item>ErrABC = error ABC</item>
        /// <item>Wrn000 = no warning</item>
        /// <item>WrnABC = warning ABC</item>
        /// </list>
        /// </para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ErrorCode
        {
            get { return GetStringParameter(303); }
        }

        /// <summary>
        /// Limit High in mbar l/s, pa m^3/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single LimitHigh
        {
            get { return GetExpoNewParameter(689); }
            set { SetExpoNewParameter(689, value); }
        }

        /// <summary>
        /// Limit Low in mbar l/s, pa m^3/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single LimitLow
        {
            get { return GetExpoNewParameter(691); }
            set { SetExpoNewParameter(691, value); }
        }

        /// <summary>
        /// Machine Factor
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single MachineFactor
        {
            get { return GetExpoNewParameter(637); }
            set { SetExpoNewParameter(637, value); }
        }

        /// <summary>
        /// Sniff Factor
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single SniffFactor
        {
            get { return GetExpoNewParameter(638); }
            set { SetExpoNewParameter(638, value); }
        }

        /// <summary>
        /// Leak Rate for the external calibration in vacuum mode in mbar l/s, pa m^s/s, atm cc/s, torr l/s
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single LeakRateExtCalVac
        {
            get { return GetExpoNewParameter(671); }
            set { SetExpoNewParameter(671, value); }
        }

        /// <summary>
        /// Leak Rate for the external calibration in sniff mode in mbar l/s, pa m^s/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single LeakRateExtCalSniff
        {
            get { return GetExpoNewParameter(673); }
            set { SetExpoNewParameter(673, value); }
        }

        /// <summary>
        /// Leak Rate for the internal calibration in mbar l/s
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single LeakRateIntCal
        {
            get { return GetExpoNewParameter(676); }
            set { SetExpoNewParameter(676, value); }
        }

        /// <summary>
        /// Averaging filter active
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean FilterActive
        {
            get { return GetBooleanNewParameter(639); }
        }

        /// <summary>
        /// Leak rate threshold for switching the averaging time, in mbar l/s, pa m^3/s, atm cc/s, torr l/s, ppm, g/a
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single FilterSetPoint
        {
            get { return GetExpoNewParameter(677); }
            set { SetExpoNewParameter(677, value); }
        }

        /// <summary>
        /// Leak rate threshold for switching the averaging time, in mbar l/s
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single FilterSetPoint_mbarls
        {
            get { return GetExpoNewParameter(678); }
            set { SetExpoNewParameter(678, value); }
        }

        /// <summary>
        /// <para>Cal Request</para>
        /// <para>CAL request: When the temperature at the preamplifier changes 5 deg. C or
        /// 30 min. after switching the machine on without executed calibration.</para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalRequestModes CalRequest
        {
            get { return (CalRequestModes)GetShortIntegerParameter(654); }
            set { SetShortIntegerParameter(654, (ushort)value); }
        }

        /// <summary>
        /// Calibration Mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalModes CalMode
        {
            get { return (CalModes)GetShortIntegerParameter(601); }
            set { SetShortIntegerParameter(601, (ushort)value); }
        }

        /// <summary>
        /// Zero Time in 0.5 sec increments, min = 0.5, max = 30
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single ZeroTime
        {
            get { return ((Single)GetShortIntegerParameter(646)) / 2F; }
            set
            {
                if (value < 0.5) throw new Exception("Zero Time too short");
                if (value > 30) throw new Exception("Zero Time too long");
                SetShortIntegerParameter(646, (ushort)(value * 2F));
            }
        }

        /// <summary>
        /// Analog Output Mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogOutputModes AnalogOutputMode
        {
            get { return (AnalogOutputModes)GetShortIntegerParameter(602); }
            set { SetShortIntegerParameter(602, (ushort)value); }
        }

        /// <summary>
        /// Zero Mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ZeroModes ZeroMode
        {
            get { return (ZeroModes)GetShortIntegerParameter(603); }
            set { SetShortIntegerParameter(603, (ushort)value); }
        }

        /// <summary>
        /// Audio Level
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort AudioLevel
        {
            get { return GetShortIntegerParameter(640); }
            set
            {
                if (value < 1) throw new Exception("Audio level too low");
                if (value > 15) throw new Exception("Audio level too high");
                SetShortIntegerParameter(640, value);
            }
        }

        /// <summary>
        /// Audio Mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioModes AudioMode
        {
            get { return GetBooleanNewParameter(641) ? AudioModes.Steady : AudioModes.Varying; }
            set { SetBooleanNewParameter(641, (value == AudioModes.Steady)); }
        }

        /// <summary>
        /// Mass of the gas to be measured in amu
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort Mass
        {
            get { return GetShortIntegerParameter(642); }
            set
            {
                if (value < 2) throw new Exception("Mass too low");
                if (value > 4) throw new Exception("Mass too high");
                SetShortIntegerParameter(642, value);
            }
        }

        /// <summary>
        /// Units for the leak rates and pressures
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PhysicalUnitsType PhysicalUnits
        {
            get
            {
                ushort ret = GetShortIntegerParameter(643);
                PhysicalUnitsType units = new PhysicalUnitsType
                {
                    LeakRate = (LeakRateUnits)(ret / 100),
                    LeakRateSniffer = (LeakRateSnifferUnits)((ret / 10) % 10),
                    Pressure = (PressureUnits)(ret % 10)
                };
                return units;
            }
            set
            {
                int val = (int)value.LeakRate * 100 + (int)value.LeakRateSniffer * 10 + (int)value.Pressure;
                SetShortIntegerParameter(643, (ushort)val);
            }
        }

        /// <summary>
        /// Date and Time
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime DateTime
        {
            get { return DateTime.ParseExact(GetString16Parameter(718), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture); }
            set { SetString16Parameter(718, value.ToString("yyyy-MM-dd HH:mm")); }
        }

        /// <summary>
        /// Vacuum Calibration Factor
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single CalFacVac
        {
            get { return GetExpoNewParameter(635); }
            set
            {
                if (value < 1e-5) throw new Exception("Vacuum Calibration Factor too low");
                if (value > 1e6) throw new Exception("Vacuum Calibration Factor too high");
                SetExpoNewParameter(635, value);
            }
        }

        /// <summary>
        /// Sniffing Calibration Factor
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single CalFacSniff
        {
            get { return GetExpoNewParameter(636); }
            set
            {
                if (value < 1e-5) throw new Exception("Sniffing Calibration Factor too low");
                if (value > 1e6) throw new Exception("Sniffing Calibration Factor too high");
                SetExpoNewParameter(636, value);
            }
        }

        /// <summary>
        /// Filament mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FilamentModes FilamentMode
        {
            get { return (FilamentModes)GetShortIntegerParameter(645); }
            set { SetShortIntegerParameter(645, (ushort)value); }
        }

        /// <summary>
        /// Pressure threshold for warning "sniff capillary blockage" in mbar, pa, atm, torr
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single CapillaryPressureThreshold
        {
            get { return GetExpoNewParameter(693); }
            set { SetExpoNewParameter(693, value); }
        }

        /// <summary>
        /// Key Switch
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort KeySwitch
        {
            get { return GetShortIntegerParameter(608); }
        }

        /// <summary>
        /// RS-485 Address
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint RS485Address
        {
            get { return GetIntegerParameter(697); }
            set
            {
                if (value < 1) throw new Exception("RS-485 Address too low");
                if (value > 255) throw new Exception("RS-485 Address too high");
                SetIntegerParameter(697, value);
            }
        }

        /// <summary>
        /// Enables the Zero button at the sniffer probe
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean ZeroButtonEnable
        {
            get { return GetBooleanNewParameter(613); }
            set { SetBooleanNewParameter(613, value); }
        }

        /// <summary>
        /// Firmware Version
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public String FirmwareVersion
        {
            get { return GetStringParameter(312); }
        }

        /// <summary>
        /// Hours of operation
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint OpHours
        {
            get { return GetIntegerParameter(314); }
        }

        /// <summary>
        /// Control Mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ControlModes ControlMode
        {
            get { return (ControlModes)GetShortIntegerParameter(604); }
            set { SetShortIntegerParameter(604, (ushort)value); }
        }

        /// <summary>
        /// Serial Number
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SerialNumber
        {
            get { return GetString16Parameter(355); }
        }

        /// <summary>
        /// Turbo State
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TurboStates TurboState
        {
            get { return (TurboStates)GetShortIntegerParameter(611); }
        }

        /// <summary>
        /// Turbo Error Code
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TurboErrorCode
        {
            get { return GetStringParameter(626); }
        }

        /// <summary>
        /// Actual turbo rotation speed in Hz
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint TurboSpeed
        {
            get { return GetIntegerParameter(309); }
        }

        /// <summary>
        /// Hours of operation for the turbo pump
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint TurboOpHours
        {
            get { return GetIntegerParameter(627); }
        }

        /// <summary>
        /// Hours of operation for the turbo pump electronics
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint TurboDriveOpHours
        {
            get { return GetIntegerParameter(628); }
        }

        /// <summary>
        /// Turbo Firmware Version
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TurboFirmwareVersion
        {
            get { return GetStringParameter(629); }
        }

        /// <summary>
        /// Current of the turbo pump in Amps
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single TurboCurrent
        {
            get { return GetSingleParameter(310); }
        }

        /// <summary>
        /// Previous Errors
        /// </summary>
        /// <param name="ErrorNumber">Error Number (1 - 10)</param>
        /// <returns>Error Code</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PastError(ushort ErrorNumber)
        {
            if (ErrorNumber < 1 || ErrorNumber > 10) throw new Exception("Invalid ErrorNumber");
            return GetStringParameter((ushort)(360 + ErrorNumber - 1));
        }

        /// <summary>
        /// Date and Time the errors occured
        /// </summary>
        /// <param name="ErrorNumber">Error Number (1 - 10)</param>
        /// <returns></returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime ErrorDateTime(ushort ErrorNumber)
        {
            if (ErrorNumber < 1 || ErrorNumber > 10) throw new Exception("Invalid ErrorNumber");
            return DateTime.ParseExact(GetString16Parameter((ushort)(370 + ErrorNumber - 1)), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Type of preamplifier
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Amplifiers Amplifier
        {
            get { return (Amplifiers)GetShortIntegerParameter(612); }
            set { SetShortIntegerParameter(612, (ushort)value); }
        }

        /// <summary>
        /// Voltage of preamplifier in mV
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PreAmVolt
        {
            get { return GetString16Parameter(618); }
        }

        /// <summary>
        /// Name of the Device
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DeviceName
        {
            get { return GetStringParameter(349); }
        }

        /// <summary>
        /// PIN for changing the access level via software menu
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint PIN
        {
            get { return GetIntegerParameter(616); }
            set { SetIntegerParameter(616, value); }
        }

        /// <summary>
        /// Access level without key switch
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort PinLevel
        {
            get { return GetShortIntegerParameter(617); }
            set { SetShortIntegerParameter(617, value); }
        }

        /// <summary>
        /// <para>State of the 6 valves</para>
        /// <para>0 .. 64 as 6-Bit binary number (0:off, 1:on)</para>
        /// <para>
        /// <list type="bullet">
        /// <item>Bit0: V1 (test leak valve)</item>
        /// <item>Bit1: V2 (sniffer valve)</item>
        /// <item>Bit2: V3 (gas ballast valve)</item>
        /// <item>Bit3: V4 (ext. valves plug Pin 1)</item>
        /// <item>Bit4: V4 (ext. valves plug Pin 3)</item>
        /// <item>Bit5: V4 (ext. valves plug Pin 5)</item>
        /// </list>
        /// </para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort ValveTest
        {
            get { return GetShortIntegerParameter(609); }
            set { SetShortIntegerParameter(609, value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the calibrated leak valve is open.
        /// </summary>
        public Boolean CalLeakValve
        {
            get { return (ValveTest & (1 << 0)) != 0; }
            set
            {
                if (value)
                    ValveTest = (ushort)(ValveTest | (1 << 0));
                else
                    ValveTest = (ushort)(ValveTest & (0x1F ^ (1 << 0)));
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the Sniff Valve is open.
        /// </summary>
        public Boolean SniffValve
        {
            get { return (ValveTest & (1 << 1)) != 0; }
            set
            {
                if (value)
                    ValveTest = (ushort)(ValveTest | (1 << 1));
                else
                    ValveTest = (ushort)(ValveTest & (0x1F ^ (1 << 1)));
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the Gas Ballast valve is open.
        /// </summary>
        public Boolean GasBallastValve
        {
            get { return (ValveTest & (1 << 2)) != 0; }
            set
            {
                if (value)
                    ValveTest = (ushort)(ValveTest | (1 << 2));
                else
                    ValveTest = (ushort)(ValveTest & (0x1F ^ (1 << 2)));
            }
        }

        /// <summary>
        /// <para>State of the digital inputs</para>
        /// <para>0 .. 255 as 8-Bit binary number (0:off, 1:on)</para>
        /// <para>
        /// <list type="bullet">
        /// <item>Bit0: Stand-By</item>
        /// <item>Bit1: Internal Cal</item>
        /// <item>Bit2: Zero</item>
        /// <item>Bit3: External Cal</item>
        /// <item>Bit4: Clear</item>
        /// <item>Bit5: Vac/Sniff</item>
        /// <item>Bit6: Sniffer Probe Button</item>
        /// <item>Bit7: Cal Mode</item>
        /// </list>
        /// </para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort DigitalIn
        {
            get { return GetShortIntegerParameter(609); }
            set { SetShortIntegerParameter(609, value); }
        }

        /// <summary>
        /// <para>State of the digital output relays</para>
        /// <para>0 .. 511 as 9-Bit binary number (0:off, 1:on)</para>
        /// <para>
        /// <list type="bullet">
        /// <item>Bit0: Trigger 1</item>
        /// <item>Bit1: Trigger 2</item>
        /// <item>Bit2: Trigger 3</item>
        /// <item>Bit3: Trigger 4</item>
        /// <item>Bit4: Ready Relay</item>
        /// <item>Bit5: Overrange Relay</item>
        /// <item>Bit6: Cal Req Relay</item>
        /// <item>Bit7: Error Relay</item>
        /// <item>Bit8: Relay Readable (0 = normal/readable, 1 = test mode/writable)</item>
        /// </list>
        /// </para>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort RelayTest
        {
            get { return GetShortIntegerParameter(609); }
            set { SetShortIntegerParameter(609, value); }
        }

        /// <summary>
        /// Analog Output Test Mode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean AnalogOutputTestMode
        {
            get { return GetBooleanNewParameter(606); }
            set { SetBooleanNewParameter(606, value); }
        }

        /// <summary>
        /// Voltage of analog output 1 in mV
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint AnalogOut1
        {
            get { return GetIntegerParameter(647); }
            set { SetIntegerParameter(647, value); }
        }

        /// <summary>
        /// Voltage of analog output 2 in mV
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint AnalogOut2
        {
            get { return GetIntegerParameter(648); }
            set { SetIntegerParameter(648, value); }
        }

        /// <summary>
        /// Temperature of the electronics module in deg. C
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint TempElectronic
        {
            get { return GetIntegerParameter(326); }
        }

        /// <summary>
        /// Temperature of the preamplifier in deg. C
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint TempPreAmp
        {
            get { return GetIntegerParameter(619); }
        }

        /// <summary>
        /// Anode voltage in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort AnodeVoltage
        {
            get { return GetShortIntegerParameter(620); }
        }

        /// <summary>
        /// Cathode voltage in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort CathodeVoltage
        {
            get { return GetShortIntegerParameter(621); }
        }

        /// <summary>
        /// Suppressor voltage in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort SuppressorVoltage
        {
            get { return GetShortIntegerParameter(622); }
        }

        /// <summary>
        /// Power supply voltage for remote control in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single PanelVoltage
        {
            get { return GetSingleParameter(623); }
        }

        /// <summary>
        /// Power supply voltage for Digital I/O in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single DigIOVoltage
        {
            get { return GetSingleParameter(624); }
        }

        /// <summary>
        /// Preamplifier signal at A/D converter in mV
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Single PreampSignal
        {
            get { return GetSingleParameter(625); }
        }

        /// <summary>
        /// Stored anode voltage for mass 2 in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort AnodeVoltage_M2
        {
            get { return GetShortIntegerParameter(631); }
        }

        /// <summary>
        /// Stored anode voltage for mass 3 in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort AnodeVoltage_M3
        {
            get { return GetShortIntegerParameter(632); }
        }

        /// <summary>
        /// Stored anode voltage for mass 4 in V
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort AnodeVoltage_M4
        {
            get { return GetShortIntegerParameter(633); }
        }

        #endregion Public Properties
    }
}