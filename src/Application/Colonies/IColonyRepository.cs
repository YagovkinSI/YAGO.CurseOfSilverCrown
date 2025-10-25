using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyRepository
    {
        Task<Colony?> Find(long colonyId, CancellationToken cancellationToken);
        Task<Colony?> FindByUserId(long userId, CancellationToken cancellationToken);
        Task<Colony?> FindByName(string name, CancellationToken cancellationToken);
        Task<Colony> CreateColomy(CreateColonyDto colony, CancellationToken cancellationToken);
        Task<Colony> ByuBuilding(long colonyId, Building building, CancellationToken cancellationToken);
    }
}
