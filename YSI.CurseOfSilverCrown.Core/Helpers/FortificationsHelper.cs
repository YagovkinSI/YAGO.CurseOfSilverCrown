using System;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class FortificationsHelper
    {
        private const int GarrisonPlaceCost = 25;

        public static int GetMaxGarisson(int fortifications)
        {
            return fortifications / GarrisonPlaceCost;
        }

        public static int GetGarrisonDamage(int fortifications, int warrioirInDomain)
        {
            var garrison = Math.Min(warrioirInDomain, fortifications / 25);
            if (garrison < 100)
                return garrison;
            return (int)(Math.Sqrt(garrison) * 10);
        }

        public static int RecomendWarriorsForSiege(int fortifications, int currentGarrison)
        {
            if (currentGarrison == 0 || fortifications < 1)
                return WarConstants.MinWarrioirsForAtack;

            var garrisonDamage = GetGarrisonDamage(fortifications, currentGarrison);
            var needWarriois = garrisonDamage * 20;
            return needWarriois;
        }
    }
}
