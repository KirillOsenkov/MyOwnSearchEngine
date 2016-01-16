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
