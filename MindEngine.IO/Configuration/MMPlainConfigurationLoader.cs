namespace MindEngine.IO.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using Directory;
    using Parser.Grammar;
    using Sprache;

    /// <example>
    /// A = 1 " A is something
    /// B = 1 " B is something
    /// ...
    /// </example>
    public static class MMPlainConfigurationLoader
    {
        #region Configuration Interface Loading

        public static List<KeyValuePair<string, string>> LoadDuplicable(string configurationFile)
        {
            return LoadListPairs(LoadAllLines(configurationFile));
        }

        public static Dictionary<string, string> LoadUnique(string configurationFile)
        {
            return LoadDictPairs(LoadAllLines(configurationFile));
        }

        #endregion

        #region Configuration Pair Loading

        private static string[] LoadAllLines(string configurationFile)
        {
            return File.ReadAllLines(MMDirectoryManager.ConfigurationPath(configurationFile));
        }

        private static List<KeyValuePair<string, string>> LoadListPairs(string[] lines)
        {
            var list = new List<KeyValuePair<string, string>>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var result = ConfigurationGrammar.LineParser.TryParse(line);
                if (result.WasSuccessful)
                {
                    var pair = result.Value;

                    // Filter out comments
                    if (pair.Key != null)
                    {
                        list.Add(pair);
                    }
                }
            }

            return list;
        }

        private static Dictionary<string, string> LoadDictPairs(string[] lines)
        {
            var list = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var result = ConfigurationGrammar.LineParser.TryParse(line);
                if (result.WasSuccessful)
                {
                    var pair = result.Value;

                    // Filter out comments
                    if (pair.Key != null)
                    {
                        list.Add(pair.Key, pair.Value);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}