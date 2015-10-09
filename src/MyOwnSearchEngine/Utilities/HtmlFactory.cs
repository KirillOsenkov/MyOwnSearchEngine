using System;
using System.Net;
using System.Text;

namespace MyOwnSearchEngine
{
    public class HtmlFactory
    {
        public static string Div(string content)
        {
            return Tag(content, "div");
        }

        public static string Attribute(string name, object value)
        {
            return name + "=\"" + WebUtility.HtmlEncode(Convert.ToString(value)) + "\"";
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
            sb.Append(WebUtility.HtmlEncode(content));
            sb.Append("</");
            sb.Append(tag);
            sb.Append(">");
            return sb.ToString();
        }
    }
}
