using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.ConcludeContract
{
    public class ConcludeContractResult : IProcessorResult
    {
        public ColonyWithShipAndContracts MyColony { get; }

        public ConcludeContractResult(ColonyWithShipAndContracts myColony)
        {
            MyColony = myColony;
        }
    }
}
