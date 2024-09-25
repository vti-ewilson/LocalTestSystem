using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Classes.Data
{
    /// <summary>
    /// Represents a schema update script
    /// </summary>
    public class Script : IComparable
    {
        #region Fields (2) 

        #region Private Fields (2) 

        private List<string> _Commands;
        private SchemaRevision _Revision;

        #endregion Private Fields 

        #endregion Fields 

        #region Constructors (1) 

        /// <summary>
        /// Initializes a new instance of the <see cref="Script">Script</see>
        /// </summary>
        /// <param name="FileName"></param>
        public Script(string FileName)
        {
            this.FileName = FileName;
            this.Name = Path.GetFileName(FileName);
        }

        #endregion Constructors 

        #region Properties (4) 

        /// <summary>
        /// Gets the list of SQL Command Strings contained within the script
        /// </summary>
        public List<string> Commands
        {
            get
            {
                if (_Commands == null)
                    _Commands = Script.Parse(FileName);
                return _Commands;
            }
        }

        /// <summary>
        /// Gets the FileName of the script
        /// </summary>
        public string FileName { get; internal set; }

        /// <summary>
        /// Gets the Name of the script without any of the path information
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the <see cref="SchemaRevision">Revision</see> of the script
        /// </summary>
        public SchemaRevision Revision
        {
            get
            {
                if (_Revision == null)
                {
                    try
                    {
                        _Revision = SchemaRevision.FromFileName(FileName);
                    }
                    catch { }
                }
                return _Revision;
            }
        }

        #endregion Properties 

        #region Methods (3) 

        #region Public Methods (3) 

        /// <summary>
        /// Applys the script to a database
        /// </summary>
        /// <param name="data">DataContext of the database to apply the script to</param>
        /// <returns>True if successful</returns>
        public Boolean Apply(DataContext data)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    // Execute the update script
                    VtiEvent.Log.WriteInfo(
                        String.Format("Applying Script '{0}'", this.Name),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Database,
                        String.Format("{0} SQL Command(s) to be executed.", this.Commands.Count));
                    foreach (string command in this.Commands)
                    {
                        VtiEvent.Log.WriteVerbose("Executing SQL Command", VTIWindowsControlLibrary.Enums.VtiEventCatType.Database, command);
                        try
                        {
                            Actions.Retry(5,
                                delegate
                                {
                                    data.ExecuteCommand(command);
                                },
                                delegate
                                {
                                    if (data.Connection.State == System.Data.ConnectionState.Open)
                                        data.Connection.Close();
                                    Thread.Sleep(2000);
                                });
                        }
                        catch (Exception e)
                        {
                            VtiEvent.Log.WriteError(
                                "An error occurred executing the SQL Script.",
                                VTIWindowsControlLibrary.Enums.VtiEventCatType.Database,
                                "SQL Command: " + command,
                                e.ToString());
                            return false;
                        }
                    }

                    // Add the SchemaChange record
                    if (this.Revision != null)
                    {
                        //if (data is VtiDataContext)
                        //{
                        //    (data as VtiDataContext).SchemaChanges.InsertOnSubmit(
                        //        new SchemaChange
                        //        {
                        //            Major = this.Revision.Major,
                        //            Minor = this.Revision.Minor,
                        //            Release = this.Revision.Release,
                        //            Script_name = this.Name,
                        //            Applied = DateTime.Now
                        //        });
                        //}
                        // Do this with a SQL command, since the data context isn't always VtiData
                        data.ExecuteCommand(
                            string.Format(
                                "insert into dbo.SchemaChanges (major, minor, release, script_name, applied) values ({0}, {1}, {2}, '{3}', '{4:G}')",
                                _Revision.Major, _Revision.Minor, _Revision.Release, this.Name, DateTime.Now));
                    }
                }
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError(
                        String.Format("An error occurred applying update script '{0}'.", this.Name),
                        VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                        e.ToString());
                    return false;
                }

                ts.Complete();
            }
            return true;
        }

        /// <summary>
        /// Compares this instance with a specified <see cref="Script">Script</see> object and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified <see cref="Script">Script</see>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is Script)
                return this.Name.CompareTo((obj as Script).Name);
            else
                return 0;
        }

        /// <summary>
        /// Parses a script file and splits out the SQL commands by the GO clause.
        /// </summary>
        /// <param name="FileName">FileName of the script</param>
        /// <returns>List of strings containing the SQL commands in the script</returns>
        public static List<string> Parse(string FileName)
        {
            List<string> commands = new List<string>();
            string contents = File.ReadAllText(FileName);

            //  Split script by GO clause.
            Regex endOfCommand = new Regex("^(?<go>go)[\r\n]+", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            int startOfScript = 0;

            while (startOfScript < contents.Length)
            {
                Match go = endOfCommand.Match(contents, startOfScript);
                if (!go.Success)
                    break;

                //  Split file contents at GO.
                int whereIsGo = go.Groups["go"].Index;

                //  Extract command and apply all the macros.
                string command = contents.Substring(startOfScript, whereIsGo - startOfScript).Trim('\r', '\n', '\t', ' ');
                //foreach (string key in Macros.Keys)
                //    command = command.Replace(string.Format("*{0}*", key), Macros[key]);

                //  Add a command to list.
                commands.Add(command);

                //  Move the start pointer.
                startOfScript = whereIsGo + 2;
            }

            return commands;
        }

        #endregion Public Methods 

        #endregion Methods 
    }
}