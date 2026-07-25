using System;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ValueTypes.States
{
    public class State : IState
    {
        private readonly Func<ColonyState, double> _valueCalculator;
        public StateKey Key { get; }
        public double MinValue { get; }
        public double MaxValue { get; }

        public State(
            StateKey key,
            Func<ColonyState, double> valueCalculator,
            double minValue,
            double maxValue)
        {
            Key = key;
            _valueCalculator = valueCalculator;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public double GetValue(ColonyState colonyState)
        {
            return _valueCalculator(colonyState);
        }

        public bool IsLessThan(double value, ColonyState colonyState)
        {
            return GetValue(colonyState) < value;
        }
    }
}
