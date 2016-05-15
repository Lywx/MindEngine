namespace MindEngine.Core.Contents.Fonts.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal static class SpriteFontExtension
    {
        #region Characters

        public static string AvailableString(this SpriteFont font, string str)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }

            return str.Where(t => font.Characters.Contains(t)).Aggregate(string.Empty, (current, t) => current + t);
        }

        public static List<int> UnavailableCharIndexes(this SpriteFont font, string str)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }

            var indexes = new List<int>();

            for (var i = 0; i < str.Length; i++)
            {
                if (!font.Characters.Contains(str[i]))
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        #endregion

        #region Measurement

        public static Vector2 MeasureString(
            this SpriteFont font,
            string text,
            float scale)
        {
            return font.MeasureString(text) * scale;
        }

        #endregion
    }
}