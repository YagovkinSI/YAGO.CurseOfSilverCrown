namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonySummary(
        long Id,
        string Name,
        decimal Reputation,
        int Population);
}
