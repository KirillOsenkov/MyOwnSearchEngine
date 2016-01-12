﻿using System.Globalization;
using System.Numerics;

namespace MyOwnSearchEngine
{
    public static class StringUtilities
    {
        public static bool IsHexOrDecimalChar(this char c)
        {
            return
                (c >= '0' && c <= '9') ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }

        public static bool IsHexChar(this char c)
        {
            return
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }

        public static bool ContainsHexChars(this string s)
        {
            if (s == null)
            {
                return false;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if (IsHexChar(s[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToHex(this char c)
        {
            return ToHex((int)c);
        }

        public static string ToHex(this int i)
        {
            return i.ToString("X");
        }

        public static string ToHex(this BigInteger i)
        {
            return i.ToString("X");
        }

        public static bool TryParseHex(this string s, out int result)
        {
            return int.TryParse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result);
        }

        public static bool TryParseHex(this string s, out BigInteger result)
        {
            return BigInteger.TryParse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result);
        }

        public static bool IsPrintable(this char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }

            return true;
        }
    }
}
