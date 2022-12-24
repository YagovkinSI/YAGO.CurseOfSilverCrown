using System;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.Parameters
{
    public static class Constants
    {
        public static int MaxPlayerCount = 10;

        public static double BaseVassalTax = 0.1;
        public static int MaxUnitCount = 10;

        public static int GetAdditionalTax(int additionalWarriors)
        {
            return additionalWarriors * 4;
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
