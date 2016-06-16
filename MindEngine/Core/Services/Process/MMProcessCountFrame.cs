namespace MindEngine.Core.Services.Process
{
    using Microsoft.Xna.Framework;
    using Utils;

    public class MMProcessCountFrame : MMProcess
    {
        private readonly MMFrameCounterInstance counterInstance;

        public MMProcessCountFrame(MMFrameCounter counter)
            : base("Process (Count Frame)", MMProcessCategory.User)
        {
            this.counterInstance = counter.CreateInstance();
        }

        public override void OnEnter()
        {
            this.counterInstance.Start();
        }

        public override void OnUpdate(GameTime time)
        {
            this.counterInstance.Update(time);

            // When the sound instance is finished playing
            if (this.counterInstance.State == MMFrameCounterState.Stopped)
            {
                this.Exit();
            }
        }
    }
}
