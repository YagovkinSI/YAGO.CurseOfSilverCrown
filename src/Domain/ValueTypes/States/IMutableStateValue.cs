namespace YAGO.World.Domain.ValueTypes.States
{
    public interface IMutableState : IState
    {
        double Value { get; }
        void Add(double delta);
        void Set(double value);
    }
}
