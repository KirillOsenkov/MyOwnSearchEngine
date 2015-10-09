using System.Net;
using System.Text;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class UrlDecode : IProcessor
    {
        public string GetResult(Query query)
        {
            var input = query.OriginalInput;
            var percent = input.IndexOf('%');
            while (percent != -1)
            {
                if (percent < input.Length - 2 && input[percent + 1].IsHexChar() && input[percent + 2].IsHexChar())
                {
                    return GetResult(input);
                }

                percent = input.IndexOf('%', percent + 1);
            }

            return null;
        }

        private string GetResult(string input)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Div("URL decoded: " + WebUtility.UrlDecode(input)));
            return sb.ToString();
        }
    }
}
