using YAGO.World.Domain.Entities.Buildings;

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
            var buildingCount = colonyState.Industries[IndustryType.Mining].Total;
            return Total - buildingCount;
        }
    }
}
