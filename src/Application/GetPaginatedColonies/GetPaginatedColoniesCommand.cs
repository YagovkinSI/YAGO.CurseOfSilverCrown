using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.GetPaginatedColonies
{
    public record GetPaginatedColoniesCommand(
        int Page) 
        : IProcessorCommand
    { }
}
