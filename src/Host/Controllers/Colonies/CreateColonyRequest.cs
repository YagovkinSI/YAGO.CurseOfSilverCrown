using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Host.Controllers.Colonies.Attributes;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record CreateColonyRequest(
        [ColonyNameValidation] string Name,
        CodeOfLaws PresetType);
}
