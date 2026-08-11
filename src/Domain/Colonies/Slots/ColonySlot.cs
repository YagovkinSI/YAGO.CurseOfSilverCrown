using System.Collections.Generic;

namespace YAGO.World.Domain.Colonies.Slots
{
    public abstract class ColonySlot
    {
        public abstract ColonySlotType Type { get; }

        public abstract int GetTotal(ColonyState colonyState);
        public abstract int GetUsed(ColonyState colonyState);
        public int GetFree(ColonyState colonyState)
        {
            return GetTotal(colonyState) - GetUsed(colonyState);
        }

        internal abstract void AddTotal(int delta);

        internal static List<ColonySlot> CreateNew()
        {
            return
            [
                new ColonyModules(),
                new ColonyMiningSlots(),
            ];
        }
    }
}
