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
            int result = 0;
            if (int.TryParse(query, out result))
            {
                return new Integer(result);
            }

            return null;
        }
    }
}
