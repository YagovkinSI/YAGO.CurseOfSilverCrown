using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        string? Id,
        IReadOnlyList<PrologueSlideResponse> PrologueSlides,
        DilemmaResponse? Dilemma,
        bool IsCycleCompleted);
}
