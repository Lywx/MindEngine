namespace MindEngine.Core.Utils
{
    using Microsoft.Xna.Framework;

    public class MMFrameCounterInstance 
    {
        public MMFrameCounterInstance(int frameCount)
        {
            this.FrameCount = frameCount;
        }

        public MMFrameCounterState State { get; set; } = MMFrameCounterState.Stopped;

        public int FrameCount { get; private set; }

        public int FrameCurrent { get; private set; } = 0;

        public int FrameLast => this.FrameCount - 1;

        public double FrameProgress => (double)this.FrameCurrent / this.FrameCount;

        public void Update(GameTime time)
        {
            if (this.State == MMFrameCounterState.Running 
                && this.FrameCurrent < this.FrameCount)
            {
                ++this.FrameCurrent;
            }
        }

        public void Start()
        {
            this.State = MMFrameCounterState.Running;
        }
    }
}