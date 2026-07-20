using YAGO.World.Domain.Entities.Colonies.Industries;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class IndustryMapper
    {
        public static IndustryEntity ToEntity(this BaseIndustry source)
        {
            return new IndustryEntity(
                source.PrivateBuildingCount,
                source.StateOwnedBuildingCount);
        }

        public static AdministrativeIndustry ToAdministrativeIndustry(this IndustryEntity source)
        {
            return new AdministrativeIndustry(
                source.PrivateBuildingCount,
                source.StateOwnedBuildingCount);
        }

        public static MinningIndustry ToMinningIndustry(this IndustryEntity source)
        {
            return new MinningIndustry(
                source.PrivateBuildingCount,
                source.StateOwnedBuildingCount);
        }

        public static ProductionIndustry ToProductionIndustry(this IndustryEntity source)
        {
            return new ProductionIndustry(
                source.PrivateBuildingCount,
                source.StateOwnedBuildingCount);
        }

        public static ServiceIndustry ToServiceIndustry(this IndustryEntity source)
        {
            return new ServiceIndustry(
                source.PrivateBuildingCount,
                source.StateOwnedBuildingCount);
        }
    }
}
