using System.Linq;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ColonyStats.Parameters
{
    public class AreaCapacity
    {
        public int Total { get; private set; }
        public int Occupied { get; private set; }
        public int Available { get; private set; }

        public AreaCapacity(Colony colony)
        {
            Total = colony.Zones;
            Occupied = colony.Industries.Sum(x => x.ZonesOccupied) + 20;
            Available = Total - Occupied;
        }
    }
}
