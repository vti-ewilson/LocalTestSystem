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
    /// COMMON PRESSURE parameters
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [SettingsProvider(typeof(AllUsersSettingsProvider))]
    [XmlRoot("PressureSettings")]
    public class PressureSettings : EditCycleApplicationSettingsBase
    {

		//#region RecoveryEvacSetpoint : NumericParameter
		//[UserScopedSetting()]
		//[XmlElement("NumericParameter")]
		//[DefaultSettingValue(@"
  //          <NumericParameter>
  //              <DisplayName>Recovery Evac Setpoint</DisplayName>
  //              <ProcessValue>5</ProcessValue>
  //              <MinValue>1</MinValue>
  //              <MaxValue>1000</MaxValue>
  //              <SmallStep>1</SmallStep>
  //              <LargeStep>10</LargeStep>
  //              <Units>Torr</Units>
  //              <ToolTip>Evacuation Target for recovery tank</ToolTip>
  //          </NumericParameter>
  //      ")]
		//public NumericParameter RecoveryEvacSetpoint {
		//	get {
		//		return ((NumericParameter)this["RecoveryEvacSetpoint"]);
		//	}
		//	set {
		//		this["RecoveryEvacSetpoint"] = (NumericParameter)value;
		//	}
		//}
		//#endregion


    }
}
