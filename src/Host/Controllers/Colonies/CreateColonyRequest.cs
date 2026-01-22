using YAGO.World.Domain.Colonies;
using YAGO.World.Host.Controllers.Colonies.Attributes;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record CreateColonyRequest(
        [ColonyNameValidation] string Name,
        GavernorType PresetType);
}
