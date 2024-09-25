using System;
using VTIWindowsControlLibrary.Classes;

namespace VTIWindowsControlLibrary.Interfaces
{
    /// <summary>
    /// Interface which must be implemented by the ModelSettings class of the client application.
    /// </summary>
    public interface IModelSettings
    {
        /// <summary>
        /// Loads the settings for the specified model number.
        /// </summary>
        /// <param name="modelNo">The model number.</param>
        /// <returns></returns>
        bool Load(string modelNo);

        /// <summary>
        /// Loads the settings from the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void LoadFrom(ModelSettingsBase model);

        /// <summary>
        /// Saves the model settings.
        /// </summary>
        void Save();

        /// <summary>
        /// Upgrades the model settings.
        /// </summary>
        void Upgrade();

        /// <summary>
        /// Occurs when the model is loaded.
        /// </summary>
        event EventHandler Loaded;
    }
}