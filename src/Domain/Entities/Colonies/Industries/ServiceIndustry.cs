using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ServiceIndustry : BaseIndustry
    {
        public override IndustryType Type => IndustryType.Service;

        public ServiceIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
        }

        public static ServiceIndustry CreateNew()
        {
            return new ServiceIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0);
        }

        internal double NeedCalculation(int populationTotal)
        {
            return (populationTotal / 50.0) - BuildingCount - 1.5;
        }
    }
}
