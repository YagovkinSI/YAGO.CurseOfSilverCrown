using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Mappings;

namespace YAGO.World.Domain.Colonies.Slots
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
            var buildingContext = colonyState.GetBuildingContext();
            foreach (var industry in colonyState.Industries.Values)
            {
                for (var i = 0; i < 2; i++)
                {
                    var isPrivate = i == 1;
                    var building = industry.GetBuilding(isPrivate, buildingContext);
                    var buildingCount = isPrivate ? industry.PrivateCount : industry.StateCount;
                    result += buildingCount * building.ModulesUsed;
                }
            }
            return result;
        }
    }
}
