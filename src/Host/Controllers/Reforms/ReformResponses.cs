using System.Collections.Generic;
using YAGO.World.Host.Controllers.Common.GameRequirements;
using YAGO.World.Host.Controllers.Common.GameVisibleEffects;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Reforms
{
    public record ReformDetails(
        string Code,
        string Name,
        string Image,
        IReadOnlyList<GameVisibleEffectResponse> VisibleEffects,
        IReadOnlyList<GameRequirementResponse> Requirements,
        string[] Description,
        SlideButtonResponse Button);
}
