using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameParameters;

namespace YAGO.World.Domain.GameActions
{
    public class GameActionChance
    {
        public IReadOnlyList<GameRequirement> Requirements { get; }
        public double ChanceDefault { get; }
        public IReadOnlyList<GameParameter> ChanceModifiers { get; }

        public GameActionChance(
            IReadOnlyList<GameRequirement> requirements,
            double chanceDefault,
            IReadOnlyList<GameParameter> chanceModifiers)
        {
            Requirements = requirements;
            ChanceDefault = chanceDefault;
            ChanceModifiers = chanceModifiers;
        }

        public double ChanceCalculate(Colony colony)
        {
            foreach (var requirement in Requirements)
            {
                if (!requirement.Check(colony.State))
                    return 0;
            }

            var finalChance = ChanceDefault;
            foreach (var modifier in ChanceModifiers)
            {
                var parameterValue = colony.GetValue(modifier.ParameterType);
                finalChance += modifier.Value * parameterValue;
            }

            return finalChance;
        }
    }
}
