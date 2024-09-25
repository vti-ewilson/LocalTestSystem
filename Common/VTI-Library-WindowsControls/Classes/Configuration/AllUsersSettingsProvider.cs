using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Implements a <see cref="SettingsProvider"/> that saves the settings to the
    /// "All Users" application data folder.
    /// </summary>
    public class AllUsersSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        #region Properties (1)

        /// <summary>
        /// Gets or sets the name of the currently running application.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                if (Application.ProductName.Trim().Length > 0) return Application.ProductName;
                else return Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            }
            set
            {
                // do nothing
            }
        }

        #endregion Properties

        #region Methods (13)

        #region Public Methods (8)

        /// <summary>
        /// Gets the path to the previous version of the settings.
        /// </summary>
        /// <returns>Path to the previous version of the settings.</returns>
        public virtual string GetPrevSettingsPath()
        {
            // Get the directory info on the [CommonApplicationData]\[CompanyName]\[ApplicationName] directory
            DirectoryInfo di = new DirectoryInfo(
               string.Format(@"{0}",
                    GetSettingsPath().Replace(@"\" + Application.ProductVersion, "")));

            //DirectoryInfo di = new DirectoryInfo(
            //    string.Format(@"{0}\{1}\{2}",
            //        //System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), //this is %ProgramData%
            //        "C:\\VTI PC\\Config",
            //        Application.CompanyName,
            //        ApplicationName));

            if (!di.Exists) return string.Empty;

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
                return string.Empty;
            }
            else
            {
                return string.Format(@"{0}\{1}",
                    GetSettingsPath().Replace(@"\" + Application.ProductVersion, ""),
                    prevDir.name);

                //return string.Format(@"{0}\{1}\{2}\{3}",
                //    //System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), //this is %ProgramData%
                //    "C:\\VTI PC\\Config",
                //Application.CompanyName,
                //ApplicationName,
                //prevDir.name);
            }
        }

        /// <summary>
        /// Gets the previous settings path and filename.
        /// </summary>
        /// <returns>Path and filename to the previous settings.</returns>
        public virtual string GetPrevSettingsPathAndFilename()
        {
            string prevPath = GetPrevSettingsPath();
            if (string.IsNullOrEmpty(prevPath)) return string.Empty;
            if (prevPath == GetSettingsPath()) return string.Empty;
            else return Path.Combine(prevPath, GetSettingsFilename());
        }

        /// <summary>
        /// Returns the collection of settings property values for the specified application instance and settings property group.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext"/> describing the current application use.</param>
        /// <param name="collection">A <see cref="T:System.Configuration.SettingsPropertyCollection"/> containing the settings property group whose values are to be retrieved.</param>
        /// <returns>
        /// A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> containing the values for the specified settings property group.
        /// </returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            System.Configuration.Configuration config;
            ConfigurationSectionGroup group;
            ClientSettingsSection configSection;
            string sectionName;

            // Get the configuration, group, and section
            GetConfigurationGroupAndSection(context, out config, out group, out configSection, out sectionName);

            // Create new collection of values
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            // Iterate through the settings to be retrieved
            foreach (SettingsProperty setting in collection)
            {
                SettingsPropertyValue value = new SettingsPropertyValue(setting);
                value.IsDirty = false;
                value.SerializedValue = GetValue(configSection, setting);
                values.Add(value);
            }
            return values;
        }

        /// <summary>
        /// Gets the settings filename.
        /// </summary>
        /// <returns>Filename of the settings.</returns>
        public virtual string GetSettingsFilename()
        {
            return string.Format("{0}.config", ApplicationName.Replace(' ', '_'));
        }

        /// <summary>
        /// Gets the settings path.
        /// </summary>
        /// <returns>Path to the settings file.</returns>
        public virtual string GetSettingsPath()
        {
            return
                string.Format(@"{0}\{1}\{2}\{3}",
                    //System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), //this is %ProgramData%
                    "C:\\VTI PC\\Config",
                    Application.CompanyName,
                    ApplicationName,
                    Application.ProductVersion);
        }

        /// <summary>
        /// Gets the settings path and filename.
        /// </summary>
        /// <returns></returns>
        public virtual string GetSettingsPathAndFilename()
        {
            return Path.Combine(GetSettingsPath(), GetSettingsFilename());
        }

        /// <summary>
        /// Returns true if the config file exists, false otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool ConfigFileExists()
        {
            string configFile = Path.Combine(GetSettingsPath(), GetSettingsFilename());
            return File.Exists(configFile);
        }

        /// <summary>
        /// Initializes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public override void Initialize(string name, NameValueCollection values)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "AllUsersSettingsProvider";
            }
            base.Initialize(name, values);
        }

        /// <summary>
        /// Sets the values of the specified group of property settings.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext"/> describing the current application usage.</param>
        /// <param name="collection">A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> representing the group of property settings to set.</param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            System.Configuration.Configuration config;
            ConfigurationSectionGroup group;
            ClientSettingsSection configSection;
            string sectionName;

            // Get the configuration, group, and section, create the group and section if they don't already exist
            GetOrCreateConfigurationGroupAndSection(context, out config, out group, out configSection, out sectionName);

            // Iterate through the settings to be stored
            // Only dirty settings are included in propvals, and only ones relevant to this provider
            foreach (SettingsPropertyValue propval in collection)
            {
                //if (CleanSerializedValue(propval.SerializedValue.ToString()) == CleanSerializedValue(propval.Property.DefaultValue.ToString()))
                //if (propval.PropertyValue.GetType() == typeof(IEditCycleParameter) &&
                //    (propval.PropertyValue as IEditCycleParameter).ProcessValueString ==
                //     (propval.Property.DefaultValue as IEditCycleParameter).ProcessValueString)

                //if (propval.Property.SerializeAs == SettingsSerializeAs.Xml &&
                //    XmlValuesEqual(propval.SerializedValue.ToString(), propval.Property.DefaultValue.ToString()))
                //    RemoveValue(configSection, propval);
                //else if (propval.Property.SerializeAs == SettingsSerializeAs.String &&
                //    propval.SerializedValue.ToString() == propval.Property.DefaultValue.ToString())
                //    RemoveValue(configSection, propval);
                //else
                if (propval.SerializedValue != null)
                    SetValue(configSection, propval);
            }
            //configSection.SectionInformation.ForceSave = true;

            //config.Save(ConfigurationSaveMode.Minimal, true);
            //config.Save(ConfigurationSaveMode.Modified);
            config.Save(ConfigurationSaveMode.Minimal);
        }

        #endregion Public Methods
        #region Private Methods (5)

        private void GetConfigurationGroupAndSection(
            SettingsContext context,
            out System.Configuration.Configuration config,
            out ConfigurationSectionGroup group,
            out ClientSettingsSection section,
            out string sectionName)
        {
            GetConfigurationGroupAndSection(
                context,
                GetSettingsPathAndFilename(),
                out config,
                out group,
                out section,
                out sectionName);
        }

        private void GetConfigurationGroupAndSection(
            SettingsContext context,
            string configurationFile,
            out System.Configuration.Configuration config,
            out ConfigurationSectionGroup group,
            out ClientSettingsSection section,
            out string sectionName)
        {
            Type settingsClassType;
            ExeConfigurationFileMap fileMap;
            // Get the type of the settings class
            settingsClassType = context["SettingsClassType"] as Type;
            // Save the name
            sectionName = settingsClassType.FullName;
            // Create a file map to the [CommonApplicationData]\[CompanyName]\[ApplicationName]\[Version]\[ApplicationName].config file
            fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configurationFile;
            // Read the configuration file
            //****mdb 3/4/16
            try
            {
                config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            }
            catch
            {
                fileMap.ExeConfigFilename = configurationFile + "Good";
                config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            }
            //****mdb 3/4/a6
            // Get the userSettings group
            group = config.SectionGroups["userSettings"];
            if (group == null) section = null;
            // Get the settings section
            else section = (ClientSettingsSection)group.Sections[sectionName];
            if (section != null)
            {
                section.SectionInformation.ForceSave = true;
                section.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
                section.SectionInformation.RequirePermission = false;
            }
        }

        private void GetOrCreateConfigurationGroupAndSection(
            SettingsContext context,
            out System.Configuration.Configuration config,
            out ConfigurationSectionGroup group,
            out ClientSettingsSection section,
            out string sectionName)
        {
            // Get the configuration, userSettings group, and settings section
            GetConfigurationGroupAndSection(context, out config, out group, out section, out sectionName);
            // Create the userSettings group if it doesn't exist
            if (group == null)
            {
                group = new ConfigurationSectionGroup();
                config.SectionGroups.Add("userSettings", group);
            }
            // Create the settings section if it doesn't exist
            if (section == null)
            {
                section = new ClientSettingsSection();
                section.SectionInformation.ForceSave = true;
                section.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
                section.SectionInformation.RequirePermission = false;
                group.Sections.Add(sectionName, section);
            }
        }

        private string GetValue(ClientSettingsSection configSection, SettingsProperty setting)
        {
            string ret = string.Empty;

            // If the settings section isn't empty, attempt to read the setting
            if (configSection != null)
            {
                try
                {
                    // Read the setting from the settings section
                    SettingElement element = configSection.Settings.Get(setting.Name);
                    if (element != null)
                    {
                        if (setting.SerializeAs == SettingsSerializeAs.Xml)
                            ret = element.Value.ValueXml.InnerXml;
                        else
                            ret = element.Value.ValueXml.InnerText;
                    }
                }
                catch { }
            }

            // If there's no value yet, it's because he settings section was empty,
            // the setting wasn't there, or it was corrupt, so get the default value
            if (string.IsNullOrEmpty(ret) && setting.DefaultValue != null)
                ret = setting.DefaultValue.ToString();

            return ret;
        }

        private void SetValue(ClientSettingsSection configSection, SettingsPropertyValue propVal)
        {
            SettingElement element;
            // Get the setting element from the settings section
            element = configSection.Settings.Get(propVal.Name);
            // Create a new one if one doesn't exist
            if (element == null)
            {
                element = new SettingElement(propVal.Name, propVal.Property.SerializeAs);
                configSection.Settings.Add(element);
            }
            // Create the value node if it doesn't exist
            if (element.Value.ValueXml == null)
            {
                element.Value.ValueXml = new XmlDocument().CreateElement("value");
            }
            // Determine the type of serialization
            switch (propVal.Property.SerializeAs)
            {
                // Save the XML serialized value, stripping off some extraneous XML tags
                case SettingsSerializeAs.Xml:
                    element.Value.ValueXml.InnerXml = CleanSerializedValue(propVal.SerializedValue.ToString());
                    break;
                // Save the string serialized value
                case SettingsSerializeAs.String:
                    element.Value.ValueXml.InnerText = propVal.SerializedValue.ToString();
                    break;
                // Save the binary serialized value
                case SettingsSerializeAs.Binary:
                    element.Value.ValueXml.InnerText = Convert.ToBase64String(propVal.SerializedValue as byte[]);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private string CleanSerializedValue(string serializedValue)
        {
            string cleanValue;
            cleanValue = Regex.Replace(serializedValue, @"^<\?xml .*?\?>", string.Empty);
            cleanValue = Regex.Replace(cleanValue, @"\s*xmlns:xsi="".*""", string.Empty);
            cleanValue = Regex.Replace(cleanValue, @"\s*xmlns:xsd="".*""", string.Empty);
            //cleanValue = cleanValue.Trim();
            return cleanValue;
        }

        //private bool XmlValuesEqual(string left, string right)
        //{
        //    string cleanLeft;
        //    cleanLeft = Regex.Replace(left, @"^<\?xml .*?\?>", string.Empty);
        //    cleanLeft = Regex.Replace(cleanLeft, @"\s*xmlns:xsi="".*?""", string.Empty);
        //    cleanLeft = Regex.Replace(cleanLeft, @"\s*xmlns:xsd="".*?""", string.Empty);
        //    cleanLeft = Regex.Replace(cleanLeft, @"(?<=(>))\s*(?=(<))", string.Empty);
        //    cleanLeft = cleanLeft.Trim();
        //    string cleanRight;
        //    cleanRight = Regex.Replace(right, @"^<\?xml .*?\?>", string.Empty);
        //    cleanRight = Regex.Replace(cleanRight, @"\s*xmlns:xsi="".*?""", string.Empty);
        //    cleanRight = Regex.Replace(cleanRight, @"\s*xmlns:xsd="".*?""", string.Empty);
        //    cleanRight = Regex.Replace(cleanRight, @"(?<=(>))\s*(?=(<))", string.Empty);
        //    cleanRight = cleanRight.Trim();

        //    return cleanLeft == cleanRight;
        //}

        private void RemoveValue(ClientSettingsSection configSection, SettingsPropertyValue propVal)
        {
            SettingElement element;
            // Get the setting element from the settings section
            element = configSection.Settings.Get(propVal.Name);
            // Create a new one if one doesn't exist
            if (element != null)
                configSection.Settings.Remove(element);
        }

        #endregion Private Methods

        #endregion Methods

        #region IApplicationSettingsProvider Members

        /// <summary>
        /// Returns the value of the specified settings property for the previous version of the same application.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext"/> describing the current application usage.</param>
        /// <param name="property">The <see cref="T:System.Configuration.SettingsProperty"/> whose value is to be returned.</param>
        /// <returns>
        /// A <see cref="T:System.Configuration.SettingsPropertyValue"/> containing the value of the specified property setting as it was last set in the previous version of the application; or null if the setting cannot be found.
        /// </returns>
        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            System.Configuration.Configuration config;
            ConfigurationSectionGroup group;
            ClientSettingsSection configSection;
            string sectionName;
            string configurationFilename = GetPrevSettingsPathAndFilename();

            // Create the settings property value
            SettingsPropertyValue propVal = new SettingsPropertyValue(property);

            if (!string.IsNullOrEmpty(configurationFilename))
            {
                // Get the configuration, group, and settings section from the previous configuration file
                GetConfigurationGroupAndSection(context, configurationFilename, out config, out group, out configSection, out sectionName);
                // Read the value from the settings section, if possible
                if (configSection != null)
                {
                    try
                    {
                        SettingElement element = configSection.Settings.Get(property.Name);
                        if (property.SerializeAs == SettingsSerializeAs.Xml)
                            propVal.SerializedValue = element.Value.ValueXml.InnerXml;
                        else
                            propVal.SerializedValue = element.Value.ValueXml.InnerText;
                    }
                    catch { }
                }
            }

            // If no value yet, provide the default
            if (propVal.SerializedValue == null)
            {
                if (property.DefaultValue != null) propVal.SerializedValue = property.DefaultValue.ToString();
                else propVal.SerializedValue = string.Empty;
            }

            return propVal;
        }

        /// <summary>
        /// Resets the application settings associated with the specified application to their default values.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext"/> describing the current application usage.</param>
        public void Reset(SettingsContext context)
        {
            System.Configuration.Configuration config;
            ConfigurationSectionGroup group;
            ClientSettingsSection configSection;
            string sectionName;
            // Get the config, group, and settings section
            GetConfigurationGroupAndSection(context, out config, out group, out configSection, out sectionName);
            // Remove the settings section from the group
            if (group != null) group.Sections.Remove(sectionName);
            // Save the config
            config.Save(ConfigurationSaveMode.Minimal);
        }

        /// <summary>
        /// Indicates to the provider that the application has been upgraded. This offers the provider an opportunity to upgrade its stored settings as appropriate.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext"/> describing the current application usage.</param>
        /// <param name="properties">A <see cref="T:System.Configuration.SettingsPropertyCollection"/> containing the settings property group whose values are to be retrieved.</param>
        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            System.Configuration.Configuration config;
            ConfigurationSectionGroup group;
            ClientSettingsSection configSection;
            string sectionName;

            // Get the config, group, and section, create the group and section if they are missing
            GetOrCreateConfigurationGroupAndSection(context, out config, out group, out configSection, out sectionName);

            // Iterate through the properties, retrieving the values from the previous version
            // and saving them in the new version
            foreach (SettingsProperty property in properties)
            {
                SettingsPropertyValue propval = GetPreviousVersion(context, property);
                // Remove any values that are equal to the default values
                //if (CleanSerializedValue(propVal.SerializedValue.ToString()) == CleanSerializedValue(property.DefaultValue.ToString()))
                //if (XmlValuesEqual(propval.SerializedValue.ToString(), propval.Property.DefaultValue.ToString()))
                //    RemoveValue(configSection, propval);
                //// Save the rest
                //else
                SetValue(configSection, propval);
            }

            config.Save(ConfigurationSaveMode.Minimal);
        }

        #endregion
    }
}