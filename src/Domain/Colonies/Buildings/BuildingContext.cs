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

        public BuildingContext(
            float corporateTaxRate,
            double stability,
            double logisticEffect = 1.0)
        {
            CorporateTaxRate = corporateTaxRate;
            Stability = stability;
            LogisticEffect = logisticEffect;
        }

        /// <summary>
        /// Эффективная ставка налога с учётом льгот
        /// </summary>
        public float EffectiveTaxRate
        {
            get
            {
                // Налог не может быть ниже 0%
                return Math.Max(0, CorporateTaxRate + AdditionalTaxRate);
            }
        }
    }
}
