using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                    ColonyStatNames.ActionPoints_Resourses => Resources.ActionPoints.Value,
                    ColonyStatNames.ActionPoints_Trend => ActionPointsTrend,
                    ColonyStatNames.Economic_Reserves => Resources.Solars,
                    ColonyStatNames.Mood_Total => MoodTotal.Value,
                    ColonyStatNames.Mood_Total_Balance => MoodTotalBalanceCacl(),
                    ColonyStatNames.Population_Total => GetPopulation(),
                    ColonyStatNames.AreaCapacity_Occupied => GetZonesOccupied(),
                    ColonyStatNames.Economic_Budget_Balance => GetSolarsIncome(),
                    ColonyStatNames.AreaCapacity_Total => Resources.ZonesTotal,
                    ColonyStatNames.AreaCapacity_Available => GetZonesAvailable(),
                    ColonyStatNames.Laws_TaxLevel => Settings.TaxLevel,
                    ColonyStatNames.Laws_SocialGuaranteesLevel => Settings.SocialGuaranteesLevel,
                    ColonyStatNames.Attractiveness_Total => AttractivenessTotalCalc(),
                    ColonyStatNames.FirstWedding => FirstWedding ? 1 : 0,
                    ColonyStatNames.CurrentWeek => CurrentWeek,
                    ColonyStatNames.Industry_Service_Need => Industries.Service.NeedCalculation(GetPopulation()),

                    ColonyStatNames.Industry_Administrative_Companies_StateOwned => Industries.Administrative.StateOwnedBuildingCount,
                    ColonyStatNames.Industry_Administrative_Companies_Private => Industries.Administrative.PrivateBuildingCount,
                    ColonyStatNames.Industry_Minning_Available => Industries.Minning.UnitAvailable,
                    ColonyStatNames.Industry_Minning_Companies_StateOwned => Industries.Minning.StateOwnedBuildingCount,
                    ColonyStatNames.Industry_Minning_Companies_Private => Industries.Minning.PrivateBuildingCount,
                    ColonyStatNames.Industry_Production_Companies_StateOwned => Industries.Production.StateOwnedBuildingCount,
                    ColonyStatNames.Industry_Production_Companies_Private => Industries.Production.PrivateBuildingCount,
                    ColonyStatNames.Industry_Service_Companies_StateOwned => Industries.Service.StateOwnedBuildingCount,
                    ColonyStatNames.Industry_Service_Companies_Private => Industries.Service.PrivateBuildingCount,
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
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.ActionPoints_Resourses)?.Value ?? 0;
            if (Resources.ActionPoints.Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves)?.Value ?? 0;
            if (Resources.Solars < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = GetZonesAvailable();
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.AreaCapacity_Occupied)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            Resources.AddActionPoints((int)actionPoints);
            Resources.AddSolars(solarResservesParameter);
            MoodTotal += decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total)?.Value ?? 0;
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            foreach (var paramter in colonyParameters)
            {
                Action action = paramter.Name switch
                {
                    ColonyStatNames.ActionPoints_Resourses => () => Resources.AddActionPoints((int)paramter.Value),
                    ColonyStatNames.ActionPoints_Trend => () => ActionPointsTrend += (int)paramter.Value,
                    ColonyStatNames.Economic_Reserves => () => Resources.AddSolars((int)paramter.Value),

                    ColonyStatNames.Industry_Administrative_Companies_StateOwned => () => Industries.Administrative.AddStateOwnedBuilding((int)paramter.Value),
                    ColonyStatNames.Industry_Administrative_Companies_Private => () => Industries.Administrative.AddPrivateBuilding((int)paramter.Value),

                    ColonyStatNames.Industry_Minning_Companies_StateOwned => () => Industries.Minning.AddStateOwnedBuilding((int)paramter.Value),
                    ColonyStatNames.Industry_Minning_Companies_Private => () => Industries.Minning.AddPrivateBuilding((int)paramter.Value),

                    ColonyStatNames.Industry_Production_Companies_StateOwned => () => Industries.Production.AddStateOwnedBuilding((int)paramter.Value),
                    ColonyStatNames.Industry_Production_Companies_Private => () => Industries.Production.AddPrivateBuilding((int)paramter.Value),

                    ColonyStatNames.Industry_Service_Companies_StateOwned => () => Industries.Service.AddStateOwnedBuilding((int)paramter.Value),
                    ColonyStatNames.Industry_Service_Companies_Private => () => Industries.Service.AddPrivateBuilding((int)paramter.Value),

                    ColonyStatNames.Mood_Total => () => MoodTotal += paramter.Value,
                    ColonyStatNames.FirstWedding => () => FirstWedding = true,
                    ColonyStatNames.Laws_TaxLevel => () => Settings.SetTaxLevel((int)paramter.Value),
                    ColonyStatNames.Laws_SocialGuaranteesLevel => () => Settings.SetSocialGuaranteesLevel((int)paramter.Value),
                    ColonyStatNames.CurrentWeek => () => CurrentWeek += (int)paramter.Value,
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