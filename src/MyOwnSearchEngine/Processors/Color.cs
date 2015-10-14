using System;
using System.Globalization;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Color : IProcessor
    {
        public string GetResult(Query query)
        {
            if (string.Equals(query.OriginalInput, "color", StringComparison.OrdinalIgnoreCase))
            {
                return Div(Img(@"http://kirillosenkov.github.io/images/ColorChartSorted.png"));
            }

            if (query.Structure == null)
            {
                return null;
            }

            SeparatedList list = query.TryGetStructure<SeparatedList>();

            if (list == null)
            {
                var invocation = query.TryGetStructure<Invocation>();
                if (invocation != null &&
                    string.Equals(invocation.Prefix, "rgb", StringComparison.OrdinalIgnoreCase))
                {
                    list = Engine.TryGetStructure<SeparatedList>(invocation.ArgumentListParsed);
                    if (list.Parts.Count != 3)
                    {
                        list = null;
                    }
                }
            }

            if (list != null &&
                (list.Parts.Count == 3 ||
                (list.Parts.Count == 4 &&
                    ((list.Parts[0] is Keyword && list.Parts[0].ToString() == "rgb") ||
                     (list.Parts[3] is Keyword && list.Parts[3].ToString() == "rgb")))))
            {
                var intList = list.GetStructuresOfType<Integer>();
                if (intList.Count == 3)
                {
                    int r = intList[0].Value;
                    int g = intList[1].Value;
                    int b = intList[2].Value;
                    if (r >= 0 && r < 256 && g >= 0 && g < 256 && b >= 0 && b < 256)
                    {
                        return GetResult(r, g, b);
                    }
                }
            }

            var hashPrefix = query.TryGetStructure<Prefix>();
            if (hashPrefix != null &&
                hashPrefix.PrefixString == "#")
            {
                var remainderString = hashPrefix.RemainderString;
                if (remainderString.Length == 3 || remainderString.Length == 6)
                {
                    var integer = Engine.TryGetStructure<Integer>(hashPrefix.Remainder);
                    if (integer != null)
                    {
                        int r;
                        int g;
                        int b;

                        if (remainderString.Length == 3)
                        {
                            r = int.Parse(new string(remainderString[0], 2), NumberStyles.AllowHexSpecifier);
                            g = int.Parse(new string(remainderString[1], 2), NumberStyles.AllowHexSpecifier);
                            b = int.Parse(new string(remainderString[2], 2), NumberStyles.AllowHexSpecifier);
                        }
                        else
                        {
                            r = int.Parse(remainderString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                            g = int.Parse(remainderString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                            b = int.Parse(remainderString.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                        }

                        return GetResult(r, g, b);
                    }
                }
            }

            return null;
        }

        private string GetResult(int r, int g, int b)
        {
            var hexColor = GetHexColor(r, g, b);
            return
                Div(Escape($"RGB({r},{g},{b}) = {hexColor}")) +
                Tag("", "canvas",
                    Attribute("width", 500),
                    Attribute("height", 200),
                    Attribute("style", "background:" + hexColor + ";margin-top:20px"));
        }

        private string GetHexColor(int r, int g, int b)
        {
            string rhex = ToHex(r);
            string ghex = ToHex(g);
            string bhex = ToHex(b);
            if (rhex[0] == rhex[1] && ghex[0] == ghex[1] && bhex[0] == bhex[1])
            {
                rhex = rhex.Substring(1);
                ghex = ghex.Substring(1);
                bhex = bhex.Substring(1);
            }

            return "#" + rhex + ghex + bhex;
        }

        private string ToHex(int c)
        {
            return string.Format("{0:X2}", c);
        }
    }
}
