using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Application.InfrastructureInterfaces
{
    public interface IIdentityManager
    {
        Task<CurrentUser?> GetCurrentUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
        Task Register(CurrentUser user, string password, CancellationToken cancellationToken);
        Task UpgradeRegister(ClaimsPrincipal claimsPrincipal, string userName, string email, string password, CancellationToken cancellationToken);
        Task Login(string userName, string password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
    }
}
