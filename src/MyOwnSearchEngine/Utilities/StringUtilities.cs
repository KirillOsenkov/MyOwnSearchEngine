namespace MyOwnSearchEngine
{
    public static class StringUtilities
    {
        public static bool IsHexChar(this char c)
        {
            return
                (c >= '0' && c <= '9') ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }
    }
}
