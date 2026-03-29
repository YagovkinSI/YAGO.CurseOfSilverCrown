using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class IndustryMapper
    {
        public static BaseIndustry ToDomain(this IndustryEntity source)
        {
            return source.Name switch
            {
                IndustryNameConstants.Administrative => ToAdministrativeIndustry(source),
                IndustryNameConstants.Minning => ToMinningIndustry(source),
                IndustryNameConstants.Production => ToProductionIndustry(source),
                IndustryNameConstants.Service => ToServiceIndustry(source),
                _ => throw new YagoUnknownTypeException(nameof(source.Name)),
            };
        }

        public static IndustryEntity ToEntity(this BaseIndustry source)
        {
            string? name = null;
            name = source switch
            {
                AdministrativeIndustry => IndustryNameConstants.Administrative,
                MinningIndustry => IndustryNameConstants.Minning,
                ProductionIndustry => IndustryNameConstants.Production,
                ServiceIndustry => IndustryNameConstants.Service,
                _ => throw new YagoUnknownTypeException(nameof(BaseIndustry)),
            };

            return new IndustryEntity(
                name,
                source.UnitCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        private static AdministrativeIndustry ToAdministrativeIndustry(IndustryEntity source)
        {
            return new AdministrativeIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        private static MinningIndustry ToMinningIndustry(IndustryEntity source)
        {
            return new MinningIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        private static ProductionIndustry ToProductionIndustry(IndustryEntity source)
        {
            return new ProductionIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }

        private static ServiceIndustry ToServiceIndustry(IndustryEntity source)
        {
            return new ServiceIndustry(
                source.CompanyCount,
                source.ZonesOccupied,
                source.SolarsIncome,
                source.Population);
        }
    }
}
