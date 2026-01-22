using YAGO.World.Domain.Colonies;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record MyColony(
        long Id,
        long UserId,
        string Name,
        int Solars,
        int SolarsIncome,
        double GavernorType,
        int Population,
        int ZonesOccupied,
        int ZonesTotal,
        GavernorType codeOfLaws)
        : ColonyDetails(
            Id,
            UserId,
            Name,
            SolarsIncome,
            GavernorType,
            Population,
            ZonesOccupied);
}

