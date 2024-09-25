using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Configuration.Interfaces;
using System.Collections.Generic;
using System.Xml;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Base class to be used for Edit Cycle configuration classes.
    /// </summary>
    /// <remarks>This class is derived from the <see cref="ApplicationSettingsBase">ApplicationSettingsBase</see>
    /// class.  It overrides the <c>Upgrade()</c> method in order to reset the <c>DisplayName, ToolTip,
    /// SmallStep, LargeStep, MinValue, MaxValue, and Units</c> properties of parameters when the
    /// configuration is upgraded.</remarks>
    public class EditCycleApplicationSettingsBase : ApplicationSettingsBase
    {
        /// <summary>
        /// Updates application settings to reflect a more recent installation of the application.
        /// </summary>
        public new void Upgrade()
        {
            IEditCycleParameter param, defaultParam;
            TimeDelayParameter timeDelayParameter, timeDelayParameterDefault;
            NumericParameter numericParameter, numericParameterDefault;

            // Call the base upgrade method, which will update the settings from the user.config file
            base.Upgrade();

            // Iterate through the parameters and update the appropriate properties
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (property.PropertyType.GetInterface("IEditCycleParameter") != null)
                {
                    param = property.GetValue(this, null) as IEditCycleParameter;

                    if (property.GetCustomAttributes(typeof(DefaultSettingValueAttribute), false).Length > 0)
                    {
                        DefaultSettingValueAttribute defaultSettingValue =
                            (DefaultSettingValueAttribute)property.GetCustomAttributes(typeof(DefaultSettingValueAttribute), false)[0];

                        XmlSerializer x = new XmlSerializer(property.PropertyType);
                        using (MemoryStream s = new MemoryStream(
                            Encoding.ASCII.GetBytes(defaultSettingValue.Value)))
                        {
                            defaultParam = x.Deserialize(s) as IEditCycleParameter;
                        }

                        param.DisplayName = defaultParam.DisplayName;
                        param.ToolTip = defaultParam.ToolTip;

                        // Process time delay parameters
                        if (property.PropertyType == typeof(TimeDelayParameter))
                        {
                            timeDelayParameter = param as TimeDelayParameter;
                            timeDelayParameterDefault = defaultParam as TimeDelayParameter;

                            timeDelayParameter.SmallStep = timeDelayParameterDefault.SmallStep;
                            timeDelayParameter.LargeStep = timeDelayParameterDefault.LargeStep;
                            timeDelayParameter.MinValue = timeDelayParameterDefault.MinValue;
                            timeDelayParameter.MaxValue = timeDelayParameterDefault.MaxValue;
                            timeDelayParameter.Units = timeDelayParameterDefault.Units;
                        }

                        // Process numeric parameters
                        else if (property.PropertyType == typeof(NumericParameter))
                        {
                            numericParameter = param as NumericParameter;
                            numericParameterDefault = defaultParam as NumericParameter;

                            numericParameter.SmallStep = numericParameterDefault.SmallStep;
                            numericParameter.LargeStep = numericParameterDefault.LargeStep;
                            numericParameter.MinValue = numericParameterDefault.MinValue;
                            numericParameter.MaxValue = numericParameterDefault.MaxValue;
                            numericParameter.Units = numericParameterDefault.Units;
                        }
                    }
                }
            }

            // Save the updates
            base.Save();
        }

        /// <summary>
        /// Backs up the entire Edit Cycle config file.
        /// Returns true if backup was successful, false if not.
        /// </summary>
        public bool BackupEditCycleConfig()
        {
            var _allUsersSettingsProvider = new AllUsersSettingsProvider();
            if (!_allUsersSettingsProvider.ConfigFileExists())
            {
                VtiEvent.Log.WriteError("Error backing up Edit Cycle config file: config file does not exist.");
                return false;
            }
            string strConfigFolder = _allUsersSettingsProvider.GetSettingsPath();
            string strConfigFile = _allUsersSettingsProvider.GetSettingsPathAndFilename();

            try
            {
                int cnt = 0;
                string configBackupFolder = strConfigFolder + @"\Backup\";
                // create the backup folder if it does not already exist
                if (!Directory.Exists(configBackupFolder))
                {
                    Directory.CreateDirectory(configBackupFolder);
                    DirectoryInfo di = new DirectoryInfo(configBackupFolder);
                    di.Attributes = di.Attributes & ~FileAttributes.ReadOnly;
                }
                while (true)
                {
                    string configBackupFileName = configBackupFolder + Application.ProductName.Replace(' ', '_') + string.Format("_{0:00}.config", cnt);
                    if (!File.Exists(configBackupFileName))
                    {
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            //try to load the XML config file. It it fails to load, it's corrupted. Don't back it up.
                            doc.Load(strConfigFile);
                        }
                        catch (Exception ee)
                        {
                            VtiEvent.Log.WriteError("Failed to load config file when creating backup. It may be corrupted. Exception: " + ee.Message);
                            return false;
                        }
                        File.Copy(strConfigFile, configBackupFileName);
                        File.SetLastWriteTime(configBackupFileName, DateTime.Now);
                        //delete oldest files if number of backup config files > numBackupFilesToKeep
                        DirectoryInfo d = new DirectoryInfo(configBackupFolder);
                        List<FileInfo> fileList = d.GetFiles().Where(x => x.Extension == ".config").ToList();
                        int numBackupFilesToKeep = 200;
                        if (fileList.Count > numBackupFilesToKeep)
                        {
                            fileList.OrderBy(x => x.LastWriteTime).Take(fileList.Count - numBackupFilesToKeep).ToList().ForEach(x => x.Delete());
                        }
                        break;
                        //// delete all backup config files that are more than a month old
                        //foreach (string f in Directory.GetFiles(strConfigFolder + @"Backup\"))
                        //{
                        //    if (f.Substring(f.Length - ".config".Length) == ".config" && f.Substring(f.Length - "00.config".Length - 1, 1) == "_")
                        //    {
                        //        DateTime dt = File.GetLastWriteTime(f), CrntTime = DateTime.Now;
                        //        TimeSpan ElapsedTime = CrntTime - dt;
                        //        if (ElapsedTime.Days > 30)
                        //            File.Delete(f);
                        //    }
                        //}
                    }
                    cnt++;
                }
                return true;
            }
            catch (Exception ex)
            {
                VtiEvent.Log.WriteError("Error backing up Edit Cycle config file: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="VTIWindowsControlLibrary.Classes.Configuration.Interfaces.IEditCycleParameter"/>
        /// with the specified property name.
        /// </summary>
        /// <value></value>
        public new IEditCycleParameter this[string propertyName]
        {
            get
            {
                lock(_lockobj) {
                    bool dbState = VtiLib.MuteParamChangeLog;
                    VtiLib.MuteParamChangeLog = true;
                    var x = base[propertyName] as IEditCycleParameter;
				    VtiLib.MuteParamChangeLog = dbState;
				    return x;
                }
                
			}
			set
            {
                base[propertyName] = value;
            }
        }

        private static object _lockobj = new object();
    }
}