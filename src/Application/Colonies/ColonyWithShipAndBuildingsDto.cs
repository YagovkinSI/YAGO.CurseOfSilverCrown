using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Application.Colonies
{
    public record ColonyWithShipAndBuildingsDto(
        Colony Colony,
        Ship Ship,
        Building[] Buildings,
        decimal SolarIncome,
        decimal Reputation,
        int Population,
        int ZonesOccupied);
}
