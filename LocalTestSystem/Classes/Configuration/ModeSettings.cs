using System;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace LocalTestSystem.Classes.Configuration
{
    /// <summary>
    /// COMMON MODE parameters
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [SettingsProvider(typeof(AllUsersSettingsProvider))]
    [XmlRoot("ModeSettings")]
    public class ModeSettings : EditCycleApplicationSettingsBase
    {


        #region common
        #region Trace_Level : EnumParameter<TraceLevel>
        [UserScopedSetting()]
        [XmlElement("EnumParameter<TraceLevel>")]
        [DefaultSettingValue(@"
			<EnumParameterOfTraceLevel xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
				xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                <DisplayName>Trace Level</DisplayName>
				<ProcessValue>Error</ProcessValue>
				<ToolTip>Determines the level of detail created in the diagnostic output trace for the system.  Default level is Info.  Please change only at the request of VTI.</ToolTip>
			</EnumParameterOfTraceLevel>
        ")]
        public EnumParameter<VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel> Trace_Level
        {
            get
            {
                return ((EnumParameter<VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel>)this["Trace_Level"]);
            }
            set
            {
                this["Trace_Level"] = (EnumParameter<VTIWindowsControlLibrary.Classes.SystemDiagnostics.TraceLevel>)value;
            }
        }
        #endregion

        #region BarcodeScannerEnabled : BooleanParameter
        [UserScopedSetting()]
        [XmlElement("BooleanParameter")]
        [DefaultSettingValue(@"
            <BooleanParameter>
                <DisplayName>Enable Barcode Scanner</DisplayName>
                <ProcessValue>true</ProcessValue>
                <ToolTip>Enables or disables barcode scanner. Requires application restart.</ToolTip>
            </BooleanParameter>
        ")]
        public BooleanParameter BarcodeScannerEnabled
        {
            get
            {
                return ((BooleanParameter)this["BarcodeScannerEnabled"]);
            }
            set
            {
                this["BarcodeScannerEnabled"] = (BooleanParameter)value;
            }
        }
        #endregion

        #region DigitalOutputInterlocks : BooleanParameter
        [UserScopedSetting()]
        [XmlElement("BooleanParameter")]
        [DefaultSettingValue(@"
            <BooleanParameter>
                <DisplayName>Digital Output Interlocks</DisplayName>
                <ProcessValue>true</ProcessValue>
                <ToolTip>Enables or disables the Digital Output Interlocks.  The Digital Output Interlocks prevent the system from changing the state of a digital output if it will result in an undesirable or unsafe machine state.  The interlocks can be disabled if necessary for system diagnostics.</ToolTip>
            </BooleanParameter>
        ")]
        public BooleanParameter DigitalOutputInterlocks
        {
            get
            {
                return ((BooleanParameter)this["DigitalOutputInterlocks"]);
            }
            set
            {
                this["DigitalOutputInterlocks"] = (BooleanParameter)value;
            }
        }
        #endregion

        #region ShowCycleSteps : BooleanParameter
        [UserScopedSetting()]
        [XmlElement("BooleanParameter")]
        [DefaultSettingValue(@"
            <BooleanParameter>
                <DisplayName>Display Cycle Steps Form</DisplayName>
                <ProcessValue>false</ProcessValue>
                <ToolTip>Displays the active cycle steps form for debugging software.</ToolTip>
            </BooleanParameter>
        ")]
        public BooleanParameter ShowCycleSteps
        {
            get
            {
                return ((BooleanParameter)this["ShowCycleSteps"]);
            }
            set
            {
                this["ShowCycleSteps"] = (BooleanParameter)value;
            }
        }
        #endregion

       
        #region RemoteVTIDataconnection2Enable : BooleanParameter

        [UserScopedSetting()]
        [XmlElement("BooleanParameter")]
        [DefaultSettingValue(@"
            <BooleanParameter>
                <DisplayName>Remote VtiData Connection Enable</DisplayName>
                <ProcessValue>false</ProcessValue>
                <ToolTip>Enables or disables second connection to remote database containing tables dbo.UutRecords, dbo.UutRecordDetails, dbo.ParamChangeLog, and dbo.ManualCmdExecLog.</ToolTip>
            </BooleanParameter>
        ")]
        public BooleanParameter RemoteVTIDataconnection2Enable
        {
            get
            {
                return ((BooleanParameter)this["RemoteVTIDataconnection2Enable"]);
            }
            set
            {
                this["RemoteVTIDataconnection2Enable"] = (BooleanParameter)value;
            }
        }

		#endregion RemoteVTIDataconnection2Enable : BooleanParameter

		#endregion

		#region DatabaseMode : EnumParameter<DatabaseOptions>

		[UserScopedSetting()]
		[XmlElement("EnumParameter<DatabaseOptions>")]
		[DefaultSettingValue(@"
			<EnumParameterOfDatabaseOptions xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
				xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
        <DisplayName>Database Mode</DisplayName>
				<ProcessValue>Local</ProcessValue>
				<ToolTip>Database Mode</ToolTip>
			</EnumParameterOfDatabaseOptions>
        ")]
		public EnumParameter<DatabaseOptions> DatabaseMode
		{
			get
			{
				return ((EnumParameter<DatabaseOptions>)this["DatabaseMode"]);
			}
			set
			{
				this["DatabaseMode"] = (EnumParameter<DatabaseOptions>)value;
			}
		}
		#endregion
	}
}
