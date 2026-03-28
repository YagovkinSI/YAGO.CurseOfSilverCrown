using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IColonyRepository
    {
        Task<Colony?> Find(long colonyId, CancellationToken cancellationToken);
        Task<Colony?> FindByUserId(long userId, CancellationToken cancellationToken);
        Task<bool> IsNameAvailable(string name, CancellationToken cancellationToken);
        Task<Colony> Add(Colony colony, CancellationToken cancellationToken);
        Task<Colony> Update(Colony colony, CancellationToken cancellationToken);
        Task<PaginatedData<Colony>> GetPaginatedColonies(int page, int itemsInPage, CancellationToken cancellationToken);
    }
}
