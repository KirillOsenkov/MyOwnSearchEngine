using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Hex : IProcessor
    {
        public string GetResult(Query query)
        {
            var integer = query.TryGetStructure<Integer>();
            if (integer != null)
            {
                return GetResult(integer.Value);
            }

            var separatedList = query.TryGetStructure<SeparatedList>();
            if (separatedList != null && separatedList.Count >= 2)
            {
                var number = separatedList.TryGetStructure<Integer>(0);
                if (number != null)
                {
                    var keyword1 = separatedList.TryGetStructure<Keyword>(1);
                    if (keyword1 != null)
                    {
                        if (keyword1 == "hex" && separatedList.Count == 2)
                        {
                            return GetResult(number.Value);
                        }

                        if (separatedList.Count == 3 &&
                            (keyword1 == "in" || keyword1 == "to") &&
                            separatedList.TryGetStructure<Keyword>(2) == "hex")
                        {
                            return GetResult(number.Value);
                        }
                    }
                }
            }

            return null;
        }

        private string GetResult(int value)
        {
            return Div(Escape($"{value} = 0x{value.ToString("X")}"));
        }
    }
}
