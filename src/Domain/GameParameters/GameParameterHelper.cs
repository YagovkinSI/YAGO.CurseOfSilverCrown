using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterHelper
    {
        public static double GetSolarDelta(this Colony colony)
        {
            var result = 0.0;
            result += colony.GetSolarDeltaIndustries(isPrivate: false);
            result += colony.GetSolarDeltaIndustries(isPrivate: true);
            result += colony.State.GetPublicDebt().SolarDelta;
            result -= colony.GetAdministrationSalary();
            result += GetPopulationTaxSolars(colony);
            return result;
        }

        public static double GetSolarDeltaIndustries(this Colony colony, bool isPrivate)
        {
            var result = 0.0;
            var buildingContext = colony.State.GetBuildingContext();
            foreach (var industry in colony.State.Industries.Values)
            {
                var count = isPrivate ? industry.PrivateCount : industry.StateCount;
                var industryBuildingInfo = industry.GetBuilding(isPrivate, buildingContext);
                result += count * industryBuildingInfo.SolarsDelta;
            }
            return result;
        }

        public static double GetAdministrationSalary(this Colony colony)
        {
            return colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? GameConstants.RulerSalary / GameConstants.WeeksInYear
                : 0;
        }

        public static double GetPopulationTaxSolars(this Colony colony)
        {
            return colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? (GameConstants.RulerSalary * GameConstants.PopulationTaxPercent / 100.0) / GameConstants.WeeksInYear
                : 0;
        }
    }
}
