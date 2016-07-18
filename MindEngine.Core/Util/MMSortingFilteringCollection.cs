namespace MindEngine.Core.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class provides efficient, reusable
    /// sorting and filtering based on a configurable sort comparer, filter
    /// predicate, and associate change events. This class comes from Game.cs 
    /// in MonoGame Framework.
    /// </summary>
    public class MMSortingFilteringCollection<T, TFilterChangedEventArgs, TSortChangedEventArgs> : ICollection<T>
        where TFilterChangedEventArgs : EventArgs 
        where TSortChangedEventArgs : EventArgs
    {
        /// <summary>
        /// This field "items" is always sorted.
        /// </summary>
        private readonly List<T> items = new List<T>();

        /// <summary>
        /// Filtered, sorted cached items.
        /// </summary>
        private readonly List<T> itemsCached = new List<T>();

        private bool itemsCacheDirty = true;

        /// <summary>
        /// 
        /// </summary>
        private readonly List<AddJournalEntry<T>> addJournal = new List<AddJournalEntry<T>>();

        private readonly Comparison<AddJournalEntry<T>> addJournalSortComparison;

        /// <summary>
        /// Document entry indexes that need to remove.
        /// </summary>
        private readonly List<int> removeJournal = new List<int>();

        /// <remarks>
        /// Sort remove journal high to low.
        /// </remarks>
        private readonly Comparison<int> removeJournalSortComparison =
            (x, y) => -Comparer<int>.Default.Compare(x, y);

        /// <summary>
        /// It is used to filter items into cached items.
        /// </summary>
        private readonly Predicate<T> filter;

        private readonly Action<T, EventHandler<TFilterChangedEventArgs>> filterChangedSubscriber;

        private readonly Action<T, EventHandler<TFilterChangedEventArgs>> filterChangedUnsubscriber;

        private readonly Comparison<T> sort;

        private readonly Action<T, EventHandler<TSortChangedEventArgs>> sortChangedSubscriber;

        private readonly Action<T, EventHandler<TSortChangedEventArgs>> sortChangedUnsubscriber;

        #region Constructors and Finalizer

        public MMSortingFilteringCollection(
            Predicate<T> filter,
            Action<T, EventHandler<TFilterChangedEventArgs>> filterChangedSubscriber,
            Action<T, EventHandler<TFilterChangedEventArgs>> filterChangedUnsubscriber,
            Comparison<T> sort,
            Action<T, EventHandler<TSortChangedEventArgs>> sortChangedSubscriber,
            Action<T, EventHandler<TSortChangedEventArgs>> sortChangedUnsubscriber)
        {
            this.filter = filter;
            this.filterChangedSubscriber = filterChangedSubscriber;
            this.filterChangedUnsubscriber = filterChangedUnsubscriber;

            this.sort = sort;
            this.sortChangedSubscriber = sortChangedSubscriber;
            this.sortChangedUnsubscriber = sortChangedUnsubscriber;

            // I have to put it here because its body use "this" keyword so that 
            // it cannot be initialized in member initializer
            this.addJournalSortComparison = this.AddJournalSortComparison;
        }

        #endregion

        #region Action Operations

        public void ForEach<TUserData>(Action<T, TUserData> userAction, TUserData userData)
        {
            this.BeginAction();

            foreach (var item in this.itemsCached)
            {
                userAction(item, userData);
            }

            this.EndAction();
        }

        public void First<TUserData>(Action<T, TUserData> userAction, TUserData userData)
        {
            this.BeginAction();

            userAction(this.itemsCached.First(), userData);

            this.EndAction();
        }

        public void Last<TUserData>(Action<T, TUserData> userAction, TUserData userData)
        {
            this.BeginAction();

            userAction(this.itemsCached.Last(), userData);

            this.EndAction();
        }

        private void BeginAction()
        {
            if (this.itemsCacheDirty)
            {
                this.ProcessRemoveJournal();
                this.ProcessAddJournal();

                this.RebuildCache();
                this.itemsCacheDirty = false;
            }
        }

        private void EndAction()
        {
            // If the cache was invalidated as a result of processing items,
            // now is a good time to clear it and give the GC (more of) a
            // chance to do its thing.
            if (this.itemsCacheDirty)
            {
                this.itemsCached.Clear();
            }
        }

        #endregion

        #region Collection Operations

        public void Add(T item)
        {
            // Note: We subscribe to item events after items in addJournal have been merged.
            this.addJournal.Add(new AddJournalEntry<T>(this.addJournal.Count, item));

            this.InvalidateCache();
        }

        public bool Remove(T item)
        {
            // There are 2 possible place to find the item:
            // - 1. It still remains in the "addJournal"
            // - 2. It is already added to the "items"

            // First try to remove from "addJournal"
            if (this.TryRemoveAddJournal(item))
            {
                return true;
            }

            // Then try to remove from "items"
            if (this.TryRemoveItem(item))
            {
                return true;
            }

            // Remove nothing
            return false;
        }

        private bool TryRemoveItem(T item)
        {
            // See if the item in the "items"
            var index = this.items.IndexOf(item);

            // When found inside "items"
            if (index >= 0)
            {
                this.UnsubscribeItemEvents(item);
                this.removeJournal.Add(index);
                this.InvalidateCache();

                return true;
            }

            return false;
        }

        public void Clear()
        {
            foreach (var item in this.items)
            {
                this.UnsubscribeItemEvents(item);
            }

            this.addJournal.Clear();
            this.removeJournal.Clear();
            this.items.Clear();

            this.InvalidateCache();
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public int Count => this.items.Count;

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)this.items).GetEnumerator();
        }

        #endregion

        #region Add Journal Operations

        private void ProcessAddJournal()
        {
            if (this.addJournal.Count == 0)
            {
                return;
            }

            // Prepare the "addJournal" to be merge-sorted with "items". 
            this.SortAddJournal();
            this.ApplyAddJournal();

            this.ClearAddJournal();
        }

        /// <see cref="Documentation/Class/SortingFilteringCollection/ApplyAddJournal Method.png"/>
        private void ApplyAddJournal()
        {
            var itemCount = 0;
            var addJournalCount = 0;

            // Note: Both "items" and "addJournal" are already sorted at this point.
            while (itemCount < this.items.Count
                   && addJournalCount < this.addJournal.Count)
            {
                var addJournalItem = this.addJournal[addJournalCount].Item;

                // Translate: If "addJournalItem" is less than (belongs before)
                // "items[itemsCount]", insert it.
                if (this.sort(addJournalItem, this.items[itemCount]) < 0)
                {
                    this.SubscribeItemEvents(addJournalItem);
                    this.items.Insert(itemCount, addJournalItem);
                    ++addJournalCount;
                }

                // Always increment iItems, either because we inserted and
                // need to move past the insertion, or because we didn't
                // insert and need to consider the next element.
                ++itemCount;
            }

            // If "addJournal" had any "tail" items, append them all now.
            for (; addJournalCount < this.addJournal.Count; ++addJournalCount)
            {
                var addJournalItem = this.addJournal[addJournalCount].Item;
                this.SubscribeItemEvents(addJournalItem);
                this.items.Add(addJournalItem);
            }
        }

        private int AddJournalSortComparison(AddJournalEntry<T> x, AddJournalEntry<T> y)
        {
            var result = this.sort(x.Item, y.Item);
            if (result != 0)
            {
                return result;
            }

            return x.Order - y.Order;
        }

        private void ClearAddJournal()
        {
            this.addJournal.Clear();
        }

        /// <summary>
        /// Sort addJournal from low to high.
        /// </summary>
        private void SortAddJournal()
        {
            this.addJournal.Sort(this.addJournalSortComparison);
        }

        private bool TryRemoveAddJournal(T item)
        {
            return this.addJournal.Remove(AddJournalEntry<T>.RemoveEntry(item));
        }

        #endregion

        #region Remove Journal Operations

        private void ProcessRemoveJournal()
        {
            if (this.removeJournal.Count == 0)
            {
                return;
            }

            // Remove items in reverse. (Technically there exist faster
            // ways to bulk-remove from a variable-length array, but List<T>
            // does not provide such a method.)

            // Note: Sort in reverse order, high to low
            this.SortRemoveJournal();
            this.ApplyRemoveJournal();

            this.ClearRemoveJournal();
        }

        private void ApplyRemoveJournal()
        {
            for (var i = 0; i < this.removeJournal.Count; ++i)
            {
                this.items.RemoveAt(this.removeJournal[i]);
            }
        }

        private void ClearRemoveJournal()
        {
            this.removeJournal.Clear();
        }

        private void SortRemoveJournal()
        {
            this.removeJournal.Sort(this.removeJournalSortComparison);
        }

        #endregion

        #region Cache Operations

        private void InvalidateCache()
        {
            this.itemsCacheDirty = true;
        }

        private void RebuildCache()
        {
            this.itemsCached.Clear();

            for (var i = 0; i < this.items.Count; ++i)
            {
                if (this.filter(this.items[i]))
                {
                    this.itemsCached.Add(this.items[i]);
                }
            }
        }

        #endregion

        #region Event Handlers

        private void SubscribeItemEvents(T item)
        {
            this.filterChangedSubscriber(item, this.Item_FilterPropertyChanged);
            this.sortChangedSubscriber(item, this.Item_SortPropertyChanged);
        }

        private void UnsubscribeItemEvents(T item)
        {
            this.filterChangedUnsubscriber(item, this.Item_FilterPropertyChanged);
            this.sortChangedUnsubscriber(item, this.Item_SortPropertyChanged);
        }

        private void Item_FilterPropertyChanged(object sender, EventArgs e)
        {
            this.InvalidateCache();
        }

        private void Item_SortPropertyChanged(object sender, EventArgs e)
        {
            var item = (T)sender;
            var index = this.items.IndexOf(item);

            this.addJournal.Add(new AddJournalEntry<T>(this.addJournal.Count, item));
            this.removeJournal.Add(index);

            // Until the item is back in place, we don't care about its
            // events. We will re-subscribe when addJournal is processed.
            this.UnsubscribeItemEvents(item);
            this.InvalidateCache();
        }

        #endregion

#pragma warning disable 693
        private struct AddJournalEntry<T>
#pragma warning restore 693
        {
            /// <summary>
            /// Create an entry for item removal in a collection.
            /// </summary>
            /// <param name="item">item to be removed</param>
            /// <returns>an entry could be used for removal</returns>
            public static AddJournalEntry<T> RemoveEntry(T item)
            {
                return new AddJournalEntry<T>(-1, item);
            }

            public AddJournalEntry(int order, T item)
            {
                this.Order = order;
                this.Item = item;
            }

            public int Order { get; }

            public T Item { get; }

            public override int GetHashCode()
            {
                return this.Item.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var isEntry = obj is AddJournalEntry<T>;
                if (!isEntry)
                {
                    return false;
                }

                var otherEntry = (AddJournalEntry<T>)obj;
                return object.Equals(this.Item, otherEntry.Item);
            }
        }
    }
}