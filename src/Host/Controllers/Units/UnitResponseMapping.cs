using YAGO.World.Domain.Contracts;

namespace YAGO.World.Host.Controllers.Units
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
