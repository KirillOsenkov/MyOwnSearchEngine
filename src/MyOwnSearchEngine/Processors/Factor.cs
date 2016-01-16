using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyOwnSearchEngine
{
    public class Factor : IProcessor
    {
        public string GetResult(Query query)
        {
            var integer = query.TryGetStructure<Integer>();
            if (integer != null)
            {
                return GetResult(integer.Value);
            }

            var separatedList = query.TryGetStructure<SeparatedList>();
            if (separatedList != null && separatedList.Count == 2)
            {
                var number = separatedList.TryGetStructure<Integer>(0);
                if (number != null)
                {
                    var keyword1 = separatedList.TryGetStructure<Keyword>(1);
                    if (keyword1 == "factor")
                    {
                        return GetResult(number.Value);
                    }
                }
                else
                {
                    number = separatedList.TryGetStructure<Integer>(1);
                    var keyword1 = separatedList.TryGetStructure<Keyword>(0);
                    if (number != null && keyword1 == "factor")
                    {
                        return GetResult(number.Value);
                    }
                }
            }

            return null;
        }

        private string GetResult(BigInteger number)
        {
            number = BigInteger.Abs(number);

            var factors = new List<BigInteger>();
            var bound = number / 2;
            for (long i = 2; i <= bound; i++)
            {
                while (number % i == 0)
                {
                    number /= i;
                    factors.Add(i);
                }

                if (number == 1)
                {
                    break;
                }
            }

            if (number != 1)
            {
                return "Prime number.";
            }

            var sb = new StringBuilder();
            sb.Append("Factors: ");

            sb.Append(string.Join(" ", factors));

            return sb.ToString();
        }
    }
}
