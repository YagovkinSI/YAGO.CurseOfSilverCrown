namespace YAGO.World.Domain.ValueTypes.States
{
    public class MutableState : State, IMutableState
    {
        public MutableState(
            string key,
            double initialValue)
            : base(key, initialValue)
        {
        }

        public void Add(double delta)
        {
            Value += delta;
        }

        public void Set(double value)
        {
            Value = value;
        }
    }
}
