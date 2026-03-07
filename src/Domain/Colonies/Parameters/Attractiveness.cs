using System;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Attractiveness
    {
        public double Total { get; private set; }

        public Attractiveness(Colony colony, ColonyCompanies companies)
        {
            var defaultValue = 100;
            var taxEffect = -30 * ((int)colony.CodeOfLaws);
            var standartsEffect = -30 * (3 - (int)colony.CodeOfLaws);
            var stabilityEffect = Math.Min(100, colony.Stats.CurrentWeek / 5.0);

            Total = Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }
    }
}
