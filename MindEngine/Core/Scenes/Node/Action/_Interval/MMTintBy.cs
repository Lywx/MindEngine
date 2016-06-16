namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    public class MMTintBy : MMFiniteTimeAction
    {
        #region Constructors

        public MMTintBy(
            float duration,
            short RDelta,
            short GDelta,
            short BDelta) : base(duration)
        {
            this.RDelta = RDelta;
            this.GDelta = GDelta;
            this.BDelta = BDelta;
        }

        #endregion Constructors

        public short BDelta { get; }

        public short GDelta { get; }

        public short RDelta { get; }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMTintByState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMTintBy(
                this.Duration,
                (short)-this.RDelta,
                (short)-this.GDelta,
                (short)-this.BDelta);
        }
    }
}