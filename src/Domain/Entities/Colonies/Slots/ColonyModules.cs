using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Slots
{
    public class ColonyModules : ColonySlot
    {
        public override ColonySlotType Type => ColonySlotType.Modules;

        public ColonyModules(int total) : base(total)
        {
        }

        public override int GetUsed(ColonyState colonyState)
        {
            var result = 0;
            foreach (var industryType in ColonyState.IndustryTypes)
            {
                var building = BuildingDataset.GetByType(industryType);
                var privateBuildingCount = colonyState.GetBuildCount(industryType, isPrivate: true);
                var stateOwnedBuildingCount = colonyState.GetBuildCount(industryType, isPrivate: false);
                var buildingCount = privateBuildingCount + stateOwnedBuildingCount;
                result += buildingCount * building.ZonesOccupied;
            }
            return result;
        }
    }
}
