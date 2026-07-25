using System;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ValueTypes.States
{
    public class MutableState : IMutableState
    {
        public StateKey Key { get; }
        public double Value { get; private set; }
        public double MinValue { get; }
        public double MaxValue { get; }

        public MutableState(
            StateKey key,
            double value,
            double minValue = double.MinValue,
            double maxValue = double.MaxValue)
        {
            Key = key;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public double GetValue(ColonyState colonyStats) => Value;

        public bool IsLessThan(double value) => Value < value;

        public void Add(double delta)
        {
            var newValue = Value + delta;
            Set(newValue);
        }

        public void Set(double value)
        {
            Value = Math.Clamp(value, MinValue, MaxValue);
        }
    }
}
