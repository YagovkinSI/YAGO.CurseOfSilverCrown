using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAGO.World.Host.Helpers
{
    public static class ViewHelper
    {
        public static string GetSweetNumber(double number)
        {
            var level = 0;
            while (number >= 10000)
            {
                level++;
                number /= 1000;
            }

            switch (level)
            {
                case 0:
                    return $"{Math.Round(number, 2)}";
                case 1:
                    return $"{Math.Round(number, 2)}K";
                case 2:
                    return $"{Math.Round(number, 2)}M";
                case 3:
                    return $"{Math.Round(number, 2)}B";
                case 4:
                    return $"{Math.Round(number, 2)}T";
                default:
                    return "Тьма";
            }
        }
    }
}
