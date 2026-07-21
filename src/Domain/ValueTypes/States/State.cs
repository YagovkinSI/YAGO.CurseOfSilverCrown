using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ValueTypes.States
{
    public class State : IState
    {
        public string Key { get; }
        public double Value { get; protected set; }

        public State(string key, double value)
        {
            Key = key;
            Value = value;
        }

        public double GetValue(ColonyStats colonyStats) => Value;

        public bool IsLessThan(double value) => Value < value;

        public bool IsMoreThan(double value) => Value > value;
    }
}
