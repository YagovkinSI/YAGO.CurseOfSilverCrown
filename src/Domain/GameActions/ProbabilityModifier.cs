namespace YAGO.World.Domain.GameActions
{
    public class ProbabilityModifier
    {
        public GameRequirement Condition { get; }
        public ProbabilityModifierType Type { get; }
        public double Value { get; }

        public ProbabilityModifier(
            GameRequirement condition,
            ProbabilityModifierType type,
            double value)
        {
            Condition = condition;
            Type = type;
            Value = value;
        }
    }
}
