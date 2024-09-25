using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO;

//using System.Diagnostics;

using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Sub-class of the <see cref="EventLog">EventLog</see>, customized for the
    /// <see cref="VTIWindowsControlLibrary">VTIWindowsControlLibrary</see>
    /// </summary>
    /// <remarks>
    /// Adds a TraceLevel to the event log, and WriteVerbose, WriteInfo, WriteWarning, and WriteError methods
    /// that write to the event log only if the TraceLevel is at or above their level.
    /// Each Write* method is overloaded to allow simple messages, event categories, and
    /// the ability to automatically log the value of any parameters, analog signals, or other string values
    /// that are passed to an optional parameter list
    /// </remarks>
    public class VtiEventLog : VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLog
    {
        private VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel traceLevel = VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Info;
        private String _lastMessage; // keep last message and don't write repeats
        private bool bWriteToSimpleTextFile;
        private int bDaysToKeepOldVtiEventLogTextFiles;
        private string strLogFolder, strLogFile;

        //public VtiEventLog() : base() { }
        //public VtiEventLog(string logName) : base(logName) { }
        //public VtiEventLog(string logName, string machineName) : base(logName, machineName) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VtiEventLog">VtiEventLog</see> class
        /// </summary>
        /// <param name="logName">Name of the Event Log</param>
        /// <param name="machineName">Machine Name for the Event Log</param>
        /// <param name="source">Source name to be used in the Event Log</param>
        public VtiEventLog(string logName, string machineName, string source) : base(logName, machineName, source)
        {
            //bWriteToSimpleTextFile = false; // attempt to write to Event Log until have and problem, then write to simple test file instead
            bWriteToSimpleTextFile = true;
            bDaysToKeepOldVtiEventLogTextFiles = 6; // keep old VtiEventLog
            strLogFolder = "";
            strLogFile = "";
        }

        /// <summary>
        /// Indicates the level of messages to be written to the
        /// <see cref="VtiEventLog">VtiEventLog</see>
        /// </summary>
        public VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel Level
        {
            get { return traceLevel; }
            set { traceLevel = value; }
        }

        /// <summary>
        /// Indicates whether the
        /// <see cref="VtiEventLog">VtiEventLog</see>
        /// is set to write events to a simple text file
        /// </summary>
        public bool WriteToSimpleTextFile
        {
            get { return bWriteToSimpleTextFile; }
            set { bWriteToSimpleTextFile = value; }
        }

        /// <summary>
        /// Number of days to keep old records of
        /// <see cref="VtiEventLog">VtiEventLog</see>
        /// simple text file
        /// </summary>
        public int DaysToKeepOldVtiEventLogTextFiles
        {
            get { return bDaysToKeepOldVtiEventLogTextFiles; }
            set { bDaysToKeepOldVtiEventLogTextFiles = value; }
        }

        /// <summary>
        /// Indicates name of the
        /// <see cref="VtiEventLog">VtiEventLog</see>
        /// text file that last received events from the EventLog
        /// </summary>
        public string LogFileName
        {
            get { return strLogFile; }
        }

        /// <summary>
        /// Write a verbose message to the Event Log.
        /// Verbose messages only get written to the event log if the Trace Level at the Verbose level.
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        public void WriteVerbose(string message)
        {
            this.WriteVerbose(message, VtiEventCatType.None);
        }

        /// <summary>
        /// Write a verbose message to the Event Log.
        /// Verbose messages only get written to the event log if the Trace Level at the Verbose level.
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        public void WriteVerbose(string message, VtiEventCatType cat)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Verbose)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Information, 0, (short)cat);
        }

        /// <summary>
        /// Write a verbose message to the Event Log.
        /// Verbose messages only get written to the event log if the Trace Level at the Verbose level.
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        /// <param name="args">NumericParamter, AnalogSignal, or String</param>
        public void WriteVerbose(string message, VtiEventCatType cat, params object[] args)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Verbose)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Information, cat, args);
        }

        /// <summary>
        /// Write an informational message to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        public void WriteInfo(string message)
        {
            this.WriteInfo(message, VtiEventCatType.None);
        }

        /// <summary>
        /// Write an informational message to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        public void WriteInfo(string message, VtiEventCatType cat)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Info)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Information, 0, (short)cat);
        }

        /// <summary>
        /// Write an informational message to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        /// <param name="args">NumericParamter, AnalogSignal, or String</param>
        public void WriteInfo(string message, VtiEventCatType cat, params object[] args)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Info)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Information, cat, args);
        }

        /// <summary>
        /// Write a warning to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        public void WriteWarning(string message)
        {
            this.WriteWarning(message, VtiEventCatType.None);
        }

        /// <summary>
        /// Write a warning to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        public void WriteWarning(string message, VtiEventCatType cat)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Warning)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Warning, 0, (short)cat);
        }

        /// <summary>
        /// Write a warning to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        /// <param name="args">NumericParamter, AnalogSignal, or String</param>
        public void WriteWarning(string message, VtiEventCatType cat, params object[] args)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Warning)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Warning, cat, args);
        }

        /// <summary>
        /// Write an error entry to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        public void WriteError(string message)
        {
            this.WriteError(message, VtiEventCatType.None);
        }

        /// <summary>
        /// Write an error entry to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        public void WriteError(string message, VtiEventCatType cat)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Error)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Error, 0, (short)cat);
        }

        /// <summary>
        /// Write an error entry to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="cat">Event Category</param>
        /// <param name="args">NumericParamter, AnalogSignal, or String</param>
        public void WriteError(string message, VtiEventCatType cat, params object[] args)
        {
            if (traceLevel >= VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Error)
                this.WriteEntry(message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Error, cat, args);
        }

        /// <summary>
        /// Write an entry to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="type">Event Type</param>
        /// <param name="cat">Event Category</param>
        public void WriteEntry(string message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType type, VtiEventCatType cat)
        {
            VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel level;
            if (type == VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Warning) level = VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Warning;
            else if (type == VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType.Error) level = VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Error;
            else level = VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel.Info;
            //if (traceLevel >= level)
            WriteEntry(message, type, 0, (short)cat);
        }

        /// <summary>
        /// WriteEntry an entry to the Event Log
        /// </summary>
        /// <param name="message">Message text to appear in Event Log</param>
        /// <param name="type">Event Type</param>
        /// <param name="cat">Event Category</param>
        /// <param name="args">NumericParamter, AnalogSignal, or String</param>
        public void WriteEntry(string message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType type, VtiEventCatType cat, params object[] args)
        {
            string sMessage;
            TimeDelayParameter timeDelay;
            NumericParameter numericParameter;
            AnalogSignal signal;

            //            if (message != _lastMessage)  // Rod Edit - 2010-05-14
            {
                _lastMessage = message;

                // Append a couple of newlines to the message
                sMessage = message + Environment.NewLine + Environment.NewLine;

                // Interate through the parameters
                foreach (object obj in args.Where(A => A != null))
                {
                    // parameter is a NumericParameter...
                    if (obj is NumericParameter)
                    {
                        numericParameter = obj as NumericParameter;
                        // format a message using the Display Name, Process Value, and Units of the parameter
                        sMessage += string.Format("     {0}: {1} {2}", numericParameter.DisplayName, numericParameter.ProcessValue.ToString(numericParameter.StringFormat), numericParameter.Units);
                        sMessage += Environment.NewLine;
                    }
                    else if (obj is TimeDelayParameter) // parameter is a time delay
                    {
                        timeDelay = obj as TimeDelayParameter;
                        // format a message using the Label, Value, and Units of the time delay
                        sMessage += string.Format("     {0}: {1} {2}", timeDelay.DisplayName, timeDelay.ProcessValue.ToString(), timeDelay.Units);
                        sMessage += Environment.NewLine;
                    }
                    else if (obj is AnalogSignal) // parameter is an AnalogSignal
                    {
                        signal = obj as AnalogSignal;
                        // format a message using the Label, Value, and Units of the AnalogSignal
                        sMessage += string.Format("     {0}: {1} {2}", signal.Label, signal.Value.ToString(signal.Format), signal.Units);
                        sMessage += Environment.NewLine;
                    }
                    // parameter is a string
                    else if (obj is string)
                    {
                        // just add the string to the message
                        sMessage += "     " + (obj as string);
                        sMessage += Environment.NewLine;
                    }
                }
                WriteEntry(sMessage, type, cat);
            }
        }

        private new void WriteEntry(string message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType type, int eventID, short category)
        {
            try
            {
                if (!bWriteToSimpleTextFile) // write to Event Log
                    base.WriteEntry(message, type, eventID, category);
                else // write to text file
                    WriteTextEntry(message, type, eventID, category);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                bWriteToSimpleTextFile = true;
                WriteTextEntry(message, type, eventID, category);
            }
        }

        private void WriteTextEntry(string message, VTIWindowsControlLibrary.Classes.SystemDiagnostics.EventLogEntryType type, int eventID, short category)
        {
            try
            {
                if (strLogFolder.Length == 0)
                {
                    var _allUserSettingsProvider = new AllUsersSettingsProvider();
                    strLogFolder = _allUserSettingsProvider.GetSettingsPath() + @"\Log\";

                    //strLogFolder = string.Format(@"{0}\{1}\{2}\{3}\Log\",
                    //  //System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), //this is %ProgramData%
                    //  "C:\\VTI PC\\Config",
                    //  Application.CompanyName,
                    //  Application.ProductName,
                    //  Application.ProductVersion);

                    // create the Log folder if it does not already exist
                    if (!Directory.Exists(strLogFolder))
                    {
                        Directory.CreateDirectory(strLogFolder);
                        DirectoryInfo di = new DirectoryInfo(strLogFolder);
                        di.Attributes = di.Attributes & ~FileAttributes.ReadOnly;
                    }
                    // create the System Log folder if it does not already exist
                    strLogFolder += @"System Log\";
                    if (!Directory.Exists(strLogFolder))
                    {
                        Directory.CreateDirectory(strLogFolder);
                        DirectoryInfo di = new DirectoryInfo(strLogFolder);
                        di.Attributes = di.Attributes & ~FileAttributes.ReadOnly;
                    }
                    if (bDaysToKeepOldVtiEventLogTextFiles > 0)
                    {
                        // delete old log files
                        foreach (string f in Directory.GetFiles(strLogFolder))
                        {
                            if (f.Substring(f.Length - ".txt".Length) == ".txt")
                            {
                                DateTime dt = File.GetLastWriteTime(f), CrntTime = DateTime.Now;
                                TimeSpan ElapsedTime = CrntTime - dt;
                                if (ElapsedTime.Days > bDaysToKeepOldVtiEventLogTextFiles)
                                    File.Delete(f);
                            }
                        }
                    }
                }
                if (strLogFolder.Length > 0)
                {
                    strLogFile = strLogFolder + "Log " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    if (!File.Exists(strLogFile))
                    {
                        FileStream fs = File.Create(strLogFile);
                        FileInfo fi = new FileInfo(strLogFile);
                        fi.Attributes = fi.Attributes & ~FileAttributes.ReadOnly;
                        fs.Close();
                    }

                    using (StreamWriter sw = new StreamWriter(strLogFile, true))
                    {
                        sw.Write(DateTime.Now.ToString("HH:mm:ss") + " : " + type.ToString() + " : " + message);
                        sw.Write("\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error occurred during EventLog WriteTextEntry at location " + strLogFolder, " Failed VtiEvent WriteTextEntry",
                //  MessageBoxButtons.OK);
            }
        }

        List<(string deviceName, DateTime lastLogEntry)> commErrorLogList = new List<(string deviceName, DateTime lastLogEntry)>();
        /// <summary>
        /// Records a communication error message to the Event Log if the last communication error message was more than 5 minutes ago, as to not flood the Event Log with the same message.
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="exceptionMessage"></param>
        internal void RecordCOMErrorToEventLog(string deviceName, string exceptionMessage)
        {
            if (!commErrorLogList.Select(x => x.deviceName).Contains(deviceName))
            {
                commErrorLogList.Add((deviceName, DateTime.Now));
                VtiEvent.Log.WriteError(
                            $"Error communicating with {deviceName}.",
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            exceptionMessage);
            }
            else
            {
                var item = commErrorLogList.Where(x => x.deviceName == deviceName).FirstOrDefault();
                if ((DateTime.Now - item.lastLogEntry).TotalMinutes > 5)
                {
                    int index = commErrorLogList.IndexOf(item);
                    commErrorLogList[index] = (item.deviceName, DateTime.Now);
                    VtiEvent.Log.WriteError(
                            $"Error communicating with {deviceName}.",
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                            exceptionMessage);
                }
            }
        }
    }
}