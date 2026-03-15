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
        public long Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Иллюстрация
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// Текстовое описание
        /// </summary>
        public string[] Text { get; }

        /// <summary>
        /// Вероятность возникновения (от 0 до 1)
        /// </summary>
        public double ChanceDefault { get; }

        /// <summary>
        /// Требования для события
        /// </summary>
        public IReadOnlyList<KeyValueParameter> Requirements { get; }

        /// <summary>
        /// Расчет вероятности события
        /// </summary>
        public IReadOnlyList<KeyValueParameter> ParameterModifiers { get; }

        /// <summary>
        /// Изменение параметров по результатам событий
        /// </summary>
        public IReadOnlyList<KeyValueParameter> ParameterChanges { get; }

        public GameEvent(
            long id,
            string title,
            string image,
            string[] text,
            double chanceDefault,
            IReadOnlyList<KeyValueParameter> requirements,
            IReadOnlyList<KeyValueParameter> parameterModifiers,
            IReadOnlyList<KeyValueParameter> parameterChanges)
        {
            Id = id;
            Title = title;
            Image = image;
            Text = text;
            ChanceDefault = chanceDefault;
            Requirements = requirements;
            ParameterModifiers = parameterModifiers;
            ParameterChanges = parameterChanges;
        }

        public bool Check(Colony colony)
        {
            var randomResult = new Random().NextDouble();
            var finalChance = CalculateFinalChance(colony);
            return randomResult < finalChance;
        }

        public Slide ToNotification()
        {
            return new Slide(Title, Image, Text, ParameterChanges);
        }

        private double CalculateFinalChance(Colony colony)
        {
            var colonyStats = colony.Stats;

            var finalChance = ChanceDefault;

            foreach (var requirement in Requirements)
            {
                var parameterValue = colonyStats.GetGameParameter(requirement.Name);
                if (parameterValue < requirement.Value)
                    return 0;
            }

            foreach (var modifier in ParameterModifiers)
            {
                var parameterValue = colonyStats.GetGameParameter(modifier.Name);
                finalChance += modifier.Value * parameterValue;
            }

            return Math.Clamp(finalChance, 0f, 1f);
        }
    }
}
