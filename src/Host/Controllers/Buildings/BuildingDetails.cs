namespace YAGO.World.Host.Controllers.Buildings
{
    public record BuildingDetails(
        long Id,
        string Name,
        int Cost,
        int ZonesOccupied,
        int SolarsIncome,
        int Challenges,
        int Population,
        string[] Description);
}
