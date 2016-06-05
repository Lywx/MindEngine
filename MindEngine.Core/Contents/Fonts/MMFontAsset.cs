namespace MindEngine.Core.Contents.Fonts
{
    using Microsoft.Xna.Framework.Graphics;
    using MindEngine.Core.Contents.Assets;

    public class MMFontAsset : MMAsset
    {
        public MMFontAsset(string name, string asset, int size, SpriteFont resource) 
            : this(name, asset, size)
        {
            this.Resource = resource;
        }

        public MMFontAsset(string name, string asset, int size) 
            : base(name, asset)
        {
            this.Size = size;
        }

        public int Size { get; }

        public SpriteFont Resource { get; set; }
    }
}
