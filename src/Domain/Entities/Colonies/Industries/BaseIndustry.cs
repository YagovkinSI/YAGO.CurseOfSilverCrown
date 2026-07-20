using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Industries
{
    public abstract class BaseIndustry : IIndustry
    {
        public int PrivateBuildingCount { get; protected set; }
        public int StateOwnedBuildingCount { get; protected set; }
        public abstract IndustryType Type { get; }

        public int BuildingCount => PrivateBuildingCount + StateOwnedBuildingCount;

        protected BaseIndustry(
            int privateBuildingCount,
            int stateOwnedBuildingCount)
        {
            PrivateBuildingCount = privateBuildingCount;
            StateOwnedBuildingCount = stateOwnedBuildingCount;
        }

        internal void AddPrivateBuilding(int count)
        {
            PrivateBuildingCount += count;
        }

        internal void AddStateOwnedBuilding(int count)
        {
            StateOwnedBuildingCount += count;
        }
    }
}
