using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.ValueTypes.States
{
    public interface IState
    {
        string Key { get; }

        double GetValue(ColonyStats colonyStats);

        bool IsLessThan(double value);
    }
}
