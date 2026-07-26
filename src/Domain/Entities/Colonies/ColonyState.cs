using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.ValueTypes.States;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyState
    {
        private Dictionary<StateKey, double> _states { get; }
        public Dictionary<ColonyResourceType, ColonyResource> Resources { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<IndustryType, ColonyIndustry> Industries { get; }

        public static readonly IndustryType[] IndustryTypes =
        [
            IndustryType.Administrative,
            IndustryType.Mining,
            IndustryType.Service,
            IndustryType.Production
        ];

        public ColonyState(
            IEnumerable<ColonyResource> resources,
            IEnumerable<ColonySlot> slots,
            IEnumerable<ColonyIndustry> industries,
            IEnumerable<IState> states)
        {
            Resources = resources.ToDictionary(x => x.Type);
            Slots = slots.ToDictionary(x => x.Type);
            Industries = industries.ToDictionary(x => x.Type);
            _states = states.ToDictionary(x => x.Key, x => x.GetValue(this));
        }

        public static ColonyState CreateNew()
        {
            var resouces = new List<ColonyResource>
            {
                new ColonySolars(value: 0),
                new ColonyReformPoints(value: 1),
                new ColonyMood(value: 50),
                new ColonyTurns(value: 1),
            };
            var slots = new List<ColonySlot>
            {
                new ColonyModules(total: 140),
                new ColonyMiningSlots(total: 12),
            };
            var industrines = new List<ColonyIndustry>
            {
                new(IndustryType.Administrative, privateCount: 0, stateCount: 0),
                new(IndustryType.Mining, privateCount: 0, stateCount: 0),
                new(IndustryType.Production, privateCount: 0, stateCount: 0),
                new(IndustryType.Service, privateCount: 0, stateCount: 0),
            };
            var states = new List<IState>()
            {
                new MutableState(StateKey.ReformsTaxLevel, 3),
                new MutableState(StateKey.ReformsSocialGuaranteesLevel, 3),

                new MutableState(StateKey.FlagsFirstWedding, 0)
            };
            return new ColonyState(resouces, slots, industrines, states);
        }

        public double GetGameParameter(StateKey stateKey)
        {
            return _states.ContainsKey(stateKey)
                ? _states[stateKey]
                : stateKey switch
                {
                    StateKey.SolarsCurrent => Resources[ColonyResourceType.Solars].Value,
                    StateKey.SolarsDelta => Resources[ColonyResourceType.Solars].GetDeltaPerTurn(this),

                    StateKey.ReformPointsCurrent => Resources[ColonyResourceType.ReformPoints].Value,
                    StateKey.ReformPointsDelta => Resources[ColonyResourceType.ReformPoints].GetDeltaPerTurn(this),

                    StateKey.MoodCurrent => Resources[ColonyResourceType.Mood].Value,
                    StateKey.MoodDelta => Resources[ColonyResourceType.Mood].GetDeltaPerTurn(this),

                    StateKey.TurnsCurrent => Resources[ColonyResourceType.Turns].Value,
                    StateKey.TurnsDelta => Resources[ColonyResourceType.Turns].GetDeltaPerTurn(this),

                    StateKey.ModulesTotal => Slots[ColonySlotType.Modules].Total,
                    StateKey.ModulesUsed => Slots[ColonySlotType.Modules].GetUsed(this),
                    StateKey.ModulesFree => Slots[ColonySlotType.Modules].GetFree(this),

                    StateKey.MiningSlotsTotal => Slots[ColonySlotType.Mining].Total,
                    StateKey.MiningSlotsUsed => Slots[ColonySlotType.Mining].GetUsed(this),
                    StateKey.MiningSlotsFree => Slots[ColonySlotType.Mining].GetFree(this),

                    StateKey.BuildingsAdministrativePrivate => Industries[IndustryType.Administrative].PrivateCount,
                    StateKey.BuildingsAdministrativeState => Industries[IndustryType.Administrative].StateCount,
                    StateKey.BuildingsAdministrativeTotal => Industries[IndustryType.Administrative].Total,

                    StateKey.BuildingsMiningPrivate => Industries[IndustryType.Mining].PrivateCount,
                    StateKey.BuildingsMiningState => Industries[IndustryType.Mining].StateCount,
                    StateKey.BuildingsMiningTotal => Industries[IndustryType.Mining].Total,

                    StateKey.BuildingsProductionPrivate => Industries[IndustryType.Production].PrivateCount,
                    StateKey.BuildingsProductionState => Industries[IndustryType.Production].StateCount,
                    StateKey.BuildingsProductionTotal => Industries[IndustryType.Production].Total,

                    StateKey.BuildingsServicePrivate => Industries[IndustryType.Service].PrivateCount,
                    StateKey.BuildingsServiceState => Industries[IndustryType.Service].StateCount,
                    StateKey.BuildingsServiceTotal => Industries[IndustryType.Service].Total,

                    StateKey.Population => GetPopulation(),
                    StateKey.Attractiveness => AttractivenessTotalCalc(),
                    StateKey.ServiceNeed => ServiceNeedCalculation(GetPopulation()),
                    _ => throw new YagoUnknownTypeException(stateKey.ToString())
                };
        }

        private void AddParameter(StateKey stateKey, double delta)
        {
            if (_states.ContainsKey(stateKey))
            {
                _states[stateKey] += delta;
            }
            else
            {
                switch (stateKey)
                {
                    case StateKey.SolarsCurrent:
                        Resources[ColonyResourceType.Solars].Add(delta);
                        break;
                    case StateKey.ReformPointsCurrent:
                        Resources[ColonyResourceType.ReformPoints].Add(delta);
                        break;
                    case StateKey.MoodCurrent:
                        Resources[ColonyResourceType.Mood].Add(delta);
                        break;
                    case StateKey.TurnsCurrent:
                        Resources[ColonyResourceType.Turns].Add(delta);
                        break;


                    case StateKey.ModulesTotal:
                        Slots[ColonySlotType.Modules].AddTotal((int)delta);
                        break;
                    case StateKey.MiningSlotsTotal:
                        Slots[ColonySlotType.Mining].AddTotal((int)delta);
                        break;

                    case StateKey.BuildingsAdministrativePrivate:
                        Industries[IndustryType.Administrative].AddPrivate((int)delta);
                        break;
                    case StateKey.BuildingsAdministrativeState:
                        Industries[IndustryType.Administrative].AddState((int)delta);
                        break;
                    case StateKey.BuildingsAdministrativeTotal:
                        throw new YagoException($"Параметр {stateKey} недоступен для изменения.");

                    case StateKey.BuildingsMiningPrivate:
                        Industries[IndustryType.Mining].AddPrivate((int)delta);
                        break;
                    case StateKey.BuildingsMiningState:
                        Industries[IndustryType.Mining].AddState((int)delta);
                        break;
                    case StateKey.BuildingsMiningTotal:
                        throw new YagoException($"Параметр {stateKey} недоступен для изменения.");

                    case StateKey.BuildingsProductionPrivate:
                        Industries[IndustryType.Production].AddPrivate((int)delta);
                        break;
                    case StateKey.BuildingsProductionState:
                        Industries[IndustryType.Production].AddState((int)delta);
                        break;
                    case StateKey.BuildingsProductionTotal:
                        throw new YagoException($"Параметр {stateKey} недоступен для изменения.");

                    case StateKey.BuildingsServicePrivate:
                        Industries[IndustryType.Service].AddPrivate((int)delta);
                        break;
                    case StateKey.BuildingsServiceState:
                        Industries[IndustryType.Service].AddState((int)delta);
                        break;
                    case StateKey.BuildingsServiceTotal:
                        throw new YagoException($"Параметр {stateKey} недоступен для изменения.");
                }
            }
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

        public void IssueDecree(Decree decree)
        {
            var actionPoints = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ReformPointsCurrent)?.Value ?? 0;
            if (Resources[ColonyResourceType.ReformPoints].Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == StateKey.SolarsCurrent)?.Value ?? 0;
            if (_states[StateKey.SolarsCurrent] < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = Slots[ColonySlotType.Modules].GetFree(this);
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ModulesUsed)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            foreach (var parameter in decree.Parameters)
            {
                AddParameter(parameter.Name, parameter.Value);
            }
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            foreach (var parameter in colonyParameters)
            {
                AddParameter(parameter.Name, parameter.Value);
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

        public double GdpCalc()
        {
            var socialGuaranteesCoef = 1 + ((GetGameParameter(StateKey.ReformsSocialGuaranteesLevel) - 3) / 10.0);
            return GetPopulation() * socialGuaranteesCoef * 10.0;
        }

        public double GdpTrendCalc()
        {
            var miningWorkerTrend = Slots[ColonySlotType.Mining].GetFree(this) > 0 ? 20 : 0;
            var productWorkerTrend = AttractivenessTotalCalc() / 100.0 * 20;
            var population = GetPopulation();
            var serviceWorkerTrend = ServiceNeedCalculation(population) * 10;
            var workersTrend = miningWorkerTrend + productWorkerTrend + serviceWorkerTrend;

            return workersTrend / population * 100.0;
        }

        public int GetBuildCount(IndustryType industryType, bool isPrivate)
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

        public static IReadOnlyList<StateKey> MainParameters =>
        [
            StateKey.SolarsCurrent,
            StateKey.SolarsDelta,
            StateKey.MoodCurrent,
            StateKey.ModulesUsed,
            StateKey.Population
        ];
    }
}