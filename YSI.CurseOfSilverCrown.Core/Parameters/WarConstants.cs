using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.Parameters
{
    public static class WarConstants
    {
        public static double WariorDefenseTax = 0.55d;
        public static double WariorDefenseSupport = 1.0d;

        public static double AgressorLost = 0.20; //+ 0-5 рандом
        public static double TargetLost = 0.10; //+ 0-5 рандом
    }
}
