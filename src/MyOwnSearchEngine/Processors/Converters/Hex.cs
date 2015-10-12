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

            return null;
        }

        private string GetResult(int value)
        {
            return Div($"{value} = 0x{value.ToString("X")}");
        }
    }
}
