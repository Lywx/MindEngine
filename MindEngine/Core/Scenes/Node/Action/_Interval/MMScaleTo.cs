namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using Microsoft.Xna.Framework;

    public class MMScaleToState : MMFiniteTimeActionState
    {
        protected Vector3 DeltaScale;

        protected Vector3 EndScale;

        protected Vector3 StartScale;

        public MMScaleToState(MMScaleTo action, IMMNode target)
            : base(action, target)
        {
            this.StartScale = target.NodeScale;
            this.EndScale = action.EndScale;
            this.DeltaScale = this.EndScale - this.StartScale;
        }

        public override void Progress(float percent)
        {
            if (this.Target != null)
            {
                this.Target.NodeScale = this.StartScale + this.DeltaScale * percent;
            }
        }
    }

    public class MMScaleTo : MMFiniteTimeAction
    {
        #region Constructors

        public MMScaleTo(float duration, Vector3 scale) : base(duration)
        {
            this.EndScale = scale;
        }

        #endregion Constructors

        public Vector3 EndScale { get; }

        public override MMFiniteTimeAction Reverse()
        {
            throw new System.NotImplementedException();
        }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMScaleToState(this, target);
        }
    }
}
