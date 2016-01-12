﻿using System.Numerics;

namespace MyOwnSearchEngine
{
    public enum IntegerKind
    {
        Decimal,
        Hexadecimal
    }

    public class Integer : IStructureParser
    {
        public BigInteger Value { get; }
        public IntegerKind Kind { get; private set; }

        public Integer()
        {
        }

        public Integer(BigInteger i)
        {
            Value = i;
        }

        public object TryParse(string query)
        {
            return TryParseInteger(query);
        }

        public static Integer TryParseInteger(string query)
        {
            var trimmed = query.Trim();

            BigInteger result = 0;
            if (BigInteger.TryParse(trimmed, out result))
            {
                return new Integer(result);
            }

            if (trimmed.TryParseHex(out result) ||
                (trimmed.Length > 2 &&
                 trimmed.StartsWith("0x") &&
                 trimmed.Substring(2).TryParseHex(out result)))
            {
                return new Integer(result) { Kind = IntegerKind.Hexadecimal };
            }

            return null;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
