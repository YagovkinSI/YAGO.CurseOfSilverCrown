using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameActions
{
    public class GameActionChance
    {
        public IReadOnlyList<GameParameterRequirement> Requirements { get; }
        public double ChanceDefault { get; }
        public IReadOnlyList<GameParameterNumberValue> ChanceModifiers { get; }

        public GameActionChance(
            IReadOnlyList<GameParameterRequirement> requirements,
            double chanceDefault,
            IReadOnlyList<GameParameterNumberValue> chanceModifiers)
        {
            Requirements = requirements;
            ChanceDefault = chanceDefault;
            ChanceModifiers = chanceModifiers;
        }

        public double ChanceCalculate(ColonyState colonyStats)
        {
            foreach (var requirement in Requirements)
            {
                if (!requirement.Check(colonyStats))
                    return 0;
            }

            var finalChance = ChanceDefault;
            foreach (var modifier in ChanceModifiers)
            {
                var parameterValue = colonyStats.GetValue(modifier.ParameterType);
                finalChance += modifier.Value * parameterValue;
            }

            return finalChance;
        }
    }
}
