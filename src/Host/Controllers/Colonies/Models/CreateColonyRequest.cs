using YAGO.World.Host.Controllers.Colonies.Attributes;

namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record CreateColonyRequest(
        [ColonyNameValidation] string Name,
        CodeOfLaws PresetType);
}
