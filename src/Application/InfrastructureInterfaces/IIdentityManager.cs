using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.InfrastructureInterfaces
{
    public interface IIdentityManager
    {
        Task<User?> GetCurrentUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
        Task Register(User user, string password, CancellationToken cancellationToken);
        Task UpdateUserName(ClaimsPrincipal claimsPrincipal, string userName, CancellationToken cancellationToken);
        Task UpdatePassword(ClaimsPrincipal claimsPrincipal, string password, CancellationToken cancellationToken);
        Task Login(string userName, string password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
    }
}
