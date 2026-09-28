using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Buildings;
using YAGO.World.Domain.Colonies.Councils;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Colonies.Policies;
using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Colonies.Slots;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyState
    {
        public TurnReserve TurnReserve { get; }
        public ColonyResources Resources { get; }
        public ColonyPolicy Policy { get; }
        public int TurnNumber { get; private set; }
        public ColonyAchievements Achievements => _progress.Achievements;
        public UnlockedWikiArticles UnlockedWikiArticles => _progress.UnlockedWikiArticles;

        public ColonyMood Mood { get; }
        public Dictionary<ColonySlotType, ColonySlot> Slots { get; }
        public Dictionary<ColonyIndustryType, ColonyIndustry> Industries { get; }
        public Council Council => _progress.Council;

        private readonly ColonyProgress _progress;


        public ColonyState(
            TurnReserve turnReserve,
            ColonyResources resources,
            ColonyPolicy policy,
            IEnumerable<ColonyIndustry> industries,
            ColonyProgress progress,
            int turnNumber,
            ColonyMood mood)
        {
            TurnReserve = turnReserve;
            Resources = resources;
            Policy = policy;
            Industries = industries.ToDictionary(x => x.Type);
            _progress = progress;
            TurnNumber = turnNumber;
            Mood = mood;
            Slots = ColonySlot.CreateNew().ToDictionary(x => x.Type);
        }

        public static ColonyState CreateNew()
        {
            var turnReserve = TurnReserve.CreateNew();
            var resouces = ColonyResources.CreateNew();
            var policy = ColonyPolicy.CreateNew();
            var industries = ColonyIndustry.CreateNew();
            var progress = ColonyProgress.CreateNew();
            var mood = new ColonyMood(value: 50);
            return new ColonyState(
                turnReserve,
                resouces,
                policy,
                industries,
                progress,
                turnNumber: 1,
                mood);
        }

        internal void SetStation(string stationCode)
        {
            Policy.SetStation(stationCode);
        }

        internal void SetAsteroid(string asteroidCode)
        {
            Policy.SetAsteroid(asteroidCode);
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
            return TurnNumber / 3.0;
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

        public YagoLevel GetYagoLevel()
        {
            return YagoLevel.Gray;
        }

        public double GetMoodDelta()
        {
            if (!Achievements.HasAchievement(AchievementConstants.ColonyOpen))
                return 0;
            var medicalInsuranceCoef = 1 - (Policy.Reforms.MedicalInsurance.Value / 4.0);
            return -GetPopulation() * 0.02 * medicalInsuranceCoef;
        }

        public PublicDebt GetPublicDebt()
        {
            var yagoLevel = GetYagoLevel();
            var publicDebtContext = new PublicDebtContext(yagoLevel);
            return new PublicDebt(Policy.Reforms.PublicDebt, publicDebtContext);
        }

        internal void AddTurnNumber()
        {
            TurnNumber++;
        }
    }
}
