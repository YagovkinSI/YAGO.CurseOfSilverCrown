using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cities;
using YAGO.World.Domain.Cities;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Cities
{
    internal class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public CityRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<City?> Find(long id, CancellationToken cancellationToken)
        {
            var cityEntity = await _databaseContext.Cities
                .FindAsync([id], cancellationToken);
            return cityEntity?.ToDomain();
        }

        public async Task<City?> FindByUser(long userId, CancellationToken cancellationToken)
        {
            var cityEntity = await _databaseContext.Cities
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            return cityEntity?.ToDomain();
        }

        public async Task<City> Create(long userId, string name, string description, CancellationToken cancellationToken)
        {
            var cityEntity = CityEntity.CreateNew(userId, name, description);

            _databaseContext.Add(cityEntity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return cityEntity.ToDomain();
        }
    }
}