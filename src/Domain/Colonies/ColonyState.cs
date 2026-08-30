using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyState
    {
        public TurnReserve TurnReserve { get; }
        public Station Station { get; }
        public ColonyResources Resources { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<ColonyReformType, ColonyReform> Reforms { get; }
        public Dictionary<ColonyIndustryType, ColonyIndustry> Industries { get; }
        public ColonyAchievements Achievements { get; }

        public ColonyState(
            TurnReserve turnReserve,
            Station station,
            ColonyResources resources,
            IEnumerable<ColonySlot> slots,
            IEnumerable<ColonyReform> reforms,
            IEnumerable<ColonyIndustry> industries,
            ColonyAchievements achievements)
        {
            TurnReserve = turnReserve;
            Station = station;
            Resources = resources;
            Slots = slots.ToDictionary(x => x.Type);
            Reforms = reforms.ToDictionary(x => x.Type);
            Industries = industries.ToDictionary(x => x.Type);
            Achievements = achievements;
        }

        public static ColonyState CreateNew()
        {
            var turnReserve = TurnReserve.CreateNew();
            var station = Station.CreateNew(
                StationModelId.Dawn_342);
            var resouces = ColonyResources.CreateNew();
            var slots = ColonySlot.CreateNew();
            var reforms = ColonyReform.CreateNew();
            var industries = ColonyIndustry.CreateNew();
            var achievements = ColonyAchievements.CreateNew();
            return new ColonyState(
                turnReserve,
                station,
                resouces,
                slots,
                reforms,
                industries,
                achievements);
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
            if (result == 0)
                result = 1; //правитель
            return result;
        }

        public double GetStability()
        {
            var turns = Resources.TurnNumber.Value;
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
    }
}