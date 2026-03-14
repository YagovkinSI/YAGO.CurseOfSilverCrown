using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyService
    {
        Task<Colony?> GetMyColony(long userId, CancellationToken cancellationToken);
    }
}
