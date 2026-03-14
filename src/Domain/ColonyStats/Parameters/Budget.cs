using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Ships;

namespace YAGO.World.Domain.ColonyStats.Parameters
{
    public class Budget
    {
        public double Balance { get; private set; }

        public Budget(Colony colony, ColonyCompanies companies, Ship ship)
        {
            colony.ValidateShip(ship);
            colony.ValidateContracts(companies);

            Balance = companies.Companies.Sum(x => x.SolarsIncome) - ship.Maintenance;
        }
    }
}
