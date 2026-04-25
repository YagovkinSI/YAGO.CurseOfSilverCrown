using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class ChoiceRequirement
    {
        public RequirementsParameter Parameter { get; }
        public string Message { get; }

        public ChoiceRequirement(RequirementsParameter parameter, string message)
        {
            Parameter = parameter;
            Message = message;
        }

        public static ChoiceRequirement Cost(int solars)
        {
            return new ChoiceRequirement(
                new RequirementsParameter(
                    ColonyStatNames.Finance_Resource, solars),
                    "Недостаточно Солар");
        }
    }
}
