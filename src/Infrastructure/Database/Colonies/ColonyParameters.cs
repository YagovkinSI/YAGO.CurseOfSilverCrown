using System.Collections.Generic;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyParameters
    {
        public GavernorType StartGavernorType { get; }
        public Dictionary<long, int> Contracts { get; private set; }

        public ColonyParameters(
            GavernorType startGavernorType,
            Dictionary<long, int> contracts)
        {
            StartGavernorType = startGavernorType;
            Contracts = contracts;
        }
    }
}
