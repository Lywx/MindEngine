namespace MindEngine.Core.Service.Process
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMProcessPriorityChangedEventArgs : EventArgs
    {
        public MMProcessPriorityChangedEventArgs(int priority)
        {
            this.Priority = priority;
        }

        public int Priority { get; }
    }

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

        #region Process States

        public string Name { get; set; }

        public MMProcessState State { get; private set; } = MMProcessState.Inertial;

        #endregion Process States

        #region Process Event Operations

        public event EventHandler<EventArgs> Exited = delegate {};

        public event EventHandler<EventArgs> Waited = delegate {};

        public event EventHandler<EventArgs> Updated = delegate {};

        public event EventHandler<EventArgs> Entered = delegate {};

        /// <summary>
        /// The process will need to populate manually to enter the running state.
        /// </summary>
        public void Enter()
        {
            if (this.State == MMProcessState.Inertial)
            {
                this.State = MMProcessState.New;
            }
        }

        public void Run()
        {
            if (this.State == MMProcessState.New)
            {
                this.State = MMProcessState.Running;
            }
        }

        public void Wait()
        {
            if (this.State == MMProcessState.Running)
            {
                this.State = MMProcessState.Waiting;
            }
        }

        public void Dispatch()
        {
            if (this.State == MMProcessState.Waiting)
            {
                this.State = MMProcessState.Running;
            }
        }

        public void Exit()
        {
            if (this.State == MMProcessState.Running)
            {
                this.State = MMProcessState.Terminated;
            }
        }

        public virtual void OnExit()
        {
            this.Exited?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnUpdate(GameTime time)
        {
            this.Updated?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnWait(GameTime time)
        {
            this.Waited?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnEnter()
        {
            this.State = MMProcessState.Running;

            this.Entered?.Invoke(this, EventArgs.Empty);
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
