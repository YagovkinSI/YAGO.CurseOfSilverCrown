using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ValueTypes.States
{
    public interface IState
    {
        StateKey Key { get; }
        double GetValue(ColonyState colonyStats);

        double MinValue { get; }
        double MaxValue { get; }

        bool IsLessThan(double value);
    }
}
