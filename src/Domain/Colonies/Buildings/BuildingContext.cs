using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies.Industries;

namespace YAGO.World.Domain.Colonies.Buildings
{
    public class BuildingContext
    {
        public Dictionary<ColonyIndustryType, int> BuildingCount { get; }

        /// <summary>
        /// Налог на прибыль корпораций (в процентах, например 20 = 20%)
        /// </summary>
        public float CorporateTaxRate { get; }

        /// <summary>
        /// Дополнительные налоги (например на социальную страховку)
        /// </summary>
        public float MiningTaxRate { get; } = 25;

        public double Stability { get; }

        /// <summary>
        /// Логистический эффект близости к Церере (во сколько раз уменьшаются расходы)
        /// </summary>
        public double LogisticEffect { get; }

        /// <summary>
        /// Коэффициент налоговой льготы от стимулирования автоматизации или занятости.
        /// Меньше 1 на обоих полюсах шкалы (льгота и роботизации, и ручному труду), равен 1 в нейтральном состоянии.
        /// </summary>
        public double AutomationTaxCoefficient { get; }

        /// <summary>
        /// Коэффициент занятости от стимулирования автоматизации.
        /// Меньше 1 при роботизации (людей заменяют роботы), больше 1 при ручном труде.
        /// </summary>
        public double AutomationPopulationCoefficient { get; }

        /// <summary>
        /// Коэффициент стоимости инвестиций: больше 1 при роботизации (дороже), меньше 1 при ручном труде (дешевле).
        /// </summary>
        public double AutomationInvestmentCoefficient { get; }

        /// <summary>
        /// Коэфициент оптимальности впуска продукции при разных уровнях автомаитизации
        /// </summary>
        public double AutomationGdpCoefficient { get; }

        public BuildingContext(
            Dictionary<ColonyIndustryType, int> buildingCount,
            float corporateTaxRate,
            double stability,
            double logisticEffect,
            double automationTaxCoefficient,
            double automationPopulationCoefficient,
            double automationInvestmentCoefficient,
            double automationGdpCoefficient)
        {
            BuildingCount = buildingCount;
            CorporateTaxRate = corporateTaxRate;
            Stability = stability;
            LogisticEffect = logisticEffect;
            AutomationTaxCoefficient = automationTaxCoefficient;
            AutomationPopulationCoefficient = automationPopulationCoefficient;
            AutomationInvestmentCoefficient = automationInvestmentCoefficient;
            AutomationGdpCoefficient = automationGdpCoefficient;
        }

        /// <summary>
        /// Эффективная ставка налога с учётом льгот
        /// </summary>
        public float EffectiveTaxRate(ColonyIndustryType industryType)
        {
            var corporateTaxRate = CorporateTaxRate * (float)AutomationTaxCoefficient;
            var baseAdditionalTax = 15;
            return industryType switch
            {
                ColonyIndustryType.Mining => Math.Max(0, corporateTaxRate + baseAdditionalTax + MiningTaxRate),
                _ => Math.Max(0, corporateTaxRate + baseAdditionalTax)
            };
        }

        internal double GetCompetition(ColonyIndustryType type) => BuildingCount[type];
    }
}
