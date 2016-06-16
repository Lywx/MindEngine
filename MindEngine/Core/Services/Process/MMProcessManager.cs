namespace MindEngine.Core.Services.Process
{
    using System;
    using Components;
    using Debug;
    using Microsoft.Xna.Framework;
    using Scenes.Entity;
    using Utils;

    /// <summary>
    /// This is the interface used in the manager class. It provide a minimal 
    /// version of process in the operating system.
    /// </summary>
    public interface IMMProcessManagerItem : IComparable<IMMProcessManagerItem>, IDisposable
    {
        #region Process States

        MMProcessState State { get; }

        #endregion

        /// Process events are scheduled by the process manager. It should not 
        /// be called by anything else.

        #region Process Event Operations

        void OnEnter();

        void OnExit();

        void OnUpdate(GameTime time);

        void OnWait(GameTime time);

        #endregion

        #region Process State Operations

        void Enter();

        #endregion

        #region Process Priority Semantics

        MMProcessCategory Category { get; }

        int Priority { get; }

        #endregion
    }

    public interface IMMProcessManager : IMMGameComponent
    {
        void AttachProcess(IMMProcessManagerItem process);
    }

    public class MMProcessManager : GameComponent, IMMProcessManager
    {
        #region Process Data

        private MMSortingCollection<IMMProcessManagerItem> processes 
            = new MMSortingCollection<IMMProcessManagerItem>(
                (item1, item2) => item1.CompareTo(item2), 
                (item, handler) => {}, 
                (item, handler) => {}); 

        #endregion Process Data

        #region Constructors

        public MMProcessManager(MMEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }
        }

        #endregion Constructors

        #region Deconstruction

        ~MMProcessManager()
        {
            this.Dispose(true);
        }

        #endregion Deconstruction

        #region Update 

        public override void Update(GameTime time)
        {
            using (new MMDebugBlockTimer())
            {
                this.processes.ForEach((processParam, timeParam) =>
                {
                    switch (processParam.State)
                    {
                        case MMProcessState.New:
                        {
                            // The process normally won't call Enter themselves.
                            processParam.Enter();
                            processParam.OnEnter();
                            break;
                        }

                        case MMProcessState.Running:
                        {
                            processParam.OnUpdate(time);
                            break;
                        }

                        case MMProcessState.Waiting:
                        {
                            processParam.OnWait(time);
                            break;
                        }

                        case MMProcessState.Terminated:
                        {
                            processParam.OnExit();

                            this.processes.Remove(processParam);
                            processParam.Dispose();
                            break;
                        }

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }, time);
            }
        }

        #endregion Update

        #region Operations

        public void AttachProcess(IMMProcessManagerItem process)
        {
            this.processes.Add(process);
        }

        #endregion Operations

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.processes?.Clear();
                this.processes = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
