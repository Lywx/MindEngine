namespace MindEngine.Core.Scenes.Entity
{
    using System;
    using Services.Process;

    public class MMTimeSchedule : MMEntitySchedule
    {
        public MMTimeSchedule(TimeSpan duration)
        {
            this.Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        public override void AttachBehavior(Action behavior)
        {
            this.EngineInterop.Process.AttachProcess(
                new MMProcessChain(
                    new MMProcessWaitDuration(this.Duration), 
                    new MMProcessInstant(behavior)));
        }
    }

    public class MMEventSchedule : MMEntitySchedule
    {
        public MMEventSchedule(int eventType)
        {
            this.EventType = eventType;
        }

        public int EventType { get; set; }

        public override void AttachBehavior(Action behavior)
        {
            this.EngineInterop.Process.AttachProcess(
                new MMProcessChain(
                    new MMProcessWaitEvent(this.EventType), 
                    new MMProcessInstant(behavior)));
        }
    }

    public interface IMMEntitySchedule
    {
        void AttachBehavior(Action behavior);
    }

    public abstract class MMEntitySchedule : MMObject, IMMEntitySchedule
    {
        public abstract void AttachBehavior(Action behavior);
    }
}
