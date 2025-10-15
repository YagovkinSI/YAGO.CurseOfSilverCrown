using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Users;

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

        public async Task<Colony?> FindByName(string name, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Colonies
                .FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Colony> CreateColomy(CreateColonyDto colony, CancellationToken cancellationToken)
        {
            var entity = ColonyEntity.CreateNew(
                colony.UserId,
                colony.Name,
                colony.SolarsIncome,
                colony.Reputation,
                colony.Population);

            _databaseContext.Add(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }
    }
}