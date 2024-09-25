using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Graphing;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationSection">ConfigurationSection</see> class which can save
    /// any object to a configuration section.
    /// </summary>
    public class ObjectConfigurationSection : ConfigurationSection
    {
        #region Fields (1)

        #region Private Fields (1)

        private Type type;
        private object data;
        private int YMinMaxExpUpperBound = 6;
        private int YMinMaxExpLowerBound = -14;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationSection">ObjectConfigurationSection</see> class.
        /// </summary>
        /// <param name="configurationObject">Object to be saved to the configuration</param>
        public ObjectConfigurationSection(object configurationObject)
        {
            type = configurationObject.GetType();
            data = configurationObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConfigurationSection">ObjectConfigurationSection</see> class.
        /// </summary>
        public ObjectConfigurationSection()
        {
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets or sets the object to be saved to or retrieved from the configuration
        /// </summary>
        public object Data
        {
            get { return data; }
            set { data = value; }
        }

        #endregion Properties

        #region Methods (4)

        #region Protected Methods (4)

        /// <summary>
        /// Reads XML from the configuration file.
        /// </summary>
        /// <param name="xmlReader">The
        /// <see cref="System.Xml.XmlReader">XmlReader</see> object,
        /// which reads from the configuration file.</param>
        protected override void DeserializeSection(System.Xml.XmlReader xmlReader)
        {
            xmlReader.MoveToContent();
            // Read the object type attribute
            string typeName = xmlReader.GetAttribute("type");
            type = Type.GetType(typeName);
            if (type == null) return;
            // Create an XmlSerializer of the object's type
            XmlSerializer serializer = new XmlSerializer(type);
            // Read the XML
            StringReader stringReader = new StringReader(xmlReader.ReadInnerXml());
            // Deserialize the object
            data = serializer.Deserialize(stringReader);
            // Cast obj to GraphSettings
            GraphSettings graphSettings = data as GraphSettings;
            if (graphSettings != null)
            {
                // Modify the property
                if (graphSettings.YMaxExp > YMinMaxExpUpperBound)
                {
                    graphSettings.YMaxExp = YMinMaxExpUpperBound;
                }
                else if (graphSettings.YMaxExp < YMinMaxExpLowerBound)
                {
                    graphSettings.YMaxExp = YMinMaxExpLowerBound;
                }
                if (graphSettings.YMinExp > YMinMaxExpUpperBound)
                {
                    graphSettings.YMinExp = YMinMaxExpUpperBound;
                }
                else if (graphSettings.YMinExp < YMinMaxExpLowerBound)
                {
                    graphSettings.YMinExp = YMinMaxExpLowerBound;
                }

                if (graphSettings.YMinExp > graphSettings.YMaxExp)
                {
                    graphSettings.YMaxExp = -3;
                    graphSettings.YMinExp = -6;

                }
                // Assign the modified GraphSettings back to obj
                data = graphSettings;
            }
        }

        /// <summary>
        /// Writes the contents of this configuration element to the configuration file.
        /// </summary>
        /// <param name="writer">The System.Xml.XmlWriter that writes to the configuration file.</param>
        /// <param name="serializeCollectionKey">true to serialize only the collection key properties; otherwise, false.</param>
        /// <returns>true if any data was actually serialized; otherwise, false.</returns>
        protected override bool SerializeElement(System.Xml.XmlWriter writer, bool serializeCollectionKey)
        {
            if (writer == null) return false;
            // Add attribute for object type
            writer.WriteAttributeString("type", type.AssemblyQualifiedName);
            // Create a namespace
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            // Set blank namespace values, this keeps the XmlSerializer from writing the namespace, which we don't need
            ns.Add("", "");

            // Cast obj to GraphSettings
            GraphSettings graphSettings = data as GraphSettings;
            if (graphSettings != null)
            {
                // Modify the property
                if (graphSettings.YMaxExp > YMinMaxExpUpperBound)
                {
                    graphSettings.YMaxExp = YMinMaxExpUpperBound;
                }
                else if (graphSettings.YMaxExp < YMinMaxExpLowerBound)
                {
                    graphSettings.YMaxExp = YMinMaxExpLowerBound;
                }
                if (graphSettings.YMinExp > YMinMaxExpUpperBound)
                {
                    graphSettings.YMinExp = YMinMaxExpUpperBound;
                }
                else if (graphSettings.YMinExp < YMinMaxExpLowerBound)
                {
                    graphSettings.YMinExp = YMinMaxExpLowerBound;
                }

                if (graphSettings.YMinExp > graphSettings.YMaxExp)
                {
                    graphSettings.YMaxExp = -3;
                    graphSettings.YMinExp = -6;

                }
                // Assign the modified GraphSettings back to obj
                data = graphSettings;
            }

            // Create the XmlSerializer
            XmlSerializer serializer = new XmlSerializer(data.GetType());
            // Serialize the data
            serializer.Serialize(writer, data, ns);
            return true;
        }

        /// <summary>
        /// Creates an XML string containing an unmerged view of the
        /// <see cref="System.Configuration.ConfigurationSection">ConfigurationSection</see>
        ///  object as a single section to write to a file.
        /// </summary>
        /// <param name="parentElement">The
        /// <see cref="System.Configuration.ConfigurationElement">ConfigurationElement</see>
        /// instance to use as the parent when performing the un-merge.</param>
        /// <param name="name">The name of the section to create.</param>
        /// <param name="saveMode">The
        /// <see cref="System.Configuration.ConfigurationSaveMode">ConfigurationSaveMode</see>
        /// instance to use when writing to a string.</param>
        /// <returns></returns>
        protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
        {
            StringWriter sWriter = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
            XmlTextWriter xWriter = new XmlTextWriter(sWriter);
            xWriter.Formatting = Formatting.Indented;
            xWriter.Indentation = 4;
            xWriter.IndentChar = ' ';

            this.SerializeToXmlElement(xWriter, name);

            xWriter.Flush();

            return sWriter.ToString();
        }

        /// <summary>
        /// Writes the outer tags of this configuration element to the configuration
        /// file when implemented in a derived class.
        /// </summary>
        /// <param name="writer">The
        /// <see cref="System.Xml.XmlWriter">XmlWriter</see>
        /// that writes to the configuration file.</param>
        /// <param name="elementName">The name of the
        /// <see cref="System.Configuration.ConfigurationElement">ConfigurationElement</see>
        /// to be written.</param>
        /// <returns></returns>
        protected override bool SerializeToXmlElement(System.Xml.XmlWriter writer, string elementName)
        {
            if (writer == null) return false;
            bool success = true;
            writer.WriteStartElement(elementName);
            success = SerializeElement(writer, false);
            writer.WriteEndElement();
            return success;
        }

        #endregion Protected Methods

        #endregion Methods
    }
}