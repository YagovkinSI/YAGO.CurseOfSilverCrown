using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Exceptions;

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
            var finalChance = ChanceDefault;

            foreach (var requirement in Requirements)
            {
                var parameterValue = GetGameParameter(colony, requirement.Name);
                if (parameterValue < requirement.Value)
                    return 0;
            }

            foreach (var modifier in ParameterModifiers)
            {
                var parameterValue = GetGameParameter(colony, modifier.Name);
                finalChance += modifier.Value * parameterValue;
            }

            return Math.Clamp(finalChance, 0f, 1f);
        }

        private double GetGameParameter(
            Colony colony,
            string name)
        {
            return name switch
            {
                ColonyParameterNames.Economic_Reserves => colony.Solars,
                ColonyParameterNames.Mood_Total => colony.MoodTotalCacl(),
                ColonyParameterNames.Population_Total => colony.PopulationTotal,
                ColonyParameterNames.AreaCapacity_Occupied => colony.ZonesOccupied,
                ColonyParameterNames.Economic_Budget_Balance => colony.BudgetBalance,
                ColonyParameterNames.Industry_Minning_Available => 12 - colony.Industries.Minning.CompanyCount,
                ColonyParameterNames.Industry_Minning_Companies => colony.Industries.Minning.CompanyCount,
                ColonyParameterNames.Industry_Production_Companies => colony.Industries.Production.CompanyCount,
                ColonyParameterNames.Industry_Service_Companies => colony.Industries.Service.CompanyCount,
                ColonyParameterNames.Industry_Service_Need => (colony.PopulationTotal / 50.0) - colony.Industries.Service.CompanyCount - 1.5,
                ColonyParameterNames.AreaCapacity_Total => colony.ZonesTotal,
                ColonyParameterNames.AreaCapacity_Available => colony.ZonesAvailable,
                ColonyParameterNames.Laws_CodeOfLaws => (double)colony.CodeOfLaws,
                ColonyParameterNames.Laws_CodeOfLaws_HighTax => colony.CodeOfLaws == CodeOfLaws.Capitalist ? 1 : 0,
                ColonyParameterNames.Laws_CodeOfLaws_HighStandart => colony.CodeOfLaws == CodeOfLaws.Humanist ? 1 : 0,
                ColonyParameterNames.Attractiveness_Total => colony.AttractivenessTotalCalc(),
                ColonyParameterNames.FirstWedding => colony.FirstWedding ? 1 : 0,
                ColonyParameterNames.CurrentWeek => colony.CurrentWeek,
                _ => throw new YagoUnknownTypeException(name)
            };
        }
    }
}
