namespace MindEngine.Core.Scene.Widget.Element.Style
{
    using Property;

    public class MMPrimitiveStyle : MMElementStyle
    {
        public MMPrimitiveStyle(string name) 
            : base(name)
        {
            
        }

        public MMWireframeProperty WireframeProperty { get; set; } 

        public override void Clone(MMWidgetDesign source)
        {
            base.Clone(source);
            this.CloneExtra(source as MMPrimitiveStyle);
        }

        private void CloneExtra(MMPrimitiveStyle source)
        {
            if (source != null)
            {
                this.WireframeProperty = source.WireframeProperty;
            }
        }
    }
}