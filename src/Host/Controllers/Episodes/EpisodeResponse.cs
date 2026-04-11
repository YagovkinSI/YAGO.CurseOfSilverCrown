using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        string? Id,
        IReadOnlyList<SlideResponse> PrologSlides,
        DilemmaResponse? Dilemma,
        bool IsCycleCompleted);
}
