using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface ICurrentUserRepository
    {
        Task<CurrentUser?> Find(long userId, CancellationToken cancellationToken);
        Task<CurrentUserWithStoryNode?> FindCurrentUserWithStoryNode(long userId, CancellationToken cancellationToken);
        Task<CurrentUser?> FindByUserName(string userName, CancellationToken cancellationToken);
        Task<CurrentUserWithStoryNode?> FindCurrentUserWithStoryNodeByUserName(string userName, CancellationToken cancellationToken);
        Task UpdateLastActivity(long userId, DateTime lastActivity, CancellationToken cancellationToken);
    }
}
