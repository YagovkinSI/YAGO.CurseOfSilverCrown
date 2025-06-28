using YAGO.World.Domain.Units;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Database.Models.Units.Extensions
{
    public static class UnitWithFactionExtensions
    {
        public static UnitWithFaction ToUnitWithFaction(this Unit source)
        {
            var faction = source.Domain.ToDomain();

            return new UnitWithFaction(
                source.Id,
                faction
            );
        }
    }
}
