using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Cycles
{
    public interface ICycleService
    {
        Task<Cycle> GetMyLastCycle(long userId, CancellationToken cancellationToken);
    }
}
