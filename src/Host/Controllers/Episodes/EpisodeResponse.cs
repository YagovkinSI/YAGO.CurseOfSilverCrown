using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        string? Id,
        IReadOnlyList<SlideResponse> Slides,
        string? ChoiceLabel,
        IReadOnlyList<SlideResponse>? Choice,
        bool IsCycleCompleted);
}
