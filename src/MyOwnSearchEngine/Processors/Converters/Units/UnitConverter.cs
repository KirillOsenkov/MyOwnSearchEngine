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
            if (list != null && list.SeparatorChar == ' ')
            {
                if (list.Parts.Count == 2 || list.Parts.Count == 4)
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

                if (list.Parts.Count == 3)
                {
                    var firstTuple = Engine.TryGetStructure<Tuple<Double, Unit>>(list.Parts[0]);
                    var keyword = Engine.TryGetStructure<Keyword>(list.Parts[1]);
                    var toUnit = Engine.TryGetStructure<Unit>(list.Parts[2]);
                    if (firstTuple != null &&
                        keyword != null &&
                        (keyword.KeywordText == "in" || keyword.KeywordText == "to") &&
                        toUnit != null)
                    {
                        return GetResult(firstTuple.Item1.Value, firstTuple.Item2, toUnit);
                    }
                }

                if (list.Parts.Count == 2)
                {
                    var firstTuple = Engine.TryGetStructure<Tuple<Double, Unit>>(list.Parts[0]);
                    var secondTuple = Engine.TryGetStructure<Tuple<Double, Unit>>(list.Parts[1]);
                    if (firstTuple != null &&
                        secondTuple != null &&
                        firstTuple.Item2 == Units.Foot &&
                        secondTuple.Item2 == Units.Inch)
                    {
                        return GetResult(firstTuple.Item1.Value * 12 + secondTuple.Item1.Value, Units.Inch);
                    }

                    var secondNumber = Engine.TryGetStructure<Double>(list.Parts[1]);
                    if (firstTuple != null &&
                        firstTuple.Item2 == Units.Foot &&
                        secondNumber != null)
                    {
                        return GetResult(firstTuple.Item1.Value * 12 + secondNumber.Value, Units.Inch);
                    }
                }

                if (list.Parts.Count == 3 || list.Parts.Count == 4)
                {
                    var firstNumber = Engine.TryGetStructure<Double>(list.Parts[0]);
                    var unit = Engine.TryGetStructure<Unit>(list.Parts[1]);
                    var secondNumber = Engine.TryGetStructure<Double>(list.Parts[2]);

                    if (list.Parts.Count == 3 &&
                        firstNumber != null &&
                        unit == Units.Foot &&
                        secondNumber != null)
                    {
                        return GetResult(firstNumber.Value * 12 + secondNumber.Value, Units.Inch);
                    }

                    if (list.Parts.Count == 4 &&
                        firstNumber != null &&
                        unit == Units.Foot &&
                        secondNumber != null &&
                        Engine.TryGetStructure<Unit>(list.Parts[3]) == Units.Inch)
                    {
                        return GetResult(firstNumber.Value * 12 + secondNumber.Value, Units.Inch);
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
                Conversion first = null;
                Conversion second = null;

                // no direct conversion, try 2-step chain
                foreach (var conversion in Units.Conversions)
                {
                    if (conversion.From == unit)
                    {
                        first = conversion;
                    }

                    if (conversion.To == toUnit)
                    {
                        second = conversion;
                    }
                }

                if (first != null && second != null && first.To == second.From)
                {
                    var composite = new Conversion(first.From, second.To, v => second.Converter(first.Converter(v)));
                    sb.Append(GetResult(value, composite));
                }
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
