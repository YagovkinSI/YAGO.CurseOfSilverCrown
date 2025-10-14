using YAGO.World.Domain.Colonies;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            return new Colony(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                source.SolarsIncome,
                source.Reputation,
                source.Population,
                source.ZonesOccupied,
                source.ZonesTotal);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            return new ColonyEntity(
                source.Id,
                source.UserId,
                source.Name,
                source.Solars,
                source.SolarsIncome,
                source.Reputation,
                source.Population,
                source.ZonesOccupied,
                source.ZonesTotal);
        }
    }
}
