namespace MindEngine.Parser.Grammar
{
    using System.Linq;
    using Sprache;
    using Tokens;

    public static class CommonGrammar
    {
        public static Parser<string> BracketParser =
            (from lquot in Parse.Char('[')
             from content in Parse.CharExcept(']').Many().Text()
             from rquot in Parse.Char(']')
             select content).Token();

        public static Parser<string> WordParser =
            Parse.AnyChar.Except(Parse.Chars(' ', '[')).
                  AtLeastOnce().
                  Text().
                  Token();

        public static Parser<WordList> WordListParser =
            from words in WordParser.AtLeastOnce()
            select new WordList(words.ToList());
    }
}
