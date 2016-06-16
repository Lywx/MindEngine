namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;
    using Graphics;
    using Microsoft.Xna.Framework;

    public class MMTintToState : MMFiniteTimeActionState
    {
        public MMTintToState(MMTintTo action, IMMNode target, MMNodeColor targetColor)
            : base(action, target)
        {
            if (targetColor == null)
            {
                throw new ArgumentNullException(nameof(targetColor));
            }

            this.TargetColor = targetColor;

            this.ColorTo   = action.ColorTo;
            this.ColorFrom = targetColor.Raw;
        }

        protected Color ColorFrom { get; set; }

        protected Color ColorTo { get; set; }

        protected MMNodeColor TargetColor { get; set; }

        public override void Progress(float percent)
        {
            this.TargetColor.Raw =
                new Color(
                    (byte)(this.ColorFrom.R + (this.ColorTo.R - this.ColorFrom.R) * percent),
                    (byte)(this.ColorFrom.G + (this.ColorTo.G - this.ColorFrom.G) * percent),
                    (byte)(this.ColorFrom.B + (this.ColorTo.B - this.ColorFrom.B) * percent));
        }
    }

    public class MMTintTo : MMFiniteTimeAction
    {
        #region Constructors

        public MMTintTo(float duration, byte red, byte green, byte blue)
            : base(duration)
        {
            this.ColorTo = new Color(red, green, blue);
        }

        #endregion Constructors

        public Color ColorTo { get; }

        public override MMFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMTintToState(this, target, );
        }
    }
}