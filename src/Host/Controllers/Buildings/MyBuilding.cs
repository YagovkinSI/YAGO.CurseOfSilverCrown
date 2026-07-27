namespace YAGO.World.Host.Controllers.Buildings
{
    public record MyBuilding(
        string Type,
        string Name,
        string ImageName,
        string[] Description,
        MyBuildingBase Private,
        MyBuildingBase State);


    public record MyBuildingBase(
        bool IsPrivate,
        int BuildingCount,
        bool BuildAvailable,
        string? UnavailabilityReason,
        double Cost);
}
