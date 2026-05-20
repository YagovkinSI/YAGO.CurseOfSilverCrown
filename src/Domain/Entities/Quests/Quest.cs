using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Quests
{
    public class Quest
    {
        public string Id { get; }
        public string Name { get; }
        public QuestType Type { get; }

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

        public Slide PrologueSlide { get; }
        public Episode CompleteEpisode { get; }

        public Quest(
            string id,
            string name,
            QuestType type,
            IReadOnlyList<RequirementsParameter> requirements,
            double chanceDefault,
            IReadOnlyList<KeyValueParameter> chanceModifiers,
            Slide prologueSlide,
            Episode completeEpisode)
        {
            Id = id;
            Name = name;
            Type = type;
            PrologueSlide = prologueSlide;
            CompleteEpisode = completeEpisode;
            ChanceDefault = chanceDefault;
            Requirements = requirements;
            ChanceModifiers = chanceModifiers;
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
            foreach (var modifier in ChanceModifiers)
            {
                var parameterValue = colonyStats.GetGameParameter(modifier.Name);
                finalChance += modifier.Value * parameterValue;
            }

            return finalChance;
        }
    }
}
