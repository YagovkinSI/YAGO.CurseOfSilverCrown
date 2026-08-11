using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.ValueTypes.States
{
    public interface IState
    {
        StateKey Key { get; }
        double GetValue(ColonyState colonyState);

        double MinValue { get; }
        double MaxValue { get; }

        bool IsLessThan(double value, ColonyState colonyState);
    }
}
