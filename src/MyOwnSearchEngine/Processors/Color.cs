using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Color : IProcessor
    {
        public string GetResult(Query query)
        {
            if (query.Structure == null)
            {
                return null;
            }

            SeparatedList list = query.TryGetStructure<SeparatedList>();
            if (list != null)
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

            return null;
        }

        private string GetResult(int r, int g, int b)
        {
            var hexColor = GetHexColor(r, g, b);
            return 
                Div($"RGB({r},{g},{b}) = {hexColor}") + 
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
