namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;

    public class MMRotateToState : MMFiniteTimeActionState
    {
        private float DiffAngleY;

        private float DiffAngleX;

        private float StartAngleX;

        private float StartAngleY;

        public MMRotateToState(MMRotateTo action, IMMNode target)
            : base(action, target)
        {
            this.DistanceAngleX = action.DistanceAngleX;
            this.DistanceAngleY = action.DistanceAngleY;

            // Calculate X
            this.StartAngleX = this.Target.RotationX;
            if (this.StartAngleX > 0)
            {
                this.StartAngleX = this.StartAngleX % 360.0f;
            }
            else
            {
                this.StartAngleX = this.StartAngleX % -360.0f;
            }

            this.DiffAngleX = this.DistanceAngleX - this.StartAngleX;
            if (this.DiffAngleX > 180)
            {
                this.DiffAngleX -= 360;
            }
            if (this.DiffAngleX < -180)
            {
                this.DiffAngleX += 360;
            }

            //Calculate Y: It's duplicated from calculating X since the rotation wrap should be the same
            this.StartAngleY = this.Target.RotationY;

            if (this.StartAngleY > 0)
            {
                this.StartAngleY = this.StartAngleY % 360.0f;
            }
            else
            {
                this.StartAngleY = this.StartAngleY % -360.0f;
            }

            this.DiffAngleY = this.DistanceAngleY - this.StartAngleY;
            if (this.DiffAngleY > 180)
            {
                this.DiffAngleY -= 360;
            }

            if (this.DiffAngleY < -180)
            {
                this.DiffAngleY += 360;
            }
        }

        protected float DistanceAngleX { get; set; }

        protected float DistanceAngleY { get; set; }

        public override void Progress(float percent)
        {
            if (this.Target != null)
            {
                this.Target.RotationX = this.StartAngleX + this.DiffAngleX * percent;
                this.Target.RotationY = this.StartAngleY + this.DiffAngleY * percent;
            }
        }
    }

    public class MMRotateTo : MMFiniteTimeAction
    {
        public float DistanceAngleX { get; private set; }
        public float DistanceAngleY { get; private set; }

        #region Constructors

        public MMRotateTo(float duration, float deltaAngleX, float deltaAngleY) : base(duration)
        {
            this.DistanceAngleX = deltaAngleX;
            this.DistanceAngleY = deltaAngleY;
        }

        public MMRotateTo(float duration, float deltaAngle) : this(duration, deltaAngle, deltaAngle)
        {
        }

        #endregion Constructors

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMRotateToState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }
    }
}