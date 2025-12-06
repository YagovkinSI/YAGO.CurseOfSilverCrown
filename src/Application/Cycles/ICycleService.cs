using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Colonies
{
    public interface ICycleService
    {
        Task<Cycle?> GetMyLastCycle(long userId, CancellationToken cancellationToken);
        Task<Cycle?> RunCycle(long userId, CancellationToken cancellationToken);
        Task<Cycle?> AttackColony(long userId, long targetColonyId, AttackColonyPrizeType prizeType, CancellationToken cancellationToken);
    }
}
