using System;

namespace YAGO.World.Host.Controllers.Cycles
{
    public record MyCycle(
        Guid Id,
        Guid ColonyId,
        DateTime StartAtUtc,
        DateTime? RunAtUtc);
}
