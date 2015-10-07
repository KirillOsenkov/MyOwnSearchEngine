using System;
using System.Collections.Generic;
using System.Linq;

namespace MyOwnSearchEngine
{
    public class SeparatedList : IStructureParser
    {
        public ICollection<object> Parts { get; }
        public char SeparatorChar { get; }

        private char[] separatorCharArray;

        public SeparatedList(char separator)
        {
            SeparatorChar = separator;
            separatorCharArray = new[] { separator };
        }

        public SeparatedList(IEnumerable<object> parts, char separatorChar)
        {
            Parts = parts.ToArray();
            SeparatorChar = separatorChar;
        }

        public IReadOnlyList<T> GetStructuresOfType<T>() where T : IStructureParser
        {
            List<T> result = new List<T>();

            foreach (var part in Parts)
            {
                var instance = Engine.TryGetStructure<T>(part);
                if (instance != null)
                {
                    result.Add(instance);
                }
            }

            return result;
        }

        public object TryParse(string query)
        {
            if (query.IndexOf(separatorCharArray[0]) != -1)
            {
                var parts = query.Split(separatorCharArray, StringSplitOptions.RemoveEmptyEntries);
                return new SeparatedList(parts.Select(p => Engine.Parse(p)).ToArray(), SeparatorChar);
            }

            return null;
        }
    }
}
