using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Dtos;

namespace YAGO.World.Application.Playthroughs.Interfaces
{
    public interface IPlaythroughService
    {
        Task<Playthrough> GetPlaythrough(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<Playthrough> SetNextFragment(ClaimsPrincipal userClaimsPrincipal, long currentFragmentId, long nextFragmentId, CancellationToken cancellationToken);
        Task<Playthrough> DropPlaythrough(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
    }
}
