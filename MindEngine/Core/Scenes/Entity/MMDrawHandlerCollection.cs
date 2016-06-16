namespace MindEngine.Core.Scenes.Entity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Core;
    using Utils;

    public class MMDrawHandlerCollection : MMDrawHandlerCollection<MMDrawEntity>
    {
    }

    /// <summary>
    /// Provide sorting, filtering for some of the most common operations.
    /// </summary>
    [Serializable]
    public class MMDrawHandlerCollection<T> : ICollection<T>, IMMUpdateableOperations, IMMDrawableOperations
    {
        #region Order Sensitive Collections

        protected MMSortingFilteringCollection<MMUpdateEntity, MMUpdateEnabledChangedEventArgs, MMUpdateOrderChangedEventArgs>
            ItemsUpdateable { get; } =
            new MMSortingFilteringCollection<MMUpdateEntity, MMUpdateEnabledChangedEventArgs, MMUpdateOrderChangedEventArgs>(
                (d)          => d.UpdateEnabled,
                (d, handler) => d.UpdateEnabledChanged += handler,
                (d, handler) => d.UpdateEnabledChanged -= handler,
                (d1, d2)     => d1.CompareTo(d2),
                (d, handler) => d.UpdateOrderChanged += handler,
                (d, handler) => d.UpdateOrderChanged -= handler);

        protected MMSortingFilteringCollection<MMDrawEntity, MMDrawEnabledChangedEventArgs, MMDrawOrderChangedEventArgs>
            ItemsDrawable { get; } =
            new MMSortingFilteringCollection<MMDrawEntity, MMDrawEnabledChangedEventArgs, MMDrawOrderChangedEventArgs>(
                (d)          => d.DrawEnabled,
                (d, handler) => d.DrawEnabledChanged += handler,
                (d, handler) => d.DrawEnabledChanged -= handler,
                (d1, d2)     => d1.CompareTo(d2),
                (d, handler) => d.DrawOrderChanged += handler,
                (d, handler) => d.DrawOrderChanged -= handler);

        #endregion

        #region Order Insensitive Collections

        /// <summary>
        /// Provides enumerators.
        /// </summary>
        protected List<T> Items { get; } = new List<T>();

        #endregion

        #region Constructors and Finalizer

        public MMDrawHandlerCollection()
        {
        }

        #endregion

        #region Drawable Operations

        public void Draw(GameTime time)
        {
            this.ItemsDrawable.ForEach(
                (drawableParam, timeParam) => drawableParam.Draw(timeParam), time);
        }
        #endregion

        #region Update Operations

        public void Update(GameTime time)
        {
            this.ItemsUpdateable.ForEach(
                (updateableParam, timeParam) => updateableParam.Update(timeParam), time);
        }

        #endregion

        #region Collection Enumeration

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

        protected void Add<TInterface>(T item, ICollection<TInterface> itemCollection) where TInterface : class 
        {
            if (item is TInterface)
            {
                itemCollection.Add(item as TInterface);
            }
        }

        public virtual void Add(T item)
        {
            if (this.Items.Contains(item))
            {
                return;
            }

            // Interface are order sensitive
            this.Add(item, this.ItemsUpdateable);
            this.Add(item, this.ItemsDrawable);

            this.Items.Add(item);
        }

        public virtual void Clear()
        {
            // Interface are order sensitive
            this.ItemsUpdateable.Clear();
            this.ItemsDrawable.Clear();

            this.Items.Clear();
        }

        protected void Remove<TInterface>(T item, ICollection<TInterface> itemCollection) where TInterface : class 
        {
            if (item is TInterface)
            {
                itemCollection.Remove(item as TInterface);
            }
        }

        public virtual bool Remove(T item)
        {
            if (!this.Items.Contains(item))
            {
                return false;
            }

            // Interface are order sensitive
            this.Remove(item, this.ItemsUpdateable);
            this.Remove(item, this.ItemsDrawable);

            return this.Items.Remove(item);
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