using System.Collections.Generic;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public long ShipId { get; private set; }
        public CodeOfLaws StartGavernorType { get; }
        public IReadOnlyList<long> Companies { get; private set; }
        public double FestivalEffect { get; private set; }
        public bool FirstWedding { get; private set; }
        public int CurrentWeek { get; private set; }

        public ColonyParameters(
            long shipId,
            CodeOfLaws startGavernorType,
            IReadOnlyList<long> companies,
            double festivalEffect,
            bool firstWedding,
            int currentWeek)
        {
            ShipId = shipId;
            StartGavernorType = startGavernorType;
            Companies = companies;
            FestivalEffect = festivalEffect;
            FirstWedding = firstWedding;
            CurrentWeek = currentWeek;
        }
    }
}
