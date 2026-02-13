using YAGO.World.Application.Colonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.GetPaginatedColonies
{
    public interface IPaginatedColoniesProvider : IProvider<GetPaginatedColoniesCommand, PaginatedData<ColonyWithDetails>>
    {
    }
}
