namespace MindEngine.Core.Services.Processes
{
    using System;
    using Microsoft.Xna.Framework;

    // TODO(Wuxiang): Extract something common with the cocos2d-x schedule system and think about the possible ways to integrate them
    public class MMProcessInstance_Wait : MMProcess
    {
        public readonly TimeSpan TotalDuration;

        public MMProcessInstance_Wait(TimeSpan duration) 
            : base("Process (Wait)", MMProcessCategory.User)
        {
            this.TotalDuration = duration;
        }

        public TimeSpan Duration { get; private set; }

        public override void Update(GameTime time)
        {
            base.Update(time);

            this.Duration -= time.ElapsedGameTime;
            if (this.Duration <= TimeSpan.Zero)
            {
                this.Succeed();
            }
        }

        public override void OnInit()
        {
            base.OnInit();

            this.Duration = this.TotalDuration;
        }

        public override void OnSucceed()
        {
        }

        public override void OnFail()
        {
        }

        public override void OnAbort()
        {
        }
    }
}