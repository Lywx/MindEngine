namespace MindEngine.Core.Scene.Widget.Component.Style
{
    public class MMComponentStyle : MMWidgetDesign
    {
        public MMComponentStyle()
        {
        }

        public override void Clone(MMWidgetDesign source)
        {
            base.Clone(source);
            this.CloneExtra(source as MMComponentStyle);
        }

        private void CloneExtra(MMComponentStyle mMComponentStyle)
        {

        }
    }
}