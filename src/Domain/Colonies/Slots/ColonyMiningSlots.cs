using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Industries;

namespace YAGO.World.Domain.Colonies.Slots
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
