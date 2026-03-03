using System;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Mood
    {
        public double Total { get; private set; }

        public Mood(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            var moodTotal = 52.0;

            var colonyStats = colony.Stats;
            moodTotal += colonyStats.FestivalEffect;

            Total = Math.Clamp(moodTotal, 2, 98);
        }

        internal static double CalculateReduction(Population population, CodeOfLaws codeOfLaws)
        {
            var codeOfLawsCoef = 1 + ((int)codeOfLaws - 2) / 5.0;
            return -population.Total * 0.01 * codeOfLawsCoef;
        }
    }
}
