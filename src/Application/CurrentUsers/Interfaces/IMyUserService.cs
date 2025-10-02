using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.CurrentUsers.Interfaces
{
    public interface IMyUserService
    {
        Task<User?> GetMyUser(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<User> Login(string userName, string password, CancellationToken cancellationToken);
        Task<User> Register(string userName, string password, string? email, CancellationToken cancellationToken);
        Task<User> CreateTemporaryUser(CancellationToken cancellationToken);
        Task<User> ConvertToPermanentUser(ClaimsPrincipal userClaimsPrincipal, string userName, string? email, string password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
    }
}
