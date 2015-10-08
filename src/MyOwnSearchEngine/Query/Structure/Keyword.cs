using System;

namespace MyOwnSearchEngine
{
    public class Keyword : IStructureParser
    {
        public string KeywordText { get; }

        public Keyword(string keyword)
        {
            KeywordText = keyword;
        }

        public object TryParse(string query)
        {
            if (string.Equals(query, KeywordText, StringComparison.OrdinalIgnoreCase))
            {
                return this;
            }

            return null;
        }

        public override string ToString()
        {
            return KeywordText.ToLowerInvariant();
        }
    }
}
