using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Councils;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyState
    {
        public ColonyResources Resources { get; }

        public ColonyTurnNumber TurnNumber { get; }
        public ColonyMood Mood { get; }

        public TurnReserve TurnReserve { get; }
        public Station Station { get; }
        public Asteroid? Asteroid { get; private set; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public ColonyReforms Reforms { get; }
        public Dictionary<ColonyIndustryType, ColonyIndustry> Industries { get; }
        public ColonyAchievements Achievements => _progress.Achievements;
        public UnlockedWikiArticles UnlockedWikiArticles => _progress.UnlockedWikiArticles;
        public Council Council => _progress.Council;
        public ColonyName Name { get; }

        private readonly ColonyProgress _progress;

        public string DisplayName => Name.Named
            ? Name.DatabaseName
            : "Колония";

        public ColonyState(
            TurnReserve turnReserve,
            Station station,
            Asteroid? asteroid,
            ColonyResources resources,
            ColonyReforms reforms,
            IEnumerable<ColonyIndustry> industries,
            ColonyProgress progress,
            ColonyName name,
            ColonyTurnNumber turns,
            ColonyMood mood)
        {
            TurnReserve = turnReserve;
            Station = station;
            Asteroid = asteroid;
            Resources = resources;
            Slots = ColonySlot.CreateNew().ToDictionary(x => x.Type);
            Reforms = reforms;
            Industries = industries.ToDictionary(x => x.Type);
            _progress = progress;
            Name = name;
            TurnNumber = turns;
            Mood = mood;
        }

        public static ColonyState CreateNew()
        {
            var turnReserve = TurnReserve.CreateNew();
            var station = Station.CreateNew(
                StationModelId.Dawn_342);
            var resouces = ColonyResources.CreateNew();
            var reforms = ColonyReforms.CreateNew();
            var industries = ColonyIndustry.CreateNew();
            var progress = ColonyProgress.CreateNew();
            var name = ColonyName.CreateNew();
            var turns = new ColonyTurnNumber(value: 1);
            var mood = new ColonyMood(value: 50);
            return new ColonyState(
                turnReserve,
                station,
                asteroid: null,
                resouces,
                reforms,
                industries,
                progress,
                name, 
                turns, 
                mood);
        }

        internal void SetAsteroid(AsteroidId asteroidId)
        {
            Asteroid = AsteroidDataset.Get(asteroidId);
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
            var medicalInsuranceCoef = 1 - Reforms.MedicalInsurance.Value / 4.0;
            return -GetPopulation() * 0.02 * medicalInsuranceCoef;
        }

        public PublicDebt GetPublicDebt()
        {
            var yagoLevel = GetYagoLevel();
            var publicDebtContext = new PublicDebtContext(yagoLevel);
            return new PublicDebt(Reforms.PublicDebt, publicDebtContext);
        }
    }
}
