using YAGO.World.Domain.Entities.Colonies.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ColonyMining : ColonyIndustry
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Mining;

        public ColonyMining(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override Building GetBuilding(bool isPrivate, BuildingContext buildingContext)
        {
            return new BuildingMining(
                isPrivate,
                buildingContext);
        }
    }
}
