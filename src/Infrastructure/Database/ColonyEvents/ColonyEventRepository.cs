using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Infrastructure.Database.ColonyEvents
{
    internal class ColonyEventRepository : IColonyEventRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public ColonyEventRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ColonyEvent?> Find(long colonyEventId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.ColonyEvents
                .FindAsync([colonyEventId], cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<IReadOnlyList<ColonyEvent>> FindByColonyId(long colonyId, bool onlyNotComplited, CancellationToken cancellationToken)
        {
            var entities = _databaseContext.ColonyEvents
                .Where(u => u.ColonyId == colonyId)
                .Where(x => !onlyNotComplited || !x.IsCompleted);
            var result = entities.Select(x => x.ToDomain()).ToList();
            return await Task.FromResult(result);
        }

        public async Task<ColonyEvent> Add(ColonyEvent colonyEvent, CancellationToken cancellationToken)
        {
            var entity = colonyEvent.ToEntity();

            _databaseContext.Add(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }

        public async Task Update(ColonyEvent colonyEvent, CancellationToken cancellationToken)
        {
            var source = colonyEvent.ToEntity();

            var target = await _databaseContext.ColonyEvents.FindAsync([colonyEvent.Id], cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Colony), colonyEvent.Id.ToString());

            _databaseContext.Entry(target).CurrentValues.SetValues(source);
            _databaseContext.Update(target);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}