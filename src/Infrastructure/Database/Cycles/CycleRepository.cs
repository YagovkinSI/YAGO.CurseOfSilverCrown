using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Cycles;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    internal class CycleRepository : ICycleRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public CycleRepository(
            ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Cycle> Add(Cycle cycle, CancellationToken cancellationToken)
        {
            var entity = cycle.ToEntity();

            _databaseContext.Add(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }

        public async Task<Cycle?> Find(long cycleId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Cycles
                .FindAsync([cycleId], cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Cycle?> GetLast(long colonyId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Cycles
                .Where(x => x.ColonyId == colonyId)
                .OrderByDescending(x => x.RunAtUtc ?? DateTime.MaxValue)
                .FirstOrDefaultAsync(cancellationToken);
            return entity?.ToDomain();
        }
    }
}