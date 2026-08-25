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
            var parameters = new List<IGameParameter>()
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

        public static GameParameter<double> GetSolarDelta(this Colony colony)
        {
            var displayInfo = new DisplayInfo("Доход колонии");
            var result = 0.0;
            result += colony.GetSolarDeltaIndustries(isPrivate: false).Value;
            result += colony.GetSolarDeltaIndustries(isPrivate: true).Value;
            result -= colony.State.GetPublicDebt().SolarDelta;
            result -= colony.GetAdministrationSalary().Value;
            result += GetPopulationTaxSolars(colony).Value;
            return new GameParameter<double>(displayInfo, result);
        }

        public static GameParameter<double> GetSolarDeltaIndustries(this Colony colony, bool isPrivate)
        {
            var displayInfo = new DisplayInfo(isPrivate ? "Частные компании" : "Бюджетные компании");
            var result = 0.0;
            var buildingContext = colony.State.GetBuildingContext();
            foreach (var industry in colony.State.Industries.Values)
            {
                var count = isPrivate ? industry.PrivateCount : industry.StateCount;
                var industryBuildingInfo = industry.GetBuilding(isPrivate, buildingContext);
                result += count * industryBuildingInfo.SolarsDelta;
            }
            return new GameParameter<double>(displayInfo, result);
        }

        private static GameParameter<double> GetPublicDebtService(this Colony colony)
        {
            var displayInfo = new DisplayInfo("Платеж по долгу");
            var result = colony.State.GetPublicDebt().SolarDelta;
            return new GameParameter<double>(displayInfo, result);
        }

        public static GameParameter<double> GetAdministrationSalary(this Colony colony)
        {
            var displayInfo = new DisplayInfo("Госаппарат");
            var result = colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? GameConstants.RulerSalary / GameConstants.WeeksInYear
                : 0;
            return new GameParameter<double>(displayInfo, result);
        }

        private static GameParameter<double> GetPopulationTaxSolars(this Colony colony)
        {
            var displayInfo = new DisplayInfo("Налоги на доходы");
            var result = colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? (GameConstants.RulerSalary * GameConstants.PopulationTaxPercent / 100.0) / GameConstants.WeeksInYear
                : 0;
            return new GameParameter<double>(displayInfo, result);
        }
    }
}
