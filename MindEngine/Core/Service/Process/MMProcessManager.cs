namespace MindEngine.Core.Service.Process
{
    using System;
    using Component;
    using Microsoft.Xna.Framework;
    using Debug;
    using Util.Collection;

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

        event EventHandler<MMProcessPriorityChangedEventArgs> PriorityChanged;

        #endregion
    }

    public interface IMMProcessManager : IMMGameComponent
    {
        void AttachProcess(IMMProcessManagerItem process);
    }

    public class MMProcessManager : GameComponent, IMMProcessManager
    {
        #region Process Data

        private MMSortingList<IMMProcessManagerItem, MMProcessPriorityChangedEventArgs> Processes { get; set; }
            = new MMSortingList<IMMProcessManagerItem, MMProcessPriorityChangedEventArgs>(
                (process, processOther) => process.CompareTo(processOther), 
                (process, handler) => process.PriorityChanged += handler, 
                (process, handler) => process.PriorityChanged -= handler); 

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
                this.Processes.ForEach(
                    delegate(IMMProcessManagerItem processParam, GameTime timeParam)
                    {
                        switch (processParam.State)
                        {
                            case MMProcessState.Inertial:
                            {
                                break;
                            }

                            case MMProcessState.New:
                            {
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
                                // After this method there should be no reference 
                                // connected to this process except the process 
                                // manager.
                                processParam.OnExit();

                                this.Processes.Remove(processParam);

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
            this.Processes.Add(process);
        }

        #endregion Operations

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Processes?.Clear();
                this.Processes = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
