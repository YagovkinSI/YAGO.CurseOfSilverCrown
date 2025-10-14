namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        decimal Reputation,
        int Population)
        : ColonySummary(
            Id,
            Name,
            Reputation,
            Population);
}
