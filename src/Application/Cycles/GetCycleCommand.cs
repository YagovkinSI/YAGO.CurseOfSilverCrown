using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Cycles
{
    public record GetCycleCommand(
        long UserId)
        : IProcessorCommand;
}
