using System;

namespace YAGO.World.Host.Controllers.Cycles
{
    public record MyCycle(
        long Id,
        long ColonyId,
        DateTime? CompletedUtc);
}
