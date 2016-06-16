namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    public class MMRepeatState : MMFiniteTimeActionState
    {
        #region Constructors

        public MMRepeatState(MMRepeat action, IMMNode target)
            : base(action, target)
        {
            this.InnerAction = action.InnerAction;
            this.InnerActionState = (MMFiniteTimeActionState)this.InnerAction.StartAction(target);

            this.Times = action.Times;
            this.Count = action.Count;

            this.NextDt = this.InnerAction.Duration / this.Duration;
        }

        #endregion

        public bool IsInstant => (this.InnerAction is MMRepeat) && ((MMRepeat)this.InnerAction).IsInstant;

        public override bool IsDone => this.Count == this.Times;

        protected float NextDt { get; set; }

        protected MMFiniteTimeAction InnerAction { get; set; }

        protected MMFiniteTimeActionState InnerActionState { get; set; }

        protected uint Times { get; set; }

        protected uint Count { get; set; }

        protected internal override void Stop()
        {
            this.InnerActionState.Stop();
            base                 .Stop();
        }

        public override void Progress(float percent)
        {
            // When the repeat should be finished
            if (percent >= this.NextDt)
            {
                // When the repeat duration is reached but the times remain incomplete 
                while (percent > this.NextDt
                       && this.Count < this.Times)
                {
                    // Do extra work to complete 1 time in this frame
                    this.InnerActionState.Progress(1.0f);

                    ++this.Count;

                    this.InnerActionState.Stop();
                    this.InnerActionState = (MMFiniteTimeActionState)this.InnerAction.StartAction(this.Target);

                    // TODO(Unknown):
                    this.NextDt = this.InnerAction.Duration / this.Duration * (this.Count + 1f);
                }

                // fix for issue #1288, incorrect end value of repeat
                if (percent >= 1.0f
                    && this.Count < this.Times)
                {
                    ++this.Count;
                }

                // don't set an instant action back or update it, it has no use because it has no duration
                if (!this.IsInstant)
                {
                    if (this.Count == this.Times)
                    {
                        this.InnerActionState.Progress(1f);
                        this.InnerActionState.Stop();
                    }
                    else
                    {
                        // issue #390 prevent jerk, use right update
                        this.InnerActionState.Progress(percent - (this.NextDt - this.InnerAction.Duration / this.Duration));
                    }
                }
            }
            else
            {
                this.InnerActionState.Progress((percent * this.Times) % 1.0f);
            }
        }
    }
}