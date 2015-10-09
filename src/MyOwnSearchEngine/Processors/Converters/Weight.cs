using System;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Weight : IProcessor
    {
        public string GetResult(Query query)
        {
            var suffix = query.TryGetStructure<Suffix>();
            if (suffix != null)
            {
                if (suffix.SuffixString == "kg")
                {
                    var number = Engine.TryGetStructure<Double>(suffix.Remainder);
                    if (number != null)
                    {
                        return KgToPounds(number.Value);
                    }
                }
            }

            var list = query.TryGetStructure<SeparatedList>();
            if (list != null &&
                list.SeparatorChar == ' ' &&
                list.Parts.Count == 2)
            {
                var firstNumber = Engine.TryGetStructure<Double>(list.Parts[0]);
                if (firstNumber != null)
                {
                    double value = firstNumber.Value;

                    var unit = Engine.TryGetStructure<Keyword>(list.Parts[1]).KeywordText;
                    if (unit == "kg" || unit == "kilograms")
                    {
                        return KgToPounds(value);
                    }

                    if (unit == "lb" || unit == "pounds")
                    {
                        return PoundsToKg(value);
                    }
                }
            }

            return null;
        }

        private string PoundsToKg(double value)
        {
            return Div($"{value} pounds = {value * 0.45359237} kg");
        }

        private string KgToPounds(double value)
        {
            return Div($"{value} kg = {value / 0.45359237} pounds");
        }
    }
}
