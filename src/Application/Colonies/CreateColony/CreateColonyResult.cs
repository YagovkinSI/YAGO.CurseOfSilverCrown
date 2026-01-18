using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.CreateColony
{
    public class CreateColonyResult : IProcessorResult
    {
        public ColonyWithShipAndContracts MyColony { get; }

        public CreateColonyResult(ColonyWithShipAndContracts myColony)
        {
            MyColony = myColony;
        }
    }
}
