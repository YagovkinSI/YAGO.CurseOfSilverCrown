using System.Linq;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ColonyStats.Parameters
{
    public class Budget
    {
        public double Balance { get; private set; }

        public Budget(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            Balance = companies.Companies.Sum(x => x.SolarsIncome) - colony.Maintenance;
        }
    }
}
