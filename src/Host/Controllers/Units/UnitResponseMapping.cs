using YAGO.World.Domain.Units;
using YAGO.World.Host.Controllers.Units;

namespace YAGO.World.Host.Controllers.Buildings
{
    public static class UnitResponseMapping
    {
        public static UnitDetails ToMyDataResponse(
            this Contract source)
        {
            return new UnitDetails(
                source.Id,
                source.Name,
                source.Cost,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.GavernorType,
                source.Population,
                source.Text,
                source.Description);
        }
    }
}
