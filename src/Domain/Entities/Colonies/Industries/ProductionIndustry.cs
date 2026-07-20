using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ProductionIndustry : BaseIndustry
    {
        public override IndustryType Type => IndustryType.Production;

        public ProductionIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
        }

        public static ProductionIndustry CreateNew()
        {
            return new ProductionIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0);
        }
    }
}
