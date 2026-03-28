using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<User?> Find(long userId, CancellationToken cancellationToken);
        Task<User?> FindByName(string userName, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
    }
}
