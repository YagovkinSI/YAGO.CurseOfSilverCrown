using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.CreateColony
{
    public record CreateColonyResult(
        Colony MyColony)
        : IProcessorResult
    { }
}
