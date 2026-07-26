using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Buildings;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyState
    {
        public Dictionary<ColonyResourceType, ColonyResource> Resources { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<ColonyReformType, ColonyReform> Reforms { get; }
        public Dictionary<ColonyBuildingType, ColonyBuilding> Buildings { get; }
        public Dictionary<ColonyProgressType, bool> Progress { get; }

        public ColonyState(
            IEnumerable<ColonyResource> resources,
            IEnumerable<ColonySlot> slots,
            IEnumerable<ColonyReform> reforms,
            IEnumerable<ColonyBuilding> buildings,
            Dictionary<ColonyProgressType, bool> progress)
        {
            Resources = resources.ToDictionary(x => x.Type);
            Slots = slots.ToDictionary(x => x.Type);
            Reforms = reforms.ToDictionary(x => x.Type);
            Buildings = buildings.ToDictionary(x => x.Type);
            Progress = progress;
        }

        public static ColonyState CreateNew()
        {
            var resouces = ColonyResource.CreateNew();
            var slots = ColonySlot.CreateNew();
            var reforms = ColonyReform.CreateNew();
            var buildings = ColonyBuilding.CreateNew();
            var progress = CreateNewProgress();
            return new ColonyState(resouces, slots, reforms, buildings, progress);
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
            foreach (var building in Buildings.Values)
            {
                var buildingSettings = building.GetSettings();
                var buildingCount = building.Total; 
                result += buildingCount * buildingSettings.Population;
            }
            return result;
        }

        public double GetAttractiveness()
        {
            var defaultValue = 100;
            var taxEffect = -15 * Reforms[ColonyReformType.TaxLevel].Value;
            var standartsEffect = -15 * Reforms[ColonyReformType.SocialGuaranteesLevel].Value;
            var turns = Resources[ColonyResourceType.Turns].Value;
            var stabilityEffect = Math.Min(50, turns / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double GetGdp()
        {
            var socialGuaranteesCoef = 1 + ((Reforms[ColonyReformType.SocialGuaranteesLevel].Value - 3) / 10.0);
            return GetPopulation() * socialGuaranteesCoef * 10.0;
        }

        public double GetGdpDelta()
        {
            var miningWorkerTrend = Slots[ColonySlotType.Mining].GetFree(this) > 0 ? 20 : 0;
            var productWorkerTrend = GetAttractiveness() / 100.0 * 20;
            var population = GetPopulation();
            var serviceWorkerTrend = GetServiceNeed() * 10;
            var workersTrend = miningWorkerTrend + productWorkerTrend + serviceWorkerTrend;

            return workersTrend / population * 100.0;
        }

        internal double GetServiceNeed()
        {
            var buildingCount = Buildings[ColonyBuildingType.Service].Total;
            var population = GetPopulation();
            return (population / 50.0) - buildingCount - 1.5;
        }
    }
}