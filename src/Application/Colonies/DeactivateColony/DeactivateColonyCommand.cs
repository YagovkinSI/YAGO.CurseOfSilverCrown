using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.DeactivateColony
{
    public record DeactivateColonyCommand(
        long UserId) : IProcessorCommand
    {
    }
}
