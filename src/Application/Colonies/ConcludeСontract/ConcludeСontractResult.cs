using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.ConcludeСontract
{
    public class ConcludeСontractResult : IProcessorResult
    {
        public ColonyWithShipAndContracts MyColony { get; }

        public ConcludeСontractResult(ColonyWithShipAndContracts myColony)
        {
            MyColony = myColony;
        }
    }
}
