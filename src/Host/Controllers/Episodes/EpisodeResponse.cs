using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record EpisodeResponse(
        IReadOnlyList<SlideResponse> Slides,
        DilemmaResponse? Dilemma,
        bool IsCycleCompleted);
}
