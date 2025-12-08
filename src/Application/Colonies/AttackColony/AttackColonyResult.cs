using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Colonies.AttackColony
{
    public class AttackColonyResult : IProcessorResult
    {
        public Cycle Cycle { get; }
        public ColonyWithShipAndBuildings ColonyWithShipAndBuildings { get; }

        public AttackColonyResult(
            Cycle cycle,
            ColonyWithShipAndBuildings colonyWithShipAndBuildings)
        {
            Cycle = cycle;
            ColonyWithShipAndBuildings = colonyWithShipAndBuildings;
        }
    }
}
