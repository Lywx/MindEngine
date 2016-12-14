namespace MindEngine.Core.Scene.Entity
{
    using System;
    using Microsoft.Xna.Framework;
    using Util.Collection;

    internal interface IMMEntityUpdateableList<T> : IMMUpdateableOperations where T : class
    {
        void Update<TDerived>(Action<TDerived, GameTime> updateAction, GameTime time)
            where TDerived : class;
    }

    public class MMEntityUpdatableList : MMEntityUpdatableList<MMEntityUpdatable>
    {
    }

    public class MMEntityUpdatableList<T> : MMMultiList<T> where T : class  
    {
        protected MMSortingFilteringList<MMEntityUpdatable, MMUpdateEnabledChangedEventArgs, MMUpdateOrderChangedEventArgs>
            UpdateItems { get; } =
            new MMSortingFilteringList<MMEntityUpdatable, MMUpdateEnabledChangedEventArgs, MMUpdateOrderChangedEventArgs>(
                (updatable)          => updatable.UpdateEnabled,
                (updatable, handler) => updatable.UpdateEnabledChanged += handler,
                (updatable, handler) => updatable.UpdateEnabledChanged -= handler,
                (updatable, updatableOther)     => updatable.CompareTo(updatableOther),
                (updatable, handler) => updatable.UpdateOrderChanged += handler,
                (updatable, handler) => updatable.UpdateOrderChanged -= handler);

        public MMEntityUpdatableList()
        {
            this.AddItems<MMEntityUpdatable>(this.UpdateItems);
        }

        #region Collection Operations

        public override void Add(T item)
        {
            if (this.Items.Contains(item))
            {
                return;
            }

            this.AddInternal(item);

            base.Add(item);
        }

        protected virtual void AddInternal(T item)
        {
            this.Add(item, this.UpdateItems);
        }

        public override bool Remove(T item)
        {
            if (this.Items.IndexOf(item) < 0)
            {
                return false;
            }

            this.RemoveInternal(item);

            return base.Remove(item);
        }

        protected virtual void RemoveInternal(T item)
        {
            this.Remove(item, this.UpdateItems);
        }

        public override void RemoveAt(int index)
        {
            var item = this[index];

            this.RemoveInternal(item);

            base.RemoveAt(index);
        }

        public override void Clear()
        {
            this.ClearInternal();
            base.Clear();
        }

        protected virtual void ClearInternal()
        {
            this.UpdateItems.Clear();
        }

        #endregion

        #region Update Operations

        public virtual void Update(GameTime time)
        {
            this.UpdateItems.ForEach(
                     (updateableParam, timeParam) => updateableParam.Update(timeParam), time);
        }

        public void Update<TDerived>(Action<TDerived, GameTime> updateAction, GameTime time) 
            where TDerived : class
        {
            this.UpdateItems.ForEach(
                     (updateableParam, timeParam) => updateAction.Invoke(updateableParam as TDerived, timeParam), time);
        }

        #endregion
    }
}