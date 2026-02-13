using YAGO.World.Domain.Companies;

namespace YAGO.World.Host.Controllers.Units
{
    public static class UnitResponseMapping
    {
        public static UnitDetails ToMyDataResponse(
            this Company source)
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
