using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameActions
{
    public class GameParameterRequirement
    {
        public GameParameterType ParameterType { get; }
        public double Threshold { get; }
        public bool IsLessThan { get; }

        public GameParameterRequirement(
            GameParameterType name,
            double threshold,
            bool isLessThan = false)
        {
            ParameterType = name;
            Threshold = threshold;
            IsLessThan = isLessThan;
        }

        public bool Check(ColonyState colonyStats)
        {
            var parameterValue = colonyStats.GetValue(ParameterType);
            return IsLessThan
                ? parameterValue <= Threshold
                : parameterValue >= Threshold;
        }

        public static GameParameterRequirement Cost(int solars)
        {
            return new GameParameterRequirement(
                    GameParameterType.SolarsCurrent, solars);
        }

        public static GameParameterRequirement ActionPoints(int actionPoints)
        {
            return new GameParameterRequirement(
                    GameParameterType.ActionPointsCurrent, actionPoints);
        }

        public static GameParameterRequirement Modules(int modules)
        {
            return new GameParameterRequirement(
                    GameParameterType.ModulesFree, modules);
        }
    }
}
