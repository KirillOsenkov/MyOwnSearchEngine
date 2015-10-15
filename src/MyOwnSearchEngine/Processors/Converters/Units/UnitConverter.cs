using System;
using System.Text;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class UnitConverter : IProcessor
    {
        public string GetResult(Query query)
        {
            var tuple = query.TryGetStructure<Tuple<Double, Unit>>();
            if (tuple != null)
            {
                return GetResult(tuple.Item1.Value, tuple.Item2);
            }

            var list = query.TryGetStructure<SeparatedList>();
            if (list != null &&
                list.SeparatorChar == ' ' &&
                (list.Parts.Count == 2 || list.Parts.Count == 4))
            {
                var firstNumber = Engine.TryGetStructure<Double>(list.Parts[0]);
                var unit = Engine.TryGetStructure<Unit>(list.Parts[1]);
                if (firstNumber != null && unit != null)
                {
                    if (list.Parts.Count == 2)
                    {
                        return GetResult(firstNumber.Value, unit);
                    }

                    var keyword = Engine.TryGetStructure<Keyword>(list.Parts[2]);
                    var tounit = Engine.TryGetStructure<Unit>(list.Parts[3]);
                    if (keyword != null &&
                        (keyword.KeywordText == "in" || keyword.KeywordText == "to") &&
                        tounit != null)
                    {
                        return GetResult(firstNumber.Value, unit, tounit);
                    }
                }
            }

            return null;
        }

        private string GetResult(double value, Unit unit, Unit toUnit)
        {
            var sb = new StringBuilder();

            foreach (var conversion in Units.Conversions)
            {
                if (conversion.From == unit && conversion.To == toUnit)
                {
                    var result = GetResult(value, conversion);
                    sb.Append(result);
                }
            }

            if (sb.Length == 0)
            {
                return null;
            }

            return sb.ToString();
        }

        private string GetResult(double value, Unit unit)
        {
            var sb = new StringBuilder();

            foreach (var conversion in Units.Conversions)
            {
                if (conversion.From == unit)
                {
                    var result = GetResult(value, conversion);
                    sb.Append(result);
                }
            }

            if (sb.Length == 0)
            {
                return null;
            }

            return sb.ToString();
        }

        private string GetResult(double value, Conversion conversion)
        {
            return Div($"{value} {conversion.From.ToString()} = {conversion.Converter(value)} {conversion.To.ToString()}");
        }
    }
}
