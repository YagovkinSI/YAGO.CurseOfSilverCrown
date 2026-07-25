using System;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyResource
    {
        public ColonyResourceType Type { get; }
        public double Value { get; private set; }
        public double MinValue { get; }
        public double MaxValue { get; }

        public ColonyResource(
            ColonyResourceType type, 
            double value, 
            double minValue = double.MinValue, 
            double maxValue = double.MaxValue)
        {
            Type = type;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        internal void Add(double delta)
        {
            Value += delta;
        }
    }
}
