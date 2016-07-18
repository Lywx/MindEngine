namespace MindEngine.Core.Scene.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.Xna.Framework;
    using Service.Process;

    public interface IMMUpdateEntity : IMMEntity
    {
        #region Easy Behavior

        void AttachProcess(MMProcess process);

        /// <summary>
        /// This is only used for the versatile usage of the entity. This is 
        /// often used to implement something like "screen shaking" or many 
        /// different things without creating a whole lot of subclass. 
        /// 
        /// It employs the multiple controllers "pattern" that I've seen in the 
        /// Wild Magic code.
        /// </summary>
        void AttachBehavior(Action<GameTime> behavior);

        void RemoveBehavior(int index);

        void RemoveBehaviors();

        bool BehaviorsEnabled { get; set; }
        
        #endregion 
    }

    [DataContract]
    public abstract class MMUpdateEntity : MMEntity, IComparable<MMUpdateEntity>, IMMUpdateEntity
    {
        #region Constructors and Finalizer 

        protected MMUpdateEntity()
        {
        }

        ~MMUpdateEntity()
        {
            this.Dispose(true);
        }

        #endregion Destructors

        #region Comparison

        public int CompareTo(MMUpdateEntity other)
        {
            return this.UpdateOrder.CompareTo(other.UpdateOrder);
        }

        #endregion

        #region Events

        public event EventHandler<MMUpdateEnabledChangedEventArgs> UpdateEnabledChanged;

        public event EventHandler<MMUpdateOrderChangedEventArgs> UpdateOrderChanged;

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

        public bool UpdateEnabled
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

        public int UpdateOrder
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
            foreach (var behavior in this.Behaviors.ToArray())
            {
                behavior.Update(time);
            }
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
                    this.RemoveBehaviors();
                    this.RemoveConnections();

                    this.UpdateOrderChanged = null;
                    this.UpdateEnabledChanged = null;

                    this.IsDisposed = true;
                }
            }
        }

        #endregion IDisposable

        #region Easy Behavior

        private bool behaviorEnabled = true;

        public bool BehaviorsEnabled
        {
            get { return this.behaviorEnabled && this.updateEnabled; }
            set { this.behaviorEnabled = value; }
        }

        private List<MMEntityBehavior> Behaviors { get; set; } = new List<MMEntityBehavior>();

        public void AttachProcess(MMProcess process)
        {
            this.EngineInterop.Process.AttachProcess(process);
        }

        public void AttachBehavior(Action<GameTime> behavior)
        {
            this.Behaviors.Add(new MMUpdateBehavior(behavior));
        }

        public void RemoveBehavior(int index)
        {
            this.Behaviors.RemoveAt(index);
        }

        public void RemoveBehaviors()
        {
            this.Behaviors.Clear();
        }

        #endregion
    }
}