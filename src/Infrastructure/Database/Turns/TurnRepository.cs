using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Turns;

namespace YAGO.World.Infrastructure.Database.Turns
{
    internal class TurnRepository : ITurnRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public TurnRepository(
            ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Turn> Add(Turn turn, CancellationToken cancellationToken)
        {
            var entity = turn.ToEntity();

            _databaseContext.Add(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }

        public async Task<Turn?> Find(Guid turnId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Turns
                .FindAsync([turnId], cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Turn?> FindLastColonyTurn(Guid colonyId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Turns
                .Where(x => x.ColonyId == colonyId)
                .OrderByDescending(x => x.RunAtUtc ?? DateTime.MaxValue)
                .FirstOrDefaultAsync(cancellationToken);
            return entity?.ToDomain();
        }
    }
}