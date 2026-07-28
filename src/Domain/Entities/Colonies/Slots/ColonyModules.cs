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
            foreach (var building in colonyState.Buildings.Values)
            {
                var buildingSettings = building.GetSettings();
                var buildingCount = building.Total;
                result += buildingCount * buildingSettings.ZonesOccupied;
            }
            return result;
        }
    }
}
