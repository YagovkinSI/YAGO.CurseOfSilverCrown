using YAGO.World.Domain.Colonies.Buildings;

namespace YAGO.World.Domain.Colonies.Industries
{
    public class ColonyService : ColonyIndustry
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Service;

        public ColonyService(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override Building GetBuilding(bool isPrivate, BuildingContext buildingContext)
        {
            return new BuildingService(
                isPrivate,
                buildingContext);
        }
    }
}
