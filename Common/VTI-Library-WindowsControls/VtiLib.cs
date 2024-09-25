using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Classes.Data;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Interfaces;


namespace VTIWindowsControlLibrary
{
    /// <summary>
    /// Provides a bridge between the VTIWindowsControlLibrary and the client application.
    /// </summary>
    /// <remarks>
    /// Provides access within the VTIWindowsControlLibrary to the
    /// Machine, Config, and ManualCommands classes within the client application.
    /// Since nothing is known about these classes at compile time, all interaction
    /// with them must be done at run time through reflection.
    /// Provices access within the library to the IO configuration,
    /// the Localization Resource and other resources of the client application.
    /// </remarks>
    public class VtiLib
    {
        #region Fields (8)

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// ConnectionString for the VtiData database.
        /// </summary>
        internal static String ConnectionString;

        internal static String ConnectionString2;

        internal static String ConnectionString3;

        internal static User overrideUser;

        internal static bool UseRemoteModelDB;

        internal static string ModelDBSystemType;

        /// <summary>
        /// Provides access within the client application to a
        /// static instance of the VtiData class.
        /// </summary>
        public static VtiDataContext Data { get; private set; }

        public static VtiDataContext Data2 { get; private set; }

        public static Data.VtiDataContext2.VtiDataContext2 Data3 { get; private set; }

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// I/O configuration of the client application.
        /// </summary>
        //internal static IIOSettings IOSettings;
        internal static IIoConfig IO;

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// string localization resource file of the client application.
        /// </summary>
        internal static ResourceManager Localization;

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// ManualCommands class of the client application.
        /// </summary>
        internal static Type ManualCommandsType;

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// resources file of the client application.
        /// </summary>
        internal static ResourceManager Resources;

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// Machine class of the client application.
        /// </summary>
        internal static IMachine Machine;

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// Config class of the client application.
        /// </summary>
        internal static IConfig Config;

        /// <summary>
        /// Provides access within the VTIWindowsControlLibrary to the
        /// ManualCommands class of the client application.
        /// </summary>
        internal static IManualCommands ManualCommands;

        internal static Type ModelSettingsType;

        public static ResourceManager StandardMessages { get; private set; }

        internal static bool _MuteParamChangeLog = false;

        internal static bool _IsDualPortSystem = false;

        #endregion Fields

        #region Properties (1)

        /// <summary>
        /// Gets the assembly copyright.
        /// </summary>
        /// <value>The assembly copyright.</value>
        public static string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string DBConnectionString { get { return ConnectionString; } }
        public static string DBConnectionString2 { get { return ConnectionString2; } }

        public static bool MuteParamChangeLog { get { return _MuteParamChangeLog; } set { _MuteParamChangeLog = value; } }
        public static bool IsDualPortSystem { get { return _IsDualPortSystem; } }

        #endregion Properties

        #region Methods (1)

        static VtiLib()
        {
            if (Properties.Settings.Default.CallUpgrade)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.CallUpgrade = false;
                Properties.Settings.Default.Save();
            }

            StandardMessages = new ResourceManager(typeof(VtiStandardMessages));
        }

        /// <summary>
        /// Checks the elements in the config file to make sure all necessary tags are present. Ex. ProcessValue, DisplayName, Tooltip, etc.
        /// </summary>
        static void CheckConfigSchema()
        {
            string returnMsg = "";
            var _allUsersSettingsProvider = new Classes.Configuration.AllUsersSettingsProvider();
            //only do the check if the config file exists
            if (_allUsersSettingsProvider.ConfigFileExists())
            {
                string configFile = _allUsersSettingsProvider.GetSettingsPathAndFilename();
                XmlDocument doc = new XmlDocument();
                doc.Load(configFile);
                //only edit cycle parameter elements have the setting tag
                XmlNodeList editCycleParamList = doc.GetElementsByTagName("setting");
                for (int i = 0; i < editCycleParamList.Count; i++)
                {
                    var param = editCycleParamList[i].ChildNodes[0].ChildNodes[0];
                    string paramName = editCycleParamList[i].OuterXml.Substring(editCycleParamList[i].OuterXml.IndexOf("name=") + 6, editCycleParamList[i].OuterXml.IndexOf("serializeAs") - 17);
                    string paramType = param.Name;
                    XmlNodeList tags = param.ChildNodes;
                    List<string> tagNames = new List<string>();
                    foreach (XmlNode tag in tags)
                    {
                        tagNames.Add(tag.Name);
                    }
                    //every parameter, regardless of type, should have a DisplayName, ProcessValue, and Tooltip tag
                    //Boolean, String, and Enum parameters only have DisplayName, ProcessValue, and Tooltip tags
                    if (!tagNames.Contains("DisplayName"))
                    {
                        returnMsg += paramName + " is missing DisplayName tag." + Environment.NewLine;
                    }
                    if (!tagNames.Contains("ProcessValue"))
                    {
                        returnMsg += paramName + " is missing ProcessValue tag." + Environment.NewLine;
                    }
                    if (!tagNames.Contains("ToolTip"))
                    {
                        returnMsg += paramName + " is missing ToolTip tag." + Environment.NewLine;
                    }

                    if (paramType == "NumericParameter" || paramType == "TimeDelayParameter")
                    {
                        //must contain MinValue, MaxValue, LargeStep, SmallStep, and Units tags
                        if (!tagNames.Contains("MinValue"))
                        {
                            returnMsg += paramName + " is missing MinValue tag." + Environment.NewLine;
                        }
                        if (!tagNames.Contains("MaxValue"))
                        {
                            returnMsg += paramName + " is missing MaxValue tag." + Environment.NewLine;
                        }
                        if (!tagNames.Contains("LargeStep"))
                        {
                            returnMsg += paramName + " is missing LargeStep tag." + Environment.NewLine;
                        }
                        if (!tagNames.Contains("SmallStep"))
                        {
                            returnMsg += paramName + " is missing SmallStep tag." + Environment.NewLine;
                        }
                        if (!tagNames.Contains("Units"))
                        {
                            returnMsg += paramName + " is missing Units tag." + Environment.NewLine;
                        }
                    }
                    else if (paramType == "SerialPortParameter")
                    {
                        var PVtags = param.ChildNodes[1];
                        List<string> PVtagNames = new List<string>();
                        foreach (XmlNode tag in PVtags)
                        {
                            PVtagNames.Add(tag.Name);
                        }
                        if (!PVtagNames.Contains("PortName"))
                        {
                            returnMsg += paramName + " is missing PortName tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                        if (!PVtagNames.Contains("BaudRate"))
                        {
                            returnMsg += paramName + " is missing BaudRate tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                        if (!PVtagNames.Contains("Parity"))
                        {
                            returnMsg += paramName + " is missing Parity tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                        if (!PVtagNames.Contains("DataBits"))
                        {
                            returnMsg += paramName + " is missing DataBits tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                        if (!PVtagNames.Contains("StopBits"))
                        {
                            returnMsg += paramName + " is missing StopBits tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                        if (!PVtagNames.Contains("Handshake"))
                        {
                            returnMsg += paramName + " is missing Handshake tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                    }
                    else if (paramType == "EthernetPortParameter")
                    {
                        var PVtags = param.ChildNodes[1];
                        List<string> PVtagNames = new List<string>();
                        foreach (XmlNode tag in PVtags)
                        {
                            PVtagNames.Add(tag.Name);
                        }
                        if (!PVtagNames.Contains("PortName"))
                        {
                            returnMsg += paramName + " is missing PortName tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                        if (!PVtagNames.Contains("Port"))
                        {
                            returnMsg += paramName + " is missing Port tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                        if (!PVtagNames.Contains("IPAddress"))
                        {
                            returnMsg += paramName + " is missing IPAddress tag inside of ProcessValue tag." + Environment.NewLine;
                        }
                    }
                }
            }
            else
            {
                //Classes.Configuration.EditCycleApplicationSettingsBase editCycleApplicationSettingsBase = new Classes.Configuration.EditCycleApplicationSettingsBase();
                //var t = editCycleApplicationSettingsBase.GetType();
                //foreach (FieldInfo field in t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                //{
                //    if (field.FieldType == typeof(Classes.Configuration.EditCycleApplicationSettingsBase))
                //    {
                //        // Get the value of the CycleStep
                //        editCycleApplicationSettingsBase = field.GetValue(editCycleApplicationSettingsBase) as Classes.Configuration.EditCycleApplicationSettingsBase;
                //    }
                //}

                //_allUsersSettingsProvider.Upgrade();
                //Classes.Configuration.EditCycleApplicationSettingsBase
                //VtiLib.Config._Save();
            }

            if (returnMsg != "")
            {
                MessageBox.Show(returnMsg, "Config File XML Schema Warning");
            }
        }

        /// <summary>
        /// Inserts new models and model parameters from the local dbo.Models and dbo.ModelParameters tables into those in the remote database.
        /// If a model exists in the remote database that is not in the local database, the remote model will not be deleted.
        /// If a model with the same name exists in both the local and remote databases, a message box will appear with the LastUpdated and LastUpdatedBy VALUES, allowing the user to decide whether to update the model in the remote database or not.
        /// </summary>
        public static void PushLocalModelsToRemoteDatabase(string SystemType)
        {
            if (VtiLib.ConnectionString2 != "")
            {
                string sqlCmd = "";
                string connString = "";

                // get list of models from local database
                // reusing ModelParameterToUpdate class to hold data
                List<ModelParameterToUpdate> modelDataFromLocalDB = new List<ModelParameterToUpdate>();
                try
                {
                    connString = VtiLib.ConnectionString;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT ModelNo, LastModifiedBy, LastModified FROM dbo.Models;",
                    })
                    {
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ModelParameterToUpdate model = new ModelParameterToUpdate();
                            model.ModelNo = reader.GetString(0).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            if (reader.IsDBNull(1))
                            {
                                model.LastModifiedBy = null;
                            }
                            else
                            {
                                model.LastModifiedBy = reader.GetString(1).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            if (reader.IsDBNull(2))
                            {
                                model.LastModified = null;
                            }
                            else
                            {
                                model.LastModified = reader.GetDateTime(2);
                            }
                            modelDataFromLocalDB.Add(model);
                        }
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error retrieving models from local database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error retrieving models from local database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    return;
                }

                // get list of model names from remote database
                // get list of models from local database
                // reusing ModelParameterToUpdate class to hold data
                List<ModelParameterToUpdate> modelDataFromRemoteDB = new List<ModelParameterToUpdate>();
                try
                {
                    connString = VtiLib.ConnectionString2;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT ModelNo, LastModifiedBy, LastModified FROM dbo.Models WHERE SystemType = @SystemType;",
                    })
                    {
                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ModelParameterToUpdate model = new ModelParameterToUpdate();
                            model.ModelNo = reader.GetString(0).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            if (reader.IsDBNull(1))
                            {
                                model.LastModifiedBy = null;
                            }
                            else
                            {
                                model.LastModifiedBy = reader.GetString(1).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            if (reader.IsDBNull(2))
                            {
                                model.LastModified = null;
                            }
                            else
                            {
                                model.LastModified = reader.GetDateTime(2);
                            }
                            modelDataFromRemoteDB.Add(model);
                        }
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error retrieving models from remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error retrieving models from remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    return;
                }

                // iterate through the local models
                foreach (ModelParameterToUpdate model in modelDataFromLocalDB)
                {
                    // If local model does not exist in remote database, insert it into remote dbo.Models
                    if (!modelDataFromRemoteDB.Select(x => x.ModelNo).Contains(model.ModelNo))
                    {
                        try
                        {
                            connString = VtiLib.ConnectionString2;
                            using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                            using (SqlCommand cmd = new SqlCommand
                            {
                                Connection = sqlConnection1,
                                CommandType = CommandType.Text,
                                CommandText = "INSERT INTO dbo.Models " +
                                "(ModelNo, LastModifiedBy, LastModified, SystemType) " +
                                "VALUES (@ModelNo, @LastModifiedBy, @LastModified, @SystemType);"
                            })
                            {
                                if (string.IsNullOrEmpty(model.ModelNo))
                                    cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                                else
                                    cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;

                                if (string.IsNullOrEmpty(model.LastModifiedBy))
                                    cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = DBNull.Value;
                                else
                                    cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = model.LastModifiedBy;

                                cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = model.LastModified ?? DateTime.Now;
                                cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;
                                sqlCmd = cmd.CommandText;
                                sqlConnection1.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ee)
                        {
                            VtiEvent.Log.WriteError("Error inserting model into remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                                Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                            MessageBox.Show("Error inserting model into remote table dbo.Models. Connection string is " + connString + ". Command is " +
                                Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                            return;
                        }

                        // Get the local model's parameters and then insert them into remote dbo.ModelParameters
                        List<ModelParameterToUpdate> ListOfParamsForLocalModel = new List<ModelParameterToUpdate>();
                        try
                        {
                            connString = VtiLib.ConnectionString;
                            using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                            using (SqlCommand cmd = new SqlCommand
                            {
                                Connection = sqlConnection1,
                                CommandType = CommandType.Text,
                                CommandText = "SELECT ParameterID, ProcessValue, LastModifiedBy, LastModified FROM dbo.ModelParameters WHERE ModelNo = @ModelNo;"
                            })
                            {
                                cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;
                                sqlCmd = cmd.CommandText;
                                sqlConnection1.Open();
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    ModelParameterToUpdate mp = new ModelParameterToUpdate();

                                    if (reader.IsDBNull(0))
                                    {
                                        mp.ParameterID = null;
                                    }
                                    else
                                    {
                                        mp.ParameterID = reader.GetString(0).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                                    }
                                    if (reader.IsDBNull(1))
                                    {
                                        mp.ProcessValue = null;
                                    }
                                    else
                                    {
                                        mp.ProcessValue = reader.GetString(1).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                                    }
                                    if (reader.IsDBNull(2))
                                    {
                                        mp.LastModifiedBy = null;
                                    }
                                    else
                                    {
                                        mp.LastModifiedBy = reader.GetString(2).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                                    }
                                    if (reader.IsDBNull(3))
                                    {
                                        mp.LastModified = null;
                                    }
                                    else
                                    {
                                        mp.LastModified = reader.GetDateTime(3);
                                    }
                                    ListOfParamsForLocalModel.Add(mp);
                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            VtiEvent.Log.WriteError("Error retrieving model parameters from local database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                            MessageBox.Show("Error retrieving model parameters from local database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                            return;
                        }
                        foreach (ModelParameterToUpdate mp in ListOfParamsForLocalModel)
                        {
                            try
                            {
                                connString = VtiLib.ConnectionString2;
                                using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                                using (SqlCommand cmd = new SqlCommand
                                {
                                    Connection = sqlConnection1,
                                    CommandType = CommandType.Text,
                                    CommandText = "INSERT INTO dbo.ModelParameters " +
                                    "(ModelNo, ParameterID, ProcessValue, LastModifiedBy, LastModified, SystemType) " +
                                    "VALUES (@ModelNo, @ParameterID, @ProcessValue, @LastModifiedBy, @LastModified, @SystemType);",
                                })
                                {
                                    if (string.IsNullOrEmpty(model.ModelNo))
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;

                                    if (string.IsNullOrEmpty(mp.ParameterID))
                                        cmd.Parameters.Add("@ParameterID", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@ParameterID", SqlDbType.NVarChar).Value = mp.ParameterID;

                                    if (string.IsNullOrEmpty(mp.ProcessValue))
                                        cmd.Parameters.Add("@ProcessValue", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@ProcessValue", SqlDbType.NVarChar).Value = mp.ProcessValue;

                                    if (string.IsNullOrEmpty(mp.LastModifiedBy))
                                        cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = mp.LastModifiedBy;

                                    cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = mp.LastModified ?? DateTime.Now;
                                    cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;

                                    sqlCmd = cmd.CommandText;
                                    sqlConnection1.Open();
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ee)
                            {
                                VtiEvent.Log.WriteError("Error inserting model parameter into remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                    Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                MessageBox.Show("Error inserting model parameter into remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                    Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                return;
                            }
                        }
                    }
                    else
                    {
                        // if this local model also exist in the remote database, prompt the user with a message box, asking whether to update the remote model or not
                        if (MessageBox.Show($"Local model '{model.ModelNo}' (Last Modified by '{model.LastModifiedBy}' on {model.LastModified}) already exists in the remote database (Last Modified by '{modelDataFromRemoteDB.Where(x => x.ModelNo == model.ModelNo).First().LastModifiedBy}' on {modelDataFromRemoteDB.Where(x => x.ModelNo == model.ModelNo).First().LastModified})."
                            + Environment.NewLine + $"Update remote model data for '{model.ModelNo}'?", "MODEL ALREADY EXISTS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            // Update the LastModified and LastModifiedBy fields for this model in remote dbo.Models
                            try
                            {
                                connString = VtiLib.ConnectionString2;
                                using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                                using (SqlCommand cmd = new SqlCommand
                                {
                                    Connection = sqlConnection1,
                                    CommandType = CommandType.Text,
                                    CommandText = "UPDATE dbo.Models SET LastModifiedBy = @LastModifiedBy', LastModified = @LastModified WHERE ModelNo = @ModelNo AND SystemType = @SystemType;",
                                })
                                {
                                    if (string.IsNullOrEmpty(model.ModelNo))
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;

                                    if (string.IsNullOrEmpty(model.LastModifiedBy))
                                        cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = model.LastModifiedBy;

                                    cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = model.LastModified ?? DateTime.Now;
                                    cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;

                                    sqlCmd = cmd.CommandText;
                                    sqlConnection1.Open();
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ee)
                            {
                                VtiEvent.Log.WriteError($"Error updating model '{model.ModelNo}' on remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                                    Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                MessageBox.Show($"Error updating model '{model.ModelNo}' on remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                                    Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                return;
                            }
                            // Delete all remote model parameters for this model
                            try
                            {
                                connString = VtiLib.ConnectionString2;
                                using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                                using (SqlCommand cmd = new SqlCommand
                                {
                                    Connection = sqlConnection1,
                                    CommandType = CommandType.Text,
                                    CommandText = "DELETE FROM dbo.ModelParameters WHERE ModelNo = @ModelNo AND SystemType = @SystemType;",
                                })
                                {
                                    if (string.IsNullOrEmpty(model.ModelNo))
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;
                                    cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;
                                    sqlCmd = cmd.CommandText;
                                    sqlConnection1.Open();
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ee)
                            {
                                string msg = $"Error deleting model parameters from remote table dbo.ModelParameters for model '{model.ModelNo}'. Connection string is " + connString + ". Command is " +
                                    Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message;
                                VtiEvent.Log.WriteError(msg);
                                MessageBox.Show(msg);
                                return;
                            }
                            // Get local model parameters for this model then insert them into remote dbo.ModelParameters
                            List<ModelParameterToUpdate> ListOfParamsForLocalModel = new List<ModelParameterToUpdate>();
                            try
                            {
                                connString = VtiLib.ConnectionString;
                                using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                                using (SqlCommand cmd = new SqlCommand
                                {
                                    Connection = sqlConnection1,
                                    CommandType = CommandType.Text,
                                    CommandText = "SELECT ParameterID, ProcessValue, LastModifiedBy, LastModified FROM dbo.ModelParameters WHERE ModelNo = @ModelNo;"
                                })
                                {
                                    if (string.IsNullOrEmpty(model.ModelNo))
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                                    else
                                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;
                                    sqlCmd = cmd.CommandText;
                                    sqlConnection1.Open();
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        ModelParameterToUpdate mp = new ModelParameterToUpdate();

                                        if (reader.IsDBNull(0))
                                        {
                                            mp.ParameterID = null;
                                        }
                                        else
                                        {
                                            mp.ParameterID = reader.GetString(0).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                                        }
                                        if (reader.IsDBNull(1))
                                        {
                                            mp.ProcessValue = null;
                                        }
                                        else
                                        {
                                            mp.ProcessValue = reader.GetString(1).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                                        }
                                        if (reader.IsDBNull(2))
                                        {
                                            mp.LastModifiedBy = null;
                                        }
                                        else
                                        {
                                            mp.LastModifiedBy = reader.GetString(2).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                                        }
                                        if (reader.IsDBNull(3))
                                        {
                                            mp.LastModified = null;
                                        }
                                        else
                                        {
                                            mp.LastModified = reader.GetDateTime(3);
                                        }
                                        ListOfParamsForLocalModel.Add(mp);
                                    }
                                }
                            }
                            catch (Exception ee)
                            {
                                VtiEvent.Log.WriteError("Error retrieving model parameters from local database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                    Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                MessageBox.Show("Error retrieving model parameters from local database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                    Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                return;
                            }
                            foreach (ModelParameterToUpdate mp in ListOfParamsForLocalModel)
                            {
                                try
                                {
                                    connString = VtiLib.ConnectionString2;
                                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                                    using (SqlCommand cmd = new SqlCommand
                                    {
                                        Connection = sqlConnection1,
                                        CommandType = CommandType.Text,
                                        CommandText = "INSERT INTO dbo.ModelParameters " +
                                        "(ModelNo, ParameterID, ProcessValue, LastModifiedBy, LastModified, SystemType) " +
                                        "VALUES (@ModelNo, @ParameterID, @ProcessValue, @LastModifiedBy, @LastModified, @SystemType);",
                                    })
                                    {
                                        if (string.IsNullOrEmpty(model.ModelNo))
                                            cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                                        else
                                            cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;

                                        if (string.IsNullOrEmpty(mp.ParameterID))
                                            cmd.Parameters.Add("@ParameterID", SqlDbType.NVarChar).Value = DBNull.Value;
                                        else
                                            cmd.Parameters.Add("@ParameterID", SqlDbType.NVarChar).Value = mp.ParameterID;

                                        if (string.IsNullOrEmpty(mp.ProcessValue))
                                            cmd.Parameters.Add("@ProcessValue", SqlDbType.NVarChar).Value = DBNull.Value;
                                        else
                                            cmd.Parameters.Add("@ProcessValue", SqlDbType.NVarChar).Value = mp.ProcessValue;

                                        if (string.IsNullOrEmpty(mp.LastModifiedBy))
                                            cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = DBNull.Value;
                                        else
                                            cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = mp.LastModifiedBy;

                                        cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = mp.LastModified ?? DateTime.Now;
                                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;
                                        sqlCmd = cmd.CommandText;
                                        sqlConnection1.Open();
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ee)
                                {
                                    VtiEvent.Log.WriteError("Error inserting model parameter into remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                    MessageBox.Show("Error inserting model parameter into remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                VtiEvent.Log.WriteError("Unable to connect to remote database. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                MessageBox.Show("Unable to connect to remote database. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                return;
            }
            MessageBox.Show("Successfully updated remote model data.");
        }


        /// <summary>
        /// Overwrites all data in the local VtiData dbo.Models and dbo.ModelParameters table with all data from the remote VtiData dbo.Models and dbo.ModelParameters table where the SystemType value in the row matches the SystemType provided to this method.
        /// </summary>
        /// <returns>
        /// True if the method executed with no errors, false otherwise.
        /// </returns>
        public static bool PullModelsFromRemoteDatabase(string SystemType)
        {
            if (VtiLib.ConnectionString2 != "")
            {
                // Delete all data in local dbo.Models and dbo.ModelParameters
                string sqlCmd = "";
                string connString = "";
                try
                {
                    connString = VtiLib.ConnectionString;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "DELETE FROM dbo.Models;",
                    })
                    {
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error clearing data from local table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error clearing data from local table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    return false;
                }
                try
                {
                    connString = VtiLib.ConnectionString;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "DELETE FROM dbo.ModelParameters;",
                    })
                    {
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error clearing data from local table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error clearing data from local table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    return false;
                }

                // Get all data from remote dbo.Models table where the SystemType matches, reusing ModelParameterToUpdate class to hold data
                List<ModelParameterToUpdate> modelDataFromRemoteDB = new List<ModelParameterToUpdate>();
                try
                {
                    connString = VtiLib.ConnectionString2;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT ModelNo, LastModifiedBy, LastModified, SystemType FROM dbo.Models WHERE SystemType = @SystemType;",
                    })
                    {
                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ModelParameterToUpdate model = new ModelParameterToUpdate();
                            model.ModelNo = reader.GetString(0).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            if (reader.IsDBNull(1))
                            {
                                model.LastModifiedBy = null;
                            }
                            else
                            {
                                model.LastModifiedBy = reader.GetString(1).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            if (reader.IsDBNull(2))
                            {
                                model.LastModified = null;
                            }
                            else
                            {
                                model.LastModified = reader.GetDateTime(2);
                            }
                            if (reader.IsDBNull(3))
                            {
                                model.SystemType = null;
                            }
                            else
                            {
                                model.SystemType = reader.GetString(3).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            modelDataFromRemoteDB.Add(model);
                        }
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error retrieving models from remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error retrieving models from remote database table dbo.Models. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    return false;
                }
                //Insert models into local dbo.Models table
                foreach (ModelParameterToUpdate model in modelDataFromRemoteDB)
                {
                    try
                    {
                        connString = VtiLib.ConnectionString;
                        using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                        using (SqlCommand cmd = new SqlCommand
                        {
                            Connection = sqlConnection1,
                            CommandType = CommandType.Text,
                            CommandText = "INSERT INTO dbo.Models " +
                            "(ModelNo, LastModifiedBy, LastModified) " +
                            "VALUES (@ModelNo, @LastModifiedBy, @LastModified);",
                        })
                        {
                            if (string.IsNullOrEmpty(model.ModelNo))
                                cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = model.ModelNo;

                            if (string.IsNullOrEmpty(model.LastModifiedBy))
                                cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = model.LastModifiedBy;

                            cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = model.LastModified ?? DateTime.Now;

                            sqlCmd = cmd.CommandText;
                            sqlConnection1.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ee)
                    {
                        VtiEvent.Log.WriteError("Error inserting model into local database table dbo.Models. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                        MessageBox.Show("Error inserting model into local database table dbo.Models. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                        return false;
                    }
                }
                // Get all data from remote dbo.ModelParameters table where the SystemType matches, reusing ModelParameterToUpdate class to hold data
                List<ModelParameterToUpdate> modelParameterDataFromRemoteDB = new List<ModelParameterToUpdate>();
                try
                {
                    connString = VtiLib.ConnectionString2;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT ModelNo, ParameterID, ProcessValue, LastModifiedBy, LastModified, SystemType FROM dbo.ModelParameters WHERE SystemType = @SystemType;",
                    })
                    {
                        cmd.Parameters.Add("@SystemType", SqlDbType.NVarChar).Value = SystemType;
                        sqlCmd = cmd.CommandText;
                        sqlConnection1.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ModelParameterToUpdate mp = new ModelParameterToUpdate();
                            if (reader.IsDBNull(0))
                            {
                                mp.ModelNo = null;
                            }
                            else
                            {
                                mp.ModelNo = reader.GetString(0).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            if (reader.IsDBNull(1))
                            {
                                mp.ParameterID = null;
                            }
                            else
                            {
                                mp.ParameterID = reader.GetString(1).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            if (reader.IsDBNull(2))
                            {
                                mp.ProcessValue = null;
                            }
                            else
                            {
                                mp.ProcessValue = reader.GetString(2).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            if (reader.IsDBNull(3))
                            {
                                mp.LastModifiedBy = null;
                            }
                            else
                            {
                                mp.LastModifiedBy = reader.GetString(3).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            if (reader.IsDBNull(4))
                            {
                                mp.LastModified = null;
                            }
                            else
                            {
                                mp.LastModified = reader.GetDateTime(4);
                            }
                            if (reader.IsDBNull(5))
                            {
                                mp.SystemType = null;
                            }
                            else
                            {
                                mp.SystemType = reader.GetString(5).ReplaceOneSingleQuoteWithTwoSingleQuotes();
                            }
                            // If ModelNo in remote dbo.ModelParameters does not exist in remote dbo.Models, do not add it
                            if (modelDataFromRemoteDB.Select(x => x.ModelNo).Contains(mp.ModelNo))
                            {
                                modelParameterDataFromRemoteDB.Add(mp);
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error retrieving model parameters from remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    MessageBox.Show("Error retrieving model parameters from remote database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                    return false;
                }

                //Insert model parameters into local dbo.ModelParameters table
                foreach (ModelParameterToUpdate mp in modelParameterDataFromRemoteDB)
                {
                    try
                    {
                        connString = VtiLib.ConnectionString;
                        using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                        using (SqlCommand cmd = new SqlCommand
                        {
                            Connection = sqlConnection1,
                            CommandType = CommandType.Text,
                            CommandText = "INSERT INTO dbo.ModelParameters " +
                            "(ModelNo, ParameterID, ProcessValue, LastModifiedBy, LastModified) " +
                            "VALUES (@ModelNo, @ParameterID, @ProcessValue, @LastModifiedBy, @LastModified);"
                        })
                        {
                            if (string.IsNullOrEmpty(mp.ModelNo))
                                cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = mp.ModelNo;

                            if (string.IsNullOrEmpty(mp.ParameterID))
                                cmd.Parameters.Add("@ParameterID", SqlDbType.NVarChar).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@ParameterID", SqlDbType.NVarChar).Value = mp.ParameterID;

                            if (string.IsNullOrEmpty(mp.ProcessValue))
                                cmd.Parameters.Add("@ProcessValue", SqlDbType.NVarChar).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@ProcessValue", SqlDbType.NVarChar).Value = mp.ProcessValue;

                            if (string.IsNullOrEmpty(mp.LastModifiedBy))
                                cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = DBNull.Value;
                            else
                                cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = mp.LastModifiedBy;

                            cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = mp.LastModified ?? DateTime.Now;

                            sqlCmd = cmd.CommandText;
                            sqlConnection1.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ee)
                    {
                        VtiEvent.Log.WriteError("Error inserting model parameter into local database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                        MessageBox.Show("Error inserting model parameter into local database table dbo.ModelParameters. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.Message);
                        return false;
                    }
                }
            }
            else
            {
                VtiEvent.Log.WriteError("Unable to update models from remote database. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                MessageBox.Show("Unable to update models from remote database. Remote database connection string is undefined. Check that the connection string is defined and the Common Mode parameter to enable the remote database is enabled and restart the application.");
                return false;
            }
            return true;
        }

        public static void CreateShortcuts()
        {
            try
            {
                WshShell shell = new WshShell();
                DirectoryInfo binDebugPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
                string iconPath = binDebugPath.Parent.Parent.FullName + "\\Resources\\Vtilogo.ico";
                string[] files = System.IO.Directory.GetFiles(binDebugPath.FullName, "*.exe");
                string exePath = files[0];
                string lnkFile = System.IO.Path.GetFileNameWithoutExtension(exePath) + ".lnk";
                string shortcutLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + lnkFile;
                string startupPath = "C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Startup";


                if (files.Length > 1) throw new Exception("Too many executables found in directory"); // give up if there are multiple executables in bin/debug

                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
                shortcut.IconLocation = iconPath;
                shortcut.TargetPath = exePath;                 // The path of the file that will launch when the shortcut is run
                shortcut.WorkingDirectory = binDebugPath.FullName;
                shortcut.Save();                               // Save the shortcut


                string args = "/C copy \"" + shortcutLocation + "\" " + "\"" + startupPath + "\"";
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.Arguments = args;
                info.UseShellExecute = true;
                info.Verb = "runas";
                Process.Start(info);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create shortcuts: \n\n" + ex.Message);
            }

        }

        /// <summary>
        /// Initializes the VTI Windows Control Library, allowing the library access to certain
        /// classes within the client application.
        /// </summary>
        /// <typeparam name="TMachine">Type of the Machine class of the client application.</typeparam>
        /// <typeparam name="TConfig">Type of the Config class of the client application.</typeparam>
        /// <typeparam name="TManualCommands">Type of the Manual Commands class of the client application.</typeparam>
        /// <typeparam name="TModelSettings">Type of the Model Settings of the client application.</typeparam>
        /// <typeparam name="TIOSettings">Type of the IO Settings of the client application.</typeparam>
        /// <param name="machine">Instance of the Machine class of the client application.</param>
        /// <param name="config">Instance of the Config class of the client application.</param>
        /// <param name="vtiDataConnectionString">ConnectionString for the VtiData database.</param>
        public static void Initialize<TMachine, TConfig, TManualCommands, TModelSettings, TIOSettings>
         (TMachine machine, TConfig config, string vtiDataConnectionString)
            where TMachine : class, IMachine
            where TConfig : class, IConfig
            where TManualCommands : IManualCommands
            where TModelSettings : ModelSettingsBase
            where TIOSettings : class, IIOSettings
        {

            // Gets value of DualPortSystem Setting, defaults to false if not found
            // Should eventually be added to IMachine so it can be accessed without Reflection
            try
            {
                Type[] settingsArray = Assembly.GetCallingAssembly().GetTypes().Where(t => t.FullName.Contains("Properties.Settings")).ToArray();
                if (settingsArray.Length > 0)
                {
                    var b = settingsArray[0].GetProperty("Default").GetValue(null, null);
                    var c = b.GetType().GetProperties().Where(p => p.Name == "DualPortSystem").ToArray();
                    if (c.Length > 0)
                    {
                        _IsDualPortSystem = (bool)c[0].GetValue(b, null);
                    }
                    else
                    {
                        throw new Exception("DualPortSystem not found in client side Properties.Settings");
                    }
                }
                else
                {
                    throw new Exception("Properties.Settings not found in client code");
                }
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteInfo("ERROR: " + ex.Message);
                _IsDualPortSystem = false;
            }

            Properties.Settings.Default.Reload();
            VtiEvent.Log.WriteVerbose("Initializing VTI Control Library...");

            SplashScreen.Message = "Initializing Machine...";
            Machine = machine;

            SplashScreen.Message = "Initializing Config...";
            Config = config;
            // save the config to save any new Edit Cycle parameters that were added to the existing config file
            Config._Save();

            SplashScreen.Message = "Checking Config Schema...";
            CheckConfigSchema();

            ModelSettingsType = typeof(TModelSettings);

            SplashScreen.Message = "Initializing Manual Commands...";
            ManualCommands = Machine.ManualCommandsInstance;

            SplashScreen.Message = "Initializing IO...";
            IO = Config.IOInstance.Interface;

            SplashScreen.Message = "Initializing Resource Manager...";
            Localization = Machine.LocalizationInstance;

            SplashScreen.Message = "Initializing Database...";
            if (Properties.Settings.Default.UsesVtiDataDatabase)
            {
                ConnectionString = vtiDataConnectionString;
            }
            else
            {
                ConnectionString = "";
            }
            ConnectionString2 = "";

            Data = new VtiDataContext(vtiDataConnectionString);
            Data.VerifyLocalVtiDataConfiguration();

            //If this is a new VtiData database, grant all permissions to GROUP09
            if (Properties.Settings.Default.UsesVtiDataDatabase)
            {
                string query = "SELECT COUNT(*) from dbo.UutRecords;";
                int numRecords = -1;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandType = CommandType.Text,
                    CommandText = query,
                })
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            numRecords = reader.GetInt32(0);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during datbase query. Connection string: {ConnectionString}. Query string: {query}. Exception message: {ex.Message}");
                        return;
                    }
                }
                if (numRecords == 0)
                {
                    Classes.ClientForms.Permissions.GrantAllCommandsToGroup(9);
                }
                else
                {
                    DeleteOldestRecords(numRecords);
                }
                //Data.UpdateSchema();
            }
            SplashScreen.Message = "";
            UseRemoteModelDB = false;
        }

        /// <summary>
        /// Initializes the VTI Windows Control Library, allowing the library access to certain
        /// classes within the client application.
        /// </summary>
        /// <typeparam name="TMachine">Type of the Machine class of the client application.</typeparam>
        /// <typeparam name="TConfig">Type of the Config class of the client application.</typeparam>
        /// <typeparam name="TManualCommands">Type of the Manual Commands class of the client application.</typeparam>
        /// <typeparam name="TModelSettings">Type of the Model Settings of the client application.</typeparam>
        /// <typeparam name="TIOSettings">Type of the IO Settings of the client application.</typeparam>
        /// <param name="machine">Instance of the Machine class of the client application.</param>
        /// <param name="config">Instance of the Config class of the client application.</param>
        /// <param name="vtiDataConnectionString">ConnectionString for the VtiData database.</param>
        /// <param name="vtiDataConnectionString2">ConnectionString for the remote VtiData database.</param>
        /// <param name="systemType">SystemType column value in remote database</param>
        public static void Initialize<TMachine, TConfig, TManualCommands, TModelSettings, TIOSettings>
            (TMachine machine, TConfig config, string vtiDataConnectionString, string vtiDataConnectionString2, string systemType)
          where TMachine : class, IMachine
          where TConfig : class, IConfig
          where TManualCommands : IManualCommands
          where TModelSettings : ModelSettingsBase
            where TIOSettings : class, IIOSettings
        {
            bool connStringBlank = String.IsNullOrWhiteSpace(vtiDataConnectionString2);
            if (!connStringBlank)
            {
                Data3 = new Data.VtiDataContext2.VtiDataContext2(vtiDataConnectionString2);
                ConnectionString3 = vtiDataConnectionString2;
            }

            ModelDBSystemType = systemType;

            VtiLib.Initialize<TMachine, TConfig, TManualCommands, TModelSettings, TIOSettings>(machine, config, vtiDataConnectionString);
            UseRemoteModelDB = !connStringBlank;
        }

        /// <summary>
        /// Initializes the VTI Windows Control Library, allowing the library access to certain
        /// classes within the client application.
        /// </summary>
        /// <typeparam name="TMachine">Type of the Machine class of the client application.</typeparam>
        /// <typeparam name="TConfig">Type of the Config class of the client application.</typeparam>
        /// <typeparam name="TManualCommands">Type of the Manual Commands class of the client application.</typeparam>
        /// <typeparam name="TModelSettings">Type of the Model Settings of the client application.</typeparam>
        /// <typeparam name="TIOSettings">Type of the IO Settings of the client application.</typeparam>
        /// <param name="machine">Instance of the Machine class of the client application.</param>
        /// <param name="config">Instance of the Config class of the client application.</param>
        /// <param name="vtiDataConnectionString">ConnectionString for the VtiData database.</param>
        /// <param name="vtiDataConnectionString2">ConnectionString for the remote VtiData database.</param>
        public static void Initialize<TMachine, TConfig, TManualCommands, TModelSettings, TIOSettings>
            (TMachine machine, TConfig config, string vtiDataConnectionString, string vtiDataConnectionString2)
          where TMachine : class, IMachine
          where TConfig : class, IConfig
          where TManualCommands : IManualCommands
          where TModelSettings : ModelSettingsBase
          where TIOSettings : class, IIOSettings
        {
            // Gets value of DualPortSystem Setting, defaults to false if not found
            // Should eventually be added to IMachine so it can be accessed without Reflection
            try
            {
                Type[] settingsArray = Assembly.GetCallingAssembly().GetTypes().Where(t => t.FullName.Contains("Properties.Settings")).ToArray();
                if (settingsArray.Length > 0)
                {
                    var b = settingsArray[0].GetProperty("Default").GetValue(null, null);
                    var c = b.GetType().GetProperties().Where(p => p.Name == "DualPortSystem").ToArray();
                    if (c.Length > 0)
                    {
                        _IsDualPortSystem = (bool)c[0].GetValue(b, null);
                    }
                    else
                    {
                        throw new Exception("DualPortSystem not found in client side Properties.Settings");
                    }
                }
                else
                {
                    throw new Exception("Properties.Settings not found in client code");
                }
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteInfo("ERROR: " + ex.Message);
                _IsDualPortSystem = false;
            }

            Properties.Settings.Default.Reload();
            VtiEvent.Log.WriteVerbose("Initializing VTI Control Library...");

            SplashScreen.Message = "Initializing Machine...";
            Machine = machine;

            SplashScreen.Message = "Initializing Config...";
            Config = config;
            // save the config to save any new Edit Cycle parameters that were added to the existing config file
            Config._Save();

            SplashScreen.Message = "Checking Config Schema...";
            CheckConfigSchema();

            ModelSettingsType = typeof(TModelSettings);

            SplashScreen.Message = "Initializing Manual Commands...";
            ManualCommands = Machine.ManualCommandsInstance;

            SplashScreen.Message = "Initializing IO...";
            IO = Config.IOInstance.Interface;

            SplashScreen.Message = "Initializing Resource Manager...";
            Localization = Machine.LocalizationInstance;

            SplashScreen.Message = "Initializing Database...";
            if (Properties.Settings.Default.UsesVtiDataDatabase)
            {
                ConnectionString = vtiDataConnectionString;
                ConnectionString2 = vtiDataConnectionString2;
            }
            else
            {
                ConnectionString = ConnectionString2 = "";
            }

            Data = new VtiDataContext(vtiDataConnectionString);
            Data2 = new VtiDataContext(vtiDataConnectionString2);
            Data.VerifyLocalVtiDataConfiguration();

            //If this is a new VtiData database, grant all permissions to GROUP09
            string query = "SELECT COUNT(*) from dbo.UutRecords;";
            int numRecords = -1;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = query,
            })
            {
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        numRecords = reader.GetInt32(0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("Error during datbase query. Query string: " + query + ". Exception message: " + ex.Message);
                    return;
                }
            }
            if (numRecords == 0)
            {
                Classes.ClientForms.Permissions.GrantAllCommandsToGroup(9);
            }
            else
            {
                DeleteOldestRecords(numRecords);
            }

            //Data.UpdateSchema();
            SplashScreen.Message = "";
            UseRemoteModelDB = false;

        }

        /// <summary>
        /// Delete oldest X% records if VtiData database file size is greater than Y GB
        /// </summary>
        /// <param name="numRecords"></param>
        private static void DeleteOldestRecords(int numRecords)
        {
            string connStrWithAdminUser = ConnectionString.ToLower().Replace("= ", "=").Replace(" =", "=").Replace("id=vtiuser", "id=vtisa").Replace("vtiuser", @"V@cuum3342");
            int maxFileSizeInGB = 2;
            string databaseName = "VtiData";
            string fileName = "";
            double sizeGB = 0;
            string query = "";
            //SQL SELECT template to copy
            try
            {
                using (SqlConnection conn = new SqlConnection(connStrWithAdminUser))
                {
                    // Get file size of VtiData database
                    query = $"SELECT name AS FileName, size * 8.0 / 1024 / 1024 AS SizeGB FROM sys.master_files WHERE type_desc = 'ROWS' AND database_id = DB_ID(@DatabaseName)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.Add("@DatabaseName", SqlDbType.NVarChar).Value = databaseName;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        fileName = reader["FileName"].ToString();
                        sizeGB = Convert.ToDouble(reader["SizeGB"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during datbase query. Connection string: {ConnectionString}. Query string: {query}. Exception message: {ex.Message}");
            }
            if (sizeGB > maxFileSizeInGB)
            {
                double percentOfRecordsToDelete = 10;
                if (MessageBox.Show($"Local VtiData database file size on this PC is larger than {maxFileSizeInGB} GB. Do you want to delete the oldest {percentOfRecordsToDelete}% of test records?", "Delete records from database?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int numRecordsToDelete = (int)((percentOfRecordsToDelete / 100.0) * numRecords);
                    try
                    {
                        // create temporary table with oldest UutRecord IDs, then delete the corresponding UutRecordDetails, then delete the UutRecords
                        using (SqlConnection conn = new SqlConnection(connStrWithAdminUser))
                        {
                            query = "CREATE TABLE #OldestRecordIDs (ID BIGINT); " +
                                $"INSERT INTO #OldestRecordIDs (ID) SELECT TOP {numRecordsToDelete} ID FROM dbo.UutRecords ORDER BY ID; " +
                                "DELETE FROM dbo.UutRecordDetails WHERE UutRecordID IN (SELECT ID FROM #OldestRecordIDs); " +
                                "DELETE FROM dbo.UutRecords WHERE ID IN (SELECT ID FROM #OldestRecordIDs); " +
                                "DROP TABLE #OldestRecordIDs;";
                            conn.Open();
                            using (SqlTransaction transaction = conn.BeginTransaction())
                            {
                                try
                                {
                                    SqlCommand cmd = new SqlCommand(query, conn, transaction);
                                    int rowsDeleted = cmd.ExecuteNonQuery();
                                    // Commit the transaction if both deletions succeed
                                    transaction.Commit();
                                }
                                catch (Exception ee)
                                {
                                    // Rollback the transaction if an error occurs
                                    transaction.Rollback();
                                    string error = ee.Message;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during datbase query. Connection string: {ConnectionString}. Query string: {query}. Exception message: {ex.Message}");
                    }

                    // also delete oldest records from dbo.ManualCmdExecLog
                    try
                    {
                        string tableName = "dbo.ManualCmdExecLog";
                        using (SqlConnection conn = new SqlConnection(ConnectionString))
                        {
                            conn.Open();
                            using (SqlTransaction transaction = conn.BeginTransaction())
                            {
                                try
                                {
                                    query = @"
                                        DECLARE @TotalRows INT;
                                        DECLARE @RowsToDelete INT;
                                        DECLARE @Percentage FLOAT;

                                        -- Calculate the total number of rows in the table
                                        SELECT @TotalRows = COUNT(*)
                                        FROM " + tableName + @";

                                        -- Set the percentage to delete
                                        SET @Percentage = @PercentageParam;

                                        -- Calculate the number of rows to delete based on the percentage
                                        SET @RowsToDelete = CEILING(@TotalRows * @Percentage / 100.0);

                                        -- Delete the oldest rows based on the calculated number
                                        WITH OldestRecords AS (
                                            SELECT TOP (@RowsToDelete) ID
                                            FROM " + tableName + @"
                                            ORDER BY ID
                                        )
                                        DELETE FROM " + tableName + @"
                                        WHERE ID IN (SELECT ID FROM OldestRecords);";

                                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                                    {
                                        // Add the percentage parameter
                                        cmd.Parameters.AddWithValue("@PercentageParam", percentOfRecordsToDelete);

                                        int rowsDeleted = cmd.ExecuteNonQuery();
                                    }
                                    // Commit the transaction if the deletion succeeds
                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    MessageBox.Show($"Error during datbase query. Connection string: {ConnectionString}. Query string: {query}. Exception message: {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during datbase query. Connection string: {ConnectionString}. Query string: {query}. Exception message: {ex.Message}");
                    }

                    // also delete oldest records from dbo.ParamChangeLog
                    try
                    {
                        string tableName = "dbo.ParamChangeLog";
                        using (SqlConnection conn = new SqlConnection(ConnectionString))
                        {
                            conn.Open();
                            using (SqlTransaction transaction = conn.BeginTransaction())
                            {
                                try
                                {
                                    query = @"
                                        DECLARE @TotalRows INT;
                                        DECLARE @RowsToDelete INT;
                                        DECLARE @Percentage FLOAT;

                                        -- Calculate the total number of rows in the table
                                        SELECT @TotalRows = COUNT(*)
                                        FROM " + tableName + @";

                                        -- Set the percentage to delete
                                        SET @Percentage = @PercentageParam;

                                        -- Calculate the number of rows to delete based on the percentage
                                        SET @RowsToDelete = CEILING(@TotalRows * @Percentage / 100.0);

                                        -- Delete the oldest rows based on the calculated number
                                        WITH OldestRecords AS (
                                            SELECT TOP (@RowsToDelete) ID
                                            FROM " + tableName + @"
                                            ORDER BY ID
                                        )
                                        DELETE FROM " + tableName + @"
                                        WHERE ID IN (SELECT ID FROM OldestRecords);";

                                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                                    {
                                        // Add the percentage parameter
                                        cmd.Parameters.AddWithValue("@PercentageParam", percentOfRecordsToDelete);

                                        int rowsDeleted = cmd.ExecuteNonQuery();
                                    }
                                    // Commit the transaction if the deletion succeeds
                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    MessageBox.Show($"Error during datbase query. Connection string: {ConnectionString}. Query string: {query}. Exception message: {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during datbase query. Connection string: {ConnectionString}. Query string: {query}. Exception message: {ex.Message}");
                    }
                }
            }
        }


        #endregion Methods
    }
}