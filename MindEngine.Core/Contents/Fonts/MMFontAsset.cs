namespace MindEngine.Core.Contents.Fonts
{
    using Microsoft.Xna.Framework.Graphics;
    using MindEngine.Core.Contents.Assets;

    public class MMFontAsset : MMAsset
    {
        public MMFontAsset(string name, string path, int size, SpriteFont resource) 
            : this(name, path, size)
        {
            this.Resource = resource;
        }

        public MMFontAsset(string name, string path, int size) 
            : base(name, path)
        {
            this.Size = size;
        }

        public int Size { get; }

        public SpriteFont Resource { get; set; }
    }
}
