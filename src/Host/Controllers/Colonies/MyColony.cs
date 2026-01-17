namespace YAGO.World.Host.Controllers.Colonies
{
    public record MyColony(
        long Id,
        long UserId,
        string Name,
        int Solars,
        int SolarsIncome,
        int GavernorType,
        int Population,
        int ZonesOccupied,
        int ZonesTotal)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            SolarsIncome,
            GavernorType,
            Population,
            ZonesOccupied);
}

