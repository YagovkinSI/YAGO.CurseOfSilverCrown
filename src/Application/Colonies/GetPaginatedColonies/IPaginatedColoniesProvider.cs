using YAGO.World.Application.Common.Pagination;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.GetPaginatedColonies
{
    public interface IPaginatedColoniesProvider : IProvider<GetPaginatedColoniesCommand, PaginatedData<Colony>>
    {
    }
}
