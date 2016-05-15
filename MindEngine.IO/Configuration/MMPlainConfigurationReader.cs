namespace MindEngine.IO.Configuration
{
    using System;
    using System.Collections.Generic;
    using Parser;

    public static class MMPlainConfigurationReader
    {
        public static bool ReadValueBool(Dictionary<string, string> dict, string key, bool @default = false)
        {
            return (bool)ReadValueFrom(dict, key, 0, typeof(bool), @default);
        }

        public static int ReadValueInt(Dictionary<string, string> dict, string key, int @default = 0)
        {
            return ReadValueInts(dict, key, 0, @default);
        }

        /// <summary>
        /// Get integer value from multiple int string separated by single white space.
        /// </summary>
        public static int ReadValueInts(Dictionary<string, string> dict, string key, int index, int @default = 0)
        {
            return (int)ReadValueFrom(dict, key, index, typeof(int), @default);
        }

        public static float ReadValueFloat(Dictionary<string, string> dict, string key, float @default = 0f)
        {
            return ReadValueFloats(dict, key, 0, @default);
        }

        public static float ReadValueFloats(Dictionary<string, string> dict, string key, int index, float @default = 0f)
        {
            return (float)ReadValueFrom(dict, key, index, typeof(float), @default);
        }

        private static object ReadValueFrom(Dictionary<string, string> dict, string key, int index, Type type, object @default)
        {
            return MMValueReader.ReadValue(dict.ValueAt(key, index), type, @default);
        }

        private static string ValueAt(this Dictionary<string, string> dict, string key, int index)
        {
            return dict[key].Split(' ')[index];
        }
    }
}