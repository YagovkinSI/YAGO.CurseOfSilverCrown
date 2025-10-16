using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Colonies
{
    public interface ICycleService
    {
        Task<Cycle?> GetMyLastCycle(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
    }
}
