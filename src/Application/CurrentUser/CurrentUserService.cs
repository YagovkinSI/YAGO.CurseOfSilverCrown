using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.CurrentUser;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.CurrentUser
{
    public interface ICurrentUserService
    {
        Task<User> Get(ClaimsPrincipal userClaimsPrincipal);
        Task<AuthorizationData> GetAuthorizationData(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<bool> IsAdmin(string userId);
    }
}
