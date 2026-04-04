using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        string? Id,
        IReadOnlyList<SlideResponse> PrologSlides,
        IReadOnlyList<ChoiceResponse> Choice,
        string? ChoiceLabel,
        bool IsCycleCompleted);
}
