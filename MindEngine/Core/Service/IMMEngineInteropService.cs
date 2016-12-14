namespace MindEngine.Core.Service
{
    using System;
    using System.Collections.Generic;
    using Content.Asset;
    using Event;
    using IO.Directory;
    using Process;
    using Save;
    using Scripting;
    using State;

    public enum MMTaskMode
    {
        Synchronous,

        Asynchronous,

        Hybrid,
    }

    public enum MMTaskTimeline
    {
        Setup,

        Shutdown,

        User
    }

    public class MMTask : IMMTask
    {
        public MMTask(MMTaskTimeline timeline, MMTaskMode mode)
        {
            this.Timeline = timeline;
            this.Mode     = mode;
        }

        public MMTaskTimeline Timeline { get; }

        public MMTaskMode Mode { get; }

        public Func<MMTask> Work { get; set; }
    } 

    public interface IMMTask
    {
        MMTaskMode Mode { get; }

        MMTaskTimeline Timeline { get; }

        Func<MMTask> Work { get; set; }
    }

    public interface IMMTaskManager
    {
        void AttachTask(Action action, MMTaskMode schedule);
    }

    public class MMTaskManager : IMMTaskManager
    {
        List<MMTask> Tasks { get; set; } = new List<MMTask>();

        public void AttachTask(IMMTask task)
        {
            this.Tasks.Add(task);
        }

        public void RemoveTask(IMMTask task)
        {
            this.Tasks.Remove(task);
        }

        public void Initialize()
        {
        }
    }

    public interface IMMEngineInteropService
    {
        MMAssetManager Asset { get; }

        MMDirectoryManager File { get; }

        MMEventManager Event { get; }

        MMGameManager Game { get; }

        MMEngine Engine { get; }

        IMMScriptManager Script { get; set; }

        MMProcessManager Process { get; }

        MMScreenManager Screen { get; }

        /// <remarks>
        ///     Save that is replaceable in specific game
        /// </remarks>
        MMSaveManager Save { get; set; }
    }
}
