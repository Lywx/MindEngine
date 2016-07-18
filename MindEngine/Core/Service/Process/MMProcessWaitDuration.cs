namespace MindEngine.Core.Service.Process
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMProcessWaitDuration : MMProcess
    {
        public MMProcessWaitDuration(TimeSpan duration)
            : base("Process (Wait Duration)", MMProcessCategory.User)
        {
            this.Duration = duration;
        }

        public TimeSpan Duration { get; }

        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;

        public override void OnUpdate(GameTime time)
        {
            this.Elapsed += time.ElapsedGameTime;

            if (this.Elapsed >= this.Duration)
            {
                this.Exit();
            }
        }

        public override void OnEnter()
        {
        }
    }
}
