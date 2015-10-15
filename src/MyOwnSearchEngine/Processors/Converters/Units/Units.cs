using System.Linq;
using System.Reflection;

namespace MyOwnSearchEngine
{
    public class Units
    {
        public static readonly Unit Pound = new Unit("lb", "lbs", "pound", "pounds");
        public static readonly Unit Kilogram = new Unit("kg", "kilo", "kilogram", "kilograms");
        public static readonly Unit Fahrenheit = new Unit("f", "fahrenheit", "fahrenheits");
        public static readonly Unit Celsius = new Unit("c", "celsius");
        public static readonly Unit Meter = new Unit("m", "meter", "metre", "meters", "metres");
        public static readonly Unit Foot = new Unit("ft", "foot", "feet");
        public static readonly Unit Inch = new Unit("in", "inch");
        public static readonly Unit Mile = new Unit("mile", "miles");
        public static readonly Unit Ounce = new Unit("oz", "ounce", "ounces");

        public static readonly Conversion[] Conversions =
        {
            new Conversion(Units.Pound, Units.Kilogram, p => p * 0.45359237),
            new Conversion(Units.Kilogram, Units.Pound, p => p / 0.45359237),
            new Conversion(Units.Fahrenheit, Units.Celsius, p => (p - 32) * 5 / 9),
            new Conversion(Units.Celsius, Units.Fahrenheit, p => p * 9 / 5 + 32),
        };

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
}
