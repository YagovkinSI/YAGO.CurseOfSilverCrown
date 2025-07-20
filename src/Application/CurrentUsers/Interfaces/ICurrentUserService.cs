using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Application.CurrentUsers.Interfaces
{
    public interface ICurrentUserService
    {
        Task<AuthorizationData> GetAuthorizationData(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<AuthorizationData> Login(string userName, string password, CancellationToken cancellationToken);
        Task<AuthorizationData> Register(string userName, string email, string password, CancellationToken cancellationToken);
        Task<AuthorizationData> AutoRegister(CancellationToken cancellationToken);
        Task<AuthorizationData> UpgradeRegister(ClaimsPrincipal userClaimsPrincipal, string userName, string email, string password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
    }
}
