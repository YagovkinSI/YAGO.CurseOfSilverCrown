using YAGO.World.Domain.Entities.Colonies.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ColonyProduction : ColonyIndustry
    {
        public override ColonyIndustryType Type => ColonyIndustryType.Production;

        public ColonyProduction(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override Building GetBuilding(bool isPrivate, BuildingContext buildingContext)
        {
            return new BuildingProduction(
                isPrivate,
                buildingContext);
        }
    }
}
