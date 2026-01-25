using System.Collections.Generic;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public long ShipId { get; private set; }
        public GavernorType StartGavernorType { get; }
        public Dictionary<long, int> Contracts { get; private set; }

        public ColonyParameters(
            long shipId,
            GavernorType startGavernorType,
            Dictionary<long, int> contracts)
        {
            ShipId = shipId;
            StartGavernorType = startGavernorType;
            Contracts = contracts;
        }

        public void SetShipDefault()
        {
            ShipId = 1;
        }
    }
}
