namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using Microsoft.Xna.Framework;

    public class MMScaleByState : MMScaleToState
    {
        public MMScaleByState(MMScaleTo action, IMMNode target)
            : base(action, target)
        {
            this.DeltaScale = this.StartScale * this.EndScale - this.StartScale;
        }
    }

    public class MMScaleBy : MMScaleTo
    {
        #region Constructors

        public MMScaleBy(float duration, Vector3 scale) : base(duration, scale) {}

        #endregion Constructors

        #region Operations

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMScaleByState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMScaleBy(this.Duration, new Vector3(1 / this.EndScale.X, 1 / this.EndScale.Y, 1 / this.EndScale.Z));
        }

        #endregion
    }
}
