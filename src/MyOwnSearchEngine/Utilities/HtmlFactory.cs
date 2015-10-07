using System;
using System.Text;

namespace MyOwnSearchEngine
{
    public class HtmlFactory
    {
        public static string Div(string content)
        {
            return Tag(content, "div");
        }

        public static string Canvas(int width, int height)
        {
            return Tag("", "canvas", $"width=\"{width}\"", $"height=\"{height}\"");
        }

        public static string Attribute(string name, object value)
        {
            return name + "=\"" + Convert.ToString(value) + "\"";
        }

        public static string Tag(string content, string tag, params string[] attributes)
        {
            var sb = new StringBuilder();
            sb.Append("<");
            sb.Append(tag);
            if (attributes != null && attributes.Length > 0)
            {
                foreach (var attribute in attributes)
                {
                    sb.Append(" ");
                    sb.Append(attribute);
                }
            }
            sb.Append(">");
            sb.Append(content);
            sb.Append("</");
            sb.Append(tag);
            sb.Append(">");
            return sb.ToString();
        }
    }
}
