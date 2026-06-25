using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class ActionAvailableRequirement
    {
        public RequirementsParameter Parameter { get; }
        public string Message { get; }

        public ActionAvailableRequirement(RequirementsParameter parameter, string message)
        {
            Parameter = parameter;
            Message = message;
        }

        public static ActionAvailableRequirement Cost(int solars)
        {
            return new ActionAvailableRequirement(
                new RequirementsParameter(
                    ColonyStatNames.Economic_Reserves, solars),
                    "Недостаточно Солар");
        }

        public static ActionAvailableRequirement ActionPoints(int actionPoints)
        {
            return new ActionAvailableRequirement(
                new RequirementsParameter(
                    ColonyStatNames.ActionPoints_Resourses, actionPoints),
                    "Недостаточно ОД");
        }

        public static ActionAvailableRequirement Zones(int zones)
        {
            return new ActionAvailableRequirement(
                new RequirementsParameter(
                    ColonyStatNames.AreaCapacity_Available, zones),
                    "Недостаточно места");
        }
    }

    public static class ActionAvailableRequirementHelper
    {
        public static (bool IsAvailable, string? RefusalReason) Check(
            this IReadOnlyList<ActionAvailableRequirement> availableRequirements,
            ColonyStats colonyStats)
        {
            foreach (var requirement in availableRequirements)
            {
                var parameter = requirement.Parameter;
                if (!parameter.Check(colonyStats))
                    return (false, requirement.Message);
            }
            return (true, null);
        }
    }
}
