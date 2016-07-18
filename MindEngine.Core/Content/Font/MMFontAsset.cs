namespace MindEngine.Core.Content.Font
{
    using Asset;
    using Microsoft.Xna.Framework.Graphics;

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

        public override void Dispose()
        {
            this.Resource = null;
        }
    }
}
