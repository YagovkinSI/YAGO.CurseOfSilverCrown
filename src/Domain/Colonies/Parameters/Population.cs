using System.Linq;
using YAGO.World.Domain.Companies;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Population
    {
        public int Total { get; private set; }

        public Population(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            Total = companies.Companies.Sum(x => x.Population);
        }
    }
}
