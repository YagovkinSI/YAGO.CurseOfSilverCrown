using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Cities;
using YAGO.World.Infrastructure.Database.Models.Cities;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public CityRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<long> CreateNew(long userId, CancellationToken cancellationToken)
        {
            var city = new CityEntity()
            {
                Id = default,
                Name = City.NewCityConstants.Name,
                UserId = userId,
                Gold = City.NewCityConstants.Gold,
                Population = City.NewCityConstants.Population,
                Military = City.NewCityConstants.Military,
                Fortifications = City.NewCityConstants.Fortifications,
                Control = City.NewCityConstants.Control
            };

            var result = _databaseContext.Cities.Add(city);

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return result.Entity.Id;
        }
    }
}
