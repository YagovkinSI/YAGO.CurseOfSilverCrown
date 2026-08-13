using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class ColonyRequirementsParameter
    {
        public Colony Colony { get; }
        public RequirementsParameter Parameter { get; }

        public ColonyRequirementsParameter(
            Colony colony,
            RequirementsParameter parameter)
        {
            Colony = colony;
            Parameter = parameter;
        }

        public bool IsMet()
        {
            return Parameter.Check(Colony.State);
        }
    }
}
