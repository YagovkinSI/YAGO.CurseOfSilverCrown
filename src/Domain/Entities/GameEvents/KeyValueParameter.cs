namespace YAGO.World.Domain.Entities.GameEvents
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
