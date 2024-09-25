using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.IO;
using System.Runtime.InteropServices;
using VTIWindowsControlLibrary;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Enums;
using AdvancedHMIDrivers; //For Allen-Bradley CompctLogix PLC
using System.Linq;

namespace VTIPLCInterface
{
    // State object for receiving data from remote device.
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }


    /// <summary>
    /// MccConfig
    /// 
    /// Implements the IIoConfig interface.
    /// Deserializes the VtiMCCInterface.config file to configure
    /// the Measurement Computing boards
    /// 
    /// The ProcessIO method runs as a seperate thread to process the
    /// analog and digital inputs.
    /// </summary>
    public class MccConfig : IIoConfig
    {
        #region Globals

        private static MccConfig defaultInstance;   // static "default instance" of the MccConfig class
        private List<AnalogBoard> _AnalogBoards;    // List of Analog Boards
        private List<DigitalBoard> _DigitalBoards;  // List of Digital Boards
                                                    //private ThreadStart thrdStart;              // ThreadStart object for processing I/O
        private Thread thrd;                        // Thread object for processing I/O
        private static Dictionary<string, IAnalogInput> _AnalogInputs = new Dictionary<string, IAnalogInput>();
        private static Dictionary<string, IAnalogOutput> _AnalogOutputs = new Dictionary<string, IAnalogOutput>();
        private static Dictionary<string, IDigitalInput> _DigitalInputs = new Dictionary<string, IDigitalInput>();
        private static Dictionary<string, IDigitalOutput> _DigitalOutputs = new Dictionary<string, IDigitalOutput>();
        private System.Timers.Timer timerDigitalInputs; // Timer to cause digital inputs to be read (they aren't read as often as the analog inputs)
        private Boolean bReadDigitalInputs;             // Boolean to trigger reading the digital inputs
        internal static Object MccLock = new object();  // Object to be used for locking when doing I/O functions
        private Boolean stopProcessing;
        private EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private int waitDelay = 50;
        private bool MessageBoxShown = false;

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent connectDone1 =
        new ManualResetEvent(false);
        private static ManualResetEvent connectDone2 =
        new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone1 =
        new ManualResetEvent(false);
        private static ManualResetEvent sendDone2 =
        new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone1 =
        new ManualResetEvent(false);
        private static ManualResetEvent receiveDone2 =
       new ManualResetEvent(false);

        // The response from the remote device.
        private static String response = String.Empty;

        private static Socket _PLC;
        private static Socket _PLC1;
        private static Socket _PLC2;
        //For Allen-Bradley CompactLogix PLC
        private static EthernetIPforCLXComm _ABCompactLogixPLC;

        private static bool DoNotSendAnaIn;
        private static bool DoNotSendDIn;
        private static bool DoNotSendDOut;
        private static bool _readPLCDigOutputsIntoPC = true;

        private static float AnaInValue;
        private static float StatusValue;
        private static float StatusValue1;
        private static float StatusValue2;
        private static byte DInValue;
        private static byte DOutValue;
        private static int bytesSentValue;
        private static int bytesReadValue;

        private static string strIPAddress = "192.168.1.10";
        private static string strIPAddress1 = "192.168.0.11";
        private static string strIPAddress2 = "192.168.0.12";

        private static DateTime DisconnectStartTime;
        private static DateTime DisconnectStartTime1;
        private static DateTime DisconnectStartTime2;

        #region For Allen-Bradley CompactLogix PLC
        private static bool[] Mod2 = new bool[4];
        private static bool[] Mod3 = new bool[4];
        private static bool[] Mod5 = new bool[4];
        private static bool[] Mod6 = new bool[4];
        private static bool[] Mod7 = new bool[4];

        private static int _Counter1 = 0;
        private static int _Counter2 = 0;

        private static int _CounterLimit1 = 20000;
        private static int _CounterLimit2 = 20000;

        private static int _CounterStart1 = 500;
        private static int _CounterStart2 = 500;

        private static float _CounterOutput = 0;

        private static bool _ResetCounter1;
        private static bool _ResetCounter2;
        #endregion

        #endregion

        #region Private Members

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                if (client.RemoteEndPoint.ToString().Contains("10") || client.RemoteEndPoint.ToString().Contains("20"))
                {
                    connectDone.Set();
                }
                else if (client.RemoteEndPoint.ToString().Contains("11") || client.RemoteEndPoint.ToString().Contains("21"))
                {
                    connectDone1.Set();
                }
                else if (client.RemoteEndPoint.ToString().Contains("12") || client.RemoteEndPoint.ToString().Contains("22"))
                {
                    connectDone2.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Send(Socket client, byte[] byteData)
        {
            // Convert the string data to byte data using ASCII encoding.
            //byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            try
            {
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                bytesSentValue = bytesSent;
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);
                bytesReadValue = bytesRead;

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    bytesReadValue = bytesReadValue + bytesRead;

                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        //response = state.sb.ToString();
                        try
                        {
                            if (DoNotSendDIn)
                            {
                                DInValue = state.buffer[9];
                            }
                            if (DoNotSendAnaIn)
                            {
                                //AnaInValue = BitConverter.ToSingle(new byte[]{state.buffer[11],state.buffer[12],state.buffer[10],state.buffer[9]},0);
                                AnaInValue = BitConverter.ToSingle(new byte[] { state.buffer[9], state.buffer[10], state.buffer[12], state.buffer[11] }, 0);
                            }
                        }
                        catch
                        {

                        }
                        DoNotSendDIn = false;
                        DoNotSendDOut = false;
                        DoNotSendAnaIn = false;
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();


                    //// Get the rest of the data.
                    //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //    new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        //response = state.sb.ToString();
                        try
                        {
                            if (DoNotSendDIn)
                            {
                                DInValue = state.buffer[9];
                            }
                            if (DoNotSendAnaIn)
                            {
                                AnaInValue = BitConverter.ToSingle(new byte[] { state.buffer[11], state.buffer[12], state.buffer[10], state.buffer[9] }, 0);
                            }
                        }
                        catch
                        {

                        }
                        DoNotSendDIn = false;
                        DoNotSendDOut = false;
                        DoNotSendAnaIn = false;
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void ReadAndParseSocket(Socket client)
        {
            try
            {
                bytesReadValue = 0;
                Receive(client);
                //receiveDone.WaitOne();

                DateTime LoopStartTime = DateTime.Now;
                while (bytesReadValue == 0)
                {
                    TimeSpan LoopTime = DateTime.Now - LoopStartTime;
                    if (LoopTime.TotalSeconds > 1.0)
                    {
                        break;
                    }
                }
                if (bytesReadValue == 0)
                {
                    //timeout error
                }

            }
            catch
            {

            }

        }

        #region For Allen-Bradley CompactLogix PLC
        int[] iainHandle = new int[16];
        int[] idinHandle = new int[16];
        int[] idoutHandle = new int[16];
        private static bool FeelsLikeTheFirstTime = true;
        private static bool[] oldBoolValueA = new bool[16];
        private static bool[] oldBoolValueB = new bool[16];
        #endregion
        /// <summary>
        /// ProcessIO
        /// 
        /// This thread processes all of the I/O boards in sequence.
        /// For analog boards, it reads the specified NumberOfSamples, 
        /// averages the result and stores it in the value property
        /// of the analog channel.
        /// For digital inputs, it reads the values of the digital inputs
        /// and stores them in the value property of the DigitalPortBit.
        /// </summary>
        private void ProcessIO()
        {
            if (VtiPLCInterface.Properties.Settings.Default.PLCEnabled)
            {
                ushort dataValue;
                int i;
                long dataSum;
                //MccDaq.ErrorInfo errorInfo = new MccDaq.ErrorInfo();

                while (!stopProcessing)
                {
                    //Debug.WriteLine(string.Format("Start milliseconds: {0:0}", DateTime.Now.Millisecond));
                    SaveOrClearABCompactLogixPLCVariables();
                    if (defaultInstance.AnalogBoards != null)
                    {
                        foreach (AnalogBoard board in defaultInstance.AnalogBoards)
                        {
                            //Analog Ouputs
                            //Writing Analog Outputs not implemented yet with Allen-Bradley CompactLogix (board.BoardNum = 30-39) PLC.
                            if (board.AnalogOutputs != null)
                            {
                                foreach (AnalogChannel channel in board.AnalogOutputs)
                                {
                                    if (board.BoardNum >= 10 && board.BoardNum < 30)
                                    {
                                        DoNotSendDOut = true;
                                        byte[] byteData = GetAnalogByteDataToSend(board.BoardNum, channel, false, true);
                                        if (SendByteData(byteData, board.BoardNum))
                                        {
                                            ReceiveByteData(new byte[12], board.BoardNum);
                                        }
                                        else
                                        {
                                            //send timeout error
                                            Console.WriteLine("AnaOutSendTimeOutErr");
                                        }
                                    }
                                    else if ((board.BoardNum >= 30) && (board.BoardNum < 40))
                                    {
                                        try
                                        {
                                            if (!MyIOStaticVariables.DoNotWriteAnalogOutputs)
                                            {
                                                string ThisAnalogOutputName = channel.Name;
                                                int AnalogOutDesiredState = Convert.ToInt32(channel.Value);//int worked float did not
                                                                                                           //do not continously write the analog output
                                                if (channel.Value > -100000)
                                                {
                                                    _ABCompactLogixPLC.WriteData(ThisAnalogOutputName, AnalogOutDesiredState);
                                                    channel.Value = -200000;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                                if ((board.BoardNum >= 30) && (board.BoardNum < 40))
                                {
                                    MyIOStaticVariables.DoNotWriteAnalogOutputs = true;
                                }
                            }
                            //Analog Inputs
                            if (board.AnalogInputs != null)
                            {
                                foreach (AnalogChannel channel in board.AnalogInputs)
                                {
                                    if (board.BoardNum >= 10 && board.BoardNum < 30)
                                    {
                                        if (channel.Channel != 0)
                                        {
                                            DoNotSendAnaIn = true;
                                            byte[] byteData = GetAnalogByteDataToSend(board.BoardNum, channel, true, false);
                                            bytesSentValue = 0;
                                            try
                                            {
                                                if (SendByteData(byteData, board.BoardNum))
                                                {
                                                    try
                                                    {
                                                        byte[] byteReadData = new byte[13];
                                                        if (board.BoardNum >= 20)
                                                        {
                                                            byteReadData = new byte[11];
                                                        }
                                                        if (ReceiveByteData(byteReadData, board.BoardNum))
                                                        {
                                                            if (board.BoardNum < 20)
                                                            {
                                                                AnaInValue = BitConverter.ToSingle(new byte[] { byteReadData[10], byteReadData[9], byteReadData[12], byteReadData[11] }, 0);
                                                            }
                                                            else
                                                            {
                                                                AnaInValue = ((float)(byteReadData[10]) + 256.0f * (float)(byteReadData[9]));
                                                            }
                                                            if (board.BoardNum == 10 || board.BoardNum == 20)
                                                            {
                                                                StatusValue = 1.0F;
                                                            }
                                                            else if (board.BoardNum == 11 || board.BoardNum == 21)
                                                            {
                                                                StatusValue1 = 1.0F;
                                                            }
                                                            else if (board.BoardNum == 12 || board.BoardNum == 22)
                                                            {
                                                                StatusValue2 = 1.0F;
                                                            }
                                                            //Console.WriteLine("AnalogIn Channel " + channel.Channel + "val: " + AnaInValue.ToString());
                                                        }
                                                        else
                                                        {
                                                            //timeout error read
                                                            Console.WriteLine("AnaInReadTimeOutErr");
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                        ReconnectPLC(board.BoardNum);
                                                    }
                                                }
                                                else
                                                {
                                                    //timeout error send
                                                    Console.WriteLine("AnaInSendTimeOutErr");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                                ReconnectPLC(board.BoardNum);
                                            }
                                        }
                                        if (channel.Channel != 0)
                                        {
                                            if (board.BoardNum < 20)
                                            {
                                                channel.Value = AnaInValue * 6553.5F;
                                            }
                                            else
                                            {
                                                channel.Value = AnaInValue;
                                            }
                                        }
                                        else
                                        {
                                            if (board.BoardNum == 10)
                                            {
                                                channel.Value = StatusValue * 6553.5F;
                                            }
                                            else if (board.BoardNum == 20)
                                            {
                                                channel.Value = StatusValue;
                                            }
                                            else if (board.BoardNum == 11)
                                            {
                                                channel.Value = StatusValue1 * 6553.5F;
                                            }
                                            else if (board.BoardNum == 21)
                                            {
                                                channel.Value = StatusValue1;
                                            }
                                            else if (board.BoardNum == 12)
                                            {
                                                channel.Value = StatusValue2 * 6553.5F;
                                            }
                                            else if (board.BoardNum == 22)
                                            {
                                                channel.Value = StatusValue2;
                                            }
                                        }
                                    }
                                    else if (board.BoardNum >= 30 && board.BoardNum < 40)
                                    {
                                        try
                                        {
                                            //Don't read the IP Address channel
                                            if (channel.Channel != 0)
                                            {
                                                string TempString = channel.Name;
                                                string TempResult = _ABCompactLogixPLC.ReadAny(TempString);
                                                channel.Value = (float)(((Convert.ToDouble(TempResult)) * (65535.0 / 20.0)) + 32786.0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (FeelsLikeTheFirstTime)
                                            {
                                                //MessageBox.Show("Allen Bradley PLC Communication Error: " + ex.ToString());
                                            }
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        #region old
                                        //dataSum = 0;
                                        //for (i = 0; i < channel.NumberOfSamples; i++)
                                        //{
                                        //    lock (MccLock)
                                        //    {
                                        //        errorInfo = board.MccBoard.AIn(channel.Channel, channel.Range, out dataValue);
                                        //    }
                                        //    dataSum += dataValue;
                                        //}
                                        //if (errorInfo.Value != 0)
                                        //{
                                        //    //throw new Exception(errorInfo.Message);
                                        //    if (channel.errorTimer == null) channel.errorTimer = new System.Diagnostics.Stopwatch();
                                        //    if (channel.errorTimer.IsRunning == false || channel.errorTimer.Elapsed.Minutes >= 5)
                                        //    {
                                        //        VtiEvent.Log.WriteError("Error reading analog channel " + channel.Name, VtiEventCatType.Analog_IO, errorInfo.Message);
                                        //        if (channel.errorTimer.IsRunning) channel.errorTimer.Reset();
                                        //        channel.errorTimer.Start();
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (channel.NumberOfMovingAverages == 0)
                                        //        channel.Value = (int)(dataSum / channel.NumberOfSamples);
                                        //    else
                                        //    {
                                        //        channel.MovingAverageSum -= channel.MovingAverages[channel.MovingAverageNum];
                                        //        channel.MovingAverages[channel.MovingAverageNum] = Convert.ToInt32(dataSum / channel.NumberOfSamples);
                                        //        channel.MovingAverageSum += channel.MovingAverages[channel.MovingAverageNum];
                                        //        channel.MovingAverageNum++;
                                        //        if (channel.MovingAverageNum >= channel._NumberOfMovingAverages) channel.MovingAverageNum = 0;
                                        //        channel.Value = ((Single)channel.MovingAverageSum / (Single)channel._NumberOfMovingAverages);
                                        //    }
                                        //}
                                        #endregion
                                    }
                                    Thread.Sleep(0);  // Yield to other threads
                                }
                            }
                        }
                    }

                    if (bReadDigitalInputs)
                    {
                        if (defaultInstance.DigitalBoards != null)
                        {
                            foreach (DigitalBoard board in defaultInstance.DigitalBoards)
                            {
                                if (board.DigitalPorts != null)
                                {
                                    foreach (DigitalPort port in board.DigitalPorts)
                                    {
                                        if (board.BoardNum >= 10 && board.BoardNum < 30)
                                        {
                                            if (port.DigitalPortDirection == MccDaq.DigitalPortDirection.DigitalIn)
                                            {
                                                DoNotSendDIn = true;
                                                byte[] byteData = GetDigitalByteDataToSend(board.BoardNum, port, true, false);
                                                if (SendByteData(byteData, board.BoardNum))
                                                {
                                                    try
                                                    {
                                                        byte[] byteReadData = new byte[10];
                                                        if (board.BoardNum >= 20)
                                                        {
                                                            //byteReadData = new byte[11]; //not needed? //temp //need to test
                                                        }
                                                        if (ReceiveByteData(byteReadData, board.BoardNum))
                                                        {
                                                            //no error
                                                            DInValue = byteReadData[9];
                                                        }
                                                        else
                                                        {
                                                            //timeout error read
                                                            Console.WriteLine("DigInReadTimeOutErr");
                                                            ReconnectPLC(board.BoardNum);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ReconnectPLC(board.BoardNum);
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                                else
                                                {
                                                    //send timeout error
                                                    Console.WriteLine("DigInSendTimeOutErr");
                                                    ReconnectPLC(board.BoardNum);
                                                }

                                                foreach (DigitalPortBit bit in port.DigitalPortBits)
                                                    if (board.InvertedLogic)
                                                        bit.Value = ((DInValue & (1 << bit.Bit)) == 0);
                                                    else
                                                        bit.Value = ((DInValue & (1 << bit.Bit)) != 0);
                                            }
                                            else//digital output write
                                            {
                                                DoNotSendDOut = true;
                                                byte[] byteData;
                                                //read in the current state of the digital outputs and 
                                                //then assign the values to those bits when executing ProcessIO() the first time.
                                                if (_readPLCDigOutputsIntoPC)
                                                {
                                                    byteData = GetDigitalByteDataToSend(board.BoardNum, port, true, false);
                                                    if (SendByteData(byteData, board.BoardNum))
                                                    {
                                                        byte[] byteReadData = new byte[10];
                                                        if (board.BoardNum >= 20)
                                                        {
                                                            byteReadData = new byte[11]; //not needed? //temp //need to test
                                                        }
                                                        if (ReceiveByteData(byteReadData, board.BoardNum))
                                                        {
                                                            //no error
                                                            DOutValue = byteReadData[9];
                                                        }
                                                        else
                                                        {
                                                            //read timeout error
                                                            Console.WriteLine("DigOutReadTimeOutErr");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //send timeout error
                                                        Console.WriteLine("DigOutSendTimeOutErr");
                                                    }
                                                    foreach (DigitalPortBit bit in port.DigitalPortBits)
                                                        if (board.InvertedLogic)
                                                            bit.Value = ((DOutValue & (1 << bit.Bit)) == 0);
                                                        else
                                                            bit.Value = ((DOutValue & (1 << bit.Bit)) != 0);
                                                }
                                                else
                                                {
                                                    byteData = GetDigitalByteDataToSend(board.BoardNum, port, false, true);
                                                    if (SendByteData(byteData, board.BoardNum))
                                                    {
                                                        try
                                                        {
                                                            byte[] byteReadData = new byte[12];
                                                            if (ReceiveByteData(byteReadData, board.BoardNum))
                                                            {
                                                                //no error
                                                            }
                                                            else
                                                            {
                                                                //timeout error read
                                                                Console.WriteLine("DigOutReadTimeOutErr");
                                                            }
                                                        }
                                                        catch (Exception ee)
                                                        {
                                                            Console.WriteLine("Dig Out Read error:" + ee.Message);
                                                            ReconnectPLC(board.BoardNum);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //send timeout error
                                                        Console.WriteLine("DigOutSendTimeOutErr");
                                                    }
                                                }
                                            }
                                        }
                                        else if (board.BoardNum >= 30 && board.BoardNum < 40)
                                        {
                                            //For Allen-Bradley CompactLogix PLC
                                            if (port.DigitalPortDirection == MccDaq.DigitalPortDirection.DigitalIn)
                                            {
                                                foreach (DigitalPortBit bit in port.DigitalPortBits)
                                                {
                                                    try
                                                    {
                                                        string TempString = bit.Name;
                                                        string TempResult = _ABCompactLogixPLC.ReadAny(TempString);
                                                        bit.Value = (bool)(Convert.ToBoolean(TempResult));
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }
                                            else//write digital outputs
                                            {
                                                if (!MyIOStaticVariables.ReadDigIOOnly)
                                                {
                                                    foreach (DigitalPortBit bit in port.DigitalPortBits)
                                                    {
                                                        try
                                                        {
                                                            string ThisDigitalOutputName = bit.Name;
                                                            bool OnOffDesiredState = bit.Value;
                                                            string OnOffResultFromPLC = _ABCompactLogixPLC.ReadAny(ThisDigitalOutputName);
                                                            bool OnOffCurrentState = false;
                                                            int ParsedOnOffIntegerFormatFlag;
                                                            if (int.TryParse(OnOffResultFromPLC, out ParsedOnOffIntegerFormatFlag))
                                                            {
                                                                OnOffCurrentState = ParsedOnOffIntegerFormatFlag == 1;
                                                            }
                                                            else
                                                            {
                                                                OnOffCurrentState = OnOffResultFromPLC.ToLower() == "true";
                                                            }
                                                            if (!MyIOStaticVariables.DoNotWriteDigitalOutputs)
                                                            {
                                                                if (OnOffCurrentState != OnOffDesiredState)
                                                                {
                                                                    _ABCompactLogixPLC.WriteData(ThisDigitalOutputName, OnOffDesiredState ? 1 : 0);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                bit.Value = OnOffCurrentState;
                                                            }

                                                            /*
                                                             * this commented-out code below worked with CompactLogix 5380 but not with CompactLogix 5370
                                                             * Added the code above to work with both
                                                             * */
                                                            /*string TempString = bit.Name;

                                                            string TempResult = _ABCompactLogixPLC.ReadAny(TempString);
                                                            int TempInt = 0;
                                                            try
                                                            {
                                                                TempInt = Convert.ToInt32(TempResult);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                if (TempResult != "False")
                                                                {
                                                                    TempInt = 1;
                                                                }
                                                            }

                                                            if (((TempInt == 1) && (!bit.Value)) || ((TempInt != 1) && (bit.Value)))
                                                            {
                                                                if (bit.Value)
                                                                {
                                                                    _ABCompactLogixPLC.WriteData(TempString, 1);

                                                                }
                                                                else
                                                                {
                                                                    _ABCompactLogixPLC.WriteData(TempString, 0);
                                                                }
                                                            }*/
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //restored digital outputs on all boards
                            _readPLCDigOutputsIntoPC = false;
                        }
                        bReadDigitalInputs = false;
                    }
                    FeelsLikeTheFirstTime = false;
                    //Debug.WriteLine(string.Format("End milliseconds: {0:0}", DateTime.Now.Millisecond));
                    //Debug.Flush();
                    Thread.Sleep(waitDelay);
                    //exitEvent.WaitOne(waitDelay, true);
                }
            }
            else if (!MessageBoxShown)
            {
                MessageBox.Show("WARNING: PLCEnabled setting in VtiPLCInterface is set to false. PLC communication is disabled. Set to true before shipping.");
                MessageBoxShown = true;
            }
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Start Method
        /// 
        /// Implements Start method of IIoConfig interface.
        /// Used to start the I/O thread
        /// </summary>
        public void Start()
        {
            if (timerDigitalInputs == null)
            {
                timerDigitalInputs = new System.Timers.Timer();
                timerDigitalInputs.Elapsed += new System.Timers.ElapsedEventHandler(timerDigitalInputs_Elapsed);
            }
            timerDigitalInputs.Enabled = true;
            thrd = new Thread(new ThreadStart(ProcessIO));
            thrd.Start();
            thrd.IsBackground = true;
            thrd.Name = "MccConfig IO Process";

            //foreach (AnalogBoard analogBoard in defaultInstance.AnalogBoards)
            //    analogBoard.StartBackground();
        }

        void timerDigitalInputs_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.bReadDigitalInputs = true;
        }

        /// <summary>
        /// Stop Method
        /// 
        /// Implements Stop method of IIoConfig interface.
        /// Used to stop the I/O thread
        /// </summary>
        public void Stop()
        {
            timerDigitalInputs.Enabled = false;
            stopProcessing = true;
            if (exitEvent != null)
                exitEvent.Set();
            //if (thrd != null)
            //    thrd.Abort();
            //foreach (AnalogBoard analogBoard in defaultInstance.AnalogBoards)
            //    analogBoard.StopBackground();
            //analogBoard.MccBoard.StopBackground(MccDaq.FunctionType.AiFunction);
        }

        /// <summary>
        /// Turns off all of the <see cref="IDigitalOutput">DigitalOutputs</see> from the I/O Interface
        /// </summary>
        public void TurnAllOff()
        {
            if (VtiPLCInterface.Properties.Settings.Default.PLCEnabled && defaultInstance.DigitalBoards != null)
            {
                foreach (DigitalBoard board in defaultInstance.DigitalBoards)
                {
                    if (board.DigitalPorts != null)
                    {
                        foreach (DigitalPort port in board.DigitalPorts)
                        {
                            if (port.DigitalPortDirection == MccDaq.DigitalPortDirection.DigitalOut)
                            {
                                foreach (DigitalPortBit bit in port.DigitalPortBits)
                                    bit.Value = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads all of the <see cref="IDigitalOutput">DigitalOutput</see> bit values from the PLC and assigns those values to the Digital Outputs' values in the PC software.
        /// </summary>
        public bool ReadPLCDigOutputsIntoPC
        {
            get
            {
                return _readPLCDigOutputsIntoPC;
            }
            set
            {
                _readPLCDigOutputsIntoPC = value;
            }
        }

        /// <summary>
        /// Returns a byte array which contains the addresses of the digital bits on the respective port (Aux, FirstPortA, etc.) to read or write.
        /// </summary>
        /// <param name="boardNum"></param>
        /// <param name="port"></param>
        /// <param name="readData"></param>
        /// <param name="writeData"></param>
        /// <returns></returns>
        public byte[] GetDigitalByteDataToSend(int boardNum, DigitalPort port, bool readData, bool writeData)
        {
            byte[] byteData = new byte[1];
            byte[] byteData89 = GetByteDataForPortType(port.DigitalPortType, port.DigitalPortDirection);
            if (port.DigitalPortDirection == MccDaq.DigitalPortDirection.DigitalIn)
            {
                if (boardNum >= 10 && boardNum < 30)
                {
                    //read DigIn by TCP/IP
                    byteData = new byte[12];
                    byteData[0] = 0;//transaction number msb
                    byteData[1] = 1;//transaction number lsb
                    byteData[2] = 0;//0 for modbus tcp
                    byteData[3] = 0;//0 for modbus tcp
                    byteData[4] = 0;//number of remaining bytes in this frame msb
                    byteData[5] = 6;//number of remaining bytes in this frame lsb
                    byteData[6] = 255;//slave address, 255 if not used
                    if (boardNum < 20)
                    {
                        byteData[7] = 2;//function code 2 to read multiple coils
                    }
                    else
                    {
                        byteData[7] = 1;//function code 2 to read multiple coils//2 multiple coils//3 is read holding register//1 is read coil status
                    }
                    byteData[8] = byteData89[0];
                    byteData[9] = byteData89[1];
                    byteData[10] = 0x00;//number of data point msb
                    byteData[11] = 0x08;//number of data points lsb
                }
            }
            else if (port.DigitalPortDirection == MccDaq.DigitalPortDirection.DigitalOut)
            {
                if (boardNum >= 10 && boardNum < 30)
                {
                    if (writeData)
                    {
                        //write DigOut by TCP/IP
                        byteData = new byte[14];
                        byteData[0] = 0;//transaction number msb
                        byteData[1] = 1;//transaction number lsb
                        byteData[2] = 0;//0 for modbus tcp
                        byteData[3] = 0;//0 for modbus tcp
                        byteData[4] = 0;//number of remaining bytes in this frame msb
                        byteData[5] = 8;//number of remaining bytes in this frame lsb
                        byteData[6] = 255;//slave address, 255 if not used
                        byteData[7] = 15;//function code 15 to write multiple coils
                        byteData[8] = byteData89[0];
                        byteData[9] = byteData89[1];
                        byteData[10] = 0x00;//number of data point msb
                        byteData[11] = 0x08;//number of data points lsb
                        byteData[12] = 0x01;//number of bytes of coil values to follow
                        byteData[13] = (byte)port.Value;//data to write lsb
                    }
                    else if (readData)
                    {
                        byteData = new byte[12];
                        byteData[0] = 0;//transaction number msb
                        byteData[1] = 1;//transaction number lsb
                        byteData[2] = 0;//0 for modbus tcp
                        byteData[3] = 0;//0 for modbus tcp
                        byteData[4] = 0;//number of remaining bytes in this frame msb
                        byteData[5] = 6;//*number of remaining bytes in this frame lsb
                        byteData[6] = 255;//slave address, 255 if not used
                        if (boardNum < 20)
                        {
                            byteData[7] = 2;//function code 2 to read multiple coils
                        }
                        else
                        {
                            byteData[7] = 1;//function code 2 to read multiple coils//2 multiple coils//3 is read holding register//1 is read coil status
                        }
                        byteData[8] = byteData89[0];
                        byteData[9] = byteData89[1];
                        byteData[10] = 0x00;//number of data point msb
                        byteData[11] = 0x08;//number of data points lsb
                    }
                }
            }
            return byteData;
        }

        /// <summary>
        /// Returns a byte array which contains the addresses of the analog channels to read or write.
        /// </summary>
        /// <param name="boardNum"></param>
        /// <param name="channel"></param>
        /// <param name="readData"></param>
        /// <param name="writeData"></param>
        /// <returns></returns>
        public byte[] GetAnalogByteDataToSend(int boardNum, AnalogChannel channel, bool readData, bool writeData)
        {
            byte[] byteData = new byte[1];
            if (boardNum >= 10 && boardNum < 30)
            {
                if (writeData)
                {
                    //write AnalogOut by TCP/IP
                    byteData = new byte[15];
                    byteData[0] = 0;//transaction number msb
                    byteData[1] = 1;//transaction number lsb
                    byteData[2] = 0;//0 for modbus tcp
                    byteData[3] = 0;//0 for modbus tcp
                    byteData[4] = 0;//number of remaining bytes in this frame msb
                    byteData[5] = 9;//number of remaining bytes in this frame lsb
                    byteData[6] = 255;//slave address, 255 if not used
                    byteData[7] = 16;//function code 16 to write multiple coils

                    byteData[8] = 0x00;//start address msb modbus hex addressing
                    byteData[9] = (byte)(channel.Channel + 19);//start address lsb modbus hex addressing, Channel 1 is 0014h, DS21

                    byteData[10] = 0x00;//number of data point msb
                    byteData[11] = 0x01;//number of data points lsb
                    byteData[12] = 0x02;//number of bytes of values to follow

                    int TempValue = 0;
                    try
                    {
                        TempValue = Convert.ToInt16(channel.Value * 3276.8F);
                    }
                    catch
                    {
                    }
                    //TempValue = 10000;
                    byteData[13] = (byte)(TempValue / 256);//data to write msb
                    byteData[14] = (byte)(TempValue - (int)(TempValue / 256) * 256);//data to write lsb
                }
                else if (readData)
                {
                    //read analog inputs by TCP/IP
                    // Send test data to the remote device.
                    byteData = new byte[12];
                    byteData[0] = 0;//transaction number msb
                    byteData[1] = 1;//transaction number lsb
                    byteData[2] = 0;//0 for modbus tcp
                    byteData[3] = 0;//0 for modbus tcp
                    byteData[4] = 0;//number of remaining bytes in this frame msb
                    byteData[5] = 6;//number of remaining byts in this fram lsb
                    byteData[6] = 255;//slave address, 255 if not used
                    byteData[7] = 3;//function code 3 to read holding register
                    byteData[8] = 0x70;//start address msb modbus hex addressing
                    if (boardNum < 20)
                    {
                        //for CLICK PLC
                        byteData[9] = (byte)(2 * (byte)(channel.Channel - 1));//start address lsb modbus hex addressing
                    }
                    else
                    {
                        //for Allen-Bradley Micro850 PLC
                        byteData[9] = (byte)(channel.Channel - 1);//start address lsb modbus hex addressing
                    }
                    byteData[10] = 0x00;//number of data point msb
                    if (boardNum < 20)
                    {
                        //for CLICK PLC
                        byteData[11] = 0x02;//number of data points lsb
                    }
                    else
                    {
                        //for Allen-Bradley Micro850 PLC
                        byteData[11] = 0x01;//number of data points lsb
                    }
                }
            }
            return byteData;
        }

        public bool SendByteData(byte[] byteData, int boardNum)
        {
            bool success = false;
            int numBytesSent = 0;
            try
            {
                if (boardNum >= 10 && boardNum < 30)
                {
                    if (boardNum == 10 || boardNum == 20)
                    {
                        _PLC.SendTimeout = 1000;
                        numBytesSent = _PLC.Send(byteData, byteData.Length, SocketFlags.None);
                    }
                    else if (boardNum == 11 || boardNum == 21)
                    {
                        _PLC1.SendTimeout = 1000;
                        numBytesSent = _PLC1.Send(byteData, byteData.Length, SocketFlags.None);
                    }
                    else if (boardNum == 12 || boardNum == 22)
                    {
                        _PLC2.SendTimeout = 1000;
                        numBytesSent = _PLC2.Send(byteData, byteData.Length, SocketFlags.None);
                    }
                    if (numBytesSent == byteData.Length)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("Error sending data to PLC: " + ee.ToString());
            }
            return success;
        }

        public bool ReceiveByteData(byte[] byteReadData, int boardNum)
        {
            bool success = false;
            if (boardNum >= 10 && boardNum < 30)
            {
                try
                {
                    int numBytesReceived = 0;
                    if (boardNum == 10 || boardNum == 20)
                    {
                        _PLC.ReceiveTimeout = 1000;
                        numBytesReceived = _PLC.Receive(byteReadData, byteReadData.Length, SocketFlags.None);
                    }
                    else if (boardNum == 11 || boardNum == 21)
                    {
                        _PLC1.ReceiveTimeout = 1000;
                        numBytesReceived = _PLC1.Receive(byteReadData, byteReadData.Length, SocketFlags.None);
                    }
                    else if (boardNum == 12 || boardNum == 22)
                    {
                        _PLC2.ReceiveTimeout = 1000;
                        numBytesReceived = _PLC2.Receive(byteReadData, byteReadData.Length, SocketFlags.None);
                    }
                    if (boardNum < 20 && numBytesReceived == byteReadData.Length)
                    {
                        success = true;
                    }
                    else if (boardNum >= 20 && numBytesReceived == 10)
                    {
                        //byteReadData.Length is 11 but numBytesReceived is 10 on success only for Micro850 (#20-29)
                        success = true;
                    }
                    else
                    {
                        //timeout error read
                        Console.WriteLine("AnaReadTimeOutErr");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return success;
        }

        public void ReconnectPLC(int boardNum)
        {
            if (boardNum == 10 || boardNum == 20)
            {
                TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime;
                if (DisconnectTime.TotalSeconds > 10.0)
                {
                    DisconnectStartTime = DateTime.Now;
                    _PLC.Close();
                    try
                    {
                        _PLC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        IPAddress IPRemoteAddress = IPAddress.Parse(strIPAddress);
                        IPEndPoint ipRemote = new IPEndPoint(IPRemoteAddress, 502);
                        _PLC.BeginConnect(ipRemote, new AsyncCallback(ConnectCallback), _PLC);
                        connectDone.WaitOne(5000);
                        if (!_PLC.Connected)
                        {
                            Console.WriteLine("Connection Fail");
                            StatusValue = 10.0F;
                        }
                        else
                        {
                            Console.WriteLine("Connection OK");
                            StatusValue = 1.0F;
                        }
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.Message);
                        //_PLC.Disconnect(true);
                    }
                }
            }
            else if (boardNum == 11 || boardNum == 21)
            {
                TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime1;
                if (DisconnectTime.TotalSeconds > 10.0)
                {
                    DisconnectStartTime1 = DateTime.Now;
                    _PLC1.Close();
                    {
                        try
                        {
                            _PLC1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            IPAddress IPRemoteAddress = IPAddress.Parse(strIPAddress1);

                            IPEndPoint ipRemote = new IPEndPoint(IPRemoteAddress, 502);

                            _PLC1.BeginConnect(ipRemote, new AsyncCallback(ConnectCallback), _PLC1);

                            connectDone1.WaitOne(5000);
                            if (!_PLC1.Connected)
                            {
                                Console.WriteLine("Connection Fail");
                                StatusValue1 = 10.0F;
                            }
                            else
                            {
                                Console.WriteLine("Connection OK");
                                StatusValue1 = 1.0F;
                            }
                        }
                        catch (Exception ex1)
                        {
                            Console.WriteLine(ex1.Message);
                            //_PLC1.Disconnect(true);
                        }
                    }
                }
            }
            else if (boardNum == 12 || boardNum == 22)
            {
                TimeSpan DisconnectTime = DateTime.Now - DisconnectStartTime2;
                if (DisconnectTime.TotalSeconds > 10.0)
                {
                    DisconnectStartTime2 = DateTime.Now;
                    _PLC2.Close();
                    {
                        try
                        {
                            _PLC2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            IPAddress IPRemoteAddress = IPAddress.Parse(strIPAddress2);

                            IPEndPoint ipRemote = new IPEndPoint(IPRemoteAddress, 502);

                            _PLC2.BeginConnect(ipRemote, new AsyncCallback(ConnectCallback), _PLC2);

                            connectDone2.WaitOne(5000);
                            if (!_PLC2.Connected)
                            {
                                Console.WriteLine("Connection Fail");
                                StatusValue2 = 10.0F;
                            }
                            else
                            {
                                Console.WriteLine("Connection OK");
                                StatusValue2 = 1.0F;
                            }
                        }
                        catch (Exception ex1)
                        {
                            Console.WriteLine(ex1.Message);
                            //_PLC1.Disconnect(true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Save/Clear Allen-Bradley Compactlogix PLC variables. Copied from older version of Allen-Bradley CompactLogix specfic PLC library.
        /// </summary>
        public void SaveOrClearABCompactLogixPLCVariables()
        {
            //Writes string to Allen Bradley PLC tag
			if(MyIOStaticVariables.IntSendBlueString > 0) {
				_ABCompactLogixPLC.WriteData(MyIOStaticVariables.strBlueTagName, MyIOStaticVariables.strBlueStringToSend);
				MyIOStaticVariables.IntSendBlueString = 0;
			}
			if(MyIOStaticVariables.IntSendWhiteString > 0) {
				_ABCompactLogixPLC.WriteData(MyIOStaticVariables.strWhiteTagName, MyIOStaticVariables.strWhiteStringToSend);
				MyIOStaticVariables.IntSendWhiteString = 0;
			}

			if(MyIOStaticVariables.IntSaveIt > 0)
            {
                try
                {
                    string TempString;

                    ////UnitSerialNum
                    TempString = "strUnit_Serial_Num";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Unit_Serial_Num);

                    //UnitSerialLen
                    TempString = "IntUnit_Serial_Num_Len";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Unit_Serial_Num_Len);

                    //Test_Date_Time_Year
                    TempString = "IntTest_Date_Time_Year";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Test_Date_Time_Year);

                    //Test_Date_Time_Month
                    TempString = "IntTest_Date_Time_Month";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Test_Date_Time_Month);

                    //Test_Date_Time_Day
                    TempString = "IntTest_Date_Time_Day";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Test_Date_Time_Day);

                    //Test_Date_Time_Hour
                    TempString = "IntTest_Date_Time_Hour";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Test_Date_Time_Hour);

                    //Test_Date_Time_Minute
                    TempString = "IntTest_Date_Time_Minute";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Test_Date_Time_Minute);

                    //Test_Date_Time_Second
                    TempString = "IntTest_Date_Time_Second";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Test_Date_Time_Second);

                    //Passed_Test
                    TempString = "IntPassed_Test";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Passed_Test);

                    //Test_Number
                    TempString = "IntTest_Number";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Test_Number);

                    //Station_Number
                    TempString = "IntStation_Number";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Station_Number);

                    //HOLD_START_TIME
                    TempString = "IntHold_Start_Time";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.HOLD_START_TIME);

                    //TOTAL_TEST_TIME
                    TempString = "IntTotal_Test_Time";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.TOTAL_TEST_TIME);

                    //Micron_At_10sec_B4_Test_Cir1
                    TempString = "IntMicron_At_10sec_B4_Test_Cir1";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Micron_At_10sec_B4_Test_Cir1);

                    //Micron_At_10sec_B4_Test_Cir2
                    TempString = "IntMicron_At_10sec_B4_Test_Cir2";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Micron_At_10sec_B4_Test_Cir2);

                    //Micron_At_5min_Hold_Cir1
                    TempString = "IntMicron_At_5min_Hold_Cir1";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Micron_At_5min_Hold_Cir1);

                    //Micron_At_5min_Hold_Cir2
                    TempString = "IntMicron_At_5min_Hold_Cir2";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Micron_At_5min_Hold_Cir2);

                    //Micron_At_End_Test_Cir1
                    TempString = "IntMicron_At_End_Test_Cir1";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Micron_At_End_Test_Cir1);

                    //Micron_At_End_Test_Cir2
                    TempString = "IntMicron_At_End_Test_Cir2";
                    _ABCompactLogixPLC.WriteData(TempString, MyIOStaticVariables.Micron_At_End_Test_Cir2);

                    //Set Save It flag on the PLC
                    TempString = "IntSaveIt";
                    _ABCompactLogixPLC.WriteData(TempString, 1);

                    //Clear the PLC Save It flag
                    MyIOStaticVariables.IntSaveIt = 0;
                    MyIOStaticVariables.IntSavedOnce = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (MyIOStaticVariables.ClearIt > 0)
            {
                try
                {
                    string TempString;

                    //UnitSerialNum
                    TempString = "strUnit_Serial_Num";
                    string TempStringData = " ";
                    _ABCompactLogixPLC.WriteData(TempString, TempStringData);

                    //UnitSerialLen
                    TempString = "IntUnit_Serial_Num_Len";
                    _ABCompactLogixPLC.WriteData(TempString, 1);

                    //Test_Date_Time_Year
                    TempString = "IntTest_Date_Time_Year";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Test_Date_Time_Month
                    TempString = "IntTest_Date_Time_Month";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Test_Date_Time_Day
                    TempString = "IntTest_Date_Time_Day";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Test_Date_Time_Hour
                    TempString = "IntTest_Date_Time_Hour";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Test_Date_Time_Minute
                    TempString = "IntTest_Date_Time_Minute";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Test_Date_Time_Second
                    TempString = "IntTest_Date_Time_Second";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Passed_Test
                    TempString = "IntPassed_Test";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Test_Number
                    TempString = "IntTest_Number";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Station_Number
                    TempString = "IntStation_Number";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //HOLD_START_TIME
                    TempString = "IntHold_Start_Time";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //TOTAL_TEST_TIME
                    TempString = "IntTotal_Test_Time";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Micron_At_10sec_B4_Test_Cir1
                    TempString = "IntMicron_At_10sec_B4_Test_Cir1";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Micron_At_10sec_B4_Test_Cir2
                    TempString = "IntMicron_At_10sec_B4_Test_Cir2";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Micron_At_5min_Hold_Cir1
                    TempString = "IntMicron_At_5min_Hold_Cir1";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Micron_At_5min_Hold_Cir2
                    TempString = "IntMicron_At_5min_Hold_Cir2";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Micron_At_End_Test_Cir1
                    TempString = "IntMicron_At_End_Test_Cir1";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Micron_At_End_Test_Cir2
                    TempString = "IntMicron_At_End_Test_Cir2";
                    _ABCompactLogixPLC.WriteData(TempString, 0);

                    //Set Save It flag on the PLC
                    TempString = "IntSaveIt";
                    _ABCompactLogixPLC.WriteData(TempString, 1);

                    MyIOStaticVariables.ClearIt = 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public byte[] GetByteDataForPortType(MccDaq.DigitalPortType portType, MccDaq.DigitalPortDirection direction)
        {
            byte[] returnData = new byte[2];
            returnData[0] = (byte)(direction == MccDaq.DigitalPortDirection.DigitalOut ? 0x20 : 0x00);//start address msb modbus hex addressing
            if (portType == MccDaq.DigitalPortType.AuxPort)//x001
            {
                returnData[1] = 0x00;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.FirstPortA)//x101
            {
                returnData[1] = 0x20;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.FirstPortB)//x109
            {
                returnData[1] = 0x28;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.SecondPortA)//x201
            {
                returnData[1] = 0x40;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.SecondPortB)//x209
            {
                returnData[1] = 0x48;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.ThirdPortA)//x301
            {
                returnData[1] = 0x60;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.ThirdPortB)//x309
            {
                returnData[1] = 0x68;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.FourthPortA)//x401
            {
                returnData[1] = 0x80;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.FourthPortB)//x409
            {
                returnData[1] = 0x88;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.FifthPortA)//x501
            {
                returnData[1] = 0xA0;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.FifthPortB)//x509
            {
                returnData[1] = 0xA8;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.SixthPortA)//x601
            {
                returnData[1] = 0xC0;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.SixthPortB)//x609
            {
                returnData[1] = 0xC8;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.SeventhPortA)//x701
            {
                returnData[1] = 0xE0;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.SeventhPortB)//x709
            {
                returnData[1] = 0xE8;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.EighthPortA)//x801
            {
                returnData[0] = (byte)(direction == MccDaq.DigitalPortDirection.DigitalOut ? 0x21 : 0x01);//start address msb modbus hex addressing
                returnData[1] = 0x00;//start address lsb modbus hex addressing
            }
            else if (portType == MccDaq.DigitalPortType.EighthPortB)//x809
            {
                returnData[0] = (byte)(direction == MccDaq.DigitalPortDirection.DigitalOut ? 0x21 : 0x01);//start address msb modbus hex addressing
                returnData[1] = 0x08;//start address lsb modbus hex addressing
            }
            return returnData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Default instance property
        /// 
        /// If the default instance hasn't been created yet, this creates it
        /// by deserializing the VtiMCCInterface.config file.
        /// 
        /// Returns the default instance.
        /// </summary>
        public static MccConfig Default
        {
            get
            {
                AnalogInput _analogInput;
                AnalogOutput _analogOutput;
                DigitalInput _digitalInput;
                DigitalOutput _digitalOutput;

                //MccDaq.ErrorInfo errorInfo;
                if (defaultInstance == null)
                {
                    #region For Allen-Bradley CompactLogix PLC
                    for (int i = 0; i < 4; i++)
                    {
                        Mod2[i] = false;
                        Mod3[i] = false;
                        Mod5[i] = false;
                        Mod6[i] = false;
                        Mod7[i] = false;
                    }
                    #endregion
                    // Create the XmlSerializer
                    XmlSerializer x = new XmlSerializer(typeof(MccConfig));
                    // Create the StreamReader
                    StreamReader s;
                    String sConfig;
                    sConfig = "VtiPLCInterface.config";
                    FileInfo fileInfo = new FileInfo(sConfig);
                    if (!fileInfo.Exists)
                    {
                        sConfig = fileInfo.DirectoryName;
                        if (!sConfig.EndsWith(@"\")) sConfig += @"\";
                        sConfig += @"IOInterface\" + fileInfo.Name;
                        fileInfo = new FileInfo(sConfig);
                    }
                    if (fileInfo.Exists)
                    {
                        try
                        {
                            s = new StreamReader(fileInfo.FullName);
                            // Deserialize the VtiMCCInterface.config file into defaultInstance
                            defaultInstance = (MccConfig)x.Deserialize(s);
                        }
                        catch (Exception e)
                        {

                           VtiEvent.Log.WriteError("Error reading the configuration file 'VtiPLCInterface.config' for VTIPLCInterface.dll", VtiEventCatType.Application_Error, e.ToString());
                           MessageBox.Show("Error reading VtiPLCInterface.config from project folder. Make sure that:" +
                           Environment.NewLine + "1. The file is named VtiPLCInterface.config." +
                           Environment.NewLine + "2. There is a copy of the file in the bin\\Debug folder." +
                           Environment.NewLine + "3. The file is formatted correctly." +
                           Environment.NewLine + "4. There are no duplicate names in the file.");
                        }
                        if (VtiPLCInterface.Properties.Settings.Default.PLCEnabled)
                        {
                            #region VtiPLCInterface.config schema checking
                            if (defaultInstance.AnalogBoards.Count == 0 || defaultInstance.AnalogBoards[0].AnalogInputs.Count == 0 || defaultInstance.AnalogBoards[0].AnalogInputs[0].Name == null || !defaultInstance.AnalogBoards[0].AnalogInputs[0].Name.Contains("IPADDRESS:"))
                            {
                                MessageBox.Show("PLC IP Address not set in VtiPLCInterface.config.");
                            }
                            else if (defaultInstance.AnalogBoards[0].BoardNum == 0)
                            {
                                MessageBox.Show("<BoardNum> tag is missing in AnalogBoards[0] in VtiPLCInterface.config.");
                            }
                            else if (defaultInstance.AnalogBoards[0].NumberOfBits == 0)
                            {
                                MessageBox.Show("<NumberOfBits> tag is missing in AnalogBoards[0] in VtiPLCInterface.config.");
                            }
                            else if (defaultInstance.AnalogBoards[0].AnalogInputs[0].NumberOfSamples == 0)
                            {
                                MessageBox.Show("<NumberOfSamples> tag is missing in AnalogBoards[0].AnalogInputs[0] in VtiPLCInterface.config.");
                            }
                            else if (defaultInstance.AnalogBoards[0].AnalogInputs[0].NumberOfMovingAverages == 0)
                            {
                                MessageBox.Show("<NumberOfMovingAverages> tag is missing in AnalogBoards[0].AnalogInputs[0] in VtiPLCInterface.config.");
                            }
                            else if (defaultInstance.AnalogBoards[0].AnalogInputs[0].Range == 0)
                            {
                                MessageBox.Show("<Range> tag is missing or not set to Uni10Volts in AnalogBoards[0].AnalogInputs[0] in VtiPLCInterface.config.");
                            }
                            #endregion

                            try
                            {
                                // Propagate through the analog boards, and pass the
                                // MccBoard object along to each of the sub-classes
                                // Create the AnalogInput or AnalogOutput objects for each channel.
                                foreach (AnalogBoard board in defaultInstance.AnalogBoards)
                                {
                                    if (board.BoardNum >= 10 && board.BoardNum < 30)
                                    {
                                        if (board.BoardNum == 10 || board.BoardNum == 20)
                                        {
                                            //Modbus TCP communication initialize the socket
                                            _PLC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                        }
                                        else if (board.BoardNum == 11 || board.BoardNum == 21)
                                        {
                                            //Modbus TCP communication initialize the socket
                                            _PLC1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                        }
                                        else if (board.BoardNum == 12 || board.BoardNum == 22)
                                        {
                                            //Modbus TCP communication initialize the socket
                                            _PLC2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                        }
                                    }
                                    else if (board.BoardNum < 10)
                                    {
                                        //MCC IO card
                                        board.MccBoard = new MccDaq.MccBoard(board.BoardNum);
                                    }
                                    foreach (AnalogChannel channel in board.AnalogInputs)
                                    {
                                        channel.Board = board;
                                        _analogInput = new AnalogInput(channel);
                                        if (_analogInput.Name.Contains("IPADDRESS:"))
                                        {
                                            if (board.BoardNum >= 10 && board.BoardNum < 30)
                                            {
                                                string tempIPAddress = _analogInput.Name.Substring(_analogInput.Name.IndexOf(":") + 1);
                                                if (board.BoardNum == 10 || board.BoardNum == 20)
                                                {
                                                    strIPAddress = tempIPAddress;
                                                    _analogInput.Name = "STATUS1";
                                                }
                                                else if (board.BoardNum == 11 || board.BoardNum == 21)
                                                {
                                                    strIPAddress1 = tempIPAddress;
                                                    _analogInput.Name = "STATUS2";
                                                }
                                                else if (board.BoardNum == 12 || board.BoardNum == 22)
                                                {
                                                    strIPAddress2 = tempIPAddress;
                                                    _analogInput.Name = "STATUS3";
                                                }
                                                try
                                                {
                                                    _AnalogInputs.Add(_analogInput.Name, _analogInput);
                                                }
                                                catch (Exception ee)
                                                {
                                                    MessageBox.Show("Error adding analog input '" + _analogInput.Name + "'. " + ee.Message);
                                                }
                                            }
                                            else if (board.BoardNum >= 30 && board.BoardNum < 40)
                                            {
                                                strIPAddress = _analogInput.Name.Substring(_analogInput.Name.IndexOf(":") + 1);
                                                try
                                                {
                                                    _ABCompactLogixPLC = new EthernetIPforCLXComm();
                                                    _ABCompactLogixPLC.IPAddress = strIPAddress;
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                _AnalogInputs.Add(_analogInput.Name, _analogInput);
                                            }
                                            catch (Exception ee)
                                            {
                                                MessageBox.Show("Error adding analog input '" + _analogInput.Name + "'. " + ee.Message);
                                            }
                                        }
                                    }

                                    if (board.BoardNum >= 10 && board.BoardNum < 30)
                                    {
                                        try
                                        {
                                            if (board.BoardNum == 10 || board.BoardNum == 20)
                                            {
                                                IPAddress IPRemoteAddress = IPAddress.Parse(strIPAddress);
                                                IPEndPoint ipRemote = new IPEndPoint(IPRemoteAddress, 502);
                                                _PLC.BeginConnect(ipRemote, new AsyncCallback(ConnectCallback), _PLC);
                                                connectDone.WaitOne(5000);
                                                if (!_PLC.Connected)
                                                {
                                                    Console.WriteLine("Connection Fail");
                                                    board.AnalogInputs[0].Name = "STATUS1";
                                                    StatusValue = 10.0F;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Connection OK");
                                                    board.AnalogInputs[0].Name = "STATUS1";
                                                    StatusValue = 1.0F;
                                                }
                                            }
                                            else if (board.BoardNum == 11 || board.BoardNum == 21)
                                            {
                                                IPAddress IPRemoteAddress = IPAddress.Parse(strIPAddress1);
                                                IPEndPoint ipRemote = new IPEndPoint(IPRemoteAddress, 502);
                                                _PLC1.BeginConnect(ipRemote, new AsyncCallback(ConnectCallback), _PLC1);
                                                connectDone1.WaitOne(5000);
                                                if (!_PLC1.Connected)
                                                {
                                                    Console.WriteLine("Connection Fail");
                                                    board.AnalogInputs[0].Name = "STATUS2";
                                                    StatusValue1 = 10.0F;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Connection OK");
                                                    board.AnalogInputs[0].Name = "STATUS2";
                                                    StatusValue1 = 1.0F;
                                                }
                                            }
                                            else if (board.BoardNum == 12 || board.BoardNum == 22)
                                            {
                                                IPAddress IPRemoteAddress = IPAddress.Parse(strIPAddress2);
                                                IPEndPoint ipRemote = new IPEndPoint(IPRemoteAddress, 502);
                                                _PLC2.BeginConnect(ipRemote, new AsyncCallback(ConnectCallback), _PLC2);
                                                connectDone2.WaitOne(5000);
                                                if (!_PLC2.Connected)
                                                {
                                                    Console.WriteLine("Connection Fail");
                                                    board.AnalogInputs[0].Name = "STATUS3";
                                                    StatusValue2 = 10.0F;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Connection OK");
                                                    board.AnalogInputs[0].Name = "STATUS3";
                                                    StatusValue2 = 1.0F;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }


                                    foreach (AnalogChannel channel in board.AnalogOutputs)
                                    {
                                        channel.Board = board;
                                        _analogOutput = new AnalogOutput(channel);
                                        try
                                        {
                                            _AnalogOutputs.Add(_analogOutput.Name, _analogOutput);
                                        }
                                        catch (Exception ee)
                                        {
                                            MessageBox.Show("Error adding analog output '" + _analogOutput.Name + "'. " + ee.Message);
                                        }
                                    }
                                }

                                // Propagate through the digital boards, passing the
                                // MccBoard object along to each of the sub-classes
                                // Create the DigitalInput and DigitalOutput objects for the channels.
                                foreach (DigitalBoard board in defaultInstance.DigitalBoards)
                                {
                                    board.MccBoard = new MccDaq.MccBoard(board.BoardNum);
                                    foreach (DigitalPort port in board.DigitalPorts)
                                    {
                                        port.Board = board;
                                        // Set initial state of the port to be "off"
                                        if (board.InvertedLogic)
                                            port.Value = port.BitMask;
                                        else
                                            port.Value = 0;
                                        // Propagate through the DigitalPortBits, passing
                                        // along the port info
                                        foreach (DigitalPortBit bit in port.DigitalPortBits)
                                        {
                                            bit.Port = port;
                                            if (port.DigitalPortDirection == MccDaq.DigitalPortDirection.DigitalIn)
                                            {
                                                _digitalInput = new DigitalInput(bit);
                                                try
                                                {
                                                    _DigitalInputs.Add(_digitalInput.Name, _digitalInput);
                                                }
                                                catch (Exception ee)
                                                {
                                                    MessageBox.Show("Error adding digital input '" + _digitalInput.Name + "'. " + ee.Message);
                                                }
                                            }
                                            else
                                            {
                                                _digitalOutput = new DigitalOutput(bit);
                                                try
                                                {
                                                    _DigitalOutputs.Add(_digitalOutput.Name, _digitalOutput);
                                                }
                                                catch (Exception ee)
                                                {
                                                    MessageBox.Show("Error adding digital output '" + _digitalOutput.Name + "'. " + ee.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Error reading VtiPLCInterface.config from project folder. Make sure that:" +
                                Environment.NewLine + "1. The file is named VtiPLCInterface.config." +
                                Environment.NewLine + "2. There is a copy of the file in the bin\\Debug folder." +
                                Environment.NewLine + "3. The file is formatted correctly." +
                                Environment.NewLine + "4. There are no duplicate names in the file.");
                                VtiEvent.Log.WriteError("Error reading the configuration file 'VtiPLCInterface.config' for VTIPLCInterface.dll", VtiEventCatType.Application_Error, e.ToString());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error reading VtiPLCInterface.config from project folder. Make sure that:" +
                           Environment.NewLine + "1. The file is named VtiPLCInterface.config." +
                           Environment.NewLine + "2. There is a copy of the file in the bin\\Debug folder." +
                           Environment.NewLine + "3. The file is formatted correctly." +
                           Environment.NewLine + "4. There are no duplicate names in the file.");
                        VtiEvent.Log.WriteError("Unable to find the configuration file 'VtiPLCInterface.config' for VTIPLCInterface.dll", VtiEventCatType.Application_Error);
                    }
                }
                return defaultInstance;
            }
        }

        /// <summary>
        /// Gets or sets the analog boards.
        /// </summary>
        /// <value>The analog boards.</value>
        public List<AnalogBoard> AnalogBoards
        {
            get { return _AnalogBoards; }
            set { _AnalogBoards = value; }
        }

        /// <summary>
        /// Gets or sets the digital boards.
        /// </summary>
        /// <value>The digital boards.</value>
        public List<DigitalBoard> DigitalBoards
        {
            get { return _DigitalBoards; }
            set { _DigitalBoards = value; }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IAnalogInput">AnalogInputs</see> from the I/O Interface
        /// </summary>
        /// <value></value>
        [XmlIgnore()]
        public Dictionary<string, IAnalogInput> AnalogInputs
        {
            get { return _AnalogInputs; }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IAnalogOutput">AnalogOutputs</see> from the I/O Interface
        /// </summary>
        /// <value></value>
        [XmlIgnore()]
        public Dictionary<string, IAnalogOutput> AnalogOutputs
        {
            get { return _AnalogOutputs; }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IDigitalInput">DigitalInputs</see> from the I/O Interface
        /// </summary>
        /// <value></value>
        [XmlIgnore()]
        public Dictionary<string, IDigitalInput> DigitalInputs
        {
            get { return _DigitalInputs; }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{DigitalOutputs, TValue}">Dictionary</see> collection of the
        /// <see cref="IDigitalOutput">AnalogInputs</see> from the I/O Interface
        /// </summary>
        /// <value></value>
        [XmlIgnore()]
        public Dictionary<string, IDigitalOutput> DigitalOutputs
        {
            get { return _DigitalOutputs; }
        }

        /// <summary>
        /// Gets the thread that processes the I/O
        /// </summary>
        /// <value></value>
        public Thread ProcessThread
        {
            get { return thrd; }
        }

        #endregion
    }
}
