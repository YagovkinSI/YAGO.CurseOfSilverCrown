using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public long ShipId { get; private set; }
        public CodeOfLaws StartGavernorType { get; }
        public double MoodTotal { get; private set; }
        public bool FirstWedding { get; private set; }
        public int CurrentWeek { get; private set; }
        public int EpisodeCount { get; private set; }
        public int Zones { get; private set; }
        public IndustryEntity AdministrativeIndustry { get; private set; }
        public IndustryEntity MinningIndustry { get; private set; }
        public IndustryEntity ProductionIndustry { get; private set; }
        public IndustryEntity ServiceIndustry { get; private set; }

        public ColonyParameters(
            long shipId,
            CodeOfLaws startGavernorType,
            double moodTotal,
            bool firstWedding,
            int currentWeek,
            int episodeCount,
            int zones,
            IndustryEntity administrativeIndustry,
            IndustryEntity minningIndustry,
            IndustryEntity productionIndustry,
            IndustryEntity serviceIndustry)
        {
            ShipId = shipId;
            StartGavernorType = startGavernorType;
            MoodTotal = moodTotal;
            FirstWedding = firstWedding;
            CurrentWeek = currentWeek;
            EpisodeCount = episodeCount;
            Zones = zones;
            AdministrativeIndustry = administrativeIndustry;
            MinningIndustry = minningIndustry;
            ProductionIndustry = productionIndustry;
            ServiceIndustry = serviceIndustry;
        }

        internal void SetAdministrativeIndustry()
        {
            AdministrativeIndustry = new IndustryEntity(1, 20, -100, 20);
        }
    }
}
