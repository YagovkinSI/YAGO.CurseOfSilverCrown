using System;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.AI
{
    internal class AIPattern
    {
        internal double Risky { get; set; }
        internal double Peaceful { get; set; }
        internal double Loyalty { get; set; }

        internal AIPattern(int personId)
        {
            Risky = RandomHelper.DependentRandom(personId, 0);
            Peaceful = RandomHelper.DependentRandom(personId, 1);
            Loyalty = RandomHelper.DependentRandom(personId, 2);
        }

        internal double GetRisky() => CurrentParametr(Risky);
        internal double GetPeaceful() => CurrentParametr(Peaceful);
        internal double GetLoyalty() => CurrentParametr(Loyalty);

        private double CurrentParametr(double patternParameter) => (new Random().NextDouble() + patternParameter) / 2;
    }
}
