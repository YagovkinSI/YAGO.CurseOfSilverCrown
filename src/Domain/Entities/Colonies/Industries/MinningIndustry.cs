using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class MinningIndustry : BaseIndustry
    {
        private const int MaxUnitCount = 12;

        public override IndustryType Type => IndustryType.Mining;
        public int UnitAvailable => MaxUnitCount - BuildingCount;

        public MinningIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
        }

        public static MinningIndustry CreateNew()
        {
            return new MinningIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0);
        }
    }
}
