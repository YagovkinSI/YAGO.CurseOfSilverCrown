using System.Collections.Generic;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameActions
{
    public class GameActionChance
    {
        public IReadOnlyList<GameRequirement> Requirements { get; }
        public double ChanceDefault { get; }
        public IReadOnlyList<ProbabilityModifier> ChanceModifiers { get; }

        public GameActionChance(
            IReadOnlyList<GameRequirement> requirements,
            double chanceDefault,
            IReadOnlyList<ProbabilityModifier> chanceModifiers)
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
                var success = modifier.Condition.Check(colony.State);
                if (!success)
                    continue;
                finalChance = UseModifier(modifier, finalChance);
            }

            return finalChance;
        }

        private double UseModifier(ProbabilityModifier modifier, double finalChance)
        {
            return modifier.Type switch
            {
                ProbabilityModifierType.Multiplicative => finalChance * modifier.Value,
                ProbabilityModifierType.Additive => finalChance + modifier.Value,
            };
        }
    }
}
