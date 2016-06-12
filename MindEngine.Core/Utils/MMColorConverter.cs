namespace MindEngine.Core.Utils
{
    using System;
    using Microsoft.Xna.Framework;

    public static class MMColorConverter
    {
        #region Hex Converter

        public static Color ParseHex(uint hex)
        {
            var a = (int) (hex               >> 24);
            var r = (int)((hex & 0x00ffffff) >> 16);
            var g = (int)((hex & 0x0000ffff) >> 8);
            var b = (int) (hex & 0x000000ff);

            return Color.FromNonPremultiplied(r, g, b, a);
        }

        public static uint ToHex(Color color)
        {
            return ((uint)color.A << 24) + ((uint)255 << color.R) + ((uint)255 << color.G) + color.B;
        }

        #endregion

        #region RGBA Format

        public static string ToRgbaString(Color value)
        {
            return $"{value.R}, {value.G}, {value.B}, {value.A}";
        }

        public static Color ParseRgba(string valueString)
        {
            var value = valueString.Split(',');

            byte r = 255, g = 255, b = 255, a = 255;

            if (value.Length >= 1)
            {
                r = byte.Parse(value[0]);
            }

            if (value.Length >= 2)
            {
                g = byte.Parse(value[1]);
            }

            if (value.Length >= 3)
            {
                b = byte.Parse(value[2]);
            }

            if (value.Length >= 4)
            {
                a = byte.Parse(value[3]);
            }

            return Color.FromNonPremultiplied(r, g, b, a);
        }

        #endregion

        #region ARBG Hex Format

        public static string ToArgbHexString(Color value)
        {
            var aHexString = value.A.ToString("X2");
            var rHexString = value.R.ToString("X2");
            var gHexString = value.G.ToString("X2");
            var bHexString = value.B.ToString("X2");

            return $"#{aHexString}{rHexString}{gHexString}{bHexString}";
        }

        public static Color ParseArgbHex(string valueString)
        {
            var hex = Convert.ToUInt32(valueString, 16);

            return ParseHex(hex);
        }

        #endregion

    }
}
