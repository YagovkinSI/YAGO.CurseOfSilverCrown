using YAGO.World.Host.Controllers.GameActions;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record SlideButtonActionResponse(
        bool NeedsInput,
        GameActionType GameActionType,
        string Code,
        string Value);
}
