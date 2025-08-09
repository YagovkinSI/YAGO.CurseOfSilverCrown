using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface ICurrentUserRepository
    {
        Task<User?> Find(long userId, CancellationToken cancellationToken);
        Task<User?> FindByUserName(string userName, CancellationToken cancellationToken);
        Task UpdateLastActivity(long userId, DateTime lastActivity, CancellationToken cancellationToken);
    }
}
