namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    public class MMFadeOutState : MMFiniteTimeActionState
    {
        public MMFadeOutState(MMFadeOut action, IMMNode target)
            : base(action, target) {}

        public override void Progress(float percent)
        {
            if (this.Target != null)
            {
                this.Target.NodeOpacity.Raw = (byte)(255 * (1 - percent));
            }
        }
    }

    public class MMFadeOut : MMFiniteTimeAction
    {
        #region Constructors

        public MMFadeOut(float durtaion) : base(durtaion) {}

        #endregion Constructors

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMFadeOutState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMFadeIn(this.Duration);
        }
    }
}
