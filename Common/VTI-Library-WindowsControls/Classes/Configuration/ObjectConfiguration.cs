using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Represents a base class for a simple object-based configuration class.
    /// </summary>
    /// <remarks>
    /// Any class which is derived from this class can be both used as a configuration section
    /// in a user.config file while still maintaining itself as a simple enough object to pass
    /// through an XML serializer.
    /// </remarks>
    public abstract class ObjectConfiguration
    {
        #region Fields (1)

        #region Private Fields (1)

        private string _sectionName;
        private string _configurationFilename = string.Empty;
        private bool _useCommonApplicationData = true;
        private bool _callUpgrade = true;
        private DataPlotControl _dataPlot;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfiguration">ObjectConfiguration</see> class.
        /// </summary>
        /// <param name="sectionName">Section name in the user.config file to use</param>
        public ObjectConfiguration(string sectionName)
        {
            _sectionName = sectionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfiguration">ObjectConfiguration</see> class.
        /// </summary>
        public ObjectConfiguration()
        {
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets or sets the section name in the user.config file to use
        /// </summary>
        [XmlIgnore()]
        public string SectionName
        {
            get { return _sectionName; }
            set { _sectionName = value; }
        }

        ///// <summary>
        ///// Gets or sets the section name in the user.config file to use
        ///// </summary>
        //[XmlIgnore()]
        //public string ConfigurationFilename
        //{
        //    get { return _configurationFilename; }
        //    set { _configurationFilename = value; }
        //}

        [XmlIgnore()]
        public bool UseCommonApplicationData
        {
            get { return _useCommonApplicationData; }
            set { _useCommonApplicationData = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the settings need to be upgraded
        /// </summary>
        public bool CallUpgrade
        {
            get { return _callUpgrade; }
            set { _callUpgrade = value; }
        }

        #endregion Properties

        #region Methods (4)

        #region Public Methods (4)

        /// <summary>
        /// Retrieves the configuration object from the configuration file.
        /// </summary>
        public void Load()
        {
            var _allUserSettingsProvider = new AllUsersSettingsProvider();
            if (_useCommonApplicationData)
            {
                    _configurationFilename =
                        string.Format(@"{0}",
                        _allUserSettingsProvider.GetSettingsPathAndFilename());

                //_configurationFilename =
                // string.Format(@"{0}\{1}\{2}\{3}\{4}.config",
                //// System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), //this is %ProgramData%
                //"C:\\VTI PC\\Config",
                // Application.CompanyName,
                // Application.ProductName,
                // Application.ProductVersion,
                // Application.ProductName.Replace(' ', '_'));
            }
            else
            {
                _configurationFilename = string.Format(@"{0}\{1}.config",
                    _allUserSettingsProvider.GetSettingsPath(), "user");

                //_configurationFilename =
                //    string.Format(@"{0}\{1}\{2}\{3}\{4}.config",
                //    System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), //this is %AppData%\Local
                //    Application.CompanyName,
                //    Application.ProductName,
                //    Application.ProductVersion,
                //    "user");
            }

            // Load the configuration
            object configObject = ObjectConfiguration.Load(_sectionName, _configurationFilename);
            if (configObject != null) CopyProperties(configObject, this);
            else CallUpgrade = true;
            OnLoaded();
        }

        private void CopyProperties(object source, object dest)
        {
            if (source != null && dest != null)
            {
                // Use reflection to copy all of the properties to the current object
                PropertyInfo[] srcPropInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo[] destPropInfos = dest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var srcPi in srcPropInfos.Where(pi => pi.CanWrite && pi.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length == 0))
                {
                    PropertyInfo destPi = destPropInfos.FirstOrDefault(pi => pi.Name == srcPi.Name && pi.CanWrite);
                    if (destPi != null)
                        destPi.SetValue(dest, srcPi.GetValue(source, null), null);
                }
            }
        }

        /// <summary>
        /// Retrieves the configuration object from a previous version of the configuration file, if any exists
        /// </summary>
        public void Upgrade()
        {
            var _allUsersSettingsProvider = new AllUsersSettingsProvider();
            DirectoryInfo di;
            if (_useCommonApplicationData)
            {
                // Get the directory info on the [CommonApplicationData]\[CompanyName]\[ApplicationName] directory
                di = new DirectoryInfo(
                    string.Format(@"{0}",
                      _allUsersSettingsProvider.GetSettingsPath().Replace(@"\" + Application.ProductVersion, "")));

                //DirectoryInfo di2 = new DirectoryInfo(
                //    string.Format(@"{0}\{1}\{2}",
                //      //System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), // this is %ProgramData%
                //      "C:\\VTI PC\\Config",
                //        Application.CompanyName,
                //        Application.ProductName));
            }
            else
            {
                // Get the directory info on the [ApplicationData]\[CompanyName]\[ApplicationName] directory
                di = new DirectoryInfo(
                   string.Format(@"{0}",
                     _allUsersSettingsProvider.GetSettingsPath().Replace(@"\" + Application.ProductVersion, "")));

                //di = new DirectoryInfo(
                //    string.Format(@"{0}\{1}\{2}",
                //        System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                //        Application.CompanyName,
                //        Application.ProductName));
            }

            if (!di.Exists)
            {
                VtiEvent.Log.WriteError(di.FullName + " Does not exist.");
                return;
            }

            // Get the list of sub-directories
            DirectoryInfo[] dirs = di.GetDirectories();

            // Take the directories, split out the revs, sort descending, take the first one, that's the most recent
            var prevDir =
                dirs.Where(d => d.Name.Split('.').Count() == 4)
                    .Select(d =>
                    {
                        string[] parts = d.Name.Split('.');

                        return new
                        {
                            name = d.Name,
                            major = int.Parse(parts[0]),
                            minor = int.Parse(parts[1]),
                            revision = int.Parse(parts[2]),
                            build = int.Parse(parts[3])
                        };
                    })
                    .Where(v => v.name != Application.ProductVersion)
                    .OrderByDescending(v => v.major)
                    .ThenByDescending(v => v.minor)
                    .ThenByDescending(v => v.revision)
                    .ThenByDescending(v => v.build)
                    .FirstOrDefault();

            if (prevDir == null)
            {
                VtiEvent.Log.WriteError("Error upgrading config file from previous version. Previous version's directory does not exist.");
                return;
            }

            string upgradeFilename;

            if (_useCommonApplicationData)
            {
                upgradeFilename = string.Format(@"{0}.config",
                      _allUsersSettingsProvider.GetSettingsPath().Replace(Application.ProductVersion, prevDir.name));

                //upgradeFilename = string.Format(@"{0}\{1}\{2}\{3}\{4}.config",
                //      //System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData),
                //      "C:\\VTI PC\\Config",
                //    Application.CompanyName,
                //    Application.ProductName,
                //    prevDir.name,
                //    Application.ProductName.Replace(' ', '_'));
            }
            else
            {
                upgradeFilename = string.Format(@"{0}.config",
                      _allUsersSettingsProvider.GetSettingsPath().Replace(Application.ProductVersion, prevDir.name).Replace(Application.ProductName.Replace(' ', '_'), "user"));

                //upgradeFilename = string.Format(@"{0}\{1}\{2}\{3}\user.config",
                //    System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                //    Application.CompanyName,
                //    Application.ProductName,
                //    prevDir.name);
            }

            // Load the configuration
            object configObject = ObjectConfiguration.Load(_sectionName, upgradeFilename);
            CopyProperties(configObject, this);
            VtiEvent.Log.WriteError("Config file upgraded.");
            OnUpgraded();
        }

        /// <summary>
        /// Retrieves a <see cref="ObjectConfiguration">ObjectConfiguration</see> object from the specified configuration file.
        /// If the file is not specified, the user.config file is used.
        /// </summary>
        /// <param name="sectionName">Section name in the configuration file to use</param>
        /// <param name="configurationFilename">Name of the configuration file from which to load the settings.</param>
        /// <returns>A <see cref="ObjectConfiguration">ObjectConfiguration</see> object</returns>
        public static object Load(string sectionName, string configurationFilename)
        {
            ObjectConfigurationSection configSection = null;
            try
            {
                System.Configuration.Configuration config;
                // If no configuration filename specified, open the user.config file
                if (string.IsNullOrEmpty(configurationFilename))
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                }
                // Otherwise, open the specified configuration file
                else
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = configurationFilename;
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                }
                ConfigurationSectionGroup group = config.SectionGroups["userSettings"];
                if (group == null) return null;
                configSection = (ObjectConfigurationSection)group.Sections[sectionName];
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(
                    string.Format("An error occurred retrieving the {0} configuration, using default configuration.", sectionName),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());
            }
            if (configSection == null) return null;
            else return configSection.Data;
        }

        /// <summary>
        /// Saves the object to the user.config configuration file
        /// </summary>
        public void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(_sectionName)) throw new Exception("SectionName must be assigned before configuration can be saved.");

                System.Configuration.Configuration config;
                // If no configuration filename specified, open the user.config file
                if (string.IsNullOrEmpty(_configurationFilename))
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                }
                // Otherwise, open the specified configuration file
                else
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = _configurationFilename;
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                }
                ConfigurationSectionGroup group = config.SectionGroups["userSettings"];
                // Create the userSettings group if it doesn't exist
                if (group == null)
                {
                    group = new ConfigurationSectionGroup();
                    config.SectionGroups.Add("userSettings", group);
                }
                // Remove and re-add the settings section
                if (_sectionName.Contains('/'))
                    _sectionName = _sectionName.Replace('/', '_');
                group.Sections.Remove(_sectionName);
                ObjectConfigurationSection configSection =
                    new ObjectConfigurationSection(this);
                configSection.SectionInformation.ForceSave = true;
                configSection.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
                configSection.SectionInformation.RequirePermission = false;
                group.Sections.Add(_sectionName, configSection);
                config.Save();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteWarning(
                    string.Format("An error occurred saving the {0} configuration.", _sectionName),
                    VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                    e.ToString());
            }
        }

        /// <summary>
        /// Saves the object to the specified section of the user.config configuration file.
        /// </summary>
        /// <param name="sectionName">Section name in the user.config file to use</param>
        public void Save(string sectionName)
        {
            _sectionName = sectionName;
            Save();
        }

        /// <summary>
        /// Store a reference to the DataPlot object
        /// </summary>
        public void StoreDataPlot(DataPlotControl DataPlot)
        {
            _dataPlot = DataPlot;
        }

        #endregion Public Methods

        #endregion Methods

        public event EventHandler Loaded;

        public event EventHandler Upgraded;

        protected virtual void OnLoaded()
        {
            if (Loaded != null)
                Loaded(this, EventArgs.Empty);
        }

        protected virtual void OnUpgraded()
        {
            if (Upgraded != null)
                Upgraded(this, EventArgs.Empty);
        }
    }
}