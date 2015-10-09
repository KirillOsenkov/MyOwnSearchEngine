using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Temperature : IProcessor
    {
        public string GetResult(Query query)
        {
            var suffix = query.TryGetStructure<Suffix>();
            if (suffix != null)
            {
                var number = Engine.TryGetStructure<Double>(suffix.Remainder);

                if (suffix.SuffixString == "f" && number != null)
                {
                    return FahrenheitToCelsius(number.Value);
                }

                if (suffix.SuffixString == "c" && number != null)
                {
                    return CelsiusToFahrenheit(number.Value);
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
                    if (unit == "c" || unit == "celsius")
                    {
                        return CelsiusToFahrenheit(value);
                    }

                    if (unit == "f" || unit == "fahrenheit")
                    {
                        return FahrenheitToCelsius(value);
                    }
                }
            }

            return null;
        }

        private string CelsiusToFahrenheit(double value)
        {
            return Div($"{value} Celsius = {value * 9 / 5 + 32} Fahrenheit");
        }

        private string FahrenheitToCelsius(double value)
        {
            return Div($"{value} Fahrenheit = {(value - 32) * 5 / 9} Celsius");
        }
    }
}
