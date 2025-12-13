using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.BuyBuilding
{
    public class BuyBuildingResult : IProcessorResult
    {
        public ColonyWithShipAndBuildings MyColony { get; }

        public BuyBuildingResult(ColonyWithShipAndBuildings myColony)
        {
            MyColony = myColony;
        }
    }
}
