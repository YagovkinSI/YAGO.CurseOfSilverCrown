using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterHelper
    {
        public static GameParameterComposition GetBudgetComposition(Colony colony)
        {
            var displayInfo = new DisplayInfo("Бюджет колонии");
            var stateIndustries = colony.GetSolarDeltaIndustries(isPrivate: false);
            var privateIndustries = colony.GetSolarDeltaIndustries(isPrivate: true);
            var populationTax = colony.GetPopulationTaxSolars();
            var publicDebtService = colony.GetPublicDebtService();
            var administrationSalary = colony.GetAdministrationSalary();
            var budget = colony.GetSolarDelta();
            var parameters = new List<GameParameter>()
            {
                stateIndustries,
                privateIndustries,
                populationTax,
                publicDebtService,
                administrationSalary,
                budget
            };
            return new GameParameterComposition(
                displayInfo,
                parameters);
        }

        public static GameParameter GetSolarDelta(this Colony colony)
        {
            var result = 0.0;
            result += colony.GetSolarDeltaIndustries(isPrivate: false).Value;
            result += colony.GetSolarDeltaIndustries(isPrivate: true).Value;
            result += colony.State.GetPublicDebt().SolarDelta;
            result -= colony.GetAdministrationSalary().Value;
            result += GetPopulationTaxSolars(colony).Value;
            return new GameParameter(GameParameterType.SolarsDelta, result);
        }

        public static GameParameter GetSolarDeltaIndustries(this Colony colony, bool isPrivate)
        {
            var result = 0.0;
            var buildingContext = colony.State.GetBuildingContext();
            foreach (var industry in colony.State.Industries.Values)
            {
                var count = isPrivate ? industry.PrivateCount : industry.StateCount;
                var industryBuildingInfo = industry.GetBuilding(isPrivate, buildingContext);
                result += count * industryBuildingInfo.SolarsDelta;
            }
            var type = isPrivate ? GameParameterType.SolarDeltaIndustriesPrivate : GameParameterType.SolarDeltaIndustriesState;
            return new GameParameter(type, result);
        }

        public static GameParameter GetPublicDebtService(this Colony colony)
        {
            var result = colony.State.GetPublicDebt().SolarDelta;
            return new GameParameter(GameParameterType.PublicDebtService, result);
        }

        public static GameParameter GetAdministrationSalary(this Colony colony)
        {
            var result = colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? GameConstants.RulerSalary / GameConstants.WeeksInYear
                : 0;
            return new GameParameter(GameParameterType.AdministrationSalary, result);
        }

        public static GameParameter GetPopulationTaxSolars(this Colony colony)
        {
            var result = colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? (GameConstants.RulerSalary * GameConstants.PopulationTaxPercent / 100.0) / GameConstants.WeeksInYear
                : 0;
            return new GameParameter(GameParameterType.PopulationTaxSolars, result);
        }
    }
}
