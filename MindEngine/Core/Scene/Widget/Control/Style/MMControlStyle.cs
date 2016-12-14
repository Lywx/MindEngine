namespace MindEngine.Core.Scene.Widget.Control.Style
{
    using Property;

    public class MMControlStyle : MMWidgetDesign
    {
        public MMControlStyle(string name) : base(name)
        {
        }

        private MMControlStyle()
        {
        }

        /// <summary>
        /// Control layout define the most basic spatial design in a control. 
        /// This property is used across all the widgets.
        /// </summary>
        public MMLayoutProperty LayoutProperty { get; set; }

        public override void Clone(MMWidgetDesign source)
        {
            base.Clone(source);
            this.CloneExtra(source as MMControlStyle);
        }

        private void CloneExtra(MMControlStyle source)
        {
            if (source != null)
            {
                this.LayoutProperty = (MMLayoutProperty)source.LayoutProperty.Clone();
            }
        }

        public override object Clone()
        {
            var clone = new MMControlStyle();
            clone.Clone(this);

            return clone;
        }
    }
}