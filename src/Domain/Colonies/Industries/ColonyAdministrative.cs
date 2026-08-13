using YAGO.World.Domain.Colonies.Buildings;

namespace YAGO.World.Domain.Colonies.Industries
{
    public class ColonyAdministrative : ColonyIndustry
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Administrative;

        public ColonyAdministrative(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override Building GetBuilding(bool isPrivate, BuildingContext buildingContext)
        {
            return new BuildingAdministrative(
                isPrivate,
                buildingContext);
        }
    }
}
