using YAGO.World.Domain.Entities.Colonies.Buildings;

namespace YAGO.World.Domain.Entities.Colonies.Slots
{
    public class ColonyMiningSlots : ColonySlot
    {
        public override ColonySlotType Type => ColonySlotType.Mining;

        public ColonyMiningSlots(int total) : base(total)
        {
        }

        public override int GetUsed(ColonyState colonyState)
        {
            var buildingCount = colonyState.Buildings[ColonyBuildingType.Mining].Total;
            return Total - buildingCount;
        }
    }
}
