namespace YAGO.World.Host.Controllers.Buildings
{
    public record BuildingDetails(
        long Id,
        string Name,
        decimal Cost,
        int ZonesOccupied,
        decimal SolarsIncome,
        decimal Stability,
        int Population,
        string[] Description);
}
