namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    public class MMBlinkState : MMFiniteTimeActionState
    {
        #region Constructors

        public MMBlinkState(MMBlink action, IMMNode target)
            : base(action, target)
        {
            this.Times = action.Times;
            this.NodeVisibleOriginal = target.NodeVisible;
        }

        #endregion

        protected uint Times { get; set; }

        protected bool NodeVisibleOriginal { get; set; }

        public override void Progress(float percent)
        {
            if (this.Target != null
                && !this.IsDone)
            {
                var slice = 1.0f / this.Times;

                // float m = fmodf(time, slice);
                var m = percent % slice;
                this.Target.NodeVisible = m > (slice / 2);
            }
        }

        protected internal override void Stop()
        {
            this.Target.NodeVisible = this.NodeVisibleOriginal;
            base.Stop();
        }
    }

    public class MMBlink : MMFiniteTimeAction
    {
        #region Constructors

        public MMBlink(float duration, uint timeOfBlinks) : base(duration)
        {
            this.Times = timeOfBlinks;
        }

        #endregion Constructors

        public uint Times { get; }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMBlinkState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMBlink(this.Duration, this.Times);
        }
    }
}