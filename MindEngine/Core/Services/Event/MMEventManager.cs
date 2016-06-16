namespace MindEngine.Core.Services.Event
{
    using System;
    using System.Collections.Generic;
    using Components;
    using Microsoft.Xna.Framework;

    public interface IMMEventManager : IMMGameComponent
    {
        void AttachListener(MMEventListener listener);

        void RemoveListener(MMEventListener listener);

        void QueueEvent(MMEvent e);

        void QueueUniqueEvent(MMEvent e);

        void RemoveQueuedEvent(MMEvent e, bool allOccurances);

        void TriggerEvent(MMEvent e);
    }

    public class MMEventManager : GameComponent, IMMEventManager
    {
        /// <summary>
        /// 5 Millisecond per frame. Max of 5 milliseconds of processing 
        /// time. 1ms = 10,000 ticks.
        /// </summary>
        public int UpdateTickAllowed = 50000;

        #region Constructors

        public MMEventManager(MMEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            this.EventsKnown = new List<int>();
            this.EventsQueued = new List<MMEvent>();
            this.EventsActive = new List<MMEvent>();
            this.Listeners = new List<MMEventListener>();
        }

        #endregion Constructors

        #region Initialization

        public override void Initialize() {}

        #endregion Initialization

        #region Update

        public override void Update(GameTime time)
        {
            if (this.EventsQueued.Count == 0)
            {
                return;
            }

            var updateBeginTime = DateTime.Now.Ticks;

            var updateEndTime = updateBeginTime + this.UpdateTickAllowed;

            // Copy the queued events to the active events list
            this.EventsActive = new List<MMEvent>(this.EventsQueued);
            this.EventsQueued.Clear();

            // Process at least one event..or so Mr. McShaffry says
            var count = this.EventsActive.Count - 1;
            do
            {
                if (count < 0)
                {
                    break;
                }

                var currentTime = DateTime.Now.Ticks;
                var currentEvent = this.EventsActive[count];

                for (var i = this.Listeners.Count - 1; i >= 0; --i)
                {
                    if (this.Listeners[i].EventsKnown.Contains(currentEvent.Signature)
                        &&
                        this.Listeners[i].HandleEvent(currentEvent))
                    {
                        currentEvent.Handled = true;
                    }
                }

                // If an event has been around for longer than 3 seconds, remove it
                // should change to 2 attempts?
                if (currentTime >= currentEvent.Timestamp + MMEventHelper.SecondsToTicks(currentEvent.Duration))
                {
                    currentEvent.Handled = true;
                }

                if (currentEvent.Handled)
                {
                    this.EventsActive.Remove(currentEvent);
                }

                count--;

                if (DateTime.Now.Ticks >= updateEndTime)
                {
                    break;
                }
            }
            while (this.EventsActive.Count > 0);

            // Add back any not handled events
            if (this.EventsActive.Count > 0)
            {
                this.EventsQueued.AddRange(this.EventsActive);
            }
        }

        #endregion Update

        #region Event Data

        private List<MMEvent> EventsActive { get; set; }

        /// <summary>
        ///     Known events registered to provide a way to efficiently discard
        ///     never been cared events (events queued without listeners)
        /// </summary>
        private List<int> EventsKnown { get; }

        private List<MMEventListener> Listeners { get; set; }

        private List<MMEvent> EventsQueued { get; }

        #endregion

        #region Listener Operations

        public void AttachListener(MMEventListener listener)
        {
            if (!this.Listeners.Contains(listener))
            {
                this.Listeners.Add(listener);

                // Add listener's known event into manager's known events
                foreach (var eventType in listener.EventsKnown)
                {
                    this.AddKnownEvent(eventType);
                }
            }
        }

        public void RemoveListener(MMEventListener listener)
        {
            if (this.Listeners.Contains(listener))
            {
                this.Listeners.Remove(listener);
            }
        }

        #endregion Listener Operations

        #region Event Operations

        private void AddKnownEvent(int eventType)
        {
            if (!this.EventsKnown.Contains(eventType))
            {
                this.EventsKnown.Add(eventType);
            }
        }

        public void QueueEvent(MMEvent e)
        {
            // Won't validate event before adding it
            // which allow queuing events before adding listeners for it
            e.Timestamp = DateTime.Now.Ticks;
            this.EventsQueued.Add(e);
        }

        public void QueueUniqueEvent(MMEvent e)
        {
            // Won't validate event before adding it
            // which allow queuing events before adding listeners for it
            for (var x = this.EventsQueued.Count - 1; x >= 0; x--)
            {
                if (this.EventsQueued[x].Signature == e.Signature)
                {
                    this.EventsQueued.RemoveAt(x);
                }
            }

            this.EventsQueued.Add(e);
        }

        public void RemoveQueuedEvent(MMEvent e, bool allOccurances)
        {
            if (this.EventsQueued.Contains(e))
            {
                if (allOccurances)
                {
                    for (var x = this.EventsQueued.Count; x >= 0; x--)
                    {
                        this.EventsQueued.RemoveAt(x);
                    }
                }
                else
                {
                    this.EventsQueued.Remove(e);
                }
            }
        }

        public void TriggerEvent(MMEvent e)
        {
            if (this.ValidateEvent(e.Signature))
            {
                foreach (var listener in this.Listeners)
                {
                    if (listener.EventsKnown.Contains(e.Signature))
                    {
                        listener.HandleEvent(e);
                    }
                }
            }
        }

        public bool ValidateEvent(int eventType)
        {
            return this.EventsKnown.Contains(eventType);
        }

        #endregion Event Operations

        #region Time Helper

        #endregion Time Helper

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.Listeners?.Clear();
                        this.Listeners = null;
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
