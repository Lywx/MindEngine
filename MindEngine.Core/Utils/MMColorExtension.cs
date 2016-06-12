namespace MindEngine.Core.Utils
{
    using Microsoft.Xna.Framework;

    public static class MMColorExtension
    {
        public static Color MakeDark(this Color color, float value)
        {
            color.R = (byte)(color.R * value);
            color.G = (byte)(color.G * value);
            color.B = (byte)(color.B * value);

            return color;
        }

        public static Color MakeTransparent(this Color color, byte alpha)
        {
            color.A = (byte)(alpha * color.A / 255);
            color.R = (byte)(alpha * color.R / 255);
            color.G = (byte)(alpha * color.G / 255);
            color.B = (byte)(alpha * color.B / 255);

            return color;
        }
    }
}
