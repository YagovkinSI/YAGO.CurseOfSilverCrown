using YAGO.World.Domain.Entities.Colonies.Industries;

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
            return colonyState.Industries[ColonyIndustryType.Mining].Total;
        }
    }
}
