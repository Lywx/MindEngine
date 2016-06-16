namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    public class MMRotateByState : MMFiniteTimeActionState
    {
        #region Constructors

        public MMRotateByState(MMRotateBy action, IMMNode target)
            : base(action, target)
        {
            this.Angle = action.Angle;

            this.StartAngleX = target.RotationX;
            this.StartAngleY = target.RotationY;
        }

        #endregion

        public override void Progress(float percent)
        {
            // XXX: shall I add % 360
            if (this.Target != null)
            {
                this.Target.RotationX = this.StartAngleX + this.Angle * percent;
                this.Target.RotationY = this.StartAngleY + this.AngleY * percent;
            }
        }

        #region

        protected float Angle { get; set; }

        protected float StartAngleX { get; set; }

        #endregion
    }

    public class MMRotateBy : MMFiniteTimeAction
    {
        public float Angle { get; }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMRotateByState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMRotateBy(this.Duration, -this.Angle);
        }

        #region Constructors

        public MMRotateBy(float duration, float deltaAngle) : base(duration)
        {
            this.Angle = deltaAngle;
        }

        #endregion Constructors
    }
}
