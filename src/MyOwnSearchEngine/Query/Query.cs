using System.Collections.Generic;

namespace MyOwnSearchEngine
{
    public class Query
    {
        public string OriginalInput { get; }
        public object Structure { get; }

        public Query(string input)
        {
            OriginalInput = input;
            Structure = Engine.Parse(input);
        }

        public T TryGetStructure<T>() where T : IStructureParser
        {
            return Engine.TryGetStructure<T>(Structure);
        }
    }
}
