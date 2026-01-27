using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.ConcludeContract
{
    public class ConcludeContractResult : IProcessorResult
    {
        public ColonyWithDetails MyColony { get; }

        public ConcludeContractResult(ColonyWithDetails myColony)
        {
            MyColony = myColony;
        }
    }
}
