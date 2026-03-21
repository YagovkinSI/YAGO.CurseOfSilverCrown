using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface ICycleRepository
    {
        Task<Cycle?> Find(long cycleId, CancellationToken cancellationToken);
        Task<Cycle?> GetLast(long colonyId, CancellationToken cancellationToken);
    }
}
