using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Cycles
{
    public interface ICycleRepository
    {
        Task<Cycle?> Find(long cycleId, CancellationToken cancellationToken);
        Task<Cycle?> GetLast(long colonyId, CancellationToken cancellationToken);
        Task<Cycle> CreateNew(long colonyId, CancellationToken cancellationToken);
        Task<Cycle> Update(Cycle cycle, Colony colony, CancellationToken cancellationToken);
    }
}
