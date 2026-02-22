using System;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Mood
    {
        public double Total { get; private set; }

        public Mood(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            var moodTotal = 60.0;

            moodTotal += colony.FestivalEffect;

            Total = Math.Clamp(moodTotal, 2, 98);
        }

        internal static double CalculateReduction(Population population, CodeOfLaws codeOfLaws)
        {
            var codeOfLawsCoef = 1 + ((int)codeOfLaws - 2) / 5.0;
            return -population.Total * 0.02 * codeOfLawsCoef;
        }
    }
}
