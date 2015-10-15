using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public enum ConversionCategory
    {
        Mass,
        Temperature
    }

    public class Unit
    {
        public string[] Names { get; set; }

        public Unit(params string[] names)
        {
            this.Names = names;
        }

        public override string ToString()
        {
            return Names[0];
        }
    }

    public class Conversion
    {
        public ConversionCategory Category { get; set; }
        public Unit From { get; set; }
        public Unit To { get; set; }
        public Func<double, double> Converter { get; set; }

        public Conversion(ConversionCategory category, Unit from, Unit to, Func<double, double> converter)
        {
            Category = category;
            From = from;
            To = to;
            Converter = converter;
        }
    }

    public class UnitParser : IStructureParser
    {
        public object TryParse(string query)
        {
            var result = Parse(query);
            if (result != null)
            {
                return result;
            }

            foreach (var keyword in keywords)
            {
                if (query.EndsWith(keyword.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var parsed = Engine.Parse(query.Substring(0, query.Length - keyword.Key.Length));
                    var number = Engine.TryGetStructure<Double>(parsed);
                    if (number != null)
                    {
                        return Tuple.Create(number, keyword.Value);
                    }
                }
            }

            return null;
        }

        public static object Parse(string unitName)
        {
            Unit unit = null;
            keywords.TryGetValue(unitName, out unit);
            return unit;
        }

        static UnitParser()
        {
            foreach (var unit in Units.AllUnits)
            {
                foreach (var name in unit.Names)
                {
                    keywords.Add(name, unit);
                }
            }
        }

        private static readonly Dictionary<string, Unit> keywords = new Dictionary<string, Unit>(StringComparer.OrdinalIgnoreCase);
    }

    public class Units
    {
        public static readonly Unit Pound = new Unit("lb", "lbs", "pound", "pounds");
        public static readonly Unit Kilogram = new Unit("kg", "kilo", "kilogram", "kilograms");
        public static readonly Unit Fahrenheit = new Unit("f", "fahrenheit", "fahrenheits");
        public static readonly Unit Celsius = new Unit("c", "celsius");

        private static Unit[] allUnits = null;
        public static Unit[] AllUnits
        {
            get
            {
                if (allUnits == null)
                {
                    allUnits = typeof(Units).GetFields().Select(f => f.GetValue(null) as Unit).ToArray();
                }

                return allUnits;
            }
        }
    }

    public class UnitConverter : IProcessor
    {
        private static readonly Conversion[] conversions =
        {
            new Conversion(ConversionCategory.Mass, Units.Pound, Units.Kilogram, p => p / 0.45359237),
            new Conversion(ConversionCategory.Mass, Units.Kilogram, Units.Pound, p => p * 0.45359237),
            new Conversion(ConversionCategory.Temperature, Units.Fahrenheit, Units.Celsius, p => (p - 32) * 5 / 9),
            new Conversion(ConversionCategory.Temperature, Units.Celsius, Units.Fahrenheit, p => p * 9 / 5 + 32),
        };

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

            foreach (var conversion in conversions)
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

            foreach (var conversion in conversions)
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
