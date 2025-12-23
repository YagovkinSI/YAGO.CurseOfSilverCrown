namespace YAGO.World.Host.Controllers.Colonies
{
    public record MyColony(
        long Id,
        long UserId,
        string Name,
        int Solars,
        int SolarsIncome,
        int Challenges,
        int Population,
        int ZonesOccupied,
        int ZonesTotal)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            SolarsIncome,
            Challenges,
            Population,
            ZonesOccupied);
}

