using System;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ColonyStats.Parameters
{
    public class Mood
    {
        public double Total { get; private set; }

        public Mood(Colony colony)
        {
            var moodTotal = 52.0;
            moodTotal += colony.FestivalEffect;
            Total = Math.Clamp(moodTotal, 2, 98);
        }

        internal static double CalculateReduction(Population population, CodeOfLaws codeOfLaws)
        {
            var codeOfLawsCoef = 1 + (((int)codeOfLaws - 2) / 5.0);
            return -population.Total * 0.01 * codeOfLawsCoef;
        }
    }
}
