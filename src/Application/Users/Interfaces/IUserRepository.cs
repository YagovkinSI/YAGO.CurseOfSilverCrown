using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> Find(long userId, CancellationToken cancellationToken);

        Task<User?> FindByName(string userName, CancellationToken cancellationToken);

        Task UpdateLastActivity(long userId, CancellationToken cancellationToken);
    }
}
