using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Companies;
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
            IReadOnlyList<ColonyParameter> colonyParameters,
            IReadOnlyList<ParameterModifier> parameterModifiers)
        {
            Id = id;
            Title = title;
            Image = image;
            Text = text;
            ChanceDefault = chanceDefault;
            ColonyParameters = colonyParameters;
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
            return new Notification(Title, Image, Text, ColonyParameters);
        }

        private double CalculateFinalChance(Colony colony, ColonyCompanies companies, Ship ship)
        {
            var finalChance = ChanceDefault;

            foreach (var modifier in ParameterModifiers)
            {
                var parameterValue = GetGameParameter(colony, companies, ship, modifier.ParameterType);
                finalChance += modifier.Coefficient * parameterValue;
            }

            return Math.Clamp(finalChance, 0f, 100f);
        }

        private double GetGameParameter(
            Colony colony,
            ColonyCompanies companies,
            Ship ship,
            ColonyParameterType colonyParameterType)
        {
            var budget = new Budget(
                colony,
                companies,
                ship);
            var loyality = new Loyalty(
                colony,
                companies);
            var population = new Population(
                colony,
                companies);
            var areaCapacity = new AreaCapacity(
                colony,
                companies,
                ship);

            return colonyParameterType switch
            {
                ColonyParameterType.Unknown => throw new NotImplementedException(),
                ColonyParameterType.Solars => colony.Solars,
                ColonyParameterType.GavernorType => loyality.Total,
                ColonyParameterType.Population => population.Total,
                ColonyParameterType.ZonesOccupied => areaCapacity.Occupied,
                ColonyParameterType.SolarIncome => budget.Balance,
                ColonyParameterType.EngineeringTeam => companies.Companies.Count(x => x.Id == 1),
                ColonyParameterType.MiningBrigade => companies.Companies.Count(x => x.Id == 2),
                ColonyParameterType.RehabilitationContingent => companies.Companies.Count(x => x.Id == 3),
                ColonyParameterType.ZonesTotal => areaCapacity.Total
            };
        }
    }
}
