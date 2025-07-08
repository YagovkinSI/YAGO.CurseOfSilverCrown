using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Database.Models.Domains
{
    public static class OrganizationExtensions
    {
        public static Domain.Provinces.ProvinceWithUser ToProvinceWithUser(this Organization source)
        {
            if (source == null)
                return null;

            var province = new Domain.Provinces.Province(source.Id);
            var faction = source.ToDomain();
            var user = source.User?.ToDomain();

            return new Domain.Provinces.ProvinceWithUser(
                province,
                faction,
                user
            );
        }

        public static Domain.Factions.Faction ToDomain(this Organization source)
        {
            return source == null
                ? null
                : new Domain.Factions.Faction(
                source.Id,
                source.Name,
                source.Gold,
                source.Investments,
                source.Fortifications,
                source.Size,
                source.UserId,
                source.SuzerainId,
                source.TurnOfDefeat
            );
        }
    }
}
