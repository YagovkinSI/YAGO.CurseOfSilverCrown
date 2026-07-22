using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class RequirementsParameter
    {
        public string Name { get; }
        public double Threshold { get; }
        public bool IsTopThreshold { get; }

        public RequirementsParameter(
            string name,
            double threshold,
            bool isTopThreshold = false)
        {
            Name = name;
            Threshold = threshold;
            IsTopThreshold = isTopThreshold;
        }

        public bool Check(ColonyStates colonyStats)
        {
            var parameterValue = colonyStats.GetGameParameter(Name);
            return IsTopThreshold
                ? parameterValue <= Threshold
                : parameterValue >= Threshold;
        }

        public static RequirementsParameter Cost(int solars)
        {
            return new RequirementsParameter(
                    StateKeys.Solars.Reserve, solars);
        }

        public static RequirementsParameter ActionPoints(int actionPoints)
        {
            return new RequirementsParameter(
                    StateKeys.ReformPoints.Reserve, actionPoints);
        }

        public static RequirementsParameter Zones(int zones)
        {
            return new RequirementsParameter(
                    StateKeys.Modules.Free, zones);
        }
    }
}
