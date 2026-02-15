using System;
using System.Linq;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Mood
    {
        public double Total { get; private set; }

        public Mood(Colony colony, ColonyCompanies companies, double festivalEffect)
        {
            colony.ValidateContracts(companies);

            var moodTotal = 60.0;

            var codeOfLawsInfluence = CalcCodeOfLawsInfluence(colony);
            moodTotal += codeOfLawsInfluence;

            moodTotal -= 2 * companies.Companies
                .Count(x => x.GavernorType == CodeOfLaws.Centrist);
            moodTotal -= 5 * companies.Companies
                .Count(x => x.GavernorType == CodeOfLaws.Capitalist);

            moodTotal += festivalEffect;

            Total = Math.Clamp(moodTotal, 2, 98);
        }

        private static int CalcCodeOfLawsInfluence(Colony colony)
        {
            return colony.CodeOfLaws switch
            {
                CodeOfLaws.Humanist => +10,
                CodeOfLaws.Capitalist => -10,
                _ => 0,
            };
        }
    }
}
