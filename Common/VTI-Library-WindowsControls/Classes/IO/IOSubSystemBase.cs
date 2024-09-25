using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Used as a base class for the I/O classes in the client application
    /// </summary>
    /// <typeparam name="T">Type of the I/O class</typeparam>
    /// <remarks>
    /// In the constructor, the IOSubSystemBase attempts to associate each
    /// <see cref="IAnalogInput">AnalogInput</see>,
    /// <see cref="IAnalogOutput">AnalogOutput</see>,
    /// <see cref="IDigitalInput">DigitalInput</see> and
    /// <see cref="IDigitalOutput">DigitalOutput</see>
    /// to it's equivalent in the <see cref="IOInterface">IOInterface</see>.
    /// </remarks>
    public abstract class IOSubSystemBase<T> : KeyedCollection<string, T>
        where T : IAnalogDigitalIO
    {
        private Dictionary<string, T> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="IOSubSystemBase{T}">IOSubSystemBase</see> class
        /// </summary>
        /// <param name="IOInterface">IOInterface</param>
        /// <remarks>
        /// attempts to associate each
        /// <see cref="IAnalogInput">AnalogInput</see>,
        /// <see cref="IAnalogOutput">AnalogOutput</see>,
        /// <see cref="IDigitalInput">DigitalInput</see> and
        /// <see cref="IDigitalOutput">DigitalOutput</see>
        /// to it's equivalent in the <see cref="IOInterface">IOInterface</see>.
        /// </remarks>
        public IOSubSystemBase(IOInterface IOInterface)
        {
            _items = new Dictionary<string, T>();

            foreach (var field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(f => f.FieldType == typeof(T)))
            {
                try
                {
                    if (VTIPLCInterfaceAccessMethods.PLCEnabled() && field.GetValue(this) == null)
                    {
                        if (field.FieldType == typeof(IAnalogInput))
                            field.SetValue(this, IOInterface.AnalogInputs[field.Name]);
                        else if (field.FieldType == typeof(IAnalogOutput))
                            field.SetValue(this, IOInterface.AnalogOutputs[field.Name]);
                        else if (field.FieldType == typeof(IDigitalInput))
                            field.SetValue(this, IOInterface.DigitalInputs[field.Name]);
                        else if (field.FieldType == typeof(IDigitalOutput))
                            field.SetValue(this, IOInterface.DigitalOutputs[field.Name]);
                    }
                }
                catch
                {
                    string sFieldType;
                    VTIWindowsControlLibrary.Enums.VtiEventCatType cat;

                    //  I/O name is in DigitalInputs.cs/AnalogInputs.cs/etc. but NOT in VtiPLCInterface.config 
                    if (field.FieldType == typeof(IAnalogInput))
                    {
                        sFieldType = "Analog Input";
                        cat = VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO;
                        if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                        {
                            VtiEvent.Log.WriteWarning("I/O Warning: " + sFieldType + " '" + field.Name + "' not found!", cat);
                        }
                    }
                    else if (field.FieldType == typeof(IAnalogOutput))
                    {
                        sFieldType = "Analog Output";
                        cat = VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO;
                        if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                        {
                            VtiEvent.Log.WriteWarning("I/O Warning: " + sFieldType + " '" + field.Name + "' not found!", cat);
                        }
                    }
                    else if (field.FieldType == typeof(IDigitalInput))
                    {
                        sFieldType = "Digital Input";
                        cat = VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO;
                        if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                        {
                            VtiEvent.Log.WriteWarning("I/O Warning: " + sFieldType + " '" + field.Name + "' not found!", cat);
                        }
                    }
                    else if (field.FieldType == typeof(IDigitalOutput))
                    {
                        sFieldType = "Digital Output";
                        cat = VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO;
                        if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                        {
                            VtiEvent.Log.WriteWarning("I/O Warning: " + sFieldType + " '" + field.Name + "' not found!", cat);
                        }
                    }
                }

                if (field.GetValue(this) == null)
                {
                    if (field.FieldType == typeof(IAnalogInput))
                        field.SetValue(this, new UnavailableAnalogInput(field.Name));
                    else if (field.FieldType == typeof(IAnalogOutput))
                        field.SetValue(this, new UnavailableAnalogOutput(field.Name));
                    else if (field.FieldType == typeof(IDigitalInput))
                        field.SetValue(this, new UnavailableDigitalInput(field.Name));
                    else if (field.FieldType == typeof(IDigitalOutput))
                        field.SetValue(this, new UnavailableDigitalOutput(field.Name));
                }

                //_arrayList.Add(field.GetValue(this));
                _items.Add(field.Name, (T)field.GetValue(this));
            }
        }

        /// <summary>
        /// Extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }

        /// <summary>
        /// Gets the index of the item with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int IndexOf(string key)
        {
            return this.IndexOf(this[key]);
        }
    }
}