using System;

namespace VTIWindowsControlLibrary.Classes
{
    public class SystemDiagnostics
    {
        // Summary:
        //     Provides interaction with Windows event logs.
        //[DefaultEvent("EntryWritten")]
        //[InstallerType("System.Diagnostics.EventLogInstaller, System.Configuration.Install, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        //[MonitoringDescription("EventLogDesc")]
        public class EventLog
        {
            // Summary:
            //     Initializes a new instance of the System.Diagnostics.EventLog class. Does
            //     not associate the instance with any log.
            public EventLog()
            {
                return;
            }

            //
            // Summary:
            //     Initializes a new instance of the System.Diagnostics.EventLog class. Associates
            //     the instance with a log on the local computer.
            //
            // Parameters:
            //   logName:
            //     The name of the log on the local computer.
            //
            // Exceptions:
            //   System.ArgumentNullException:
            //     The log name is null.
            //
            //   System.ArgumentException:
            //     The log name is invalid.
            public EventLog(string logName)
            {
                return;
            }

            //
            // Summary:
            //     Initializes a new instance of the System.Diagnostics.EventLog class. Associates
            //     the instance with a log on the specified computer.
            //
            // Parameters:
            //   logName:
            //     The name of the log on the specified computer.
            //
            //   machineName:
            //     The computer on which the log exists.
            //
            // Exceptions:
            //   System.ArgumentNullException:
            //     The log name is null.
            //
            //   System.ArgumentException:
            //     The log name is invalid.  -or- The computer name is invalid.
            public EventLog(string logName, string machineName)
            {
                return;
            }

            //
            // Summary:
            //     Initializes a new instance of the System.Diagnostics.EventLog class. Associates
            //     the instance with a log on the specified computer and creates or assigns
            //     the specified source to the System.Diagnostics.EventLog.
            //
            // Parameters:
            //   logName:
            //     The name of the log on the specified computer
            //
            //   machineName:
            //     The computer on which the log exists.
            //
            //   source:
            //     The source of event log entries.
            //
            // Exceptions:
            //   System.ArgumentNullException:
            //     The log name is null.
            //
            //   System.ArgumentException:
            //     The log name is invalid.  -or- The computer name is invalid.
            public EventLog(string logName, string machineName, string source)
            {
                //EventLog EVL = new EventLog();
                return;
            }

            // Summary:
            //     Gets or sets a value indicating whether the System.Diagnostics.EventLog receives
            //     System.Diagnostics.EventLog.EntryWritten event notifications.
            //
            // Returns:
            //     true if the System.Diagnostics.EventLog receives notification when an entry
            //     is written to the log; otherwise, false.
            //
            // Exceptions:
            //   System.InvalidOperationException:
            //     The event log is on a remote computer.
            //[Browsable(false)]
            //[DefaultValue(false)]
            //[MonitoringDescription("LogMonitoring")]
            public bool EnableRaisingEvents { get; set; }

            //
            // Summary:
            //     Gets the contents of the event log.
            //
            // Returns:
            //     An System.Diagnostics.EventLogEntryCollection holding the entries in the
            //     event log. Each entry is associated with an instance of the System.Diagnostics.EventLogEntry
            //     class.
            //[Browsable(false)]
            //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            //[MonitoringDescription("LogEntries")]
            //public EventLogEntryCollection Entries { get; }
            //
            // Summary:
            //     Gets or sets the name of the log to read from or write to.
            //
            // Returns:
            //     The name of the log. This can be Application, System, Security, or a custom
            //     log name. The default is an empty string ("").
            //[DefaultValue("")]
            //[MonitoringDescription("LogLog")]
            //[ReadOnly(true)]
            //[RecommendedAsConfigurable(true)]
            //[TypeConverter("System.Diagnostics.Design.LogConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
            public string Log { get; set; }

            //
            // Summary:
            //     Gets the event log's friendly name.
            //
            // Returns:
            //     A name that represents the event log in the system's event viewer.
            //
            // Exceptions:
            //   System.InvalidOperationException:
            //     The specified System.Diagnostics.EventLog.Log does not exist in the registry
            //     for this computer.
            //[Browsable(false)]
            public string LogDisplayName { get; set; }

            //
            // Summary:
            //     Gets or sets the name of the computer on which to read or write events.
            //
            // Returns:
            //     The name of the server on which the event log resides. The default is the
            //     local computer (".").
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The computer name is invalid.
            //[DefaultValue(".")]
            //[MonitoringDescription("LogMachineName")]
            //[ReadOnly(true)]
            //[RecommendedAsConfigurable(true)]
            public string MachineName { get; set; }

            //
            // Summary:
            //     Gets or sets the maximum event log size in kilobytes.
            //
            // Returns:
            //     The maximum event log size in kilobytes. The default is 512, indicating a
            //     maximum file size of 512 kilobytes.
            //
            // Exceptions:
            //   System.ArgumentOutOfRangeException:
            //     The specified value is less than 64, or greater than 4194240, or not an even
            //     multiple of 64.
            //
            //   System.InvalidOperationException:
            //     The System.Diagnostics.EventLog.Log value is not a valid log name.  - or
            //     - The registry key for the event log could not be opened on the target computer.
            //[Browsable(false)]
            //[ComVisible(false)]
            //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public long MaximumKilobytes { get; set; }

            //
            // Summary:
            //     Gets the number of days to retain entries in the event log.
            //
            // Returns:
            //     The number of days that entries in the event log are retained. The default
            //     value is 7.
            //[Browsable(false)]
            //[ComVisible(false)]
            public int MinimumRetentionDays { get; set; }

            //
            //// Summary:
            ////     Gets the configured behavior for storing new entries when the event log reaches
            ////     its maximum log file size.
            ////
            //// Returns:
            ////     The System.Diagnostics.OverflowAction value that specifies the configured
            ////     behavior for storing new entries when the event log reaches its maximum log
            ////     size. The default is System.Diagnostics.OverflowAction.OverwriteOlder.
            //[Browsable(false)]
            //[ComVisible(false)]
            //public OverflowAction OverflowAction { get; }
            ////
            //// Summary:
            ////     Gets or sets the source name to register and use when writing to the event
            ////     log.
            ////
            //// Returns:
            ////     The name registered with the event log as a source of entries. The default
            ////     is an empty string ("").
            ////
            //// Exceptions:
            ////   System.ArgumentException:
            ////     The source name results in a registry key path longer than 254 characters.
            //[DefaultValue("")]
            //[MonitoringDescription("LogSource")]
            //[ReadOnly(true)]
            //[RecommendedAsConfigurable(true)]
            //[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
            public string Source { get; set; }

            //
            // Summary:
            //     Gets or sets the object used to marshal the event handler calls issued as
            //     a result of an System.Diagnostics.EventLog entry written event.
            //
            // Returns:
            //     The System.ComponentModel.ISynchronizeInvoke used to marshal event-handler
            //     calls issued as a result of an System.Diagnostics.EventLog.EntryWritten event
            //     on the event log.
            //[Browsable(false)]
            //[DefaultValue("")]
            //[MonitoringDescription("LogSynchronizingObject")]
            //public ISynchronizeInvoke SynchronizingObject { get; set; }

            //// Summary:
            ////     Occurs when an entry is written to an event log on the local computer.
            //[MonitoringDescription("LogEntryWritten")]
            //public event EntryWrittenEventHandler EntryWritten;

            // Summary:
            //     Begins the initialization of an System.Diagnostics.EventLog used on a form
            //     or used by another component. The initialization occurs at runtime.
            //
            // Exceptions:
            //   System.InvalidOperationException:
            //     System.Diagnostics.EventLog is already initialized.
            public void BeginInit()
            {
                return;
            }

            //
            // Summary:
            //     Removes all entries from the event log.
            //
            // Exceptions:
            //   System.ComponentModel.Win32Exception:
            //     The event log was not cleared successfully.  -or- The log cannot be opened.
            //     A Windows error code is not available.
            //
            //   System.ArgumentException:
            //     A value is not specified for the System.Diagnostics.EventLog.Log property.
            //     Make sure the log name is not an empty string.
            //
            //   System.InvalidOperationException:
            //     The log does not exist.
            public void Clear()
            {
                return;
            }

            //
            // Summary:
            //     Closes the event log and releases read and write handles.
            //
            // Exceptions:
            //   System.ComponentModel.Win32Exception:
            //     The event log's read handle or write handle was not released successfully.
            public void Close()
            {
                return;
            }

            //
            // Summary:
            //     Establishes an application as a valid event source for writing localized
            //     event messages, using the specified configuration properties for the event
            //     source and the corresponding event log.
            //
            // Parameters:
            //   sourceData:
            //     The configuration properties for the event source and its target event log.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The computer name specified in sourceData is not valid.  - or - The source
            //     name specified in sourceData is null.  - or - The log name specified in sourceData
            //     is not valid. Event log names must consist of printable characters and cannot
            //     include the characters '*', '?', or '\'.  - or - The log name specified in
            //     sourceData is not valid for user log creation. The Event log names AppEvent,
            //     SysEvent, and SecEvent are reserved for system use.  - or - The log name
            //     matches an existing event source name.  - or - The source name specified
            //     in sourceData results in a registry key path longer than 254 characters.
            //      - or - The first 8 characters of the log name specified in sourceData are
            //     not unique.  - or - The source name specified in sourceData is already registered.
            //      - or - The source name specified in sourceData matches an existing event
            //     log name.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ArgumentNullException:
            //     sourceData is null.
            //public static void CreateEventSource(EventSourceCreationData sourceData);
            //
            // Summary:
            //     Establishes an application, using the specified System.Diagnostics.EventLog.Source,
            //     as a valid event source for writing entries to a log on the local computer.
            //     This method can also create a new custom log on the local computer.
            //
            // Parameters:
            //   source:
            //     The source name by which the application is registered on the local computer.
            //
            //   logName:
            //     The name of the log the source's entries are written to. Possible values
            //     include: Application, System, or a custom event log.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     source is an empty string ("") or null.  - or - logName is not a valid event
            //     log name. Event log names must consist of printable characters, and cannot
            //     include the characters '*', '?', or '\'.  - or - The log name specified in
            //     sourceData is not valid for user log creation. The event log names AppEvent,
            //     SysEvent, and SecEvent are reserved for system use.  - or - The log name
            //     matches an existing event source name.  - or - The source name results in
            //     a registry key path longer than 254 characters.  - or - The first 8 characters
            //     of logName match the first 8 characters of an existing event log name.  -
            //     or - The source cannot be registered because it already exists on the local
            //     computer.  - or - The source name matches an existing event log name.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened on the local computer.
            public static void CreateEventSource(string source, string logName)
            {
                return;
            }

            //
            // Summary:
            //     Establishes an application, using the specified System.Diagnostics.EventLog.Source,
            //     as a valid event source for writing entries to a log on the computer specified
            //     by machineName. This method can also be used to create a new custom log on
            //     the specified computer.
            //
            // Parameters:
            //   source:
            //     The source by which the application is registered on the specified computer.
            //
            //   logName:
            //     The name of the log the source's entries are written to. Possible values
            //     include: Application, System, or a custom event log. If you do not specify
            //     a value, the logName defaults to Application.
            //
            //   machineName:
            //     The name of the computer to register this event source with, or "." for the
            //     local computer.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The machineName is not a valid computer name.  - or - source is an empty
            //     string ("") or null.  - or - logName is not a valid event log name. Event
            //     log names must consist of printable characters, and cannot include the characters
            //     '*', '?', or '\'.  - or - The log name specified in sourceData is not valid
            //     for user log creation. The event log names AppEvent, SysEvent, and SecEvent
            //     are reserved for system use.  - or - The log name matches an existing event
            //     source name.  - or - The source name results in a registry key path longer
            //     than 254 characters.  - or - The first 8 characters of logName match the
            //     first 8 characters of an existing event log name on the specified computer.
            //      - or - The source cannot be registered because it already exists on the
            //     specified computer.  - or - The source name matches an existing event source
            //     name.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened on the specified computer.
            [Obsolete("This method has been deprecated.  Please use System.Diagnostics.EventLog.CreateEventSource(EventSourceCreationData sourceData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
            public static void CreateEventSource(string source, string logName, string machineName)
            {
                return;
            }

            //
            // Summary:
            //     Removes an event log from the local computer.
            //
            // Parameters:
            //   logName:
            //     The name of the log to delete. Possible values include: Application, Security,
            //     System, and any custom event logs on the computer.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     logName is an empty string ("") or null.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened on the local computer.
            //      - or - The log does not exist on the local computer.
            //
            //   System.ComponentModel.Win32Exception:
            //     The event log was not cleared successfully.  -or- The log cannot be opened.
            //     A Windows error code is not available.
            public static void Delete(string logName)
            {
                return;
            }

            //
            // Summary:
            //     Removes an event log from the specified computer.
            //
            // Parameters:
            //   logName:
            //     The name of the log to delete. Possible values include: Application, Security,
            //     System, and any custom event logs on the specified computer.
            //
            //   machineName:
            //     The name of the computer to delete the log from, or "." for the local computer.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     logName is an empty string ("") or null. - or - machineName is not a valid
            //     computer name.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened on the specified computer.
            //      - or - The log does not exist on the specified computer.
            //
            //   System.ComponentModel.Win32Exception:
            //     The event log was not cleared successfully.  -or- The log cannot be opened.
            //     A Windows error code is not available.
            public static void Delete(string logName, string machineName)
            {
                return;
            }

            //
            // Summary:
            //     Removes the event source registration from the event log of the local computer.
            //
            // Parameters:
            //   source:
            //     The name by which the application is registered in the event log system.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source parameter does not exist in the registry of the local computer.
            //      - or - You do not have write access on the registry key for the event log.
            public static void DeleteEventSource(string source)
            {
                return;
            }

            //
            // Summary:
            //     Removes the application's event source registration from the specified computer.
            //
            // Parameters:
            //   source:
            //     The name by which the application is registered in the event log system.
            //
            //   machineName:
            //     The name of the computer to remove the registration from, or "." for the
            //     local computer.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The machineName parameter is invalid.  - or - The source parameter does not
            //     exist in the registry of the specified computer.  - or - You do not have
            //     write access on the registry key for the event log.
            //
            //   System.InvalidOperationException:
            //     source cannot be deleted because in the registry, the parent registry key
            //     for source does not contain a subkey with the same name.
            public static void DeleteEventSource(string source, string machineName)
            {
                return;
            }

            //
            // Summary:
            //     Releases the unmanaged resources used by the System.Diagnostics.EventLog,
            //     and optionally releases the managed resources.
            //
            // Parameters:
            //   disposing:
            //     true to release both managed and unmanaged resources; false to release only
            //     unmanaged resources.
            //protected override void Dispose(bool disposing)
            //{
            //    return;
            //}
            //
            // Summary:
            //     Ends the initialization of an System.Diagnostics.EventLog used on a form
            //     or by another component. The initialization occurs at runtime.
            public void EndInit()
            {
                return;
            }

            //
            // Summary:
            //     Determines whether the log exists on the local computer.
            //
            // Parameters:
            //   logName:
            //     The name of the log to search for. Possible values include: Application,
            //     Security, System, other application-specific logs (such as those associated
            //     with Active Directory), or any custom log on the computer.
            //
            // Returns:
            //     true if the log exists on the local computer; otherwise, false.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The logName is null or the value is empty.
            public static bool Exists(string logName)
            {
                return true;
            }

            //
            // Summary:
            //     Determines whether the log exists on the specified computer.
            //
            // Parameters:
            //   logName:
            //     The log for which to search. Possible values include: Application, Security,
            //     System, other application-specific logs (such as those associated with Active
            //     Directory), or any custom log on the computer.
            //
            //   machineName:
            //     The name of the computer on which to search for the log, or "." for the local
            //     computer.
            //
            // Returns:
            //     true if the log exists on the specified computer; otherwise, false.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The machineName parameter is an invalid format. Make sure you have used proper
            //     syntax for the computer on which you are searching.  -or- The logName is
            //     null or the value is empty.
            public static bool Exists(string logName, string machineName)
            {
                return true;
            }

            //
            // Summary:
            //     Searches for all event logs on the local computer and creates an array of
            //     System.Diagnostics.EventLog objects that contain the list.
            //
            // Returns:
            //     An array of type System.Diagnostics.EventLog that represents the logs on
            //     the local computer.
            //
            // Exceptions:
            //   System.SystemException:
            //     You do not have read access to the registry.  -or- There is no event log
            //     service on the computer.
            public static EventLog[] GetEventLogs()
            {
                EventLog[] EVL = new EventLog[1];
                return EVL;
            }

            //
            // Summary:
            //     Searches for all event logs on the given computer and creates an array of
            //     System.Diagnostics.EventLog objects that contain the list.
            //
            // Parameters:
            //   machineName:
            //     The computer on which to search for event logs.
            //
            // Returns:
            //     An array of type System.Diagnostics.EventLog that represents the logs on
            //     the given computer.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The machineName parameter is an invalid computer name.
            //
            //   System.InvalidOperationException:
            //     You do not have read access to the registry.  -or- There is no event log
            //     service on the computer.
            public static EventLog[] GetEventLogs(string machineName)
            {
                EventLog[] EVL = new EventLog[1];
                return EVL;
            }

            //
            // Summary:
            //     Gets the name of the log to which the specified source is registered.
            //
            // Parameters:
            //   source:
            //     The name of the event source.
            //
            //   machineName:
            //     The name of the computer on which to look, or "." for the local computer.
            //
            // Returns:
            //     The name of the log associated with the specified source in the registry.
            public static string LogNameFromSourceName(string source, string machineName)
            {
                return "dummy";
            }

            //
            // Summary:
            //     Changes the configured behavior for writing new entries when the event log
            //     reaches its maximum file size.
            //
            // Parameters:
            //   action:
            //     The overflow behavior for writing new entries to the event log.
            //
            //   retentionDays:
            //     The minimum number of days each event log entry is retained. This parameter
            //     is used only if action is set to System.Diagnostics.OverflowAction.OverwriteOlder.
            //
            // Exceptions:
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     action is not a valid System.Diagnostics.EventLog.OverflowAction value.
            //
            //   System.ArgumentOutOfRangeException:
            //     retentionDays is less than one, or larger than 365.
            //
            //   System.InvalidOperationException:
            //     The System.Diagnostics.EventLog.Log value is not a valid log name.  - or
            //     - The registry key for the event log could not be opened on the target computer.
            //[ComVisible(false)]
            //public void ModifyOverflowPolicy(OverflowAction action, int retentionDays);
            //
            // Summary:
            //     Specifies the localized name of the event log, which is displayed in the
            //     server Event Viewer.
            //
            // Parameters:
            //   resourceFile:
            //     The fully specified path to a localized resource file.
            //
            //   resourceId:
            //     The resource identifier that indexes a localized string within the resource
            //     file.
            //
            // Exceptions:
            //   System.InvalidOperationException:
            //     The System.Diagnostics.EventLog.Log value is not a valid log name.  - or
            //     - The registry key for the event log could not be opened on the target computer.
            //[ComVisible(false)]
            //public void RegisterDisplayName(string resourceFile, long resourceId);
            //
            // Summary:
            //     Determines whether an event source is registered on the local computer.
            //
            // Parameters:
            //   source:
            //     The name of the event source.
            //
            // Returns:
            //     true if the event source is registered on the local computer; otherwise,
            //     false.
            //
            // Exceptions:
            //   System.Security.SecurityException:
            //     source was not found, but some or all of the event logs could not be searched.
            public static bool SourceExists(string source)
            {
                return true;
            }

            //
            // Summary:
            //     Determines whether an event source is registered on a specified computer.
            //
            // Parameters:
            //   source:
            //     The name of the event source.
            //
            //   machineName:
            //     The name the computer on which to look, or "." for the local computer.
            //
            // Returns:
            //     true if the event source is registered on the given computer; otherwise,
            //     false.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     machineName is an invalid computer name.
            //
            //   System.Security.SecurityException:
            //     source was not found, but some or all of the event logs could not be searched.
            public static bool SourceExists(string source, string machineName)
            {
                return true;
            }

            //
            // Summary:
            //     Writes an information type entry, with the given message text, to the event
            //     log.
            //
            // Parameters:
            //   message:
            //     The string to write to the event log.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The System.Diagnostics.EventLog.Source property of the System.Diagnostics.EventLog
            //     has not been set.  -or- The method attempted to register a new event source,
            //     but the computer name in System.Diagnostics.EventLog.MachineName is not valid.
            //      - or - The source is already registered for a different event log.  - or
            //     - The message string is longer than 32766 bytes.  - or - The source name
            //     results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            
            //public void WriteEntry(string message)
            //{
            //    return;
            //}

            //
            // Summary:
            //     Writes an error, warning, information, success audit, or failure audit entry
            //     with the given message text to the event log.
            //
            // Parameters:
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The System.Diagnostics.EventLog.Source property of the System.Diagnostics.EventLog
            //     has not been set.  -or- The method attempted to register a new event source,
            //     but the computer name in System.Diagnostics.EventLog.MachineName is not valid.
            //      - or - The source is already registered for a different event log.  - or
            //     - The message string is longer than 32766 bytes.  - or - The source name
            //     results in a registry key path longer than 254 characters.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public void WriteEntry(string message, EventLogEntryType type)
            {
                return;
            }

            //
            // Summary:
            //     Writes an information type entry with the given message text to the event
            //     log, using the specified registered event source.
            //
            // Parameters:
            //   source:
            //     The source by which the application is registered on the specified computer.
            //
            //   message:
            //     The string to write to the event log.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source value is an empty string ("").  - or - The source value is null.
            //      - or - The message string is longer than 32766 bytes.  - or - The source
            //     name results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public static void WriteEntry(string source, string message)
            {
                return;
            }

            //
            // Summary:
            //     Writes an entry with the given message text and application-defined event
            //     identifier to the event log.
            //
            // Parameters:
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            //   eventID:
            //     The application-specific identifier for the event.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The System.Diagnostics.EventLog.Source property of the System.Diagnostics.EventLog
            //     has not been set.  -or- The method attempted to register a new event source,
            //     but the computer name in System.Diagnostics.EventLog.MachineName is not valid.
            //      - or - The source is already registered for a different event log.  - or
            //     - eventID is less than zero or greater than System.UInt16.MaxValue.  - or
            //     - The message string is longer than 32766 bytes.  - or - The source name
            //     results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public void WriteEntry(string message, EventLogEntryType type, int eventID)
            {
                return;
            }

            //
            // Summary:
            //     Writes an error, warning, information, success audit, or failure audit entry
            //     with the given message text to the event log, using the specified registered
            //     event source.
            //
            // Parameters:
            //   source:
            //     The source by which the application is registered on the specified computer.
            //
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source value is an empty string ("").  - or - The source value is null.
            //      - or - The message string is longer than 32766 bytes.  - or - The source
            //     name results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public static void WriteEntry(string source, string message, EventLogEntryType type)
            {
                return;
            }

            //
            // Summary:
            //     Writes an entry with the given message text, application-defined event identifier,
            //     and application-defined category to the event log.
            //
            // Parameters:
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            //   eventID:
            //     The application-specific identifier for the event.
            //
            //   category:
            //     The application-specific subcategory associated with the message.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The System.Diagnostics.EventLog.Source property of the System.Diagnostics.EventLog
            //     has not been set.  -or- The method attempted to register a new event source,
            //     but the computer name in System.Diagnostics.EventLog.MachineName is not valid.
            //      - or - The source is already registered for a different event log.  - or
            //     - eventID is less than zero or greater than System.UInt16.MaxValue.  - or
            //     - The message string is longer than 32766 bytes.  - or - The source name
            //     results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public void WriteEntry(string message, EventLogEntryType type, int eventID, short category)
            {
                return;
            }

            //
            // Summary:
            //     Writes an entry with the given message text and application-defined event
            //     identifier to the event log, using the specified registered event source.
            //
            // Parameters:
            //   source:
            //     The source by which the application is registered on the specified computer.
            //
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            //   eventID:
            //     The application-specific identifier for the event.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source value is an empty string ("").  - or - The source value is null.
            //      - or - eventID is less than zero or greater than System.UInt16.MaxValue.
            //      - or - The message string is longer than 32766 bytes.  - or - The source
            //     name results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID)
            {
                return;
            }

            //
            // Summary:
            //     Writes an entry with the given message text, application-defined event identifier,
            //     and application-defined category to the event log, and appends binary data
            //     to the message.
            //
            // Parameters:
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            //   eventID:
            //     The application-specific identifier for the event.
            //
            //   category:
            //     The application-specific subcategory associated with the message.
            //
            //   rawData:
            //     An array of bytes that holds the binary data associated with the entry.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The System.Diagnostics.EventLog.Source property of the System.Diagnostics.EventLog
            //     has not been set.  -or- The method attempted to register a new event source,
            //     but the computer name in System.Diagnostics.EventLog.MachineName is not valid.
            //      - or - The source is already registered for a different event log.  - or
            //     - eventID is less than zero or greater than System.UInt16.MaxValue.  - or
            //     - The message string is longer than 32766 bytes.  - or - The source name
            //     results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
            {
                return;
            }

            //
            // Summary:
            //     Writes an entry with the given message text, application-defined event identifier,
            //     and application-defined category to the event log, using the specified registered
            //     event source. The category can be used by the Event Viewer to filter events
            //     in the log.
            //
            // Parameters:
            //   source:
            //     The source by which the application is registered on the specified computer.
            //
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            //   eventID:
            //     The application-specific identifier for the event.
            //
            //   category:
            //     The application-specific subcategory associated with the message.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source value is an empty string ("").  - or - The source value is null.
            //      - or - eventID is less than zero or greater than System.UInt16.MaxValue.
            //      - or - The message string is longer than 32766 bytes.  - or - The source
            //     name results in a registry key path longer than 254 characters.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category)
            {
                return;
            }

            //
            // Summary:
            //     Writes an entry with the given message text, application-defined event identifier,
            //     and application-defined category to the event log (using the specified registered
            //     event source) and appends binary data to the message.
            //
            // Parameters:
            //   source:
            //     The source by which the application is registered on the specified computer.
            //
            //   message:
            //     The string to write to the event log.
            //
            //   type:
            //     One of the System.Diagnostics.EventLogEntryType values.
            //
            //   eventID:
            //     The application-specific identifier for the event.
            //
            //   category:
            //     The application-specific subcategory associated with the message.
            //
            //   rawData:
            //     An array of bytes that holds the binary data associated with the entry.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source value is an empty string ("").  - or - The source value is null.
            //      - or - eventID is less than zero or greater than System.UInt16.MaxValue.
            //      - or - The message string is longer than 32766 bytes.  - or - The source
            //     name results in a registry key path longer than 254 characters.
            //
            //   System.ComponentModel.InvalidEnumArgumentException:
            //     type is not a valid System.Diagnostics.EventLogEntryType.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
            {
                return;
            }

            //
            // Summary:
            //     Writes a localized entry to the event log.
            //
            // Parameters:
            //   instance:
            //     An System.Diagnostics.EventInstance instance that represents a localized
            //     event log entry.
            //
            //   values:
            //     An array of strings to merge into the message text of the event log entry.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The System.Diagnostics.EventLog.Source property of the System.Diagnostics.EventLog
            //     has not been set.  -or- The method attempted to register a new event source,
            //     but the computer name in System.Diagnostics.EventLog.MachineName is not valid.
            //      - or - The source is already registered for a different event log.  - or
            //     - instance.InstanceId is less than zero or greater than System.UInt16.MaxValue.
            //      - or - values has more than 256 elements.  - or - One of the values elements
            //     is longer than 32766 bytes.  - or - The source name results in a registry
            //     key path longer than 254 characters.
            //
            //   System.ArgumentNullException:
            //     instance is null.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            //[ComVisible(false)]
            //public void WriteEvent(EventInstance instance, params object[] values);
            //
            // Summary:
            //     Writes an event log entry with the given event data, message replacement
            //     strings, and associated binary data.
            //
            // Parameters:
            //   instance:
            //     An System.Diagnostics.EventInstance instance that represents a localized
            //     event log entry.
            //
            //   data:
            //     An array of bytes that holds the binary data associated with the entry.
            //
            //   values:
            //     An array of strings to merge into the message text of the event log entry.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The System.Diagnostics.EventLog.Source property of the System.Diagnostics.EventLog
            //     has not been set.  -or- The method attempted to register a new event source,
            //     but the computer name in System.Diagnostics.EventLog.MachineName is not valid.
            //      - or - The source is already registered for a different event log.  - or
            //     - instance.InstanceId is less than zero or greater than System.UInt16.MaxValue.
            //      - or - values has more than 256 elements.  - or - One of the values elements
            //     is longer than 32766 bytes.  - or - The source name results in a registry
            //     key path longer than 254 characters.
            //
            //   System.ArgumentNullException:
            //     instance is null.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            //[ComVisible(false)]
            //public void WriteEvent(EventInstance instance, byte[] data, params object[] values);
            //
            // Summary:
            //     Writes an event log entry with the given event data and message replacement
            //     strings, using the specified registered event source.
            //
            // Parameters:
            //   source:
            //     The name of the event source registered for the application on the specified
            //     computer.
            //
            //   instance:
            //     An System.Diagnostics.EventInstance instance that represents a localized
            //     event log entry.
            //
            //   values:
            //     An array of strings to merge into the message text of the event log entry.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source value is an empty string ("").  - or - The source value is null.
            //      - or - instance.InstanceId is less than zero or greater than System.UInt16.MaxValue.
            //      - or - values has more than 256 elements.  - or - One of the values elements
            //     is longer than 32766 bytes.  - or - The source name results in a registry
            //     key path longer than 254 characters.
            //
            //   System.ArgumentNullException:
            //     instance is null.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            //public static void WriteEvent(string source, EventInstance instance, params object[] values);
            //
            // Summary:
            //     Writes an event log entry with the given event data, message replacement
            //     strings, and associated binary data, and using the specified registered event
            //     source.
            //
            // Parameters:
            //   source:
            //     The name of the event source registered for the application on the specified
            //     computer.
            //
            //   instance:
            //     An System.Diagnostics.EventInstance instance that represents a localized
            //     event log entry.
            //
            //   data:
            //     An array of bytes that holds the binary data associated with the entry.
            //
            //   values:
            //     An array of strings to merge into the message text of the event log entry.
            //
            // Exceptions:
            //   System.ArgumentException:
            //     The source value is an empty string ("").  - or - The source value is null.
            //      - or - instance.InstanceId is less than zero or greater than System.UInt16.MaxValue.
            //      - or - values has more than 256 elements.  - or - One of the values elements
            //     is longer than 32766 bytes.  - or - The source name results in a registry
            //     key path longer than 254 characters.
            //
            //   System.ArgumentNullException:
            //     instance is null.
            //
            //   System.InvalidOperationException:
            //     The registry key for the event log could not be opened.
            //
            //   System.ComponentModel.Win32Exception:
            //     The operating system reported an error when writing the event entry to the
            //     event log. A Windows error code is not available.
            //public static void WriteEvent(string source, EventInstance instance, byte[] data, params object[] values);
        }

        // Summary:
        //     Specifies what messages to output for the System.Diagnostics.Debug, System.Diagnostics.Trace
        //     and System.Diagnostics.TraceSwitch classes.
        public enum TraceLevel
        {
            // Summary:
            //     Output no tracing and debugging messages.
            Off = 0,

            //
            // Summary:
            //     Output informational messages.
            Info = 1,

            //
            // Summary:
            //     Output info, warning, and error-handling messages.
            Error = 2,

            //
            // Summary:
            //     Output info and warnings and error-handling messages.
            Warning = 3,

            //
            // Summary:
            //     Output all debugging and tracing messages.
            Verbose = 4,
        }

        // Summary:
        //     Specifies the event type of an event log entry.
        public enum EventLogEntryType
        {
            // Summary:
            //     An error event. This indicates a significant problem the user should know
            //     about; usually a loss of functionality or data.
            Error = 1,

            //
            // Summary:
            //     A warning event. This indicates a problem that is not immediately significant,
            //     but that may signify conditions that could cause future problems.
            Warning = 2,

            //
            // Summary:
            //     An information event. This indicates a significant, successful operation.
            Information = 4,

            //
            // Summary:
            //     A success audit event. This indicates a security event that occurs when an
            //     audited access attempt is successful; for example, logging on successfully.
            SuccessAudit = 8,

            //
            // Summary:
            //     A failure audit event. This indicates a security event that occurs when an
            //     audited access attempt fails; for example, a failed attempt to open a file.
            FailureAudit = 16,
        }
    }
}