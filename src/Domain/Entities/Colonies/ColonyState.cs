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
        public Dictionary<StateKey, IState> States { get; }

        private readonly IndustryType[] IndustryTypes =
        [
            IndustryType.Administrative,
            IndustryType.Mining,
            IndustryType.Service,
            IndustryType.Production
        ];

        public ColonyState(
            IEnumerable<IState> states)
        {
            States = states.ToDictionary(x => x.Key);
        }

        public static ColonyState CreateNew()
        {
            var states = new List<IState>()
            {
                new MutableState(StateKey.SolarsCurrent, 0),

                new MutableState(StateKey.ReformPointsCurrent, 1, minValue: 0, maxValue: 10),
                new MutableState(StateKey.ReformPointsDelta, 1),

                new MutableState(StateKey.MoodReserve, 50, minValue: 0, maxValue: 100),

                new MutableState(StateKey.TurnsCurrent, 1),

                new MutableState(StateKey.ModulesTotal, 140),

                new MutableState(StateKey.ReformsTaxLevel, 3),
                new MutableState(StateKey.ReformsSocialGuaranteesLevel, 3),

                new MutableState(StateKey.BuildingsAdministrativePrivate, 0),
                new MutableState(StateKey.BuildingsAdministrativeState, 0),

                new MutableState(StateKey.BuildingsMiningPrivate, 0),
                new MutableState(StateKey.BuildingsMiningState, 0),

                new MutableState(StateKey.BuildingsServicePrivate, 0),
                new MutableState(StateKey.BuildingsServiceState, 0),

                new MutableState(StateKey.BuildingsProductionPrivate, 0),
                new MutableState(StateKey.BuildingsProductionState, 0),

                new MutableState(StateKey.FlagsFirstWedding, 0)
            };
            return new ColonyState(states);
        }

        public double GetGameParameter(StateKey stateKey)
        {
            return States.ContainsKey(stateKey)
                ? States[stateKey].GetValue(this)
                : stateKey switch
                {
                    StateKey.MoodDelta => MoodTotalBalanceCacl(),
                    StateKey.Population => GetPopulation(),
                    StateKey.ModulesUsed => GetZonesOccupied(),
                    StateKey.SolarsDelta => GetSolarsIncome(),
                    StateKey.ModulesFree => GetZonesAvailable(),
                    StateKey.Attractiveness => AttractivenessTotalCalc(),
                    StateKey.ServiceNeed => ServiceNeedCalculation(GetPopulation()),
                    StateKey.MiningSlotsFree => GetMiningUnitAvailable(),
                    _ => throw new YagoUnknownTypeException(stateKey.ToString())
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
            return (int)GetGameParameter(StateKey.ModulesTotal) - GetZonesOccupied();
        }

        public void IssueDecree(Decree decree)
        {
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ReformPointsCurrent)?.Value ?? 0;
            if (States[StateKey.ReformPointsCurrent].IsLessThan(-actionPoints, this))
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.SolarsCurrent)?.Value ?? 0;
            if (States[StateKey.SolarsCurrent].IsLessThan(-solarResservesParameter, this))
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = GetZonesAvailable();
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ModulesUsed)?.Value ?? 0))
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
            var taxEffect = -15 * GetGameParameter(StateKey.ReformsTaxLevel);
            var standartsEffect = -15 * GetGameParameter(StateKey.ReformsSocialGuaranteesLevel);
            var turns = GetGameParameter(StateKey.TurnsCurrent);
            var stabilityEffect = Math.Min(50, turns / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double MoodTotalBalanceCacl()
        {
            var socialGuaranteesCoef = 1 + ((GetGameParameter(StateKey.ReformsSocialGuaranteesLevel) - 3) / 10.0);
            return -GetPopulation() * 0.01 * socialGuaranteesCoef;
        }

        public double GdpCalc()
        {
            var socialGuaranteesCoef = 1 + ((GetGameParameter(StateKey.ReformsSocialGuaranteesLevel) - 3) / 10.0);
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
                    ? (int)GetGameParameter(StateKey.BuildingsAdministrativePrivate)
                    : (int)GetGameParameter(StateKey.BuildingsAdministrativeState),
                IndustryType.Mining => isPrivate
                    ? (int)GetGameParameter(StateKey.BuildingsMiningPrivate)
                    : (int)GetGameParameter(StateKey.BuildingsMiningState),
                IndustryType.Service => isPrivate
                    ? (int)GetGameParameter(StateKey.BuildingsServicePrivate)
                    : (int)GetGameParameter(StateKey.BuildingsServiceState),
                IndustryType.Production => isPrivate
                    ? (int)GetGameParameter(StateKey.BuildingsProductionPrivate)
                    : (int)GetGameParameter(StateKey.BuildingsProductionState),
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

        public static IReadOnlyList<StateKey> MainParameters =>
        [
            StateKey.SolarsCurrent,
            StateKey.SolarsDelta,
            StateKey.MoodReserve,
            StateKey.ModulesUsed,
            StateKey.Population
        ];
    }
}