using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyState
    {
        public Station Station { get; }
        public ColonyResources Resources { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<ColonyReformType, ColonyReform> Reforms { get; }
        public Dictionary<ColonyIndustryType, ColonyIndustry> Industries { get; }
        public Dictionary<ColonyProgressType, bool> Progress { get; }

        public ColonyState(
            Station station,
            ColonyResources resources,
            IEnumerable<ColonySlot> slots,
            IEnumerable<ColonyReform> reforms,
            IEnumerable<ColonyIndustry> industries,
            Dictionary<ColonyProgressType, bool> progress)
        {
            Station = station;
            Resources = resources;
            Slots = slots.ToDictionary(x => x.Type);
            Reforms = reforms.ToDictionary(x => x.Type);
            Industries = industries.ToDictionary(x => x.Type);
            Progress = progress;
        }

        public static ColonyState CreateNew(Guid colonyId)
        {
            var station = Station.CreateNew(
                colonyId,
                StationModelId.Dawn_342);
            var resouces = ColonyResources.CreateNew();
            var slots = ColonySlot.CreateNew();
            var reforms = ColonyReform.CreateNew();
            var industries = ColonyIndustry.CreateNew();
            var progress = CreateNewProgress();
            return new ColonyState(
                station,
                resouces,
                slots,
                reforms,
                industries,
                progress);
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
            var turns = Resources.Turns.Value;
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
            return population / 50.0 - buildingCount - 1.5;
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

                result += privateBuildingCount * solarDeltaPrivate + stateOwnedBuildingCount * solarDeltaState;
            }

            var publicDebt = GetPublicDebt();
            return result + publicDebt.SolarDelta;
        }

        public double GetMoodDelta()
        {
            var socialGuaranteesCoef = 1 - (Reforms[ColonyReformType.SocialGuaranteesLevel].Value - 3) / 4.0;
            return -GetPopulation() * 0.005 * socialGuaranteesCoef;
        }

        public PublicDebt GetPublicDebt()
        {
            var yagoLevel = GetYagoLevel();
            var publicDebtContext = new PublicDebtContext(yagoLevel);
            return new PublicDebt(Reforms[ColonyReformType.PublicDebt].Value, publicDebtContext);
        }

        public Reform GetReform(long reformId)
        {
            var reformDataset = ReformDataset.Get().ToList();
            var reform = reformDataset.Find(x => x.Id == reformId)
                ?? throw new YagoNotFoundException(nameof(Reform), reformId.ToString());
            return reform;
        }

        public void SetReform(Reform reform) => reform.SetReform(this);
    }
}