using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies.Slots
{
    public class ColonyMiningSlots : ColonySlot
    {
        public override ColonySlotType Type => ColonySlotType.Mining;

        public override int GetTotal(ColonyState colonyState) => 12;

        public override int GetUsed(ColonyState colonyState)
        {
            return colonyState.Industries[ColonyIndustryType.Mining].Total;
        }

        internal override void AddTotal(int delta) => throw new YagoException("Недотупно для изменения.");
    }
}
