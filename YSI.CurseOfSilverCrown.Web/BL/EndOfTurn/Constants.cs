using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public static class Constants
    {
        public static int StartCoffers = 4000;
        public static int StartWarriors = 100;

        public static int MaintenanceWarrioir = 20;
        public static int OutfitWarrioir = 50;
        public static int BaseCountWarriors = 100;
        public static TimeSpan CorruptionStartTime = new TimeSpan(5, 0, 0, 0);

        //Мнимальный доход в сезон - 10.000
        //Максимальный дозод в сезон - 20.000
        //Плавающая часть дозода - 10.000

        //Отправка воийск на заработки

        //30 воинов - получают базовые 10.000
        public static int MinTaxAuthorities = 30;
        public static int MinTax = 10000;

        //Ещё 470 могут максимум принести - 2.000 зм
        public static int GetAdditionalTax(int additionalWarriors, double random)
        {
            var tax = 4 * Math.Sqrt(additionalWarriors * 500);
            var randomTax = AddRandom10(tax, random);
            return randomTax;
        }

        internal static int GetCorruptionLevel(User user)
        {
            if (user == null)
                return 100;

            var daysOfCorruption = DateTime.UtcNow - user.LastActivityTime - Constants.CorruptionStartTime;
            if (daysOfCorruption < new TimeSpan(0))
                return 0;

            var level = (daysOfCorruption.TotalDays + 1) * (daysOfCorruption.TotalDays + 1);
            return level < 100
                ? (int)level
                : 100;
        }

        //Остальное (10000 - 2000 доп. налоговоики - 3500 на содержание = 4500) достигается инвестициями
        //Максимум при инвестициях в 50.000
        internal static int GetInvestmentTax(int investments)
        {
            var maxInvestment = 50000.0;
            var maxProfit = 4500.0 / 10.0;
            var koef = (maxProfit * maxProfit) / (double)maxInvestment;

            return (int)Math.Round(Math.Sqrt(investments * koef)) * 10;
        }

        public static int AddRandom10(double number, double random)
        {
            var doubleNumber = number * (0.9 + random / 5.0);
            var sweetNumber = (int)Math.Round(doubleNumber / 10.0) * 10;
            return sweetNumber;
        }

        //Траты на двор - 3-10 тысяч в сезон
        public static int MinIdleness = 3000;
        public static int MaxIdleness = 10000;


        public static int VassalTax = 1000;

    }
}
