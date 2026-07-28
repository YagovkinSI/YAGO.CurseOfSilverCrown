using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Colonies.Slots
{
    public abstract class ColonySlot
    {
        public abstract ColonySlotType Type { get; }
        public int Total { get; private set; }

        protected ColonySlot(int total)
        {
            Total = total;
        }

        public abstract int GetUsed(ColonyState colonyState);
        public int GetFree(ColonyState colonyState)
        {
            return Total - GetUsed(colonyState);
        }

        internal void AddTotal(int delta)
        {
            Total += delta;
        }

        internal static List<ColonySlot> CreateNew()
        {
            return
            [
                new ColonyModules(total: 140),
                new ColonyMiningSlots(total: 12),
            ];
        }
    }
}
