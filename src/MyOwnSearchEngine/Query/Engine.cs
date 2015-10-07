﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            structureParsers.Add(new SeparatedList(' '));
            structureParsers.Add(new SeparatedList(','));
            structureParsers.Add(new Integer());
        }

        public static Engine Instance { get; } = new Engine();

        public static string GetResponse(string input)
        {
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

            return default(T);
        }

        private object ParseWorker(string input)
        {
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
