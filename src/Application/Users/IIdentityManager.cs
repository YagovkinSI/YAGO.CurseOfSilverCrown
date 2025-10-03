using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users
{
    public interface IIdentityManager
    {
        Task<User?> GetCurrentUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
        Task Register(string userName, string password, string? email, CancellationToken cancellationToken);
        Task<User> CreateTemporaryUser(CancellationToken cancellationToken);
        Task<User> ConvertToPermanentAccount(ClaimsPrincipal claimsPrincipal, string userName, string password, string? email, CancellationToken cancellationToken);
        Task Login(string userName, string? password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
    }
}
