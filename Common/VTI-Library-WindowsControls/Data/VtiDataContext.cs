using System;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Data;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VTIWindowsControlLibrary.Classes.Util;
using System.Reflection;
using VTIWindowsControlLibrary.Data.VtiDataContext2;

namespace VTIWindowsControlLibrary.Data
{
    partial class ParamChangeLog
    {
    }

    partial class ManualCmdExecLog
    {

    }
    partial class VtiDataContext
    {
        /// <summary>
        /// CheckConnStatus
        /// </summary>
        /// <returns>True if connection to database can be established.</returns>
        public Boolean CheckConnStatus()
        {
            try
            {
                if (VtiLib.Data.Connection.State != ConnectionState.Open)
                    VtiLib.Data.Connection.Open();
                return (VtiLib.Data.Connection.State == ConnectionState.Open);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean CheckConnStatus2()
        {
            try
            {
                if (VtiLib.Data2.Connection.State != ConnectionState.Open)
                    VtiLib.Data2.Connection.Open();
                return (VtiLib.Data2.Connection.State == ConnectionState.Open);
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Checks if the C:\VTI PC\Databases folder is accessible. 
        /// If it is, check if the VtiData.mdf and VtiData.ldf files are there. 
        /// If not, place a clean copy of the two files there from the control library's Data folder.
        /// </summary>
        /// <returns></returns>
        public void VerifyLocalVtiDataConfiguration()
        {
            if (Properties.Settings.Default.VerifyLocalVtiDataConfiguration && Properties.Settings.Default.UsesVtiDataDatabase)
            {
                bool VtiDataWasAttached = false;
                string errorMessage = "";
                string mdfSeedPath = ""; string ldfSeedPath = "";
                string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                int indexOfSecondBackSlash = assemblyLocation.IndexOf(@"\", assemblyLocation.IndexOf(@"\") + 1);
                string startSearchFolder = assemblyLocation.Substring(0, indexOfSecondBackSlash + 1);
                //if VtiData.mdf or VtiData_log.ldf does not exist in Databases folder, 
                //move a copy there from the library's Data folder
                DirectoryInfo databasesFolder = new DirectoryInfo(@"C:\VTI PC\Databases");
                if (!databasesFolder.Exists)
                {
                    Directory.CreateDirectory(databasesFolder.FullName);
                }
                FileInfo mdfTarget = new FileInfo(@"C:\VTI PC\Databases\VtiData.mdf");
                if (!mdfTarget.Exists)
                {
                    mdfSeedPath = Directory.GetFiles(startSearchFolder, "VtiData.mdf.SEED", SearchOption.AllDirectories)
                        .Where(x => x.ToLower().Contains(@"vtiwindowscontrollibrary\data")).FirstOrDefault();
                    FileInfo mdfFile = new FileInfo(mdfSeedPath);
                    try
                    {
                        mdfFile.CopyTo(mdfTarget.FullName);
                    }
                    catch (Exception ex)
                    {
                        VtiEvent.Log.WriteInfo("Error copying VtiData.mdf to Databases folder. To disable VtiData schema verification, set 'VerifyLocalVtiDataConfiguration' to false in VTIWindowsControlLibrary -> Properties -> Settings.settings." + Environment.NewLine + " Exception message: " + ex.Message);
                        MessageBox.Show("Error copying VtiData.mdf to Databases folder. To disable VtiData schema verification, set 'VerifyLocalVtiDataConfiguration' to false in VTIWindowsControlLibrary -> Properties -> Settings.settings." + Environment.NewLine + " Exception message: " + ex.Message);
                    }
                }
                FileInfo ldfTarget = new FileInfo(@"C:\VTI PC\Databases\VtiData_log.ldf");
                if (!ldfTarget.Exists)
                {
                    if (mdfSeedPath != "")
                    {
                        ldfSeedPath = mdfSeedPath.Replace("VtiData.mdf.SEED", "VtiData_log.ldf.SEED");
                    }
                    else
                    {
                        ldfSeedPath = Directory.GetFiles(startSearchFolder, "VtiData_log.ldf.SEED", SearchOption.AllDirectories)
                            .Where(x => x.ToLower().Contains(@"vtiwindowscontrollibrary\data")).FirstOrDefault();
                    }
                    FileInfo ldfFile = new FileInfo(ldfSeedPath);
                    try
                    {
                        ldfFile.CopyTo(ldfTarget.FullName);
                    }
                    catch (Exception ex)
                    {
                        VtiEvent.Log.WriteInfo("Error copying VtiData_log.ldf to Databases folder. To disable VtiData schema verification, set 'VerifyLocalVtiDataConfiguration' to false in VTIWindowsControlLibrary -> Properties -> Settings.settings." + Environment.NewLine + " Exception message: " + ex.Message);
                        MessageBox.Show("Error copying VtiData_log.ldf to Databases folder. To disable VtiData schema verification, set 'VerifyLocalVtiDataConfiguration' to false in VTIWindowsControlLibrary -> Properties -> Settings.settings." + Environment.NewLine + " Exception message: " + ex.Message);
                    }
                }
                using (SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True"))
                {
                    string VtiDataDBName = "VtiData";
                    //Get database names on local SQL Server
                    string query = "SELECT name FROM sys.databases WHERE name LIKE '%VtiData%'";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    try
                    {
                        con.Open();
                        da.Fill(dt);
                        //If VtiData is not attached, attach it
                        if (dt.Rows.Count == 0)
                        {
                            if (MessageBox.Show("There was no database with a name containing 'VtiData' found attached to the local SQL Server. Attach a copy of VtiData?"
                                + Environment.NewLine + Environment.NewLine + "To disable VtiData schema verification, set 'VerifyLocalVtiDataConfiguration' to false in VTIWindowsControlLibrary -> Properties -> Settings.settings.",
                                    "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                query = " CREATE DATABASE VtiData ON" +
                            @" (FILENAME = 'C:\VTI PC\Databases\VtiData.mdf'), " +
                            @" (FILENAME = 'C:\VTI PC\Databases\VtiData_log.ldf')" +
                            " FOR ATTACH";
                                SqlCommand cmd = new SqlCommand(query);
                                cmd.Connection = con;
                                cmd.ExecuteNonQuery();
                                VtiDataWasAttached = true;
                            }
                            else
                            {
                                con.Close();
                                return;
                            }
                        }
                        else
                        {
                            VtiDataDBName = dt.Rows[0].ItemArray[0].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "Error getting database names or attaching VtiData database on local SQL Server. To disable VtiData schema verification, set 'VerifyLocalVtiDataConfiguration' to false in VTIWindowsControlLibrary -> Properties -> Settings.settings." + Environment.NewLine + " Exception message: " + ex.Message;
                    }

                    if (errorMessage == "")
                    {
                        try
                        {
                            //Get table names in VtiData
                            query = "SELECT TABLE_NAME FROM " + VtiDataDBName + ".INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
                            da = new SqlDataAdapter(query, con);
                            dt = new DataTable();
                            da.Fill(dt);
                            //if dbo.ParamChangeLog does not exist, create it
                            if (dt.Select("TABLE_NAME LIKE '%ParamChangeLog%'").Count() == 0)
                            {
                                string scriptPath = Directory.GetFiles(startSearchFolder, "ParamChangeLogScript.sql", SearchOption.AllDirectories)
                                    .Where(x => x.ToLower().Contains(@"vtiwindowscontrollibrary\data")).FirstOrDefault();
                                query = File.ReadAllText(scriptPath);
                                // split script on GO command
                                IEnumerable<string> commandStrings = Regex.Split(query, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                foreach (string commandString in commandStrings)
                                {
                                    if (!string.IsNullOrEmpty(commandString.Trim()))
                                    {
                                        using (var command = new SqlCommand(commandString, con))
                                        {
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            //if dbo.ManualCmdExecLog does not exist, create it
                            if (dt.Select("TABLE_NAME LIKE '%ManualCmdExecLog%'").Count() == 0)
                            {
                                string scriptPath = Directory.GetFiles(startSearchFolder, "ManualCmdExecLogScript.sql", SearchOption.AllDirectories)
                                    .Where(x => x.ToLower().Contains(@"vtiwindowscontrollibrary\data")).FirstOrDefault();
                                query = File.ReadAllText(scriptPath);
                                // split script on GO command
                                IEnumerable<string> commandStrings = Regex.Split(query, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                foreach (string commandString in commandStrings)
                                {
                                    if (!string.IsNullOrEmpty(commandString.Trim()))
                                    {
                                        using (var command = new SqlCommand(commandString, con))
                                        {
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            //Grant all permissions to GROUP09
                            Classes.ClientForms.Permissions.GrantAllCommandsToGroup(9);
                        }
                        catch (Exception ex)
                        {
                            errorMessage = "Error getting VtiData table names or creating missing VtiData tables on local SQL Server. To disable VtiData schema verification, set 'VerifyLocalVtiDataConfiguration' to false in VTIWindowsControlLibrary -> Properties -> Settings.settings." + Environment.NewLine + " Exception message: " + ex.Message;
                        }
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    if (VtiDataWasAttached)
                    {
                        MessageBox.Show("VtiData database was attached to local SQL Server. Close software and verify connection string is: Data Source=.\\SQLEXPRESS;Initial Catalog=VtiData;Integrated Security=True");
                        VtiEvent.Log.WriteInfo("VtiData database was attached to local SQL Server. Close software and verify connection string is: Data Source=.\\SQLEXPRESS;Initial Catalog=VtiData;Integrated Security=True");
                    }
                    if (errorMessage != "")
                    {
                        DisplayErrorMessage(errorMessage);
                    }
                }
            }
        }

        public void DisplayErrorMessage(string message)
        {
            VtiEvent.Log.WriteError(message);
            MessageBox.Show(message);
        }

        /// <summary>
        /// CheckGroupCommand
        /// </summary>
        /// <param name="GroupID">Group</param>
        /// <param name="CommandID">Command</param>
        /// <returns>True if the group has permission to execute the command.</returns>
        public Boolean CheckGroupCommand(String GroupID, String CommandID)
        {
            return (VtiLib.Data.GroupCommands.Count(gc => gc.GroupID == GroupID && gc.CommandID == CommandID) > 0);
        }

        /// <summary>
        /// CheckCommand
        /// </summary>
        /// <param name="OpID">Operator ID</param>
        /// <param name="CommandID">Command</param>
        /// <returns>True if operator has permission to execute the command.</returns>
        public Boolean CheckCommand(String OpID, String CommandID)
        {
            if (VtiLib.Data.Users.Where(x => x.OpID == OpID).Count() == 0)
            {
                VtiEvent.Log.WriteWarning("The current user '" + OpID +
                               "' does not exist in the dbo.Users table and therefore does have permission to execute the command '" + CommandID + "'.",
                               VTIWindowsControlLibrary.Enums.VtiEventCatType.Manual_Command);
                return false;
            }
            User user = VtiLib.Data.Users.Single(u => u.OpID == OpID);
            if (user.GroupID == "GROUP10" || VtiLib.Data.GroupCommands.Count(gc => gc.GroupID == user.GroupID && gc.CommandID == CommandID) > 0)
            {
                LogManualCmdExecution(OpID, string.Empty, CommandID);
                return true;
            }
            else
            {
                String tempOpId;
                tempOpId = VtiLib.Data.OpIDfromPassword(VTIWindowsControlLibrary.Classes.ClientForms.KeyPad.Show("'" + OpID +
                               "' does not have permission to execute the command '" + CommandID + "'. Enter password with permission for this command!", true));
                if (tempOpId == string.Empty)
                {
                    return false;
                }
                else
                {
                    User user2 = VtiLib.Data.Users.Single(u => u.OpID == tempOpId);
                    if (user2.GroupID == "GROUP10" || VtiLib.Data.GroupCommands.Count(gc => gc.GroupID == user2.GroupID && gc.CommandID == CommandID) > 0)
                    {
                        VtiLib.overrideUser = user2;
                        LogManualCmdExecution(OpID, VtiLib.overrideUser.OpID, CommandID);
                        return true;
                    }
                    else
                    {
                        VtiEvent.Log.WriteWarning("The current user '" + OpID +
                               "' does not have permission to execute the command '" + CommandID + "'.",
                               VTIWindowsControlLibrary.Enums.VtiEventCatType.Manual_Command);
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// CheckCommand
        ///
        /// Checks if the current operator has permission to execute a command, and displays a warning if not.
        /// </summary>
        /// <param name="OpID">Operator ID</param>
        /// <param name="CommandID">Command</param>
        /// <param name="WarnIfNoPermission">Display a warning if permission not granted</param>
        /// <returns>True if operator has permission to execute the command.</returns>
        public Boolean CheckCommand(String OpID, String CommandID, Boolean WarnIfNoPermission)
        {
            if (string.IsNullOrEmpty(OpID))
            {
                MessageBox.Show(VtiLib.Localization.GetString("PleaseLogIn"), Application.ProductName);
                return false;
            }
            else
            {
                if (VtiLib.overrideUser != null || CheckCommand(OpID, CommandID))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(string.Format(VtiLibLocalization.DoesNotHavePermission, OpID, CommandID), Application.ProductName);
                    return false;
                }
            }
        }

        /// <summary>
        /// Log the manual command execution to the local or remote dbo.ManualCmdExecLog table
        /// </summary>
        public void LogManualCmdExecution(string OpID, string overrideOpID, string CommandID)
        {
            if (Properties.Settings.Default.UsesVtiDataDatabase)
            {
                string sqlCmd = "";
                string connString = "";
                string SystemID = "";
                //read SystemID from Config file
                var _allUsersSettingsProvider = new VTIWindowsControlLibrary.Classes.Configuration.AllUsersSettingsProvider();
                string configFile = _allUsersSettingsProvider.GetSettingsPathAndFilename();
                XmlDocument doc = new XmlDocument();
                doc.Load(configFile);
                //list of all DisplayName values in config file
                XmlNodeList editCycleParamList = doc.GetElementsByTagName("DisplayName");
                for (int i = 0; i < editCycleParamList.Count; i++)
                {
                    if (editCycleParamList[i].InnerXml.Equals("System ID", StringComparison.CurrentCultureIgnoreCase)
                     || editCycleParamList[i].InnerXml.Equals("System_ID", StringComparison.CurrentCultureIgnoreCase)
                     || editCycleParamList[i].InnerXml.Equals("SystemID", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SystemID = editCycleParamList[i].NextSibling.InnerXml;
                        break;
                    }
                }
                try
                {
                    connString = VtiLib.ConnectionString;
                    using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection1,
                        CommandType = CommandType.Text,
                        CommandText = "Insert into dbo.ManualCmdExecLog " +
                            "(DateTime, OpID, OverrideOpID, SystemID, ManualCommand) " +
                            "values (@DateTime, @OpID, @OverrideOpID, @SystemID, @ManualCommand)",
                    })
                    {
                        sqlCmd = cmd.CommandText;
                        cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@OpID", SqlDbType.NVarChar).Value = OpID;
                        cmd.Parameters.Add("@OverrideOpID", SqlDbType.NVarChar).Value = overrideOpID;
                        cmd.Parameters.Add("@SystemID", SqlDbType.NVarChar).Value = SystemID;
                        cmd.Parameters.Add("@ManualCommand", SqlDbType.NVarChar).Value = CommandID;
                        sqlConnection1.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ee)
                {
                    VtiEvent.Log.WriteError("Error inserting parameter change record into local dbo.ManualCmdExecLog. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                    MessageBox.Show("Error inserting parameter change record into local dbo.ManualCmdExecLog. Connection string is " + connString + ". Command is " +
                        Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                }
                if (VtiLib.ConnectionString2 != "")
                {

                    sqlCmd = "";
                    connString = "";
                    try
                    {
                        connString = VtiLib.ConnectionString2;
                        using (SqlConnection sqlConnection1 = new SqlConnection(connString))
                        using (SqlCommand cmd = new SqlCommand
                        {
                            Connection = sqlConnection1,
                            CommandType = CommandType.Text,
                            CommandText = "Insert into dbo.ManualCmdExecLog " +
                                "(DateTime, OpID, OverrideOpID, SystemID, ManualCommand) " +
                                "values (@DateTime, @OpID, @OverrideOpID, @SystemID, @ManualCommand)",
                        })
                        {
                            sqlCmd = cmd.CommandText;
                            cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@OpID", SqlDbType.NVarChar).Value = OpID;
                            cmd.Parameters.Add("@OverrideOpID", SqlDbType.NVarChar).Value = overrideOpID;
                            cmd.Parameters.Add("@SystemID", SqlDbType.NVarChar).Value = SystemID;
                            cmd.Parameters.Add("@ManualCommand", SqlDbType.NVarChar).Value = CommandID;
                            sqlConnection1.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ee)
                    {
                        VtiEvent.Log.WriteError("Error inserting parameter change record into remote dbo.ManualCmdExecLog. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                        MessageBox.Show("Error inserting parameter change record into remote dbo.ManualCmdExecLog. Connection string is " + connString + ". Command is " +
                            Environment.NewLine + sqlCmd + Environment.NewLine + "Exception: " + ee.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// GroupIDFromOpID
        /// </summary>
        /// <param name="OpID">OpID</param>
        /// <returns>Group ID if valid OpID, or empty string if invalid</returns>
        public String GroupIDFromOpID(String OpID)
        {
            User user = VtiLib.Data.Users.Single(u => u.OpID == OpID);
            return user.GroupID;
        }

        /// <summary>
        /// OpIDfromPassword
        /// </summary>
        /// <param name="Password">Password</param>
        /// <returns>Operator ID if valid password, or empty string if invalid</returns>
        public String OpIDfromPassword(String Password)
        {
            if (VtiLib.Data.Users.Count(u => u.Password == Password) > 0)
                return VtiLib.Data.Users.First(u => u.Password == Password).OpID;
            else
                return string.Empty;
        }

        /// <summary>
        /// Gets the current revision of the database from the <see cref="SchemaChanges">SchemaChanges</see> table
        /// </summary>
        public SchemaRevision CurrentRevision
        {
            get
            {
                SchemaChange schemaChange = null;
                SchemaRevision revision = null;
                try
                {
                    schemaChange = this.SchemaChanges.Single(sc =>
                        sc.Id == this.SchemaChanges.Max(sc2 => sc2.Id));
                }
                catch { }
                if (schemaChange != null)
                {
                    revision = new SchemaRevision(
                        (int)schemaChange.Major,
                        (int)schemaChange.Minor,
                        (int)schemaChange.Release);
                }
                //else
                //{
                //    revision = new SchemaRevision(1, 0, 0);
                //}
                return revision;
            }
        }

        /// <summary>
        /// Updates the database schema.
        /// </summary>
        /// <remarks>
        /// <para>The database will always exist in the <see cref="Application.StartupPath">Application.StartupPath</see>
        /// in the "Data" sub-folder.  It will always have the name VtiData.mdf</para>
        /// <para>There will be a Data\VtiDataSchema folder which will contain a schema.ddl file,
        /// and a Data\VtiDataSchema\Updates folder which can contain update scripts.  Update
        /// scripts must have filenames in the format of "update.MAJ.MIN.REL.ddl"
        /// where MAJ, MIN, and REL are numerical values.
        /// (i.e. update-001.000.001.ddl, update-001.000.002.ddl, etc.)</para>
        /// <para>If the database doesn't exist, UpdateSchema will attempt to create it.</para>
        /// <para>After verifying that the database exists (or creating it), UpdateSchema will
        /// then search for update scrips in the Data\VtiDataSchema\Updates folder.
        /// If any scripts are found, it will check in the
        /// <see cref="SchemaChanges">SchemaChanges</see> table for a record to indicate
        /// if the script has already been applied.  If there is no record to indicate that
        /// the script has been applied, UpdateSchema will apply the script and add a
        /// <see cref="SchemaChange">SchemaChange</see> record to the database</para>
        /// </remarks>
        public void UpdateSchema()
        {
            SchemaRevision revision;

            try
            {
                VtiEvent.Log.WriteInfo("Checking schema for Database 'VtiData'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Database);

                // Create the database if it doesn't exist
                if (!DatabaseExists())
                {
                    VtiEvent.Log.WriteInfo("Database 'VtiData' doesn't exist. Creating database.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Database);
                    try
                    {
                        //CreateDatabase();

                        // Log into the master db and create an empty VtiData database
                        // This will allow the schema script to build the database and
                        // the update scripts to apply the updates.
                        // If I left it with the CreateDatabase above, it would create
                        // to the current version, and then the update scripts would attempt
                        // to duplicate the changes, which might break.
                        DataContext db = new DataContext(VtiLib.ConnectionString.Replace("VtiData", "master"));
                        db.ExecuteCommand("CREATE DATABASE VtiData");
                        db.Connection.Close();
                        db.Dispose();

                        // Make several attempts to connect to the database
                        for (int i = 0; i < 5; i++)
                        {
                            try
                            {
                                if (this.Connection.State == ConnectionState.Open)
                                    this.Connection.Close();
                                this.Connection.Open();
                                Thread.Sleep(2000);
                            }
                            catch (Exception e) { }
                            if (this.Connection.State == ConnectionState.Open) break;
                        }
                    }
                    catch (Exception e)
                    {
                        VtiEvent.Log.WriteError(
                            String.Format("An error occurred creating the 'VtiData' database."),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Database,
                            e.ToString());
                        return;
                    }
                }

                // Get the database revision
                revision = this.CurrentRevision;

                // If there is no revision, the primary schema script has not yet been applied
                if (revision == null)
                {
                    // Execute the primary schema script
                    Script schema = new Script(AppDomain.CurrentDomain.BaseDirectory + @"\Data\VtiDataSchema\schema.ddl");
                    if (!schema.Apply(this)) return;
                }

                // Get the database revision
                revision = this.CurrentRevision;

                // Create a list of update scripts
                //List<Script> updateScripts = new List<Script>();

                // Retrieve all scripts in the Updates directory
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Application.StartupPath + @"\Data\VtiDataSchema\Updates");
                //foreach (var fileinfo in directoryInfo.GetFiles("update-???.???.???.ddl"))
                //    updateScripts.Add(new Script(fileinfo.FullName));
                //directoryInfo.GetFiles("update-???.???.???.ddl").ToList().ForEach(F =>
                //    updateScripts.Add(new Script(F.FullName)));

                //// Sort the list
                //updateScripts.Sort();

                var updateScripts = directoryInfo.GetFiles("update-???.???.???.ddl").Select(F =>
                    new Script(F.FullName)).OrderBy(S => S.Name).ToList();

                VtiEvent.Log.WriteVerbose(
                    "Installing Update Scripts", VTIWindowsControlLibrary.Enums.VtiEventCatType.Database,
                    String.Format("{0} Update Script(s) to be processed.", updateScripts.Count));

                // Find each schema update that doesn't exist in the database and execute it
                foreach (var script in updateScripts)
                {
                    if (this.SchemaChanges.Count(sc => sc.Script_name.Equals(script.Name)) == 0)
                    {
                        if (script.Revision.Major > revision.Major)
                        {
                            VtiEvent.Log.WriteWarning(
                                String.Format("Update Script '{0}' applies to a more recent version of the database schema.", script.Name),
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Database,
                                String.Format("Database Revision is {0}.{1}.{2}", revision.Major, revision.Minor, revision.Release),
                                String.Format("Script Revision is {0}.{1}.{2}", script.Revision.Major, script.Revision.Minor, script.Revision.Release));
                            break;
                        }
                        else
                        {
                            if (!script.Apply(this)) break;
                        }
                    }
                    else
                        VtiEvent.Log.WriteVerbose(
                            String.Format("Script '{0}' has already been applied.", script.Name),
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Database);
                }

                this.SubmitChanges();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError(
                    "An error occurred updating the schema for the 'VtiData' database.",
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Database,
                    e.ToString());
            }
        }
    }
}
