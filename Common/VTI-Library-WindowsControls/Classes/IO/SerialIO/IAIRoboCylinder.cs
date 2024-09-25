using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    /// <summary>
    /// Serial Interface for an IAI RoboCylinder
    /// </summary>
    public partial class IAIRoboCylinder : SerialIOBase
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

        private ASCIIEncoding ascii = new ASCIIEncoding();
        private StatusClass _status = new StatusClass();
        private String _response;
        private Single _lead = 6, _length = 300;
        private Boolean _positionReady;
        private Single _position;
        private Byte _axis = 0;
        private String _format = "0.00";
        private String _Units = "mm";
        private Boolean _homePositionIsMotorEnd = false;

        private String _CheckStatusCommand = "n0000000000";
        private String _commandString;

        private char[] _charArrayResponse = new char[16];
        private Stopwatch _responseSw = new Stopwatch();

        #endregion Globals

        #region Nested Classes

        private enum AddressLocation
        {
            PositionData = 0x00000400,
            PositionBand = 0x00000403,
            Velocity = 0x00000404,
            AccelerationDeceleration = 0x00000405,
            PushForce = 0x00000406,
            PushTime = 0x00000407,
            MaxAccFlag = 0x00000409
        }

        /// <summary>
        /// Defines the status of the RoboCylinder
        /// </summary>
        public class StatusClass
        {
            internal Boolean _power, _servo, _run, _home, _commandRefused;
            internal Byte _alarms, _inputs, _outputs;

            /// <summary>
            /// Indicates if the power is supplied to the RoboCylinder
            /// </summary>
            public Boolean Power
            {
                get { return _power; }
            }

            /// <summary>
            /// Indicates if the Servo for the RoboCylinder is on
            /// </summary>
            public Boolean Servo
            {
                get { return _servo; }
            }

            /// <summary>
            /// Indicates if the RoboCylinder is running
            /// </summary>
            public Boolean Run
            {
                get { return _run; }
            }

            /// <summary>
            /// Indicates if the RoboCylinder is in the Home position
            /// </summary>
            public Boolean Home
            {
                get { return _home; }
            }

            /// <summary>
            /// Indicates that the last command was refused
            /// </summary>
            public Boolean CommandRefused
            {
                get { return _commandRefused; }
            }

            /// <summary>
            /// Alarm value
            /// </summary>
            public Byte Alarms
            {
                get { return _alarms; }
            }

            /// <summary>
            /// Input value
            /// </summary>
            public Byte Inputs
            {
                get { return _inputs; }
            }

            /// <summary>
            /// Outputs value
            /// </summary>
            public Byte Outputs
            {
                get { return _outputs; }
            }
        }

        #endregion Nested Classes

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="IAIRoboCylinder">IAIRoboCylinder</see> class
        /// </summary>
        public IAIRoboCylinder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IAIRoboCylinder">IAIRoboCylinder</see> class
        /// </summary>
        public IAIRoboCylinder(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion Construction

        #region Private Members

        private byte CalculateBCC(String Command)
        {
            //int sum = 0;

            if (Command.Length != 12) return 0;

            //foreach (char c in Command.ToCharArray())
            //    sum += Convert.ToByte(c);

            //sum = ~sum + 1;

            //return (Byte)(sum & 0xFF);

            return (byte)((~Command.ToCharArray().Sum(c => c) + 1) & 0xFF);
        }

        private Boolean SendCommand(String Command)
        {
            //String s;
            try
            {
                serialPort1.DiscardInBuffer();
                _commandString = String.Format("{0:X1}{1}", _axis, Command);
                _commandString = String.Format("{0}{1}{2:X2}{3}", (char)2, _commandString, CalculateBCC(_commandString), (char)3);
                //Debug.WriteLine("Sent to RoboCylinder: '" + s + "'");
                Thread.Sleep(25);
                serialPort1.Write(_commandString);
                Thread.Sleep(25);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private Boolean ReadResponse()
        {
            //char[] cb = new char[16];
            //Stopwatch sw = new Stopwatch();
            try
            {
                _responseSw.Start();
                while (serialPort1.BytesToRead < 16)
                {
                    if (_responseSw.Elapsed.Milliseconds > 500)
                    {
                        _responseSw.Reset();
                        serialPort1.DiscardInBuffer();
                        return false;
                    }
                    Thread.Sleep(0);
                }
                serialPort1.Read(_charArrayResponse, 0, 16);
                _response = new String(_charArrayResponse);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private Boolean ParseStatusCodes()
        {
            Byte b;
            try
            {
                b = Byte.Parse(_response.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                _status._power = ((b & (1 << 0)) != 0);
                _status._servo = ((b & (1 << 1)) != 0);
                _status._run = ((b & (1 << 2)) != 0);
                _status._home = ((b & (1 << 3)) != 0);
                _status._commandRefused = ((b & (1 << 7)) != 0);

                _status._alarms = Byte.Parse(_response.Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                _status._inputs = Byte.Parse(_response.Substring(8, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                _status._outputs = Byte.Parse(_response.Substring(10, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private Single EncoderPulsesToPosition(String EncoderPulses)
        {
            //Debug.WriteLine( String.Format("{0}", (0xFFFFFFFF - Convert.ToUInt32(EncoderPulses, 16) + 1)));
            //uint i = Convert.ToUInt32(EncoderPulses, 16);
            if (_homePositionIsMotorEnd)
                return (Single)(-Convert.ToInt32(EncoderPulses, 16) * _lead / 800);
            else
                return (Single)(Convert.ToInt32(EncoderPulses, 16) * _lead / 800);
        }

        private String PositionToEncoderPulses(Single Position)
        {
            if (_homePositionIsMotorEnd)
                return String.Format("{0:X8}", Convert.ToInt32(-Position * 800 / _lead + 1));
            else
                return String.Format("{0:X8}", Convert.ToInt32(Position * 800 / _lead));
        }

        private Boolean TransferAtoB(Byte Position)
        {
            try
            {
                if (SendCommand(String.Format("Q1010{0:X1}00000", Position)))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            catch
            {
            }
            return false;
        }

        private Boolean AddressAllocation(AddressLocation Address)
        {
            try
            {
                if (SendCommand(String.Format("T4{0:X8}0", Convert.ToInt32(Address))))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            catch
            {
            }
            return false;
        }

        private Boolean DataWrite(String Data)
        {
            try
            {
                if (SendCommand(String.Format("W4{0}0", Data)))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            catch
            {
            }
            return false;
        }

        private Boolean TransferBtoA(Byte Position)
        {
            try
            {
                if (SendCommand(String.Format("V5010{0:X1}00000", Position)))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            catch
            {
            }
            return false;
        }

        #endregion Private Members

        #region Public Members

        /// <summary>
        /// Retrieves the status of the RoboCylinder
        /// </summary>
        /// <returns>True if successful</returns>
        public Boolean CheckStatus()
        {
            bool retVal = false;
            //lock(SerialLock)
            //{
            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                try
                {
                    if (SendCommand(_CheckStatusCommand))
                        if (ReadResponse())
                            retVal = ParseStatusCodes();
                }
                catch { }
                finally { Monitor.Exit(this.SerialLock); }
            }
            //}
            return retVal;
        }

        /// <summary>
        /// Moves the RoboCylinder to a Pre-Programmed Position
        /// </summary>
        /// <param name="Position">Minimum = 0, Maximum = 15</param>
        /// <returns>True if successful</returns>
        public Boolean MovePosition(Byte Position)
        {
            lock (SerialLock)
            {
                if (SendCommand(String.Format("Q3010{0:X1}00000", Position)))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Moves the RoboCylinder to the Home Position
        /// </summary>
        /// <returns>True if successful</returns>
        public Boolean MoveHome()
        {
            lock (SerialLock)
            {
                if (SendCommand(String.Format("o0{0}00000000", (_homePositionIsMotorEnd ? 7 : 8))))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Moves the RoboCylinder to an absolute position
        /// </summary>
        /// <param name="AbsolutePosition">mm</param>
        /// <returns>True if successful</returns>
        public Boolean MoveAbsolute(Single AbsolutePosition)
        {
            lock (SerialLock)
            {
                if (AbsolutePosition >= 0 || AbsolutePosition <= _length)
                    if (SendCommand(String.Format("a{0}00", PositionToEncoderPulses(AbsolutePosition))))
                        if (ReadResponse())
                            return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Moves the RoboCylinder incrementally from the current position
        /// </summary>
        /// <param name="IncrementalPosition">+/- mm to move from current position</param>
        /// <returns>True if successful</returns>
        public Boolean MoveIncremental(Single IncrementalPosition)
        {
            lock (SerialLock)
            {
                if (SendCommand(String.Format("m{0}00", PositionToEncoderPulses(IncrementalPosition))))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Sets the Velocity and Acceleration for the RoboCylinder
        /// </summary>
        /// <param name="Velocity">mm/s</param>
        /// <param name="Acceleration">G</param>
        /// <returns>True if successful</returns>
        public Boolean VelocityAcceleration(Single Velocity, Single Acceleration)
        {
            lock (SerialLock)
            {
                if (SendCommand(String.Format("v2{0:X4}{1:X4}0", Convert.ToInt32(Velocity * 300 / _lead), Convert.ToInt32(Acceleration * 5883.99 / _lead))))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Turns the Servo On
        /// </summary>
        /// <returns>True if successful</returns>
        public Boolean ServoOn()
        {
            lock (SerialLock)
            {
                if (SendCommand("q1000000000"))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Turns the Servo Off
        /// </summary>
        /// <returns>True if successful</returns>
        public Boolean ServoOff()
        {
            lock (SerialLock)
            {
                if (SendCommand("q0000000000"))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Stops the RoboCylinder motion immediately
        /// </summary>
        /// <returns>True if successful</returns>
        public Boolean StopMotion()
        {
            lock (SerialLock)
            {
                if (SendCommand("d0000000000"))
                    if (ReadResponse())
                        return ParseStatusCodes();
            }
            return false;
        }

        /// <summary>
        /// Sets up the PreProgrammed Position Data
        /// </summary>
        /// <param name="PositionNumber">Position Number (Minimum = 0, Maximum = 15)</param>
        /// <param name="Position">Destination position in mm</param>
        /// <param name="Velocity">Velocity in mm/s</param>
        /// <param name="Acceleration">Acceleration in G</param>
        /// <param name="PushPercentage">Push Force %</param>
        /// <param name="PushTime">0 to 255 msec</param>
        /// <param name="PositionBand">Required position accuracy in mm</param>
        /// <param name="MaxAccFlag">Set to true for max acceleration</param>
        /// <returns>True if successful</returns>
        public Boolean SetUpPositionData(Byte PositionNumber, Single Position, Single Velocity, Single Acceleration, Byte PushPercentage, Byte PushTime, Single PositionBand, Boolean MaxAccFlag)
        {
            bool retVal = false;
            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                try
                {
                    if (TransferAtoB(PositionNumber))

                        // Position
                        if (AddressAllocation(AddressLocation.PositionData))
                            if (DataWrite(PositionToEncoderPulses(Position)))

                                // Velocity
                                if (AddressAllocation(AddressLocation.Velocity))
                                    if (DataWrite(String.Format("{0:X8}", Convert.ToInt32(Velocity * 300 / _lead))))

                                        // Acceleration / Deceleration
                                        if (AddressAllocation(AddressLocation.AccelerationDeceleration))
                                            if (DataWrite(String.Format("{0:X8}", Convert.ToInt32(Acceleration * 5883.99 / _lead))))

                                                // Push Force
                                                if (AddressAllocation(AddressLocation.PushForce))
                                                    if (DataWrite(String.Format("{0:X8}", Convert.ToInt32(PushPercentage * _lead))))

                                                        // Push Time
                                                        if (AddressAllocation(AddressLocation.PushTime))
                                                            if (DataWrite(String.Format("{0:X8}", PushTime)))

                                                                // Position Band
                                                                if (AddressAllocation(AddressLocation.PositionBand))
                                                                    if (DataWrite(PositionToEncoderPulses(PositionBand)))

                                                                        // Max Acceleration Flag
                                                                        if (AddressAllocation(AddressLocation.MaxAccFlag))
                                                                            if (DataWrite(String.Format("{0:X8}", MaxAccFlag ? (PushPercentage == 0 ? 1 : 7) : (PushPercentage == 0 ? 0 : 6))))
                                                                                if (TransferBtoA(PositionNumber))
                                                                                    retVal = true;
                }
                catch
                {
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Thread for reading the <see cref="Value">Value</see> (position) of the RoboCylinder
        /// </summary>
        public override void Process()
        {
            Single tmp;

            if (Monitor.TryEnter(this.SerialLock, 500))
            {
                try
                {
                    _positionReady = false;
                    tmp = Single.NaN;
                    if (SendCommand("R4000074000"))
                        if (ReadResponse())
                            tmp = EncoderPulsesToPosition(_response.Substring(5, 8));

                    if (Single.IsNaN(tmp) == false)
                        _position = tmp;

                    _positionReady = true;
                }
                catch
                {
                    commError = true;
                }
                finally
                {
                    Monitor.Exit(this.SerialLock);
                }

                if (_positionReady)
                {
                    CheckStatus();

                    if (!this.backgroundWorker1.IsBusy)
                        this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
                }
            }
        }

        /// <summary>
        /// Method for processing events inside the <see cref="Process">Process</see> thread.
        /// This method runs outside of the <see cref="Process">Process</see> thread.
        /// </summary>
        public override void BackgroundProcess()
        {
            if (ValueChanged != null) ValueChanged(this, null);
        }

        #endregion Public Members

        #region Public Properties

        /// <summary>
        /// Position in mm
        /// </summary>
        public Single Position
        {
            get
            {
                lock (this.SerialLock)
                {
                    if (_positionReady) return _position;
                    else
                    {
                        if (SendCommand("R4000074000"))
                            if (ReadResponse())
                                return EncoderPulsesToPosition(_response.Substring(5, 8));
                    }
                    return _position; // return last known position // Single.NaN;
                }
            }
        }

        /// <summary>
        /// Position in mm
        /// </summary>
        public override double Value
        {
            get { return this.Position; }
            internal set
            {
                _position = (Single)value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Formatted value including the <see cref="Units">Units</see>
        /// </summary>
        public override string FormattedValue
        {
            get { return this.Position.ToString(_format) + " " + this.Units; }
        }

        /// <summary>
        /// Name for the RoboCylinder
        /// </summary>
        public override string Name
        {
            get { return "IAI RoboCylinder Controller on port " + this.PortName; }
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public override double RawValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Minimum value for the RoboCylinder
        /// </summary>
        /// <remarks>Always returns zero</remarks>
        public override double Min
        {
            get { return 0; }
        }

        /// <summary>
        /// Maximum value (length) for the RoboCylinder
        /// </summary>
        public override double Max
        {
            get { return _length; }
        }

        /// <summary>
        /// Units for the RoboCylinder
        /// </summary>
        /// <remarks>Always returns "mm"</remarks>
        public override string Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        /// <summary>
        /// Format string for the RoboCylinder
        /// </summary>
        public override string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Status information for the RoboCylinder
        /// </summary>
        public StatusClass Status
        {
            get { return _status; }
        }

        /// <summary>
        /// Axis number for the RoboCylinder
        /// </summary>
        public Byte Axis
        {
            get { return _axis; }
            set { _axis = value; }
        }

        /// <summary>
        /// Indicates if the Home Position is at the Motor End of the RoboCylinder
        /// </summary>
        public Boolean HomePositionIsMotorEnd
        {
            get { return _homePositionIsMotorEnd; }
            set { _homePositionIsMotorEnd = value; }
        }

        /// <summary>
        /// Lead length in mm for the RoboCylinder
        /// </summary>
        public Single Lead
        {
            get { return _lead; }
            set { _lead = value; }
        }

        /// <summary>
        /// Length in mm of the RoboCylinder
        /// </summary>
        public Single Length
        {
            get { return _length; }
            set { _length = value; }
        }

        #endregion Public Properties
    }
}