namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        int SolarsIncome,
        int GavernorType,
        int Population,
        int ZonesOccupied)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
