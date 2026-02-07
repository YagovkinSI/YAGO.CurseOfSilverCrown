using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Companies;
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

        public bool Check(ColonyWithShipAndContracts colonyWithShipAndContracts)
        {
            var randomResult = new Random().NextDouble();
            var finalChance = CalculateFinalChance(colonyWithShipAndContracts);
            return randomResult < finalChance;
        }

        public Notification ToNotification()
        {
            return new Notification(Title, Image, Text, ColonyParameters);
        }

        private double CalculateFinalChance(ColonyWithShipAndContracts colonyWithShipAndContracts)
        {
            var finalChance = ChanceDefault;

            foreach (var modifier in ParameterModifiers)
            {
                var parameterValue = GetGameParameter(colonyWithShipAndContracts, modifier.ParameterType);
                finalChance += modifier.Coefficient * parameterValue;
            }

            return Math.Clamp(finalChance, 0f, 100f);
        }

        private double GetGameParameter(
            ColonyWithShipAndContracts colonyWithShipAndContracts,
            ColonyParameterType colonyParameterType)
        {
            return colonyParameterType switch
            {
                ColonyParameterType.Unknown => throw new NotImplementedException(),
                ColonyParameterType.Solars => colonyWithShipAndContracts.Colony.Solars,
                ColonyParameterType.GavernorType => colonyWithShipAndContracts.GavernorType,
                ColonyParameterType.Population => colonyWithShipAndContracts.Population,
                ColonyParameterType.ZonesOccupied => colonyWithShipAndContracts.ZonesOccupied,
                ColonyParameterType.SolarIncome => colonyWithShipAndContracts.SolarIncome,
                ColonyParameterType.EngineeringTeam => GetContactCount(colonyWithShipAndContracts.Contracts, 1),
                ColonyParameterType.MiningBrigade => GetContactCount(colonyWithShipAndContracts.Contracts, 2),
                ColonyParameterType.RehabilitationContingent => GetContactCount(colonyWithShipAndContracts.Contracts, 3),
            };
        }

        private double GetContactCount(Dictionary<Company, int> contacts, long contactId)
        {
            return !contacts.Any(x => x.Key.Id == contactId) ? 0 : contacts.Single(x => x.Key.Id == contactId).Value;
        }
    }
}
