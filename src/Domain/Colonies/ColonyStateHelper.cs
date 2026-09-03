using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Colonies
{
    public static class ColonyStateHelper
    {
        public static double GetSolarDelta(this Colony colony)
        {
            return GetSolarDeltaPerYear(colony) / GameConstants.WeeksInYear;
        }

        public static double GetSolarDeltaPerYear(this Colony colony)
        {
            var result = 0.0;
            result += colony.GetSolarDeltaIndustries(isPrivate: false);
            result += colony.GetSolarDeltaIndustries(isPrivate: true);
            result += colony.State.GetPublicDebt().SolarDelta;
            result -= colony.GetAdministrationSalary();
            result += colony.GetPopulationTaxSolars();
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
                result += count * industryBuildingInfo.SolarsDeltaPerYear;
            }
            return result;
        }

        public static double GetAdministrationSalary(this Colony colony)
        {
            return colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? GameConstants.RulerSalary
                : 0;
        }

        public static double GetPopulationTaxSolars(this Colony colony)
        {
            return colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? GameConstants.RulerSalary * GameConstants.PopulationTaxPercent / 100.0
                : 0;
        }
    }
}
