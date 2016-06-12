namespace MindEngine.Core.Services.Processes
{
    using System;
    using Microsoft.Xna.Framework;

    public abstract class MMProcess : IMMProcess
    {
        #region Constructors and Finalizer

        protected MMProcess(string name, MMProcessCategory category)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name     = name;
            this.Category = category;
        }

        ~MMProcess()
        {
            this.Dispose(true);
        }

        #endregion

        public string Name { get; set; }

        #region Process States

        public MMProcessState State { get; private set; } = MMProcessState.Uninitialized;

        public bool IsAlive => this.State == MMProcessState.Running ||
                               this.State == MMProcessState.Paused;

        public bool IsDead => this.State == MMProcessState.Succeeded ||
                              this.State == MMProcessState.Failed ||
                              this.State == MMProcessState.Aborted;

        public bool IsUninitialized => this.State == MMProcessState.Uninitialized;

        public bool IsRemoved => this.State == MMProcessState.Removed;

        #endregion Process States

        #region Process Event Operations

        public abstract void OnAbort();

        public abstract void OnFail();

        public virtual void OnInit()
        {
            this.State = MMProcessState.Running;
        }

        public abstract void OnSucceed();

        #endregion 

        #region Process State Operations

        public void Abort()
        {
            this.State = MMProcessState.Aborted;
        }

        public void Fail()
        {
            this.State = MMProcessState.Failed;
        }

        public void Pause()
        {
            if (this.State == MMProcessState.Running)
            {
                this.State = MMProcessState.Paused;
            }
        }

        public void Resume()
        {
            if (this.State == MMProcessState.Paused)
            {
                this.State = MMProcessState.Running;
            }
        }

        public void Succeed()
        {
            this.State = MMProcessState.Succeeded;
        }

        #endregion

        #region Process Chain Semantics

        public IMMProcess Child { get; private set; }

        public void AttachChild(IMMProcess process)
        {
            if (this.Child != null
                && this.Child != process)
            {
                this.Child.AttachChild(process);
            }
            else
            {
                this.Child = process;
            }
        }

        public T RemoveChild<T>() where T : IMMProcessManagerItem
        {
            if (this.Child != null)
            {
                var removedChild = this.Child;
                this.Child = null;

                return (T)removedChild;
            }

            return default(T);
        }

        #endregion

        #region Process Priority Semantics

        public MMProcessCategory Category { get; }

        public virtual int Priority => (int)this.Category;

        #endregion

        #region Update

        public virtual void Update(GameTime time)
        {
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.IsDisposed)
                {
                    // ignored
                }

                this.IsDisposed = true;
            }
        }

        #endregion
    }
}
