using System;
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
            foreach (var industryType in Enum.GetValues<IndustryType>())
            {
                var building = BuildingDataset.GetByType(industryType);
                var buildingCount = colonyState.Industries[industryType].Total;
                result += buildingCount * building.ZonesOccupied;
            }
            return result;
        }
    }
}
