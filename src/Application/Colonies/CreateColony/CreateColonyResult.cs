using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.CreateColony
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
