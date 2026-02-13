using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.GetColonyWithDetails
{
    public record GetColonyWithDetailsCommand(
        long UserId)
        : IProcessorCommand
    { }
}
