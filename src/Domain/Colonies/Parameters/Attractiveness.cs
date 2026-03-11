using System;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Attractiveness
    {
        public double Total { get; private set; }

        public Attractiveness(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            var policies = colony.Policies;

            var defaultValue = 100;
            var taxEffect = -30 * ((int)policies.CodeOfLaws);
            var standartsEffect = -30 * (3 - (int)policies.CodeOfLaws);
            var stabilityEffect = Math.Min(50, colony.Stats.CurrentWeek / 10.0);

            Total = Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }
    }
}
