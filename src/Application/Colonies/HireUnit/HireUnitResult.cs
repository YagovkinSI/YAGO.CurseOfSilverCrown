using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.HireUnit
{
    public class HireUnitResult : IProcessorResult
    {
        public ColonyWithShipAndContracts MyColony { get; }

        public HireUnitResult(ColonyWithShipAndContracts myColony)
        {
            MyColony = myColony;
        }
    }
}
