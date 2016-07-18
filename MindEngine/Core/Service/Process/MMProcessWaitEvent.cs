namespace MindEngine.Core.Service.Process
{
    using System.Collections.Generic;
    using Event;

    public class MMProcessWaitEvent : MMProcess
    {
        public MMProcessWaitEvent(int eventType) 
            : base("Process (Wait Event)", MMProcessCategory.User)
        {
            this.EventType = eventType;
        }

        public override void OnEnter()
        {
            this.EventListeners.Add(new MMEventListener(this.EventType, eventParam =>
            {
                this.Exit();
                return true;
            }));

            this.Wait();
        }

        public override void OnExit()
        {
            foreach (var listener in this.EventListeners)
            {
                this.EngineInterop.Event.RemoveListener(listener);
            }
        }

        public int EventType { get; }

        public List<MMEventListener> EventListeners { get; set; }
    }
}