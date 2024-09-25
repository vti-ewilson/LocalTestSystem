using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.FormatProviders;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;
using VTIWindowsControlLibrary.Classes.Configuration;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    public class Fault
    {
        public int ID;
        public string Name;
        public string Message;
        public Fault() { }
        public Fault(int id, string name, string message = "")
        {
            ID = id;
            Name = name;
            Message = message;
            if (message == "")
                Message = name;
        }
    }
    public partial class SorensenAsterion : SerialIOBase
    {
        public override event EventHandler ValueChanged;
        protected override void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        public override event EventHandler RawValueChanged;
        protected override void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        public event EventHandler FaultOccurred;
        protected void OnFaultOccurred()
        {
            if (FaultOccurred != null)
                FaultOccurred(this, null);
        }

        private double v;
        public override double Value { get => voltage; internal set => v = value; }

        public override double RawValue { get => current; }

        public override double Min => throw new System.NotImplementedException();

        public override double Max => throw new System.NotImplementedException();

        public override string Units { get => voltageUnits; set => throw new System.NotImplementedException(); }
        public override string Format { get => ""; set => throw new System.NotImplementedException(); }

        private double voltage = double.NaN;
        private double voltageProgram = 0;
        private string voltageUnits = " VDC ";
        private bool voltageSet = false;
        private bool voltageReady = false;
        private string voltageGetCommand = "MEAS:VOLT?";
        private string voltageSetCommand = "SOUR:VOLT";

        private double current = double.NaN;
        private double currentProgram = 0;
        private string currentUnits = " Amps ";
        private bool currentSet = false;
        private bool currentReady = false;
        private string currentGetCommand = "MEAS:CURR?";
        private string currentSetCommand = "SOUR:CURR";

        private double power = double.NaN;
        private double powerProgram = 0;
        private string powerUnits = " Watts ";
        private bool powerSet = false;
        private bool powerReady = false;
        private string powerGetCommand = "MEAS:POW?";
        private string powerSetCommand = "SOUR:POW";

        private string clearCommand = "*CLS";
        private bool clearErrors = false;
        private string identifyCommand = "*IDN?";
        private string resetCommand = "*RST";

        private bool enable = false;
        private bool enableSet = false;
        private string enableCommand = "OUTP:STAT";

        public bool resetFlag = false;

        private int nextRead = 0;

        private int faultMask = 0;
        private string faultEnable = "STAT:PROT:ENAB ";
        private string faultEnabled = "STAT:PROT:ENAB?";
        private string faultEvents = "STAT:PROT:EVEN?";
        private string faultConditions = "STAT:PROT:COND?";
        private bool faultMaskSet = false;
        private string fault = "";
        public string Fault
        {
            get { return fault; }
            internal set
            {
                fault = value;
                if (!string.IsNullOrWhiteSpace(fault))
                    OnFaultOccurred();
            }
        }

        public static List<Fault> FaultCodes = new List<Fault>
        {
            new Fault(0x08, "Overvoltage Protection Fault"),
            new Fault(0x10, "Over Temperature Fault"),
            new Fault(0x20, "External Shutdown"),
            new Fault(0x40, "Foldback Mode Operation"),
            new Fault(0x80, "Remote Programming Error"),
            new Fault(0x100, "Fan Fault"),
            new Fault(0x200, "Line Drop Fault"),
            new Fault(0x400, "DC Module Fault"),
            new Fault(0x800, "PFC Fault"),
            new Fault(0x1000, "OCP Fault"),
            new Fault(0x2000, "AUX Supply Fault "),
            new Fault(0x4000, "Line Status Changed"),
            new Fault(0x8000, "Parallel Cable Fault"),
            new Fault(0x10000, "Salve System Fault"),
            new Fault(0x40000, "Remote Sense Fault"),
            new Fault(0x80000, "Regulation Fault"),
            new Fault(0x100000, "Current Feedback Fault"),
        };

        private int allFaultsMask
        {
            get
            {
                int x = 0;
                foreach (Fault f in FaultCodes)
                {
                    x = x | f.ID;
                }
                return x;
            }
        }

        private string getFaults(string hex)
        {
            int x = conditionCodeFromHex(hex.TrimEnd(new char[] { '\r' } ));
            string res = "";
            List<string> faults = new List<string>();
            foreach (Fault f in FaultCodes)
            {
                if ((f.ID & x) == f.ID)
                {
                    faults.Add(f.Name);
                }
            }
            if (faults.Count > 0)
                res = string.Join(", ", faults);
            Fault = res;
            return res;
        }

        private int conditionCodeFromHex(string hex)
        {
            int res = 0;
            if (hex.StartsWith("1:#H"))
                hex = hex.Replace("1:#H", "0x");
            if (!string.IsNullOrEmpty(hex))
            {
                if (!hex.StartsWith("0x"))
                    hex = "0x" + hex;
                res = Convert.ToInt32(hex, 16);
            }
            return res;
        }

        public SorensenAsterion()
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 9600;
        }

        public SorensenAsterion(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.serialPort1.BaudRate = 9600;
        }

        public SorensenAsterion(SerialPortParameter SerialPortParameter)
        {
            InitializeComponent();
            this.serialPort1.BaudRate = 9600;
            this.SerialPortParameter = SerialPortParameter;
        }

        public override void Process()
        {
            if (Monitor.TryEnter(this.SerialLock, 1000))
            {
                try
                {
                    serialPort1.DiscardInBuffer();
                    bool readVoltage = false;
                    bool readCurrent = false;
                    bool readPower = false;
                    bool readFault = false;

                    if (resetFlag)
                    {
                        resetFlag = false;
                        serialPort1.DiscardInBuffer();
                        serialPort1.WriteLine(resetCommand);
                        Thread.Sleep(250);
                        serialPort1.DiscardInBuffer();
                        serialPort1.WriteLine("OUTP:PROT:FOLD 0");
                    }
                    else if (enableSet)
                    {
                        //if (enable)
                        //{
                        //    serialPort1.WriteLine($"{resetCommand}");
                        //}
                        serialPort1.WriteLine($"{enableCommand} {(enable ? "1" : "0")}");
                        enableSet = false;
                        return;
                    }
                    else if (voltageSet)
                    {
                        serialPort1.WriteLine($"{voltageSetCommand} {voltageProgram.ToString("F2")}");
                        voltageSet = false;
                        return;
                    }
                    else if (currentSet)
                    {
                        serialPort1.WriteLine($"{currentSetCommand} {currentProgram.ToString("F2")}");
                        currentSet = false;
                        return;
                    }
                    else if (powerSet)
                    {
                        serialPort1.WriteLine($"{powerSetCommand} {powerProgram.ToString("F2")}");
                        powerSet = false;
                        return;
                    }
                    else if (clearErrors)
                    {
                        serialPort1.WriteLine($"{clearCommand}");
                        clearErrors = false;
                        return;
                    }
                    else if (faultMaskSet)
                    {
                        serialPort1.WriteLine($"{faultEnable} 'H#{faultMask.ToString("X").PadLeft(7, '0')}'");
                        faultMaskSet = false;
                        return;
                    }
                    else if (nextRead == 0)
                    {
                        nextRead = 1;
                        serialPort1.WriteLine(voltageGetCommand);
                        readVoltage = true;
                    }
                    else if (nextRead == 1)
                    {
                        nextRead = 2;
                        serialPort1.WriteLine(currentGetCommand);
                        readCurrent = true;
                    }
                    else if (nextRead == 2)
                    {
                        nextRead = 3;
                        serialPort1.WriteLine(powerGetCommand);
                        readPower = true;
                    }
                    else
                    {
                        nextRead = 0;
                        serialPort1.WriteLine(faultEvents);
                        readFault = true;
                    }

                    string ret = serialPort1.ReadLine();

                    if (readVoltage)
                    {
                        if (double.TryParse(ret, out double v))
                        {
                            voltage = v;
                            OnValueChanged();
                            voltageReady = true;
                        }
                    }
                    else if (readCurrent)
                    {
                        if (double.TryParse(ret, out double c))
                        {
                            current = c;
                            OnValueChanged();
                            currentReady = true;
                        }
                    }
                    else if (readPower)
                    {
                        if (double.TryParse(ret, out double p))
                        {
                            power = p;
                            OnValueChanged();
                            powerReady = true;
                        }
                    }
                    else if (readFault)
                    {
                        getFaults(ret);
                        if (!string.IsNullOrEmpty(Fault))
                        {
                            OnFaultOccurred();
                            clearErrors = true; // this will clear the faults the next loop through processing
                        }
                    }
                }
                catch (Exception ex)
                {
                    voltageReady = false;
                    currentReady = false;
                    powerReady = false;
                    commError = true;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
        }

        public override void BackgroundProcess()
        {
            OnValueChanged();
        }

        public bool ClearError
        {
            get => clearErrors;
            set
            {
                clearErrors = value;
            }
        }

        public bool Enable
        {
            get => enable;
            set
            {
                bool newVal = enable != value;
                enable = value; 
                enableSet = true;
            }
        }

        public double VoltageProgram
        {
            get => voltageProgram;
        }

        public double Voltage
        {
            get
            {
                if (voltageReady)
                    return voltage;
                else
                    return double.NaN;
            }
            set
            {
                bool newVal = voltageProgram != value;
                voltageProgram = value;
                voltageSet = true;
            }
        }
        public double CurrentProgram
        {
            get => currentProgram;
        }

        public double Current
        {
            get
            {
                if (currentReady)
                    return current;
                else
                    return double.NaN;
            }
            set
            {
                bool newVal = currentProgram != value;
                currentProgram = value;
                currentSet = true;
            }
        }
        public double PowerProgram
        {
            get => powerProgram;
        }

        public double Power
        {
            get
            {
                if (powerReady)
                    return power;
                else
                    return double.NaN;
            }
            set
            {
                powerProgram = value;
                powerSet = true;
            }
        }

        public int FaultMask
        {
            get
            {
                return faultMask;
            }
            set
            {
                faultMask = value;
                faultMaskSet = true;
            }
        }

        public void SetAllMasks()
        {
            FaultMask = allFaultsMask;
        }

        public void SetNoMasks()
        {
            FaultMask = 0;
        }

        public override string Name
        {
            get { return "Sorensen Asterion Series Power Supply on port " + serialPort1.PortName; }
        }

        public override string FormattedValue
        {
            get
            {
                return $"{(voltage == double.NaN ? voltage + voltageUnits : "")}{(current == double.NaN ? current + currentUnits : "")}{(power == double.NaN ? power + powerUnits : "")}";
            }
        }
    }
}
