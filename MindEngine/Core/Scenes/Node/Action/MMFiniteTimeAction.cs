namespace MindEngine.Core.Scenes.Node.Action
{
    using Behavior;

    public abstract class MMFiniteTimeAction : MMNodeBehavior
    {
        #region Constructors

        protected MMFiniteTimeAction()
            : this(0)
        {
        }

        protected MMFiniteTimeAction(float duration)
        {
            this.Duration = duration;
        }

        #endregion Constructors

        #region Duration Data

        float duration;

        public float Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                // Prevent division by 0
                if (value == 0f)
                {
                    this.duration = float.Epsilon;
                }

                this.duration = value;
            }
        }

        #endregion Properties

        #region Operations

        protected internal override MMNodeBehaviorState StartAction(IMMNode target)
        {
            return new MMFiniteTimeActionState(this, target);
        }

        #endregion
    }
}