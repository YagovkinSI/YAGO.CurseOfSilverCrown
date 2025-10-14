using YAGO.World.Application.Colonies;
using YAGO.World.Host.Controllers.MyUsers.Attributes;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record CreateColonyRequest(
        [ColonyNameValidation] string Name,
        ColonyPresetType PresetType);
}
