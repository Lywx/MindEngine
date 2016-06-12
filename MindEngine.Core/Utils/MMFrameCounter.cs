namespace MindEngine.Core.Utils
{
    public class MMFrameCounter
    {
        public MMFrameCounter(int count)
        {
            this.FrameCount = count;
        }

        public int FrameCount { get; private set; }

        public MMFrameCounterInstance CreateInstance()
        {
            return new MMFrameCounterInstance(this.FrameCount);
        }
    }
}