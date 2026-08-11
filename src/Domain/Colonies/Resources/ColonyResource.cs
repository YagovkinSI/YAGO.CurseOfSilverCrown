using System.Numerics;

namespace YAGO.World.Domain.Colonies.Resources
{
    public abstract class ColonyResource<T>
        where T : INumber<T>
    {
        public T Value { get; private set; }
        public abstract T MinValue { get; }
        public abstract T MaxValue { get; }

        protected ColonyResource(
            T value)
        {
            Value = Clamp(value);
        }

        internal void Add(T delta)
        {
            var newValue = Value + delta;
            Value = Clamp(newValue);
        }

        private T Clamp(T value)
        {
            if (value < MinValue) return MinValue;
            if (value > MaxValue) return MaxValue;
            return value;
        }
    }
}
