using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users
{
    public interface IUserService
    {
        Task<User?> GetMyUser(long userId, CancellationToken cancellationToken);
        Task<User> Login(string userName, string password, CancellationToken cancellationToken);
        Task<User> Register(string userName, string password, string? email, CancellationToken cancellationToken);
        Task<User> CreateTemporaryUser(CancellationToken cancellationToken);
        Task<User> ConvertToPermanentUser(long userId, string userName, string? email, string password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
        Task UpdateLastActivity(long userId, CancellationToken cancellationToken);
    }
}
