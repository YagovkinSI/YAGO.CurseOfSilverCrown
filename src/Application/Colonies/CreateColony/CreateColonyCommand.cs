using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.CreateColony
{
    public class CreateColonyCommand : IProcessorCommand
    {
        public long UserId { get; }
        public string ColonyName { get; }
        public CodeOfLaws GavernorType { get; }

        public CreateColonyCommand(
            long userId,
            string colonyName,
            CodeOfLaws presetType)
        {
            UserId = userId;
            ColonyName = colonyName;
            GavernorType = presetType;
        }
    }
}
