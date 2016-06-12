namespace MindEngine.Core.Services.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public class MMEventManager : GameComponent, IMMEventManager
    {
        #region Event Data

        private List<IMMEvent> activeEvents;

        private List<int> knownEvents;

        private List<IMMEventListener> listeners;

        private List<IMMEvent> queuedEvents;

        public List<int> KnownEvents => this.knownEvents;

        public List<IMMEventListener> Listeners => this.listeners;

        #endregion

        #region Constructors

        public MMEventManager(MMEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }
            
            this.knownEvents  = new List<int>();
            this.queuedEvents = new List<IMMEvent>();
            this.activeEvents = new List<IMMEvent>();
            this.listeners    = new List<IMMEventListener>();
        }

        #endregion Constructors

        #region Initialization

        public override void Initialize()
        {
        }

        #endregion Initialization

        #region Listener Operations

        public void AddListener(IMMEventListener listener)
        {
            if (!this.listeners.Contains(listener))
            {
                this.listeners.Add(listener);
                foreach (var eventType in listener.RegisteredEvents)
                {
                    this.AddKnownEvent(eventType);
                }
            }
        }

        public void RemoveListener(IMMEventListener listener)
        {
            if (this.listeners.Contains(listener))
            {
                this.listeners.Remove(listener);
            }
        }

        #endregion Listener Operations

        #region Event Operations

        private void AddKnownEvent(int eventType)
        {
            if (!this.knownEvents.Contains(eventType))
            {
                this.knownEvents.Add(eventType);
            }
        }

        public void QueueEvent(IMMEvent e)
        {
            // won't validate event before adding it
            // which allow queuing events before adding listeners for it
            e.CreationTime = DateTime.Now.Ticks;
            this.queuedEvents.Add(e);
        }

        public void QueueUniqueEvent(IMMEvent e)
        {
            // won't validate event before adding it
            // which allow queuing events before adding listeners for it
            for (var x = this.queuedEvents.Count - 1; x >= 0; x--)
            {
                if (this.queuedEvents[x].Type == e.Type)
                {
                    this.queuedEvents.RemoveAt(x);
                }
            }

            this.queuedEvents.Add(e);
        }

        public void RemoveQueuedEvent(IMMEvent e, bool allOccurances)
        {
            if (this.queuedEvents.Contains(e))
            {
                if (allOccurances)
                {
                    for (var x = this.queuedEvents.Count; x >= 0; x--)
                    {
                        this.queuedEvents.RemoveAt(x);
                    }
                }
                else
                {
                    this.queuedEvents.Remove(e);
                }
            }
        }

        public void TriggerEvent(IMMEvent e)
        {
            if (this.ValidateEvent(e.Type))
            {
                foreach (var listener in this.listeners
                                             .Where(listener => listener.RegisteredEvents.Contains(e.Type))
                                             .ToList())
                {
                    listener.HandleEvent(e);
                }
            }
        }

        public bool ValidateEvent(int eventType)
        {
            return this.knownEvents.Contains(eventType);
        }

        #endregion Event Operations

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (this.queuedEvents.Count == 0)
            {
                return;
            }

            var startTime = DateTime.Now.Ticks;

            // Max of 5 milliseconds of processing time. 1ms = 10,000 ticks
            var stopTime = startTime + 50000;

            // Copy the queued events to the active events list
            this.activeEvents = new List<IMMEvent>(this.queuedEvents);
            this.queuedEvents.Clear();

            // Process at least one event..or so Mr. McShaffry says
            var counter = this.activeEvents.Count - 1;
            do
            {
                if (counter < 0)
                {
                    break;
                }

                var currentTime = DateTime.Now.Ticks;
                var @event = this.activeEvents[counter];

                ++@event.HandleAttempts;

                for (var i = this.listeners.Count - 1; i >= 0; --i)
                {
                    if (this.listeners[i].RegisteredEvents.Contains(@event.Type) && 
                        this.listeners[i].HandleEvent(@event))
                    {
                        @event.Handled = true;
                    }
                }

                // If an event has been around for longer than 3 seconds, remove it
                // should change to 2 attempts?
                if (currentTime >= @event.CreationTime + this.SecondsToTicks(@event.LifeTime))
                {
                    @event.Handled = true;
                }

                if (@event.Handled)
                {
                    this.activeEvents.Remove(@event);
                }

                counter--;

                if (DateTime.Now.Ticks >= stopTime)
                {
                    break;
                }
            }
            while (this.activeEvents.Count > 0);

            // Add back any not handled events
            if (this.activeEvents.Count > 0)
            {
                this.queuedEvents.AddRange(this.activeEvents);
            }
        }

        #endregion Update

        #region Time Helper

        private long SecondsToTicks(int second)
        {
            return second * TimeSpan.TicksPerSecond;
        }

        #endregion Time Helper

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            this.listeners?.Clear();
            this.listeners = null;

            base.Dispose(disposing);
        }

        #endregion
    }
}