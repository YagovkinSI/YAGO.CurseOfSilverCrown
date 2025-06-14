using YAGO.World.Host.Models;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Helpers;

namespace YAGO.World.Host.Extensions
{
    public static class FactionOnListExtensions
    {
        public static FactionOnList ToApi(this Organization source)
        {
            var fortCoef = FortificationsHelper.GetFortCoef(source.Fortifications);

            return new FactionOnList(
                source.Id,
                source.Name,
                source.WarriorCount,
                source.Gold,
                source.Investments,
                fortCoef,
                source.Suzerain?.Name,
                source.Vassals.Count,
                source.User?.UserName
            );
        }
    }
}
