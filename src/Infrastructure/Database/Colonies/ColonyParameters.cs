using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;

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
        public int Maintenance { get; private set; }
        public int Zones { get; private set; }

        public ColonyParameters(
            long shipId,
            CodeOfLaws startGavernorType,
            IReadOnlyList<long> companies,
            double festivalEffect,
            bool firstWedding,
            int currentWeek,
            int maintenance,
            int zones)
        {
            ShipId = shipId;
            StartGavernorType = startGavernorType;
            Companies = companies;
            FestivalEffect = festivalEffect;
            FirstWedding = firstWedding;
            CurrentWeek = currentWeek;
            Maintenance = maintenance;
            Zones = zones;
        }

        internal void SetShipParameters()
        {
            Maintenance = 100;
            Zones = 140;
        }
    }
}
