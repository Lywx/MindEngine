namespace MindEngine.Core.Services.Process
{
    using System;
    using Microsoft.Xna.Framework;

    public interface IMMProcess : IMMProcessManagerItem
    {
        string Name { get; set; }
    }

    public abstract class MMProcess : MMObject, IMMProcess
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

        public MMProcessState State { get; private set; } = MMProcessState.New;

        #endregion Process States

        #region Process Event Operations

        public void Enter()
        {
            if (this.State == MMProcessState.New)
            {
                this.State = MMProcessState.Running;
            }
        }

        protected void Wait()
        {
            if (this.State == MMProcessState.Running)
            {
                this.State = MMProcessState.Waiting;
            }
        }

        protected void Dispatch()
        {
            if (this.State == MMProcessState.Waiting)
            {
                this.State = MMProcessState.Running;
            }
        }

        protected void Exit()
        {
            if (this.State == MMProcessState.Running)
            {
                this.State = MMProcessState.Terminated;
            }
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void OnUpdate(GameTime time)
        {
            
        }

        public virtual void OnWait(GameTime time)
        {
        }

        public virtual void OnEnter()
        {
        }

        #endregion 

        #region Process Priority Semantics

        public MMProcessCategory Category { get; }

        public virtual int Priority => (int)this.Category;

        public int CompareTo(IMMProcessManagerItem other)
        {
            return this.Priority.CompareTo(other.Priority);
        }

        #endregion

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
                    // ignored
                }

                this.IsDisposed = true;
            }
        }

        #endregion
    }
}
