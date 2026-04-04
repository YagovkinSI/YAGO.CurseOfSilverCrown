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

        public bool Check(ColonyStats colonyStats)
        {
            var parameterValue = colonyStats.GetGameParameter(Name);
            return IsTopThreshold
                ? parameterValue <= Threshold
                : parameterValue >= Threshold;
        }
    }
}
