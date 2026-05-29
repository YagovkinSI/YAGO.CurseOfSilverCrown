using System;
using System.Collections.Generic;
using YAGO.World.Host.Controllers.Episodes;

namespace YAGO.World.Host.Controllers.Cycles
{
    public record MyCycle(
        Guid Id,
        Guid ColonyId,
        int StepNumber,
        DateTime StartAtUtc,
        DateTime? RunAtUtc,
        IReadOnlyCollection<EpisodeResponse> Episodes);
}
