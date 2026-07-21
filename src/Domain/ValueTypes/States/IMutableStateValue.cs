namespace YAGO.World.Domain.ValueTypes.States
{
    public interface IMutableState : IState
    {
        void Add(double delta);
        void Set(double value);
    }
}
