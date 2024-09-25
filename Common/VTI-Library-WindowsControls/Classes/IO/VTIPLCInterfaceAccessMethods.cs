using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VTIWindowsControlLibrary.Classes.IO
{
    public static class VTIPLCInterfaceAccessMethods
    {
        public static bool PLCEnabled()
        {
            bool PLCEnabled = true;
            // Load the assembly containing the settings
            Assembly vtiPlcInterfaceAssembly = Assembly.Load("VtiPLCInterface");
            // Get the SettingsProvider class type
            Type settingsProviderType = vtiPlcInterfaceAssembly.GetType("VtiPLCInterface.SettingsProvider");
            if (settingsProviderType != null)
            {
                // Get the property info for PLCEnabled
                PropertyInfo PLCEnabledProperty = settingsProviderType.GetProperty("PLCEnabled", BindingFlags.Static | BindingFlags.Public);

                if (PLCEnabledProperty != null)
                {
                    // Get the value of PLCEnabled
                    PLCEnabled = (bool)PLCEnabledProperty.GetValue(null);
                }
                else
                {
                    Console.WriteLine("PLCEnabled property not found.");
                }
            }
            else
            {
                Console.WriteLine("SettingsProvider type not found.");
            }

            return PLCEnabled;
        }
    }
}
