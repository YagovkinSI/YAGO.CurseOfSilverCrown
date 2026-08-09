using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Mappings;
using YAGO.World.Domain.Reforms;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyState
    {
        public Dictionary<ColonyResourceType, ColonyResource> Resources { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<ColonyReformType, ColonyReform> Reforms { get; }
        public Dictionary<ColonyIndustryType, ColonyIndustry> Industries { get; }
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
            var industries = ColonyIndustry.CreateNew();
            var progress = CreateNewProgress();
            return new ColonyState(resouces, slots, reforms, industries, progress);
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
            var buildingContext = this.GetBuildingContext();
            foreach (var industry in Industries.Values)
            {
                for (var i = 0; i < 2; i++)
                {
                    var isPrivate = i == 1;
                    var building = industry.GetBuilding(isPrivate, buildingContext);
                    var buildingCount = isPrivate ? industry.PrivateCount : industry.StateCount;
                    result += buildingCount * building.Population;
                }
            }
            return result;
        }

        public double GetStability()
        {
            var turns = Resources[ColonyResourceType.Turns].Value;
            var stabilityEffect = Math.Min(50, turns / 3.0);
            return Math.Clamp(stabilityEffect, -100, 100);
        }

        public double GetAttractiveness()
        {
            var taxEffect = 15 * (3 - Reforms[ColonyReformType.TaxLevel].Value);
            var standartsEffect = 5 * (Reforms[ColonyReformType.SocialGuaranteesLevel].Value - 3);
            var stabilityEffect = GetStability();
            return Math.Clamp(taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double GetGdp()
        {
            var result = 0.0;
            var buildingContext = this.GetBuildingContext();
            foreach (var industry in Industries.Values)
            {
                for (var i = 0; i < 2; i++)
                {
                    var isPrivate = i == 1;
                    var building = industry.GetBuilding(isPrivate, buildingContext);
                    var buildingCount = isPrivate ? industry.PrivateCount : industry.StateCount;
                    result += buildingCount * building.Gdp;
                }
            }
            return result;
        }

        internal double GetServiceNeed()
        {
            var buildingCount = Industries[ColonyIndustryType.Service].Total;
            var population = GetPopulation();
            return (population / 50.0) - buildingCount - 1.5;
        }

        public YagoLevel GetYagoLevel() => YagoLevel.Gray;

        public double GetSolarDelta()
        {
            var result = 0.0;

            var buildingContext = this.GetBuildingContext();
            foreach (var industry in Industries.Values)
            {
                var buildingPrivate = industry.GetBuilding(isPrivate: true, buildingContext);
                var privateBuildingCount = industry.PrivateCount;
                var solarDeltaPrivate = buildingPrivate.SolarsDelta;

                var buildingState = industry.GetBuilding(isPrivate: false, buildingContext);
                var stateOwnedBuildingCount = industry.StateCount;
                var solarDeltaState = buildingState.SolarsDelta;

                result += (privateBuildingCount * solarDeltaPrivate) + (stateOwnedBuildingCount * solarDeltaState);
            }

            var publicDeptContext = this.ToPublicDebtContext();
            var publicDept = new PublicDebt(Reforms[ColonyReformType.PublicDebt].Value, publicDeptContext);

            return result + publicDept.SolarDelta;
        }

        public double GetMoodDelta()
        {
            var socialGuaranteesCoef = 1 - ((Reforms[ColonyReformType.SocialGuaranteesLevel].Value - 3) / 4.0);
            return -GetPopulation() * 0.005 * socialGuaranteesCoef;
        }
    }
}