using System;
using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public int ActionPoints { get; private set; }
        public int ActionPointsTrend { get; private set; }
        public long ShipId { get; private set; }
        public int TaxLevel { get; private set; }
        public int SocialGuaranteesLevel { get; private set; }
        public double MoodTotal { get; private set; }
        public bool FirstWedding { get; private set; }
        public int CurrentWeek { get; private set; }
        public int Zones { get; private set; }
        public IndustryEntity AdministrativeIndustry { get; private set; }
        public IndustryEntity MinningIndustry { get; private set; }
        public IndustryEntity ProductionIndustry { get; private set; }
        public IndustryEntity ServiceIndustry { get; private set; }
        public IReadOnlyList<string> EventIds { get; private set; }

        public ColonyParameters(
            int actionPoints,
            int actionPointsTrend,
            long shipId,
            int taxLevel,
            int socialGuaranteesLevel,
            double moodTotal,
            bool firstWedding,
            int currentWeek,
            int zones,
            IndustryEntity administrativeIndustry,
            IndustryEntity minningIndustry,
            IndustryEntity productionIndustry,
            IndustryEntity serviceIndustry,
            IReadOnlyList<string> eventIds)
        {
            ActionPoints = actionPoints;
            ActionPointsTrend = actionPointsTrend;
            ShipId = shipId;
            TaxLevel = taxLevel;
            SocialGuaranteesLevel = socialGuaranteesLevel;
            MoodTotal = moodTotal;
            FirstWedding = firstWedding;
            CurrentWeek = currentWeek;
            Zones = zones;
            AdministrativeIndustry = administrativeIndustry;
            MinningIndustry = minningIndustry;
            ProductionIndustry = productionIndustry;
            ServiceIndustry = serviceIndustry;
            EventIds = eventIds;
        }

        internal void SetAdministrativeIndustry()
        {
            AdministrativeIndustry = new IndustryEntity(1, 20, -100, 20);
        }
    }
}
