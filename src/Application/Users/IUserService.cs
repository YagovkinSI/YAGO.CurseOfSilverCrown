using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Users
{
    public interface IUserService
    {
        Task CreateTemporaryUser(CancellationToken cancellationToken);
        Task<User> ConvertToPermanentUser(long userId, string userName, string? email, string password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
        Task UpdateLastActivity(long userId, CancellationToken cancellationToken);
    }
}
