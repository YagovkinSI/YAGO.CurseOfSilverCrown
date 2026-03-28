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
                IndustryNameConstants.Minning => new MinningIndustry(
                    source.CompanyCount,
                    source.ZonesOccupied,
                    source.SolarsIncome,
                    source.Population),
                IndustryNameConstants.Production => new ProductionIndustry(
                    source.CompanyCount,
                    source.ZonesOccupied,
                    source.SolarsIncome,
                    source.Population),
                IndustryNameConstants.Service => new ServiceIndustry(
                    source.CompanyCount,
                    source.ZonesOccupied,
                    source.SolarsIncome,
                    source.Population),
                _ => throw new YagoUnknownTypeException(nameof(source.Name)),
            };
        }

        public static IndustryEntity ToEntity(this BaseIndustry source)
        {
            string? name = null;
            name = source switch
            {
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
    }
}
