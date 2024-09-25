using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for a MKS SRG Controller
    /// </summary>
    public class MKSSRG : SerialIOBase
    {
        #region Event Handlers

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> changes
        /// </summary>
        public override event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        protected override void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> changes
        /// </summary>
        public override event EventHandler RawValueChanged;

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        protected override void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        #endregion Event Handlers

        #region Globals

        private Boolean _DoNotCall;
        private Double _pressure;
        private Single _min, _max;
        private String _units;
        private String _format = "0.00";
        private Boolean _sp1Latch = true, _sp2Latch = true, _sp1ActiveHigh = true, _sp2ActiveHigh = false, _IsCommunicatingRemotely;
        private String[] pressureUnitNames = { "mbar", "Pa", "atm", "Torr" };
        private AnalogSignal pressureSignal;

        public String[] RotorControlStatusMessages = {"Disarmed", "No sensor detected","Dismount Sensor", "Idle", "Standby",
                                                    "Starting", "Measuring", "Stopping", "Shutdown","decelerate",
                                                    "Driver operating", "Sensor unstable", "Busy"};

        private Int32 _SampleInterval;

        public String RotorControlStatusMessage;
        public Boolean _accelerate, _decelerate;
        public Boolean _driveOperating;
        public Boolean _SensorUnstable;
        public Boolean _Busy;

        private int RotorControlStatusValue; //see ReadRotorControlStatus()
        public bool LocalMode = false;
        public bool setLocalMode = false;

        private Stopwatch _errorSW = new Stopwatch();

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MKSSRG">MKSSRG</see> class
        /// </summary>
        public MKSSRG(Single Min, Single Max, String Units)
          : base()
        {
            _min = Min;
            _max = Max;
            _units = Units;
            _IsCommunicatingRemotely = true;
            this.BaudRate = 9600;
            this.SerialPort.ReadTimeout = 100000;
            pressureSignal = new AnalogSignal("SRG Press", pressureUnitNames[3], "0.0000E-0", 1, false, true);
            Format = "0.0000E-0";
        }

        #endregion Construction

        #region Private Methods

        private void SendWithDelay(String Text)
        {
            if (!serialPort1.IsOpen) serialPort1.Open();
            for (int i = 0; i < Text.Length; i++)
            {
                serialPort1.Write(Text.Substring(i, 1));
                Thread.Sleep(25);
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> of the MKS SRG Controller
        /// </summary>
        public override void Process()
        {
            if (LocalMode == true)
                return;
            if (setLocalMode == true)
            {
                ReturnToLocal();
                LocalMode = true;
                setLocalMode = false;
            }
            else
            {
                if (Monitor.TryEnter(this.SerialLock, 500))
                {
                    try
                    {
                        Thread.Sleep(300);
                        if (_DoNotCall == false)
                        {
                            serialPort1.DiscardInBuffer();
                            if (RotorControlStatusMessage == "Idle")
                            {
                                _pressure = 1000;
                            }
                            else
                            { _pressure = ReadPressure(); }

                            RotorControlStatusValue = ReadRotorControlStatus();
                        }
                        else
                        {
                            //Console.Write("Skipped a read");
                        }

                        if (!this.backgroundWorker1.IsBusy)
                            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                    }
                    catch (Exception e)
                    {
                        pressureSignal.Value = Double.NaN;
                        VtiEvent.Log.WriteError("Error inside MKSSRG | Process", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                        //commError = true;
                    }
                    finally
                    {
                        Monitor.Exit(this.SerialLock);
                    }
                }
            }
        }

        /// <summary>
        /// Processes commands to the MKS SRG Controller
        /// </summary>
        public string ProcessCommandAndReturnValue(string strCommand)
        {
            // Send command
            _DoNotCall = true;
            string CommandSent = strCommand + "\r";
            try
            {
                Thread.Sleep(50);
                serialPort1.DiscardInBuffer();
                serialPort1.WriteLine(CommandSent);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ProcessCommandAndReturnValue, Serial Port Write", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
                return "-1"; // write error
            }
            // Read back response
            DateTime dt = DateTime.Now;
            long dataLength = 20; double timeout = 2;
            string strRead = ""; // CLEAR VALUE
            try
            {
                DateTime dtStart = DateTime.Now;
                TimeSpan ts;
                int numReads = 0, prevStrReadLength, numEmptyReads = 0;
                do
                {
                    Thread.Sleep(50);
                    prevStrReadLength = strRead.Length;
                    strRead += serialPort1.ReadExisting();
                    if (strRead.Length == prevStrReadLength)
                        numEmptyReads++;
                    else
                        numEmptyReads = 0;
                    if (strRead.Length >= dataLength)
                        break;
                    numReads++;
                    ts = DateTime.Now - dtStart;
                }
                while (numEmptyReads < 3 && ts.TotalSeconds < timeout);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ProcessCommandAndReturnValue, Serial Port Read", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                _DoNotCall = false;
                return "0"; // Read error
            }
            strRead = strRead.Replace("\r", "");
            strRead = strRead.Replace("\n", "");
            strRead = strRead.Replace(">", "");
            strRead = strRead.Trim();
            _DoNotCall = false;
            return strRead; // no error
        }

        /// <summary>
        /// Set Accomodation Factor
        /// </summary>
        public string SetAccomodationFactor(string strAccomodationFactor)
        {
            float fAccomodationFactor;
            try
            {
                fAccomodationFactor = Convert.ToSingle(strAccomodationFactor);
            }
            catch (Exception e)
            { // could not convert strAccomodationFactor to a float
                VtiEvent.Log.WriteError("Error inside SetAccomodationFactor", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
            string strDummy = "", strCMD = string.Format("{0:0.0000}", fAccomodationFactor) + " ACC" + "\r"; ;
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SetAccomodationFactor", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "?";
        }

        /// <summary>
        /// Set Temperature Scale
        /// </summary>
        // Units:
        // 0 = Kelvin
        // 1 = Celsius
        public string SetTemperatureScale(int nTemperatureScale)
        {
            string strDummy = "", strCMD = string.Format("{0:0}", nTemperatureScale) + " TSC" + "\r";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SetTemperatureScale", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "?";
        }

        /// <summary>
        /// Set Pressure Unit
        /// </summary>
        /// <remarks>Units:
        /// 0 = Deceleration Rate
        /// 1 = Pascals
        /// 2 = millibar
        /// 3 = Torr</remarks>
        public string SetPressureUnit(int nPressureUnit)
        {
            string strDummy = "", strCMD = string.Format("{0:0}", nPressureUnit) + " UNT" + "\r";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SetPressureUnit", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "?";
        }

        /// <summary>
        /// Set Temperature
        /// </summary>
        public string SetTemperature(string strTemperature)
        {
            float fTemperature;
            try
            {
                fTemperature = Convert.ToSingle(strTemperature);
            }
            catch (Exception e)
            { // could not convert strTemperature to a float
                VtiEvent.Log.WriteError("Error inside SetTemperature conversion to single", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
            string strDummy = "", strCMD = string.Format("{0:0.00}", fTemperature) + " TMP" + "\r";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SetTemperature", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "?";
        }

        /// <summary>
        /// Set Read Interval
        /// </summary>
        public string SetInterval(string strInterval)
        {
            Int16 nInterval;
            try
            {
                nInterval = Convert.ToInt16(Single.Parse(strInterval));
                _SampleInterval = nInterval;
            }
            catch (Exception e)
            { // could not convert strInterval to an integer
                VtiEvent.Log.WriteError("Error inside SetInterval conversion to Int16", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
            string strDummy = "", strCMD = string.Format("{0}", nInterval) + " SIN" + "\r";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                Thread.Sleep(500);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SetInterval", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "?";
        }

        /// <summary>
        /// Set Gas Type
        /// </summary>
        /// <remarks>
        /// n GAS Selects predefined gas type n (1 to 25). The gas type numbers are
        ///*** assigned as follows (the strings in parentheses are the labels returned
        ///by command GLB):
        //1 user-definable (Usr1)
        //2 user-definable (Usr2)
        //3 user-definable (Usr3)
        //4 user-definable (Usr4)
        //5 user-definable (Usr5)
        //6 user-definable (Usr6)
        //7 user-definable (Usr7)
        //8 user-definable (Usr8)
        //9 Air (Air)
        //10 Argon (Ar)
        //11 Acethylene (C2H2)
        //12 Freon-14 (CF4)
        //13 Methane (CH4)
        //14 Carbon dioxide (CO2)
        //15 Deuterium (D2)
        //16 Hydrogen (H2)
        //17 Helium (He)
        //18 Hydrogen fluoride (HF)
        //19 Nitrogen (N2)
        //20 Nitrous oxide (N2O)
        //21 Neon (Ne)
        //22 Oxygen (O2)
        //23 Sulfur dioxide (SO2)
        //24 Sulfur hexafluoride (SF6)
        //25 Xenon (Xe)</remarks>
        ///<see cref="ReadGasType"/>
        public string SetGasType(int nGasType)
        {
            string strDummy = "", strCMD = string.Format("{0}", nGasType) + " GAS" + "\r";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SetGasType", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "?";
        }

        /// <summary>
        /// Get Read Interval
        /// </summary>
        public string ReadInterval()
        {
            Single nRet = 0;
            string strDummy = "";
            try
            {
                Thread.Sleep(500);
                strDummy = ProcessCommandAndReturnValue("SIN");

                //nRet = Convert.ToInt16(Convert.ToSingle(strDummy)); // cleans it up if 2.0001E1/n/r is returned to an int value
                bool success = Single.TryParse(strDummy, out nRet);
                if (success)
                {
                    if (nRet == 6)
                    {
                        VtiEvent.Log.WriteError("Error inside ReadInterval", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, nRet);
                    }
                    _SampleInterval = Convert.ToInt16(nRet);
                    strDummy = nRet.ToString();
                    return strDummy;
                }
                return _SampleInterval.ToString();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadInterval", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "0";
            }
        }

        /// <summary>
        /// Dismount SRG
        /// </summary>
        public string DismountSRG()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("DMT");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside DismountSRG", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
        }

        /// <summary>
        /// Disarm SRG
        /// </summary>
        public string DisarmSRG()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("0 ARM");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside DisarmSRG", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
        }

        /// <summary>
        /// Arm SRG
        /// </summary>
        public string ArmSRG()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("1 ARM");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ArmSRG", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
        }

        /// <summary>
        /// Start SRG
        /// </summary>
        public string StartSRG()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("STA");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StartSRG", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
        }

        /// <summary>
        /// Stop SRG
        /// </summary>
        public string StopSRG()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("STP");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside StopSRG", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
        }

        /// <summary>
        /// Clear Errors
        /// </summary>
        public string ClearErrors()
        {
            string strDummy = "";
            string sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue("0 STS");
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ClearErrors", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                return "?";
            }
        }

        /// <summary>
        /// Get Temperature
        /// </summary>
        public float ReadTemperature()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("TMP");
                Single fRet = 0;
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadTemperature", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Residual Drag Calc
        /// </summary>
        public void ResidualDragCalc(int DragStep)
        {
            switch (DragStep)
            {
                case 0:
                    MessageBox.Show(
                      "To perform the SRG - Residual Drag Offset calculation please follow all recommendations by the manufacture prior to calculating and setting the offset on the SRG.",
                      "Residual Drag Offset",
                      MessageBoxButtons.OK);
                    break;

                case 5:
                    break;
            }
        }

        /// <summary>
        /// Get Dampening
        /// </summary>
        public float ReadDampening()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("DMP");
                Single fRet = 0;
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadDampening", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Get Signal Strength
        /// </summary>
        public float ReadSignalStrength()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("SGL");
                Single fRet = 0;
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadSignalStrength", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        public int getRotorControlStatus()
        {
            return RotorControlStatusValue;
        }

            /// <summary>
            /// Get ReadRotorControlStatus
            /// </summary> <remarks>
            ///    RCS Returns the rotor control status:
            ///Bit     Value   Meaning
            ///3..0    0       Disarmed (automatic sensor control off)
            ///        1       No sensor detected
            ///        2       Dismount sensor
            ///        3       Idle (sensor at rest)
            ///        4       Standby (sensor coasting)
            ///        5       Starting...
            ///        6       Measuring (READY relay on)
            ///        7       Stopping...
            ///        8       Shutdown...
            /// 4      16      Drive direction (0=accelerate, 1=decelerate)
            /// 5      32      Drive operating
            /// 6      64      Sensor unstable
            /// 7      128     Busy (background task executing)</remarks>
        public int ReadRotorControlStatus()
        {
            int nRet;
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("RCS");

                nRet = Convert.ToInt16(strDummy);
                if ((nRet & 15) == 0)
                    RotorControlStatusMessage = RotorControlStatusMessages[0];
                if (((nRet & 15) & 1) == 1)
                    RotorControlStatusMessage = RotorControlStatusMessages[1];
                if (((nRet & 15) & 2) == 2)
                    RotorControlStatusMessage = RotorControlStatusMessages[2];
                if (((nRet & 15) & 3) == 3)
                    RotorControlStatusMessage = RotorControlStatusMessages[3];
                if (((nRet & 15) & 4) == 4)
                    RotorControlStatusMessage = RotorControlStatusMessages[4];
                if (((nRet & 15) & 5) == 5)
                    RotorControlStatusMessage = RotorControlStatusMessages[5];
                if (((nRet & 15) & 6) == 6)
                    RotorControlStatusMessage = RotorControlStatusMessages[6];
                if (((nRet & 15) & 7) == 7)
                    RotorControlStatusMessage = RotorControlStatusMessages[7];
                if (((nRet & 15) & 8) == 8)
                    RotorControlStatusMessage = RotorControlStatusMessages[8];

                if ((nRet & 16) == 16)
                { _accelerate = false; _decelerate = true; }
                else { _accelerate = true; _decelerate = false; }

                if ((nRet & 32) == 32)
                { _driveOperating = true; }
                else { _driveOperating = false; }

                if ((nRet & 64) == 64)
                { _SensorUnstable = true; }
                else { _SensorUnstable = false; }

                if ((nRet & 128) == 128)
                { _Busy = true; }
                else { _Busy = false; }

                return nRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadRotorControlStatus", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                commError = true;
            }

            return 0;
        }

        /// <summary>
        /// Get Density
        /// </summary>
        public float ReadDensity()
        {
            Single fRet;
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("DEN");
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadDensity", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Get Pressure
        /// </summary>
        public float ReadPressure()
        {
            string strDummy = "", strCMD = "";
            bool bUsingSRG2Protocol = false;
            if (bUsingSRG2Protocol)
            {
                strCMD = "TRG";
            }
            else
            {
                strCMD = "VAL";
            }
            Single fRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                if (Single.TryParse(strDummy, out fRet))
                {
                    return fRet;
                }
                else { return 0.0f; }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadPressure", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Get Diameter
        /// </summary>
        public float ReadNextValue()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("NXT VAL");
                Single fRet = 0;
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadNextValue", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Get Diameter
        /// </summary>
        public float ReadDiameter()
        {
            string strDummy = "", strCMD = "DIA";
            Single fRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadDiameter", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Get Diameter
        /// </summary>
        public float ReadViscosity()
        {
            string strDummy = "", strCMD = "VIS";

            Single fRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCMD);
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadViscosity", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Get Accomodation Factor
        /// </summary>
        public float ReadAccomodationFactor()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("ACC");
                Single fRet = 0;
                fRet = Convert.ToSingle(strDummy);
                return fRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadAccomodationFactor", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0.0f;
        }

        /// <summary>
        /// Get System Status </summary>
        /// <remarks>
        /// Syntax: STS Returns the system status and clears status bit 7.
        ///Bit   Value   Meaning
        ///0     1       SP1 activated
        ///1     2       SP2 activated
        ///2     4       RDY activated
        ///3     8       Printer not ready
        ///4*    16      Data available
        ///5*    32      Message pending
        ///6     64      Backup failed/Setup defaulted¹
        ///7*    128     Power failure
        ///0 STS Clears system status bits 4, 5 and 7.
        ///Note 1: Status bit 6 is set during power-up when the internal clock/calendar has lost
        ///information due to low backup battery. The bit is also set by command 1 DEF (restore
        ///defaults). The bit is reset by executing 0 DEF.or by modifying the setup.</remarks>
        public byte ReadSystemStatus()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("STS");
                try
                {
                    byte iRet = 0;
                    iRet = Convert.ToByte(strDummy);
                    return iRet;
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError("Error inside ReadSystemStatus", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                    return 0;
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadSystemStatus", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0;
        }

        public Int16 ReadPressureUnits()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("UNT");
                try
                {
                    Int16 iRet = 0;
                    iRet = Convert.ToInt16(strDummy);
                    return iRet;
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError("Error inside ReadPressureUnits", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                    return 0;
                }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadPressureUnits", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0;
        }

        /// <summary>
        /// Send Manual Command
        /// </summary>
        public string SendManualCommand(string strCommand)
        {
            string strDummy = "";
            String sRet;
            try
            {
                strDummy = ProcessCommandAndReturnValue(strCommand);
                sRet = strDummy;
                return sRet;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside SendManualCommand", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "";
        }

        /// <summary>
        /// Return To Local
        /// </summary>
        public int ReturnToLocal()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("RTL");
                int nRet = 0;
                if (strDummy != "")
                {
                    nRet = Convert.ToInt16(strDummy);
                    return nRet;
                }
                else { return nRet; }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReturnToLocal", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0;
        }

        /// <summary>
        /// Get Gas Type
        /// </summary>
        // Gas Types:
        // 1-8 = UDT's
        // 9  = Air
        // 10 = Argon
        // 11 = Acethylene (C2H2)
        // 12 = Freon-14 (CF4)
        // 13 = Methane (CH4)
        // 14 = Carbon Dioxide
        // 15 = Deuterium
        // 16 = Hydrogen
        // 17 = Helium
        // 18 = Hydrogen Fluoride
        // 19 = Nitrogen
        // 20 = Nitrous Oxide
        // 21 = Neon
        // 22 = Oxygen
        // 23 = Sulfur Dioxide
        // 24 = Sulfur Hexafluoride
        // 25 = Xenon
        public int ReadGasType()
        {
            string strDummy = "";
            try
            {
                int CommTry = 0;
                while (true)
                {
                    Thread.Sleep(2000);

                    strDummy = ProcessCommandAndReturnValue("GAS");
                    if (strDummy != "") break;
                    if (CommTry > 2) break;
                    CommTry++;
                };
                int nRet = 0;

                if (int.TryParse(strDummy, out nRet))
                {
                    return nRet;
                }
                else { return 0; }
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadGasType", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return 0;
        }

        /// <summary>
        /// Get Speed
        /// </summary>
        public string ReadSpeed()
        {
            string strDummy = "";
            try
            {
                strDummy = ProcessCommandAndReturnValue("RSP");
                return strDummy;
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error inside ReadSpeed", VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            return "0";
        }

        #endregion Public Methods

        #region Events

        /// <summary>
        /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            OnValueChanged();
        }

        #endregion Events

        #region Public Properties

        public int LastSampleInterval
        {
            get
            {
                return _SampleInterval;
            }
        }

        /// <summary>
        /// Value (pressure) of the MKS SRG Controller
        /// </summary>
        public override double Value
        {
            get
            {
                return _pressure;
            }
            internal set
            {
                _pressure = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Formatted value including the <see cref="Units">Units</see>
        /// </summary>
        public override string FormattedValue
        {
            get
            {
                if (Double.IsNaN(this.Value))
                    return "ERROR";
                else
                    return this.Value.ToString(_format) + " " + this.Units;
            }
        }

        /// <summary>
        /// Name for the MKS SRG Controller
        /// </summary>
        public override string Name
        {
            get { return "MKS SRG Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Get Remote Mode State
        /// </summary>
        public bool IsCommunicatingRemotely
        {
            get { return _IsCommunicatingRemotely; }
        }

        /// <summary>
        /// Minimum value for the MKS SRG Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the MKS SRG controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the MKS SRG controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the MKS SRG controller
        /// </summary>
        public override string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        #endregion Public Properties
    }

    /// <summary>
    /// Gas Types used by SRG
    /// </summary>
    public enum GasType
    {
        None = 0,
        UDT1 = 1,
        UDT2 = 2,
        UDT3 = 3,
        UDT4 = 4,
        UDT5 = 5,
        UDT6 = 6,
        UDT7 = 7,
        UDT8 = 8,
        AIR = 9,
        ARGON = 10,
        ACETYLENE = 11,
        FREON14 = 12,
        METHANE = 13,
        CARBONDIOXIDE = 14,
        DEUTERIUM = 15,
        HYDROGEN = 16,
        HELIUM = 17,
        HYDROGENFLUORIDE = 18,
        NITROGEN = 19,
        NITROUSOXIDE = 20,
        NEON = 21,
        OXYGEN = 22,
        SULFURDIOXIDE = 23,
        SULFURHEXAFLUORIDE = 24,
        XENON = 25
    }
}