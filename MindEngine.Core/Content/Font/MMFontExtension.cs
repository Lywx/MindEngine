namespace MindEngine.Core.Content.Font
{
    using System.Collections.Generic;
    using Extensions;
    using Microsoft.Xna.Framework;

    public static class MMFontExtension
    {
        #region Measure

        public static Vector2 MeasureString(this MMFont font, string text) => font.SpriteData.MeasureString(text);

        public static Vector2 MeasureString(
            this MMFont font,
            string text,
            float scale)
        {
            return font.MeasureString(text) * scale;
        }

        public static Vector2 MeasureMonospacedString(
            this MMFont font,
            string str,
            float scale)
        {
            var cjkCharCount = str.CJKUniqueCharCount();
            var asciiCharCount = str.Length - cjkCharCount;

            var monoFont = font.MonoData;
            var monoSize = monoFont.AsciiSize(scale);

            return new Vector2(
                (asciiCharCount + cjkCharCount * 2) * monoSize.X,
                monoSize.Y);
        }

        public static Vector2 MeasureString(
            this MMFont font,
            string str,
            float scale,
            bool monospaced)
        {
            if (monospaced)
            {
                return font.MeasureMonospacedString(str, scale);
            }

            return font.MeasureString(str, scale);
        }

        #endregion

        #region Available

        public static bool Available(this MMFont font, char c) => font.SpriteData.Characters.Contains(c);

        public static string AvailableString(this MMFont font, string str) => font.SpriteData.AvailableString(str);

        public static List<int> UnavailableCharIndexes(this MMFont font, string str) => font.SpriteData.UnavailableCharIndexes(str);

        #endregion
    }
}