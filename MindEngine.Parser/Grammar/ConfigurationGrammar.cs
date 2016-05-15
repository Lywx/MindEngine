namespace MindEngine.Parser.Grammar
{
    using System.Collections.Generic;
    using Sprache;

    public static class ConfigurationGrammar
    {
        public static Parser<KeyValuePair<string, string>> AssignmentParser =

            // allow multiple word on left side
            from lhs in Parse.AnyChar.Except(Parse.Chars('=', '\"', '\n')).AtLeastOnce()
            from lspaces in Parse.WhiteSpace.Optional()
            from eq in Parse.Char('=')
            from rspaces in Parse.WhiteSpace.Optional()

                // allow multiple word on right side
            from rhs in Parse.AnyChar.Except(Parse.Chars('=', '\"', '\n')).AtLeastOnce()
            select new KeyValuePair<string, string>(string.Concat(lhs).Trim(), string.Concat(rhs).Trim());

        public static Parser<string> CommentParser = Parse.Regex("\"+(.*)");

        public static Parser<KeyValuePair<string, string>> LineParser =
            from assignment in AssignmentParser.Optional()
            from comment in CommentParser.Optional()
            select assignment.IsDefined ? assignment.Get() : new KeyValuePair<string, string>();
    }
}
