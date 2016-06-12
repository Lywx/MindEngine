namespace MindEngine.Core.Contents.Fonts.Extensions
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;

    public static class StringExtension
    {
        public static Vector2 MeasureMonospacedString(this string str, float scale)
        {
            // TODO(Minor): May not use standard font here.
            return NSimSunRegularFont.MeasureMonospacedString(str, scale);
        }

        #region Characters

        public static int CJKUniqueCharCount(this string str)
        {
            return str.CJKUniqueCharIndexes().Count;
        }

        public static List<int> CJKUniqueCharIndexes(this string str)
        {
            // TODO(Minor): May not use standard font here.
            return LucidaConsoleRegularFont.UnavailableCharIndexes(str);
        }

        public static List<float> CJKUniqueCharAmendedPosition(this string str, List<int> CJKUniqueCharIndexes)
        {
            var position = 0f;
            var indexes = new List<float>();

            for (var i = 0; i < str.Length; i++)
            {
                if (CJKUniqueCharIndexes.Contains(i))
                {
                    position += 0.5f;
                }

                if (i > 0
                    && CJKUniqueCharIndexes.Contains(i - 1))
                {
                    position += 0.5f;
                }

                indexes.Add(position);
                position += 1f;
            }

            return indexes;
        }

        public static string CJKAvailableString(this string str)
        {
            return NSimSunRegularFont.AvailableString(str);
        }

        public static bool IsAscii(this string value)
        {
            // ASCII encoding replaces non-ascii with question marks, so we use UTF8 to see if multi-byte sequences are there
            return Encoding.UTF8.GetByteCount(value) == value.Length;
        }

        #endregion

        #region Initialization

        public static void Initialize(IMMFontManager fonts)
        {
            NSimSunRegularFont = fonts["NSimSum Regular"];
            LucidaConsoleRegularFont = fonts["Lucida Console Regular"];
        }

        private static MMFont NSimSunRegularFont { get; set; }

        private static MMFont LucidaConsoleRegularFont { get; set; }

        #endregion
    }
}
