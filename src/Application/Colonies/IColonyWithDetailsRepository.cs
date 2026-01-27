using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies
{
    public interface IColonyWithDetailsRepository
    {
        Task<ColonyWithDetails?> Find(long colonyId, CancellationToken cancellationToken);
    }
}
