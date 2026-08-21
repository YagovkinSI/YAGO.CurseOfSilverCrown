using System.Collections.Generic;
using YAGO.World.Host.Controllers.Common.GameRequirements;
using YAGO.World.Host.Controllers.Common.GameVisibleEffects;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record SlideResponse(
        string Id,
        string Title,
        string ImageName,
        string[] Text,
        IReadOnlyList<GameVisibleEffectResponse> VisibleEffects,
        IReadOnlyList<GameRequirementResponse> Requirements,
        IReadOnlyList<SlideButtonResponse> Buttons,
        TextInputResponse? TextInput);
}
