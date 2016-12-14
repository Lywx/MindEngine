namespace MindEngine.Core.Scene.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Microsoft.Xna.Framework;
    using Service.Process;

    public class MMUpdateOrderChangedEventArgs : EventArgs
    {
        public MMUpdateOrderChangedEventArgs(int updapteOrder)
        {
            this.UpdapteOrder = updapteOrder;
        }

        public int UpdapteOrder { get; }
    }

    public class MMUpdateEnabledChangedEventArgs : EventArgs
    {
        public MMUpdateEnabledChangedEventArgs(bool updateEnabled)
        {
            this.UpdateEnabled = updateEnabled;
        }

        public bool UpdateEnabled { get; }
    }

    public interface IMMEntityUpdatable : IMMUpdateableOperations, IComparable<MMEntityUpdatable>, IDisposable
    {
        #region Entity Events

        event EventHandler<MMUpdateEnabledChangedEventArgs> UpdateEnabledChanged;

        event EventHandler<MMUpdateOrderChangedEventArgs> UpdateOrderChanged;

        #endregion

        #region Entity Properties

        bool UpdateEnabled { get; set; }

        int UpdateOrder { get; set; }

        #endregion
    }

    [Serializable]
    public abstract class MMEntityUpdatable : MMObject, IMMEntityUpdatable
    {
        #region Constructors and Finalizer 

        protected MMEntityUpdatable()
        {
        }

        ~MMEntityUpdatable()
        {
            this.Dispose(true);
        }

        #endregion Destructors

        #region Comparison

        public int CompareTo(MMEntityUpdatable other)
        {
            return this.UpdateOrder.CompareTo(other.UpdateOrder);
        }

        #endregion

        #region Events

        public event EventHandler<MMUpdateEnabledChangedEventArgs> UpdateEnabledChanged = delegate {};

        public event EventHandler<MMUpdateOrderChangedEventArgs> UpdateOrderChanged = delegate {};

        #endregion

        #region Event Ons

        private void OnUpdateEnabledChanged()
        {
            this.UpdateEnabledChanged?.Invoke(this, new MMUpdateEnabledChangedEventArgs(this.UpdateEnabled));
        }

        private void OnUpdateOrderChanged()
        {
            this.UpdateOrderChanged?.Invoke(this, new MMUpdateOrderChangedEventArgs(this.UpdateOrder));
        }

        #endregion

        #region States

        private bool updateEnabled = true;

        public virtual bool UpdateEnabled
        {
            get
            {
                return this.updateEnabled;
            }

            set
            {
                var changed = this.updateEnabled != value;
                if (changed)
                {
                    this.updateEnabled = value;
                    this.OnUpdateEnabledChanged();
                }
            }
        }

        #endregion States

        #region Update

        private int updateOrder;

        public virtual int UpdateOrder
        {
            get
            {
                return this.updateOrder;
            }

            set
            {
                if (this.updateOrder != value)
                {
                    this.updateOrder = value;
                    this.OnUpdateOrderChanged();
                }
            }
        }

        public virtual void Update(GameTime time)
        {
            if (this.UpdateEnabled)
            {
                this.UpdateInternal(time);
            }
        }

        protected virtual void UpdateInternal(GameTime time)
        {
            
        }

        #endregion Update

        #region IDisposable

        private bool IsDisposed { get; set; }
     
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.IsDisposed)
                {
                    this.UpdateOrderChanged = null;
                    this.UpdateEnabledChanged = null;

                    this.IsDisposed = true;
                }
            }
        }

        #endregion IDisposable
    }
}