using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameEvents
{
    public class RequirementsParameter
    {
        public StateKey Name { get; }
        public double Threshold { get; }
        public bool IsTopThreshold { get; }

        public RequirementsParameter(
            StateKey name,
            double threshold,
            bool isTopThreshold = false)
        {
            Name = name;
            Threshold = threshold;
            IsTopThreshold = isTopThreshold;
        }

        public bool Check(ColonyState colonyStats)
        {
            var parameterValue = colonyStats.GetValue(Name);
            return IsTopThreshold
                ? parameterValue <= Threshold
                : parameterValue >= Threshold;
        }

        public static RequirementsParameter Cost(int solars)
        {
            return new RequirementsParameter(
                    StateKey.SolarsCurrent, solars);
        }

        public static RequirementsParameter ActionPoints(int actionPoints)
        {
            return new RequirementsParameter(
                    StateKey.ActionPointsCurrent, actionPoints);
        }

        public static RequirementsParameter Zones(int zones)
        {
            return new RequirementsParameter(
                    StateKey.ModulesFree, zones);
        }
    }
}
