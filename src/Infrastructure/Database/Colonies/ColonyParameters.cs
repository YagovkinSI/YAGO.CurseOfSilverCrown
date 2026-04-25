namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public long ShipId { get; private set; }
        public int TaxLevel { get; private set; }
        public int SocialGuaranteesLevel { get; private set; }
        public double GovernmentDebt { get; private set; }
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
            int taxLevel,
            int socialGuaranteesLevel,
            double governmentDebt,
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
            TaxLevel = taxLevel;
            SocialGuaranteesLevel = socialGuaranteesLevel;
            MoodTotal = moodTotal;
            GovernmentDebt = governmentDebt;
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
