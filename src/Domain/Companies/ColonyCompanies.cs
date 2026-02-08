using System.Collections.Generic;
using System.Linq;

namespace YAGO.World.Domain.Companies
{
    public class ColonyCompanies
    {
        public IReadOnlyList<Company> Companies { get; private set; }

        public ColonyCompanies(
            IReadOnlyList<Company> companies) 
        {
            Companies = companies;
        }

        internal void AddCompany(Company company)
        {
            var list = Companies.ToList();
            list.Add(company);
            Companies = list;
        }

        internal void Update(ColonyCompanies newCompanies)
        {
            Companies = newCompanies.Companies;
        }
    }
}
