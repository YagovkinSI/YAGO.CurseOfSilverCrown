using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.GameEvents
{
    /// <summary>
    /// Настроки возникновения события
    /// </summary>
    public class EventOccurrenceOptions
    {
        /// <summary>
        /// Требования для события
        /// </summary>
        public IReadOnlyList<RequirementsParameter> Requirements { get; }

        /// <summary>
        /// Вероятность возникновения (от 0 до 1)
        /// </summary>
        public double ChanceDefault { get; }

        /// <summary>
        /// Расчет вероятности события
        /// </summary>
        public IReadOnlyList<KeyValueParameter> ChanceModifiers { get; }

        public EventOccurrenceOptions(
            IReadOnlyList<RequirementsParameter> requirements,
            double chanceDefault,
            IReadOnlyList<KeyValueParameter> chanceModifiers)
        {
            Requirements = requirements;
            ChanceDefault = chanceDefault;
            ChanceModifiers = chanceModifiers;
        }

        public bool Check(ColonyState colonyStats)
        {
            var finalChance = CalculateFinalChance(colonyStats);

            switch (finalChance)
            {
                case <= 0:
                    return false;
                case >= 1:
                    return true;
                default:
                    var randomResult = new Random().NextDouble();
                    return randomResult < finalChance;
            }
        }

        private double CalculateFinalChance(ColonyState colonyStats)
        {
            foreach (var requirement in Requirements)
            {
                if (!requirement.Check(colonyStats))
                    return 0;
            }

            var finalChance = ChanceDefault;
            foreach (var modifier in ChanceModifiers)
            {
                var parameterValue = colonyStats.GetValue(modifier.Name);
                finalChance += modifier.Value * parameterValue;
            }

            return finalChance;
        }
    }
}
