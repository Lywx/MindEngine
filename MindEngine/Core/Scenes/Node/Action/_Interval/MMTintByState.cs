namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;
    using Graphics;
    using Microsoft.Xna.Framework;

    public class MMTintByState : MMFiniteTimeActionState
    {
        public MMTintByState(MMTintBy action, IMMNode target, MMNodeColor targetColor)
            : base(action, target)
        {
            if (targetColor == null)
            {
                throw new ArgumentNullException(nameof(targetColor));
            }

            this.TargetColor = targetColor;

            this.BDelta = action.BDelta;
            this.GDelta = action.GDelta;
            this.RDelta = action.RDelta;

            this.RFrom = targetColor.Raw.R;
            this.GFrom = targetColor.Raw.G;
            this.BFrom = targetColor.Raw.B;
        }

        protected MMNodeColor TargetColor { get; set; }

        protected short BDelta { get; set; }

        protected short GDelta { get; set; }

        protected short RDelta { get; set; }

        protected short BFrom { get; set; }

        protected short GFrom { get; set; }

        protected short RFrom { get; set; }

        public override void Progress(float percent)
        {
            this.TargetColor.Raw = new Color(
                (byte)(this.RFrom + this.RDelta * percent),
                (byte)(this.GFrom + this.GDelta * percent),
                (byte)(this.BFrom + this.BDelta * percent));
        }
    }
}