using System.Linq;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ColonyStats.Parameters
{
    public class Budget
    {
        public double Balance { get; private set; }

        public Budget(Colony colony)
        {
            Balance = colony.Industries.Sum(x => x.SolarsIncome) - colony.Maintenance;
        }
    }
}
