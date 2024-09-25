using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Interfaces;

namespace LocalTestSystem.Classes.Configuration
{
    /// <summary>
    /// IOSettings
    /// 
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    [XmlRoot("IOSettings")]
    public class IOSettings : ApplicationSettingsBase, IIOSettings
    {
        [UserScopedSetting()]
        [XmlElement("Interface")]
        [DefaultSettingValue(@"
            <IOInterface>
                <Name>MCC</Name>
                <InterfaceDLL>VtiPLCInterface.DLL</InterfaceDLL>
            </IOInterface>
        ")]
        public IOInterface Interface
        {
            get
            {
                return ((IOInterface)this["Interface"]);
            }
            set
            {
                this["Interface"] = (IOInterface)value;
            }
        }
    }

}
