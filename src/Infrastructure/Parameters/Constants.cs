using System;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Parameters
{
    public static class Constants
    {
        public static double BaseVassalTax = 0.2;
        public static int MaxUnitCount = 10;
        public static int DisbandmentWarriorProfit = 5;

        public static int GetDisbandmentUnitProfit(int warriorCount)
        {
            return warriorCount * DisbandmentWarriorProfit;
        }

        public static int GetCorruptionLevel(User user)
        {
            if (user == null)
                return 100;

            var daysOfCorruption = DateTime.UtcNow - user.LastActivityTime - CorruptionParameters.CorruptionStartTime;
            if (daysOfCorruption < new TimeSpan(0))
                return 0;

            var level = ((int)daysOfCorruption.TotalDays + 1) * ((int)daysOfCorruption.TotalDays + 1);
            return level < 100
                ? level
                : 100;
        }
    }
}
