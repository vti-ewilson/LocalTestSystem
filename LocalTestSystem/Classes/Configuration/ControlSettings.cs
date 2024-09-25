using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary.Classes.Configuration;
using Newtonsoft.Json;

namespace LocalTestSystem.Classes.Configuration
{
    /// <summary>
    /// COMMON CONTROL parameters
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [SettingsProvider(typeof(AllUsersSettingsProvider))]
    [XmlRoot("ControlSettings")]
    public class ControlSettings : EditCycleApplicationSettingsBase
    {


        #region System_ID : StringParameter
        [UserScopedSetting()]
        [XmlElement("StringParameter")]
        [DefaultSettingValue(@"
            <StringParameter>
                <DisplayName>System ID</DisplayName>
                <ProcessValue>Recovery</ProcessValue>
                <ToolTip>Unique Name for this Station, used to denote tests performed on this machine in the Machine Records and UUT Records databases.</ToolTip>
            </StringParameter>
        ")]
        public StringParameter System_ID
        {
            get
            {
                return ((StringParameter)this["System_ID"]);
            }
            set
            {
                this["System_ID"] = (StringParameter)value;
            }
        }
		#endregion

		#region LastSyncedSettings : StringParameter
		[UserScopedSetting()]
		[JsonIgnore]
		[XmlElement("StringParameter")]
		[DefaultSettingValue(@"
            <StringParameter>
                <DisplayName>Last Synced Settings</DisplayName>
                <ProcessValue></ProcessValue>
                <ToolTip>(Hidden Parameter) timestamp for last remote read, used to for configuration management.</ToolTip>
            </StringParameter>
        ")]
		public StringParameter LastSyncedSettings
		{
			get
			{
				return ((StringParameter)this["LastSyncedSettings"]);
			}
			set
			{
				this["LastSyncedSettings"] = (StringParameter)value;
			}
		}
		#endregion


		#region ParameterSyncInterval : TimeDelayParameter
		[UserScopedSetting()]
		[XmlElement("TimeDelayParameter")]
		[DefaultSettingValue(@"
		          <TimeDelayParameter>
		              <DisplayName>Parameter Sync Interval</DisplayName>
		              <ProcessValue>30</ProcessValue>
		              <MinValue>1</MinValue>
		              <MaxValue>3600</MaxValue>
		              <SmallStep>1</SmallStep>
		              <LargeStep>10</LargeStep>
		              <Units>Seconds</Units>
		              <ToolTip>How often to check remote database for changes to common parameters</ToolTip>
		          </TimeDelayParameter>
		      ")]
		public TimeDelayParameter ParameterSyncInterval
		{
			get
			{
				return ((TimeDelayParameter)this["ParameterSyncInterval"]);
			}
			set
			{
				this["ParameterSyncInterval"] = (TimeDelayParameter)value;
			}
		}
		#endregion

		#region UutRecordTestType : StringParameter
		[UserScopedSetting()]
        [XmlElement("StringParameter")]
        [DefaultSettingValue(@"
            <StringParameter>
                <DisplayName>UUT Record Test Type</DisplayName>
                <ProcessValue>Recovery</ProcessValue>
                <ToolTip>Value to use for the 'Test Type' field of the UUT Records in the database.</ToolTip>
            </StringParameter>
        ")]
        public StringParameter UutRecordTestType
        {
            get
            {
                return ((StringParameter)this["UutRecordTestType"]);
            }
            set
            {
                this["UutRecordTestType"] = (StringParameter)value;
            }
        }
        #endregion

        //#region SerialNumberPattern : StringParameter
        //[UserScopedSetting()]
        //[XmlElement("StringParameter")]
        //[DefaultSettingValue(@"
        //    <StringParameter>
        //        <DisplayName>Serial Number Pattern</DisplayName>
        //        <ProcessValue>.*</ProcessValue>
        //        <ToolTip>Pattern that the serial number must match.  This is a 'regular expression' pattern.  Default value of '.*' matches anything.  A pattern of '.{X}' where X is a number would match any serial number X characters long.  For further assistance, contact VTI.</ToolTip>
        //    </StringParameter>
        //")]
        //public StringParameter SerialNumberPattern
        //{
        //    get
        //    {
        //        return ((StringParameter)this["SerialNumberPattern"]);
        //    }
        //    set
        //    {
        //        this["SerialNumberPattern"] = (StringParameter)value;
        //    }
        //}
        //#endregion

        //#region ModelNumberPattern : StringParameter
        //[UserScopedSetting()]
        //[XmlElement("StringParameter")]
        //[DefaultSettingValue(@"
        //    <StringParameter>
        //        <DisplayName>Model Number Pattern</DisplayName>
        //        <ProcessValue>.*</ProcessValue>
        //        <ToolTip>Pattern that the model number must match.  This is a 'regular expression' pattern.  Default value of '.*' matches anything.  A pattern of '.{X}' where X is a number would match any model number X characters long.  For further assistance, contact VTI.</ToolTip>
        //    </StringParameter>
        //")]
        //public StringParameter ModelNumberPattern
        //{
        //    get
        //    {
        //        return ((StringParameter)this["ModelNumberPattern"]);
        //    }
        //    set
        //    {
        //        this["ModelNumberPattern"] = (StringParameter)value;
        //    }
        //}
        //#endregion

        //#region ScannerPort : SerialPortParameter
        //[UserScopedSetting()]
        //[XmlElement("SerialPortParameter")]
        //[DefaultSettingValue(@"
        //  <SerialPortParameter>
        //    <DisplayName>Scanner Serial Port</DisplayName>
        //    <ProcessValue>
	       //     <PortName>COM3</PortName>
        //        <BaudRate>19200</BaudRate>
        //        <Parity>None</Parity>
        //        <DataBits>8</DataBits>
        //        <StopBits>One</StopBits>
        //        <Handshake>None</Handshake>
        //    </ProcessValue>
        //    <ToolTip>Serial Port for Barcode Scanner</ToolTip>
        //  </SerialPortParameter>
        //")]
        //public SerialPortParameter ScannerPort
        //{
        //    get
        //    {
        //        return ((SerialPortParameter)this["ScannerPort"]);
        //    }
        //    set
        //    {
        //        this["ScannerPort"] = (SerialPortParameter)value;
        //    }
        //}
        //#endregion

        #region Language : EnumParameter<Languages>

        [UserScopedSetting()]
        [XmlElement("EnumParameter<Languages>")]
        [DefaultSettingValue(@"
			<EnumParameterOfLanguages xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
				xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
        <DisplayName>Language</DisplayName>
				<ProcessValue>English</ProcessValue>
				<ToolTip>Language to use to display on-screen prompts.</ToolTip>
			</EnumParameterOfLanguages>
        ")]
        public EnumParameter<Languages> Language
        {
            get
            {
                return ((EnumParameter<Languages>)this["Language"]);
            }
            set
            {
                this["Language"] = (EnumParameter<Languages>)value;
            }
        }

        #endregion Language : EnumParameter<Languages>

        #region VtiConnectionString2 : StringParameter

        [UserScopedSetting()]
        [XmlElement("StringParameter")]
        [DefaultSettingValue(@"
            <StringParameter>
                <DisplayName>Remote Connection String VTI Data</DisplayName>
                <ProcessValue>Server=ServerName;Database=DatabaseName;User Id=user;Password=pass;</ProcessValue>
                <ToolTip>Connection string to remote database containing dbo.UutRecords, dbo.UutRecordDetails, dbo.ParamChangeLog, and dbo.ManualCmdExecLog.</ToolTip>
            </StringParameter>
        ")]
        public StringParameter VtiConnectionString2
        {
            get
            {
                return ((StringParameter)this["VtiConnectionString2"]);
            }
            set
            {
                this["VtiConnectionString2"] = (StringParameter)value;
            }
        }

		#endregion VtiConnectionString2 : StringParameter

		#region VtiConnectionString3 : StringParameter

		[UserScopedSetting()]
		[XmlElement("StringParameter")]
		[DefaultSettingValue(@"
            <StringParameter>
                <DisplayName>Connection String for Remote VtiData</DisplayName>
                <ProcessValue>Server=ServerName;Database=DatabaseName;User Id=user;Password=pass;</ProcessValue>
                <ToolTip>Connection string to remote VtiData database.</ToolTip>
            </StringParameter>
        ")]
		public StringParameter VtiConnectionString3
		{
			get
			{
				return ((StringParameter)this["VtiConnectionString3"]);
			}
			set
			{
				this["VtiConnectionString3"] = (StringParameter)value;
			}
		}

		#endregion VtiConnectionString3 : StringParameter
	}
}
