namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using Instant;

    public class MMRepeat : MMFiniteTimeAction
    {
        #region Constructors

        public MMRepeat(MMFiniteTimeAction action, uint times)
            : base(action.Duration * times)
        {
            this.InnerAction = action;
            this.Times = times;

            // TODO(Unknown): 
            // An instant action needs to be executed one time less in the
            // update method since it uses startWithTarget to execute the action
            if (this.IsInstant)
            {
                this.Times -= 1;
            }
        }

        #endregion Constructors

        #region Properties

        public bool IsInstant => this.InnerAction is MMActionInstant;

        public uint Times { get; }

        public uint Count { get; private set; } = 0;

        public MMFiniteTimeAction InnerAction { get; }

        #endregion Properties

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMRepeatState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMRepeat(this.InnerAction.Reverse(), this.Times);
        }
    }
}
