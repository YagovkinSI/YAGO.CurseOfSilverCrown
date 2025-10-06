namespace YAGO.World.Host.Controllers.MyUsers
{
    public record CityDetails (
        long Id,
        long UserId,
        string Name,
        string Description)
        : CitySummary(
            Id,
            UserId,
            Name);
}
