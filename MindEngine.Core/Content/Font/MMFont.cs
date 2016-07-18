namespace MindEngine.Core.Content.Font
{
    using Microsoft.Xna.Framework.Graphics;

    public class MMFont
    {
        public MMFont(MMFontAsset asset)
        {
            this.Name       = asset.Name;
            this.SpriteData = asset.Resource;
            this.MonoData   = new MMMonoFont(asset.Name, asset.Size, asset.Resource);
        }

        public string Name { get; }

        public SpriteFont SpriteData { get; private set; }

        public MMMonoFont MonoData { get; private set; }
    }
}