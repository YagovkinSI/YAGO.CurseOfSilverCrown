using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Common;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IUnitOfWorkRepository
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        Task<T> Add<T>(IEntity<T> domainEntity, CancellationToken cancellationToken);
        Task Update<T>(IEntity<T> domainEntity, CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
    }
}
