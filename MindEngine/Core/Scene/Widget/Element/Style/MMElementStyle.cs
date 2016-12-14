namespace MindEngine.Core.Scene.Widget.Element.Style
{
    using Property;

    /// <summary>
    /// This class describes a rectangular region, in which is a non-interactive
    /// graphical unit. This basically describes a textured rectangle region.
    /// </summary>
    public class MMElementStyle : MMWidgetDesign
    {
        public MMElementStyle(string name) : base(name)
        {
        }

        private MMElementStyle()
        {
        }

        public MMPositionProperty Position { get; set; }

        public override void Clone(MMWidgetDesign source)
        {
            base.Clone(source);
            this.CloneExtra(source as MMElementStyle);
        }

        private void CloneExtra(MMElementStyle source)
        {
            if (source != null)
            {
                this.Position = (MMPositionProperty)source.Position.Clone();
            }
        }

        public override object Clone()
        {
            var clone = new MMElementStyle();
            clone.Clone(this);
            return clone;
        }
    }
}