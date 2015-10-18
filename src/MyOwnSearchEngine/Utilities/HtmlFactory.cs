using System;
using System.Net;
using System.Text;

namespace MyOwnSearchEngine
{
    public class HtmlFactory
    {
        public static string Escape(string text)
        {
            return WebUtility.HtmlEncode(text);
        }

        public static string Tr(string s)
        {
            return "<tr>" + s + "</tr>";
        }

        public static string Td(string s, string attributes = null)
        {
            if (attributes == null)
            {
                return "<td>" + s + "</td>";
            }
            else
            {
                return "<td " + attributes + ">" + s + "</td>";
            }
        }

        public static string Th(string s, string attributes = null)
        {
            if (attributes == null)
            {
                return "<th>" + s + "</th>";
            }
            else
            {
                return "<th " + attributes + ">" + s + "</th>";
            }
        }

        public static string Img(string src)
        {
            return Tag(null, "img", Attribute("src", src));
        }

        public static string Div(string content)
        {
            return Tag(content, "div");
        }

        public static string Attribute(string name, object value)
        {
            return name + "=\"" + Escape(Convert.ToString(value)) + "\"";
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

            if (content == null)
            {
                sb.Append(" />");
            }
            else
            {
                sb.Append(">");
                sb.Append(content);
                sb.Append("</");
                sb.Append(tag);
                sb.Append(">");
            }

            return sb.ToString();
        }
    }
}
