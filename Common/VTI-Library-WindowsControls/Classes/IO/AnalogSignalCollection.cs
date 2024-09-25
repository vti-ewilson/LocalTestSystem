using System.Collections.ObjectModel;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Implements a <see cref="KeyedCollection{K, T}">keyed collection</see> of
    /// <see cref="AnalogSignal">analog signals</see>.
    /// </summary>
    public class AnalogSignalCollection : KeyedCollection<string, AnalogSignal>
    {
        /// <summary>
        /// Extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected override string GetKeyForItem(AnalogSignal item)
        {
            return item.Key;
        }
    }
}