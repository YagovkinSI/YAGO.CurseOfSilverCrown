using System;
using System.Numerics;

namespace YAGO.World.Host.Controllers.Common
{
    public static class CommonExtensions
    {
        private static readonly Unit[] units =
        [
            new Unit(1L, ""),
            new Unit(1000L, "K"), // Тысячи
            new Unit(1000000L, "M"), // Миллионы
            new Unit(1000000000L, "B"), // Миллиарды
            new Unit(1000000000000L, "T"), // Триллионы
            new Unit(1000000000000000L, "Q"), // Квадриллионы
            new Unit(1000000000000000000L, "QT"), // Квинтиллионы
            // Примечание: для больших значений используйте decimal или пользовательские типы
        ];

        public static string ToBeautifulString(this double value, bool setPlus = false)
            => ToBeautifulString(value, setPlus, isInteger: false);

        public static string ToBeautifulString(this int value, bool setPlus = false)
            => ToBeautifulString(value, setPlus, isInteger: true);

        private static string ToBeautifulString(this double value, bool setPlus, bool isInteger)
        {
            var symbol = GetSymbolBeforeNumber(value, setPlus);

            if (value == 0)
                return symbol + "0";

            double absValue = Math.Abs(value);
            for (int i = units.Length - 1; i >= 0; i--)
            {
                if (absValue >= units[i].Value)
                {
                    double abbreviatedValue = absValue / units[i].Value;
                    var formatIfLess100 = isInteger ? "0.##" : "0.0#";
                    string formattedValue = abbreviatedValue switch
                    {
                        > 999.5 => "1000",
                        >= 100 => abbreviatedValue.ToString("G3"),
                        _ => abbreviatedValue.ToString(formatIfLess100)
                    };
                    return symbol + formattedValue + units[i].Symbol;
                }
            }

            string formatted = absValue.ToString("0.##");
            return symbol + double.Parse(formatted).ToString();
        }

        private static string GetSymbolBeforeNumber(double value, bool setPlus)
        {
            var isNegative = value < 0;
            var symbol = "";
            if (isNegative)
                symbol = "-";
            else if (setPlus)
                symbol = "+";
            return symbol;
        }

        private readonly struct Unit(long value, string symbol)
        {
            public long Value { get; } = value;
            public string Symbol { get; } = symbol;
        }
    }
}
