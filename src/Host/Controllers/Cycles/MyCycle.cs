using System;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Host.Controllers.Cycles
{
    public record MyCycle(
        long Id,
        long ColonyId,
        int StepNumber,
        DateTime? RunAtUtc,
        CycleState State);
}
