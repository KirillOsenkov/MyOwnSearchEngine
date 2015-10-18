﻿using System.Linq;
using System.Reflection;

namespace MyOwnSearchEngine
{
    public class Units
    {
        public static readonly Unit Pound = new Unit("lb", "lbs", "pound", "pounds");
        public static readonly Unit Ounce = new Unit("oz", "ounce", "ounces");
        public static readonly Unit Gram = new Unit("gr", "gram", "grams");
        public static readonly Unit Kilogram = new Unit("kg", "kilo", "kilogram", "kilograms");
        public static readonly Unit Ton = new Unit("t", "ton", "tons");

        public static readonly Unit Kilometer = new Unit("km", "kilometer", "kilometers", "kilometre", "kilometres");
        public static readonly Unit Centimeter = new Unit("cm", "centimeter", "centimetre", "centimeters", "centimetres");
        public static readonly Unit Millimeter = new Unit("mm", "millimeter", "millimetre", "millimeters", "millimetres");
        public static readonly Unit Inch = new Unit("in", "inch", "inches");
        public static readonly Unit Foot = new Unit("ft", "foot", "feet");
        public static readonly Unit Mile = new Unit("miles", "mile");
        public static readonly Unit Yard = new Unit("yd", "yard", "yards");
        public static readonly Unit Meter = new Unit("m", "meter", "metre", "meters", "metres");

        public static readonly Unit Mph = new Unit("mph", "mile/h", "miles/h", "mile/hour", "miles/hour");
        public static readonly Unit Kmh = new Unit("kmh", "km/h", "km/hour", "kilometer/hour", "kilometers/hour");
        public static readonly Unit FtSecond = new Unit("f/s", "fps", "ft/s", "foot/s", "foot/second", "feet/s", "feet/second");
        public static readonly Unit Mps = new Unit("m/s", "mps", "meters/second");
        public static readonly Unit Knot = new Unit("knot", "knots");

        public static readonly Unit Fahrenheit = new Unit("f", "fahrenheit", "fahrenheits");
        public static readonly Unit Celsius = new Unit("c", "celsius");
        public static readonly Unit Kelvin = new Unit("k", "kelvin", "kelvins");

        public static readonly Unit Gallon = new Unit("gallon", "gallons");
        public static readonly Unit Liter = new Unit("l", "liter", "liters", "litre", "litres");

        public static readonly Unit Mpg = new Unit("mpg", "miles/gallon");
        public static readonly Unit LitersPer100Km = new Unit("liters/100km", "l/100km");

        public static readonly Conversion[] Conversions =
        {
            new Conversion(Pound, Kilogram, p => p * 0.45359237),
            new Conversion(Kilogram, Pound, p => p / 0.45359237),
            new Conversion(Ounce, Kilogram, p => p * 0.028349523125),
            new Conversion(Kilogram, Ounce, p => p / 0.028349523125),
            new Conversion(Gram, Kilogram, p => p / 1000),
            new Conversion(Kilogram, Gram, p => p * 1000),
            new Conversion(Ton, Kilogram, p => p * 1000),
            new Conversion(Kilogram, Ton, p => p / 1000),

            new Conversion(Mile, Meter, p => p * 1609.344),
            new Conversion(Meter, Mile, p => p / 1609.344),
            new Conversion(Foot, Meter, p => p * 0.3048),
            new Conversion(Meter, Foot, p => p / 0.3048),
            new Conversion(Inch, Meter, p => p * 0.0254),
            new Conversion(Meter, Inch, p => p / 0.0254),
            new Conversion(Yard, Meter, p => p * 0.9144),
            new Conversion(Meter, Yard, p => p / 0.9144),
            new Conversion(Kilometer, Meter, p => p * 1000),
            new Conversion(Meter, Kilometer, p => p / 1000),
            new Conversion(Centimeter, Meter, p => p / 100),
            new Conversion(Meter, Centimeter, p => p * 100),
            new Conversion(Millimeter, Meter, p => p / 1000),
            new Conversion(Meter, Millimeter, p => p * 1000),

            new Conversion(Mph, Kmh, p => p * 1.609344),
            new Conversion(Kmh, Mph, p => p / 1.609344),
            new Conversion(FtSecond, Kmh, p => p * 1.09728),
            new Conversion(Kmh, FtSecond, p => p / 1.09728),
            new Conversion(Mps, Kmh, p => p * 3.6),
            new Conversion(Kmh, Mps, p => p / 3.6),
            new Conversion(Knot, Kmh, p => p * 1.852),
            new Conversion(Kmh, Knot, p => p / 1.852),

            new Conversion(Fahrenheit, Celsius, p => (p - 32) * 5 / 9),
            new Conversion(Celsius, Fahrenheit, p => p * 9 / 5 + 32),
            new Conversion(Kelvin, Celsius, p => p - 273.15),
            new Conversion(Celsius, Kelvin, p => p + 273.15),

            new Conversion(Mpg, LitersPer100Km, p => 235.214583084785 / p),
            new Conversion(LitersPer100Km, Mpg, p => 235.214583084785 / p),
        };

        private static Unit[] allUnits = null;
        public static Unit[] AllUnits
        {
            get
            {
                if (allUnits == null)
                {
                    allUnits = typeof(Units)
                        .GetFields()
                        .Select(f => f.GetValue(null) as Unit)
                        .Where(v => v != null)
                        .ToArray();
                }

                return allUnits;
            }
        }
    }
}
