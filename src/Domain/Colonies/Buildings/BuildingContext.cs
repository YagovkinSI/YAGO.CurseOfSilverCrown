using System;

namespace YAGO.World.Domain.Colonies.Buildings
{
    public class BuildingContext
    {
        /// <summary>
        /// Налог на прибыль корпораций (в процентах, например 20 = 20%)
        /// </summary>
        public float CorporateTaxRate { get; }

        /// <summary>
        /// Дополнительные налоги (например на социальную страховку)
        /// </summary>
        public float AdditionalTaxRate { get; } = 13.3f;

        public double Stability { get; }

        /// <summary>
        /// Логистический эффект близости к Церере (0.775 / корень из расстояния до Цереры)
        /// </summary>
        public double LogisticEffect { get; }

        /// <summary>
        /// Коэффициент налоговых льгот от стимулирования автоматизации.
        /// Больше 1 при роботизации (скидка), меньше 1 при ручном труде (надбавка).
        /// </summary>
        public double AutomationTaxCoefficient { get; }

        /// <summary>
        /// Коэффициент занятости от стимулирования автоматизации.
        /// Меньше 1 при роботизации (людей заменяют роботы), больше 1 при ручном труде.
        /// </summary>
        public double AutomationPopulationCoefficient { get; }


        /// <summary>
        /// Коэффициент влияния автоматизации на выпуск продукции и услуг
        /// </summary>
        public double AutomationGdpCoefficient { get; }

        public BuildingContext(
            float corporateTaxRate,
            double stability,
            double logisticEffect = 1.0,
            double automationTaxCoefficient = 1.0,
            double automationPopulationCoefficient = 1.0,
            double automationGdpCoefficient = 1.0)
        {
            CorporateTaxRate = corporateTaxRate;
            Stability = stability;
            LogisticEffect = logisticEffect;
            AutomationTaxCoefficient = automationTaxCoefficient;
            AutomationPopulationCoefficient = automationPopulationCoefficient;
            AutomationGdpCoefficient = automationGdpCoefficient;
        }

        /// <summary>
        /// Эффективная ставка налога с учётом льгот
        /// </summary>
        public float EffectiveTaxRate
        {
            get
            {
                var corporateTaxRate = CorporateTaxRate * (float)AutomationTaxCoefficient;
                // Налог не может быть ниже 0%
                return Math.Max(0, corporateTaxRate + AdditionalTaxRate);
            }
        }
    }
}
