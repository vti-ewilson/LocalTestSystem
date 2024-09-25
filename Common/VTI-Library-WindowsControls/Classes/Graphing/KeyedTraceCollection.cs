using System;
using System.Collections.ObjectModel;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;

namespace VTIWindowsControlLibrary.Classes.Graphing
{
    /// <summary>
    /// Collection of <see cref="IGraphTrace{TTrace, TPoint}">Traces</see> to be displayed on a
    /// <see cref="VTIWindowsControlLibrary.Components.Graphing.GraphControl{TData, TCollection, TTrace, TPoint, TSettings}">Graph Control</see>.
    /// </summary>
    /// <typeparam name="TTrace"></typeparam>
    /// <typeparam name="TPoint"></typeparam>
    public class KeyedTraceCollection<TTrace, TPoint> : KeyedCollection<string, TTrace>
        where TTrace : class, IGraphTrace<TTrace, TPoint>
        where TPoint : class, IGraphPoint
    {
        /// <summary>
        /// Gets the key for the specified item.
        /// </summary>
        /// <param name="item">Item for which to return the key</param>
        /// <returns>Key for the item</returns>
        protected override string GetKeyForItem(TTrace item)
        {
            return item.Key;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the entire
        /// <see cref="KeyedTraceCollection{TTrace, TPoint}">KeyedTraceCollection</see>.
        /// </summary>
        /// <param name="key">The key of the item to locate.</param>
        /// <returns>Index of the item in the collection.</returns>
        public int IndexOf(string key)
        {
            return this.IndexOf(this[key]);
        }

        /// <summary>
        /// Occurs when an item in the collection changes
        /// </summary>
        public event EventHandler<TraceCollectionChangedEventArgs<TTrace, TPoint>> Changed;

        /// <summary>
        /// Raises the <see cref="Changed">Changed</see> event
        /// </summary>
        /// <param name="Change">Type of change that occured</param>
        /// <param name="Item">Item being changed</param>
        /// <param name="Replacement">Replacement item, if the item is being replaced</param>
        protected virtual void OnChanged(TraceChangeType Change,
            TTrace Item, TTrace Replacement)
        {
            if (Changed != null)
                Changed(this, new TraceCollectionChangedEventArgs<TTrace, TPoint>(Change, Item, Replacement));
        }

        /// <summary>
        /// Adds an object to the end of the
        /// <see cref="KeyedTraceCollection{TTrace, TPoint}">KeyedTraceCollection</see>.
        /// </summary>
        /// <param name="newTrace">Item to be added.</param>
        public new void Add(TTrace newTrace)
        {
            base.Add(newTrace);
            newTrace.Changed += new EventHandler<TraceCollectionChangedEventArgs<TTrace, TPoint>>(newTrace_Changed);
        }

        // Changed event handler for the general-purpose trace
        private void newTrace_Changed(object sender, TraceCollectionChangedEventArgs<TTrace, TPoint> e)
        {
            OnChanged(e.ChangeType, e.ChangedItem as TTrace, e.ReplacedWith as TTrace);
        }

        /// <summary>
        /// Removes all elements from the collection
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            OnChanged(TraceChangeType.Cleared, null, null);
        }

        /// <summary>
        /// Inserts an element into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="newItem">The item to insert.</param>
        protected override void InsertItem(int index, TTrace newItem)
        {
            base.InsertItem(index, newItem);
            OnChanged(TraceChangeType.Added, newItem, null);
        }

        /// <summary>
        /// Removes the item at the specified index from the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the item to be removed.</param>
        protected override void RemoveItem(int index)
        {
            TTrace removedItem = Items[index];
            base.RemoveItem(index);
            OnChanged(TraceChangeType.Removed, removedItem, null);
        }

        /// <summary>
        /// Replaces the item at the specified index with the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item to be replaced.</param>
        /// <param name="newItem">The new item.</param>
        protected override void SetItem(int index, TTrace newItem)
        {
            TTrace replaced = Items[index];
            base.SetItem(index, newItem);
            OnChanged(TraceChangeType.Replaced, replaced, newItem);
        }
    }
}