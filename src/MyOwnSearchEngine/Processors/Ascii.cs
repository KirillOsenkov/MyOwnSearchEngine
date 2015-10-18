using System;
using System.Text;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Ascii : IProcessor
    {
        public string GetResult(Query query)
        {
            if (query.OriginalInput.Equals("ascii", StringComparison.OrdinalIgnoreCase))
            {
                return AsciiTable();
            }

            return null;
        }

        private string AsciiTable()
        {
            var sb = new StringBuilder();
            sb.Append("<table style=\"font-size: 12pt\">");
            int columns = 8;
            int columnLength = 256 / columns;

            sb.Append("<tr>");
            var headers = Th("code", "style=\"color: lightseagreen\"") + Th("hex", "style=\"color: lightgray\"") + Th("char");
            for (int i = 0; i < columns; i++)
            {
                sb.Append(headers);
            }

            sb.AppendLine("</tr>");

            for (int i = 0; i < columnLength; i++)
            {
                sb.Append("<tr>");

                for (int column = 0; column < columns; column++)
                {
                    int character = i + column * columnLength;
                    var number = Td(character.ToString(), "style=\"color: lightseagreen\"");
                    var hex = Td(character.ToHex(), "style=\"color: lightgray\"");
                    var characterText = Td(Escape(((char)character).ToString()), "style=\"column-width: 60px\"");
                    //var escaped = Td(Escape(Escape(((char)character).ToString())));
                    //if (escaped == characterText)
                    //{
                    //    escaped = Td("");
                    //}

                    sb.Append(number + hex + characterText);
                }

                sb.AppendLine("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
