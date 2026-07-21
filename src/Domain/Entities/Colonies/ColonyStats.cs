using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyStats
    {
        public ColonySettings Settings { get; }
        public ColonyResources Resources { get; }
        public ColonyIndustryList Industries { get; }

        /// <summary>
        /// Доход очков действий
        /// </summary>
        public int ActionPointsTrend { get; private set; }

        /// <summary>
        /// Настроение
        /// </summary>
        public LimitedDouble MoodTotal { get; private set; }

        /// <summary>
        /// Текущая неделя
        /// </summary>
        public int CurrentWeek { get; private set; }

        /// <summary>
        /// Была ли первая свадьба
        /// </summary>
        public bool FirstWedding { get; private set; }

        public ColonyStats(
            ColonySettings settings,
            ColonyResources resources,
            ColonyIndustryList industries,
            int actionPointsTrend,
            double moodTotal,
            int currentWeek,
            bool firstWedding)
        {
            Settings = settings;
            Resources = resources;
            Industries = industries;
            ActionPointsTrend = actionPointsTrend;
            MoodTotal = new LimitedDouble(moodTotal, 0, 100);
            CurrentWeek = currentWeek;
            FirstWedding = firstWedding;
        }

        public static ColonyStats CreateNew()
        {
            var colonySettings = ColonySettings.CreateNew();
            var colonyResources = ColonyResources.CreateNew();
            var colonyIndustryList = new ColonyIndustryList(
                administrativeIndustry: AdministrativeIndustry.CreateNew(),
                minningIndustry: MinningIndustry.CreateNew(),
                productionIndustry: ProductionIndustry.CreateNew(),
                serviceIndustry: ServiceIndustry.CreateNew());
            return new ColonyStats(
                colonySettings,
                colonyResources,
                colonyIndustryList,
                actionPointsTrend: 1,
                moodTotal: 50,
                currentWeek: 1,
                firstWedding: false);
        }

        public double GetGameParameter(string parameterName)
        {
            return parameterName switch
                {
                    StateKeys.ReformPoints.Reserve => Resources.ActionPoints.Value,
                    StateKeys.ReformPoints.Income => ActionPointsTrend,
                    StateKeys.Solars.Reserve => Resources.Solars,
                    StateKeys.Mood.Reserve => MoodTotal.Value,
                    StateKeys.Mood.Income => MoodTotalBalanceCacl(),
                    StateKeys.Population => GetPopulation(),
                    StateKeys.Modules.Used => GetZonesOccupied(),
                    StateKeys.Solars.Income => GetSolarsIncome(),
                    StateKeys.Modules.Total => Resources.ZonesTotal,
                    StateKeys.Modules.Free => GetZonesAvailable(),
                    StateKeys.Reforms.TaxLevel => Settings.TaxLevel,
                    StateKeys.Reforms.SocialGuaranteesLevel => Settings.SocialGuaranteesLevel,
                    StateKeys.Industries.Attractiveness => AttractivenessTotalCalc(),
                    StateKeys.Flags.Events.FirstWedding => FirstWedding ? 1 : 0,
                    StateKeys.Counters.Turns => CurrentWeek,
                    StateKeys.Industries.Service.Buildings.Need => Industries.Service.NeedCalculation(GetPopulation()),

                    StateKeys.Industries.Administrative.Buildings.State => Industries.Administrative.StateOwnedBuildingCount,
                    StateKeys.Industries.Administrative.Buildings.Private => Industries.Administrative.PrivateBuildingCount,
                    StateKeys.Industries.Minning.Buildings.Available => Industries.Minning.UnitAvailable,
                    StateKeys.Industries.Minning.Buildings.State => Industries.Minning.StateOwnedBuildingCount,
                    StateKeys.Industries.Minning.Buildings.Private => Industries.Minning.PrivateBuildingCount,
                    StateKeys.Industries.Production.Buildings.State => Industries.Production.StateOwnedBuildingCount,
                    StateKeys.Industries.Production.Buildings.Private => Industries.Production.PrivateBuildingCount,
                    StateKeys.Industries.Service.Buildings.State => Industries.Service.StateOwnedBuildingCount,
                    StateKeys.Industries.Service.Buildings.Private => Industries.Service.PrivateBuildingCount,
                    _ => throw new YagoUnknownTypeException(parameterName)
                };
        }

        public int GetPopulation()
        {
            var result = 0;
            foreach (var industry in Industries)
            {
                var building = BuildingDataset.GetByType(industry.Type);
                result += industry.BuildingCount * building.Population;
            }
            return result;
        }

        public int GetZonesOccupied()
        {
            var result = 0;
            foreach (var industry in Industries)
            {
                var building = BuildingDataset.GetByType(industry.Type);
                result += industry.BuildingCount * building.ZonesOccupied;
            }
            return result;
        }

        public double GetSolarsIncome()
        {
            var result = 0.0;
            foreach (var industry in Industries)
            {
                var building = BuildingDataset.GetByType(industry.Type);
                result += (industry.PrivateBuildingCount + 3 * industry.StateOwnedBuildingCount) * building.SolarsIncome;
            }
            return result;
        }

        public int GetZonesAvailable() => Resources.ZonesTotal - GetZonesOccupied();

        public void IssueDecree(Decree decree)
        {
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.ReformPoints.Reserve)?.Value ?? 0;
            if (Resources.ActionPoints.Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.Solars.Reserve)?.Value ?? 0;
            if (Resources.Solars < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = GetZonesAvailable();
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.Modules.Used)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            Resources.AddActionPoints((int)actionPoints);
            Resources.AddSolars(solarResservesParameter);
            MoodTotal += decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.Mood.Reserve)?.Value ?? 0;
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            foreach (var paramter in colonyParameters)
            {
                Action action = paramter.Name switch
                {
                    StateKeys.ReformPoints.Reserve => () => Resources.AddActionPoints((int)paramter.Value),
                    StateKeys.ReformPoints.Income => () => ActionPointsTrend += (int)paramter.Value,
                    StateKeys.Solars.Reserve => () => Resources.AddSolars((int)paramter.Value),

                    StateKeys.Industries.Administrative.Buildings.State => () => Industries.Administrative.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Administrative.Buildings.Private => () => Industries.Administrative.AddPrivateBuilding((int)paramter.Value),

                    StateKeys.Industries.Minning.Buildings.State => () => Industries.Minning.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Minning.Buildings.Private => () => Industries.Minning.AddPrivateBuilding((int)paramter.Value),

                    StateKeys.Industries.Production.Buildings.State => () => Industries.Production.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Production.Buildings.Private => () => Industries.Production.AddPrivateBuilding((int)paramter.Value),

                    StateKeys.Industries.Service.Buildings.State => () => Industries.Service.AddStateOwnedBuilding((int)paramter.Value),
                    StateKeys.Industries.Service.Buildings.Private => () => Industries.Service.AddPrivateBuilding((int)paramter.Value),

                    StateKeys.Mood.Reserve => () => MoodTotal += paramter.Value,
                    StateKeys.Flags.Events.FirstWedding => () => FirstWedding = true,
                    StateKeys.Reforms.TaxLevel => () => Settings.SetTaxLevel((int)paramter.Value),
                    StateKeys.Reforms.SocialGuaranteesLevel => () => Settings.SetSocialGuaranteesLevel((int)paramter.Value),
                    StateKeys.Counters.Turns => () => CurrentWeek += (int)paramter.Value,
                    _ => () => { },
                }; 
                action.Invoke();
            }
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -15 * Settings.TaxLevel;
            var standartsEffect = -15 * Settings.SocialGuaranteesLevel;
            var stabilityEffect = Math.Min(50, CurrentWeek / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double MoodTotalBalanceCacl()
        {
            var socialGuaranteesCoef = 1 + ((Settings.SocialGuaranteesLevel - 3) / 10.0);
            return -GetPopulation() * 0.01 * socialGuaranteesCoef;
        }

        public double GdpCalc()
        {
            var socialGuaranteesCoef = 1 + ((Settings.SocialGuaranteesLevel - 3) / 10.0);
            return GetPopulation() * socialGuaranteesCoef * 10.0;
        }

        public double GdpTrendCalc()
        {
            var miningWorkerTrend = Industries.Minning.UnitAvailable > 0 ? 20 : 0;
            var productWorkerTrend = AttractivenessTotalCalc() / 100.0 * 20;
            var population = GetPopulation();
            var serviceWorkerTrend = Industries.Service.NeedCalculation(population) * 10;
            var workersTrend = miningWorkerTrend + productWorkerTrend + serviceWorkerTrend;

            return workersTrend / population * 100.0;
        }
    }
}