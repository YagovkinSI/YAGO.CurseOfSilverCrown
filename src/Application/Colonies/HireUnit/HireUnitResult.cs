using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.HireUnit
{
    public class HireUnitResult : IProcessorResult
    {
        public ColonyWithShipAndBuildings MyColony { get; }

        public HireUnitResult(ColonyWithShipAndBuildings myColony)
        {
            MyColony = myColony;
        }
    }
}
