namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        decimal SolarsIncome,
        decimal Stability,
        int Population,
        int ZonesOccupied)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
