namespace MindEngine.Core.Service.Process
{
    using System;
    using System.Diagnostics;
    using Microsoft.Xna.Framework;
    using NLog;

    public class MMProcessPriorityChangedEventArgs : EventArgs
    {
        public MMProcessPriorityChangedEventArgs(int priority)
        {
            this.Priority = priority;
        }

        public int Priority { get; }
    }

    internal interface IMMProcess : IMMProcessManagerItem
    {
        void Enter();

        void Run();

        void Wait();

        void Dispatch();

        void Exit();
    }

    public abstract class MMProcess : MMObject, IMMProcess
    {
#if DEBUG

        #region Logger

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

#endif

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

        public MMProcessState State { get; private set; } = MMProcessState.Inertial;

        #region Process State Operations

        public event EventHandler<EventArgs> Exited = delegate {};

        public event EventHandler<EventArgs> Waited = delegate {};

        public event EventHandler<EventArgs> Updated = delegate {};

        public event EventHandler<EventArgs> Entered = delegate {};

        /// <summary>
        /// The process will need to populate manually to enter the running state.
        /// </summary>
        public void Enter()
        {
            this.ChangeState(MMProcessState.Inertial, MMProcessState.New);
        }

        public void Run()
        {
            this.ChangeState(MMProcessState.New, MMProcessState.Running);
        }

        public void Wait()
        {
            this.ChangeState(MMProcessState.Running, MMProcessState.Waiting);
        }

        public void Dispatch()
        {
            this.ChangeState(MMProcessState.Waiting, MMProcessState.Running);
        }

        public void Exit()
        {
            this.ChangeState(MMProcessState.Running, MMProcessState.Terminated);
        }

        private void ChangeState(MMProcessState stateCurrent, MMProcessState stateNext)
        {
            if (this.State == stateCurrent)
            {
                this.State = stateNext;

                this.LogStateChange(stateCurrent, stateNext);
            }
            else
            {
                this.LogStateError(stateCurrent, stateNext);
            }
        }

        [Conditional("DEBUG")]
        private void LogStateChange(MMProcessState stateCurrent, MMProcessState stateNext)
        {
            logger.Info($"{this.Name} switch state : {stateCurrent} -> {stateNext}");
        }

        [Conditional("DEBUG")]
        private void LogStateError(MMProcessState stateCurrent, MMProcessState stateNext)
        {
            logger.Warn($"{this.Name} incorrectly try to switch change : {stateCurrent} -> {stateNext}");
        }

        #endregion Process States

        #region Process Event Operations

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
            this.Entered?.Invoke(this, EventArgs.Empty);
        }

        #endregion 

        #region Process Priority Semantics

        public MMProcessCategory Category { get; }

        public virtual int Priority => (int)this.Category;

        /// TODO(Feature): Dynamic process priority. To do that you need to 
        /// implement Priority property with a event calling get set pair.
        public event EventHandler<MMProcessPriorityChangedEventArgs> PriorityChanged = delegate {};

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
