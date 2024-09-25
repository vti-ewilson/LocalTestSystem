using System;
using System.Resources;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// Implementes a <see cref="ResourceManager">resource manager</see> which can chain
    /// to another resource manager when GetString() is called and the
    /// value can't be found in the current resource manager.
    /// </summary>
    public class ChainableResourceManager : ResourceManager
    {
        private ResourceManager _chainResourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainableResourceManager"/> class.
        /// </summary>
        /// <param name="resourceSource">A <see cref="T:System.Type"/> from which the <see cref="T:System.Resources.ResourceManager"/> derives all information for finding .resources files.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="resourceSource"/> parameter is null.
        /// </exception>
        public ChainableResourceManager(Type resourceSource)
            : base(resourceSource)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainableResourceManager"/> class.
        /// </summary>
        /// <param name="baseName">The root name of the resources. For example, the root name for the resource
        ///     file named "MyResource.en-US.resources" is "MyResource".</param>
        /// <param name="assembly">The main <see cref="System.Reflection.Assembly"/> for the resources.</param>
        public ChainableResourceManager(string baseName, System.Reflection.Assembly assembly)
            : base(baseName, assembly)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainableResourceManager"/> class.
        /// </summary>
        /// <param name="baseName">The root name of the resources. For example, the root name for the resource
        ///     file named "MyResource.en-US.resources" is "MyResource".</param>
        /// <param name="assembly">The main <see cref="System.Reflection.Assembly"/> for the resources.</param>
        /// <param name="usingResourceSet">The using resource set.</param>
        public ChainableResourceManager(string baseName, System.Reflection.Assembly assembly, Type usingResourceSet)
            : base(baseName, assembly, usingResourceSet)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainableResourceManager"/> class.
        /// </summary>
        /// <param name="chainResourceManager">The resource manager to chain to.</param>
        /// <param name="resourceSource">The resource source.</param>
        public ChainableResourceManager(ResourceManager chainResourceManager, Type resourceSource)
            : base(resourceSource)
        {
            _chainResourceManager = chainResourceManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainableResourceManager"/> class.
        /// </summary>
        /// <param name="chainResourceManager">The resource manager to chain to.</param>
        /// <param name="baseName">The root name of the resources. For example, the root name for the resource
        ///     file named "MyResource.en-US.resources" is "MyResource".</param>
        /// <param name="assembly">The main <see cref="System.Reflection.Assembly"/> for the resources.</param>
        public ChainableResourceManager(ResourceManager chainResourceManager, string baseName, System.Reflection.Assembly assembly)
            : base(baseName, assembly)
        {
            _chainResourceManager = chainResourceManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainableResourceManager"/> class.
        /// </summary>
        /// <param name="chainResourceManager">The resource manager to chain to.</param>
        /// <param name="baseName">The root name of the resources. For example, the root name for the resource
        ///     file named "MyResource.en-US.resources" is "MyResource".</param>
        /// <param name="assembly">The main <see cref="System.Reflection.Assembly"/> for the resources.</param>
        /// <param name="usingResourceSet">The using resource set.</param>
        public ChainableResourceManager(ResourceManager chainResourceManager, string baseName, System.Reflection.Assembly assembly, Type usingResourceSet)
            : base(baseName, assembly, usingResourceSet)
        {
            _chainResourceManager = chainResourceManager;
        }

        /// <summary>
        /// Returns the value of the specified <see cref="T:System.String"/> resource.
        /// </summary>
        /// <param name="name">The name of the resource to get.</param>
        /// <returns>
        /// The value of the resource localized for the caller's current culture settings. If a match is not possible, null is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="name"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// The value of the specified resource is not a string.
        /// </exception>
        /// <exception cref="T:System.Resources.MissingManifestResourceException">
        /// No usable set of resources has been found, and there are no neutral culture resources.
        /// </exception>
        public override string GetString(string name)
        {
            string baseString = base.GetString(name);

            if (!string.IsNullOrEmpty(baseString))
                return baseString;
            else if (_chainResourceManager != null)
                return _chainResourceManager.GetString(name);
            else return null;
        }

        /// <summary>
        /// Gets the value of the <see cref="T:System.String"/> resource localized for the specified culture.
        /// </summary>
        /// <param name="name">The name of the resource to get.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> object that represents the culture for which the resource is localized. Note that if the resource is not localized for this culture, the lookup will fall back using the culture's <see cref="P:System.Globalization.CultureInfo.Parent"/> property, stopping after looking in the neutral culture.
        /// If this value is null, the <see cref="T:System.Globalization.CultureInfo"/> is obtained using the culture's <see cref="P:System.Globalization.CultureInfo.CurrentUICulture"/> property.</param>
        /// <returns>
        /// The value of the resource localized for the specified culture. If a best match is not possible, null is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="name"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// The value of the specified resource is not a <see cref="T:System.String"/>.
        /// </exception>
        /// <exception cref="T:System.Resources.MissingManifestResourceException">
        /// No usable set of resources has been found, and there are no neutral culture resources.
        /// </exception>
        public override string GetString(string name, System.Globalization.CultureInfo culture)
        {
            string baseString = base.GetString(name, culture);

            if (!string.IsNullOrEmpty(baseString))
                return baseString;
            else if (_chainResourceManager != null)
                return _chainResourceManager.GetString(name, culture);
            else return null;
        }

        /// <summary>
        /// Returns the value of the specified <see cref="T:System.Object"/> resource.
        /// </summary>
        /// <param name="name">The name of the resource to get.</param>
        /// <returns>
        /// The value of the resource localized for the caller's current culture settings. If a match is not possible, null is returned. The resource value can be null.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="name"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Resources.MissingManifestResourceException">
        /// No usable set of resources has been found, and there are no neutral culture resources.
        /// </exception>
        public override object GetObject(string name)
        {
            object baseObject = base.GetObject(name);

            if (baseObject != null)
                return baseObject;
            else if (_chainResourceManager != null)
                return _chainResourceManager.GetObject(name);
            else return null;
        }

        /// <summary>
        /// Gets the value of the <see cref="T:System.Object"/> resource localized for the specified culture.
        /// </summary>
        /// <param name="name">The name of the resource to get.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> object that represents the culture for which the resource is localized. Note that if the resource is not localized for this culture, the lookup will fall back using the culture's <see cref="P:System.Globalization.CultureInfo.Parent"/> property, stopping after checking in the neutral culture.
        /// If this value is null, the <see cref="T:System.Globalization.CultureInfo"/> is obtained using the culture's <see cref="P:System.Globalization.CultureInfo.CurrentUICulture"/> property.</param>
        /// <returns>
        /// The value of the resource, localized for the specified culture. If a "best match" is not possible, null is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="name"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Resources.MissingManifestResourceException">
        /// No usable set of resources have been found, and there are no neutral culture resources.
        /// </exception>
        public override object GetObject(string name, System.Globalization.CultureInfo culture)
        {
            object baseObject = base.GetObject(name, culture);

            if (baseObject != null)
                return baseObject;
            else if (_chainResourceManager != null)
                return _chainResourceManager.GetObject(name, culture);
            else return null;
        }
    }
}