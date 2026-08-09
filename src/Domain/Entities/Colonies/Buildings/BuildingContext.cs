using System;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
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

        public BuildingContext(
            float corporateTaxRate,
            double stability)
        {
            CorporateTaxRate = corporateTaxRate;
            Stability = stability;
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
