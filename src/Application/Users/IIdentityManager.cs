using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users
{
    public interface IIdentityManager
    {
        Task Register(string userName, string password, string? email, CancellationToken cancellationToken);
        Task<User> CreateTemporaryUser(CancellationToken cancellationToken);
        Task<User> ConvertToPermanentAccount(long userId, string userName, string password, string? email, CancellationToken cancellationToken);
        Task Login(string userName, string? password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
    }
}
