using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.ValueTypes.States;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyState
    {
        private readonly IndustryType[] IndustryTypes =
        [
            IndustryType.Administrative,
            IndustryType.Mining,
            IndustryType.Service,
            IndustryType.Production
        ];

        public Dictionary<string, IState> States { get; }

        public ColonyState(
            Dictionary<string, IState> states)
        {
            States = states;
        }

        public static ColonyState CreateNew()
        {
            var states = new Dictionary<string, IState>()
            {
                { StateKeys.Solars.Reserve, new MutableState(StateKeys.Solars.Reserve, 0) },
                { StateKeys.ReformPoints.Income, new MutableState(StateKeys.ReformPoints.Income, 1) },
                { StateKeys.Mood.Reserve, new MutableState(StateKeys.Mood.Reserve, 50, minValue: 0, maxValue: 100) },
                { StateKeys.Counters.Turns, new MutableState(StateKeys.Counters.Turns, 1) },
                { StateKeys.Flags.Events.FirstWedding, new MutableState(StateKeys.Flags.Events.FirstWedding, 0) },
                { StateKeys.ReformPoints.Reserve, new MutableState(StateKeys.ReformPoints.Reserve, 1, minValue: 0, maxValue: 10) },
                { StateKeys.Modules.Total, new MutableState(StateKeys.Modules.Total, 140) },
                { StateKeys.Reforms.TaxLevel, new MutableState(StateKeys.Reforms.TaxLevel, 3) },
                { StateKeys.Reforms.SocialGuaranteesLevel, new MutableState(StateKeys.Reforms.SocialGuaranteesLevel, 3) },
                { StateKeys.Industries.Administrative.Buildings.Private, new MutableState(StateKeys.Industries.Administrative.Buildings.Private, 0) },
                { StateKeys.Industries.Administrative.Buildings.State, new MutableState(StateKeys.Industries.Administrative.Buildings.State, 0) },
                { StateKeys.Industries.Mining.Buildings.Private, new MutableState(StateKeys.Industries.Mining.Buildings.Private, 0) },
                { StateKeys.Industries.Mining.Buildings.State, new MutableState(StateKeys.Industries.Mining.Buildings.State, 0) },
                { StateKeys.Industries.Service.Buildings.Private, new MutableState(StateKeys.Industries.Service.Buildings.Private, 0) },
                { StateKeys.Industries.Service.Buildings.State, new MutableState(StateKeys.Industries.Service.Buildings.State, 0) },
                { StateKeys.Industries.Production.Buildings.Private, new MutableState(StateKeys.Industries.Production.Buildings.Private, 0) },
                { StateKeys.Industries.Production.Buildings.State, new MutableState(StateKeys.Industries.Production.Buildings.State, 0) },
            };
            return new ColonyState(states);
        }

        public double GetGameParameter(string parameterName)
        {
            return States.ContainsKey(parameterName)
                ? States[parameterName].GetValue(this)
                : parameterName switch
                {
                    StateKeys.Mood.Income => MoodTotalBalanceCacl(),
                    StateKeys.Population => GetPopulation(),
                    StateKeys.Modules.Used => GetZonesOccupied(),
                    StateKeys.Solars.Income => GetSolarsIncome(),
                    StateKeys.Modules.Free => GetZonesAvailable(),
                    StateKeys.Industries.Attractiveness => AttractivenessTotalCalc(),
                    StateKeys.Industries.Service.Buildings.Need => ServiceNeedCalculation(GetPopulation()),
                    StateKeys.Industries.Mining.Buildings.Available => GetMiningUnitAvailable(),
                    _ => throw new YagoUnknownTypeException(parameterName)
                };
        }

        public int GetPopulation()
        {
            var result = 0;
            foreach (var industryType in IndustryTypes)
            {
                var building = BuildingDataset.GetByType(industryType);
                var privateBuildingCount = GetBuildCount(industryType, isPrivate: true);
                var stateOwnedBuildingCount = GetBuildCount(industryType, isPrivate: false);
                var buildingCount = privateBuildingCount + stateOwnedBuildingCount;
                result += buildingCount * building.Population;
            }
            return result;
        }

        public int GetZonesOccupied()
        {
            var result = 0;
            foreach (var industryType in IndustryTypes)
            {
                var building = BuildingDataset.GetByType(industryType);
                var privateBuildingCount = GetBuildCount(industryType, isPrivate: true);
                var stateOwnedBuildingCount = GetBuildCount(industryType, isPrivate: false);
                var buildingCount = privateBuildingCount + stateOwnedBuildingCount;
                result += buildingCount * building.ZonesOccupied;
            }
            return result;
        }

        public double GetSolarsIncome()
        {
            var result = 0.0;

            foreach (var industryType in IndustryTypes)
            {
                var privateBuildingCount = GetBuildCount(industryType, isPrivate: true);
                var stateOwnedBuildingCount = GetBuildCount(industryType, isPrivate: false);
                var building = BuildingDataset.GetByType(industryType);
                result += (privateBuildingCount + (3 * stateOwnedBuildingCount)) * building.SolarsIncome;
            }
            return result;
        }

        public int GetZonesAvailable()
        {
            return (int)GetGameParameter(StateKeys.Modules.Total) - GetZonesOccupied();
        }

        public void IssueDecree(Decree decree)
        {
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.ReformPoints.Reserve)?.Value ?? 0;
            if (States[StateKeys.ReformPoints.Reserve].IsLessThan(-actionPoints))
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.Solars.Reserve)?.Value ?? 0;
            if (States[StateKeys.Solars.Reserve].IsLessThan(-solarResservesParameter))
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = GetZonesAvailable();
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKeys.Modules.Used)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");


            foreach (var parameter in decree.Parameters)
            {
                (States[parameter.Name] as IMutableState)?.Add(solarResservesParameter);
            }
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            foreach (var paramter in colonyParameters)
            {
                if (States.ContainsKey(paramter.Name))
                {
                    if (States[paramter.Name] is not IMutableState state)
                        throw new YagoException($"Параметр {paramter.Name} не доступен для изменения.");
                    state.Add(paramter.Value);
                    continue;
                }
            }
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -15 * GetGameParameter(StateKeys.Reforms.TaxLevel);
            var standartsEffect = -15 * GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel);
            var turns = GetGameParameter(StateKeys.Counters.Turns);
            var stabilityEffect = Math.Min(50, turns / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double MoodTotalBalanceCacl()
        {
            var socialGuaranteesCoef = 1 + ((GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel) - 3) / 10.0);
            return -GetPopulation() * 0.01 * socialGuaranteesCoef;
        }

        public double GdpCalc()
        {
            var socialGuaranteesCoef = 1 + ((GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel) - 3) / 10.0);
            return GetPopulation() * socialGuaranteesCoef * 10.0;
        }

        public double GdpTrendCalc()
        {
            var miningWorkerTrend = GetMiningUnitAvailable() > 0 ? 20 : 0;
            var productWorkerTrend = AttractivenessTotalCalc() / 100.0 * 20;
            var population = GetPopulation();
            var serviceWorkerTrend = ServiceNeedCalculation(population) * 10;
            var workersTrend = miningWorkerTrend + productWorkerTrend + serviceWorkerTrend;

            return workersTrend / population * 100.0;
        }

        private int GetBuildCount(IndustryType industryType, bool isPrivate)
        {
            return industryType switch
            {
                IndustryType.Administrative => isPrivate
                    ? (int)GetGameParameter(StateKeys.Industries.Administrative.Buildings.Private)
                    : (int)GetGameParameter(StateKeys.Industries.Administrative.Buildings.State),
                IndustryType.Mining => isPrivate
                    ? (int)GetGameParameter(StateKeys.Industries.Mining.Buildings.Private)
                    : (int)GetGameParameter(StateKeys.Industries.Mining.Buildings.State),
                IndustryType.Service => isPrivate
                    ? (int)GetGameParameter(StateKeys.Industries.Service.Buildings.Private)
                    : (int)GetGameParameter(StateKeys.Industries.Service.Buildings.State),
                IndustryType.Production => isPrivate
                    ? (int)GetGameParameter(StateKeys.Industries.Production.Buildings.Private)
                    : (int)GetGameParameter(StateKeys.Industries.Production.Buildings.State),
                _ => 0
            };
        }

        internal double ServiceNeedCalculation(int populationTotal)
        {
            var privateBuildingCount = GetBuildCount(IndustryType.Service, isPrivate: true);
            var stateOwnedBuildingCount = GetBuildCount(IndustryType.Service, isPrivate: false);
            var buildingCount = privateBuildingCount + stateOwnedBuildingCount;
            return (populationTotal / 50.0) - buildingCount - 1.5;
        }

        private int GetMiningUnitAvailable()
        {
            var privateBuildingCount = GetBuildCount(IndustryType.Mining, isPrivate: true);
            var stateOwnedBuildingCount = GetBuildCount(IndustryType.Mining, isPrivate: false);
            var buildingCount = privateBuildingCount + stateOwnedBuildingCount;
            return 12 - buildingCount;
        }
    }
}