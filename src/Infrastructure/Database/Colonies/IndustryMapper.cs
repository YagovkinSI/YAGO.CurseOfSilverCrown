using YAGO.World.Domain.Entities.Colonies.Industries;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class IndustryMapper
    {
        public static IndustryEntity ToEntity(this BaseIndustry source)
        {
            return new IndustryEntity(
                source.UnitCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        public static AdministrativeIndustry ToAdministrativeIndustry(this IndustryEntity source)
        {
            return new AdministrativeIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        public static MinningIndustry ToMinningIndustry(this IndustryEntity source)
        {
            return new MinningIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        public static ProductionIndustry ToProductionIndustry(this IndustryEntity source)
        {
            return new ProductionIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        public static ServiceIndustry ToServiceIndustry(this IndustryEntity source)
        {
            return new ServiceIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }
    }
}
