namespace YAGO.World.Domain.GameEvents
{
    public class KeyValueParameter
    {
        public StateKey Name { get; }
        public double Value { get; }

        public KeyValueParameter(
            StateKey name,
            double value)
        {
            Name = name;
            Value = value;
        }
    }
}
