using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class ButtonAvailableRequirement
    {
        public RequirementsParameter Parameter { get; }
        public string Message { get; }

        public ButtonAvailableRequirement(RequirementsParameter parameter, string message)
        {
            Parameter = parameter;
            Message = message;
        }

        public static ButtonAvailableRequirement Cost(int solars)
        {
            return new ButtonAvailableRequirement(
                new RequirementsParameter(
                    ColonyStatNames.Economic_Reserves, solars),
                    "Недостаточно Солар");
        }

        public static ButtonAvailableRequirement ActionPoints(int actionPoints)
        {
            return new ButtonAvailableRequirement(
                new RequirementsParameter(
                    ColonyStatNames.ActionPoints_Resourses, actionPoints),
                    "Недостаточно ОД");
        }
    }
}
