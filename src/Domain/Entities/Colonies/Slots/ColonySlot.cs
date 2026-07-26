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
        public int GetFree(ColonyState colonyState) => Total - GetUsed(colonyState);

        internal void AddTotal(int delta)
        {
            Total += delta;
        }
    }
}
