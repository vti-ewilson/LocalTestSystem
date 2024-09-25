namespace VTIWindowsControlLibrary.Classes.Util
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Management;

    /// <summary>
    /// Represents the system event log
    /// </summary>
    /// <remarks>
    /// Functions ShouldSerialize&lt;PropertyName&gt; are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is&lt;PropertyName&gt;Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    /// Functions Is&lt;PropertyName&gt;Null() are used to check if a property is NULL.
    /// Functions Reset&lt;PropertyName&gt; are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    /// Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    /// Datetime conversion functions ToDateTime and ToDmtfDateTime are added to the class to convert DMTF datetime to System.DateTime and vice-versa.
    /// An Early Bound class generated for the WMI class.Win32_NTLogEvent
    /// </remarks>
    public class NTLogEvent : System.ComponentModel.Component
    {
        // Private property to hold the WMI namespace in which the class resides.
        private static string CreatedWmiNamespace = "root\\CIMV2";

        // Private property to hold the name of WMI class which created this class.
        private static string CreatedClassName = "Win32_NTLogEvent";

        // Private member variable to hold the ManagementScope which is used by the various methods.
        private static System.Management.ManagementScope statMgmtScope = null;

        private ManagementSystemProperties PrivateSystemProperties;

        // Underlying lateBound WMI object.
        private System.Management.ManagementObject PrivateLateBoundObject;

        // Member variable to store the 'automatic commit' behavior for the class.
        private bool AutoCommitProp;

        // Private variable to hold the embedded property representing the instance.
        private System.Management.ManagementBaseObject embeddedObj;

        // The current WMI object used
        private System.Management.ManagementBaseObject curObj;

        // Flag to indicate if the instance is an embedded object.
        private bool isEmbedded;

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        public NTLogEvent()
        {
            this.InitializeObject(null, null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="keyLogfile"></param>
        /// <param name="keyRecordNumber"></param>
        public NTLogEvent(string keyLogfile, uint keyRecordNumber)
        {
            this.InitializeObject(null, new System.Management.ManagementPath(NTLogEvent.ConstructPath(keyLogfile, keyRecordNumber)), null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="mgmtScope"></param>
        /// <param name="keyLogfile"></param>
        /// <param name="keyRecordNumber"></param>
        public NTLogEvent(System.Management.ManagementScope mgmtScope, string keyLogfile, uint keyRecordNumber)
        {
            this.InitializeObject(((System.Management.ManagementScope)(mgmtScope)), new System.Management.ManagementPath(NTLogEvent.ConstructPath(keyLogfile, keyRecordNumber)), null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="path"></param>
        /// <param name="getOptions"></param>
        public NTLogEvent(System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions)
        {
            this.InitializeObject(null, path, getOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="mgmtScope"></param>
        /// <param name="path"></param>
        public NTLogEvent(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path)
        {
            this.InitializeObject(mgmtScope, path, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="path"></param>
        public NTLogEvent(System.Management.ManagementPath path)
        {
            this.InitializeObject(null, path, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="mgmtScope"></param>
        /// <param name="path"></param>
        /// <param name="getOptions"></param>
        public NTLogEvent(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions)
        {
            this.InitializeObject(mgmtScope, path, getOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="theObject"></param>
        public NTLogEvent(System.Management.ManagementObject theObject)
        {
            Initialize();
            if ((CheckIfProperClass(theObject) == true))
            {
                PrivateLateBoundObject = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
                curObj = PrivateLateBoundObject;
            }
            else
            {
                throw new System.ArgumentException("Class name does not match.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <param name="theObject"></param>
        public NTLogEvent(System.Management.ManagementBaseObject theObject)
        {
            Initialize();
            embeddedObj = theObject;
            // PrivateSystemProperties = new ManagementSystemProperties(theObject);
            curObj = embeddedObj;
            isEmbedded = true;
        }

        /// <summary>
        /// Gets the namespace of the WMI class.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace
        {
            get
            {
                return "root\\CIMV2";
            }
        }

        /// <summary>
        /// Gets the Name of the ManagementClass
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName
        {
            get
            {
                string strRet = CreatedClassName;
                if ((curObj != null))
                {
                    if ((curObj.ClassPath != null))
                    {
                        strRet = ((string)(curObj["__CLASS"]));
                        if (string.IsNullOrEmpty(strRet))
                        {
                            strRet = CreatedClassName;
                        }
                    }
                }
                return strRet;
            }
        }

        /// <summary>
        /// Gets an embedded object pointing to get System properties of the WMI object.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementSystemProperties SystemProperties
        {
            get
            {
                return PrivateSystemProperties;
            }
        }

        /// <summary>
        /// Gets the underlying lateBound object.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementBaseObject LateBoundObject
        {
            get
            {
                return curObj;
            }
        }

        /// <summary>
        /// Gets the ManagementScope of the object.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementScope Scope
        {
            get
            {
                if ((isEmbedded == false))
                {
                    return PrivateLateBoundObject.Scope;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if ((isEmbedded == false))
                {
                    PrivateLateBoundObject.Scope = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate the commit behavior for the WMI object.
        /// If true, WMI object will be automatically saved after each property modification.
        /// (ie. Put() is called after modification of a property).
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit
        {
            get
            {
                return AutoCommitProp;
            }
            set
            {
                AutoCommitProp = value;
            }
        }

        /// <summary>
        /// Gets or sets the ManagementPath of the underlying WMI object.
        /// </summary>
        [Browsable(true)]
        public System.Management.ManagementPath Path
        {
            get
            {
                if ((isEmbedded == false))
                {
                    return PrivateLateBoundObject.Path;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if ((isEmbedded == false))
                {
                    if ((CheckIfProperClass(null, value, null) != true))
                    {
                        throw new System.ArgumentException("Class name does not match.");
                    }
                    PrivateLateBoundObject.Path = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the scope property which is used by the various methods.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static System.Management.ManagementScope StaticScope
        {
            get
            {
                return statMgmtScope;
            }
            set
            {
                statMgmtScope = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Category is null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCategoryNull
        {
            get
            {
                if ((curObj["Category"] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value specifying a subcategory for this event. This subcategory is source specific.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Specifies a subcategory for this event. This subcategory is source specific.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort Category
        {
            get
            {
                if ((curObj["Category"] == null))
                {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["Category"]));
            }
        }

        /// <summary>
        /// Gets a value specifying the translation of the subcategory. The translation is source specific." +
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Specifies the translation of the subcategory. The translation is source specific." +
            "")]
        public string CategoryString
        {
            get
            {
                return ((string)(curObj["CategoryString"]));
            }
        }

        /// <summary>
        /// Gets the name of the computer that generated this event.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The variable-length null-terminated string specifying the name of the computer th" +
            "at generated this event.")]
        public string ComputerName
        {
            get
            {
                return ((string)(curObj["ComputerName"]));
            }
        }

        /// <summary>
        /// Gets the binary data that accompanied the report of the NT event.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The binary data that accompanied the report of the NT event.")]
        public byte[] Data
        {
            get
            {
                return ((byte[])(curObj["Data"]));
            }
        }

        /// <summary>
        /// Gets a value indicating if the EventCode is null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEventCodeNull
        {
            get
            {
                if ((curObj["EventCode"] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the EventCode for the event
        /// </summary>
        /// <remarks>
        /// This property has the value of the lower 16-bits of the
        /// <see cref="EventIdentifier">EventIdentifier</see> property.
        /// It is present to match the value displayed in the NT Event Viewer.
        /// NOTE: Two events from the same source may have the same value for this
        /// property but may have different severity and
        /// <see cref="EventIdentifier">EventIdentifier</see> values.
        /// </remarks>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"This property has the value of the lower 16-bits of the EventIdentifier property. It is present to match the value displayed in the NT Event Viewer. NOTE: Two events from the same source may have the same value for this property but may have different severity and EventIdentifier values")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort EventCode
        {
            get
            {
                if ((curObj["EventCode"] == null))
                {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["EventCode"]));
            }
        }

        /// <summary>
        /// Gets a value indicating if the EventIdentifier is null.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEventIdentifierNull
        {
            get
            {
                if ((curObj["EventIdentifier"] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the EventIdentifier
        /// </summary>
        /// <remarks>
        /// Identifies the event. This is specific to the source that generated the event log
        /// entry, and is used, together with SourceName, to uniquely identify an NT event
        /// type.
        /// </remarks>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Identifies the event. This is specific to the source that generated the event log" +
            " entry, and is used, together with SourceName, to uniquely identify an NT event " +
            "type.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint EventIdentifier
        {
            get
            {
                if ((curObj["EventIdentifier"] == null))
                {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["EventIdentifier"]));
            }
        }

        /// <summary>
        /// Gets a value indicating if the EventType is null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEventTypeNull
        {
            get
            {
                if ((curObj["EventType"] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value that specifies the type of event.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Type property specifies the type of event.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public EventTypeValues EventType
        {
            get
            {
                if ((curObj["EventType"] == null))
                {
                    return ((EventTypeValues)(System.Convert.ToInt32(0)));
                }
                return ((EventTypeValues)(System.Convert.ToInt32(curObj["EventType"])));
            }
        }

        /// <summary>
        /// Gets an array of insertion strings that accompanied the report of the NT event.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The insertion strings that accompanied the report of the NT event.")]
        public string[] InsertionStrings
        {
            get
            {
                return ((string[])(curObj["InsertionStrings"]));
            }
        }

        /// <summary>
        /// Gets the name of the NT Eventlog logfile.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The name of NT Eventlog logfile. This is used together with the RecordNumber to u" +
            "niquely identify an instance of this class.")]
        public string Logfile
        {
            get
            {
                return ((string)(curObj["Logfile"]));
            }
        }

        /// <summary>
        /// Gets the message as it appears in the NT Eventlog.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The event message as it appears in the NT Eventlog. This is a standard message with zero or more insertion strings supplied by the source of the NT event. The insertion strings are inserted into the standard message in a predefined format. If there are no insertion strings or there is a problem inserting the insertion strings, only the standard message will be present in this field.")]
        public string Message
        {
            get
            {
                return ((string)(curObj["Message"]));
            }
        }

        /// <summary>
        /// Gets a value indicating if the RecordNumber is null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRecordNumberNull
        {
            get
            {
                if ((curObj["RecordNumber"] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value that identifies the event within the NT Eventlog logfile.
        /// </summary>
        /// <remarks>
        /// This is specific to the logfile and is used together with the logfile name
        /// to uniquely identify an instance of this class.
        /// </remarks>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Identifies the event within the NT Eventlog logfile. This is specific to the logf" +
            "ile and is used together with the logfile name to uniquely identify an instance " +
            "of this class.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint RecordNumber
        {
            get
            {
                if ((curObj["RecordNumber"] == null))
                {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["RecordNumber"]));
            }
        }

        /// <summary>
        /// Gets a value specifying the name of the source (application, service,
        /// driver, subsystem) that generated the entry.
        /// </summary>
        /// <remarks>
        /// It is used, together with the EventIdentifier, to uniquely identify an NT event type.
        /// </remarks>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The variable-length null-terminated string specifying the name of the source (app" +
            "lication, service, driver, subsystem) that generated the entry. It is used, toge" +
            "ther with the EventIdentifier, to uniquely identify an NT event type.")]
        public string SourceName
        {
            get
            {
                return ((string)(curObj["SourceName"]));
            }
        }

        /// <summary>
        /// Gets a value indicating if the TimeGenerated is null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTimeGeneratedNull
        {
            get
            {
                if ((curObj["TimeGenerated"] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value specifying the time at which the source generated the event.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Specifies the time at which the source generated the event.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public System.DateTime TimeGenerated
        {
            get
            {
                if ((curObj["TimeGenerated"] != null))
                {
                    return WMIUtil.ToDateTime(((string)(curObj["TimeGenerated"])));
                }
                else
                {
                    return System.DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating if the TimeWritten is null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTimeWrittenNull
        {
            get
            {
                if ((curObj["TimeWritten"] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value specifying the time at which the event was written to the logfile.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Specifies the time at which the event was written to the logfile.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public System.DateTime TimeWritten
        {
            get
            {
                if ((curObj["TimeWritten"] != null))
                {
                    return WMIUtil.ToDateTime(((string)(curObj["TimeWritten"])));
                }
                else
                {
                    return System.DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Gets a value specifying the type of event. This is an enumerated string.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Specifies the type of event. This is an enumerated string")]
        public string Type
        {
            get
            {
                return ((string)(curObj["Type"]));
            }
        }

        /// <summary>
        /// Gets the user name of the logged on user when the event ocurred.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The user name of the logged on user when the event ocurred. If the user name cann" +
            "ot be determined this will be NULL")]
        public string User
        {
            get
            {
                return ((string)(curObj["User"]));
            }
        }

        private bool CheckIfProperClass(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions OptionsParam)
        {
            if (((path != null)
                        && (string.Compare(path.ClassName, this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)))
            {
                return true;
            }
            else
            {
                return CheckIfProperClass(new System.Management.ManagementObject(mgmtScope, path, OptionsParam));
            }
        }

        private bool CheckIfProperClass(System.Management.ManagementBaseObject theObj)
        {
            if (((theObj != null)
                        && (string.Compare(((string)(theObj["__CLASS"])), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)))
            {
                return true;
            }
            else
            {
                System.Array parentClasses = ((System.Array)(theObj["__DERIVATION"]));
                if ((parentClasses != null))
                {
                    int count = 0;
                    for (count = 0; (count < parentClasses.Length); count = (count + 1))
                    {
                        if ((string.Compare(((string)(parentClasses.GetValue(count))), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool ShouldSerializeCategory()
        {
            if ((this.IsCategoryNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeEventCode()
        {
            if ((this.IsEventCodeNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeEventIdentifier()
        {
            if ((this.IsEventIdentifierNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeEventType()
        {
            if ((this.IsEventTypeNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeRecordNumber()
        {
            if ((this.IsRecordNumberNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeTimeGenerated()
        {
            if ((this.IsTimeGeneratedNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeTimeWritten()
        {
            if ((this.IsTimeWrittenNull == false))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Commits changes to the eventlog
        /// </summary>
        [Browsable(true)]
        public void CommitObject()
        {
            if ((isEmbedded == false))
            {
                PrivateLateBoundObject.Put();
            }
        }

        /// <summary>
        /// Commits changes to the eventlog
        /// </summary>
        /// <param name="putOptions">Specifies options for committing management object changes.</param>
        [Browsable(true)]
        public void CommitObject(System.Management.PutOptions putOptions)
        {
            if ((isEmbedded == false))
            {
                PrivateLateBoundObject.Put(putOptions);
            }
        }

        private void Initialize()
        {
            AutoCommitProp = true;
            isEmbedded = false;
        }

        private static string ConstructPath(string keyLogfile, uint keyRecordNumber)
        {
            string strPath = "root\\CIMV2:Win32_NTLogEvent";
            strPath = string.Concat(strPath, string.Concat(".Logfile=", string.Concat("\"", string.Concat(keyLogfile, "\""))));
            strPath = string.Concat(strPath, string.Concat(",RecordNumber=", ((System.UInt32)(keyRecordNumber)).ToString()));
            return strPath;
        }

        private void InitializeObject(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions)
        {
            Initialize();
            if ((path != null))
            {
                if ((CheckIfProperClass(mgmtScope, path, getOptions) != true))
                {
                    throw new System.ArgumentException("Class name does not match.");
                }
            }
            PrivateLateBoundObject = new System.Management.ManagementObject(mgmtScope, path, getOptions);
            PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
            curObj = PrivateLateBoundObject;
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances()
        {
            return GetInstances(null, null, null);
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <param name="condition">Condition matching the events to retrieve</param>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances(string condition)
        {
            return GetInstances(null, condition, null);
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <param name="selectedProperties">Properties matching the events to retrieve</param>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances(System.String[] selectedProperties)
        {
            return GetInstances(null, null, selectedProperties);
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <param name="condition">Condition matching the events to retrieve</param>
        /// <param name="selectedProperties">Properties matching the events to retrieve</param>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances(string condition, System.String[] selectedProperties)
        {
            return GetInstances(null, condition, selectedProperties);
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <param name="mgmtScope"></param>
        /// <param name="enumOptions"></param>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances(System.Management.ManagementScope mgmtScope, System.Management.EnumerationOptions enumOptions)
        {
            if ((mgmtScope == null))
            {
                if ((statMgmtScope == null))
                {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\CIMV2";
                }
                else
                {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementPath pathObj = new System.Management.ManagementPath();
            pathObj.ClassName = "Win32_NTLogEvent";
            pathObj.NamespacePath = "root\\CIMV2";
            System.Management.ManagementClass clsObject = new System.Management.ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null))
            {
                enumOptions = new System.Management.EnumerationOptions();
                enumOptions.EnsureLocatable = true;
            }
            return new NTLogEventCollection(clsObject.GetInstances(enumOptions));
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <param name="mgmtScope"></param>
        /// <param name="condition"></param>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition)
        {
            return GetInstances(mgmtScope, condition, null);
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <param name="mgmtScope"></param>
        /// <param name="selectedProperties"></param>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances(System.Management.ManagementScope mgmtScope, System.String[] selectedProperties)
        {
            return GetInstances(mgmtScope, null, selectedProperties);
        }

        /// <summary>
        /// Gets a <see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see>
        /// </summary>
        /// <param name="mgmtScope"></param>
        /// <param name="condition"></param>
        /// <param name="selectedProperties"></param>
        /// <returns><see cref="NTLogEventCollection">Collection</see> of <see cref="NTLogEvent">NTLogEvents</see></returns>
        public static NTLogEventCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition, System.String[] selectedProperties)
        {
            if ((mgmtScope == null))
            {
                if ((statMgmtScope == null))
                {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\CIMV2";
                }
                else
                {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementObjectSearcher ObjectSearcher = new System.Management.ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_NTLogEvent", condition, selectedProperties));
            System.Management.EnumerationOptions enumOptions = new System.Management.EnumerationOptions();
            enumOptions.EnsureLocatable = true;
            ObjectSearcher.Options = enumOptions;
            return new NTLogEventCollection(ObjectSearcher.Get());
        }

        /// <summary>
        /// Creates an instance of the <see cref="NTLogEvent">NTLogEvent</see> class
        /// </summary>
        /// <returns>New <see cref="NTLogEvent">NTLogEvent</see></returns>
        [Browsable(true)]
        public static NTLogEvent CreateInstance()
        {
            System.Management.ManagementScope mgmtScope = null;
            if ((statMgmtScope == null))
            {
                mgmtScope = new System.Management.ManagementScope();
                mgmtScope.Path.NamespacePath = CreatedWmiNamespace;
            }
            else
            {
                mgmtScope = statMgmtScope;
            }
            System.Management.ManagementPath mgmtPath = new System.Management.ManagementPath(CreatedClassName);
            System.Management.ManagementClass tmpMgmtClass = new System.Management.ManagementClass(mgmtScope, mgmtPath, null);
            return new NTLogEvent(tmpMgmtClass.CreateInstance());
        }

        /// <summary>
        /// Deletes this instance of the NTLogEvent
        /// </summary>
        [Browsable(true)]
        public void Delete()
        {
            PrivateLateBoundObject.Delete();
        }

        /// <summary>
        /// Categories for NTLogEvents
        /// </summary>
        public enum EventTypeValues
        {
            /// <summary>
            /// Indicates an Error event
            /// </summary>
            Error = 1,

            /// <summary>
            /// Indicates a Warning event
            /// </summary>
            Warning = 2,

            /// <summary>
            /// Indicates in Information event
            /// </summary>
            Information = 3,

            /// <summary>
            /// Indicates a Security Audit Success event
            /// </summary>
            Security_audit_success = 4,

            /// <summary>
            /// Indicates a Security Audit Failure event
            /// </summary>
            Security_audit_failure = 5,

            /// <summary>
            /// Indicates an event without a specified type
            /// </summary>
            NULL_ENUM_VALUE = 0,
        }

        /// <summary>
        /// Enumerator implementation for enumerating instances of the class.
        /// </summary>
        public class NTLogEventCollection : object, ICollection
        {
            private ManagementObjectCollection privColObj;

            /// <summary>
            /// Initializes a new instance of the <see cref="NTLogEventCollection">NTLogEventCollection</see>
            /// </summary>
            /// <param name="objCollection"></param>
            public NTLogEventCollection(ManagementObjectCollection objCollection)
            {
                privColObj = objCollection;
            }

            /// <summary>
            /// Gets a value indicating the number of objects in the collection.
            /// </summary>
            public virtual int Count
            {
                get
                {
                    return privColObj.Count;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the object is synchronized.
            /// </summary>
            public virtual bool IsSynchronized
            {
                get
                {
                    return privColObj.IsSynchronized;
                }
            }

            /// <summary>
            /// Gets this instance of the NTLogEventCollection
            /// </summary>
            public virtual object SyncRoot
            {
                get
                {
                    return this;
                }
            }

            /// <summary>
            /// Copies the collection to an array
            /// </summary>
            /// <param name="array">An array to copy to.</param>
            /// <param name="index">The index to start from.</param>
            public virtual void CopyTo(System.Array array, int index)
            {
                privColObj.CopyTo(array, index);
                int nCtr;
                for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1))
                {
                    array.SetValue(new NTLogEvent(((System.Management.ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }

            /// <summary>
            /// Returns the enumerator of the collection.
            /// </summary>
            /// <returns>Enumerator of the collection</returns>
            public virtual System.Collections.IEnumerator GetEnumerator()
            {
                return new NTLogEventEnumerator(privColObj.GetEnumerator());
            }

            /// <summary>
            /// Supports simple iteration over the NTLogEvents
            /// </summary>
            public class NTLogEventEnumerator : object, System.Collections.IEnumerator
            {
                private ManagementObjectCollection.ManagementObjectEnumerator privObjEnum;

                /// <summary>
                /// Initializes a new instance of the NTLogEventEnumerator
                /// </summary>
                /// <param name="objEnum"></param>
                public NTLogEventEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum)
                {
                    privObjEnum = objEnum;
                }

                /// <summary>
                /// Gets the current <see cref="System.Management.ManagementObject">system.Management.ManagementObject</see>
                /// that this enumerator points to.
                /// </summary>
                public virtual object Current
                {
                    get
                    {
                        return new NTLogEvent(((System.Management.ManagementObject)(privObjEnum.Current)));
                    }
                }

                /// <summary>
                /// Indicates whether the enumerator has moved to the next object in the enumeration.
                /// </summary>
                /// <returns>
                /// true, if the enumerator was successfully advanced to the next element; false
                /// if the enumerator has passed the end of the collection.
                /// </returns>
                public virtual bool MoveNext()
                {
                    return privObjEnum.MoveNext();
                }

                /// <summary>
                /// Resets the enumerator to the beginning of the collection.
                /// </summary>
                public virtual void Reset()
                {
                    privObjEnum.Reset();
                }
            }
        }

        /// <summary>
        /// TypeConverter to handle null values for <see cref="ValueType">ValueType</see> properties
        /// </summary>
        public class WMIValueTypeConverter : TypeConverter
        {
            private TypeConverter baseConverter;

            private System.Type baseType;

            /// <summary>
            /// Initializes a new instance of the <see cref="WMIValueTypeConverter">WMIValueTypeConverter</see> class
            /// </summary>
            /// <param name="inBaseType"></param>
            public WMIValueTypeConverter(System.Type inBaseType)
            {
                baseConverter = TypeDescriptor.GetConverter(inBaseType);
                baseType = inBaseType;
            }

            /// <summary>
            /// Returns whether this converter can convert an object of the given type to
            /// the type of this converter, using the specified context.
            /// </summary>
            /// <param name="context">An
            /// <see cref="System.ComponentModel.ITypeDescriptorContext">System.ComponentModel.ITypeDescriptorContext</see> that provides a format context.</param>
            /// <param name="srcType">A System.Type that represents the type you want to convert from.</param>
            /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type srcType)
            {
                return baseConverter.CanConvertFrom(context, srcType);
            }

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type,
            /// using the specified context.
            /// </summary>
            /// <param name="context">An
            /// <see cref="System.ComponentModel.ITypeDescriptorContext">System.ComponentModel.ITypeDescriptorContext</see> that provides a format context.</param>
            /// <param name="destinationType">A System.Type that represents the type you want to convert to.</param>
            /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
            {
                return baseConverter.CanConvertTo(context, destinationType);
            }

            /// <summary>
            /// Converts the given value to the type of this converter.
            /// </summary>
            /// <param name="context">An
            /// <see cref="System.ComponentModel.ITypeDescriptorContext">System.ComponentModel.ITypeDescriptorContext</see> that provides a format context.</param>
            /// <param name="culture">The
            /// <see cref="System.Globalization.CultureInfo">System.Globalization.CultureInfo</see>
            /// to use as the current culture.</param>
            /// <param name="value">The System.Object to convert.</param>
            /// <returns></returns>
            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                return baseConverter.ConvertFrom(context, culture, value);
            }

            /// <summary>
            /// Creates an instance of the type that this
            /// <see cref="System.ComponentModel.TypeConverter">System.ComponentModel.TypeConverter</see>
            /// is associated with, using the specified context, given a set of property
            /// values for the object.
            /// </summary>
            /// <param name="context">An
            /// <see cref="System.ComponentModel.ITypeDescriptorContext">System.ComponentModel.ITypeDescriptorContext</see>
            /// that provides a format context.</param>
            /// <param name="dictionary">An
            /// <see cref="System.Collections.IDictionary">System.Collections.IDictionary</see>
            /// of new property values.</param>
            /// <returns>An System.Object representing the given System.Collections.IDictionary, or
            /// null if the object cannot be created. This method always returns null.</returns>
            public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary dictionary)
            {
                return baseConverter.CreateInstance(context, dictionary);
            }

            /// <summary>
            /// Returns whether changing a value on this object requires a call to System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)
            ///     to create a new value, using the specified context.
            /// </summary>
            /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
            /// <returns>true if changing a property on this object requires a call to System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)
            ///     to create a new value; otherwise, false.</returns>
            public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
            {
                return baseConverter.GetCreateInstanceSupported(context);
            }

            /// <summary>
            /// Returns a collection of properties for the type of array specified by the
            ///     value parameter, using the specified context and attributes.
            /// </summary>
            /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
            /// <param name="value">An System.Object that specifies the type of array for which to get properties.</param>
            /// <param name="attributeVar">An array of type System.Attribute that is used as a filter.</param>
            /// <returns>A System.ComponentModel.PropertyDescriptorCollection with the properties
            ///     that are exposed for this data type, or null if there are no properties.</returns>
            public override PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, System.Attribute[] attributeVar)
            {
                return baseConverter.GetProperties(context, value, attributeVar);
            }

            /// <summary>
            /// Returns whether this object supports properties, using the specified context.
            /// </summary>
            /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
            /// <returns>true if System.ComponentModel.TypeConverter.GetProperties(System.Object)
            ///     should be called to find the properties of this object; otherwise, false.</returns>
            public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context)
            {
                return baseConverter.GetPropertiesSupported(context);
            }

            /// <summary>
            /// Returns a collection of standard values for the data type this type converter
            ///     is designed for when provided with a format context.
            /// </summary>
            /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context
            ///     that can be used to extract additional information about the environment
            ///     from which this converter is invoked. This parameter or properties of this
            ///     parameter can be null.</param>
            /// <returns></returns>
            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
            {
                return baseConverter.GetStandardValues(context);
            }

            /// <summary>
            /// Returns whether the collection of standard values returned from System.ComponentModel.TypeConverter.GetStandardValues()
            ///     is an exclusive list of possible values, using the specified context.
            /// </summary>
            /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
            /// <returns>true if the System.ComponentModel.TypeConverter.StandardValuesCollection
            ///     returned from System.ComponentModel.TypeConverter.GetStandardValues() is
            ///     an exhaustive list of possible values; false if other values are possible.</returns>
            public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
            {
                return baseConverter.GetStandardValuesExclusive(context);
            }

            /// <summary>
            /// Returns whether this object supports a standard set of values that can be
            ///     picked from a list, using the specified context.
            /// </summary>
            /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
            /// <returns>true if System.ComponentModel.TypeConverter.GetStandardValues() should be
            ///     called to find a common set of values the object supports; otherwise, false.</returns>
            public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
            {
                return baseConverter.GetStandardValuesSupported(context);
            }

            /// <summary>
            /// Converts the given value object to the specified type, using the specified
            ///     context and culture information.
            /// </summary>
            /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
            /// <param name="culture">A System.Globalization.CultureInfo. If null is passed, the current culture
            ///     is assumed.</param>
            /// <param name="value">The System.Object to convert.</param>
            /// <param name="destinationType">The System.Type to convert the value parameter to.</param>
            /// <returns>An System.Object that represents the converted value.</returns>
            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
            {
                if ((baseType.BaseType == typeof(System.Enum)))
                {
                    if ((value.GetType() == destinationType))
                    {
                        return value;
                    }
                    if ((((value == null)
                                && (context != null))
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
                    {
                        return "NULL_ENUM_VALUE";
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((baseType == typeof(bool))
                            && (baseType.BaseType == typeof(System.ValueType))))
                {
                    if ((((value == null)
                                && (context != null))
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
                    {
                        return "";
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((context != null)
                            && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
                {
                    return "";
                }
                return baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }

        /// <summary>
        /// Embedded class to represent WMI system Properties.
        /// </summary>
        [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
        public class ManagementSystemProperties
        {
            private System.Management.ManagementBaseObject PrivateLateBoundObject;

            /// <summary>
            /// Initializes a new instance of the <see cref="ManagementSystemProperties">ManagementSystemProperties</see> class
            /// </summary>
            /// <param name="ManagedObject"></param>
            public ManagementSystemProperties(System.Management.ManagementBaseObject ManagedObject)
            {
                PrivateLateBoundObject = ManagedObject;
            }

            /// <summary>
            /// Gets the GENUS value
            /// </summary>
            [Browsable(true)]
            public int GENUS
            {
                get
                {
                    return ((int)(PrivateLateBoundObject["__GENUS"]));
                }
            }

            /// <summary>
            /// Gets the CLASS value
            /// </summary>
            [Browsable(true)]
            public string CLASS
            {
                get
                {
                    return ((string)(PrivateLateBoundObject["__CLASS"]));
                }
            }

            /// <summary>
            /// Gets the SUPERCLASS value
            /// </summary>
            [Browsable(true)]
            public string SUPERCLASS
            {
                get
                {
                    return ((string)(PrivateLateBoundObject["__SUPERCLASS"]));
                }
            }

            /// <summary>
            /// Gets the DYNASTY value
            /// </summary>
            [Browsable(true)]
            public string DYNASTY
            {
                get
                {
                    return ((string)(PrivateLateBoundObject["__DYNASTY"]));
                }
            }

            /// <summary>
            /// Gets the RELPATH value
            /// </summary>
            [Browsable(true)]
            public string RELPATH
            {
                get
                {
                    return ((string)(PrivateLateBoundObject["__RELPATH"]));
                }
            }

            /// <summary>
            /// Gets the PROPERTY_COUNT
            /// </summary>
            [Browsable(true)]
            public int PROPERTY_COUNT
            {
                get
                {
                    return ((int)(PrivateLateBoundObject["__PROPERTY_COUNT"]));
                }
            }

            /// <summary>
            /// Gets the DERIVATION value
            /// </summary>
            [Browsable(true)]
            public string[] DERIVATION
            {
                get
                {
                    return ((string[])(PrivateLateBoundObject["__DERIVATION"]));
                }
            }

            /// <summary>
            /// Gets the SERVER value
            /// </summary>
            [Browsable(true)]
            public string SERVER
            {
                get
                {
                    return ((string)(PrivateLateBoundObject["__SERVER"]));
                }
            }

            /// <summary>
            /// Gets the NAMESPACE value
            /// </summary>
            [Browsable(true)]
            public string NAMESPACE
            {
                get
                {
                    return ((string)(PrivateLateBoundObject["__NAMESPACE"]));
                }
            }

            /// <summary>
            /// Gets the PATH value
            /// </summary>
            [Browsable(true)]
            public string PATH
            {
                get
                {
                    return ((string)(PrivateLateBoundObject["__PATH"]));
                }
            }
        }
    }
}