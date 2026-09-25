using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Councils;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Policies;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyState
    {
        public ColonyName Name { get; }
        public ColonyResources Resources { get; }
        public ColonyPolicy Policy { get; }
        public ColonyTurnNumber TurnNumber { get; }
        public TurnReserve TurnReserve { get; }
        public ColonyAchievements Achievements => _progress.Achievements;
        public UnlockedWikiArticles UnlockedWikiArticles => _progress.UnlockedWikiArticles;

        public ColonyMood Mood { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<ColonyIndustryType, ColonyIndustry> Industries { get; }
        public Council Council => _progress.Council;

        private readonly ColonyProgress _progress;

        public string DisplayName => Name.Named
            ? Name.DatabaseName
            : "Колония";

        public ColonyState(
            TurnReserve turnReserve,
            ColonyResources resources,
            ColonyPolicy policy,
            IEnumerable<ColonyIndustry> industries,
            ColonyProgress progress,
            ColonyName name,
            ColonyTurnNumber turns,
            ColonyMood mood)
        {
            TurnReserve = turnReserve;
            Resources = resources;
            Slots = ColonySlot.CreateNew().ToDictionary(x => x.Type);
            Policy = policy;
            Industries = industries.ToDictionary(x => x.Type);
            _progress = progress;
            Name = name;
            TurnNumber = turns;
            Mood = mood;
        }

        public static ColonyState CreateNew()
        {
            var turnReserve = TurnReserve.CreateNew();
            var resouces = ColonyResources.CreateNew();
            var policy = ColonyPolicy.CreateNew();
            var industries = ColonyIndustry.CreateNew();
            var progress = ColonyProgress.CreateNew();
            var name = ColonyName.CreateNew();
            var turns = new ColonyTurnNumber(value: 1);
            var mood = new ColonyMood(value: 50);
            return new ColonyState(
                turnReserve,
                resouces,
                policy,
                industries,
                progress,
                name, 
                turns, 
                mood);
        }

        internal void SetStation(StationModelId stationId)
        {
            Policy.SetStation(stationId);
        }

        internal void SetAsteroid(AsteroidId asteroidId)
        {
            Policy.SetAsteroid(asteroidId);
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
            var turns = TurnNumber.Value;
            return turns / 3.0;
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
            if (!Achievements.HasAchievement(AchievementConstants.ColonyOpen))
                return 0;
            var medicalInsuranceCoef = 1 - Policy.Reforms.MedicalInsurance.Value / 4.0;
            return -GetPopulation() * 0.02 * medicalInsuranceCoef;
        }

        public PublicDebt GetPublicDebt()
        {
            var yagoLevel = GetYagoLevel();
            var publicDebtContext = new PublicDebtContext(yagoLevel);
            return new PublicDebt(Policy.Reforms.PublicDebt, publicDebtContext);
        }
    }
}
