using YAGO.World.Domain.Cities;
using YAGO.World.Infrastructure.Database.Cities;

namespace YAGO.World.Infrastructure.Database.Users
{
    internal static class CityEntityMapper
    {
        public static City ToDomain(this CityEntity source)
        {
            return new City(
                source.Id,
                source.UserId,
                source.Name,
                source.Descripion);
        }

        public static CityEntity ToEntity(this City source)
        {
            return new CityEntity(
                source.Id,
                source.UserId,
                source.Name,
                source.Descripion);
        }
    }
}
