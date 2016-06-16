namespace MindEngine.Core.Services.Event
{
    public interface IMMEvent
    {
        /// <summary>
        /// The type signature of event being sent.
        /// </summary>
        int Signature { get; set; }

        /// <summary>
        /// Any type of data that needs to go with the event. Can be an object, a value, null, etc.
        /// </summary>
        object Data { get; set; }

        /// <summary>
        /// Tick number when entering the event queue.
        /// </summary>
        long Timestamp { get; set; }

        /// <summary>
        /// The length in seconds the event should stay alive if not picked up.
        /// </summary>
        int Duration { get; set; }

        bool Handled { get; set; }
    }

    public class MMEvent : IMMEvent
    {
        public MMEvent(int signature, object data, int duration = 1)
        {
            this.Signature = signature;
            this.Data      = data;
            this.Duration  = duration;
        }

        public int Signature { get; set; }

        public object Data { get; set; }

        public long Timestamp { get; set; }

        public int Duration { get; set; }

        public bool Handled { get; set; }
    }
}
