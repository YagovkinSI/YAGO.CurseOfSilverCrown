namespace YAGO.World.Host.Controllers.Colonies
{
    public record MyColony(
        long Id,
        long UserId,
        string Name,
        decimal Solars,
        decimal SolarsIncome,
        decimal Reputation,
        int Population,
        int ZonesOccupied,
        int ZonesTotal)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            Reputation,
            Population);
}

