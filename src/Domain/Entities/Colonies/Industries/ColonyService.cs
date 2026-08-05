using YAGO.World.Domain.Entities.Colonies.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
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
