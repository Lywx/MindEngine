namespace MindEngine.Core.Services.Process
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMProcessWaitDuration : MMProcess
    {
        public MMProcessWaitDuration(TimeSpan duration)
            : base("Process (Wait Duration)", MMProcessCategory.User)
        {
            this.DurationTotal = duration;
        }

        public TimeSpan DurationTotal { get; }

        public TimeSpan DurationCurrent { get; private set; }

        public override void OnUpdate(GameTime time)
        {
            this.DurationCurrent -= time.ElapsedGameTime;

            if (this.DurationCurrent <= TimeSpan.Zero)
            {
                this.Exit();
            }
        }

        public override void OnEnter()
        {
            this.DurationCurrent = this.DurationTotal;
        }
    }
}
