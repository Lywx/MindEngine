namespace MindEngine.Core.Util
{
    using Microsoft.Xna.Framework.Graphics;

    public static class MMTexture2DExtension
    {
        public static float Aspect(this Texture2D texture)
        {
            return (float)texture.Width / (float)texture.Height;
        }
    }
}