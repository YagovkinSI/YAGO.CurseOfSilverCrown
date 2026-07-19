using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public class AdministrativeIndustry : BaseIndustry
    {
        public override Building Building { get; protected set; }

        public AdministrativeIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount,
            Building building)
            : base(privateBuildingCount, stateOwnedBuildingCount)
        {
            Building = building;
        }

        public static AdministrativeIndustry CreateNew()
        {
            var building = BuildingDataset.GetAdministrative();

            return new AdministrativeIndustry(
                privateBuildingCount: 0,
                stateOwnedBuildingCount: 0,
                building);
        }
    }
}
