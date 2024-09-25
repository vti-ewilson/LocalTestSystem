//using System.Diagnostics;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Components;

namespace VTIWindowsControlLibrary.Classes
{
    /// <summary>
    /// Provides static access to the Event Log and Event Viewer
    /// Calls Initialize() on the first use of either the log or the viewer to set up the event source
    /// </summary>
    public class VtiEvent
    {
        private static VtiEventLog eventLog;
        //private static EventLogViewerForm eventLogViewer;
        //private static EventSourceCreationData eventSourceCreationData;

        /// <summary>
        /// Initializes the static instance of the <see cref="VtiEvent">VtiEvent</see> class
        /// </summary>
        static VtiEvent()
        {
            //// Delete and recreate the event source, to make sure that the latest VtiEventCatMsgs.dll
            //// is associated with the event source in the registry
            //if (VtiEventLog.SourceExists(Application.ProductName))
            //  VtiEventLog.DeleteEventSource(Application.ProductName);
            //if (!VtiEventLog.SourceExists(Application.ProductName))
            //{
            //  eventSourceCreationData = new EventSourceCreationData(Application.ProductName, "VTIEvent");
            //  eventSourceCreationData.CategoryCount = 6;
            //  eventSourceCreationData.CategoryResourceFile = @"C:\WINDOWS\system32\VtiEventCatMsgs.dll";
            //  VtiEventLog.CreateEventSource(eventSourceCreationData);
            //}
            // Create the event log
            eventLog = new VtiEventLog("VTIEvent", ".", Application.ProductName);
            // Create the event viewer
            //eventLogViewer = new EventLogViewerForm(eventLog);
            //// Add EntryWrittenEventHandler to cause events to get appended to the event viewer
            //eventLog.EnableRaisingEvents = true;
            //eventLog.EntryWritten += new EntryWrittenEventHandler(eventLogViewer.VtiLog_EntryWritten);
            //eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 7);
            VtiEvent.Log.WriteVerbose("Event Log Initialized...");

            //is editCycleParameters config missing or corrupt?
            bool isCorruptOrMissing = false;
            var _allUserSettingsProvider = new AllUsersSettingsProvider();
            FileInfo configFile = new FileInfo(_allUserSettingsProvider.GetSettingsPathAndFilename());
            if (configFile.Exists)
            {
                using (StreamReader s = new StreamReader(configFile.FullName))
                {
                    try
                    {
                        //try to load the config file
                        XmlDocument doc = new XmlDocument();
                        doc.Load(configFile.FullName);
                        XmlNodeList editCycleParamList = doc.GetElementsByTagName("DisplayName");
                    }
                    catch (Exception ex)
                    {
                        //config file corrupted
                        isCorruptOrMissing = true;
                        MessageBox.Show("Parameter configuration file is corrupted. This may be due to an improper VTI application shutdown or power outage. Press OK to select a backup file.");
                        VtiEvent.Log.WriteError("Parameter configuration file is corrupted."
                            + Environment.NewLine + "Exception message: " + ex.Message);
                    }
                }
            }
            else
            {
                isCorruptOrMissing = true;
                MessageBox.Show("Parameter configuration file is missing. This may be due to an improper VTI application shutdown or power outage. Press OK to select a backup file.");
                VtiEvent.Log.WriteError("Parameter configuration file is missing.");
            }
            if (isCorruptOrMissing)
            {
                ConfigBackupSelect.ShowDialog();
            }

            //Is user.config missing or corrupt?
            try
            {
                var userConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                string filename = userConfig.FilePath;
                if (!File.Exists(userConfig.FilePath))
                {
                    VtiEvent.Log.WriteInfo("user.config file does not exist. New one will be created.");
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                string filename = ex.Filename;
                if (File.Exists(filename))
                {
                    VtiEvent.Log.WriteInfo("Cannot open existing user.config file. Recreating. Exception message: " + ex.Message);
                    File.Delete(filename);
                    Properties.Settings.Default.Upgrade();
                }
            }
        }

        /// <summary>
        /// Gets the static instance of the <see cref="VtiEventLog">VtiEventLog</see>
        /// </summary>
        public static VtiEventLog Log
        {
            get
            {
                return eventLog;
            }
        }

        ///// <summary>
        ///// Gets the static instance of the <see cref="EventLogViewerForm">EventLogViewerForm</see>
        ///// </summary>
        //public static EventLogViewerForm Viewer
        //{
        //  get
        //  {
        //    return eventLogViewer;
        //  }
        //}
    }
}