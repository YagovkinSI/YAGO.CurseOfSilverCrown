namespace YAGO.World.Host.Controllers.Colonies
{
    public record ColonyDetails(
        long Id,
        long UserId,
        string Name,
        int SolarsIncome,
        double GavernorType,
        int Population,
        int ZonesOccupied)
        : ColonySummary(
            Id,
            UserId,
            Name);
}
