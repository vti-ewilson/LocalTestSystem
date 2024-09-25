using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Implements the binary communication protocol to an Inficon LDS2010 Leak Detector
    /// </summary>
    public partial class InficonHLD6000ANSIIMDB : SerialIOBase
    {
        #region Enums

#pragma warning disable 1591

        public enum PressureUnitsType
        {
            mbar = 0,
            Pa = 1,
            atm = 2,
            Torr = 3
        }

        public enum CalFactorModes
        {
            Vacuum = 0,
            Sniff = 1
        }

        public enum CalModes
        {
            Internal = 0,
            External = 1
        }

        public enum CalStates
        {
            Inactive = 0,
            Active = 1,
            WaitForCalLeakClose = 2
        }

        public enum LeakRateUnitsType
        {
            mbar_l_s = 0,
            Pa_m3_s = 1,
            atm_cc_s = 2,
            torr_l_s = 3,
            ppm = 4,
            g_a = 5
        }

        public enum OpModes
        {
            Vacuum = 0,
            Sniff = 1,
            ByPlcInput = 2
        }

        public enum MeasurementModes
        {
            Standby = 0,
            Measurement = 1
        }

        public enum States
        {
            Standby = 0,
            Error = 1,
            Cal = 2,
            Runup = 3,
            Ready = 4,
            EmissionOff = 5
        }

        public enum GasBallastStates
        {
            Off = 0,
            On = 1,
            FailSafeOn = 2
        }

        public enum AudioModes
        {
            Analog = 0,
            Trigger = 1
        }

        public enum ExtCalModes
        {
            Normal = 0,
            Dynamic = 1,
            ByPlcInput = 2
        }

        public enum ZeroModes
        {
            Decades2to3 = 0,
            Decades1to2 = 1,
            Decades2 = 3,
            Decades3to4 = 4,
            Value = 2
        }

        public enum ControlModes
        {
            FrontPanel = 0,
            RS232 = 1,
            RS485 = 2
        }

#pragma warning restore 1591

        #endregion Enums

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

        private String format = "0.000";
        private double min = 1E-15, max = 1E0;
        private Double leakRate = 0, pressure;
        public string ExtCalStatus;
        private int InficonErrorCode;
        private LeakRateUnitsType? leakRateUnits;
        private PressureUnitsType? pressureUnits;
        private String[] leakRateUnitNames = { "mbar-l/s", "Pa-m^3/s", "atm-cc/s", "Torr-l/s", "ppm", "g/a" };
        private String[] pressureUnitNames = { "mbar", "Pa", "atm", "Torr" };
        //private Boolean commError = false;
        //private int retries = 0;

        private Boolean _DoNotCall;

        private Boolean _StandbyOn = false;

        private Boolean _StandbyOff = false;

        private Boolean _PollForExtLR;

        private Boolean _PollForLR = true;

        private Boolean _PollForLDStatus;

        private Boolean _PollForLDCalStatus;

        private Boolean _ClearError = false;

        private Boolean _StartExternalCal = false;

        private Boolean _CloseExternalCalLeak = false;

        private Boolean _SendExternalCalLeakFlow = false;

        private Boolean _SendCommandString = false;

        private String _ExtCalLeakFlow = "2.0E-7";

        private String _LDSStatus = "****";

        private String _LDCalStatus = "****";

        private String _LDextLR = "0";

        private String _CommandString = "";

        private AnalogSignal leakRateSignal, pressureSignal;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010Binary">InficonLDS2010Binary</see> class
        /// </summary>
        public InficonHLD6000ANSIIMDB()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010Binary">InficonLDS2010Binary</see> class
        /// </summary>
        /// <param name="container">Container for the object</param>
        public InficonHLD6000ANSIIMDB(IContainer container)
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

        #region Private Methods

        private void CalculateCheckSumAndSend(String Message)
        {
            String message;
            message = String.Format("{0}{1}", Message, (char)(Message.ToCharArray().Sum(c => (int)c) % 256));
            this.serialPort1.DiscardInBuffer();
            this.serialPort1.Write(message);
        }

        private bool SendMessage(String Message)
        {
            bool retVal = false;
            if (this.IsAvailable && !commError)
            {
                if (Monitor.TryEnter(this.SerialLock, 3000))
                {
                    try
                    {
                        Actions.Retry(3,
                            delegate
                            {
                                CalculateCheckSumAndSend(Message);
                                retVal = true;
                            }, 500);
                    }
                    catch (Exception e)
                    {
                        //commError = true;
                        //VtiEvent.Log.WriteError(
                        //    String.Format("Error communicating with {0}.", this.Name),
                        //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        //    e.ToString());
                    }
                    finally
                    {
                        Monitor.Exit(this.SerialLock);
                    }
                }
            }
            return retVal;
        }

        private byte[] SendMessageWithResponse(String Message)
        {
            byte[] retData = null;
            byte len;
            if (this.IsAvailable && !commError && Monitor.TryEnter(this.SerialLock, 3000))
            {
                try
                {
                    Actions.Retry(3,
                        delegate
                        {
                            CalculateCheckSumAndSend(Message);
                            Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 0);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);
                        });
                }
                catch (Exception e)
                {
                    //commError = true;
                    //VtiEvent.Log.WriteError(
                    //    String.Format("Error communicating with {0}.", this.Name),
                    //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                    //    e.ToString());
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
            return retData;
        }

        private Single ParseFloatResponse(byte[] Response)
        {
            Single retVal = Single.NaN;
            if (Response != null && Response.Length >= 6)
                retVal = BitConverter.ToSingle(new byte[] { Response[4], Response[5], Response[3], Response[2] }, 0);
            return retVal;
        }

        private byte[] SingleToInficonBytes(Single Value)
        {
            byte[] bytesIn = BitConverter.GetBytes(Value);
            byte[] bytesOut = new byte[] { bytesIn[3], bytesIn[2], bytesIn[0], bytesIn[1] };
            return bytesOut;
        }

        private Single GetFloatParameter(byte Command, byte Param1, byte Param2)
        {
            return ParseFloatResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)6, (char)Command, (char)Param1, (char)Param2)));
        }

        private Single GetFloatParameter(byte Command, byte Param1)
        {
            return ParseFloatResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}", (char)0x05, (char)5, (char)Command, (char)Param1)));
        }

        private Single GetFloatParameter(byte Command)
        {
            return ParseFloatResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}", (char)0x05, (char)5, (char)Command)));
        }

        private bool SetFloatParameter(byte Command, Single Value, byte Param1, byte Param2)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}{5}", (char)0x05, (char)10, (char)Command, (char)Param1, (char)Param2, SingleToInficonBytes(Value)));
        }

        private bool SetFloatParameter(byte Command, Single Value, byte Param1)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)9, (char)Command, (char)Param1, SingleToInficonBytes(Value)));
        }

        private bool SetFloatParameter(byte Command, Single Value)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)8, (char)Command, SingleToInficonBytes(Value)));
        }

        private Byte ParseByteResponse(byte[] Response)
        {
            if (Response != null && Response.Length >= 3)
                return Response[2];
            else
                return 0;
        }

        private Byte GetByteParameter(byte Command, byte Param1, byte Param2)
        {
            return ParseByteResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)6, (char)Command, (char)Param1, (char)Param2)));
        }

        private Byte GetByteParameter(byte Command, byte Param1)
        {
            return ParseByteResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}{3}", (char)0x05, (char)5, (char)Command, (char)Param1)));
        }

        private Byte GetByteParameter(byte Command)
        {
            return ParseByteResponse(
                SendMessageWithResponse(
                    String.Format("{0}{1}{2}", (char)0x05, (char)4, (char)Command)));
        }

        private bool SetByteParameter(byte Command, Byte Value, byte Param1, byte Param2)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}{5}", (char)0x05, (char)7, (char)Command, (char)Param1, (char)Param2, (char)Value));
        }

        private bool SetByteParameter(byte Command, Byte Value, byte Param1)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)6, (char)Command, (char)Param1, (char)Value));
        }

        private bool SetByteParameter(byte Command, Byte Value)
        {
            return SendMessage(
                String.Format("{0}{1}{2}{3}", (char)0x05, (char)5, (char)Command, (char)Value));
        }

        private Boolean GetBoolParameter(byte Command)
        {
            return GetByteParameter(Command) == 1;
        }

        private Boolean GetBoolParameter(byte Command, byte Param1)
        {
            return GetByteParameter(Command, Param1) == 1;
        }

        private Boolean GetBoolParameter(byte Command, byte Param1, byte Param2)
        {
            return GetByteParameter(Command, Param1, Param2) == 1;
        }

        private bool SetBoolParameter(byte Command, Boolean Value)
        {
            return SetByteParameter(Command, Value ? (byte)1 : (byte)0);
        }

        private bool SetBoolParameter(byte Command, Boolean Value, byte Param1)
        {
            return SetByteParameter(Command, Value ? (byte)1 : (byte)0, Param1);
        }

        private bool SetBoolParameter(byte Command, Boolean Value, byte Param1, byte Param2)
        {
            return SetByteParameter(Command, Value ? (byte)1 : (byte)0, Param1, Param2);
        }

        private bool SetVoidParameter(byte Command)
        {
            return SendMessage(
                String.Format("{0}{1}{2}", (char)0x05, (char)4, (char)Command));
        }

        private string GetStringParameter(byte Command)
        {
            byte[] retData = SendMessageWithResponse(
                    String.Format("{0}{1}{2}", (char)0x05, (char)4, (char)Command));
            if (retData == null) return String.Empty;
            else return retData.ToString();
        }

        private bool ClearErrorCommand()
        {
            if (!_DoNotCall)
            {
                _CommandString = "*cls";
                SendACommandString();
                _ClearError = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool StopCommand()
        {
            if (!_DoNotCall)
            {
                _CommandString = "*stop";
                SendACommandString();
                _StandbyOn = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool StartCommand()
        {
            if (!_DoNotCall)
            {
                _CommandString = "*start";
                SendACommandString();
                _StandbyOff = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Thread for reading the <see cref="LeakRate">Leak Rate</see> and <see cref="Pressure">Pressure</see> of the Leak Detector
        /// </summary>
        public override void Process()
        {
            String tempstring;

            #region if (_SendCommandString)

            //send a command string
            if (_SendCommandString)
            {
                //_SendCommandString = false;
                Boolean TempBool;
                TempBool = SendACommandString();
            }
            //clear error
            if (_ClearError)
            {
                Boolean TempBool;
                TempBool = ClearErrorCommand();
            }
            //send external cal leak flow value
            if (_SendExternalCalLeakFlow)
            {
                Boolean TempBool;
                TempBool = SendExternalCalFlow();
            }
            //start external cal
            if (_StartExternalCal)
            {
                Boolean TempBool;
                TempBool = StartExternalCal();
            }

            //external cal leak closed to finish calibration
            if (_CloseExternalCalLeak)
            {
                _CloseExternalCalLeak = false;
                Boolean TempBool;
                TempBool = ExternalCalLeakClosed();
            }
            //standby on
            if (_StandbyOn)
            {
                // _StandbyOn = false;
                Boolean TempBool;
                TempBool = StopCommand();
            }

            //standby off
            if (_StandbyOff)
            {
                //_StandbyOff = false;
                Boolean TempBool;
                TempBool = StartCommand();
            }

            #endregion if (_SendCommandString)

            _DoNotCall = false;
            Thread.Sleep(50);
            if (!_DoNotCall && !Asleep)
            {
                _DoNotCall = true;

                #region poll for LD Status

                //poll for LD Status
                if (_PollForLDStatus)
                {
                    try
                    {
                        serialPort1.NewLine = "\r";
                        serialPort1.DiscardInBuffer();
                        serialPort1.WriteLine("*status?");
                        Console.WriteLine("Write: *status?");
                        Thread.Sleep(100);
                        Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);
                        if (this.IsAvailable)
                        {
                            try
                            {
                                Actions.Retry(3,
                                    delegate
                                    {
                                        Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

                                        byte[] _bytearray = new byte[50];
                                        int n = serialPort1.BytesToRead;
                                        int i;
                                        for (i = 0; i < 50; i++)
                                        {
                                            _bytearray[i] = 0;
                                        }
                                        for (i = 0; i < n; i++)
                                        {
                                            byte tempbyte;
                                            tempbyte = (byte)serialPort1.ReadByte();
                                            if ((tempbyte != 10) && (tempbyte != 13))
                                            {
                                                _bytearray[i] = tempbyte;
                                            }
                                        }
                                        ASCIIEncoding ascii = new ASCIIEncoding();

                                        tempstring = ascii.GetString(_bytearray);
                                        _LDSStatus = tempstring;
                                        Console.WriteLine("Read: " + _LDSStatus);
                                    });
                            }
                            catch (Exception e)
                            {
                                ////commError = true;
                                //VtiEvent.Log.WriteError(
                                //    String.Format("Error communicating with {0}.", this.Name),
                                //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                //    e.ToString());
                                _LDSStatus = "TEST_STRING";
                                this.BackgroundProcess();
                            }
                            finally
                            {
                            }
                        }
                        _DoNotCall = false;

                        if (!this.backgroundWorker1.IsBusy)
                            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                    }
                    catch (Exception e)
                    {
                        _LDSStatus = "TEST_STRING";
                        this.BackgroundProcess();
                        //commError = true;
                    }
                    finally
                    {
                    }
                }

                #endregion poll for LD Status

                #region poll for LD Cal Status

                //poll for LD Status
                if (_PollForLDCalStatus)
                {
                    try
                    {
                        serialPort1.NewLine = "\r";
                        serialPort1.DiscardInBuffer();
                        serialPort1.WriteLine("*status:cal?");
                        Thread.Sleep(100);
                        Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);
                        if (this.IsAvailable)
                        {
                            try
                            {
                                Actions.Retry(3,
                                    delegate
                                    {
                                        Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

                                        byte[] _bytearray = new byte[50];
                                        int n = serialPort1.BytesToRead;
                                        int i;
                                        for (i = 0; i < 50; i++)
                                        {
                                            _bytearray[i] = 0;
                                        }
                                        for (i = 0; i < n; i++)
                                        {
                                            byte tempbyte;
                                            tempbyte = (byte)serialPort1.ReadByte();
                                            if ((tempbyte != 10) && (tempbyte != 13))
                                            {
                                                _bytearray[i] = tempbyte;
                                            }
                                        }
                                        ASCIIEncoding ascii = new ASCIIEncoding();

                                        tempstring = ascii.GetString(_bytearray);
                                        _LDCalStatus = tempstring;
                                    });
                            }
                            catch (Exception e)
                            {
                                ////commError = true;
                                //VtiEvent.Log.WriteError(
                                //    String.Format("Error communicating with {0}.", this.Name),
                                //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                //    e.ToString());
                                _LDCalStatus = "TEST_STRING";
                                this.BackgroundProcess();
                            }
                            finally
                            {
                            }
                        }
                        _DoNotCall = false;

                        if (!this.backgroundWorker1.IsBusy)
                            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                    }
                    catch (Exception e)
                    {
                        _LDCalStatus = "TEST_STRING";
                        this.BackgroundProcess();
                        //commError = true;
                    }
                    finally
                    {
                    }
                }

                #endregion poll for LD Cal Status

                #region poll for ExtLR

                //poll for external cal LR
                if (_PollForExtLR)
                {
                    try
                    {
                        serialPort1.NewLine = "\r";
                        serialPort1.DiscardInBuffer();
                        serialPort1.WriteLine("*config:calleak?");
                        Console.WriteLine("Write: *config:calleak?");
                        //Thread.Sleep(100);
                        Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);
                        if (this.IsAvailable)
                        {
                            try
                            {
                                Actions.Retry(3,
                                    delegate
                                    {
                                        Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

                                        byte[] _bytearray = new byte[50];
                                        int n = serialPort1.BytesToRead;
                                        int i;
                                        for (i = 0; i < 50; i++)
                                        {
                                            _bytearray[i] = 0;
                                        }
                                        for (i = 0; i < n; i++)
                                        {
                                            byte tempbyte;
                                            tempbyte = (byte)serialPort1.ReadByte();
                                            if ((tempbyte != 10) && (tempbyte != 13))
                                            {
                                                _bytearray[i] = tempbyte;
                                            }
                                        }
                                        ASCIIEncoding ascii = new ASCIIEncoding();

                                        tempstring = ascii.GetString(_bytearray);
                                    // tempstring = serialPort1.ReadLine();
                                    //tempstring = serialPort1.ReadExisting();
                                    Console.WriteLine("Read _LDExtLR GetString: " + tempstring);
                                        _LDextLR = tempstring;
                                    });
                            }
                            catch (Exception e)
                            {
                                string tempstr = e.ToString();
                                ////commError = true;
                                //VtiEvent.Log.WriteError(
                                //    String.Format("Error communicating with {0}.", this.Name),
                                //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                //    e.ToString());
                                _LDextLR = "0";
                                this.BackgroundProcess();
                            }
                            finally
                            {
                            }
                        }
                        _DoNotCall = false;

                        if (!this.backgroundWorker1.IsBusy)
                            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                    }
                    catch (Exception e)
                    {
                        string tempstr = e.ToString();
                        Console.WriteLine("Ex: " + tempstr);
                        _LDextLR = "0";
                        this.BackgroundProcess();
                        //commError = true;
                    }
                    finally
                    {
                    }
                }

                #endregion poll for ExtLR

                #region get current leak rate

                //get leak rate
                if (_PollForLR)
                {
                    try
                    {
                        serialPort1.NewLine = "\r";

                        serialPort1.DiscardInBuffer();

                        serialPort1.WriteLine("*read?");
                        Console.WriteLine("Write: *read?");
                        //serialPort1.WriteLine("LR");//****mdb3/23/2013

                        Thread.Sleep(100);

                        //Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);

                        if (this.IsAvailable)
                        {
                            try
                            {
                                Actions.Retry(3,
                                    delegate
                                    {
                                        Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

                                        byte[] _bytearray = new byte[50];
                                        int n = serialPort1.BytesToRead;
                                        int i;
                                        for (i = 0; i < 50; i++)
                                        {
                                            _bytearray[i] = 0;
                                        }
                                        for (i = 0; i < n; i++)
                                        {
                                            byte tempbyte;
                                            tempbyte = (byte)serialPort1.ReadByte();
                                            if ((tempbyte != 10) && (tempbyte != 13))
                                            {
                                                _bytearray[i] = tempbyte;
                                            }
                                        }
                                        ASCIIEncoding ascii = new ASCIIEncoding();

                                        tempstring = ascii.GetString(_bytearray);
                                        Console.WriteLine("Read: " + tempstring);
                                        //tempstring = tempstring.Replace("\\", "");
                                        // tempstring = Regex.Replace(tempstring, @"\\", "");
                                        //tempstring = tempstring.Replace(@"""", "");
                                        int int1;
                                        if (tempstring[0] == '\0')
                                        {
                                            int1 = (int)tempstring[0];
                                            tempstring = int1.ToString() + tempstring.Substring(1);
                                        }
                                        if (tempstring.Contains("OK") || tempstring.Contains("E08") || tempstring.Contains("E10") || tempstring.Contains("E03")) leakRate = 100.00 + DateTime.Now.Millisecond / 1000000;
                                        else leakRate = Convert.ToDouble(tempstring);
                                    });
                            }
                            catch (Exception e)
                            {
                                ////commError = true;
                                //VtiEvent.Log.WriteError(
                                //    String.Format("Error communicating with {0}.", this.Name),
                                //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                //    e.ToString());
                                leakRate = 100.00 + DateTime.Now.Millisecond / 1000000;
                                this.BackgroundProcess();
                            }
                            finally
                            {
                            }
                        }
                        _DoNotCall = false;

                        if (!this.backgroundWorker1.IsBusy)
                            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                    }
                    catch (Exception e)
                    {
                        leakRate = 100.00 + DateTime.Now.Millisecond / 1000000;
                        this.BackgroundProcess();
                        //commError = true;
                    }
                    finally
                    {
                    }
                }

                #endregion get current leak rate
            }
            else if (Asleep)
            {
                leakRate = 100 + DateTime.Now.Millisecond / 1000000.0;
                this.BackgroundProcess();
            }

            #region //old code

            //    //get status
            //    InficonErrorCode = 0;
            //    try
            //    {
            //        serialPort1.DiscardInBuffer();

            //        serialPort1.WriteLine("*status?");

            //        Thread.Sleep(100);

            //        Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);

            //        if (this.IsAvailable)
            //        {
            //            try
            //            {
            //                Actions.Retry(3,
            //                    delegate
            //                    {
            //                        Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

            //                        byte[] _bytearray = new byte[50];
            //                        int n = serialPort1.BytesToRead;
            //                        int i;
            //                        for (i = 0; i < 50; i++)
            //                        {
            //                            _bytearray[i] = 0;
            //                        }
            //                        for (i = 0; i < n; i++)
            //                        {
            //                            byte tempbyte;
            //                            tempbyte = (byte)serialPort1.ReadByte();
            //                            if ((tempbyte != 10) && (tempbyte != 13))
            //                            {
            //                                _bytearray[i] = tempbyte;
            //                            }
            //                        }
            //                        ASCIIEncoding ascii = new ASCIIEncoding();

            //                        tempstring = ascii.GetString(_bytearray);
            //                        _LDSStatus = tempstring;
            //                    });
            //            }
            //            catch (Exception e)
            //            {
            //                ////commError = true;
            //                //VtiEvent.Log.WriteError(
            //                //    String.Format("Error communicating with {0}.", this.Name),
            //                //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
            //                //    e.ToString());
            //            }
            //            finally
            //            {
            //            }
            //        }
            //        _DoNotCall = false;

            //        if (!this.backgroundWorker1.IsBusy)
            //            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
            //    }
            //    catch (Exception e)
            //    {
            //        _LDSStatus = "CommError";
            //        leakRate = 100.0 + DateTime.Now.Millisecond / 1000000.0;
            //        this.BackgroundProcess();
            //        //commError = true;
            //    }
            //    finally
            //    {
            //    }
            //}
            //try
            //{
            //    serialPort1.DiscardInBuffer();

            //    serialPort1.WriteLine("*status:cal?");

            //    Thread.Sleep(100);

            //    Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);

            //    if (this.IsAvailable)
            //    {
            //        try
            //        {
            //            Actions.Retry(3,
            //                delegate
            //                {
            //                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

            //                    byte[] _bytearray = new byte[50];
            //                    int n = serialPort1.BytesToRead;
            //                    int i;
            //                    for (i = 0; i < 50; i++)
            //                    {
            //                        _bytearray[i] = 0;
            //                    }
            //                    for (i = 0; i < n; i++)
            //                    {
            //                        byte tempbyte;
            //                        tempbyte = (byte)serialPort1.ReadByte();
            //                        if ((tempbyte != 10) && (tempbyte != 13))
            //                        {
            //                            _bytearray[i] = tempbyte;
            //                        }
            //                    }
            //                    ASCIIEncoding ascii = new ASCIIEncoding();

            //                    tempstring = ascii.GetString(_bytearray);
            //                    _LDCalStatus = tempstring;
            //                });
            //        }
            //        catch (Exception e)
            //        {
            //            ////commError = true;
            //            //VtiEvent.Log.WriteError(
            //            //    String.Format("Error communicating with {0}.", this.Name),
            //            //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
            //            //    e.ToString());
            //        }
            //        finally
            //        {
            //        }
            //    }
            //    _DoNotCall = false;

            //    if (!this.backgroundWorker1.IsBusy)
            //        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
            //}
            //catch (Exception e)
            //{
            //    _LDCalStatus = "CommError";
            //    leakRate = 100.0 + DateTime.Now.Millisecond / 1000000.0;
            //    this.BackgroundProcess();
            //    //commError = true;
            //}
            //finally
            //{
            //}

            //try
            //{
            //    serialPort1.DiscardInBuffer();

            //    serialPort1.WriteLine("*config:calleak?");

            //    Thread.Sleep(100);

            //    Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);

            //    if (this.IsAvailable)
            //    {
            //        try
            //        {
            //            Actions.Retry(3,
            //                delegate
            //                {
            //                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

            //                    byte[] _bytearray = new byte[50];
            //                    int n = serialPort1.BytesToRead;
            //                    int i;
            //                    for (i = 0; i < 50; i++)
            //                    {
            //                        _bytearray[i] = 0;
            //                    }
            //                    for (i = 0; i < n; i++)
            //                    {
            //                        byte tempbyte;
            //                        tempbyte = (byte)serialPort1.ReadByte();
            //                        if ((tempbyte != 10) && (tempbyte != 13))
            //                        {
            //                            _bytearray[i] = tempbyte;
            //                        }
            //                    }
            //                    ASCIIEncoding ascii = new ASCIIEncoding();

            //                    tempstring = ascii.GetString(_bytearray);
            //                    _LDextLR = tempstring;
            //                });
            //        }
            //        catch (Exception e)
            //        {
            //            ////commError = true;
            //            //VtiEvent.Log.WriteError(
            //            //    String.Format("Error communicating with {0}.", this.Name),
            //            //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
            //            //    e.ToString());
            //        }
            //        finally
            //        {
            //        }
            //    }
            //    _DoNotCall = false;

            //    if (!this.backgroundWorker1.IsBusy)
            //        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
            //}
            //catch (Exception e)
            //{
            //    _LDCalStatus = "CommError";
            //    leakRate = 100.0 + DateTime.Now.Millisecond / 1000000.0;
            //    this.BackgroundProcess();
            //    //commError = true;
            //}
            //finally
            //{
            //}

            ////if error get error number
            //if (_LDSStatus.Contains("ERROR"))
            //{
            //    try
            //    {
            //        serialPort1.DiscardInBuffer();

            //        serialPort1.WriteLine("*status:error?");

            //        Thread.Sleep(100);

            //        Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);

            //        if (this.IsAvailable)
            //        {
            //            try
            //            {
            //                Actions.Retry(3,
            //                    delegate
            //                    {
            //                        Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

            //                        byte[] _bytearray = new byte[50];
            //                        int n = serialPort1.BytesToRead;
            //                        int i;
            //                        for (i = 0; i < 50; i++)
            //                        {
            //                            _bytearray[i] = 0;
            //                        }
            //                        for (i = 0; i < n; i++)
            //                        {
            //                            byte tempbyte;
            //                            tempbyte = (byte)serialPort1.ReadByte();
            //                            if ((tempbyte != 10) && (tempbyte != 13))
            //                            {
            //                                _bytearray[i] = tempbyte;
            //                            }
            //                        }
            //                        ASCIIEncoding ascii = new ASCIIEncoding();

            //                        tempstring = ascii.GetString(_bytearray);
            //                        InficonErrorCode = Convert.ToInt32(tempstring);
            //                    });
            //            }
            //            catch (Exception e)
            //            {
            //                ////commError = true;
            //                //VtiEvent.Log.WriteError(
            //                //    String.Format("Error communicating with {0}.", this.Name),
            //                //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
            //                //    e.ToString());
            //            }
            //            finally
            //            {
            //            }
            //        }
            //        _DoNotCall = false;

            //        if (!this.backgroundWorker1.IsBusy)
            //            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
            //    }
            //    catch (Exception e)
            //    {
            //        InficonErrorCode = 666;
            //        //commError = true;
            //    }
            //    finally
            //    {
            //    }

            //}

            #endregion //old code
        }

        /// <summary>
        /// When called, this method invokes the <see cref="InficonLDS2010Binary.OnValueChanged">OnValueChanged</see>
        /// method on the main thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            leakRateSignal.Value = leakRate;
            pressureSignal.Value = pressure;

            OnValueChanged();
        }

        //public bool GetExtCalStatus()
        //{
        //    String tempstring;
        //    if (!_DoNotCall)
        //    {
        //        //_DoNotCall = true;
        //        //get leak rate
        //        try
        //        {
        //            serialPort1.NewLine = "\r";

        //            serialPort1.DiscardInBuffer();

        //            serialPort1.WriteLine("*status:cal?");
        //            //serialPort1.WriteLine("LR");//****mdb3/23/2013

        //            Thread.Sleep(100);

        //            Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);

        //            if (this.IsAvailable)
        //            {
        //                try
        //                {
        //                    Actions.Retry(3,
        //                        delegate
        //                        {
        //                            Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

        //                            byte[] _bytearray = new byte[50];
        //                            int n = serialPort1.BytesToRead;
        //                            int i;
        //                            for (i = 0; i < 50; i++)
        //                            {
        //                                _bytearray[i] = 0;
        //                            }
        //                            for (i = 0; i < n; i++)
        //                            {
        //                                byte tempbyte;
        //                                tempbyte = (byte)serialPort1.ReadByte();
        //                                if ((tempbyte != 10) && (tempbyte != 13))
        //                                {
        //                                    _bytearray[i] = tempbyte;
        //                                }
        //                            }
        //                            ASCIIEncoding ascii = new ASCIIEncoding();

        //                            tempstring = ascii.GetString(_bytearray);
        //                            ExtCalStatus = tempstring;
        //                            return true;
        //                            //leakRate = Convert.ToDouble(tempstring);
        //                        });
        //                }
        //                catch (Exception e)
        //                {
        //                    ////commError = true;
        //                    //VtiEvent.Log.WriteError(
        //                    //    String.Format("Error communicating with {0}.", this.Name),
        //                    //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
        //                    //    e.ToString());
        //                    //leakRate = 100.00 + DateTime.Now.Millisecond / 1000000.0;
        //                    ExtCalStatus = "na";
        //                    this.BackgroundProcess();
        //                }

        //            }
        //            _DoNotCall = false;

        //            ExtCalStatus = "na";
        //            if (!this.backgroundWorker1.IsBusy)
        //                this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
        //        }
        //        catch (Exception e)
        //        {
        //            //leakRate = Double.NaN;
        //            //leakRate = 100.00 + DateTime.Now.Millisecond / 1000000.0;
        //            ExtCalStatus = "na";
        //            this.BackgroundProcess();
        //            //commError = true;
        //        }
        //    }
        //    else
        //    {
        //        ExtCalStatus = "na";
        //    }
        //}

        public bool GetExtCalStatus()
        {
            string tempstring;
            if (!_DoNotCall)
            {
                _DoNotCall = true;

                Thread.Sleep(50);
                serialPort1.DiscardInBuffer();
                serialPort1.WriteLine("*status:cal?");

                Thread.Sleep(150);
                Actions.WaitForUpTo(3000, () => this.serialPort1.BytesToRead > 2);

                //if (this.IsAvailable)
                //{
                //    try
                //    {
                //        Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);

                //        _DoNotCall = false;

                //        {
                //            return true;
                //        }

                //    }
                //    catch (Exception e)
                //    {
                //        ////commError = true;
                //        //VtiEvent.Log.WriteError(
                //        //    String.Format("Error communicating with {0}.", this.Name),
                //        //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                //        //    e.ToString());
                //    }
                //    finally
                //    {
                //    }
                //}
                if (this.IsAvailable)
                {
                    try
                    {
                        Actions.Retry(3,
                            delegate
                            {
                                Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);

                            //byte[] _bytearray = new byte[50];
                            //int n = serialPort1.BytesToRead;
                            //int i;
                            //for (i = 0; i < 50; i++)
                            //{
                            //    _bytearray[i] = 0;
                            //}
                            //for (i = 0; i < n; i++)
                            //{
                            //    byte tempbyte;
                            //    tempbyte = (byte)serialPort1.ReadByte();
                            //    if ((tempbyte != 10) && (tempbyte != 13))
                            //    {
                            //        _bytearray[i] = tempbyte;
                            //    }
                            //}
                            //ASCIIEncoding ascii = new ASCIIEncoding();

                            // tempstring = ascii.GetString(_bytearray);
                            ExtCalStatus = serialPort1.ReadLine();

                            //leakRate = Convert.ToDouble(tempstring);
                        });
                        return true;
                    }
                    catch (Exception e)
                    {
                        ////commError = true;
                        //VtiEvent.Log.WriteError(
                        //    String.Format("Error communicating with {0}.", this.Name),
                        //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        //    e.ToString());
                        //leakRate = 100.00 + DateTime.Now.Millisecond / 1000000.0;
                        ExtCalStatus = "na";
                        this.BackgroundProcess();
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Starts in Internal Calibration
        /// </summary>
        public bool StartInternalCal()
        {
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;

                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 6;
                    _bytearray[2] = 55;
                    _bytearray[3] = 0;
                    _bytearray[4] = 1;
                    _bytearray[5] = Convert.ToByte(_bytearray[0] + (byte)_bytearray[1] + (byte)_bytearray[2] + (byte)_bytearray[3] + (byte)_bytearray[4]);
                    serialPort1.Write(_bytearray, 0, 6);

                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            _DoNotCall = false;

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 175) && (ChecksumOK))
                            {
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            ////commError = true;
                            //VtiEvent.Log.WriteError(
                            //    String.Format("Error communicating with {0}.", this.Name),
                            //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            //    e.ToString());
                        }
                        finally
                        {
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Stops the Internal Calibration
        /// </summary>
        public bool StopInternalCal()
        {
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;

                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 6;
                    _bytearray[2] = 55;
                    _bytearray[3] = 0;
                    _bytearray[4] = 0;
                    _bytearray[5] = Convert.ToByte(_bytearray[0] + (byte)_bytearray[1] + (byte)_bytearray[2] + (byte)_bytearray[3] + (byte)_bytearray[4]);
                    serialPort1.Write(_bytearray, 0, 6);

                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            _DoNotCall = false;

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 175) && (ChecksumOK))
                            {
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            //// commError = true;
                            // VtiEvent.Log.WriteError(
                            //     String.Format("Error communicating with {0}.", this.Name),
                            //     VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            //     e.ToString());
                        }
                        finally
                        {
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Send the External Calibration Flow to the HLD6000
        /// </summary>
        public bool SendExternalCalFlow()
        {
            if (!_DoNotCall)
            {
                _CommandString = "*config:calleak " + _ExtCalLeakFlow;
                SendACommandString();
                _SendExternalCalLeakFlow = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Send a command string to the HLD6000
        /// </summary>
        public bool SendACommandString()
        {
            if (!_DoNotCall)
            {
                _DoNotCall = true;
                Thread.Sleep(50);
                serialPort1.DiscardInBuffer();
                serialPort1.WriteLine(_CommandString);
                Console.WriteLine("Write as command: " + _CommandString);
                _CommandString = "";
                //Thread.Sleep(150);
                if (this.IsAvailable)
                {
                    try
                    {
                        Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                        Console.WriteLine("Read frm Command: " + serialPort1.ReadExisting());
                        _SendCommandString = false;
                        _SendExternalCalLeakFlow = false;
                        _DoNotCall = false;
                        return true;
                    }
                    catch (Exception e)
                    {
                        ////commError = true;
                        //VtiEvent.Log.WriteError(
                        //    String.Format("Error communicating with {0}.", this.Name),
                        //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        //    e.ToString());
                    }
                    finally
                    {
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Starts an External Calibration
        /// </summary>
        public bool StartExternalCal()
        {
            if (!_DoNotCall)
            {
                _CommandString = "*cal";
                SendACommandString();
                _StartExternalCal = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AckExternalCal()
        {
            if (!_DoNotCall)
            {
                _DoNotCall = true;

                Thread.Sleep(50);
                serialPort1.DiscardInBuffer();
                serialPort1.WriteLine("*cal:acknowledge");
                Console.WriteLine("Write: *cal:acknowledge");

                Thread.Sleep(150);

                if (this.IsAvailable)
                {
                    try
                    {
                        Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                        string temp = serialPort1.ReadExisting();
                        Console.WriteLine("Read: " + temp);
                        _DoNotCall = false;

                        {
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        ////commError = true;
                        //VtiEvent.Log.WriteError(
                        //    String.Format("Error communicating with {0}.", this.Name),
                        //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        //    e.ToString());
                    }
                    finally
                    {
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool ExternalCalLeakClosed()
        {
            if (!_DoNotCall)
            {
                _DoNotCall = true;

                Thread.Sleep(50);
                serialPort1.DiscardInBuffer();
                serialPort1.WriteLine("*cal:closed");

                Thread.Sleep(150);

                if (this.IsAvailable)
                {
                    try
                    {
                        Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);

                        _DoNotCall = false;
                        //                _DoNotCall = false;
                        {
                            return true;
                        }
                        //                    return true;
                        //                }
                    }
                    catch (Exception e)
                    {
                        //            catch (Exception e)
                        //            {
                        //                ////commError = true;
                        //                //VtiEvent.Log.WriteError(
                        //                //    String.Format("Error communicating with {0}.", this.Name),
                        //                //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                        //                //    e.ToString());
                    }
                    finally
                    {
                        //            {
                    }
                }
                //        }
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Stops the External Calibration
        /// </summary>
        public bool StopExternalCal()
        {
            //SetByteParameter(55, 0, 1);

            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;

                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 6;
                    _bytearray[2] = 55;
                    _bytearray[3] = 1;
                    _bytearray[4] = 0;
                    _bytearray[5] = Convert.ToByte(_bytearray[0] + (byte)_bytearray[1] + (byte)_bytearray[2] + (byte)_bytearray[3] + (byte)_bytearray[4]);
                    serialPort1.Write(_bytearray, 0, 6);

                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            _DoNotCall = false;

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 175) && (ChecksumOK))
                            {
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            ////commError = true;
                            //VtiEvent.Log.WriteError(
                            //    String.Format("Error communicating with {0}.", this.Name),
                            //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            //    e.ToString());
                        }
                        finally
                        {
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool FinishExternalCal()
        {
            //SetByteParameter(55, 1, 2);

            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;

                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 6;
                    _bytearray[2] = 55;
                    _bytearray[3] = 1;
                    _bytearray[4] = 2;
                    _bytearray[5] = Convert.ToByte(_bytearray[0] + (byte)_bytearray[1] + (byte)_bytearray[2] + (byte)_bytearray[3] + (byte)_bytearray[4]);
                    serialPort1.Write(_bytearray, 0, 6);

                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            _DoNotCall = false;

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 175) && (ChecksumOK))
                            {
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            ////commError = true;
                            //VtiEvent.Log.WriteError(
                            //    String.Format("Error communicating with {0}.", this.Name),
                            //    VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            //    e.ToString());
                        }
                        finally
                        {
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Clears any error on the Leak Detector
        /// </summary>
        public void ClearError()
        {
            SetVoidParameter(63);
        }

        /// <summary>
        /// Cancels any ongoing calibration
        /// </summary>
        public void CancelCalibration()
        {
            SetVoidParameter(63);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets the current Leak Rate
        /// </summary>
        public override double Value
        {
            get { return leakRate; }
            internal set
            {
                leakRate = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Gets the Leak Rate formatted in scientific notation and including the units - value used in AnalogSignals.cs
        /// </summary>
        public override string FormattedValue
        {
            get
            {
                //if ((_LDSStatus.Contains("MEAS")) || (_LDSStatus.Contains("****")))
                //    return this.Value.ToString(this.Format) + " " + this.Units;
                //else
                //    return _LDSStatus;
                return leakRate.ToString("0.000") + " oz/yr";
            }
        }

        public bool Asleep { get; set; }

        public string LDStatus
        {
            get
            {
                return Regex.Replace(_LDSStatus, @"\0", "");
            }
        }

        public string LDCalStatus
        {
            get
            {
                return _LDCalStatus;
            }
        }

        public string LDextLR
        {
            get
            {
                //string formattedextLR = Regex.Replace(_LDextLR, @"\r", "");
                //string formatted2 = Regex.Replace(formattedextLR,@"E0"
                try
                {
                    //if (_LDextLR != null && !_LDextLR.Contains("OK") && _LDextLR.Length>5 && !_LDextLR.Contains("E0"))
                    //{
                    //    string temp = _LDextLR.Substring(0, 8);
                    //    return temp;
                    //}
                    //else
                    //{
                    //    return "999";
                    //}
                    if (_LDextLR.Contains("OK") || _LDextLR.Contains("E01")) return "999";
                    else { Console.WriteLine("Read _LDExtLR GetSet: " + _LDextLR); return _LDextLR; }
                    //double temp = Convert.ToDouble(_LDextLR);
                    //Console.WriteLine("Read: " + _LDextLR);
                    //return temp.ToString();
                }
                catch { return "999"; }
                // string temp = _LDextLR.Substring(0, 8);
                //return Regex.Replace(_LDextLR, @"\r", "");

                //remove everything after leak rate number
                //return _LDextLR.Substring(0, 8);
            }
        }

        /// <summary>
        /// Name for the class
        /// </summary>
        public override string Name
        {
            get { return "Inficon LDS2010 Leak Detector on port " + this.SerialPort.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Minimum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Min
        {
            get { return min; }
        }

        /// <summary>
        /// Maximum value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override double Max
        {
            get { return max; }
        }

        /// <summary>
        /// Units for the displayed value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Units
        {
            //get { return leakRateUnitNames[(int)this.leakRateUnits]; }
            get { return "oz/yr"; }
            set { leakRateUnitNames[(int)this.leakRateUnits] = value; }
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

        public Double Pressure
        {
            get
            {
                return pressure; // GetFloatParameter(2, 1, (byte)pressureUnits, 0);
            }
        }

        public PressureUnitsType PressureUnits
        {
            get
            {
                if (pressureUnits == null)
                    pressureUnits = (PressureUnitsType)GetByteParameter(92, 2);
                return (PressureUnitsType)pressureUnits;
            }
            set
            {
                pressureUnits = value;
                pressureSignal.Units = pressureUnitNames[(int)pressureUnits];
                SetByteParameter(93, (byte)value, 2);
            }
        }

        /// <summary>
        /// Gets the current Leak Rate of the Leak Detector
        /// </summary>
        public Double LeakRate
        {
            get
            {
                return leakRate; // GetFloatParameter(99, 1, (byte)leakRateUnits, 0);
            }
        }

        /// <summary>
        /// Gets or Sets the units for the <see cref="LeakRate">Leak Rate</see>
        /// </summary>
        public LeakRateUnitsType LeakRateUnits
        {
            get
            {
                if (leakRateUnits == null)
                    leakRateUnits = (LeakRateUnitsType)GetByteParameter(92, 0);
                return (LeakRateUnitsType)leakRateUnits;
            }
            set
            {
                leakRateUnits = value;
                leakRateSignal.Units = leakRateUnitNames[(int)this.leakRateUnits];
                //SetByteParameter(93, (byte)value, 0);
            }
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
        /// Foreline pressure as an AnalogSignal
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogSignal PressureSignal
        {
            get { return pressureSignal; }
        }

        /// <summary>
        /// Gets or sets a value indicating the state of the Gas Ballast setting
        /// </summary>
        public GasBallastStates GasBallastState
        {
            get { return (GasBallastStates)GetByteParameter(8); }
            set { SetByteParameter(9, (byte)value); }
        }

        /// <summary>
        /// Gets or sets the value of the Calibration Factor for Vacuum Mode
        /// </summary>
        public Single CalFactorVacuum
        {
            get { return GetFloatParameter(36, 0); }
            set { SetFloatParameter(37, value, 0); }
        }

        /// <summary>
        /// Gets or sets the value of the Calibration Factor for Sniff Mode
        /// </summary>
        public Single CalFactorSniff
        {
            get { return GetFloatParameter(36, 1); }
            set { SetFloatParameter(37, value, 1); }
        }

        /// <summary>
        /// Gets or sets a value indicating the atomic mass setting
        /// </summary>
        public Byte Mass
        {
            get { return GetByteParameter(40); }
            set { SetByteParameter(41, Math.Max(Math.Min(value, (byte)2), (byte)4)); }
        }

        /// <summary>
        /// Gets or sets a value to enable the zero (background suppression)
        /// </summary>
        public Boolean Zero
        {
            get { return GetByteParameter(50) == 1; }
            set { SetByteParameter(51, value ? (byte)1 : (byte)0); }
        }

        /// <summary>
        /// Gets or sets a value indicating if the Display Range should be between the
        /// <see cref="LowLimit">Low Limit</see> and <see cref="HighLimit">High Limit</see>
        /// </summary>
        public Boolean Manual
        {
            get { return GetBoolParameter(52); }
            set { SetBoolParameter(53, value); }
        }

        /// <summary>
        /// Gets the state of the internal calibration
        /// </summary>
        public CalStates InternalCalState
        {
            get { return (CalStates)GetByteParameter(54, (byte)CalModes.Internal); }
        }

        /// <summary>
        /// Gets the state of the external calibration
        /// </summary>
        public CalStates ExternalCalState
        {
            get { return (CalStates)GetByteParameter(54, (byte)CalModes.External); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 1
        /// </summary>
        public Single Trigger1
        {
            get { return GetFloatParameter(56, 1, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 1, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 2
        /// </summary>
        public Single Trigger2
        {
            get { return GetFloatParameter(56, 2, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 2, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 3
        /// </summary>
        public Single Trigger3
        {
            get { return GetFloatParameter(56, 3, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 3, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the value of Trigger 4
        /// </summary>
        public Single Trigger4
        {
            get { return GetFloatParameter(56, 4, (byte)leakRateUnits); }
            set { SetFloatParameter(57, value, 4, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the current operating mode
        /// </summary>
        public OpModes OpMode
        {
            get { return (OpModes)GetByteParameter(58); }
            set { SetByteParameter(59, (byte)value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the current measurement mode
        /// </summary>
        public MeasurementModes MeasureMode
        {
            get { return (MeasurementModes)GetByteParameter(60); }
            set { SetByteParameter(61, (byte)value); }
        }

        public bool SetClearError
        {
            get
            {
                return _ClearError;
            }
            set
            {
                _ClearError = value;
            }
        }

        public bool SendExtCalLeakFlow
        {
            get
            {
                return _SendExternalCalLeakFlow;
            }
            set
            {
                _SendExternalCalLeakFlow = value;
            }
        }

        public string ExtCalLeakFlowValue
        {
            get
            {
                //Console.WriteLine("Read Ext. Flow: " + _ExtCalLeakFlow);
                return _ExtCalLeakFlow;
            }
            set
            {
                _ExtCalLeakFlow = value;
            }
        }

        public bool SendCommandString
        {
            get
            {
                return _SendCommandString;
            }
            set
            {
                _SendCommandString = value;
            }
        }

        public String CommandString
        {
            get
            {
                return _CommandString;
            }
            set
            {
                _CommandString = value;
            }
        }

        public bool SetStandbyOn
        {
            get
            {
                return _StandbyOn;
            }
            set
            {
                _StandbyOn = value;
            }
        }

        public bool PollForExtLR
        {
            //get
            //{
            //    //return _PollForExtLR;
            //}
            set
            {
                _PollForExtLR = value;
            }
        }

        public bool PollForLR
        {
            set
            {
                _PollForLR = value;
            }
        }

        public bool PollForLDStatus
        {
            get
            {
                return _PollForLDStatus;
            }
            set
            {
                _PollForLDStatus = value;
            }
        }

        public bool PollForLDCalStatus
        {
            get
            {
                return _PollForLDCalStatus;
            }
            set
            {
                _PollForLDCalStatus = value;
            }
        }

        public bool SetStandbyOff
        {
            get
            {
                return _StandbyOff;
            }
            set
            {
                _StandbyOff = value;
            }
        }

        public bool StartExtCal
        {
            get
            {
                return _StartExternalCal;
            }
            set
            {
                _StartExternalCal = value;
            }
        }

        public bool CloseExtCalLeak
        {
            get
            {
                return _CloseExternalCalLeak;
            }
            set
            {
                _CloseExternalCalLeak = value;
            }
        }

        /// <summary>
        /// Gets the current Error Code (0 = no error)
        /// </summary>
        public int ErrorCode
        {
            get
            {
                Console.WriteLine("Error Code: " + InficonErrorCode);
                return InficonErrorCode;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the Low Limit
        /// </summary>
        public Single LowLimit
        {
            get { return GetFloatParameter(64, 0, (byte)leakRateUnits); }
            set { SetFloatParameter(65, value, 0, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets a value indicating the High Limit
        /// </summary>
        public Single HighLimit
        {
            get { return GetFloatParameter(64, 1, (byte)leakRateUnits); }
            set { SetFloatParameter(65, value, 1, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the leak rate value of the Internal Calibrated Leak
        /// </summary>
        public Single InternalCalLeak
        {
            get { return GetFloatParameter(66, 0, (byte)leakRateUnits); }
            set { SetFloatParameter(67, value, 0, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the leak rate value of the External Calibrated Leak
        /// </summary>
        public Single ExternalCalLeak
        {
            get { return GetFloatParameter(66, 1, (byte)leakRateUnits); }
            set { SetFloatParameter(67, value, 1, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets the leak rate value of the Sniffer Calibrated Leak
        /// </summary>
        public Single SnifferCalLeak
        {
            get { return GetFloatParameter(66, 2, (byte)leakRateUnits); }
            set { SetFloatParameter(67, value, 2, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets or sets a value indicating the leak rate for switching the averaging time
        /// </summary>
        public Single FilterSetPoint
        {
            get { return GetFloatParameter(68, (byte)leakRateUnits); }
            set { SetFloatParameter(69, value, (byte)leakRateUnits); }
        }

        /// <summary>
        /// Gets the Serial Number of the device
        /// </summary>
        public string SerialNumber
        {
            get { return GetStringParameter(70); }
        }

        /// <summary>
        /// Gets the current operating state
        /// </summary>
        public States State
        {
            get { return (States)GetByteParameter(72); }
        }

        /// <summary>
        /// Gets a value indicating if the averaging filter is active
        /// </summary>
        public Boolean FilterActive
        {
            get { return GetBoolParameter(73); }
        }

        /// <summary>
        /// Gets the operating hours of the device
        /// </summary>
        public uint OpHours
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the software version (Byte 0: Main-Version, Byte 1: Sub-Version)
        /// </summary>
        public ushort SoftwareVersion
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the value of the Machine calibration factor
        /// </summary>
        public Single MachineFactor
        {
            get { return GetFloatParameter(78); }
            set { SetFloatParameter(79, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the External Calibration Mode
        /// </summary>
        public ExtCalModes ExtCalMode
        {
            get { return (ExtCalModes)GetByteParameter(80); }
            set { SetByteParameter(80, (byte)value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the type of Zero Mode
        /// </summary>
        public ZeroModes ZeroMode
        {
            get { return (ZeroModes)GetByteParameter(82); }
            set { SetByteParameter(83, (byte)value); }
        }

        /// <summary>
        /// Gets or sets the audio volume
        /// </summary>
        public byte AudioVolume
        {
            get { return GetByteParameter(88); }
            set { SetByteParameter(89, Math.Max(Math.Min(value, (byte)1), (byte)15)); }
        }

        /// <summary>
        /// Gets or sets the audio mode
        /// </summary>
        public AudioModes AudioMode
        {
            get { return (AudioModes)GetByteParameter(90); }
            set { SetByteParameter(91, (byte)value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the control mode
        /// </summary>
        public ControlModes ControlMode
        {
            get { return (ControlModes)GetByteParameter(94); }
            set { SetByteParameter(95, (byte)value); }
        }

        #endregion Public Properties
    }
}