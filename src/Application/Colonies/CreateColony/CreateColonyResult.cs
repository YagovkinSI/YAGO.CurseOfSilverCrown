using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.CreateColony
{
    public class CreateColonyResult : IProcessorResult
    {
        public ColonyWithDetails MyColony { get; }

        public CreateColonyResult(ColonyWithDetails myColony)
        {
            MyColony = myColony;
        }
    }
}
