using System;

namespace YAGO.World.Host.Controllers.Common
{
    public static class CommonExtensions
    {
        public static string ToBeautifulString(this double value, bool setPlus = false)
        {
            var isNegative = value < 0;
            var symbol = "";
            if (isNegative)
                symbol = "-";
            else if (setPlus)
                symbol = "+";

            if (value == 0)
                return symbol + "0";

            double absValue = Math.Abs(value);

            if (absValue < 1)
            {
                string formatted = absValue.ToString("F3");
                return symbol + double.Parse(formatted).ToString();
            }

            if (absValue < 1000)
            {
                return symbol + Math.Floor(absValue).ToString().Replace("\\B(?=(\\d{3})+(?!\\d))", " ");
            }

            var units = new[]
            {
                new { Value = 1L, Symbol = "" },
                new { Value = 1000L, Symbol = "K" },      // Тысячи
                new { Value = 1000000L, Symbol = "M" },   // Миллионы
                new { Value = 1000000000L, Symbol = "B" }, // Миллиарды
                new { Value = 1000000000000L, Symbol = "T" },     // Триллионы
                new { Value = 1000000000000000L, Symbol = "Q" },  // Квадриллионы
                new { Value = 1000000000000000000L, Symbol = "QT" } // Квинтиллионы
                // Примечание: для больших значений используйте decimal или пользовательские типы
            };

            int unitIndex = 0;
            for (int i = units.Length - 1; i >= 0; i--)
            {
                if (absValue >= units[i].Value)
                {
                    unitIndex = i;
                    break;
                }
            }

            double abbreviatedValue = absValue / units[unitIndex].Value;
            string formattedValue = abbreviatedValue.ToString("0.###");

            return symbol + formattedValue + units[unitIndex].Symbol;
        }
    }
}
