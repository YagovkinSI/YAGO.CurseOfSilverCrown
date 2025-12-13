using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.СreateColony
{
    public class CreateColonyResult : IProcessorResult
    {
        public ColonyWithShipAndBuildings MyColony { get; }

        public CreateColonyResult(ColonyWithShipAndBuildings myColony)
        {
            MyColony = myColony;
        }
    }
}
