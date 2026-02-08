using System.Linq;
using YAGO.World.Domain.Companies;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Loyalty
    {
        public double Total { get; private set; }

        public Loyalty(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            var humanistWeight = 0;
            var codeOfLawsInfluence = CalcCodeOfLawsInfluence(colony);
            humanistWeight += codeOfLawsInfluence;

            humanistWeight += companies.Companies
                .Count(x => x.GavernorType == GavernorType.Humanist);
            humanistWeight -= companies.Companies
                .Count(x => x.GavernorType == GavernorType.Capitalist);

            var maxWeight = 10 + companies.Companies.Count;
            var weight = (double)humanistWeight / maxWeight;

            Total = 2 - weight;
        }

        private static int CalcCodeOfLawsInfluence(Colony colony)
        {
            return colony.CodeOfLaws switch
            {
                GavernorType.Humanist => 10,
                GavernorType.Capitalist => -10,
                _ => 0,
            };
        }
    }
}
