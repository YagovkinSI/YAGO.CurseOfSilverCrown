using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.CurrentUsers;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryCurrentUser
    {
        Task<CurrentUser?> FindAsync(string userId, CancellationToken cancellationToken);

        Task<CurrentUser?> FindByUserName(string userName, CancellationToken cancellationToken);

        Task UpdateLastActivity(string userId, DateTime lastActivity, CancellationToken cancellationToken);
    }
}