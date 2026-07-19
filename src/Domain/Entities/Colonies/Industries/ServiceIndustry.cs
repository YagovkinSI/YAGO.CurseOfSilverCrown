using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class ServiceIndustry : BaseIndustry
    {
        public override Building Building { get; protected set; }

        public ServiceIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount,
            Building building)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
            Building = building;
        }

        public static ServiceIndustry CreateNew()
        {
            var building = BuildingDataset.GetService();

            return new ServiceIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0,
                building);
        }

        internal double NeedCalculation(int populationTotal)
        {
            return (populationTotal / 50.0) - BuildingCount - 1.5;
        }
    }
}
