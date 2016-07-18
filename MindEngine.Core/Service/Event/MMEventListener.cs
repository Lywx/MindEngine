namespace MindEngine.Core.Service.Event
{
    using System;
    using System.Collections.Generic;

    public interface IMMEventListener
    {
        List<int> EventsKnown { get; }

        Func<MMEvent, bool> EventHandler { get; set; }
    }

    public class MMEventListener : IMMEventListener
    {
        public MMEventListener(List<int> eventsKnown, Func<MMEvent, bool> eventHandler)
        {
            this.EventsKnown = eventsKnown;
            this.EventHandler = eventHandler;
        }

        public MMEventListener(int eventKnown, Func<MMEvent, bool> eventHandler)
        {
            this.EventsKnown = new List<int> { eventKnown };
            this.EventHandler = eventHandler;
        }

        public List<int> EventsKnown { get; set; }

        public Func<MMEvent, bool> EventHandler { get; set; }

        public virtual bool HandleEvent(MMEvent e)
        {
            if (this.EventHandler != null)
            {
                return this.EventHandler.Invoke(e);
            }

            return false;
        }
    }
}
