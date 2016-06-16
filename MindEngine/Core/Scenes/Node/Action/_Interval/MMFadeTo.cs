namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;

    public class MMFadeToState : MMFiniteTimeActionState
    {
        #region Constructors

        public MMFadeToState(MMFadeTo action, IMMNode target)
            : base(action, target)
        {
            this.OpacityTo = action.OpacityTo;
            this.OpacityFrom = this.Target.NodeOpacity.Raw;
        }

        #endregion

        protected byte OpacityFrom { get; set; }

        protected byte OpacityTo { get; set; }

        public override void Progress(float percent)
        {
            if (this.Target != null)
            {
                var discrepancy = this.OpacityTo - this.OpacityFrom;

                this.Target.NodeOpacity.Raw = (byte)(this.OpacityFrom + discrepancy * percent);
            }
        }
    }

    public class MMFadeTo : MMFiniteTimeAction
    {
        #region Constructors

        public MMFadeTo(float duration, byte opacity) : base(duration)
        {
            this.OpacityTo = opacity;
        }

        #endregion Constructors

        public byte OpacityTo { get; private set; }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMFadeToState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }
    }
}