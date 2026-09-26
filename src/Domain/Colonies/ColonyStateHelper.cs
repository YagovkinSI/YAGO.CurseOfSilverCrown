using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Councils;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Colonies
{
    public static class ColonyStateHelper
    {
        public static double GetSolarDelta(this Colony colony)
        {
            if (!colony.State.Achievements.HasAchievement(AchievementConstants.ColonyOpen))
                return 0;
            return GetSolarDeltaPerYear(colony) / GameConstants.WeeksInYear;
        }

        public static int GetActionPointsDelta(this Colony colony) => 2;

        public static double GetSolarDeltaPerYear(this Colony colony)
        {
            var result = 0.0;
            result += colony.GetSolarDeltaIndustries(isPrivate: false);
            result += colony.GetSolarDeltaIndustries(isPrivate: true);
            result += colony.State.GetPublicDebt().SolarDelta;
            result -= colony.GetAdministrationSalary();
            result -= colony.GetSocialSecuritySolars();
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
            return colony.GetAdministrationSalaries().Sum(salary => salary.SalaryPerYear);
        }

        public static IReadOnlyList<AdministrationSalary> GetAdministrationSalaries(this Colony colony)
        {
            return
            [
                new(CouncilAdvisorRole.Ruler, GetRulerSalary(colony)),
                new(CouncilAdvisorRole.Administrator, GetAdministratorSalary(colony)),
                new(CouncilAdvisorRole.Engineer, GetEngineerSalary(colony)),
                new(CouncilAdvisorRole.Financier, GetFinancierSalary(colony)),
                new(CouncilAdvisorRole.Social, GetSocialSalary(colony)),
            ];
        }

        private static double GetRulerSalary(Colony colony)
        {
            return colony.State.Achievements.HasAchievement(AchievementConstants.RulerContractSigned)
                ? GameConstants.RulerSalary
                : 0;
        }

        private static double GetAdministratorSalary(Colony colony)
        {
            return colony.State.Council.Administrator != null
                ? GameConstants.CamillaAdministrationSalary
                : 0;
        }

        private static double GetEngineerSalary(Colony colony) => 0;

        private static double GetFinancierSalary(Colony colony) => 0;

        private static double GetSocialSalary(Colony colony) => 0;

        public static double GetPopulationTaxSolars(this Colony colony)
        {
            /*
                Налоговые пороги:
                (5, 0.10),
                (15, 0.25),
                (30, 0.35),
                (double.MaxValue, 0.45)
             */

            var population = colony.State.GetPopulation();
            const double administrationEffectiveTax = 0.3;
            var administrationIncome = GetAdministrationSalary(colony) * administrationEffectiveTax;

            var citizenIncome = 1.5;
            return administrationIncome + Math.Max(0, (population - 5)) * citizenIncome;
        }

        public static double GetSocialSecuritySolars(this Colony colony)
        {
            var population = colony.State.GetPopulation();
            var medicalInsuranceCoefficient = colony.State.Policy.Reforms.MedicalInsurance.Value / 2.0;
            return population * (3 + medicalInsuranceCoefficient);
        }
    }
}
