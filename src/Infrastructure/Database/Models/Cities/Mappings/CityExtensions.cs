using YAGO.World.Domain.Cities;

namespace YAGO.World.Infrastructure.Database.Models.Cities.Mappings
{
    internal static class CityExtensions
    {
        public static City ToDomain(this CityEntity source)
        {
            return new City(
                source.Id,
                source.Name,
                source.UserId,
                source.Gold,
                source.Population,
                source.Military,
                source.Fortifications,
                source.Control
            );
        }
    }
}
