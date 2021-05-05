using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Commands;

namespace YSI.CurseOfSilverCrown.Core.Parameters
{
    public static class Constants
    {
        //Мнимальный доход в сезон - 10.000
        //Максимальный дозод в сезон - 20.000
        //Плавающая часть дозода - 10.000

        //Отправка воийск на заработки

        //сборщики налогов - получают базовые 10.000
        public static int MinTax = 10000;

        public static double BaseVassalTax = 0.1;

        //Ещё 470 могут максимум принести - 2.000 зм
        public static int GetAdditionalTax(int additionalWarriors, double random)
        {
            var tax = (int) (4 * Math.Sqrt(additionalWarriors * 500));
            var randomTax = RandomHelper.AddRandom(tax, randomNumber: random, roundRequest: -1);
            return randomTax;
        }

        public static int GetCorruptionLevel(User user)
        {
            if (user == null)
                return 100;

            var daysOfCorruption = DateTime.UtcNow - user.LastActivityTime - CorruptionParameters.CorruptionStartTime;
            if (daysOfCorruption < new TimeSpan(0))
                return 0;

            var level = (daysOfCorruption.TotalDays + 1) * (daysOfCorruption.TotalDays + 1);
            return level < 100
                ? (int)level
                : 100;
        }
        //Остальное (10000 - 2000 доп. налоговоики - 3500 на содержание = 4500) достигается инвестициями
        //Максимум при инвестициях в 50.000

        //Траты на двор - 3-10 тысяч в сезон
        public static int MinIdleness = 2500;
        public static int MaxIdleness = 10000;
    }
}
