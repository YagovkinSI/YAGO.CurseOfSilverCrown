using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyService
    {
        Task<Colony?> GetMyColony(long userId, CancellationToken cancellationToken);
        Task<ColonyWithShipAndBuildings?> GetMyColonyWithShipAndBuildings(long userId, CancellationToken cancellationToken);
        Task<PaginatedData<ColonyWithShipAndBuildings>> GetPaginatedColonies(int page, CancellationToken cancellationToken);
    }
}
