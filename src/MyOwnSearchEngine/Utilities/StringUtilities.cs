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
    }
}
