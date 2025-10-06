namespace YAGO.World.Host.Controllers.MyUsers
{
    public record MyCity(
        long Id,
        long UserId,
        string Name,
        string Description)
        : CityDetails(
            Id,
            UserId,
            Name,
            Description);
}
