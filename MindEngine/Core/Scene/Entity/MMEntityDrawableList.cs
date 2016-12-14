namespace MindEngine.Core.Scene.Entity
{
    using System;
    using Core;
    using Microsoft.Xna.Framework;
    using Util.Collection;

    internal interface IMMEntityDrawableList<T> : IMMEntityUpdateableList<T>, IMMDrawableOperations 
        where T : class
    {
        void Draw<TDerived>(Action<TDerived, GameTime> drawAction, GameTime time)
            where TDerived : class;
    }

    public class MMEntityDrawableList : MMEntityDrawableList<MMEntityDrawable>
    {
    }

    /// <summary>
    /// Provide sorting, filtering for some of the most common operations.
    /// </summary>
    [Serializable]
    public class MMEntityDrawableList<T> : MMEntityUpdatableList<T>, IMMEntityDrawableList<T> where T : class  
    {
        protected MMSortingFilteringList<MMEntityDrawable, MMDrawEnabledChangedEventArgs, MMDrawOrderChangedEventArgs>
            DrawItems { get; } =
            new MMSortingFilteringList<MMEntityDrawable, MMDrawEnabledChangedEventArgs, MMDrawOrderChangedEventArgs>(
                (drawable)          => drawable.DrawEnabled,
                (drawable, handler) => drawable.DrawEnabledChanged += handler,
                (drawable, handler) => drawable.DrawEnabledChanged -= handler,
                (drawable, dOther)  => drawable.CompareTo(dOther),
                (drawable, handler) => drawable.DrawOrderChanged += handler,
                (drawable, handler) => drawable.DrawOrderChanged -= handler);

        #region Constructors and Finalizer

        public MMEntityDrawableList()
        {
            this.AddItems<MMEntityDrawable>(this.DrawItems);
        }

        #endregion

        #region Collection Operations

        protected override void AddInternal(T item)
        {
            this.Add(item, this.DrawItems);
            base.AddInternal(item);
        }

        protected override void RemoveInternal(T item)
        {
            this.Remove(item, this.DrawItems);
            base.RemoveInternal(item);
        }

        protected override void ClearInternal()
        {
            this.DrawItems.Clear();
            base.ClearInternal();
        }

        #endregion

        #region Drawable Operations

        public virtual void Draw(GameTime time)
        {
            this.DrawItems.ForEach(
                (drawableParam, timeParam) => drawableParam.Draw(timeParam), time);
        }

        public void Draw<TDerived>(Action<TDerived, GameTime> drawAction, GameTime time) 
            where TDerived : class
        {
            this.DrawItems.ForEach(
                (drawableParam, timeParam) => drawAction.Invoke(drawableParam as TDerived, timeParam), time);
        }

        #endregion
    }
}