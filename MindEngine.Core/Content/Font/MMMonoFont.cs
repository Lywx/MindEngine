namespace MindEngine.Core.Content.Font
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MMMonoFont
    {
        public MMMonoFont(string name, int size, SpriteFont sprite)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (sprite == null)
            {
                throw new ArgumentNullException(nameof(sprite));
            }

            this.Name = name;
            this.Size = size;
            this.Sprite = sprite;
        }

        public string Name { get; }

        public int Size { get; set; }

        public SpriteFont Sprite { get; }

        public float Aspect { get; set; } = 73f / 102f;

        /// <summary>
        /// This is a margin prefix to monospaced string to left-parallel 
        /// normally spaced string 
        /// </summary>
        public Vector2 Offset { get; set; } = new Vector2(7, 0);

        public Vector2 AsciiSize(float scale)
        {
            return new Vector2(this.Size * this.Aspect * scale, this.Size * scale);
        }
    }
}