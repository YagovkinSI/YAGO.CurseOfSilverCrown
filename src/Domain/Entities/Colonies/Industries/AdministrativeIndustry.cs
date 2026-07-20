using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class AdministrativeIndustry : BaseIndustry
    {
        public override IndustryType Type => IndustryType.Administrative;

        public AdministrativeIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
        }

        public static AdministrativeIndustry CreateNew()
        {
            return new AdministrativeIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0);
        }
    }
}
