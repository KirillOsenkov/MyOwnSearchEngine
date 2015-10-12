using System.Globalization;

namespace MyOwnSearchEngine
{
    public class Integer : IStructureParser
    {
        public int Value { get; }

        public Integer()
        {
        }

        public Integer(int i)
        {
            Value = i;
        }

        public object TryParse(string query)
        {
            var trimmed = query.Trim();

            int result = 0;
            if (int.TryParse(trimmed, out result))
            {
                return new Integer(result);
            }

            if (int.TryParse(trimmed, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result) ||
                (trimmed.Length > 2 &&
                 trimmed.StartsWith("0x") && 
                 int.TryParse(trimmed.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result)))
            {
                return new Integer(result);
            }

            return null;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
