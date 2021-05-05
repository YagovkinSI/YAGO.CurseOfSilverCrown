using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class FortificationsHelper
    {
        private const int MaxInvestment = 20000;
        private const int MaxProfit = 4;

        public static double GetCoeficient()
        {
            return ((double)MaxProfit * MaxProfit) / MaxInvestment;
        }

        public static int GetDefencePercent(int fortifications)
        {
            var coef = GetCoeficient();
            var result = Math.Sqrt(fortifications * coef);
            return (int)Math.Round(result * 100);
        }

        public static double GetWariorDefenseCoeficient(double commandCoefficient, int fortifications)
        {
            var fortBouns = GetDefencePercent(fortifications);
            return commandCoefficient * fortBouns / 100.0;
        }
    }
}
