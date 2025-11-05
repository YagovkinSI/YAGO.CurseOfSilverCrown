using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

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
                .OrderByDescending(x => x.CompletedUtc ?? DateTime.MaxValue)
                .FirstOrDefaultAsync(cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Cycle> CreateNew(Cycle cycle, CancellationToken cancellationToken)
        {
            var newEntity = cycle.ToEntity();
            _databaseContext.Add(newEntity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return newEntity.ToDomain();
        }

        public async Task<Cycle> Update(Cycle cycle, Colony colony, CancellationToken cancellationToken)
        {
            if (cycle.ColonyId != colony.Id)
                throw new YagoException("Несовпадение идентификаиторов.");

            var cycleEtity = await _databaseContext.Cycles.FindAsync([cycle.Id], cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Cycle), cycle.Id);

            var colonyEntity = await _databaseContext.Colonies.FindAsync([colony.Id], cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Colony), colony.Id);

            cycleEtity.Update(cycle);
            colonyEntity.Update(colony);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return cycleEtity.ToDomain();
        }

        public Task<Cycle> ApplyCycle(long cycleId, decimal solarIncome, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Cycle> CreateNew(long colonyId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}