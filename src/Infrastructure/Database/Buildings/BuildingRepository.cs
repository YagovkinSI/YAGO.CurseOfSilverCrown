using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Domain.Buildings;

namespace YAGO.World.Infrastructure.Database.Buildings
{
    internal class BuildingRepository : IBuildingRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public BuildingRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Building?> Find(long buildingId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Buildings
                .FindAsync([buildingId], cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Building[]> GetBuildings(int page, int count, CancellationToken cancellationToken)
        {
            var entities = await _databaseContext.Buildings
                .OrderBy(x => x.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .ToArrayAsync();

            return entities
                .Select(x => x.ToDomain())
                .ToArray();
        }
    }
}