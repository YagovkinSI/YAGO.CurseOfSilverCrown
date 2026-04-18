using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Application.Services
{
    public interface ICurrentCycleProvider
    {
        Task<Cycle> Get(Guid colonyId, CancellationToken cancellationToken);
    }

    public class CurrentCycleProvider(
        ICycleRepository cycleRepository)
        : ICurrentCycleProvider
    {
        public async Task<Cycle> Get(Guid colonyId, CancellationToken cancellationToken)
        {
            var cycle = await cycleRepository.FindLastColonyCycle(colonyId, cancellationToken);
            if (cycle == null || cycle.IsComplited)
            {
                cycle = Cycle.CreateNew(colonyId, cycle);
                await cycleRepository.Add(cycle, cancellationToken);
            }

            return cycle;
        }
    }
}
