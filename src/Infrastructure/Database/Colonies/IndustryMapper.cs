using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class IndustryMapper
    {
        public static Industry ToDomain(this IndustryEntity source)
        {
            return new Industry(
                source.Name,
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        public static IndustryEntity ToEntity(this Industry source)
        {
            return new IndustryEntity(
                source.Name,
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }
    }
}
