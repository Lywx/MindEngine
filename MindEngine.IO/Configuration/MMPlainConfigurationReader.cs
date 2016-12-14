namespace MindEngine.IO.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Parser;

    public static class MMPlainConfigurationReader
    {
        #region Value Type Reader

        public static bool ReadBool(Dictionary<string, string> dict, string key, bool @default = false)
        {
            return (bool)ReadValueAt(dict, key, 0, typeof(bool), @default);
        }

        public static int ReadValueInt(Dictionary<string, string> dict, string key, int @default = 0)
        {
            return ReadIntAt(dict, key, 0, @default);
        }

        /// <summary>
        /// Get integer value from multiple int string separated by single white space.
        /// </summary>
        public static int ReadIntAt(Dictionary<string, string> dict, string key, int index, int @default = 0)
        {
            return (int)ReadValueAt(dict, key, index, typeof(int), @default);
        }

        public static float ReadFloat(Dictionary<string, string> dict, string key, float @default = 0f)
        {
            return ReadFloatAt(dict, key, 0, @default);
        }

        public static float ReadFloatAt(Dictionary<string, string> dict, string key, int index, float @default = 0f)
        {
            return (float)ReadValueAt(dict, key, index, typeof(float), @default);
        }

        #endregion

        #region Template Reader

        private static T ReadDataAt<T>(Dictionary<string, string> dict, string key, int index, Func<string, object, T> reader, T @default = default(T))
        {
            return MMValueReader.ReadData(dict.ValueStringAt(key, index), reader, @default);
        }

        private static object ReadValueAt(Dictionary<string, string> dict, string key, int index, Type type, object @default)
        {
            return MMValueReader.ReadValue(dict.ValueStringAt(key, index), type, @default);
        }

        #endregion

        #region Dictionary Utils

        private static string ValueStringAt(this Dictionary<string, string> dict, string key, int index)
        {
            return ValueStringAt(dict, key, index, ' ');
        }

        private static string ValueStringAt(this Dictionary<string, string> dict, string key, int index, char split)
        {
            // Add space at the end of string to streamline the edge case
            var valueString = dict[key] + split;

            var valueBegin = 0;
            var valueEnd = -1;
            var valueIndex = -1;

            var valueReady = false;

            var spacePrevious = false;
            var spaceCurrent = false;

            for (var i = 0; i < valueString.Length; ++i)
            {
                var charCurrent = valueString[i];
                if (charCurrent == split)
                {
                    spaceCurrent = true;

                    if (!spacePrevious)
                    {
                        valueEnd = i;
                    }
                }
                else
                {
                    spaceCurrent = false;

                    if (spacePrevious)
                    {
                        valueBegin = i;
                    }
                }

                var valueValid = valueBegin < valueEnd;
                if (valueValid && !valueReady)
                {
                    valueReady = true;

                    ++valueIndex;
                    if (valueIndex == index)
                    {
                        return valueString.Substring(valueBegin, valueEnd - valueBegin);
                    }
                }
                else if (!valueValid)
                {
                    valueReady = false;
                }

                spacePrevious = spaceCurrent;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        private static List<string> ValueStringListFrom(this Dictionary<string, string> dict, string key)
        {
            return ValueStringListFrom(dict, key, ',');
        }

        private static List<string> ValueStringListFrom(this Dictionary<string, string> dict, string key, char split)
        {
            return dict[key].Split(split).ToList();
        }

        #endregion
    }
}