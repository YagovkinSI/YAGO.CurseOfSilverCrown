using System;

namespace YAGO.World.Domain.Entities.Colonies.Resources
{
    public abstract class ColonyResource
    {
        public abstract ColonyResourceType Type { get; }
        public double Value { get; private set; }
        public abstract double MinValue { get; }
        public abstract double MaxValue { get; }

        protected ColonyResource(
            double value)
        {
            Value = value;
        }

        public abstract double GetDeltaPerTurn(ColonyState colonyState);

        internal void Add(double delta)
        {
            Value += delta;
        }

        internal void NextTurn(ColonyState colonyState)
        {
            Value += GetDeltaPerTurn(colonyState);
        }
    }
}
