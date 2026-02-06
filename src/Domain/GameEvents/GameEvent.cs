using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Notifications;

namespace YAGO.World.Domain.GameEvents
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
        /// Изменение количества соларов
        /// </summary>
        public IReadOnlyList<ColonyParameter> ColonyParameters { get; }

        /// <summary>
        /// Изменение количества соларов
        /// </summary>
        public IReadOnlyList<ParameterModifier> ParameterModifiers { get; }

        public int SolarChange => (int)(ColonyParameters.FirstOrDefault(x => x.Type == ColonyParameterType.Solars)?.Value ?? 0);

        public GameEvent(
            long id,
            string title,
            string image,
            string[] text,
            double chanceDefault,
            IReadOnlyList<ColonyParameter> colonyParameter,
            IReadOnlyList<ParameterModifier> parameterModifiers)
        {
            Id = id;
            Title = title;
            Image = image;
            Text = text;
            ChanceDefault = chanceDefault;
            ColonyParameters = colonyParameter;
            ParameterModifiers = parameterModifiers;
        }

        public bool Check(IReadOnlyList<ColonyParameter> colonyParameters)
        {
            var randomResult = new Random().NextDouble();
            var finalChance = CalculateFinalChance(colonyParameters);
            return randomResult < finalChance;
        }

        public Notification ToNotification()
        {
            return new Notification(Title, Image, Text, ColonyParameters);
        }

        private double CalculateFinalChance(IReadOnlyList<ColonyParameter> colonyParameters)
        {
            var finalChance = ChanceDefault;

            foreach (var modifier in ParameterModifiers)
            {
                var parameterValue = colonyParameters
                    .Single(x => x.Type == modifier.ParameterType)
                    .Value;
                finalChance += modifier.Coefficient * parameterValue;
            }

            return Math.Clamp(finalChance, 0f, 100f);
        }
    }
}
