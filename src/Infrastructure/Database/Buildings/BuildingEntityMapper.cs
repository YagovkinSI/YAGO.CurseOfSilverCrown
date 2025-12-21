using YAGO.World.Domain.Buildings;

namespace YAGO.World.Infrastructure.Database.Buildings
{
    internal static class BuildingEntityMapper
    {
        public static Building ToDomain(this BuildingEntity source)
        {
            return new Building(
                source.Id,
                source.Name,
                source.Cost,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Stability,
                source.Population,
                source.Description);
        }

        public static BuildingEntity ToEntity(this Building source)
        {
            return new BuildingEntity(
                source.Id,
                source.Name,
                source.Cost,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Stability,
                source.Population,
                source.Description);
        }
    }
}
