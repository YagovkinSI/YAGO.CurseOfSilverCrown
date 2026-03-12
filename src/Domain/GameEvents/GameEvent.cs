using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Episodes;
using YAGO.World.Domain.Exceptions;

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

        public bool Check(Colony colony, ColonyCompanies companies, Ship ship)
        {
            var randomResult = new Random().NextDouble();
            var finalChance = CalculateFinalChance(colony, companies, ship);
            return randomResult < finalChance;
        }

        public Slide ToNotification()
        {
            return new Slide(Title, Image, Text, ParameterChanges);
        }

        private double CalculateFinalChance(Colony colony, ColonyCompanies companies, Ship ship)
        {
            var finalChance = ChanceDefault;

            foreach (var requirement in Requirements)
            {
                var parameterValue = GetGameParameter(colony, companies, ship, requirement.Name);
                if (parameterValue < requirement.Value)
                    return 0;
            }

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
            var budget = new Budget(colony, companies, ship);
            var mood = new Mood(colony, companies);
            var population = new Population(colony, companies);
            var areaCapacity = new AreaCapacity(colony, companies, ship);
            var attractiveness = new Attractiveness(colony, companies);
            var colonyStats = colony.Stats;

            return name switch
            {
                ColonyParameterNames.Economic_Reserves => colonyStats.Solars,
                ColonyParameterNames.Mood_Total => mood.Total,
                ColonyParameterNames.Population_Total => population.Total,
                ColonyParameterNames.AreaCapacity_Occupied => areaCapacity.Occupied,
                ColonyParameterNames.Economic_Budget_Balance => budget.Balance,
                ColonyParameterNames.Industry_Minning_Available => 12 - companies.Companies.Count(x => x.Id == 1 || x.Id == 2 || x.Id == 3),
                ColonyParameterNames.Companies_Minning_EngineeringTeam => companies.Companies.Count(x => x.Id == 1),
                ColonyParameterNames.Companies_Minning_MiningBrigade => companies.Companies.Count(x => x.Id == 2),
                ColonyParameterNames.Companies_Minning_RehabilitationContingent => companies.Companies.Count(x => x.Id == 3),
                ColonyParameterNames.Industry_Production_Companies => companies.Companies.Count(x => x.Id == 4),
                ColonyParameterNames.Industry_Service_Companies => companies.Companies.Count(x => x.Id == 5),
                ColonyParameterNames.Industry_Service_Need => (population.Total / 50.0) - companies.Companies.Count(x => x.Id == 5) - 1.5,
                ColonyParameterNames.AreaCapacity_Total => areaCapacity.Total,
                ColonyParameterNames.AreaCapacity_Available => areaCapacity.Available,
                ColonyParameterNames.Laws_CodeOfLaws => (double)colonyStats.CodeOfLaws,
                ColonyParameterNames.Laws_CodeOfLaws_HighTax => colonyStats.CodeOfLaws == CodeOfLaws.Capitalist ? 1 : 0,
                ColonyParameterNames.Laws_CodeOfLaws_HighStandart => colonyStats.CodeOfLaws == CodeOfLaws.Humanist ? 1 : 0,
                ColonyParameterNames.Attractiveness_Total => attractiveness.Total,
                ColonyParameterNames.FirstWedding => colonyStats.FirstWedding ? 1 : 0,
                ColonyParameterNames.CurrentWeek => colonyStats.CurrentWeek,
                _ => throw new YagoUnknownTypeException(name)
            };
        }
    }
}
