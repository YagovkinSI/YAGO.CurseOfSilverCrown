using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        string? Id,
        string Title,
        IReadOnlyList<SlideResponse> Slides,
        DilemmaResponse? Dilemma,
        bool IsCycleCompleted);
}
