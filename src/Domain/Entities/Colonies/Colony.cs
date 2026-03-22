using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Колония
    /// </summary>
    public class Colony : IEntity
    {
        /// <summary>
        /// Идентифиикатор колонии
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентифиикатор пользователя владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId { get; private set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Параметры колонии
        /// </summary>
        public ColonyStats Stats { get; }

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }

        public Colony(
            long id,
            long userId,
            long shipId,
            string name,
            ColonyStats stats,
            bool deactivated,
            DateTime? deactivateAtUtc)
        {
            Id = id;
            UserId = userId;
            ShipId = shipId;
            Name = name;
            Stats = stats;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
        }

        public static Colony CreateNew(
            long userId,
            string name,
            CodeOfLaws gavernorType)
        {
            var colonyIndustryList = new ColonyIndustryList(
                minningIndustry: Industry.CreateNewMinning(),
                productionIndustry: Industry.CreateNewProduction(),
                serviceIndustry: Industry.CreateNewService());

            var colonyStats = new ColonyStats(
                codeOfLaws: gavernorType,
                solars: 1000,
                festivalEffect: 0,
                currentWeek: 0,
                firstWedding: false,
                maintenance: 100,
                zones: 140,
                colonyIndustryList);

            return new Colony(
                id: default,
                userId: userId,
                shipId: 1,
                name: name,
                colonyStats,
                deactivated: false,
                deactivateAtUtc: null);
        }

        public void Deactivate()
        {
            Deactivated = true;
            DeactivateAtUtc = DateTime.UtcNow;
        }

        public void AddSolars(double value)
        {
            Stats.AddSolars(value);
        }
        public void AddCompany(string industryName, int count, int zonesOccupied, int solarIncome, int population)
        {
            Stats.AddCompany(industryName, count, zonesOccupied, solarIncome, population);
        }

        public void AddFestivalEffect(double effect)
        {
            Stats.AddFestivalEffect(effect);
        }

        public void AddWeek()
        {
            Stats.AddWeek();
        }

        internal void SetFirstWedding()
        {
            Stats.SetFirstWedding();
        }

        public void IssueDecree(Decree decree)
        {
            var colonyStats = Stats;

            if (colonyStats.Solars < -(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves)?.Value ?? 0))
                throw new YagoException("Недостаточно средств.");

            if (colonyStats.ZonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.AreaCapacity_Occupied)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            AddSolars(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves)?.Value ?? 0);
            AddFestivalEffect(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total)?.Value ?? 0);
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters, bool isCycleOver)
        {
            var solars = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves);
            if (solars != null)
                AddSolars((int)solars.Value);

            var (industryChanges, count) = FindIndustryChanges(colonyParameters);
            if (industryChanges != null)
            {
                var zonesOccupied = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.AreaCapacity_Occupied)?.Value ?? 0);
                var solarIncome = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Budget_Balance)?.Value ?? 0);
                var population = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Population_Total)?.Value ?? 0);
                AddCompany(industryChanges, count, zonesOccupied, solarIncome, population);
            }

            var moodTotal = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total);
            if (moodTotal != null)
                AddFestivalEffect(moodTotal.Value);

            var firstWedding = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.FirstWedding);
            if (firstWedding != null)
                SetFirstWedding();

            if (isCycleOver)
                AddWeek();
        }

        private static (string? industryName, int count) FindIndustryChanges(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            if (colonyParameters.Any(x => x.Name == ColonyStatNames.Industry_Minning_Companies))
                return (IndustryNameConstants.Minning, (int)colonyParameters.Single(x => x.Name == ColonyStatNames.Industry_Minning_Companies).Value);
            else if (colonyParameters.Any(x => x.Name == ColonyStatNames.Industry_Production_Companies))
                return (IndustryNameConstants.Production, (int)colonyParameters.Single(x => x.Name == ColonyStatNames.Industry_Production_Companies).Value);
            else if (colonyParameters.Any(x => x.Name == ColonyStatNames.Industry_Service_Companies))
                return (IndustryNameConstants.Service, (int)colonyParameters.Single(x => x.Name == ColonyStatNames.Industry_Service_Companies).Value);
            else
                return (null, 0);
        }
    }
}