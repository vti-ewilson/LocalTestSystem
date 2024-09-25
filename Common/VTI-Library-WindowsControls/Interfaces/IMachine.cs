using System.Management.Instrumentation;
using System.Resources;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Components;

namespace VTIWindowsControlLibrary.Interfaces
{
    /// <summary>
    /// Interface which must be implemented by the Machine class of the client application.
    /// </summary>
    public interface IMachine
    {
        /// <summary>
        /// Called when the config changes.
        /// </summary>
        void ConfigChanged(bool serialParamsChanged);

		/// <summary>
		/// Gets the localization instance.
		/// </summary>
		/// <value>The localization instance.</value>
		ResourceManager LocalizationInstance { get; }

        /// <summary>
        /// Gets the manual commands instance.
        /// </summary>
        /// <value>The manual commands instance.</value>
        IManualCommands ManualCommandsInstance { get; }

	}
}