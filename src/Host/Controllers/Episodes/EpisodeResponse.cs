using System.Collections.Generic;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        long? Id,
        IReadOnlyList<SlideResponse> Slides,
        string? ChoiceLabel,
        IReadOnlyList<SlideResponse>? Choice);
}
