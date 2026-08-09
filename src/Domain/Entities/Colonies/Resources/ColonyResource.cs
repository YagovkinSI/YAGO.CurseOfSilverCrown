using System;
using System.Collections.Generic;

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
            var newValue = Value + delta;
            Value = Math.Clamp(newValue, MinValue, MaxValue);
        }

        internal void NextTurn(ColonyState colonyState)
        {
            var delta = GetDeltaPerTurn(colonyState);
            Add(delta);
        }

        internal static List<ColonyResource> CreateNew()
        {
            return
            [
                new ColonySolars(value: 0),
                new ColonyActionPoints(value: 2),
                new ColonyMood(value: 50),
                new ColonyTurns(value: 1),
            ];
        }
    }
}
