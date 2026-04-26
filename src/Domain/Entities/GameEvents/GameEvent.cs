using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class GameEvent
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Вероятность возникновения (от 0 до 1)
        /// </summary>
        public double ChanceDefault { get; }

        /// <summary>
        /// Требования для события
        /// </summary>
        public IReadOnlyList<RequirementsParameter> Requirements { get; }

        /// <summary>
        /// Расчет вероятности события
        /// </summary>
        public IReadOnlyList<KeyValueParameter> ParameterModifiers { get; }

        public Episode Episode { get; }

        public int AdditionalDaysPassed { get; }

        public GameEvent(
            string id,
            double chanceDefault,
            IReadOnlyList<RequirementsParameter> requirements,
            IReadOnlyList<KeyValueParameter> parameterModifiers,
            Episode episode,
            int? additionalDaysPassed = null)
        {
            Id = id;
            ChanceDefault = chanceDefault;
            Requirements = requirements;
            ParameterModifiers = parameterModifiers;
            Episode = episode;
            AdditionalDaysPassed = additionalDaysPassed ?? 0;
        }

        public bool Check(Colony colony)
        {
            var finalChance = CalculateFinalChance(colony);

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

        private double CalculateFinalChance(Colony colony)
        {
            var colonyStats = colony.Stats;

            foreach (var requirement in Requirements)
            {
                if (!requirement.Check(colonyStats))
                    return 0;
            }

            var finalChance = ChanceDefault;
            foreach (var modifier in ParameterModifiers)
            {
                var parameterValue = colonyStats.GetGameParameter(modifier.Name);
                finalChance += modifier.Value * parameterValue;
            }

            return finalChance;
        }
    }
}
