using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.GetColonyWithDetails
{
    public record GetColonyWithDetailsCommand(
        long UserId)
        : IProcessorCommand
    { }
}
