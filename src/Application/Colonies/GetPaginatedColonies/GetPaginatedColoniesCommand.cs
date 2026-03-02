using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Colonies.GetPaginatedColonies
{
    public record GetPaginatedColoniesCommand(
        int Page)
        : IProcessorCommand
    { }
}
