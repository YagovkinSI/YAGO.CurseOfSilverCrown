namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyReform
    {
        public ColonyReformType Type { get; }
        public double Value { get; private set; }

        public ColonyReform(
            ColonyReformType type,
            double value)
        {
            Type = type;
            Value = value;
        }

        internal void Add(double delta)
        {
            Value += delta;
        }
    }
}
