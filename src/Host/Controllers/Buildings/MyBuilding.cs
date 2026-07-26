namespace YAGO.World.Host.Controllers.Buildings
{
    public record MyBuilding(
        string Name,
        string ImageName,
        string[] Description,
        MyBuildingPrivate Private,
        MyBuildingState State);

    public record MyBuildingPrivate(
        int count,
        bool buildAvailable,
        string? unavailabilityReason,
        double cost) 
        : MyBuildingBase(
            IsPrivate: true,
            count, 
            buildAvailable, 
            unavailabilityReason, 
            cost);


    public record MyBuildingState(
        int count,
        bool buildAvailable,
        string? unavailabilityReason,
        double cost)
        : MyBuildingBase(
            IsPrivate: false,
            count,
            buildAvailable,
            unavailabilityReason,
            cost);

    public record MyBuildingBase(
        bool IsPrivate,
        int Count,
        bool BuildAvailable,
        string? UnavailabilityReason,
        double Cost);
}
