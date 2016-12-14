namespace MindEngine.Core.Util.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal interface IMMMultiList<T> : IMMList<T> 
    {
        IMMSortingFilteringList<TInterface> ItemsOf<TInterface>();

        T Find(Predicate<T> match);

        int FindIndex(Predicate<T> match);

        bool Exists(Predicate<T> match);
    }

    public class MMMultiList<T> : IMMMultiList<T> where T : class
    {
        /// <summary>
        /// Provides enumerators.
        /// </summary>
        protected List<T> Items { get; } = new List<T>();

        /// <summary>
        /// Provides interface dictionary for different item interfaces.
        /// </summary>
        private Dictionary<string, object> ItemsInterfaces { get; set; } = new Dictionary<string, object>();

        #region Collection Interface 

        protected void AddItems<TInterface>(object items)
        {
            this.ItemsInterfaces.Add(typeof(TInterface).Name, items);
        }

        protected bool RemoveItems<TInterface>()
        {
            return this.ItemsInterfaces.Remove(typeof(TInterface).Name);
        }

        public IMMSortingFilteringList<TInterface> ItemsOf<TInterface>()
        {
            return (IMMSortingFilteringList<TInterface>)this.ItemsInterfaces[typeof(TInterface).Name];
        }

        #endregion

        #region Collection Index and Enumeration

        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        public T this[int index] => this.Items[index];

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Items).GetEnumerator();
        }

        #endregion

        #region Collection Modification

        public virtual void Add(T item)
        {
            this.Items.Add(item);
        }

        protected void Add<TInterface>(T item, ICollection<TInterface> itemCollection) where TInterface : class 
        {
            var itemInterface = item as TInterface;
            if (itemInterface != null)
            {
                itemCollection.Add(itemInterface);
            }
        }

        public virtual bool Remove(T item)
        {
            return this.Items.Remove(item);
        }

        protected void Remove<TInterface>(T item, ICollection<TInterface> itemCollection) where TInterface : class
        {
            var itemInterface = item as TInterface;
            if (itemInterface != null)
            {
                itemCollection.Remove(itemInterface);
            }
        }

        public virtual void RemoveAt(int index)
        {
            this.Items.RemoveAt(index);
        }

        public virtual T Find(Predicate<T> match)
        {
            return this.Items.Find(match);
        }

        public int FindIndex(Predicate<T> match)
        {
            return this.Items.FindIndex(match);
        }

        public virtual bool Exists(Predicate<T> match)
        {
            return this.Items.Exists(match);
        }

        public virtual void Clear()
        {
            this.Items.Clear();
        }

        #endregion

        #region Collection Mischievous

        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        public int Count => this.Items.Count;

        public bool IsReadOnly => ((ICollection<T>)this.Items).IsReadOnly;

        #endregion
    }
}