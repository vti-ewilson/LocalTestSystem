using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Interfaces;

namespace LocalTestSystem.Classes.Configuration
{
    /// <summary>
    /// Model DEFAULT Parameters
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [SettingsProvider(typeof(AllUsersSettingsProvider))]
    [XmlRoot("ModelSettings")]
    public class ModelSettings : ModelSettingsBase, IModelSettings
    {
		#region Language : EnumParameter<Languages>
		[UserScopedSetting()]
		[XmlElement("EnumParameter<Languages>")]
		[DefaultSettingValue(@"
			<EnumParameterOfLanguages xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
				xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
        <DisplayName>Language</DisplayName>
				<ProcessValue>Spanish</ProcessValue>
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
		#endregion


		#region Final_Evac_Delay : TimeDelayParameter
		[UserScopedSetting()]
		[XmlElement("TimeDelayParameter")]
		[DefaultSettingValue(@"
            <TimeDelayParameter>
                <DisplayName>Final Evac Delay</DisplayName>
                <ProcessValue>110</ProcessValue>
                <MinValue>0</MinValue>
                <MaxValue>6000</MaxValue>
                <SmallStep>0.1</SmallStep>
                <LargeStep>10</LargeStep>
                <Units>Seconds</Units>
                <ToolTip>If the vacuum gauge pressure is greater then the Final_Evac_Pressure_Setpoint at the end of the Final_Evac_Delay the part fails, possibly indicating a leak.</ToolTip>
            </TimeDelayParameter>
        ")]
		public TimeDelayParameter Final_Evac_Delay
		{
			get
			{
				return ((TimeDelayParameter)this["Final_Evac_Delay"]);
			}
			set
			{
				this["Final_Evac_Delay"] = (TimeDelayParameter)value;
			}
		}
		#endregion

		
	}
}
