using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.CreateColony
{
    public record CreateColonyResult(
        ColonyWithDetails MyColony)
        : IProcessorResult
    { }
}
