namespace MindEngine.Core.Services.Events
{
    using System;
    using System.Collections.Generic;

    public class MMEventListener : IMMEventListener
    {
        private readonly List<int> registeredEvents;

        private readonly Func<IMMEvent, bool> handleEvents;

        public MMEventListener(List<int> registeredEvents, Func<IMMEvent, bool> handleEvents)
        {
            this.handleEvents     = handleEvents;
            this.registeredEvents = registeredEvents;
        }

        protected MMEventListener()
        {
            this.registeredEvents = new List<int>();
        }

        public List<int> RegisteredEvents => this.registeredEvents;

        public virtual bool HandleEvent(IMMEvent e)
        {
            if (this.handleEvents != null)
            {
                return this.handleEvents(e);
            }

            return false;
        }
    }
}