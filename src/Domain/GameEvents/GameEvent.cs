using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Notifications;
using YAGO.World.Domain.Ships;

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
        public IReadOnlyList<KeyValueParameter> ParameterChanges { get; }

        /// <summary>
        /// Изменение количества соларов
        /// </summary>
        public IReadOnlyList<KeyValueParameter> ParameterModifiers { get; }

        public GameEvent(
            long id,
            string title,
            string image,
            string[] text,
            double chanceDefault,
            IReadOnlyList<KeyValueParameter> parameterChanges,
            IReadOnlyList<KeyValueParameter> parameterModifiers)
        {
            Id = id;
            Title = title;
            Image = image;
            Text = text;
            ChanceDefault = chanceDefault;
            ParameterChanges = parameterChanges;
            ParameterModifiers = parameterModifiers;
        }

        public bool Check(Colony colony, ColonyCompanies companies, Ship ship)
        {
            var randomResult = new Random().NextDouble();
            var finalChance = CalculateFinalChance(colony, companies, ship);
            return randomResult < finalChance;
        }

        public Notification ToNotification()
        {
            return new Notification(Title, Image, Text, ParameterChanges);
        }

        private double CalculateFinalChance(Colony colony, ColonyCompanies companies, Ship ship)
        {
            var finalChance = ChanceDefault;

            foreach (var modifier in ParameterModifiers)
            {
                var parameterValue = GetGameParameter(colony, companies, ship, modifier.Name);
                finalChance += modifier.Value * parameterValue;
            }

            return Math.Clamp(finalChance, 0f, 1f);
        }

        private double GetGameParameter(
            Colony colony,
            ColonyCompanies companies,
            Ship ship,
            string name)
        {
            var budget = new Budget(
                colony,
                companies,
                ship);
            var mood = new Mood(
                colony,
                companies,
                colony.FestivalEffect);
            var population = new Population(
                colony,
                companies);
            var areaCapacity = new AreaCapacity(
                colony,
                companies,
                ship);

            return name switch
            {
                ColonyParameterNames.Economic_Reserves => colony.Solars,
                ColonyParameterNames.Mood_Total => mood.Total,
                ColonyParameterNames.Population_Total => population.Total,
                ColonyParameterNames.AreaCapacity_Occupied => areaCapacity.Occupied,
                ColonyParameterNames.Economic_Budget_Balance => budget.Balance,
                ColonyParameterNames.Companies_Minning_EngineeringTeam => companies.Companies.Count(x => x.Id == 1),
                ColonyParameterNames.Companies_Minning_MiningBrigade => companies.Companies.Count(x => x.Id == 2),
                ColonyParameterNames.Companies_Minning_RehabilitationContingent => companies.Companies.Count(x => x.Id == 3),
                ColonyParameterNames.AreaCapacity_Total => areaCapacity.Total,
                ColonyParameterNames.Laws_CodeOfLaws => (double)colony.CodeOfLaws,
                _ => throw new YagoUnknownTypeException(name)
            };
        }
    }
}
