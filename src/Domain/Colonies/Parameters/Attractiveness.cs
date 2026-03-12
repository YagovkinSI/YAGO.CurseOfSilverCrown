using System;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Attractiveness
    {
        public double Total { get; private set; }

        public Attractiveness(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);
            var colonyStats = colony.Stats;

            var defaultValue = 100;
            var taxEffect = -30 * ((int)colonyStats.CodeOfLaws);
            var standartsEffect = -30 * (3 - (int)colonyStats.CodeOfLaws);
            var stabilityEffect = Math.Min(50, colony.Stats.CurrentWeek / 10.0);

            Total = Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }
    }
}
