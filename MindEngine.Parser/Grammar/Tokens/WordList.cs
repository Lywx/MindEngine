namespace MindEngine.Parser.Grammar.Tokens
{
    using System.Collections.Generic;

    public class WordList
    {
        public WordList(List<string> words)
        {
            this.Words = words;
        }

        public List<string> Words { get; }

        public override string ToString()
        {
            return string.Join(" ", this.Words);
        }

        public override int GetHashCode()
        {
            return this.Words?.GetHashCode() ?? 0;
        }

        protected bool Equals(WordList other)
        {
            return Equals(this.Words, other.Words);
        }
    }
}
