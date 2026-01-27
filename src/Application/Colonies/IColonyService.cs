using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyService
    {
        Task<Colony?> GetMyColony(long userId, CancellationToken cancellationToken);
        Task<ColonyWithDetails?> GetMyColonyWithDetails(long userId, CancellationToken cancellationToken);
        Task<PaginatedData<ColonyWithDetails>> GetPaginatedColonies(int page, CancellationToken cancellationToken);
    }
}
