using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace LocalTestSystem.Classes.Configuration
{
    /// <summary>
    /// COMMON TIME parameters
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [SettingsProvider(typeof(AllUsersSettingsProvider))]
    [XmlRoot("TimeSettings")]
    public class TimeSettings : EditCycleApplicationSettingsBase
    {

		#region RecoveryEvacTimeout : TimeDelayParameter
		//[UserScopedSetting()]
		//[XmlElement("TimeDelayParameter")]
		//[DefaultSettingValue(@"
		//          <TimeDelayParameter>
		//              <DisplayName>Recovery Evacuation Timeout</DisplayName>
		//              <ProcessValue>30</ProcessValue>
		//              <MinValue>1</MinValue>
		//              <MaxValue>3600</MaxValue>
		//              <SmallStep>1</SmallStep>
		//              <LargeStep>10</LargeStep>
		//              <Units>Seconds</Units>
		//              <ToolTip>Recovery tank maximum evacution time prior to supply.</ToolTip>
		//          </TimeDelayParameter>
		//      ")]
		//public TimeDelayParameter RecoveryEvacTimeout {
		//	get {
		//		return ((TimeDelayParameter)this["RecoveryEvacTimeout"]);
		//	}
		//	set {
		//		this["RecoveryEvacTimeout"] = (TimeDelayParameter)value;
		//	}
		//}
		#endregion


	}
}
