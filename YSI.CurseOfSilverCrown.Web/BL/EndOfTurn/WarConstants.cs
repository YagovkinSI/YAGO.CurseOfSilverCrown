using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public static class WarConstants
    {
        public static int DefenceDefault = 35;

        public static double WariorDefenseTax = 1.1d;
        public static double WariorDefenseSupport = 2.0d;

        public static double AgressorLost = 0.20; //+ 0-10 рандом
        public static double TargetLost = 0.15; //+ 0-10 рандом
    }
}
