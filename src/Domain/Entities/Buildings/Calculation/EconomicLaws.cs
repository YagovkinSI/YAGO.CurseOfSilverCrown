using System;

namespace YAGO.World.Domain.Entities.Buildings.Calculation
{

    /// <summary>
    /// Законы, влияющие на экономику предприятия
    /// </summary>
    public class EconomicLaws
    {
        /// <summary>
        /// Налог на прибыль корпораций (в процентах, например 20 = 20%)
        /// </summary>
        public float CorporateTaxRate { get; set; } = 20f;

        /// <summary>
        /// Медицинский полис (уровень от -2 до +2)
        /// </summary>
        public int MedicalPolicyLevel { get; set; } = 0;

        /// <summary>
        /// Ставка стимулирования автоматизации (уровень от -3 до +3)
        /// </summary>
        public int AutomationPolicyLevel { get; set; } = 0;

        /// <summary>
        /// Бонус к налогу от политики стимулирования (в процентах)
        /// </summary>
        public float TaxBonusPercent
        {
            get
            {
                // Уровень 0 = 0%, каждый шаг даёт 15% бонуса (как в твоей шкале)
                return AutomationPolicyLevel * 15f;
            }
        }

        /// <summary>
        /// Эффективная ставка налога с учётом льгот
        /// </summary>
        public float EffectiveTaxRate
        {
            get
            {
                // Налог не может быть ниже 0%
                return Math.Max(0, CorporateTaxRate - TaxBonusPercent);
            }
        }

        /// <summary>
        /// Множитель эффективности от автоматизации (влияет на выручку и расходы)
        /// </summary>
        public float AutomationEfficiencyMultiplier
        {
            get
            {
                // -3 (полная роботизация): +20% эффективности
                // 0: 0%
                // +3 (полный ручной труд): -20% эффективности
                return 1f + AutomationPolicyLevel / 3f * 0.2f;
            }
        }

        /// <summary>
        /// Множитель затрат на персонал от политики автоматизации
        /// </summary>
        public float LaborCostMultiplier
        {
            get
            {
                // -3 (роботизация): люди дорогие, но их мало → затраты на персонал ниже
                // +3 (ручной труд): людей много → затраты выше
                return 1f - AutomationPolicyLevel / 3f * 0.15f;
            }
        }

        /// <summary>
        /// Множитель численности персонала от политики автоматизации
        /// </summary>
        public float HeadcountMultiplier
        {
            get
            {
                // -3 (роботизация): мало людей (0.5 от нормы)
                // +3 (ручной труд): много людей (1.5 от нормы)
                return 1f + AutomationPolicyLevel / 3f * 0.5f;
            }
        }
    }
}