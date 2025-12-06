using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Application.Cycles
{
    public interface IUnitOfWorkRepository
    {
        Task UpdateInTransactionAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : IEntity;
    }
}
