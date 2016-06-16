namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;

    public class MMRepeatForeverState : MMFiniteTimeActionState
    {
        #region Constructors

        public MMRepeatForeverState(MMRepeatForever action, IMMNode target)
            : base(action, target)
        {
            this.InnerAction = action.InnerAction;
            this.InnerActionState = (MMFiniteTimeActionState)this.InnerAction.StartAction(target);
        }

        #endregion

        private MMFiniteTimeAction InnerAction { get; }

        private MMFiniteTimeActionState InnerActionState { get; set; }

        public override bool IsDone => false;

        protected internal override void Step(float dt)
        {
            this.InnerActionState.Step(dt);

            // When given duration is passed
            if (this.InnerActionState.IsDone)
            {
                // Possible difference produced in MMFiniteTImeActionState.Step(dt)
                var elapsedOver = this.InnerActionState.Elapsed - this.InnerActionState.Duration;

                // Recreate new state
                this.InnerActionState = (MMFiniteTimeActionState)this.InnerAction.StartAction(this.Target);

                // Compensate for possible discontinuation
                this.InnerActionState.Step(elapsedOver);
            }
        }
    }

    public class MMRepeatForever : MMFiniteTimeAction
    {
        #region Constructors

        public MMRepeatForever(params MMFiniteTimeAction[] actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }

            this.InnerAction = new MMSequence(actions);
        }

        public MMRepeatForever(MMFiniteTimeAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.InnerAction = action;
        }

        #endregion Constructors

        public MMFiniteTimeAction InnerAction { get; }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMRepeatForeverState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMRepeatForever(this.InnerAction.Reverse());
        }
    }
}
