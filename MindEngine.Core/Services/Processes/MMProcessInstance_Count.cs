namespace MindEngine.Core.Services.Processes
{
    using Microsoft.Xna.Framework;
    using Utils;

    public class MMProcessInstance_Count : MMProcess
    {
        private readonly MMFrameCounterInstance counterInstance;

        public MMProcessInstance_Count(MMFrameCounter counter)
            : base("Process (Count)", MMProcessCategory.User)
        {
            this.counterInstance = counter.CreateInstance();
        }

        public override void OnInit()
        {
            base.OnInit();

            this.counterInstance.Start();
        }

        public override void OnAbort()
        {
        }

        public override void OnFail()
        {
        }

        public override void OnSucceed()
        {
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            this.counterInstance.Update(time);

            // When the sound instance is finished playing
            if (this.counterInstance.State == MMFrameCounterState.Stopped)
            {
                this.Succeed();
            }
        }
    }
}
