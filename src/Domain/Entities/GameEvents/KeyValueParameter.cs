namespace YAGO.World.Domain.Entities.GameEvents
{
    public class KeyValueParameter
    {
        public string Name { get; }
        public double Value { get; }

        public KeyValueParameter(
            string name,
            double value)
        {
            Name = name;
            Value = value;
        }
    }
}
