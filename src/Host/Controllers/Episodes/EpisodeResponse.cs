using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        string? Id,
        IReadOnlyList<SlideResponse> PrologSlides,
        IReadOnlyList<SlideResponse> Choice,
        string? ChoiceLabel,
        bool IsCycleCompleted);
}
