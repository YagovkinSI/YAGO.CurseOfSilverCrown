using YAGO.World.Domain.Constants;

namespace YAGO.World.Infrastructure.Helpers
{
    public static class FortificationsHelper
    {
        public static int GetFortCoef(int fortifications)
        {
            return fortifications / GameSettings.GarrisonPlaceCost;
        }
    }
}
