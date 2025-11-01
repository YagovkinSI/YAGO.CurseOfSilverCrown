using YAGO.World.Domain.Buildings;

namespace YAGO.World.Host.Controllers.Buildings
{
    public static class BuildingResponseMapping
    {
        public static BuildingDetails ToMyDataResponse(
            this Building source)
        {
            return new BuildingDetails(
                source.Id,
                source.Name,
                source.Cost,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Reputation,
                source.Population,
                source.Description);
        }
    }
}
