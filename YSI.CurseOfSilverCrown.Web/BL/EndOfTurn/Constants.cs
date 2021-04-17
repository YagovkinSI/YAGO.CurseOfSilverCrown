using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public static class Constants
    {
        public static int MaintenanceWarrioir = 20;
        public static int OutfitWarrioir = 50;
        public static int BaseCountWarriors = 100;

        //Мнимальный доход в сезон - 10.000
        //Максимальный дозод в сезон - 20.000
        //Плавающая часть дозода - 10.000

        //Отправка воийск на заработки

        //30 воинов - получают базовые 10.000
        public static int MinTaxAuthorities = 20;
        public static int MinTax = 10000;

        //Ещё 470 могут максимум принести - 1.000 зм
        public static int GetAdditionalTax(int additionalWarriors, double random)
        {
            var tax = 4 * Math.Sqrt(additionalWarriors * 500);
            var randomTax = AddRandom10(tax, random);
            return randomTax;
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
