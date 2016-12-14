namespace MindEngine.Core.Service.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public interface IQueueSession
    {
        /// <summary>
        /// End of a series of asynchronous session
        /// </summary>>
        event EventHandler ThreadStopped;

        /// <summary>
        /// Start of a series of asynchronous session
        /// </summary>>
        event EventHandler ThreadStarted;

        void Update();
    }

    public class MMThreadQueue : IQueueSession
    {
        private readonly List<Thread> threadsQueued = new List<Thread>();

        private Thread threadCurrent;

        #region Events

        /// <summary>
        /// End of a series of asynchronous session
        /// </summary>>
        public event EventHandler ThreadStopped = delegate { };

        /// <summary>
        /// Start of a series of asynchronous session
        /// </summary>>
        public event EventHandler ThreadStarted = delegate { };

        private void OnThreadStopped()
        {
            // Safe threading
            var stopped = this.ThreadStopped;

            stopped?.Invoke(this, EventArgs.Empty);
        }

        private void OnThreadStarted()
        {
            // Safe threading
            var started = this.ThreadStarted;

            started?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Operations

        protected void ContinueThread()
        {
            if (this.threadCurrent == null &&
                this.threadsQueued.Count != 0)
            {
                this.threadCurrent = this.threadsQueued.First();
                this.threadCurrent.Start();
            }
        }

        public void DeferThread(string actionName, Action action)
        {
            this.threadsQueued.Add(new Thread(() => this.ProcessThread(action))
            {
                Name = actionName
            });
        }

        public void StartThread(string actionName, Action action)
        {
            if (this.threadCurrent == null)
            {
                this.threadCurrent = new Thread(() => this.ProcessThread(action))
                {
                    Name = actionName
                };
                this.threadCurrent.Start();
            }
            else
            {
                this.DeferThread(actionName, action);
            }
        }

        protected void ProcessThread(Action action)
        {
            if (this.threadsQueued.Count == 0)
            {
                this.OnThreadStarted();
            }

            action();

            if (this.threadsQueued.Contains(this.threadCurrent))
            {
                this.threadsQueued.Remove(this.threadCurrent);
            }

            this.threadCurrent = null;

            if (this.threadsQueued.Count == 0)
            {
                this.OnThreadStopped();
            }
        }

        #endregion

        #region Update

        public void Update()
        {
            this.ContinueThread();
        }

        #endregion
    }
}