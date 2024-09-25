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
    /// COMMON FLOW parameters
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [SettingsProvider(typeof(AllUsersSettingsProvider))]
    [XmlRoot("FlowSettings")]
    public class FlowSettings : EditCycleApplicationSettingsBase
    {
		#region FlowmeterCountsPerUnitMeasure : NumericParameter
		[UserScopedSetting()]
		[XmlElement("NumericParameter")]
		[DefaultSettingValue(@"
            <NumericParameter>
                <DisplayName>Flowmeter Counts Per Unit Measure</DisplayName>
                <ProcessValue>66</ProcessValue>
                <MinValue>0</MinValue>
                <MaxValue>200</MaxValue>
                <SmallStep>0.001</SmallStep>
                <LargeStep>1</LargeStep>
                <Units>counts</Units>
                <ToolTip>Flowmeter counts per unit measure scale factor.</ToolTip>
            </NumericParameter>
        ")]
		public NumericParameter FlowmeterCountsPerUnitMeasure {
			get {
				return ((NumericParameter)this["FlowmeterCountsPerUnitMeasure"]);
			}
			set {
				this["FlowmeterCountsPerUnitMeasure"] = (NumericParameter)value;
			}
		}
		#endregion
	}

}
