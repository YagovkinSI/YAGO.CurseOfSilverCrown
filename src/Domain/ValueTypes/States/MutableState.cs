using System;

namespace YAGO.World.Domain.ValueTypes.States
{
    public class MutableState : State, IMutableState
    {

        public MutableState(
            string key,
            double initialValue,
            double minValue = double.MinValue,
            double maxValue = double.MaxValue)
            : base(key, initialValue, minValue, maxValue)
        {
        }

        public void Add(double delta)
        {
            var newValue = Value + delta;
            Value = Math.Clamp(newValue, MinValue, MaxValue);
        }

        public void Set(double value)
        {
            Value = Math.Clamp(value, MinValue, MaxValue);
        }
    }
}
