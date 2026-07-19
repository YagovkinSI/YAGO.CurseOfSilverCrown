using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ProductionIndustry : BaseIndustry
    {
        public override Building Building { get; protected set; }

        public ProductionIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount,
            Building building)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
            Building = building;
        }

        public static ProductionIndustry CreateNew()
        {
            var building = BuildingDataset.GetProduction();

            return new ProductionIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0,
                building);
        }
    }
}
