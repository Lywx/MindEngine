namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using Microsoft.Xna.Framework;

    public class MMMoveToState : MMMoveByState
    {
        public MMMoveToState(MMMoveTo action, IMMNode target)
            : base(action, target)
        {
            this.StartLocation = target.NodeLocation;
            this.DeltaLocation = action.EndLocation - target.NodeLocation;
        }

        public override void Progress(float percent)
        {
            if (this.Target != null)
            {
                var newLocation = this.StartLocation + this.DeltaLocation * percent;

                this.Target.NodeLocation = newLocation;
                this.PreviousLocation = newLocation;
            }
        }
    }

    public class MMMoveTo : MMMoveBy
    {
        #region Constructors

        public MMMoveTo(float duration, Vector3 location)
            : base(duration, location)
        {
            this.EndLocation = location;
        }

        #endregion Constructors

        public Vector3 EndLocation { get; private set; }

        #region Operations

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMMoveToState(this, target);
        }

        #endregion
    }
}
