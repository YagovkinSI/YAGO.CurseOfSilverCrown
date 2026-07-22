using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ValueTypes.States
{
    public interface IState
    {
        double MinValue { get; }
        double MaxValue { get; }
        string Key { get; }

        double GetValue(ColonyState colonyStats);

        bool IsLessThan(double value);
    }
}
