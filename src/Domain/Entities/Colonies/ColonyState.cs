using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Buildings;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyState
    {
        public Dictionary<ColonyResourceType, ColonyResource> Resources { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<ColonyReformType, ColonyReform> Reforms { get; }
        public Dictionary<IndustryType, ColonyIndustry> Industries { get; }
        public Dictionary<ColonyProgressType, bool> Progress { get; }

        public ColonyState(
            IEnumerable<ColonyResource> resources,
            IEnumerable<ColonySlot> slots,
            IEnumerable<ColonyReform> reforms,
            IEnumerable<ColonyIndustry> industries,
            Dictionary<ColonyProgressType, bool> progress)
        {
            Resources = resources.ToDictionary(x => x.Type);
            Slots = slots.ToDictionary(x => x.Type);
            Reforms = reforms.ToDictionary(x => x.Type);
            Industries = industries.ToDictionary(x => x.Type);
            Progress = progress;
        }

        public static ColonyState CreateNew()
        {
            var resouces = ColonyResource.CreateNew();
            var slots = ColonySlot.CreateNew();
            var reforms = ColonyReform.CreateNew();
            var industrines = ColonyIndustry.CreateNew();
            var progress = CreateNewProgress();
            return new ColonyState(resouces, slots, reforms, industrines, progress);
        }

        private static Dictionary<ColonyProgressType, bool> CreateNewProgress()
        {
            return new Dictionary<ColonyProgressType, bool>()
            {
                { ColonyProgressType.FirstWedding, false },
            };
        }

        public int GetPopulation()
        {
            var result = 0;
            foreach (var industryType in Enum.GetValues<IndustryType>())
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
            if (Resources[ColonyResourceType.Solars].Value < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = Slots[ColonySlotType.Modules].GetFree(this);
            if (zonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == StateKey.ModulesUsed)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            foreach (var parameter in decree.Parameters)
            {
                this.AddParameter(parameter.Name, parameter.Value);
            }
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            foreach (var parameter in colonyParameters)
            {
                this.AddParameter(parameter.Name, parameter.Value);
            }
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -15 * this.GetValue(StateKey.ReformsTaxLevel);
            var standartsEffect = -15 * this.GetValue(StateKey.ReformsSocialGuaranteesLevel);
            var turns = this.GetValue(StateKey.TurnsCurrent);
            var stabilityEffect = Math.Min(50, turns / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double GdpCalc()
        {
            var socialGuaranteesCoef = 1 + ((this.GetValue(StateKey.ReformsSocialGuaranteesLevel) - 3) / 10.0);
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
                    ? (int)this.GetValue(StateKey.BuildingsAdministrativePrivate)
                    : (int)this.GetValue(StateKey.BuildingsAdministrativeState),
                IndustryType.Mining => isPrivate
                    ? (int)this.GetValue(StateKey.BuildingsMiningPrivate)
                    : (int)this.GetValue(StateKey.BuildingsMiningState),
                IndustryType.Service => isPrivate
                    ? (int)this.GetValue(StateKey.BuildingsServicePrivate)
                    : (int)this.GetValue(StateKey.BuildingsServiceState),
                IndustryType.Production => isPrivate
                    ? (int)this.GetValue(StateKey.BuildingsProductionPrivate)
                    : (int)this.GetValue(StateKey.BuildingsProductionState),
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