namespace YAGO.World.Domain.GameEvents
{
    public class ColonyParameter
    {
        public ColonyParameterType Type { get; }
        public double Value { get; }

        public ColonyParameter(
            ColonyParameterType type,
            double value)
        {
            Type = type;
            Value = value;
        }
    }
}
