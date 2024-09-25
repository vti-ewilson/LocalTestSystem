using System;
using System.Diagnostics;
using System.Threading;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an TGuard Controller
    /// </summary>
    public class TGuardController : SerialIOBase
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

        private Double _pressure;
        private Single _min = 0, _max = 1000;
        private String _units = "amps";
        private String _format = "0.00E-00";
        private Boolean _sp1Latch = true, _sp2Latch = true, _sp1ActiveHigh = true, _sp2ActiveHigh = false;
        private int _decimalPlaces = 3;

        private Boolean _DoNotCall;

        private Stopwatch _errorSW = new Stopwatch();

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PVGC5Controller">PVGC5Controller</see> class
        /// </summary>
        public TGuardController()
          : base()
        {
            _units = Units;
            //this.BaudRate = 2400;
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
        /// Thread for reading the <see cref="Value">Value</see> of the T Guard Controller
        /// </summary>
        public override void Process()
        {
            //if (Monitor.TryEnter(this.SerialLock, 500))
            //Monitor.Enter(this.SerialLock);
            {
                try
                {
                    serialPort1.DiscardInBuffer();
                    //serialPort1.Write(string.Format("{0}{1}{2}{3}",(char)0x05,(char)4,(char)97,(char)106));

                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 4;
                    _bytearray[2] = 97;
                    _bytearray[3] = 106;
                    //                   _bytearray[4] = 200;
                    serialPort1.Write(_bytearray, 0, 4);

                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 5);
                    byte[] retData = null;
                    byte len;
                    //if (this.IsAvailable && !commError && Monitor.TryEnter(this.SerialLock, 3000))
                    if (this.IsAvailable)
                    {
                        try
                        {
                            Actions.Retry(3,
                                delegate
                                {
                      //CalculateCheckSumAndSend(Message);
                      Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                                    len = (byte)this.serialPort1.ReadByte();
                                    retData = new byte[len];
                                    retData[0] = len;
                                    this.serialPort1.Read(retData, 1, len - 1);
                                });
                        }
                        catch (Exception e)
                        {
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
                        }
                    }

                    _pressure = BitConverter.ToSingle(new byte[] { retData[4], retData[5], retData[3], retData[2] }, 0);
                    //_pressure = Convert.ToDouble(serialPort1.ReadLine());
                    if (!this.backgroundWorker1.IsBusy)
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                }
                catch (Exception e)
                {
                    _pressure = Double.NaN;
                    commError = true;
                }
                finally
                {
                    //Monitor.Exit(this.SerialLock);
                }
            }
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

        /// <summary>
        /// Value (amps) of the T Guard Controller
        /// </summary>
        public override double Value
        {
            get
            {
                //lock (SerialLock)
                //{
                //    if (_pressureReady)
                //    {
                return _pressure;
                //}
                //else
                //{
                //    try
                //    {
                //        if (!serialPort1.IsOpen) serialPort1.Open();
                //        serialPort1.Write("p");
                //        return Convert.ToDouble(serialPort1.ReadLine().Substring(2));
                //    }
                //    catch (Exception e)
                //    {
                //        if (_errorSW.IsRunning == false || _errorSW.Elapsed.Minutes >= 5)
                //        {
                //            VtiEvent.Log.WriteError("Error reading Pressure from PVGC5 on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                //            if (_errorSW.IsRunning) _errorSW.Reset();
                //            _errorSW.Start();
                //        }
                //        return Double.NaN;
                //    }
                //}
                //}
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
        /// Name for the Controller
        /// </summary>
        public override string Name
        {
            get { return "T Guard Controller on port " + this.PortName; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        ///

        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the TorrCon II Controller
        /// </summary>
        public override double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Maximum value for the T Guard controller
        /// </summary>
        public override double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Units for the value for the T Guard controller
        /// </summary>
        public override string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Format string for the value for the T Guard controller
        /// </summary>
        public override string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Setpoint 1 of the T Guard controller
        /// </summary>
        public Single Setpoint1
        {
            get
            {
                Single temp = Single.NaN;
                //Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("1");
                    temp = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
                }
                catch (Exception e)
                {
                    commError = true;
                    VtiEvent.Log.WriteError("Error reading Setpoint 1 from T Guard Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
                }
                finally
                {
                    //Monitor.Exit(this.SerialLock);
                }
                return temp;
            }
            set
            {
                //Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    SendWithDelay(string.Format("!{0:0.000}\r", value));
                    String s = serialPort1.ReadLine(); // throw away the return value
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error setting Setpoint 1 on T Guard Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    //Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Setpoint 2 of the T Guard controller
        /// </summary>
        public Single Setpoint2
        {
            get
            {
                Single temp;
                //Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.Write("2");
                    temp = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error reading Setpoint 2 from TorrCon II Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    //Monitor.Exit(this.SerialLock);
                }
                return temp;
            }
            set
            {
                //Monitor.Enter(this.SerialLock);
                try
                {
                    if (!serialPort1.IsOpen) serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    SendWithDelay(string.Format("@{0:0.000}\r", value));
                    String s = serialPort1.ReadLine(); // throw away the return value
                }
                catch
                {
                    commError = true;
                    throw new Exception("Error setting Setpoint 2 on TorrCon II Controller on port " + serialPort1.PortName);
                }
                finally
                {
                    //Monitor.Exit(this.SerialLock);
                }
            }
        }

        /// <summary>
        /// Status of the T Guard controller
        /// </summary>
        public string TGuardStatus
        {
            get
            {
                Byte ResultByte;
                String ResultString;

                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                ResultString = "ERROR";
                if (!DoNotCall)
                {
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    //serialPort1.Write(string.Format("{0}{1}{2}{3}", (char)0x05, (char)4, (char)44, (char)53));

                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 4;
                    _bytearray[2] = 44;
                    _bytearray[3] = 53;
                    //                  _bytearray[4] = 200;
                    serialPort1.Write(_bytearray, 0, 4);

                    //                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 3);
                    //                    byte[] retData = null;
                    //                    byte len;

                    Thread.Sleep(50);

                    //if (this.IsAvailable && !commError && Monitor.TryEnter(this.SerialLock, 3000))
                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });

                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 44) && (ChecksumOK))
                            {
                                ResultByte = retData[2];
                                switch (ResultByte)
                                {
                                    case 1:
                                        ResultString = "StartStandby";
                                        break;

                                    case 2:
                                        ResultString = "Standby";
                                        break;

                                    case 3:
                                        ResultString = "Contaminated";
                                        break;

                                    case 4:
                                        ResultString = "RunUp";
                                        break;

                                    case 5:
                                        ResultString = "StartAccumulation";
                                        break;

                                    case 8:
                                        ResultString = "NoPurge";
                                        break;

                                    case 10:
                                        ResultString = "AccGross1";
                                        break;

                                    case 20:
                                        ResultString = "AccFine1";
                                        break;

                                    case 25:
                                        ResultString = "AccWait";
                                        break;

                                    case 30:
                                        ResultString = "AccFine2";
                                        break;

                                    case 32:
                                        ResultString = "AccGross2";
                                        break;

                                    case 36:
                                        ResultString = "WaitPurge";
                                        break;

                                    case 38:
                                        ResultString = "Purge";
                                        break;

                                    case 40:
                                        ResultString = "Ready";
                                        break;

                                    case 50:
                                        ResultString = "CGStart";
                                        break;

                                    case 55:
                                        ResultString = "CGGross";
                                        break;

                                    case 65:
                                        ResultString = "CGFine";
                                        break;

                                    case 70:
                                        ResultString = "CGRef";
                                        break;

                                    case 150:
                                        ResultString = "CMStart";
                                        break;

                                    case 155:
                                        ResultString = "CMGross";
                                        break;

                                    case 160:
                                        ResultString = "CMFine";
                                        break;

                                    case 165:
                                        ResultString = "CMStop";
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            //    commError = true;
                            //    VtiEvent.Log.WriteError(
                            //        String.Format("Error communicating with {0}.", this.Name),
                            //        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            //        e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
                        }
                    }
                }
                return ResultString;
            }
            //            set
            //            {
            //                _format = value;
            //            }
        }

        public Double TGuardSignal
        {
            get
            {
                Double ResultDouble;
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                ResultDouble = Double.NaN;
                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    //serialPort1.Write(string.Format("{0}{1}{2}{3}", (char)0x05, (char)4, (char)97, (char)106));
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 4;
                    _bytearray[2] = 97;
                    _bytearray[3] = 106;
                    //                    _bytearray[4] = 200;
                    serialPort1.Write(_bytearray, 0, 4);

                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 5);
                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(0,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });

                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 5);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }
                            else
                            {
                                ChecksumOK = false;
                            }

                            if ((retData[1] == 97) && (ChecksumOK))
                            {
                                ResultDouble = BitConverter.ToSingle(new byte[] { retData[4], retData[5], retData[3], retData[2] }, 0);
                            }
                            else
                            {
                                if (retData[0] == 97)
                                {
                                    ResultDouble = BitConverter.ToSingle(new byte[] { retData[3], retData[4], retData[2], retData[1] }, 0);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            //    commError = true;
                            //    VtiEvent.Log.WriteError(
                            //        String.Format("Error communicating with {0}.", this.Name),
                            //        VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            //        e.ToString());
                        }
                        finally
                        {
                            // Monitor.Exit(this.SerialLock);
                        }
                    }
                }
                _DoNotCall = false;
                if ((ResultDouble > 1E-11) && (ResultDouble < 1E-8))
                {
                }
                else
                {
                    _DoNotCall = false;
                }

                return ResultDouble;
            }
            //            set
            //            {
            //                _format = value;
            //            }
        }

        public Double TGuardPressure
        {
            get
            {
                Double ResultDouble;

                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;

                byte[] retData = null;
                byte len = 0;

                ResultDouble = Double.NaN;
                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    //serialPort1.Write(string.Format("{0}{1}{2}{3}{4}", (char)5, (char)5, (char)2, (char)0, (char)12));
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 5;
                    _bytearray[2] = 2;
                    _bytearray[3] = 0;
                    _bytearray[4] = 12;
                    serialPort1.Write(_bytearray, 0, 5);

                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 5);
                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                        Actions.Retry(3,
                            //                            delegate
                            //                            {
                            //                                //CalculateCheckSumAndSend(Message);
                            //                                Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                len = (byte)this.serialPort1.ReadByte();
                            //                                retData = new byte[len];
                            //                                retData[0] = len;
                            //                                this.serialPort1.Read(retData, 1, len - 1);
                            //                            });

                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 5);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 2) && (ChecksumOK))
                            {
                                ResultDouble = BitConverter.ToSingle(new byte[] { retData[4], retData[5], retData[3], retData[2] }, 0);
                            }
                        }
                        catch (Exception e)
                        {
                            // commError = true;
                            // VtiEvent.Log.WriteError(
                            //     String.Format("Error communicating with {0}.", this.Name),
                            //     VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            //     e.ToString());
                        }
                        finally
                        {
                            // Monitor.Exit(this.SerialLock);
                        }
                    }
                }
                _DoNotCall = false;
                return ResultDouble;
            }
        }

        //       public Single TGuardForePress()
        //       {
        //       }

        public bool DoNotCall
        {
            get
            {
                return _DoNotCall;
            }
            set
            {
                _DoNotCall = value;
            }
        }

        public bool StartTGuard
        {
            get
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    //                    serialPort1.Write(string.Format("{0}{1}{2}{3}", (char)0x05, (char)4, (char)52, (char)61));

                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 4;
                    _bytearray[2] = 52;
                    _bytearray[3] = 61;
                    //                  _bytearray[4] = 200;
                    serialPort1.Write(_bytearray, 0, 4);

                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 2);
                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });
                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 5);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            _DoNotCall = false;
                            //                    Boolean ChecksumOK = false;
                            //                    Int16 i;
                            //                    Int32 Sum = 0;

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 52) && (ChecksumOK))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception e)
                        {
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
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

        public bool StopTGuard
        {
            get
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    //                    serialPort1.Write(string.Format("{0}{1}{2}{3}", (char)0x05, (char)4, (char)53, (char)62));

                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 4;
                    _bytearray[2] = 53;
                    _bytearray[3] = 62;
                    //                  _bytearray[4] = 200;
                    serialPort1.Write(_bytearray, 0, 4);

                    //                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 2);
                    //                    byte[] retData = null;
                    //                    byte len;
                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });

                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 5);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            _DoNotCall = false;
                            //Boolean ChecksumOK = false;
                            //Int16 i;
                            //Int32 Sum = 0;

                            if (retData == null) return false;

                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 53) && (ChecksumOK))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception e)
                        {
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
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

        public bool SetContMode
        {
            get
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    //serialPort1.Write(string.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)5, (char)59, (char)3, (char)72));

                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 5;
                    _bytearray[2] = 59;
                    _bytearray[3] = 3;
                    _bytearray[4] = 72;
                    serialPort1.Write(_bytearray, 0, 5);

                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 2);
                    //byte[] retData = null;
                    //byte len;

                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });

                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 5);
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

                            if ((retData[1] == 59) && (ChecksumOK))
                            {
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
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

        public bool SetStandbyOn
        {
            get
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 5;
                    _bytearray[2] = 189;
                    _bytearray[3] = 1;
                    _bytearray[4] = 200;
                    serialPort1.Write(_bytearray, 0, 5);
                    //serialPort1.Write(string.Format("{0}{1}{2}{3}{4}", (byte)0x05, (byte)5, (byte)189,(byte)1, (byte)200));
                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 2);
                    //byte[] retData = null;
                    //byte len;
                    Thread.Sleep(50);
                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });
                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);

                            _DoNotCall = false;

                            //Boolean ChecksumOK = false;
                            //Int16 i;
                            //Int32 Sum = 0;
                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 189) && (ChecksumOK))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception e)
                        {
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
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

        public bool SetStandbyOff
        {
            get
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 5;
                    _bytearray[2] = 189;
                    _bytearray[3] = 2;
                    _bytearray[4] = 201;
                    serialPort1.Write(_bytearray, 0, 5);

                    //serialPort1.Write(string.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)5, (char)189, (char)2, (char)201));
                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 2);
                    //byte[] retData = null;
                    //byte len;
                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });

                            Actions.WaitForUpTo(250, () => this.serialPort1.BytesToRead > 2);
                            len = (byte)this.serialPort1.ReadByte();
                            retData = new byte[len];
                            retData[0] = len;
                            this.serialPort1.Read(retData, 1, len - 1);
                            _DoNotCall = false;
                            //                            Boolean ChecksumOK = false;
                            //                            Int16 i;
                            //                            Int32 Sum = 0;
                            for (i = 0; (i <= (retData[0] - 2)); i++)
                            {
                                Sum = Sum + retData[i];
                            }
                            if (retData[retData[0] - 1] == (byte)(Sum % 256))
                            {
                                ChecksumOK = true;
                            }

                            if ((retData[1] == 189) && (ChecksumOK))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception e)
                        {
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
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

        public bool SetGrossOn
        {
            get
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 5;
                    _bytearray[2] = 175;
                    _bytearray[3] = 1;
                    _bytearray[4] = 186;
                    serialPort1.Write(_bytearray, 0, 5);

                    //serialPort1.Write(string.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)5, (char)175, (char)1, (char)186));
                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 2);
                    //byte[] retData = null;
                    //byte len;
                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });
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
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
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

        public bool SetGrossOff
        {
            get
            {
                Boolean ChecksumOK = false;
                Int16 i;
                Int32 Sum = 0;
                byte[] retData = null;
                byte len = 0;

                if (!_DoNotCall)
                {
                    _DoNotCall = true;
                    //Monitor.Enter(this.SerialLock);
                    Thread.Sleep(50);
                    serialPort1.DiscardInBuffer();
                    byte[] _bytearray = new byte[6];
                    _bytearray[0] = 5;
                    _bytearray[1] = 5;
                    _bytearray[2] = 175;
                    _bytearray[3] = 0;
                    _bytearray[4] = 185;
                    serialPort1.Write(_bytearray, 0, 5);

                    //serialPort1.Write(string.Format("{0}{1}{2}{3}{4}", (char)0x05, (char)5, (char)175, (char)0, (char)185));
                    //Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 2);
                    //byte[] retData = null;
                    //byte len;
                    Thread.Sleep(50);

                    if (this.IsAvailable)
                    {
                        try
                        {
                            //                            Actions.Retry(3,
                            //                                delegate
                            //                                {
                            //                                    //CalculateCheckSumAndSend(Message);
                            //                                    Actions.WaitForUpTo(300, () => this.serialPort1.BytesToRead > 0);
                            //                                    len = (byte)this.serialPort1.ReadByte();
                            //                                    retData = new byte[len];
                            //                                    retData[0] = len;
                            //                                    this.serialPort1.Read(retData, 1, len - 1);
                            //                                });

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
                            commError = true;
                            VtiEvent.Log.WriteError(
                                String.Format("Error communicating with {0}.", this.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                                e.ToString());
                        }
                        finally
                        {
                            //Monitor.Exit(this.SerialLock);
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

        #endregion Public Properties
    }
}