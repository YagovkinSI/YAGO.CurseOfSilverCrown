using YAGO.World.Domain.Colonies;

namespace YAGO.World.Host.Controllers.Units
{
    public record UnitDetails(
        long Id,
        string Name,
        int Cost,
        int ZonesOccupied,
        int SolarsIncome,
        GavernorType GavernorType,
        int Population,
        string[] Text,
        string[] Description);
}
