using System.Collections.Generic;
using System.Text;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Engine
    {
        private List<IProcessor> processors = new List<IProcessor>();
        private List<IStructureParser> structureParsers = new List<IStructureParser>();

        private Engine()
        {
            processors.Add(new Color());
            processors.Add(new UrlDecode());
            processors.Add(new Weight());
            processors.Add(new Temperature());

            structureParsers.Add(new Keyword("rgb"));
            structureParsers.Add(new Keyword("in"));
            structureParsers.Add(new Keyword("f"));
            structureParsers.Add(new Keyword("c"));
            structureParsers.Add(new Keyword("kg"));
            structureParsers.Add(new Keyword("kilograms"));
            structureParsers.Add(new Keyword("pounds"));
            structureParsers.Add(new Invocation());
            structureParsers.Add(new Prefix("#"));
            structureParsers.Add(new Suffix("kg"));
            structureParsers.Add(new Suffix("lb"));
            structureParsers.Add(new Suffix("f"));
            structureParsers.Add(new Suffix("c"));
            structureParsers.Add(new Integer());
            structureParsers.Add(new Double());
            structureParsers.Add(new SeparatedList(','));
            structureParsers.Add(new SeparatedList(' '));
        }

        public static Engine Instance { get; } = new Engine();

        public static string GetResponse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Div("");
            }

            var result = Instance.GetResponseWorker(input);
            if (string.IsNullOrEmpty(result))
            {
                result = Div("No results found.");
            }

            return result;
        }

        public static object Parse(string input)
        {
            return Instance.ParseWorker(input);
        }

        public static T TryGetStructure<T>(object instance) where T : IStructureParser
        {
            if (instance == null)
            {
                return default(T);
            }

            if (instance is T)
            {
                return (T)instance;
            }

            IEnumerable<object> list = instance as IEnumerable<object>;
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item is T)
                    {
                        return (T)item;
                    }
                }
            }

            if (instance is Integer && typeof(T) == typeof(Double))
            {
                return (T)(object)new Double(((Integer)instance).Value);
            }

            return default(T);
        }

        private object ParseWorker(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            input = input.Trim();

            var list = new List<object>();

            foreach (var parser in structureParsers)
            {
                var result = parser.TryParse(input);
                if (result != null)
                {
                    list.Add(result);
                }
            }

            if (list.Count == 0)
            {
                return null;
            }
            else if (list.Count == 1)
            {
                return list[0];
            }
            else
            {
                return list;
            }
        }

        private string GetResponseWorker(string input)
        {
            var query = new Query(input);
            var sb = new StringBuilder();
            foreach (var processor in processors)
            {
                var result = processor.GetResult(query);
                if (!string.IsNullOrEmpty(result))
                {
                    sb.AppendLine(result);
                }
            }

            return sb.ToString();
        }
    }
}
