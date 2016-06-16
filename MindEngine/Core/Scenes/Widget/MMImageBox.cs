namespace MindEngine.Core.Scenes.Widget
{
    using Contents.Texture;
    using Microsoft.Xna.Framework;

    public class MMImageBox : MMWidget
    {
        public MMImage Image { get; set; }

        public MMImageBox(MMImage image) : base("ImageBox")
        {
            this.Image = image;
        }

        protected override void DrawInternal(GameTime time)
        {
            this.EngineGraphicsRenderer.Draw(this.Image.Resource, this.Position, this.Size, this.DrawColor, this.DrawDepth);
        }
    }
}