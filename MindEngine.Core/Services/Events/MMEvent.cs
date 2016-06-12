namespace MindEngine.Core.Services.Events
{
    public class MMEvent : IMMEvent
    {
        /// <summary>
        ///     Base class for MMGame Events
        /// </summary>
        /// <param name="type">The type of event being sent</param>
        /// <param name="data">Any type of data that needs to go with the event.  Can be an object, a value, null, etc</param>
        /// <param name="lifeTime">The length in Seconds the event should stay alive if not picked up</param>
        public MMEvent(int type, object data, int lifeTime = 1, int handleAttempts = 0)
        {
            this.Type = type;
            this.Data = data;
            this.LifeTime = lifeTime;
            this.HandleAttempts = handleAttempts;
        }

        public int Type { get; set; }

        public object Data { get; set; }

        public long CreationTime { get; set; }

        public int LifeTime { get; set; }

        public bool Handled { get; set; }

        public int HandleAttempts { get; set; }
    }
}
