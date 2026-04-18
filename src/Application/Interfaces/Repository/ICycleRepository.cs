using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Application.Interfaces.Repository
{
    public interface ICycleRepository
    {
        Task<Cycle> Add(Cycle cycle, CancellationToken cancellationToken);
        Task<Cycle?> Find(Guid cycleId, CancellationToken cancellationToken);
        Task<Cycle?> FindLastColonyCycle(Guid colonyId, CancellationToken cancellationToken);
    }
}
