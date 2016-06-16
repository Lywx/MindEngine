namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using Microsoft.Xna.Framework;

    public class MMMoveByState : MMFiniteTimeActionState
    {
        public MMMoveByState(MMMoveBy action, IMMNode target)
            : base(action, target)
        {
            this.DeltaLocation = action.DeltaLocation;
            this.PreviousLocation = this.StartLocation = target.NodeLocation;
        }

        protected Vector3 PreviousLocation { get; set; }

        protected Vector3 DeltaLocation { get; set; }

        protected Vector3 StartLocation { get; set; }

        protected Vector3 EndLocation { get; set; }

        public override void Progress(float percent)
        {
            if (this.Target == null)
            {
                return;
            }

            var deltaLocation = this.Target.NodeLocation - this.PreviousLocation;

            // Update start location per update
            this.StartLocation = this.StartLocation + deltaLocation;

            var newLocation = this.StartLocation + this.DeltaLocation * percent;
            this.Target.NodeLocation = newLocation;
            this.PreviousLocation = newLocation;
        }
    }

    public class MMMoveBy : MMFiniteTimeAction
    {
        #region Constructors

        public MMMoveBy(float duration, Vector3 deltaLocation) : base(duration)
        {
            this.DeltaLocation = deltaLocation;
        }

        #endregion Constructors

        public Vector3 DeltaLocation { get; }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMMoveByState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMMoveBy(this.Duration, -this.DeltaLocation);
        }
    }
}
