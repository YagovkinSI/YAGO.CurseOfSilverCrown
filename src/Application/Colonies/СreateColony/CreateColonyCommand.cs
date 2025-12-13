using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Application.Colonies.СreateColony
{
    public class CreateColonyCommand : IProcessorCommand
    {
        public long UserId { get; }
        public string ColonyName { get; }
        public ColonyPresetType PresetType { get; }

        public CreateColonyCommand(
            long userId,
            string colonyName,
            ColonyPresetType presetType)
        {
            UserId = userId;
            ColonyName = colonyName;
            PresetType = presetType;
        }
    }
}
