using System;
using System.Reflection;
using System.Linq;
using VTIWindowsControlLibrary.Forms;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents the <see cref="AboutBoxForm">About Box Form</see> of the client application
    /// </summary>
    /// /// <remarks>
    /// This class isn't the <see cref="AboutBoxForm">AboutBoxForm</see> itself, but contains a
    /// private static instance of it, and has static methods for accessing it.
    /// </remarks>
    public class AboutBox
    {
        private static AboutBoxForm aboutBoxForm;

        /// <summary>
        /// Initializes the static instance of the <see cref="AboutBox">AboutBox</see> class
        /// </summary>
        static AboutBox()
        {
            aboutBoxForm = new AboutBoxForm();
        }

        /// <summary>
        /// Shows the <see cref="AboutBoxForm">AboutBoxForm</see>
        /// </summary>
        public static void Show()
        {
            object[] attributes;

            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            // If there is at least one Title attribute
            if (attributes.Length > 0)
            {
                // Select the first one
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                // If it is not an empty string, return it
                if (!string.IsNullOrEmpty(titleAttribute.Title))
                    aboutBoxForm.Title = string.Format("About {0}", titleAttribute.Title);
            }

            // Get all Product attributes on this assembly
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length != 0)
                aboutBoxForm.ProductName = ((AssemblyProductAttribute)attributes[0]).Product;

            aboutBoxForm.Version = string.Format("Version {0}", Assembly.GetCallingAssembly().GetName().Version.ToString());
            aboutBoxForm.ControlLibVersion = string.Format("Control Library Version {0}", typeof(AboutBox).Assembly.GetName().Version);

            Assembly assemblyVersionPLC = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("VtiPLCInterface")).FirstOrDefault();
            if (assemblyVersionPLC != null)
            {
                aboutBoxForm.PLCLibVersion = string.Format("PLC Library Version {0}", assemblyVersionPLC.GetName().Version);
            }
            else
            {
                Assembly assemblyVersionMCC = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("VtiMccInterface")).FirstOrDefault();
                if (assemblyVersionMCC != null)
                {
                    aboutBoxForm.PLCLibVersion = string.Format("MCC Library Version {0}", assemblyVersionMCC.GetName().Version);
                }
                else
                {
                    aboutBoxForm.PLCLibVersion = "Error retrieving PLC/MCC Library version.";
                }
            }

            // Get all Copyright attributes on this assembly
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length != 0)
                aboutBoxForm.Copyright = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;

            // Get all Company attributes on this assembly
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length != 0)
                aboutBoxForm.Company = ((AssemblyCompanyAttribute)attributes[0]).Company;

            // Get all Description attributes on this assembly
            attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length != 0)
                aboutBoxForm.Description = ((AssemblyDescriptionAttribute)attributes[0]).Description;

            aboutBoxForm.ShowDialog();
        }

        private static string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return string.Empty;
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
    }
}