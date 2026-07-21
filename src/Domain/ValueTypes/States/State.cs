using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ValueTypes.States
{
    public class State : IState
    {
        public double MinValue { get; }
        public double MaxValue { get; }
        public string Key { get; }
        public double Value { get; protected set; }

        public State(
            string key, 
            double value,
            double minValue = double.MinValue,
            double maxValue = double.MaxValue)
        {
            Key = key;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public double GetValue(ColonyStats colonyStats) => Value;

        public bool IsLessThan(double value) => Value < value;

        public bool IsMoreThan(double value) => Value > value;
    }
}
