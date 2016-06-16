namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    public class MMFadeInState : MMFiniteTimeActionState
    {
        public MMFadeInState(MMFadeIn action, IMMNode target)
            : base(action, target) {}

        public override void Progress(float percent)
        {
            if (this.Target != null)
            {
                this.Target.NodeOpacity.Raw = (byte)(255 * percent);
            }
        }
    }

    public class MMFadeIn : MMFiniteTimeAction
    {
        #region Constructors

        public MMFadeIn(float durataion) : base(durataion) {}

        #endregion Constructors

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMFadeInState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMFadeOut(this.Duration);
        }
    }
}
