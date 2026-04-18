using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface IUnitOfWorkRepository
    {
        Task SaveInTransactionAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : IEntity;
    }
}
