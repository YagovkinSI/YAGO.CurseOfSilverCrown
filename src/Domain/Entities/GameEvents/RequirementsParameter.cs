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

        public static RequirementsParameter Cost(int solars)
        {
            return new RequirementsParameter(
                    ColonyStatNames.Economic_Reserves, solars);
        }

        public static RequirementsParameter ActionPoints(int actionPoints)
        {
            return new RequirementsParameter(
                    ColonyStatNames.ActionPoints_Resourses, actionPoints);
        }

        public static RequirementsParameter Zones(int zones)
        {
            return new RequirementsParameter(
                    ColonyStatNames.AreaCapacity_Available, zones);
        }
    }
}
