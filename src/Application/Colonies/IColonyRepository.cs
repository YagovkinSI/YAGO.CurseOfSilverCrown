using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyRepository
    {
        Task<Colony?> Find(long colonyId, CancellationToken cancellationToken);
        Task<Colony?> FindByUserId(long userId, CancellationToken cancellationToken);
        Task<Colony?> FindByName(string name, CancellationToken cancellationToken);
        Task<Colony> Add(Colony colony, CancellationToken cancellationToken);
        Task<Colony> Update(Colony colony, CancellationToken cancellationToken);
        Task<PaginatedData<Colony>> GetPaginatedColonies(int page, CancellationToken cancellationToken);
    }
}
