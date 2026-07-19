using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class MinningIndustry : BaseIndustry
    {
        private const int MaxUnitCount = 12;

        public override Building Building { get; protected set; }
        public int UnitAvailable => MaxUnitCount - BuildingCount;

        public MinningIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount,
            Building building)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
            Building = building;
        }

        public static MinningIndustry CreateNew()
        {
            var building = BuildingDataset.GetMining();

            return new MinningIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0,
                building);
        }
    }
}
