using System;

namespace YAGO.World.Host.Controllers.Cycles
{
    public record MyCycle(
        Guid Id,
        Guid ColonyId,
        int StepNumber,
        DateTime StartAtUtc,
        DateTime? RunAtUtc);
}
