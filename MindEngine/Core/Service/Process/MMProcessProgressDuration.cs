namespace MindEngine.Core.Service.Process
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMProcessProgressDuration : MMProcess
    {
        public MMProcessProgressDuration(TimeSpan duration, Action<float> behavior) 
            : base("Process (Progress Duration)", MMProcessCategory.User)
        {
            if (behavior == null)
            {
                throw new ArgumentNullException(nameof(behavior));

            }

            this.Duration = duration;
            this.Behavior = behavior;
        }

        public TimeSpan Duration { get; set; }

        public Action<float> Behavior { get; set; }

        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;

        public override void OnUpdate(GameTime time)
        {
            this.Elapsed += time.ElapsedGameTime;

            // Avoid divided by zero
            var duration = this.Duration == TimeSpan.Zero
                               ? TimeSpan.FromTicks(1)
                               : this.Duration;
            var percent = (float)this.Elapsed.Ticks / duration.Ticks;

            // Avoid progress too much
            percent = Math.Min(1, percent);

            // Avoid progress negative value
            percent = Math.Max(0, percent);

            this.OnProgress(percent);
        }

        protected virtual void OnProgress(float percent)
        {
            if (percent >= 1f)
            {
                this.Exit();
            }

            this.Behavior.Invoke(percent);
        }
    }
}