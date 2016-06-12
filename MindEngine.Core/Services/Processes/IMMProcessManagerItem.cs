namespace MindEngine.Core.Services.Processes
{
    using System;

    /// <summary>
    /// This is the interface used in the manager class.
    /// </summary>
    public interface IMMProcessManagerItem : IDisposable, IMMUpdateableOperations
    {
        #region Process States

        MMProcessState State { get; }

        bool IsUninitialized { get; }

        bool IsAlive { get; }

        bool IsDead { get; }

        bool IsRemoved { get; }

        #endregion

        /// Process events are scheduled by the process manager. It should not 
        /// be called by anything else.

        #region Process Event Operations

        /// <summary>
        /// Scheduled abort operation.
        /// </summary>
        void OnAbort();

        /// <summary>
        /// Scheduled fail operation.
        /// </summary>
        void OnFail();

        /// <summary>
        /// Scheduled init operation.
        /// </summary>
        void OnInit();

        /// <summary>
        /// Scheduled succeed operation.
        /// </summary>
        void OnSucceed();

        #endregion

        #region Process State Operations

        void Abort();

        void Fail();

        void Pause();

        void Resume();

        void Succeed();

        #endregion

        #region Process Priority Semantics

        MMProcessCategory Category { get; }

        int Priority { get; }

        #endregion

        #region Process Chain Semantics

        T RemoveChild<T>() where T : IMMProcessManagerItem;

        #endregion
    }
}