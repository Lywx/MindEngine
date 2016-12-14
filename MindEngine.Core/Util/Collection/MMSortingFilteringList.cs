namespace MindEngine.Core.Util.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMMSortingFilteringList<T> : IMMList<T>
    {
        void Rebuild();
    }

    /// <summary>
    ///     This class provides efficient, reusable
    ///     sorting and filtering based on a configurable sort comparer, filter
    ///     predicate, and associate change events. This class comes from Game.cs
    ///     in MonoGame Framework.
    /// </summary>
    public class MMSortingFilteringList<T, TFilterChangedEventArgs, TSortChangedEventArgs> : IMMSortingFilteringList<T>
        where TFilterChangedEventArgs : EventArgs
        where TSortChangedEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        private readonly List<AdditionJournalEntry<T>> additionJournal = new List<AdditionJournalEntry<T>>();

        private readonly Comparison<AdditionJournalEntry<T>> additionJournalSortComparison;

        /// <summary>
        ///     It is used to filter items into cached items.
        /// </summary>
        private readonly Predicate<T> filter;

        private readonly Action<T, EventHandler<TFilterChangedEventArgs>> filterChangedSubscriber;

        private readonly Action<T, EventHandler<TFilterChangedEventArgs>> filterChangedUnsubscriber;

        /// <summary>
        ///     This field "items" is always sorted after processing add / remove
        ///     journal requests. This is the sorted and filtered collection
        ///     maintains during each add / remove / change request.
        /// </summary>
        private readonly List<T> items = new List<T>();

        /// <summary>
        ///     Filtered, sorted cached items. The reason this exists is that
        ///     normally you would not like to have the "items" reordered during the
        ///     for each statement. But I think it is still optional to have
        ///     "itemsCached", which is merely the mirror version of the "items"
        ///     after rebuilding cache.
        /// </summary>
        private readonly List<T> itemsCached = new List<T>();

        /// <summary>
        ///     Document entry indexes that need to remove.
        /// </summary>
        private readonly List<int> removalJournal = new List<int>();

        /// <remarks>
        ///     Sort remove journal high to low.
        /// </remarks>
        private readonly Comparison<int> removalJournalSortComparison =
            (x, y) => -Comparer<int>.Default.Compare(x, y);

        private readonly Comparison<T> sort;

        private readonly Action<T, EventHandler<TSortChangedEventArgs>> sortChangedSubscriber;

        private readonly Action<T, EventHandler<TSortChangedEventArgs>> sortChangedUnsubscriber;

        private bool itemsCacheDirty = true;

        #region Constructors and Finalizer

        public MMSortingFilteringList(
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
            this.additionJournalSortComparison = this.AdditionJournalSortComparison;
        }

        #endregion

#pragma warning disable 693
        private struct AdditionJournalEntry<T>
#pragma warning restore 693
        {
            public AdditionJournalEntry(T item, int weight)
            {
                this.Item   = item;
                this.Weight = weight;
            }

            public T Item { get; }

            public int Weight { get; }

            /// <summary>
            ///     Create an entry for item removal in a collection.
            /// </summary>
            /// <param name="item">item to be removed</param>
            /// <returns>an entry could be used for removal</returns>
            public static AdditionJournalEntry<T> CreateRemoveEntry(T item)
            {
                // The reason for -1 is inside override Equals method. 
                return new AdditionJournalEntry<T>(item, -1);
            }

            public override int GetHashCode()
            {
                return this.Item.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var isEntry = obj is AdditionJournalEntry<T>;
                if (!isEntry)
                {
                    return false;
                }

                var otherEntry = (AdditionJournalEntry<T>)obj;
                return object.Equals(this.Item, otherEntry.Item);
            }
        }

        #region Customized Action Operations

        public void ForEach<TUserData>(Action<T, TUserData> userAction, TUserData userData)
        {
            this.OnBeginAction();

            foreach (var item in this.itemsCached)
            {
                userAction(item, userData);
            }

            this.OnEndAction();
        }

        public void First<TUserData>(Action<T, TUserData> userAction, TUserData userData)
        {
            this.OnBeginAction();

            userAction(this.itemsCached.First(), userData);

            this.OnEndAction();
        }

        public void Last<TUserData>(Action<T, TUserData> userAction, TUserData userData)
        {
            this.OnBeginAction();

            userAction(this.itemsCached.Last(), userData);

            this.OnEndAction();
        }

        private void OnBeginAction()
        {
            this.TryRebuildItems();
        }

        private void OnEndAction()
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
            this.AddAdditionJournal(item);

            this.InvalidateCache();
        }

        public bool Remove(T item)
        {
            // There are 2 possible place to find the item:
            // 1. It still remains in the "additionJournal"
            // 2. It is already added to the "items"

            // First try to remove from "additionJournal"
            if (this.TryRemoveFromAdditionJournal(item))
            {
                return true;
            }

            // Then try to remove from "items"
            if (this.TryRemoveFromItems(item))
            {
                return true;
            }

            // Remove nothing
            return false;
        }

        public void Clear()
        {
            foreach (var item in this.items)
            {
                this.UnsubscribeItemEvents(item);
            }

            this.additionJournal.Clear();
            this.removalJournal.Clear();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.items).GetEnumerator();
        }

        #endregion

        #region List Operations

        public int IndexOf(T item)
        {
            return this.items.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            this.Remove(this[index]);
        }

        public T this[int index] => this.items[index];

        #endregion

        #region Item Operations

        public void Rebuild()
        {
            this.TryRebuildItems();
        }

        private void TryRebuildItems()
        {
            if (this.itemsCacheDirty)
            {
                this.ProcessRemovalJournal();
                this.ProcessAdditionJournal();

                this.RebuildCache();
                this.itemsCacheDirty = false;
            }
        }

        private bool TryRemoveFromItems(T item)
        {
            // See if the item in the "items"
            var index = this.items.IndexOf(item);

            // When found inside "items"
            if (index >= 0)
            {
                this.UnsubscribeItemEvents(item);
                this.removalJournal.Add(index);
                this.InvalidateCache();

                return true;
            }

            return false;
        }

        #endregion

        #region Add Journal Operations

        private void AddAdditionJournal(T item)
        {
            // Note: We subscribe to item events after items in addJournal have been merged.
            this.additionJournal.Add(new AdditionJournalEntry<T>(item, this.additionJournal.Count));
        }

        private int AdditionJournalSortComparison(AdditionJournalEntry<T> x, AdditionJournalEntry<T> y)
        {
            var result = this.sort(x.Item, y.Item);
            if (result != 0)
            {
                return result;
            }

            return x.Weight - y.Weight;
        }

        /// Read about the algorithm in 
        /// "Documentation/Class/SortingFilteringList/ApplyAdditionJournal.png"/>
        private void ApplyAdditionJournal()
        {
            var itemCount = 0;
            var additionJournalCount = 0;

            // Note: Both "items" and "additionJournal" are already sorted at this point.
            while (itemCount < this.items.Count
                   && additionJournalCount < this.additionJournal.Count)
            {
                var addJournalItem = this.additionJournal[additionJournalCount].Item;

                // Translate: If "addJournalItem" is less than (belongs before)
                // "items[itemsCount]", insert it.
                if (this.sort(addJournalItem, this.items[itemCount]) < 0)
                {
                    this.SubscribeItemEvents(addJournalItem);
                    this.items.Insert(itemCount, addJournalItem);
                    ++additionJournalCount;
                }

                // Always increment itemCount, either because we inserted and
                // need to move past the insertion, or because we didn't
                // insert and need to consider the next element.
                ++itemCount;
            }

            // If "addJournal" had any "tail" items, append them all now.
            for (; additionJournalCount < this.additionJournal.Count; ++additionJournalCount)
            {
                var additionJournalItem = this.additionJournal[additionJournalCount].Item;
                this.SubscribeItemEvents(additionJournalItem);
                this.items.Add(additionJournalItem);
            }
        }

        private void ClearAdditionJournal()
        {
            this.additionJournal.Clear();
        }

        private void ProcessAdditionJournal()
        {
            if (this.additionJournal.Count == 0)
            {
                return;
            }

            // Prepare the "addJournal" to be merge-sorted with "items". 
            this.SortAdditionJournal();
            this.ApplyAdditionJournal();
            this.ClearAdditionJournal();
        }

        /// <summary>
        ///     Sort addJournal from low to high.
        /// </summary>
        private void SortAdditionJournal()
        {
            this.additionJournal.Sort(this.additionJournalSortComparison);
        }

        private bool TryRemoveFromAdditionJournal(T item)
        {
            return this.additionJournal.Remove(AdditionJournalEntry<T>.CreateRemoveEntry(item));
        }

        #endregion

        #region Remove Journal Operations

        private void ProcessRemovalJournal()
        {
            if (this.removalJournal.Count == 0)
            {
                return;
            }

            // Remove items in reverse. (Technically there exist faster
            // ways to bulk-remove from a variable-length array, but List<T>
            // does not provide such a method.)

            // Note: Sort in reverse order, high to low
            this.SortRemovalJournal();
            this.ApplyRemovalJournal();

            this.ClearRemovalJournal();
        }

        private void ApplyRemovalJournal()
        {
            for (var i = 0; i < this.removalJournal.Count; ++i)
            {
                this.items.RemoveAt(this.removalJournal[i]);
            }
        }

        private void ClearRemovalJournal()
        {
            this.removalJournal.Clear();
        }

        private void SortRemovalJournal()
        {
            this.removalJournal.Sort(this.removalJournalSortComparison);
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

            this.additionJournal.Add(new AdditionJournalEntry<T>(item, this.additionJournal.Count));
            this.removalJournal.Add(index);

            // Until the item is back in place, we don't care about its
            // events. We will re-subscribe when addJournal is processed.
            this.UnsubscribeItemEvents(item);
            this.InvalidateCache();
        }

        #endregion
    }
}
