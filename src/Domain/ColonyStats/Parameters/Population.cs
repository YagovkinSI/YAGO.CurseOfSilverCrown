using System.Linq;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ColonyStats.Parameters
{
    public class Population
    {
        public int Total { get; private set; }

        public Population(Colony colony)
        {
            Total = colony.Industries.Sum(x => x.Population) + 20;
        }
    }
}
