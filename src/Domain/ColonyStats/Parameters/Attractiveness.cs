using System;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ColonyStats.Parameters
{
    public class Attractiveness
    {
        public double Total { get; private set; }

        public Attractiveness(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            var defaultValue = 100;
            var taxEffect = -30 * (int)colony.CodeOfLaws;
            var standartsEffect = -30 * (3 - (int)colony.CodeOfLaws);
            var stabilityEffect = Math.Min(50, colony.CurrentWeek / 10.0);

            Total = Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }
    }
}
