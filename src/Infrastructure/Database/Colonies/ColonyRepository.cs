using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal class ColonyRepository : IColonyRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public ColonyRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Colony?> Find(long colonyId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Colonies
                .FindAsync([colonyId], cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Colony?> FindByUserId(long userId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Colonies
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<bool> IsNameAvailable(string name, CancellationToken cancellationToken)
        {
            var hasEntity = await _databaseContext.Colonies
                .AnyAsync(u => u.Name == name, cancellationToken);
            return !hasEntity;
        }

        public async Task<Colony> Add(Colony colony, CancellationToken cancellationToken)
        {
            var entity = colony.ToEntity();

            _databaseContext.Add(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }

        public async Task Update(Colony colony, CancellationToken cancellationToken)
        {
            var source = colony.ToEntity();

            var target = await _databaseContext.Colonies.FindAsync([colony.Id], cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Colony), colony.Id.ToString());

            EntityUpdater.Update(source, target);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginatedData<Colony>> GetPaginatedColonies(int page, int itemsInPage, CancellationToken cancellationToken)
        {
            var data = await _databaseContext.Colonies
                .Include(x => x.User)
                .Where(x => x.Cycles.Count > 1)
                .OrderByDescending(x => x.User!.LastActivityAtUtc)
                .Skip((page - 1) * itemsInPage)
                .Take(itemsInPage)
                .Select(x => x.ToDomain())
                .ToArrayAsync();

            var total = await _databaseContext.Colonies.CountAsync();

            return new PaginatedData<Colony>(data, total, page, itemsInPage);
        }
    }
}